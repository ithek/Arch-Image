using System;
using System.Collections.Generic;
using System.IO;
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
            get;
            set;
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

        public ArchImage()
        {
        }


        /**
         * A appeler lorsque l'utilisateur commence à naviguer dans une categorie (depuis le menu principal ou simplement en ayant changé de catégorie)
         * Le document courant est alors le premier de sa catégorie
         * Le paramètre représente la catégorie choisie
         */
        public void Navigation(Categorie categorie)
        {
            const int pos = 1;
            this.CategorieCourante = categorie;

            /* FIXME Utiliser EnumerateFiles qui est moins couteux (acces par indice toujours possible ?) */
            var docs = Directory.GetFiles("../../Resources/" + CategorieToFolderName(CategorieCourante));

            this.DocumentCourant = new Document(CategorieCourante, docs[pos - 1], pos); //FIXME les autres attributs de Document ne sont pas initialisés : problème ? Où et quand le faire ?
        }


        /**
         * Change le document courant en prenant le prochain dans sa catégorie
         */
        public void DocumentSuivant()
        {
            //FIXME pas tres efficace, on charge tous les fichiers avant même de tester
            var docs = Directory.GetFiles("../../Resources/" + CategorieToFolderName(CategorieCourante));
            int pos = DocumentCourant.Position;

            if (pos < docs.Length)
            {
                this.DocumentCourant = new Document(CategorieCourante, docs[pos], pos + 1); // Document.Position et les tableaux c# n'utilisent pas le même indiçage  
            }
        }

        /**
         * Change le document courant en prenant celui d'avant dans sa catégorie
         */
        public void DocumentPrecedent()
        {
            // FIXME pas tres efficace, on charge tous les fichiers avant même de tester
            var docs = Directory.GetFiles("../../Resources/" + CategorieToFolderName(CategorieCourante));
            int pos = DocumentCourant.Position;

            if (pos > 1)
            {
                this.DocumentCourant = new Document(CategorieCourante, docs[pos - 2], pos - 1); // Document.Position et les tableaux c# n'utilisent pas le même indiçage  
            }
        }

        /**
         * Change le document courant en prenant la premier de la catégorie précédente
         */
        public void CategoriePrecedente()
        {
            Categorie prevCateg;
            var t = Enum.GetValues(typeof(Categorie));
            int i = Array.IndexOf(t, this.CategorieCourante);
            if (i == 0)
            {
                prevCateg = (Categorie) t.GetValue(t.Length - 1);
            }
            else
            {
                prevCateg = (Categorie) t.GetValue(i - 1);
            }

            this.Navigation(prevCateg);
        }

        /**
         * Change le document courant en prenant la premier de la catégorie suivante
         */
        public void CategorieSuivante()
        {
            Categorie nextCateg;
            var t = Enum.GetValues(typeof(Categorie));
            int i = Array.IndexOf(t, this.CategorieCourante);
            if (i == t.Length - 1)
            {
                nextCateg = (Categorie) t.GetValue(0);
            }
            else
            {
                nextCateg = (Categorie) t.GetValue(i + 1);
            }

            this.Navigation(nextCateg);
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

        public void inscription(string nom, string mdp, string email)
        {
            throw new System.NotImplementedException();
        }

        public void connexion(string nom, string mdp)
        {
            throw new System.NotImplementedException();
        }

        private static string CategorieToFolderName(Categorie c)
        {
            // TODO les autres catégories et dossiers correspondants
            string res = "RM";

            if (c == Categorie.REGISTRE_MATRICULE)
            {
                res = "RM";
            }
            else if (c == Categorie.ACTE_MARIAGE)
            {
                res = "AM";
            }
            else if (c == Categorie.TABLE_REGISTRE_MATRICULE)
            {
                res = "TRM";
            }

            return res;
        }
    }
}
