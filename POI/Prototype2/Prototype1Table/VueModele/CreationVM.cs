using System;
using System.Collections.Generic;
using Commun;
using System.Windows.Input;
using Modele;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;

namespace Prototype1Table.VueModele
{
    class CreationVM : VueModeleBase
    {
        private MainWindowVM mainWindow;

        /*
         * La vitrine ouverte dans le mode auteur
         */ 
        private VitrineModele modele;
        public VitrineModele Modele
        {
            get
            {
                return modele;
            }
        }

        /*
         * La liste des Poi de la vitrine (ObservableCollection pour une mise à jour dynamique)
         */
        private ObservableCollection<PoiCreationVM> listePois;
        public ObservableCollection<PoiCreationVM> ListePois
        {
            get { return listePois; }
            set
            {
                listePois = value;
                RaisePropertyChanged("ListePois");
            }
        }

        /*
         * La liste des noms des Poi dans le niveau courant
         */ 
        private List<string> nomsPoi;
        public List<string> NomsPoi
        {
            get { return nomsPoi; }
            set
            {
                nomsPoi = value;
                RaisePropertyChanged("NomsPoi");
            }
        }

        /*
         * Le rapport entre la taille actuelle de la carte et la taille initiale
         */
        private double ratio;
        public double Ratio
        {
            get { return ratio; }
            set
            {
                ratio = value;
                OnRatioChanged(new RatioChangedEventArgs(ratio));
            }
        }

        /*
         * Le niveau courant
         */
        private int niveau;
        public int Niveau
        {
            get
            {
                return niveau;
            }
            set
            {
                niveau = value;
                OnNiveauChanged(new NiveauChangedEventArgs(niveau));
                RaisePropertyChanged("Niveau");
            }
        }

        /*
         * 
         */

        private double valueOnRatioChanged(RatioChangedEventArgs ratioChangedEventArgs)
        {
            throw new NotImplementedException();
        }

        /*
         * Visibilité de la carte de niveau 1 
         */
        private Visibility _VisibiliteCarte1;
        public Visibility VisibiliteCarte1
        {
            get { return _VisibiliteCarte1; }
            set
            {
                _VisibiliteCarte1 = value;
                RaisePropertyChanged("VisibiliteCarte1");
            }
        }

        /*
         * Visibilité de la carte de niveau 2 
         */
        private Visibility _VisibiliteCarte2;
        public Visibility VisibiliteCarte2
        {
            get { return _VisibiliteCarte2; }
            set
            {
                _VisibiliteCarte2 = value;
                RaisePropertyChanged("VisibiliteCarte2");
            }
        }

        /*
         * Visibilité de la carte de niveau 3
         */
        private Visibility _VisibiliteCarte3;
        public Visibility VisibiliteCarte3
        {
            get { return _VisibiliteCarte3; }
            set
            {
                _VisibiliteCarte3 = value;
                RaisePropertyChanged("VisibiliteCarte3");
            }
        }

        /*
         * Largeur initiale de la carte
         */
        private double initialWidth;

        /*
         * Hauteur initiale de la carte
         */
        private double initialHeight;

        /*
         * Largeur courante de la carte
         */
        private double _WidthGridMap;
        public double WidthGridMap
        {
            get { return _WidthGridMap; }
            set
            {
                _WidthGridMap = value;
                Ratio = _WidthGridMap / initialWidth;
                RaisePropertyChanged("WidthGridMap");
            }
        }

        /*
         * Hauteur courante de la carte
         */
        private double _HeightGridMap;
        public double HeightGridMap
        {
            get { return _HeightGridMap; }
            set
            {
                _HeightGridMap = value;
                RaisePropertyChanged("HeightGridMap");
            }
        }

        /*
         * Largeur réelle de la carte
         */
        private double _ActualWidthMap;
        public double ActualWidthMap
        {
            get { return _ActualWidthMap; }
            set
            {
                _ActualWidthMap = value;
                counterActualWidthChanged++;

                // A partir du 2e changement on asservi la taille de la grid à la taille de la carte pour un bon placement des POIs
                if (counterActualWidthChanged >= 2)
                {
                    // Au 2e changement on a la taille optimale de la carte
                    if (counterActualWidthChanged == 2)
                    {
                        initialWidth = _ActualWidthMap;
                        Ratio = 1;
                    }
                    //Asservissement
                    WidthGridMap = _ActualWidthMap;
                }
            }
        }

