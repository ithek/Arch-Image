using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Commun;
using Modele;
using Prototype1Table.VueModele;
using Prototype1Table.Vue;
using Prototype1Table.Properties;

namespace Modele_Controleur
{
    public class ArchImage
    {
        public const string PATH_TO_SESSION_SAVE = "archimage.save";

        private const string PATH_TO_ARCHIVE_DOCS = "../../Resources/Archives_departementales/";
        
        public Utilisateur Utilisateur;

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
            get;
            set;
        }

        private IEnumerable<string> filenamesOfCurrentBook;

        public ArchImage()
        {
            SewelisAccess = new SewelisAccess();
        }


        /**
         * Commence la navigation directement sur le document fourni
         * Lance ArgumentException si l'objet ou ses attributs ne sont pas correctement initialisés 
         */
        public void Navigation(Document d)
        {
            if (d == null || !File.Exists(d.CheminAcces) || d.Position < 1 || d.PositionLivre < 1) {  //TODO check d.POI and check pos/posLivre to be existing and correlating with cheminAcces
                throw new ArgumentException();
            } else {
                this.DocumentCourant = d;
                this.filenamesOfCurrentBook = getFileNamesIn(d.Categorie, d.PositionLivre);
            }
        }

        /**
         * A appeler lorsque l'utilisateur commence à naviguer dans une categorie (depuis le menu principal ou simplement en ayant changé de catégorie)
         * Le document courant est alors le premier du premier livre de la catégorie
         * Le paramètre représente la catégorie dans laquelle l'utilisateur va naviguer
         * Lance System.IO.DirectoryNotFoundException si le dossier correspondant n'existe pas, n'est pas à sa place ou qu'aucun livre (sous-dossier) n'est trouvé 
         * Lance System.IO.FileNotFoundException si le livre parcouru ne contient pas de fichiers.
         */
        public void Navigation(Categorie categorie)
        {
            const int FIRST_BOOK = 1;
            var filenames = getFileNamesIn(categorie, FIRST_BOOK);

            if (filenames.Count() == 0)
            {
                throw new FileNotFoundException("Aucune image trouvée");
            }

            this.filenamesOfCurrentBook = filenames;
            this.DocumentCourant = new Document(categorie, filenames.ElementAt(0), FIRST_BOOK, 1); //TODO les autres attributs de Document ne sont pas initialisés : problème ? Où et quand le faire ?
            
            List<POICreationData> listePOIs = SewelisAccess.getPOI(DocumentCourant);
            DocumentCourant.POIs = listePOIs;
        }

        public int GetNbDocInCurrentBook()
        {
            return this.filenamesOfCurrentBook.Count();
        }

        /**
         * Change le document courant en prenant le prochain dans son livre s'il en reste, ou le premier du livre suivant sinon.
         * Lance System.IO.FileNotFoundException si le nouveau livre parcouru ne contient pas de fichiers.
         * Ne fait rien s'il n'y a plus de livres ensuite.
         */
        public void DocumentSuivant()
        {
            bool lastOfItsBook = (DocumentCourant.Position == this.filenamesOfCurrentBook.Count());
            string nouvCheminAcces;
            int nouvPosLivre, nouvPosDansLivre;

            if (lastOfItsBook)
            {
                bool lastBookOfCategory = (DocumentCourant.PositionLivre == getBookNamesIn(DocumentCourant.Categorie).Length);
                if (!lastBookOfCategory)
                {
                    // Use first doc of next book
                    nouvPosLivre = DocumentCourant.PositionLivre + 1;
                    this.filenamesOfCurrentBook = getFileNamesIn(DocumentCourant.Categorie, nouvPosLivre);
                    nouvCheminAcces = this.filenamesOfCurrentBook.ElementAt(0);
                    nouvPosDansLivre = 1;

                    this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
                }
            }
            else
            {
                // Use next doc from this book 
                nouvPosLivre = DocumentCourant.PositionLivre;
                nouvCheminAcces = this.filenamesOfCurrentBook.ElementAt(DocumentCourant.Position);
                nouvPosDansLivre = DocumentCourant.Position + 1;

                this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
            }

            List<POICreationData> listePOIs = SewelisAccess.getPOI(DocumentCourant);
            this.DocumentCourant.POIs = listePOIs;
        }

