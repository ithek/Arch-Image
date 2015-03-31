using System;
using Commun;
using Modele;
using System.Windows.Input;
using Prototype1Table.Vue;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows;

namespace Prototype1Table.VueModele
{
    class PoiCreationVM : VueModeleBase
    {
        /*
         * Modéle du Poi
         */ 
        private PoiModele modelePoi;
        public PoiModele ModelePoi
        {
            get
            {
                return modelePoi;
            }
            set
            {
                modelePoi = value;
            }
        }

        /*
         * Le mode auteur
         */ 
        private CreationVM creation;
        public CreationVM Creation
        {
            get { return creation; }
            set { creation = value; }
        }

        /*
         * Largeur initiale du Poi
         */ 
        private static double initialWidth = 100;

        /*
         * Largeur du Poi
         */ 
        private Double widthPoi;
        public Double WidthPoi
        {
            get { return widthPoi; }
            set
            {
                widthPoi = value;
                RaisePropertyChanged("WidthPoi");
            }
        }

        /*
         * Hauteur initiale du Poi
         */ 
        private static double initialHeight = 100;

        /*
         * Hauteur du Poi
         */ 
        private Double heightPoi;
        public Double HeightPoi
        {
            get { return heightPoi; }
            set
            {
                heightPoi = value;
                RaisePropertyChanged("HeightPoi");
            }
        }

        /*
         * Marge initiale du Poi
         */ 
        private static double initialMargePoi = -50;

        /*
         * Marge du Poi
         */ 
        private Double margePoi;
        public Double MargePoi
        {
            get { return margePoi; }
            set
            {
                margePoi = value;
                RaisePropertyChanged("MargePoi");
            }
        }

        /*
         * Visibilité du Poi
         */ 
        private Visibility poiVisible;
        public Visibility PoiVisible
        {
            get { return poiVisible; }
            set
            {
                poiVisible = value;
                RaisePropertyChanged("PoiVisible");
            }
        }

        /*
         * Abscisse initiale du Poi
         */ 
        private int initialX;

        /*
         * Abscisse du Poi
         */ 
        private double x;
        public double X
        {
            get { return x; }
            set
            {
                x = value;
                ModelePoi.Position = new System.Drawing.Point((int)x, (int)y);
                RaisePropertyChanged("X");
            }
        }

        /*
         * Ordonnée initiale du Poi
         */ 
        private int initialY;

