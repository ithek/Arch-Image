using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace Modele_Controleur
{
    public class XMLParser
    {
        private const String sewelisURL = "http://149.91.83.183/";
        private XmlDocument doc;
        private XmlNodeList nodesPersonnes;

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
            doc = new XmlDocument();
            doc.LoadXml(reponse);
            XmlNodeList nodes = doc.GetElementsByTagName("URI");

            return nodes.Count;
        }

        public List<POICreationData> getPOI(String reponse)
        {         
            WebClient webClient = new WebClient();
            List<POICreationData> listePois = new List<POICreationData>();
            doc = new XmlDocument();
            doc.LoadXml(reponse);
            XmlNodeList nodes = doc.SelectNodes("//node()[@uri='PossedePOI']");

            foreach (XmlNode node in nodes)
            {
                double x, y;
                string poiId,nom;
                XmlNodeList nodesPOI;
                String poiURL;
                poiURL = node.ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
                String reponsePOI = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + poiURL);      
                    
                XmlDocument docPOI = new XmlDocument();
                doc.LoadXml(reponsePOI);

                nodesPOI = doc.SelectNodes("//node()[@uri='X']");             
                x = Double.Parse(nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value);

                nodesPOI = doc.SelectNodes("//node()[@uri='Y']");
                y = Double.Parse(nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value);

                nodesPOI = doc.SelectNodes("//node()[@uri='PossedePOI']");
                poiId = nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
                
                String reponsePOIid = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + poiId);      
                nom = this.getNomPersonne(reponsePOIid);

                listePois.Add(new POICreationData(x, y, poiId, nom));
            }

            return listePois;
        }

        public List<Personne> getRecherchePersonnes(String reponse)
        {

            List<String> listeNomsPersonnes = new List<String>();
            doc = new XmlDocument();
            doc.LoadXml(reponse);

            XmlNodeList nodesPersonnes = doc.GetElementsByTagName("Literal");
            foreach (XmlNode node in nodesPersonnes)
            {
                listeNomsPersonnes.Add(node.InnerText);
            }

            return getInfosPersonnes(listeNomsPersonnes);
        }

        public List<Personne> getInfosPersonnes(List<String> listeNomsPersonnes)
        {
            WebClient webClient = new WebClient();
            List<Personne> listePersonnes = new List<Personne>();
            String nom = "", prenom = "", ddn = "", id = "", reponse = "";

            foreach (XmlNode node in nodesPersonnes)
            {
                if (listeNomsPersonnes.Contains(node.InnerText))
                {
                    id = node.NextSibling.InnerText;
                    reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + id);
                    doc.LoadXml(reponse);

                    XmlNodeList nodesList = doc.SelectNodes("//node()[@uri='nom']");
                    if(nodesList.Count > 0)
                        nom = nodesList[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodesList = doc.SelectNodes("//node()[@uri='prenom']");
                    if (nodesList.Count > 0)
                        prenom = nodesList[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodesList = doc.SelectNodes("//node()[@uri='dateNaissance']");
                    if (nodesList.Count > 0)
                        ddn = nodesList[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    listePersonnes.Add(new Personne(nom, prenom, ddn, id));
                }
            }
            return listePersonnes;
        }

        public void getListeNomsPersonnes(String reponse)
        {
            doc = new XmlDocument();
            doc.LoadXml(reponse);
            nodesPersonnes = doc.GetElementsByTagName("Literal");
        }
        
        public string getNomPersonne(String reponse)
        {
            String res;
            XmlNodeList nodesPOI;
            doc.LoadXml(reponse);
            
            nodesPOI = doc.SelectNodes("//node()[@uri='Initiale']");
            if(nodesPOI != null)
                res = nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
            
            nodesPOI = doc.SelectNodes("//node()[@uri='Nom']");
            res += nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
            
            return res;
        }

        public string getIdPersonne(String reponse)
        {
            XmlNodeList nodesPOI;
            doc.LoadXml(reponse);
            nodesPOI = doc.SelectNodes("//node()[@uri='Concerne']");
            return nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
        }

        //TODO: rajouter catégorie
        public List<Document> getListDocs(String reponse)
        {
            XmlNodeList nodesPOI;
            List<Document> listeDocs = new List<Document>();
            doc.LoadXml(reponse); 
            nodesPOI = doc.SelectNodes("//node()[@uri='EstLie']");

            foreach (XmlNode node in nodesPOI)
            {
                listeDocs.Add(new Document(node.InnerText));
            }
            return listeDocs;
        }
    }
}
