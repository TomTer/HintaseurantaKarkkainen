using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace HintaseurantaKarkkainen.Models
{
    [Serializable]
    public class Product
    {
        // Yleistä
        public string Name { get; set; }
        public double Price { get; set; }
        public string PictureUrl { get; set; }
        public string ProductUrl { get; set; }
        public string Manufacturer { get; set; } // Voi olla tyhjä
        public long ProductNumber { get; set; }

        // Attributes voi olla tyhjä
        public Dictionary<string, string> Attributes = new Dictionary<string, string>();

        // Montako kappaletta eri tavarataloissa
        public int StoreInternet { get; set; }
        public int StoreYlivieska { get; set; }
        public int StoreOulu { get; set; }
        public int StoreLahti { get; set; }
        public int StoreLi { get; set; }

        // Vakiot tässä
        public string DeliveryTime = "2-4 arkipäivää"; // Vakio
        public double DeliverPrice = 5.90; // Vakio
        public string Ean = ""; // Tätä ei ole ollenkaan

        /**
         * Funktio palauttaa XML-muodossa luokan datat
         */
        public string ToXml()
        {
            string xmlString = String.Format("\t<product>\n" +
                                 "\t\t<name>{0}</name>\n" +
                                 "\t\t<price>{1}</price>\n" +
                                 "\t\t<pictureUrl>{2}</pictureUrl>\n" +
                                 "\t\t<productUrl>{3}</productUrl>\n" +
                                 "\t\t<manufacturer>{4}</manufacturer>\n" +
                                 "\t\t<productNumber>{5}</productNumber>\n" +
                                 "\t\t<storeInternet>{6}</storeInternet>\n" +
                                 "\t\t<storeYlivieska>{7}</storeYlivieska>\n" +
                                 "\t\t<storeOulu>{8}</storeOulu>\n" +
                                 "\t\t<storeLahti>{9}</storeLahti>\n" +
                                 "\t\t<storeLi>{10}</storeLi>\n" +
                                 "\t\t<delirevyTime>{11}</delirevyTime>\n" +
                                 "\t\t<delirevyPrice>{12}</delirevyPrice>\n" +
                                 "\t\t<ean>{13}</ean>\n",
                                 Name, Price, PictureUrl, ProductUrl, Manufacturer, ProductNumber,
                                 StoreInternet, StoreYlivieska, StoreOulu, StoreLahti, StoreLi,
                                 DeliveryTime, DeliverPrice, Ean);

            xmlString += "\t\t<attributes>\n";
            foreach (var pair in Attributes)
            {
                string key = pair.Key;
                string value = pair.Value;
                xmlString += String.Format("\t\t\t<attribute name=\"{0}\">{1}</attribute>\n", key, value);
            }
            xmlString += "\t\t</attributes>\n" +
                         "\t</product>\n";

            return xmlString;
        }
    }
}
