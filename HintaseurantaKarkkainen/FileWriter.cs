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
                WriteProductToXml(product, _xmlDocument);
            }

            _xmlDocument.Save(outputFileName);
            
        }

        public static void WriteProductToXml(Product product, XmlDocument xmlDocument)
        {
            // Creating XML-element
            XmlNode productElement = xmlDocument.CreateElement("product"); // Creating element
            xmlDocument.DocumentElement.AppendChild(productElement); // Pointing the parent

            // Filling elements
            XmlNode subElementName = xmlDocument.CreateElement("name");
            subElementName.InnerText = product.Name;
            productElement.AppendChild(subElementName);

            XmlNode subElementPrice = xmlDocument.CreateElement("price");
            subElementPrice.InnerText = product.Price.ToString();
            productElement.AppendChild(subElementPrice);

            XmlNode subElementPictureUrl = xmlDocument.CreateElement("pictureUrl");
            subElementPictureUrl.InnerText = product.PictureUrl;
            productElement.AppendChild(subElementPictureUrl);

            XmlNode subElementProductUrl = xmlDocument.CreateElement("productUrl");
            subElementProductUrl.InnerText = product.ProductUrl;
            productElement.AppendChild(subElementProductUrl);

            XmlNode subElementManufacturer = xmlDocument.CreateElement("manufacturer");
            subElementManufacturer.InnerText = product.Manufacturer;
            productElement.AppendChild(subElementManufacturer);

            XmlNode subElementProductNumber = xmlDocument.CreateElement("productNumber");
            subElementProductNumber.InnerText = product.ProductNumber.ToString();
            productElement.AppendChild(subElementProductNumber);

            XmlNode subElementStoreInternet = xmlDocument.CreateElement("storeInternet");
            subElementStoreInternet.InnerText = product.StoreInternet.ToString();
            productElement.AppendChild(subElementStoreInternet);

            XmlNode subElementStoreYlivieska = xmlDocument.CreateElement("storeYlivieska");
            subElementStoreYlivieska.InnerText = product.StoreYlivieska.ToString();
            productElement.AppendChild(subElementStoreYlivieska);

            XmlNode subElementStoreOulu = xmlDocument.CreateElement("storeOulu");
            subElementStoreOulu.InnerText = product.StoreOulu.ToString();
            productElement.AppendChild(subElementStoreOulu);

            XmlNode subElementStoreLahti = xmlDocument.CreateElement("storeLahti");
            subElementStoreLahti.InnerText = product.StoreLahti.ToString();
            productElement.AppendChild(subElementStoreLahti);

            XmlNode subElementStoreLi = xmlDocument.CreateElement("storeLi");
            subElementStoreLi.InnerText = product.StoreLi.ToString();
            productElement.AppendChild(subElementStoreLi);

            XmlNode subElementDeliveryTime = xmlDocument.CreateElement("delirevyTime");
            subElementDeliveryTime.InnerText = product.DeliveryTime;
            productElement.AppendChild(subElementDeliveryTime);

            XmlNode subElementDeliverPrice = xmlDocument.CreateElement("delirevyPrice");
            subElementDeliverPrice.InnerText = product.DeliverPrice.ToString();
            productElement.AppendChild(subElementDeliverPrice);

            XmlNode subElementEan = xmlDocument.CreateElement("ean");
            subElementEan.InnerText = product.Ean;
            productElement.AppendChild(subElementEan);

            XmlNode subElementAttributes = xmlDocument.CreateElement("attributes");
            foreach (var pair in product.Attributes)
            {
                string key = pair.Key;
                string value = pair.Value;
                XmlNode attribute = xmlDocument.CreateElement("attribute");
                XmlAttribute attributeName = xmlDocument.CreateAttribute("name");
                attributeName.Value = key;
                attribute.Attributes.Append(attributeName);
                attribute.InnerText = value;
                subElementAttributes.AppendChild(attribute);

            }
            productElement.AppendChild(subElementAttributes);
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
