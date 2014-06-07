using System;
using System.Collections.Generic;
using System.Configuration;
using HintaseurantaKarkkainen.Models;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Codeplex.Data;

namespace HintaseurantaKarkkainen
{
    class Parser
    {

        /**
         * Parsaa käyttäen DynamicJson-kirjastoa
         */
        public List<Product> ParseJsonUsingDynamicJson(string jsonString, Downloader downloaderObject, int sleepTime)
        {
            // Inits
            Console.WriteLine("in ParseJsonUsingDynamicJson"); // DEBUG
            var products = new List<Product>(); // Kaikki alikategorian tuotteet
            dynamic productsJson = DynamicJson.Parse(jsonString).products;
            int totalSize = GetSize(productsJson);
            string pageHtmlSource = "";
            Console.WriteLine("Total size: {0}", totalSize); // DEBUG

            // Iteroidaan tavaroita
            foreach (dynamic productJson in productsJson)
            {
                // Tuotteella url on null? Todennäköisesti virhe on tapahtunut, jatketaan seuraavalla.
                if (productJson.seoToken == null) continue;


                long startTime = Libs.GetUnixTimestamp(); // DEBUG

                int itemsSize = GetSize(productJson.items);
                Console.WriteLine("productJson size: {0}", itemsSize); // DEBUG

                // Täytetään yleiset attribuutit. Tehdään pohja, jonka voi kloonata myöhemmin
                var productTemplate = new Product
                {
                    // nimi
                    Name = productJson.name,
                    // kuvan url
                    PictureUrl = Libs.PictureUrlPart + productJson.mainImage,
                    // url tuotteeseen
                    ProductUrl = Libs.MainSite + productJson.seoToken
                };

                Console.WriteLine("CURRENT: {0}", productTemplate.Name); // DEBUG

                // ladattu tuotteen sivun lähdekoodi. Varastoja ja valmistajaa varten
                pageHtmlSource = downloaderObject.MakeGetRequest(productTemplate.ProductUrl);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageHtmlSource);

                // Valmistaja jos on olemassa
                string manufacturer = "";
                HtmlNode manufacturerNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div[3]/main/section/a/img"); // xpath
                if (manufacturerNode != null) manufacturer = manufacturerNode.Attributes["alt"].Value;

                // Iteroidaan erilaisia kappaleita
                foreach (dynamic item in productJson.items)
                {
                    // Kloonataan ja tehdään Product-objektin omilla attribuuteilla.
                    Product product = ObjectCopier.Clone(productTemplate);

                    // Täytetään
                    // Hinta
                    product.Price = Convert.ToDouble(Libs.GetMatches(item.priceHTML, @"\d+,\d+")[0].ToString());

                    // Tuotenumero
                    product.ProductNumber = Convert.ToInt64(item.partNumber.ToString());

                    // Erilaiset attribuutit tuotteella
                    var attributesDictionary = new Dictionary<string, string>();
                    foreach (dynamic attribute in item.attributes.defining)
                    {
                        //Console.WriteLine("Attr: {0}, Value: {1}", attribute.name, attribute.value.value); // DEBUG
                        string attributeName = attribute.name.ToString();
                        string attributeValue = attribute.value.value;
                        attributesDictionary.Add(attributeName, attributeValue);
                    }
                    product.Attributes = attributesDictionary;

                    // Varastot
                    int catentryId = Convert.ToInt32(item.catentryId); // Sen avulla tunnistaa oman varaston
                    HtmlNode node = htmlDocument.GetElementbyId("availability-stores");
                    // Sivulla ei ole varastojen tietoja? Todennäköisesti joko virhesivu tai ei ole tietoja ollenkaan. 
                    // Joka tapauksessa ei sovi meille.
                    if (node == null) continue;
                    string storeJsonValue = node.Attributes["data-json"].Value;
                    dynamic correctStoreParsedJson = GetProductRelatedStoreJson(storeJsonValue, catentryId);
                    product.StoreInternet = Convert.ToInt32(correctStoreParsedJson.orderable);

                    Dictionary<string, int> storeValues = GetStoreValues(correctStoreParsedJson.ffcavl);
                    product.StoreLi = storeValues["Li"];
                    product.StoreLahti = storeValues["Lahti"];
                    product.StoreOulu = storeValues["Oulu"];
                    product.StoreYlivieska = storeValues["Ylivieska"];

                    // Valmistaja. Voi olla tyhjä
                    product.Manufacturer = manufacturer;

                    PrintProductInformation(product); // DEBUG

                    // Lisätään listaan
                    products.Add(product);
                }

                int timeTook = (int)(Libs.GetUnixTimestamp() - startTime);
                MainEntryPoint.BenchmarkList.Add(timeTook);
                Console.WriteLine("Iteration complete. Took: {0} ms. Sleeping {1} ms\n", timeTook, sleepTime);
                System.Threading.Thread.Sleep(sleepTime);
            }

