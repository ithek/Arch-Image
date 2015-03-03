using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Modele_Controleur
{
    public class ArchImage
    {

        private const string PATH_TO_ARCHIVE_DOCS = "../../Resources/Archives_departementales/";
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


        public ArchImage()
        {

        }


        /**
         * A appeler lorsque l'utilisateur commence à naviguer dans une categorie (depuis le menu principal ou simplement en ayant changé de catégorie)
         * Le document courant est alors le premier du premier livre de la catégorie
         * Le paramètre représente la catégorie dans laquelle l'utilisateur va naviguer
         */
        public void Navigation(Categorie categorie)
        {
            const int FIRST_BOOK = 1;
            var filenames = getFileNamesIn(categorie, FIRST_BOOK);
            
            this.DocumentCourant = new Document(categorie, filenames[0], FIRST_BOOK, 1); //FIXME les autres attributs de Document ne sont pas initialisés : problème ? Où et quand le faire ?
        }

        public int GetNbDocInCurrentBook()
        {
            return getFileNamesOfCurrentBook().Length;
        }   

        /**
         * Change le document courant en prenant le prochain dans son livre s'il en reste, ou le premier du livre suivant sinon.
         * Ne fait rien s'il n'y a plus de livres ensuite.
         */
        public void DocumentSuivant()
        {
            bool lastOfItsBook = (DocumentCourant.Position == getFileNamesOfCurrentBook().Length);
            bool lastBookOfCategory = (DocumentCourant.PositionLivre == getBookNamesIn(DocumentCourant.Categorie).Length);
            string nouvCheminAcces;
            int nouvPosLivre;
            int nouvPosDansLivre;

            if (lastOfItsBook)
            {
                if (! lastBookOfCategory) 
                {
                    // Use first doc of next book
                    nouvPosLivre = DocumentCourant.PositionLivre + 1;
                    nouvCheminAcces = getFileNamesIn(DocumentCourant.Categorie, nouvPosLivre)[0];
                    nouvPosDansLivre = 1;

                    this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
                }
            }
            else
            {
                // Use next doc from this book 
                nouvPosLivre = DocumentCourant.PositionLivre;
                nouvCheminAcces = getFileNamesOfCurrentBook()[DocumentCourant.Position];
                nouvPosDansLivre = DocumentCourant.Position + 1;

                this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
            }
        }

        /**
         * Change le document courant en prenant le précédent de son livre. 
         * S'il était le premier, on prend le dernier document du livre précédent.
         * S'il n'y a pas de livre précédent, on ne fait rien.
         */
        public void DocumentPrecedent()
        {
            bool firstOfItsBook = (DocumentCourant.Position == 1);
            bool firstBookOfCategory = (DocumentCourant.PositionLivre == 1);
            string nouvCheminAcces;
            int nouvPosLivre;
            int nouvPosDansLivre;

            if (firstOfItsBook)
            {
                if (!firstBookOfCategory)
                {
                    // Use last doc of previous 
                    nouvPosLivre = DocumentCourant.PositionLivre - 1;

                    var docsDansLivrePrec = getFileNamesIn(DocumentCourant.Categorie, nouvPosLivre);
                    int nombreDocDansLivrePrec = docsDansLivrePrec.Length;

                    nouvCheminAcces = docsDansLivrePrec[nombreDocDansLivrePrec - 1];
                    nouvPosDansLivre = nombreDocDansLivrePrec;

                    this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
                }
            }
            else
            {
                // Use previous doc from this book 
                nouvPosLivre = DocumentCourant.PositionLivre;
                nouvCheminAcces = getFileNamesOfCurrentBook()[DocumentCourant.Position - 2];
                nouvPosDansLivre = DocumentCourant.Position - 1;

                this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
            }
        }

        /**
         * Change le document courant en prenant le premier document du premier livre de la catégorie précédente
         */
        public void CategoriePrecedente()
        {
            Categorie prevCateg;
            var t = Enum.GetValues(typeof(Categorie));
            int i = Array.IndexOf(t, this.DocumentCourant.Categorie);
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
         * Change le document courant en prenant le premier document du premier livre de la catégorie suivante
         */
        public void CategorieSuivante()
        {
            Categorie nextCateg;
            var t = Enum.GetValues(typeof(Categorie));
            int i = Array.IndexOf(t, this.DocumentCourant.Categorie);
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
         * Change le document courant en prenant le numeroDocumentième de son livre (donc de 1 à N)
         * Lance System.ArgumentException si parametre incohérent
         */
        public void UtiliserDoc(int numeroDocument)
        {
            if (numeroDocument <= 0 || numeroDocument > GetNbDocInCurrentBook())
            {
                throw new System.ArgumentException("Il n'y a pas de document n°" + numeroDocument +
                                               " dans ce livre (categorie " + DocumentCourant.Categorie + 
                                               ", livre n°" + DocumentCourant.PositionLivre + ")");
            }
            else
            {
                //FIXME pas tres efficace
                var docs = getFileNamesOfCurrentBook();
                this.DocumentCourant = new Document(DocumentCourant.Categorie, docs[numeroDocument - 1], DocumentCourant.PositionLivre, numeroDocument);
            }
        }

        public List<Document> documentDuPoi(POIWrapper poi)
        {
            throw new System.NotImplementedException();
        }

        public void creerPOI(POICreationData poi)
        {
            Console.WriteLine("Ajout d'un POI concernant " + poi.name + " en (" + poi.posX + " ; " + poi.posY + ")");
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
            string res = "DUDE_FIXME";

            if (c == Categorie.REGISTRE_MATRICULE)
            {
                res = "REGISTRES_MILITAIRES";
            }
            else if (c == Categorie.NAISSANCE_MARIAGE_DECES)
            {
                res = "NMD";
            }
            else if (c == Categorie.TABLE_REGISTRE_MATRICULE)
            {
                res = "TABLES_RMM";
            }
            else if (c == Categorie.RECENSEMENT)
            {
                res = "RECENSEMENT";
            }
            else if (c == Categorie.TABLES_DECENNALES)
            {
                res = "TABLES_DECENNALES";
            }
            else if (c == Categorie.TSA)
            {
                res = "TSA";
            }
            else
            {
                Console.WriteLine("ERREUR : Cas non traité dans CategorieToFolderName (" + c + ")");
            }

            return res;
        }

        private string[] getFileNamesOfCurrentBook() 
        {
            return getFileNamesIn(DocumentCourant.Categorie, DocumentCourant.PositionLivre);
        }

        /**
         * Returns the files in the directory 'category', in sub-directory n°'posLivre'
         * 1 <= posLivre <= N,    N being the number of books (sub-directories) in category
         */
        private string[] getFileNamesIn(Categorie category, int posLivre)
        {
            /* FIXME Utiliser EnumerateFiles qui est moins couteux ? */
            var dirs = getBookNamesIn(category);
            return Directory.GetFiles(dirs[posLivre - 1]);
        }

        private string[] getBookNamesIn(Categorie category)
        {
            return Directory.GetDirectories(PATH_TO_ARCHIVE_DOCS + CategorieToFolderName(category));
        }
    }
}
