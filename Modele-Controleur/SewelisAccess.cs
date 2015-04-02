using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Modele_Controleur
{
    public class SewelisAccess
    {
        private const String sewelisURL = "http://149.91.83.183/";
        private WebClient webClient;
        private XMLParser parser;
       
        public SewelisAccess()
        {
            webClient = new WebClient();
            parser = new XMLParser();

            //Teste si la base existe
            String reponse = webClient.DownloadString(sewelisURL + "storeBase?userKey=123&storeId=1");

            if (!parser.baseExiste(reponse))
            {
                creerStore("archimage");
                chargerRdf("base_personnes.rdf");
                chargerRdf("base_images.rdf");
            }
        }

        public void creerStore(String nom)
        {
            webClient.DownloadString(sewelisURL + "createStore?userKey=123&filename=" + nom);
        }

        public void chargerRdf(String chemin)
        {
            webClient.DownloadString(sewelisURL + "importRdf?userKey=123&storeId=1&base=archimage&filename=" + chemin);
        }

        /**
         * Recherche la liste des personnes correspondant au motif de recherche
         */
        public List<Personne> recherchePersonnes(String motif)
        {
            List<Personne> listePersonnes = null;

            String reponse = webClient.DownloadString(sewelisURL + "getCompletions?userKey=123&placeId=5&matchingKey=" + motif);
            listePersonnes = parser.getRecherchePersonnes(reponse);
 
            return listePersonnes;
        }

        public void chargerListePersonnes()
        {
            parser.getListeNomsPersonnes();
        }

        /**
         * Ajoute un poi pour le document concerné et la personne.
         */
        public void ajouterPOI(POICreationData poi, Document doc)
        {
            String reponse = webClient.DownloadString(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=[a <POI>]");
            int nbPoi = parser.getLastIndexOf(reponse);
            string idPoi = "poi_id" + nbPoi;
            string chemin = doc.CheminAcces.Substring(16, doc.CheminAcces.Length - 16).Replace(@"\", @"/");

            webClient.DownloadString(sewelisURL + "runStatement?userKey=123&storeId=1&statement=<" + idPoi + "> [a <POI>]");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idPoi + "&p=X&o=<URI>" + poi.posX + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idPoi + "&p=Y&o=<URI>" + poi.posY + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + chemin + "&p=PossedePOI&o=<URI>" + idPoi + "</URI>");       
          
            //webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=<poi_id"+ nbPois + ">&p=Concerne&o=<URI>" + doc.CheminAcces + "</URI>";
        }

        /**
         * Récupère le doc associé au document doc.
         */
        public List<POICreationData> getPOI(Document doc)
        {
            string chemin = doc.CheminAcces.Substring(16, doc.CheminAcces.Length - 16).Replace(@"\", @"/");
            string reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + chemin);
            return parser.getPOI(reponse);
        }

        public List<Document> getListDocs(Categorie categorie)
        {
            string response = webClient.DownloadString(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=[a <file:///home/kevin/sewelis/img/FRAD035_1R_01901/FRAD035_1R_01901a/RegistreMatricule>]");
            List<Document> documents = new List<Document>();
            
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
