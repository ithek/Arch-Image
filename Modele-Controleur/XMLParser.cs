using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace Modele_Controleur
{
    class XMLParser
    {
        private const String sewelisURL = "http://149.91.83.183/";

        public bool baseExiste(String reponse)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reponse);
            XmlNodeList nodes = doc.GetElementsByTagName("storeBaseResponse");

            if (nodes[0].Attributes["status"].Value == "ok")
                return true;
            return false;
        }

        public int getLastIndexOf(String reponse)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reponse);
            XmlNodeList nodes = doc.GetElementsByTagName("URI");

            return nodes.Count;
        }

        public List<POICreationData> getPOI(String reponse)
        {         
            WebClient webClient = new WebClient();
            List<POICreationData> listePois = new List<POICreationData>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reponse);
            XmlNodeList nodes = doc.SelectNodes("//node()[@uri='PossedePOI']");

            foreach (XmlNode node in nodes)
            {
                int x, y;
                XmlNodeList nodesPOI;
                String poiURL;
                poiURL = node.ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
                String reponsePOI = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + poiURL);      
                    
                XmlDocument docPOI = new XmlDocument();
                doc.LoadXml(reponsePOI);

                nodesPOI = doc.SelectNodes("//node()[@uri='X']");             
                x = Int32.Parse(nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value);

                nodesPOI = doc.SelectNodes("//node()[@uri='Y']");           
                y = Int32.Parse(nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value);

                listePois.Add(new POICreationData(x, y));
            }

            return listePois;
        }
    }
}
