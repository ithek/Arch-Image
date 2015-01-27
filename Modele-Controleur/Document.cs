using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modele_Controleur
{
    public class Document
    {
        public string CheminAcces
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
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
         * Son emplacement dans sa catégorie
         * (ex : 5ème registre matricule)
         */

        public int Position
        {
            get; 
            set;
        }
    }
}
