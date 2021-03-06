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
        private const String sewelisURL = "http://37.59.103.120/";
        private XMLParser parser;
       
        public SewelisAccess()
        {
            WebClient webClient = new WebClient();
            parser = new XMLParser();

            //Teste si la base existe
            String reponse = webClient.DownloadString(sewelisURL + "storeBase?userKey=123&storeId=1");

            if (!parser.baseExiste(reponse))
            {
                creerStore("archimage");
                chargerRdf("base_personnes.rdf");
                chargerRdf("base_images.rdf");
                chargerRdf("base_images_stele_monument.rdf");
                webClient.DownloadString(sewelisURL + "getPlaceRoot?userKey=123&storeId=1");
                webClient.DownloadString(sewelisURL + "insertIncrement?userKey=123&storeId=1&placeId=1&incrementId=45");
            }
        }

        public void creerStore(String nom)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadString(sewelisURL + "createStore?userKey=123&filename=" + nom);
        }

        public void chargerRdf(String chemin)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadString(sewelisURL + "importRdf?userKey=123&storeId=1&base=archimage&filename=" + chemin);
        }

        /**
         * Recherche la liste des personnes correspondant au motif de recherche
         */
        public List<Personne> recherchePersonnes(String motif)
        {
            return parser.getInfosPersonnes(motif);
        }

        public void chargerListePersonnes()
        {
            WebRequest webRequest;
            webRequest = (HttpWebRequest)WebRequest.Create(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=get [ a <Personne>; <nom> [] ]");   
            webRequest.BeginGetResponse(new AsyncCallback(FinishChargerListePersonnes), webRequest);
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
            WebClient webClient = new WebClient();
            String reponse = webClient.DownloadString(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=[a <POI>]");
            int nbPoi = parser.getLastIndexOf(reponse);
            string idPoi = "poi_id" + nbPoi;
            string chemin = doc.CheminAcces.Substring(16, doc.CheminAcces.Length - 16).Replace(@"\", @"/");

            webClient.DownloadString(sewelisURL + "runStatement?userKey=123&storeId=1&statement=<" + idPoi + "> [a <POI>]");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idPoi + "&p=X&o=<URI>" + poi.posX + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idPoi + "&p=Y&o=<URI>" + poi.posY + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idPoi + "&p=NomPOI&o=<URI>" + poi.Nom + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + chemin + "&p=PossedePOI&o=<URI>" + idPoi + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + chemin + "&p=Concerne&o=<URI>" + poi.Personne.Id + "</URI>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + poi.Personne.Id + "&p=EstLie&o=<URI>" + idPoi + "</URI>");
        }

        /**
         * Ajoute une personne
         */
         public Personne ajouterPersonne(Personne p)
        {
            WebClient webClient = new WebClient();
            String reponse = webClient.DownloadString(sewelisURL + "resultsOfStatement?userKey=123&storeId=1&statement=[a <Personne>]");
            int nbP = parser.getLastIndexOf(reponse);
            string idP = "p_id" + nbP;

            webClient.DownloadString(sewelisURL + "runStatement?userKey=123&storeId=1&statement=<" + idP + "> [a <Personne>]");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=nom&o=<Literal>" + p.Nom + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=prenom&o=<Literal>" + p.Prenom + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=initiale&o=<Literal>" + p.Initiale + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=anneeRegistreMatricule&o=<Literal>" + p.AnneeRegistreMatricule + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=numeroRegistreMatricule&o=<Literal>" + p.NumeroRegistreMatricule + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=dateNaissance&o=<Literal>" + p.DateNaissance + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=lieuNaissance&o=<Literal>" + p.LieuNaissance + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=dateDeces&o=<Literal>" + p.DateDeces + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=lieuDeces&o=<Literal>" + p.LieuDeces + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=prenomPere&o=<Literal>" + p.PrenomPere + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=nomMere&o=<Literal>" + p.NomMere + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=prenomMere&o=<Literal>" + p.PrenomMere + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=prenomConjoint&o=<Literal>" + p.PrenomConjoint + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=nomConjoint&o=<Literal>" + p.NomConjoint + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=dateMariage&o=<Literal>" + p.DateMariage + "</Literal>");
            webClient.DownloadString(sewelisURL + "addTriple?userKey=123&storeId=1&s=" + idP + "&p=lieuMariage&o=<Literal>" + p.LieuMariage + "</Literal>");

            return new Personne(p.Nom, p.Prenom, p.Initiale, p.AnneeRegistreMatricule, p.NumeroRegistreMatricule, p.DateNaissance, p.LieuNaissance, p.DateDeces, p.LieuDeces, p.DateMariage, p.LieuMariage, p.NomConjoint, p.PrenomConjoint, p.PrenomPere, p.NomMere, p.PrenomMere, idP); 
        }

        /**
         * Récupère le doc associé au document doc.
         */
        public List<POICreationData> getPOI(Document doc)
        {
            WebClient webClient = new WebClient();
            string chemin = doc.CheminAcces.Replace(@"\", @"/");
            //chemin = doc.CheminAcces.Replace(@"\\", @"/");
            if(chemin.StartsWith("../../"))
                chemin = chemin.Substring(16, chemin.Length - 16);
            string reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + chemin);
            return parser.getPOI(reponse);
        }

        /**
         * Récupère la liste des docs associés à un POI
         */
        public List<Document> getListDocs(POICreationData poi)
        {
            WebClient webClient = new WebClient();
            String reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + poi.Id);
            String idPersonne = parser.getIdPersonne(reponse);
            reponse = webClient.DownloadString(sewelisURL + "uriDescription?userKey=123&storeId=1&uri=" + idPersonne);
            return parser.getListDocs(reponse);
        }
    }
}
