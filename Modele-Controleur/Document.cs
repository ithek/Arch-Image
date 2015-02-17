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
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /**
         * Son emplacement dans sa catégorie. Compris entre 1 et N (et non 0 et N-1)
         * (ex : 5ème registre matricule)
         */

        public int Position
        {
            get; 
            set;
        }

        public Document(Categorie c, string cheminAcces, int position)
        {
            this.Categorie = c;
            this.CheminAcces = cheminAcces;
            this.Position = position;
        }
    }
}