        /*
         * Compteur du nombre de changement d'ActualWidth
         */
        int counterActualWidthChanged;

        /*
         * Hauteur réelle de la carte
         */
        private double _ActualHeightMap;
        public double ActualHeightMap
        {
            get { return _ActualHeightMap; }
            set
            {
                _ActualHeightMap = value;
                counterActualHeightChanged++;

                // A partir du 2e changement on asservi la taille la taille de la grid à la taille de la carte pour un bon placement des POIs
                if (counterActualHeightChanged >= 2)
                {
                    // Au 2e changement on a la taille otpimale de la carte
                    if (counterActualHeightChanged == 2)
                    {
                        initialHeight = _ActualHeightMap;
                    }

                    //Asservissement
                    HeightGridMap = _ActualHeightMap;
                }
            }
        }

        /*
         * Compteur du nombre de changement d'ActualWidth
         */
        int counterActualHeightChanged;


        /*
         * Carte du niveau 1
         */
        private BitmapImage _Carte1;
        public BitmapImage Carte1
        {
            get
            {
                return _Carte1;
            }
            set
            {
                _Carte1 = value;
                RaisePropertyChanged("Carte1");
            }
        }

        /*
         * Carte du niveau 2
         */
        private BitmapImage _Carte2;
        public BitmapImage Carte2
        {
            get
            {
                return _Carte2;
            }
            set
            {
                _Carte2 = value;
                RaisePropertyChanged("Carte2");
            }
        }

        /*
         * Carte du niveau 3
         */
        private BitmapImage _Carte3;
        public BitmapImage Carte3
        {
            get
            {
                return _Carte3;
            }
            set
            {
                _Carte3 = value;
                RaisePropertyChanged("Carte3");
            }
        }


        /*
         * La profondeur qui sera donné au prochain poi créé
         */
        private int zindexCreation;
        public int ZindexCreation
        {
            get
            {
                return zindexCreation;
            }
            set
            {
                zindexCreation = value;
                RaisePropertyChanged("ZindexCreation");
            }
        }

        /*
         * Visibilité du menu d'aide du mode auteur
         */
        private Visibility aide_ouverte;
        public Visibility Aide_ouverte
        {
            get
            {
                return aide_ouverte;
            }
            set
            {
                aide_ouverte = value;
                RaisePropertyChanged("Aide_ouverte");
            }
        }

        /*
         * Constructeur du mode auteur
         */
        public CreationVM(MainWindowVM mw, string chemin)
        {
            mainWindow = mw;
            modele = new VitrineModele(chemin);
            ListePois = new ObservableCollection<PoiCreationVM>();
            Ratio = 1;
            // A l'initialisation on met le mode auteur au niveau 1
            Niveau = 1;
            counterActualWidthChanged = 0;
            counterActualHeightChanged = 0;
            // A l'initialisation le menu d'aide est caché
            Aide_ouverte = Visibility.Hidden;
            /******** Initialisation des trois cartes ********/
            //On utilise des BitmapImage pour pouvoir contrôler le CacheOption et le mettre en OnLoad

            //Carte de niveau 1
            using (var file = new System.IO.FileStream(modele.pathlvl1, FileMode.Open, FileAccess.Read))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = file;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                Carte1 = image;
            }

            //Carte de niveau 2
            using (var file = new System.IO.FileStream(modele.pathlvl2, FileMode.Open, FileAccess.Read))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = file;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                Carte2 = image;
            }