        /*
         * Ordonnée du Poi
         */ 
        private double y;
        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                ModelePoi.Position = new System.Drawing.Point((int)x, (int)y);
                RaisePropertyChanged("Y");
            }
        }

        /*
         * Booléen indiquant si le menu principal du Poi est ouvert ou non
         */
        private bool menuOuvert;
        public bool MenuOuvert
        {
            get
            {
                return menuOuvert;
            }
            set
            {
                menuOuvert = value;
                RaisePropertyChanged("MenuOuvert");
            }
        }

        /*
         * Abscisse du menu principal du Poi
         */ 
        private double menu_X;
        public double Menu_X
        {
            get
            {
                return menu_X;
            }
            set
            {
                menu_X = value;
                RaisePropertyChanged("Menu_X");
            }
        }

        /*
         * Ordonnée du menu principal du Poi
         */ 
        private double menu_Y;
        public double Menu_Y
        {
            get
            {
                return menu_Y;
            }
            set
            {
                menu_Y = value;
                RaisePropertyChanged("Menu_Y");
            }
        }

        /*
         * Visibilité du menu de renommage
         */
        private Visibility menuRenommage;
        public Visibility MenuRenommage
        {
            get
            {
                return menuRenommage;
            }
            set
            {
                menuRenommage = value;
                RaisePropertyChanged("MenuRenommage");
            }
        }

        /*
         * Texte ecrit dans le menu de renommage
         */
        private string nouveauNom;
        public string NouveauNom
        {
            get
            {
                return nouveauNom;
            }
            set
            {
                nouveauNom = value;
                RaisePropertyChanged("NouveauNom");
            }
        }

        /*
         * Texte inscrit dans le message d'erreur du menu de renommage du poi
         */
        private string texte;
        public string Texte
        {
            get
            {
                return texte;
            }
            set
            {
                texte = value;
                RaisePropertyChanged("Texte");
            }
        }

        /*
         * Nom du Poi
         */ 
        private string _Nom;
        public string Nom
        {
            get { return _Nom; }
            set
            {
                _Nom = value;
                RaisePropertyChanged("Nom");
            }
        }

        /*
         * Profondeur du Poi
         */ 
        private int zindex;
        public int Zindex
        {
            get
            {
                return zindex;
            }
            set
            {
                zindex = value;
                RaisePropertyChanged("Zindex");
            }
        }

        /*
         * Placement du nom du Poi sur le Poi
         */ 
        public double RotationText { get { return -90 - Nom.Length * 8; } }

        /*
         * Constructeur de Poi
         */ 
        public PoiCreationVM(PoiModele pm, CreationVM c)
        {
            Creation = c;
            ModelePoi = pm;

            // On récupère la position initiale du Poi
            initialX = (int)(modelePoi.Position.X);
            initialY = (int)(modelePoi.Position.Y);

            // La position du Poi est initialisée à ses positions initiales
            X = initialX * Creation.Ratio;
            Y = initialY * Creation.Ratio;

            // La profondeur du Poi est initialisée à 3
            Zindex = 3;

            // Le menu de renommage est caché
            MenuRenommage = Visibility.Hidden;

            // Le texte dans le message d'erreur du menu renommage est vide
            Texte = null;

            // Le menu du Poi est fermé
            MenuOuvert = false;

            // On calcule la hauteur, la largeur et la marge du Poi en fonction du ratio de la carte
            HeightPoi = initialHeight * Creation.Ratio;
            WidthPoi = initialWidth * Creation.Ratio;
            MargePoi = initialMargePoi * Creation.Ratio;

            // On calcule le placement du menu du Poi en fonction de la taille du Poi
            if (WidthPoi < 200)
            {
                Menu_X = WidthPoi / 3.5;
                Menu_Y = -HeightPoi / 3.5;
            }
            else
            {
                Menu_X = 200 / 3.5;
                Menu_Y = -200 / 3.5;
            }

            // On récupère le nom du Poi connaisant l'emplacement du dossier correspondant
            string nomPoi = "_";
            Match match = Regex.Match(ModelePoi.chemin, @"\\([A-Za-z_0-9]+)\.poi$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                nomPoi = match.Groups[1].Value;
            }
            _Nom = nomPoi;

            // On affiche le Poi si le niveau du mode auteur correspond au niveau du Poi
            if (ModelePoi.Niveau == Creation.Niveau)
                PoiVisible = System.Windows.Visibility.Visible;
            else
                PoiVisible = System.Windows.Visibility.Hidden;

            Creation.RatioChanged += HandleRatioChanged;
            Creation.NiveauChanged += HandleNiveauChanged;
            PoiTouchMove = new RelaiCommande<TouchEventArgs>(new Action<TouchEventArgs>(PoiCreation_TouchMove));
            SuppressionPoi = new RelaiCommande(new Action(supprimerPoi));
            RenommagePoi = new RelaiCommande(new Action(renommerPoi));
            AnnulationRenommage = new RelaiCommande(new Action(annulerRenommage));
            OuvertureMenuRenommage = new RelaiCommande(new Action(ouvrirMenuRenommage));
            FermetureMenu = new RelaiCommande(new Action(fermerMenu));
        }

        /*
         * Permet de supprimer le Poi
         */ 
        public void supprimerPoi()
        {
            // On modifie le nom du dossier associé au Poi pour qu'il n'apparaisse plus sur l'écran
            String chemin = Creation.Modele.Chemin + "\\niveau" + Creation.Niveau + "\\" + Nom + ".poi";
            Services.instance.renommerDossier(chemin, chemin + ".old");

            // On enlève le nom du Poi de la liste des noms du mode auteur pour qu'il puisse être utilisé
            Creation.NomsPoi.Remove(Nom);

            // On enlève le Poi de la liste des Poi du mode auteur
            Creation.ListePois.Remove(this);
            Texte = "";
        }

        /*
         * Permet de renommer le Poi
         */ 
        public void renommerPoi()
        {
            // Si aucun nom est rentré dans le nemu de renommage, on affiche un message d'erreur
            if (NouveauNom == null)
                Texte = "Veuillez rentrer un nom pour le POI";
            else
            {
                // On contrôle que tous les caractères utilisés sont tolérés
                bool b = false;
                char s = new char();
                for (int i = 0; i < NouveauNom.Length; i++)
                {
                    if (char.IsControl(NouveauNom, i) || !Services.instance.caractereValide(NouveauNom[i]))
                    {
                        b = true;
                        s = NouveauNom[i];
                    }
                }

                // Si au moins un caractère n'est pas toléré, on affiche un message d'erreur
                if (b == true)
                    Texte = "Le caractère \"" + s + "\" ne peut pas être utilisé";

                // Si le nom est déjà pris, on affiche un message d'erreur
                else if (Creation.NomsPoi.Contains(NouveauNom))
                    Texte = "Un Poi a déjà " + NouveauNom + " comme nom";
                // Si le nom n'est pas le même que le nom actuel
                else if (NouveauNom != Nom)
                {
                    // On récupère le dossier où est présent le Poi
                    String chemin = Creation.Modele.Chemin + "\\niveau" + Creation.Niveau;

                    // On défini le nouveau chemin du Poi
                    string nouveauChemin = chemin + "\\" + NouveauNom + ".poi";

                    // On renomme le dossier correspondant au Poi
                    Services.instance.renommerDossier(ModelePoi.chemin, chemin + "\\" + NouveauNom + ".poi");

                    // On change le chamin du modèle
                    ModelePoi.chemin = nouveauChemin;

                    // On change le nom, on supprime l'ancien de la liste des noms des Poi du mode auteur et on ajoute le nouveau
                    Creation.NomsPoi.Remove(Nom);
                    Nom = NouveauNom;
                    Creation.NomsPoi.Add(Nom);

                    // On enlève le message d'erreur s'il y en avait un
                    Texte = null;

                    // On vide le champs texte
                    NouveauNom = null;

                    // On cache le menu de renommage
                    MenuRenommage = Visibility.Hidden;
                }
            }
        }

        /*
         * Permet d'ouvrir le menu de renommage du Poi
         */
        public void ouvrirMenuRenommage()
        {
            MenuRenommage = Visibility.Visible;
            NouveauNom = Nom;
        }

        /*
         * Permet de fermer le menu de renommage du Poi
         */ 
        public void annulerRenommage()
        {
            MenuRenommage = Visibility.Hidden;
            NouveauNom = null;
            Texte = null;
        }

        /*
         * Permet de fermer le menu principal du Poi
         */ 
        public void fermerMenu()
        {
            MenuOuvert = false;
        }

        /*
         * Change la taille du POI en fonction du ratio passé en argument de l'évènement 
         */
        private void HandleRatioChanged(object sender, CreationVM.RatioChangedEventArgs e)
        {
            WidthPoi = initialWidth * e.Ratio;
            HeightPoi = initialHeight * e.Ratio;
            MargePoi = initialMargePoi * e.Ratio;
            if (WidthPoi < 200)
            {
                Menu_X = WidthPoi / 3.5;
                Menu_Y = -HeightPoi / 3.5;
            }

            double tmpX = initialX * e.Ratio;
            double tmpY = initialY * e.Ratio;
            X = (int)tmpX;
            Y = (int)tmpY;
        }

        /*
         * Change la visibilité du POI en fonction du niveau passé en argument de l'évènement
         */
        private void HandleNiveauChanged(object sender, CreationVM.NiveauChangedEventArgs e)
        {
            if (ModelePoi.Niveau == Creation.Niveau)
                PoiVisible = System.Windows.Visibility.Visible;
            else
                PoiVisible = System.Windows.Visibility.Hidden;
        }

        /*
         * Autorise le deplacement du POI en mode auteur
         */
        public ICommand PoiTouchMove { get; set; }

        private void PoiCreation_TouchMove(TouchEventArgs e)
        {
            // On récupère le Poi à l'origine de l'événement
            PoiCreation pc = e.OriginalSource as PoiCreation;

            // On change la profondeur du Poi
            Creation.changeZindex(this);

            // On récupère le point de contact
            TouchPoint tp = e.GetTouchPoint(pc);

            // Si le menu principal n'est pas ouvert
            if (!MenuOuvert)
            {
                double x = X + tp.Position.X - pc.ActualWidth / 2;
                double y = Y + tp.Position.Y - pc.ActualHeight / 2;

                double xmax = Creation.WidthGridMap;
                double ymax = Creation.HeightGridMap;


                // Le Poi ne peux pas sortir de la carte, d'où tous les contrôles suivants
                if (x < 0)
                {
                    X = 0;
                    if (y < 0)
                        Y = 0;
                    else if (y > ymax)
                        Y = ymax;
                    else
                        Y = y;
                }
                else if (y < 0)
                {
                    Y = 0;
                    if (x < 0)
                        X = 0;
                    else if (x > xmax)
                        X = xmax;
                    else
                        X = x;
                }
                else if (x > xmax)
                {
                    X = xmax;
                    if (y < 0)
                        Y = 0;
                    else if (y > ymax)
                        Y = ymax;
                    else
                        Y = y;
                }
                else if (y > ymax)
                {
                    Y = ymax;
                    if (x < 0)
                        X = 0;
                    else if (x > xmax)
                        X = xmax;
                    else
                        X = x;
                }
                else
                {
                    X = x;
                    Y = y;
                }

                // On change la postion initiale pour la prochaine manipulation
                initialX = (int)(X / (MargePoi / initialMargePoi));
                initialY = (int)(Y / (MargePoi / initialMargePoi));
            }
        }

        /*
         * Commande pour supprimer un Poi
         */
        private ICommand suppressionPoi;
        public ICommand SuppressionPoi
        {
            get
            {
                return suppressionPoi;
            }
            set
            {
                suppressionPoi = value;
            }
        }

        /*
         * Commande pour renommer un Poi
         */
        private ICommand renommagePoi;
        public ICommand RenommagePoi
        {
            get
            {
                return renommagePoi;
            }
            set
            {
                renommagePoi = value;
            }
        }

        /*
         * Commande pour fermer le menu de renommage du Poi
         */
        private ICommand annulationRenommage;
        public ICommand AnnulationRenommage
        {
            get
            {
                return annulationRenommage;
            }
            set
            {
                annulationRenommage = value;
            }
        }

        /*
         * Commande pour ouvrir le menu de renommage du Poi
         */
        private ICommand ouvertureMenuRenommage;
        public ICommand OuvertureMenuRenommage
        {
            get
            {
                return ouvertureMenuRenommage;
            }
            set
            {
                ouvertureMenuRenommage = value;
            }
        }

        /*
         * Commande pour fermer le menu principal du Poi
         */
        private ICommand fermetureMenu;
        public ICommand FermetureMenu
        {
            get
            {
                return fermetureMenu;
            }
            set
            {
                fermetureMenu = value;
            }
        }
    }
}
