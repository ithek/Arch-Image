using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Modele_Controleur
{
    [Serializable()]
    public class Document
    {
        /**
         * Inclut le nom du document et son extension
         */
        public string CheminAcces
        {
            get;
            set;
        }

        public int Id;



        public List<POICreationData> POIs
        {
            get;
            set;
        }

        public Categorie Categorie
        {
            get;
            set;
        }


        /**
         * L'emplacement du livre de ce document, à l'intérieur de sa catégorie.
         * Compris entre 1 et N (et non 0 et N-1)
         * (ex : 5ème livre de registres matricules)
         */
        public int PositionLivre
        {
            get;
            set;
        }

        /**
         * Son emplacement dans son livre. Compris entre 1 et N (et non 0 et N-1)
         * (ex : 5ème registre matricule (d'un certain livre))
         */

        public int Position
        {
            get; 
            set;
        }

        public Document(Categorie c, string cheminAcces, int posLivre, int position)
        {
            this.POIs = new List<POICreationData>();
            this.Categorie = c;
            this.CheminAcces = cheminAcces;
            this.PositionLivre = posLivre;
            this.Position = position;
        }

        public Document(string cheminAcces)
        {
            this.POIs = new List<POICreationData>();
            this.CheminAcces = cheminAcces;
        }

        public void addDocPoi(POICreationData poi){
            POIs.Add(poi);
        }
    }
}
