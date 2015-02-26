using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modele_Controleur
{
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

        public int Id
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<POIWrapper> POIs
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
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
            this.Categorie = c;
            this.CheminAcces = cheminAcces;
            this.PositionLivre = posLivre;
            this.Position = position;
        }
    }
}
