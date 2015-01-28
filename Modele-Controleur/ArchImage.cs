using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modele_Controleur
{
    public class ArchImage
    {
        public Utilisateur Utilisateur
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Document DocumentCourant
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public MySQLAccess MySQLAccess
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public SewelisAccess SewelisAccess
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Categorie CategorieCourante
        {
            get; 
            set;
        }

        /**
         * Change le document courant en prenant le prochain dans sa catégorie
         */
        public void DocumentSuivant()
        {
            throw new System.NotImplementedException();
        }

        /**
         * Change le document courant en prenant celui d'avant dans sa catégorie
         */
        public void DocumentPrecedent()
        {
            throw new System.NotImplementedException();
        }

        /**
         * Change le document courant en prenant la premier de la catégorie précédente
         */
        public void CategoriePrecedente()
        {
            throw new System.NotImplementedException();
        }

        /**
         * Change le document courant en prenant la premier de la catégorie suivante
         */
        public void CategorieSuivante()
        {
            throw new System.NotImplementedException();
        }

        /**
         * Change le document courant en prenant le numeroDocumentième de sa catégorie
         * FIXME throw exception si parametre incohérent
         */ 
        public void UtiliserDoc(int numeroDocument)
        {
            throw new System.NotImplementedException();

            //if (numeroDocument <= 0 || ... (trop élevé) )
            throw new System.ArgumentException("Il n'y a pas de document n°" + numeroDocument +
                                               " dans cette catégorie");
        }

        public List<Document> documentDuPoi(POIWrapper poi)
        {
            throw new System.NotImplementedException();
        }

        public void créerPOI()
        {
            throw new System.NotImplementedException();
        }

        public void POISurDoc(Document doc)
        {
            throw new System.NotImplementedException();
        }
    }
}
