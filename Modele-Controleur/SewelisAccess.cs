using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Modele_Controleur
{
    public class SewelisAccess
    {
        private WebClient webClient;
        private const String sewelisURL = "http://149.91.83.183/";

        public SewelisAccess()
        {
            webClient = new WebClient();
        }

        public void creerStore(String nom)
        {
            webClient.DownloadString(sewelisURL + "createStore?userKey=123&filename=" + nom);
        }

        public void chargerRdf(String chemin)
        {
            webClient.DownloadString(sewelisURL + "importRdf?userKey=123&storeId=1&base=archimage&filename=" + chemin);
        }

        public void ajouterPOI(POICreationData poi)
        {

        }

        public List<Document> GetListDocs(Categorie categorie)
        {
            string response = webClient.DownloadString(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=[a <file:///home/kevin/sewelis/img/FRAD035_1R_01901/FRAD035_1R_01901a/RegistreMatricule>]");
            List<Document> documents = new List<Document>();
            /*XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);

            XmlNodeList nodes = doc.GetElementsByTagName("URI");

           

            foreach (XmlNode node in nodes)
            {
                string url = node.InnerText;
                string[] tokens = url.Split(new string[] { "/" }, StringSplitOptions.None);

                Document document = new Document(Categorie.REGISTRE_MATRICULE, tokens[tokens.Length - 1], 0, 0);
                documents.Add(document);
            }*/

            return documents;
        }

        /**
         * Renvoie le document n°position
         */
        public Document GetDoc(int position, Categorie categorie)
        {
            throw new System.NotImplementedException();
        }
    }
}
