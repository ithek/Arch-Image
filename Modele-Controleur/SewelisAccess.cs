using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace Modele_Controleur
{
    public class SewelisAccess
    {
        public SewelisAccess()
        {
            var client = new WebClient();
            string response = client.DownloadString("http://172.16.1.2:9999/createStore?userKey=123&filename=plop");
            response = client.DownloadString("http://172.16.1.2:9999/importRdf?userKey=123&storeId=1&base=foo&filename=archimage_img.rdf");
        }
        
        /**
         * Renvoie le document n°position
         */
        public Document GetDoc(int position, Categorie categorie)
        {
            throw new System.NotImplementedException();
        }

        public List<Document> GetListDocs(Categorie categorie)
        {
            //var request = (HttpWebRequest)WebRequest.Create("http://172.16.1.2:9999/resultsOfStatement?userKey=123&storeId=1&statement=[a <file:///home/kevin/sewelis/img/FRAD035_1R_01901/FRAD035_1R_01901a/RegistreMatricule>]");

            //var response = (HttpWebResponse)request.GetResponse();
            var client = new WebClient();
            string response = client.DownloadString("http://172.16.1.2:9999/resultsOfStatement?userKey=123&storeId=1&statement=[a <file:///home/kevin/sewelis/img/FRAD035_1R_01901/FRAD035_1R_01901a/RegistreMatricule>]");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);

            XmlNodeList nodes = doc.GetElementsByTagName("URI");

            List<Document> documents = new List<Document>();

            foreach (XmlNode node in nodes)
            {
                string url = node.InnerText;
                string[] tokens = url.Split(new string[] { "/" }, StringSplitOptions.None);

                Document document = new Document(Categorie.REGISTRE_MATRICULE, tokens[tokens.Length-1], 0, 0);
                documents.Add(document);
            }

            return documents;
        }
    }
}
