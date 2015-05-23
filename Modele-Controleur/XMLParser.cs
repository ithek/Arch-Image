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
        private const String sewelisURL = "http://37.59.103.120/";
        private List<Tuple<string, string>> listeTuplesPersonnes = new List<Tuple<string, string>>();
        

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
                double x, y;
                String nom;
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

                nodesPOI = doc.SelectNodes("//node()[@uri='NomPOI']");
                nom = nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;

                listePois.Add(new POICreationData(x, y, poiURL, nom));
            }

            return listePois;
        }

        public List<Personne> getInfosPersonnes(string motif)
        {
            WebClient webClient = new WebClient();
            List<Personne> listePersonnes = new List<Personne>();
            String nom, prenom, initiale, anneeRM, numeroRM, ddn, lieuNaissance, dateDeces, lieuDeces, dateMariage, lieuMariage, nomConjoint, prenomConjoint, prenomPere, nomMere, prenomMere, id, reponse, p;
            nom = prenom = initiale = anneeRM = numeroRM = ddn = lieuNaissance = dateDeces = lieuDeces = dateMariage = lieuMariage = nomConjoint = prenomConjoint = prenomPere = nomMere = prenomMere = id = reponse = "";
            motif = motif.ToLower();
            foreach (Tuple<string,string> tuple in listeTuplesPersonnes)
            {
                p = tuple.Item1;
                if (p.StartsWith(motif))
                {
                    id = tuple.Item2;
                    XmlDocument doc = new XmlDocument();
                    reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + id);
                    doc.LoadXml(reponse);

                    XmlNodeList nodes = doc.SelectNodes("//node()[@uri='nom']");
                    if (nodes.Count > 0)
                        nom = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='initiale']");
                    if (nodes.Count > 0)
                        initiale = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='prenom']");
                    if (nodes.Count > 0)
                        prenom = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='anneeRegistreMatricule']");
                    if (nodes.Count > 0)
                        anneeRM = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='numeroRegistreMatricule']");
                    if (nodes.Count > 0)
                        numeroRM = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='dateNaissance']");
                    if (nodes.Count > 0)
                        ddn = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='lieuNaissance']");
                    if (nodes.Count > 0)
                        lieuNaissance = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='dateDeces']");
                    if (nodes.Count > 0)
                        dateDeces = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='lieuDeces']");
                    if (nodes.Count > 0)
                        lieuDeces = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='dateMariage']");
                    if (nodes.Count > 0)
                        dateMariage = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='lieuMariage']");
                    if (nodes.Count > 0)
                        lieuMariage = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='nomConjoint']");
                    if (nodes.Count > 0)
                        nomConjoint = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='prenomConjoint']");
                    if (nodes.Count > 0)
                        prenomConjoint = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='prenomPere']");
                    if (nodes.Count > 0)
                        prenomPere = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='nomMere']");
                    if (nodes.Count > 0)
                        nomMere = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    nodes = doc.SelectNodes("//node()[@uri='prenomMere']");
                    if (nodes.Count > 0)
                        prenomMere = nodes[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.InnerText;

                    listePersonnes.Add(new Personne(nom, prenom, initiale, anneeRM, numeroRM, ddn, lieuNaissance, dateDeces, lieuDeces, dateMariage, lieuMariage, nomConjoint, prenomConjoint, prenomPere, nomMere, prenomMere, id));
                }
            }
            return listePersonnes;
        }

        public void getListeNomsPersonnes(String reponse)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reponse);
            XmlNodeList nodesPersonnes = doc.GetElementsByTagName("Literal");
            foreach (XmlNode node in nodesPersonnes)
            {
                string nom = node.InnerText.ToLower();
                string id = node.NextSibling.InnerText;

                if(!nom.Equals("stele") && !nom.Equals("monument aux morts"))
                    listeTuplesPersonnes.Add(new Tuple<string, string>(nom, id));
            }
        }
        

        public string getIdPersonne(String reponse)
        {
            XmlNodeList nodesPOI;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reponse);
            nodesPOI = doc.SelectNodes("//node()[@uri='EstLie']");
            return nodesPOI[0].ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
        }

        //TODO: rajouter catégorie
        public List<Document> getListDocs(String reponse)
        {
            XmlNodeList nodesPOI;
            XmlDocument doc = new XmlDocument();
            List<Document> listeDocs = new List<Document>();
            doc.LoadXml(reponse);
            nodesPOI = doc.SelectNodes("//node()[@uri='Concerne']");

            foreach (XmlNode node in nodesPOI)
            {
                string chemin = node.ParentNode.NextSibling.FirstChild.FirstChild.FirstChild.Attributes["uri"].Value;
                Categorie categorie = categoryName(chemin);
                listeDocs.Add(new Document(chemin, categorie));
            }
            return listeDocs;
        }

        //Récupère le type des documents
        //TODO: Duplication de celle dans NavigationPage
        private Categorie categoryName(String path)
        {
            System.Text.RegularExpressions.Regex rMat = new System.Text.RegularExpressions.Regex("REGISTRES_MILITAIRES");
            System.Text.RegularExpressions.Regex nmd = new System.Text.RegularExpressions.Regex("NMD");
            System.Text.RegularExpressions.Regex tRMat = new System.Text.RegularExpressions.Regex("TABLES_RMM");
            System.Text.RegularExpressions.Regex recensement = new System.Text.RegularExpressions.Regex("RECENSEMENT");
            System.Text.RegularExpressions.Regex tDecen = new System.Text.RegularExpressions.Regex("TABLES_DECENNALES");
            System.Text.RegularExpressions.Regex tsa = new System.Text.RegularExpressions.Regex("TSA");

            if (rMat.IsMatch(path))
                return Categorie.REGISTRE_MATRICULE;
            if (nmd.IsMatch(path))
                return Categorie.NAISSANCE_MARIAGE_DECES;
            if (tRMat.IsMatch(path))
                return Categorie.TABLE_REGISTRE_MATRICULE;
            if (recensement.IsMatch(path))
                return Categorie.RECENSEMENT;
            if (tDecen.IsMatch(path))
                return Categorie.TABLES_DECENNALES;
            if (tsa.IsMatch(path))
                return Categorie.TSA;
            else
                return Categorie.REGISTRE_MATRICULE;
        }
    }
}