        /**
         * Change le document courant en prenant le précédent de son livre. 
         * S'il était le premier, on prend le dernier document du livre précédent.
         * Lance System.IO.FileNotFoundException si le nouveau livre parcouru ne contient pas de fichiers.
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

                    IEnumerable<string> docsDansLivrePrec = getFileNamesIn(DocumentCourant.Categorie, nouvPosLivre);

                    nouvCheminAcces = docsDansLivrePrec.Last();
                    nouvPosDansLivre = docsDansLivrePrec.Count();

                    this.filenamesOfCurrentBook = docsDansLivrePrec;

                    this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
                }
            }
            else
            {
                // Use previous doc from this book 
                nouvPosLivre = DocumentCourant.PositionLivre;
                nouvCheminAcces = this.filenamesOfCurrentBook.ElementAt(DocumentCourant.Position - 2);
                nouvPosDansLivre = DocumentCourant.Position - 1;

                this.DocumentCourant = new Document(DocumentCourant.Categorie, nouvCheminAcces, nouvPosLivre, nouvPosDansLivre);
            }

            List<POICreationData> listePOIs = SewelisAccess.getPOI(DocumentCourant);
            this.DocumentCourant.POIs = listePOIs;
        }

        /**
         * Change le document courant en prenant le premier document du premier livre de la catégorie précédente
         */
        public void CategoriePrecedente()
        {
            this.Navigation(GetPrev(this.DocumentCourant.Categorie));
        }

        /**
         * Change le document courant en prenant le premier document du premier livre de la catégorie suivante
         */
        public void CategorieSuivante()
        {
            this.Navigation(GetNext(this.DocumentCourant.Categorie));
        }

        /**
         * Returns the Categorie before c
         */
        public Categorie GetPrev(Categorie c)
        {
            Categorie prevCateg;
            var t = Enum.GetValues(typeof(Categorie));
            int i = Array.IndexOf(t, c);
            if (i == 0)
            {
                prevCateg = (Categorie)t.GetValue(t.Length - 1);
            }
            else
            {
                prevCateg = (Categorie)t.GetValue(i - 1);
            }

            return prevCateg;
        }

        /**
         * Returns the Categorie after c
         */
        public Categorie GetNext(Categorie c)
        {
            Categorie nextCateg;
            var t = Enum.GetValues(typeof(Categorie));
            int i = Array.IndexOf(t, c);
            if (i == t.Length - 1)
            {
                nextCateg = (Categorie)t.GetValue(0);
            }
            else
            {
                nextCateg = (Categorie)t.GetValue(i + 1);
            }

            return nextCateg;
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
                this.DocumentCourant = new Document(DocumentCourant.Categorie, this.filenamesOfCurrentBook.ElementAt(numeroDocument - 1), DocumentCourant.PositionLivre, numeroDocument);
            }

            List<POICreationData> listePOIs = SewelisAccess.getPOI(DocumentCourant);
            this.DocumentCourant.POIs = listePOIs;
        }

        public List<Document> documentDuPoi(POIWrapper poi)
        {
            throw new System.NotImplementedException();
        }

        public void creerPOI(POICreationData poi)
        {
            Console.WriteLine("Ajout d'un POI concernant " + poi.IdPersonne + " en (" + poi.posX + " ; " + poi.posY + ")");
            SewelisAccess.ajouterPOI(poi, DocumentCourant);
            List<POICreationData> listePOIs = SewelisAccess.getPOI(DocumentCourant);
            this.DocumentCourant.POIs = listePOIs;
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

        public static string CategorieToFolderName(Categorie c)
        {
            // TODO les autres catégories et dossiers correspondants
            string res = "DUDE_TODO";

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

        /**
         * Search for and returns the files in the directory 'category', in sub-directory n°'posLivre'
         * 1 <= posLivre <= N,    N being the number of books (sub-directories) in category
         */
        private IEnumerable<string> getFileNamesIn(Categorie category, int posLivre)
        {
            var dirs = getBookNamesIn(category);
            if (dirs.Length == 0)
            {
                throw new DirectoryNotFoundException("Chaque dossier de catégorie (ici " + CategorieToFolderName(category) +
                    ") doit contenir des sous-dossiers correspondants aux différents volumes à parcourir, ces sous-dossiers contenant eux-mêmes les images d'archives et rien d'autre. Aucun sous-dossier trouvé dans " + CategorieToFolderName(category) + ".");
            }
            string bookPath = dirs[posLivre - 1];
            IEnumerable<string> res = Directory.EnumerateFiles(bookPath);

            if (res.Count() == 0)
            {
                throw new FileNotFoundException("Aucune image d'archives dans le dossier demandé " + bookPath + "/."); 
            }
            
            return res;
        }

        private string[] getBookNamesIn(Categorie category)
        {
            try
            {
                return Directory.GetDirectories(PATH_TO_ARCHIVE_DOCS + CategorieToFolderName(category));
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Impossible de trouver les dossiers de la catégorie " + category.ToString() + " (vérifiez que le dossier Resources\\Archives_departementales\\" + ArchImage.CategorieToFolderName(category) + " existe bien)");
            }
        }
    }
}