            //Carte de niveau 3
            using (var file = new System.IO.FileStream(modele.pathlvl3, FileMode.Open, FileAccess.Read))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = file;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                Carte3 = image;
            }

            // A l'initialisation seule la carte du niveau 1 est visible
            VisibiliteCarte1 = System.Windows.Visibility.Visible;
            VisibiliteCarte2 = System.Windows.Visibility.Hidden;
            VisibiliteCarte3 = System.Windows.Visibility.Hidden;
            NomsPoi = new List<string>();

            // On récupère les noms des poi du niveau 1
            getNomsPoi(Niveau);

            // On rempli la liste des Poi avec les Poi récupérés dans le modèle
            foreach (PoiModele p in modele.ListePois)
            {
                ListePois.Add(new PoiCreationVM(p, this));
            }

            // On initialise le zindex, arbitrairement choisi
            ZindexCreation = ListePois.Count + 10;

            // Définition des actions correspondant aux commandes
            RetourMenuCommande = new RelaiCommande(new Action(retourMenu));
            CreationPoi = new RelaiCommande(new Action(creerPoi));
            ChangerNiveau1 = new RelaiCommande(new Action(niveau1));
            ChangerNiveau2 = new RelaiCommande(new Action(niveau2));
            ChangerNiveau3 = new RelaiCommande(new Action(niveau3));
            Aide = new RelaiCommande(new Action(menuAide));
        }

        /*
         * Récupère la liste des noms des Poi du niveau passé en paramètre
         */
        public void getNomsPoi(int niveau)
        {
            // On vide la liste
            NomsPoi.Clear();
            // On récupère l'adresse du niveau passé en paramètre
            String[] noms = Services.instance.getContenu(modele.Chemin + "\\niveau" + niveau, Types.poi);
            // On récupère tous les noms des poi avec une expression régulière filtrant les noms des dossiers
            for (int i = 0; i < noms.Length; i++)
            {
                string nomPoi = "_";
                Match match = Regex.Match(noms[i], @"\\([A-Za-z_0-9]+)\.poi$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    nomPoi = match.Groups[1].Value;
                }
                NomsPoi.Add(nomPoi);
            }
        }

        /*
         * Change toutes les Zindex des Poi lorsqu'un d'entre eux est manipulé
         */
        public void changeZindex(PoiCreationVM p)
        {
            foreach (PoiCreationVM poi in ListePois)
            {
                if (poi == p)
                    // Le poi manipulé passe au premier plan
                    p.Zindex = ZindexCreation;
                else
                    // Tous les autres descendent d'un cran
                    poi.Zindex--;
            }
        }

        /*
         * Sauvegarde la position de tous les Poi en prenant en compte le ratio actuel
         */     
        public void sauvegarderPosition()
        {
            foreach (PoiCreationVM poi in listePois)
            {
                poi.ModelePoi.enregistrerPosition(Ratio);
            }
        }

        /*
         * Retourne au menu d'accueil
         */ 
        public void retourMenu()
        {
            mainWindow.lancementMenu();
        }

        /*
         * Créer un nouveau Poi au milieu de la carte
         */ 
        public void creerPoi()
        {
            int numero = 0;

            // On récupère le numéro le plus grand des Poi ayant un nom de la forme "Poi_1", "Poi_2"...
            foreach (string s in NomsPoi)
            {
                Match match = Regex.Match(s, "[A-Za-z0-9]+_([0-9]+)$", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    try
                    {
                        numero = Convert.ToInt32(match.Groups[1].Value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur de parsing");
                    }
                }
            }
            numero++;

            // Le nom du Poi créé est "Poi_" + le numéro récupéré
            string temp = "Poi_" + numero ;
            String chemin = modele.Chemin + "\\niveau" + Niveau + "\\" + temp + ".poi";

            // Le Poi est placé au milieu de la carte (initialWidth est une mauvaise idée puisqu'il s'agit
            // de la taille initiale de la carte dépendant de la résolution de l'écran, ce qui crée un décalage
            // des Poi selon l'écran sur lequel est lancé l'application, a changer donc !)
            PoiModele p = new PoiModele(chemin, Niveau, (int)(initialWidth / 2), (int)(initialHeight / 2));
            PoiCreationVM poi = new PoiCreationVM(p, this);
            poi.Nom = temp;

            // On ajoute le nouveau Poi dans la liste des Poi
            ListePois.Add(poi);

            // On ajoute le nom du nouveau Poi dans la liste des noms des poi du niveau courant
            NomsPoi.Add(temp);

            // On incrémente le zindex
            ZindexCreation++;
        }

        /*
         * Change le niveau à 1
         */ 
        public void niveau1()
        {
            Niveau = 1;

            // Seule la carte 1 est visible
            VisibiliteCarte1 = System.Windows.Visibility.Visible;
            VisibiliteCarte2 = System.Windows.Visibility.Hidden;
            VisibiliteCarte3 = System.Windows.Visibility.Hidden;

            // On récupère le nom des poi du niveau 1
            getNomsPoi(1);
        }

        /*
         * Change le niveau à 2
         */ 
        public void niveau2()
        {
            Niveau = 2;

            // Seule la carte 2 est visible
            VisibiliteCarte1 = System.Windows.Visibility.Hidden;
            VisibiliteCarte2 = System.Windows.Visibility.Visible;
            VisibiliteCarte3 = System.Windows.Visibility.Hidden;

            // On récupère le nom des poi du niveau 2
            getNomsPoi(2);
        }

        /*
         * Change le niveau à 3
         */ 
        public void niveau3()
        {
            Niveau = 3;

            // Seule la carte 3 est visible
            VisibiliteCarte1 = System.Windows.Visibility.Hidden;
            VisibiliteCarte2 = System.Windows.Visibility.Hidden;
            VisibiliteCarte3 = System.Windows.Visibility.Visible;

            // On récupère le nom des poi du niveau 3
            getNomsPoi(3);
        }

        /*
         * Ouvre ou ferme le menu d'aide
         */ 
        public void menuAide()
        {
            if (Aide_ouverte == Visibility.Hidden)
                Aide_ouverte = Visibility.Visible;
            else Aide_ouverte = Visibility.Hidden;
        }

        /*
         * Initialise la taille de la carte en fonction de la taille de la fenêtre récupéré dans le code behind du mode auteur
         */ 
        public void tailleFenetre(double width, double height)
        {
            //On essaye de mettre la carte au format de la fenêtre
            WidthGridMap = width;
            HeightGridMap = height;
            //Mais la carte refuse de se déformer si elle n'a pas la bonne résolution !!!
            //Donc il faut attendre d'avoir l'ActualWidth pour placer les POI
        }

        /*
         * Classe permettant de mettre à jour dynamiquement le ratio de la carte
         */ 
        public class RatioChangedEventArgs : EventArgs
        {
            public RatioChangedEventArgs(double r)
            {
                ratio = r;
            }
            private double ratio;
            public double Ratio
            {
                get { return ratio; }
            }
        }

        /*
         * Classe permettant de mettre à jour dynamiquement le niveau selectionné
         */ 
        public class NiveauChangedEventArgs : EventArgs
        {
            public NiveauChangedEventArgs(int n)
            {
                Niveau = n;
            }
            private double niveau;
            public double Niveau
            {
                get { return niveau; }
                set { niveau = value; }
            }
        }

        /*
         * Evenemet déclenché lors d'un changement de ratio
         */ 
        public event EventHandler<RatioChangedEventArgs> RatioChanged;

        protected void OnRatioChanged(RatioChangedEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<RatioChangedEventArgs> handler = RatioChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /*
         * Evenemet déclenché lors d'un changement de niveau
         */ 
        public event EventHandler<NiveauChangedEventArgs> NiveauChanged;

        protected void OnNiveauChanged(NiveauChangedEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<NiveauChangedEventArgs> handler = NiveauChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /*
         * Définition de toutes les commandes
         */ 
        #region ICommand Commandes

        /*
         * Commande pour revenir au menu
         */ 
        private ICommand retourMenuCommande;
        public ICommand RetourMenuCommande
        {
            get
            {
                return retourMenuCommande;
            }
            set
            {
                retourMenuCommande = value;
            }
        }

        /*
         * Commande pour ouvrir ou fermer le menu d'aide
         */ 
        private ICommand aide;
        public ICommand Aide
        {
            get
            {
                return aide;
            }
            set
            {
                aide = value;
            }
        }

        /*
         * Commande pour créer un nouveau Poi
         */ 
        private ICommand creationPoi;
        public ICommand CreationPoi
        {
            get
            {
                return creationPoi;
            }
            set
            {
                creationPoi = value;
            }
        }

        /*
         * Commande pour aller au niveau 1
         */ 
        private ICommand changerNiveau1;
        public ICommand ChangerNiveau1
        {
            get
            {
                return changerNiveau1;
            }
            set
            {
                changerNiveau1 = value;
            }
        }

        /*
         * Commande pour aller au niveau 2
         */ 
        private ICommand changerNiveau2;
        public ICommand ChangerNiveau2
        {
            get
            {
                return changerNiveau2;
            }
            set
            {
                changerNiveau2 = value;
            }
        }

        /*
         * Commande pour aller au niveau 3
         */ 
        private ICommand changerNiveau3;
        public ICommand ChangerNiveau3
        {
            get
            {
                return changerNiveau3;
            }
            set
            {
                changerNiveau3 = value;
            }
        }

        #endregion
    }
}
