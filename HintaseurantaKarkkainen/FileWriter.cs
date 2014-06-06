using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HintaseurantaKarkkainen.Models;

namespace HintaseurantaKarkkainen
{
    class FileWriter
    {
        private static XmlDocument _xmlDocument = new XmlDocument();

        /**
         * Tekee XML-tiedoston alustan
         */
        public static void InitXmlFileStart(string outputFileName)
        {
            // Create XML-file
            XmlTextWriter textWritter = new XmlTextWriter(outputFileName, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("products");
            textWritter.WriteEndElement();
            textWritter.Close();

            // Loading document
            _xmlDocument.Load(outputFileName);
            if (_xmlDocument == null) throw new XmlException("Error in WritePartProductListToXmlFile");
        }

        /**
         * Tallentaa jokaisen tuotteen XML-tiedostoon
         */
        public static void WritePartProductListToXmlFile(string outputFileName, List<Product> products)
        {

            foreach (Product product in products)
            {
                // Creating XML-element
                XmlNode productElement = _xmlDocument.CreateElement("product"); // Creating element
                _xmlDocument.DocumentElement.AppendChild(productElement); // Pointing the parent

                // Filling elements
                XmlNode subElementName = _xmlDocument.CreateElement("name");
                subElementName.InnerText = product.Name;
                productElement.AppendChild(subElementName);

                XmlNode subElementPrice = _xmlDocument.CreateElement("price");
                subElementPrice.InnerText = product.Price.ToString();
                productElement.AppendChild(subElementPrice);

                XmlNode subElementPictureUrl = _xmlDocument.CreateElement("pictureUrl");
                subElementPictureUrl.InnerText = product.PictureUrl;
                productElement.AppendChild(subElementPictureUrl);

                XmlNode subElementProductUrl = _xmlDocument.CreateElement("productUrl");
                subElementProductUrl.InnerText = product.ProductUrl;
                productElement.AppendChild(subElementProductUrl);

                XmlNode subElementManufacturer = _xmlDocument.CreateElement("manufacturer");
                subElementManufacturer.InnerText = product.Manufacturer;
                productElement.AppendChild(subElementManufacturer);

                XmlNode subElementProductNumber = _xmlDocument.CreateElement("productNumber");
                subElementProductNumber.InnerText = product.ProductNumber.ToString();
                productElement.AppendChild(subElementProductNumber);

                XmlNode subElementStoreInternet = _xmlDocument.CreateElement("storeInternet");
                subElementStoreInternet.InnerText = product.StoreInternet.ToString();
                productElement.AppendChild(subElementStoreInternet);

                XmlNode subElementStoreYlivieska = _xmlDocument.CreateElement("storeYlivieska");
                subElementStoreYlivieska.InnerText = product.StoreYlivieska.ToString();
                productElement.AppendChild(subElementStoreYlivieska);

                XmlNode subElementStoreOulu = _xmlDocument.CreateElement("storeOulu");
                subElementStoreOulu.InnerText = product.StoreOulu.ToString();
                productElement.AppendChild(subElementStoreOulu);

                XmlNode subElementStoreLahti = _xmlDocument.CreateElement("storeLahti");
                subElementStoreLahti.InnerText = product.StoreLahti.ToString();
                productElement.AppendChild(subElementStoreLahti);

                XmlNode subElementStoreLi = _xmlDocument.CreateElement("storeLi");
                subElementStoreLi.InnerText = product.StoreLi.ToString();
                productElement.AppendChild(subElementStoreLi);

                XmlNode subElementDeliveryTime = _xmlDocument.CreateElement("delirevyTime");
                subElementDeliveryTime.InnerText = product.DeliveryTime;
                productElement.AppendChild(subElementDeliveryTime);

                XmlNode subElementDeliverPrice = _xmlDocument.CreateElement("delirevyPrice");
                subElementDeliverPrice.InnerText = product.DeliverPrice.ToString();
                productElement.AppendChild(subElementDeliverPrice);

                XmlNode subElementEan = _xmlDocument.CreateElement("ean");
                subElementEan.InnerText = product.Ean;
                productElement.AppendChild(subElementEan);

                XmlNode subElementAttributes = _xmlDocument.CreateElement("attributes");
                foreach (var pair in product.Attributes)
                {
                    string key = pair.Key;
                    string value = pair.Value;
                    XmlNode attribute = _xmlDocument.CreateElement("attribute");
                    XmlAttribute attributeName = _xmlDocument.CreateAttribute("name");
                    attributeName.Value = key;
                    attribute.Attributes.Append(attributeName);
                    attribute.InnerText = value;
                    subElementAttributes.AppendChild(attribute);

                }
                productElement.AppendChild(subElementAttributes);
            }

            _xmlDocument.Save(outputFileName);
            
        }


        /**
         * Funktio kirjoittaa kaikki "productList"-listassa olevat tuotteet XML-muodossa tiedostoon.
         * Käyttää System.IO.StreamWriter -luokan.
         */
        public static void WriteXmlToFile(string outputFileName, List<Product> productList)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(outputFileName);

            file.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<products>\n");
            foreach (Product product in productList)
            {
                file.Write(product.ToXml());
            }
            file.Write("</products>\n");

            file.Close();
        }


    }
}
