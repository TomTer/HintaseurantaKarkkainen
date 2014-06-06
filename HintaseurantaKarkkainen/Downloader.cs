using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HintaseurantaKarkkainen
{
    class Downloader
    {
        // Nuo ovat AJAX-kutsua varten headereita
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:29.0) Gecko/20100101 Firefox/29.0";
        private const int ContentLength = 196;
        private const string ContentType = "application/x-www-form-urlencoded; charset=UTF-8";


        /**
         * Tekee POST pyynnön ja palauttaa string-muodossa vastauksen. Kärkkäiseltä tulee JSON.
         */
        public string MakePostRequest(string url, string postData)
        {
            // Prepare data
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(postData);

            // Make request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = UserAgent;
            request.Method = "POST";
            request.ContentLength = ContentLength;
            request.ContentType = ContentType;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("MakePostRequest status: {0}", ((HttpWebResponse)response).StatusDescription);

            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }

        /**
         * Tekee GET pyynnön ja palauttaa string-muodossa vastauksen.
         */
        public string MakeGetRequest(string url)
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "GET";
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine("MakeGetRequest status: {0}", ((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return responseFromServer;
        }

    }
}