            return products;
        }

        /**
         * Funktio debugia varten. Tulostaa tuotteen informaation
         */
        private void PrintProductInformation(Product product)
        {
            Console.WriteLine("   Product:\n" +
                              "\tName: {0}\n" +
                              "\tPrice: {1}\n" +
                              "\tManufacturer: {2}\n" +
                              "\tProductNumber: {3}\n" +
                              "\tStoreInternet: {4}\n" +
                              "\tStoreYlivieska: {5}\n" +
                              "\tStoreOulu: {6}\n" +
                              "\tStoreLahti: {7}\n" +
                              "\tStoreLi: {8}\n" +
                              "\tPictureUrl: {9}\n" +
                              "\tProductUrl: {10}",
                              product.Name, product.Price, product.Manufacturer, product.ProductNumber, 
                              product.StoreInternet, product.StoreYlivieska,
                              product.StoreOulu, product.StoreLahti, product.StoreLi,
                              product.PictureUrl, product.ProductUrl);
        }

        /**
         * Funktio palauttaa sopivan varaston DynamicJson-muuttujana.
         */
        private dynamic GetProductRelatedStoreJson(string storeJsonValue, int catentryId)
        {
            dynamic allStores = DynamicJson.Parse(storeJsonValue).skuavl;
            dynamic correctStore = null;

            foreach (var store in allStores)
            {
                if (store.id == catentryId)
                {
                    correctStore = store;
                    break;
                }
            }

            return correctStore;
        }

        /**
         * Funktio iteroi varasto-taulukon läpi ja hakee kappaleiden lukumäärän
         */
        private Dictionary<string, int> GetStoreValues(dynamic storeJsonFfcavlValue)
        {
            var storeValuesDictionary = new Dictionary<string, int>();

            int currentPosition = 0;
            foreach (dynamic store in storeJsonFfcavlValue) // Taas "hacki" koska käytetään dynaamista objektia
            {
                int storeItemsNumber = Convert.ToInt32(store.Value.qtystore);

                if (currentPosition == 1) storeValuesDictionary.Add("Li", storeItemsNumber); // Li
                else if (currentPosition == 3) storeValuesDictionary.Add("Lahti", storeItemsNumber); // Lahti
                else if (currentPosition == 4) storeValuesDictionary.Add("Oulu", storeItemsNumber); // Oulu
                else if (currentPosition == 5) storeValuesDictionary.Add("Ylivieska", storeItemsNumber); // Ylivieska
                currentPosition++;
            }

            return storeValuesDictionary;
        }

        /**
         * Tämä on "hack", koska muuttujat ovat dynaamisia ja jos kyseessä on taulukko
         * siltä en voi saada taulukon kokoa käyttäen Count()-funktiota.
         */
        private int GetSize(dynamic jsonArray)
        {
            int size = 0;
            try
            {
                foreach (dynamic variable in jsonArray)
                {
                    size++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in dynamic GetSize:");
                Console.WriteLine(e.StackTrace);
            }
            return size;
        }

        

        
    }
}
