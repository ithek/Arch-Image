﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;

namespace Modele_Controleur
{
    public class SewelisAccess
    {
        private const String sewelisURL = "http://149.91.83.183/";
        private WebClient webClient;
        private HttpWebRequest webRequest;
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

            String reponse = webClient.DownloadString(sewelisURL + "getCompletions?userKey=123&placeId=2&matchingKey=" + motif);
            listePersonnes = parser.getRecherchePersonnes(reponse);
 
            return listePersonnes;
        }

        public void chargerListePersonnes()
        {
            //webClient = new WebClient();
            //String reponse = webClient.DownloadString(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=get [ a <Personne>; <nom> [] ]");

            webRequest = (HttpWebRequest)WebRequest.Create(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=get [ a <Personne>; <nom> [] ]"); ;
            webRequest.BeginGetResponse(new AsyncCallback(FinishChargerListePersonnes), webRequest);
            //parser.getListeNomsPersonnes(reponse);
        }

        public void FinishChargerListePersonnes(IAsyncResult result)
        {
            HttpWebResponse webResponse = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
            Encoding enc = System.Text.Encoding.GetEncoding(1252);
            StreamReader loResponseStream = new
            StreamReader(webResponse.GetResponseStream(), enc);

            string reponse = loResponseStream.ReadToEnd();

            loResponseStream.Close();
            webResponse.Close();

            parser.getListeNomsPersonnes(reponse);
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
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idPoi + "&p=NomPOI&o=<URI>" + poi.Nom + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + chemin + "&p=PossedePOI&o=<URI>" + idPoi + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + chemin + "&p=Concerne&o=<URI>" + poi.IdPersonne + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + poi.IdPersonne + "&p=EstLie&o=<URI>" + idPoi + "</URI>");
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

        /**
         * Récupère la liste des docs associés à un POI
         */
        public List<Document> getListDocs(POICreationData poi)
        {
            String reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + poi.Id);
            String idPersonne = parser.getIdPersonne(reponse);
            reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + idPersonne);
            return parser.getListDocs(reponse);
        }
    }
}
