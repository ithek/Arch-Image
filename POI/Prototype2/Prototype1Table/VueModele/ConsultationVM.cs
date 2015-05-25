using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using Modele;
using System.IO;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows;
using Microsoft.Surface;

namespace Prototype1Table.VueModele
{
    public class ConsultationVM : VueModeleBase
    {
        //La Vue-Modèle de la fenêtre principale
        private MainWindowVM mainWindow;
        //La modèle de vitrine de la vitrine ouverte
        public VitrineModele modele;

        //La liste des médias ouverts, on utilise une ObservableCollection pour profiter du refresh automatique de l'affichage
        public ObservableCollection<MediaVM> mediasOuverts { get; private set; }

        public MediaVM mediaToOpen
        {
            get;
            set;
        }

        //Presence de tablette
        private bool presenceTablette;
        public bool PresenceTablette
        {
            get { return presenceTablette; }
            set
            {
                presenceTablette = value;
                RaisePropertyChanged("PresenceTablette");
            }
        }
        //Presence de tbi
        private bool presenceTbi;
        public bool PresenceTbi
        {
            get { return presenceTbi; }
            set
            {
                presenceTbi = value;
                RaisePropertyChanged("PresenceTbi");
            }
        }

        //Media ouvert sur les tablettes
        private MediaVM mediaTablette;
        //Media ouvert sur les tbis
        private MediaVM mediaTbi;
        //VIsibilité des POI
        private System.Windows.Visibility _VisibilitePOI;
        public System.Windows.Visibility VisibilitePOI
        {
            get { return this._VisibilitePOI; }
            set
            {
                this._VisibilitePOI = value;
                RaisePropertyChanged("VisibilitePOI");
            }
        }

        //Liste des POI de la vitrine ouverte
        private ObservableCollection<ConteneurPoiVM> listePois;
        public ObservableCollection<ConteneurPoiVM> ListePois
        {
            get { return listePois; }
        }

        //Visibilité de la carte de niveau 1
        private System.Windows.Visibility _VisibiliteCarte1;
        public System.Windows.Visibility VisibiliteCarte1
        {
            get { return _VisibiliteCarte1; }
            set
            {
                _VisibiliteCarte1 = value;
                RaisePropertyChanged("VisibiliteCarte1");
            }
        }

        //Visibilité de la carte de niveau 2
        private System.Windows.Visibility _VisibiliteCarte2;
        public System.Windows.Visibility VisibiliteCarte2
        {
            get { return _VisibiliteCarte2; }
            set
            {
                _VisibiliteCarte2 = value;
                RaisePropertyChanged("VisibiliteCarte2");
            }
        }

        //Visibilité de la carte de niveau 3
        private System.Windows.Visibility _VisibiliteCarte3;
        public System.Windows.Visibility VisibiliteCarte3
        {
            get { return _VisibiliteCarte3; }
            set
            {
                _VisibiliteCarte3 = value;
                RaisePropertyChanged("VisibiliteCarte3");
            }
        }

        //Les valeurs qui serviront de référence pour tout ce qui est ratio
        private double initialWidth;
        private double initialHeight;

        // Taille de la largeur de la carte
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

        // Taille de la hauteur de la carte
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

        // Pour savoir la taille qu'a vraiment la map via ActualWidth
        private double _ActualWidthMap;
        public double ActualWidthMap
        {
            get { return _ActualWidthMap; }
            set
            {
                _ActualWidthMap = value;
                counterActualWidthChanged++;

                // A partir du 2e changement on asservi la taille la taille de la grid à la taille de la carte pour un bon placement des POIs
                if (counterActualWidthChanged >= 2)
                {
                    // Au 2e changement on a la taille otpimale de la carte
                    if (counterActualWidthChanged == 2)
                    {
                        initialWidth = _ActualWidthMap;
                        Ratio = 1;

                        //On place les POIs
                        foreach (PoiModele p in modele.ListePois)
                        {
                            listePois.Add(new ConteneurPoiVM(p, this));
                        }
                    }
                    //Asservissement
                    WidthGridMap = _ActualWidthMap;
                }
            }
        }

        //Compteur du nombre de fois qu'ActualWidth a été modifié
        int counterActualWidthChanged;

        // Pour savoir la taille qu'a vraiment la map via ActualHeight
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

        //Compteur du nombre de fois qu'ActualHeight a été modifié
        int counterActualHeightChanged;

        //Taile de l'écran
        public double WidthWindow;
        public double HeightWindow;
        
        // Ratio de zoom par rapport à l'ouverture de la carte
        private double ratio;
        public double Ratio
        {
            get { return ratio; }
            set
            {
                ratio = value;
                refreshCarte();
                OnRatioChanged(new RatioChangedEventArgs(ratio));
            }
        }

        //Carte de niveau 1
        //private Uri _Carte1;
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

        //Carte de niveau 2
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

        //Carte de niveau 3
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

        //Image indiquant le niveau aux utilisateurs
        private Uri _AffichageNiveau;
        public Uri AffichageNiveau
        {
            get
            {
                return _AffichageNiveau;
            }
            set
            {
                _AffichageNiveau = value;
                RaisePropertyChanged("AffichageNiveau");
            }
        }


        // code partagé par les 2 constructeurs
        private void baseCommuneConstructeur(MainWindowVM mw, string chemin)
        {
            mainWindow = mw;

            //initialisation des compteurs
            counterActualWidthChanged = 0;
            counterActualHeightChanged = 0;

            //initialisation du modèle
            modele = new VitrineModele(chemin);

            //On ne met que la premiere carte de visible
            VisibiliteCarte1 = System.Windows.Visibility.Visible;
            VisibiliteCarte2 = System.Windows.Visibility.Hidden;
            VisibiliteCarte3 = System.Windows.Visibility.Hidden;
            VisibilitePOI = System.Windows.Visibility.Hidden;
            Ratio = 1;

            //On commence au niveau 1 donc on initialise l'indicateur en conséquence
            AffichageNiveau = new Uri("/Prototype1Table;component/Resources/niv1.png", UriKind.Relative);

            // La liste des médias
            mediasOuverts = new ObservableCollection<MediaVM>();

            // La liste des POI de la vitrine
            listePois = new ObservableCollection<ConteneurPoiVM>();

            RetourMenuCommande = new RelaiCommande(new Action(retourMenu));
            RetourInitialCommande = new RelaiCommande(new Action(retourInitial));
            FermetureMedia = new RelaiCommande<MediaVM>(new Action<MediaVM>(fermerMedia));
            DemandeAideDroite = new RelaiCommande(new Action(demandeAideDroite));
            DemandeAideGauche = new RelaiCommande(new Action(demandeAideGauche));

            //Initiliasation de presenceTablette.Tbi
            RemoteServicePxS.NombreClientsChanged += new EventHandler(connexion_NombreClientsChanged);
            PresenceTablette = (MainWindowVM.connexion.nombreTablette > 0);
            PresenceTbi = (MainWindowVM.connexion.nombreTbi > 0);
        }

        private void basicConstultationVM(string chemin)
        {
            //initialisation des compteurs
            counterActualWidthChanged = 0;
            counterActualHeightChanged = 0;

            //initialisation du modèle
            modele = new VitrineModele(chemin);

            //On ne met que la premiere carte de visible
            VisibiliteCarte1 = System.Windows.Visibility.Visible;
            Ratio = 1;

            //On met les POI invisibles (pour les POI contenus dans des pop-up)
            VisibilitePOI = System.Windows.Visibility.Hidden;

            //On commence au niveau 1 donc on initialise l'indicateur en conséquence
            AffichageNiveau = new Uri("/Prototype1Table;component/Resources/niv1.png", UriKind.Relative);

            // La liste des médias
            mediasOuverts = new ObservableCollection<MediaVM>();

            // La liste des POI de la vitrine
            listePois = new ObservableCollection<ConteneurPoiVM>();

            RetourMenuCommande = new RelaiCommande(new Action(retourMenu));
            RetourInitialCommande = new RelaiCommande(new Action(retourInitial));
            FermetureMedia = new RelaiCommande<MediaVM>(new Action<MediaVM>(fermerMedia));
            DemandeAideDroite = new RelaiCommande(new Action(demandeAideDroite));
            DemandeAideGauche = new RelaiCommande(new Action(demandeAideGauche));

            //Initiliasation de presenceTablette.Tbi
            RemoteServicePxS.NombreClientsChanged += new EventHandler(connexion_NombreClientsChanged);
            //PresenceTablette = (MainWindowVM.connexion.nombreTablette > 0);
            //PresenceTbi = (MainWindowVM.connexion.nombreTbi > 0);
        }

        public ConsultationVM(string chemin)
        {
            basicConstultationVM(chemin);
        }

        //Constructeur classique
        public ConsultationVM(MainWindowVM mw, string chemin)
        {
            //Code commun aux deux constructeurs
            baseCommuneConstructeur(mw, chemin);

            string path = "";
            //On initialise les 3 cartes
            //Carte de niveau 1
            //On utilise une BitmapImage pour pouvoir contrôler le CacheOption et le mettre en OnLoad
            path = modele.pathlvl1;
            using (var file = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = file;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                Carte1 = image;
            }
            //Carte de niveau 2
            //On utilise une BitmapImage pour pouvoir contrôler le CacheOption et le mettre en OnLoad
            path = modele.pathlvl2;
            using (var file = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = file;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                Carte2 = image;
            }
            //Carte de niveau 3
            //On utilise une BitmapImage pour pouvoir contrôler le CacheOption et le mettre en OnLoad
            path = modele.pathlvl3;
            using (var file = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = file;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                Carte3 = image;
            }
        }

        //Surcharge du constructeur pour garder les cartes lors du retour à l'état initial
        public ConsultationVM(MainWindowVM mw, string chemin, BitmapImage carte1, BitmapImage carte2, BitmapImage carte3)
        {
            //Code commun aux deux constructeurs
            baseCommuneConstructeur(mw, chemin);

            //On initialise les 3 cartes
            //Carte de niveau 1
            Carte1 = carte1;
            //Carte de niveau 2
            Carte2 = carte2;
            //Carte de niveau 3
            Carte3 = carte3;
        }

        //Useless avec la taille optimale auto de la carte mais on garde le code au cas où ... ^^
        public double ratioImage()
        {
            BitmapImage image = Carte1;
            int width = image.PixelWidth;
            int height = image.PixelHeight;
            double ratio = (double)height / width;
            return ratio;
        }

        public void connexion_NombreClientsChanged(object sender, EventArgs e)
        {
            PresenceTablette = (MainWindowVM.connexion.nombreTablette > 0);
            PresenceTbi = (MainWindowVM.connexion.nombreTbi > 0);
        }

        // Fonction appelée lorsque l'on clique sur le bouton de retour au menu
        public void retourMenu()
        {
            //On libère la mémoire
            Carte1 = null;
            Carte2 = null;
            Carte3 = null;
            //On revient au menu
            mainWindow.lancementMenu();
        }

        // Fonction appelée lorsque l'on clique sur le bouton de retour à la situation initiale
        public void retourInitial()
        {
            //On ouvre une nouvelle Consultation avec les cartes déjà chargées en paramètre pour éviter d'avoir à les recharger
            mainWindow.lancementConsultation(modele.Chemin, Carte1, Carte2, Carte3);
        }

        // Fonction appelée lorsque l'on ouvre un média
        public void ouvrirMedia(MediaVM m)
        {
            //On ajoute le média à la liste des médias ouverts
            mediasOuverts.Add(m);

            if(m.cheminMedia.ToString().Contains("Archives_departementales"))
            {
                mediaToOpen = m;
            }

            VisibilitePOI = System.Windows.Visibility.Visible;
            //On signale l'ouverture d'un nouveau média
            RaisePropertyChanged("mediasOuverts");
            
        }

        // Fonction appelée lorsque l'on ferme un média
        public void fermerMedia(MediaVM m)
        {
            //On ferme le média
            m.fermeture();
            VisibilitePOI = System.Windows.Visibility.Hidden;
            //On le retire de la liste des médias ouverts
            mediasOuverts.Remove(m);
        }

        //Demande d'aide depuis le bouton à droite
        public void demandeAideDroite()
        {
            if (ApplicationServices.InitialOrientation == UserOrientation.Top)
            {
                //On inverse le sens de la vidéo selon l'application lors de l'ouverture de l'appli
                demandeAide(-1);
            }
            else
            {
                demandeAide(0);
            }
        }

        //Demande d'aide depuis le bouton à gauche
        public void demandeAideGauche()
        {
            if (ApplicationServices.InitialOrientation == UserOrientation.Top)
            {
                //On inverse le sens de la vidéo selon l'application lors de l'ouverture de l'appli
                demandeAide(0);
            }
            else
            {
                demandeAide(-1);
            }
        }

        // Méthode ouvrant la vidéo explicative en résultat de l'appui sur le bouton d'aide
        // si le paramètre est -1 alors la vidéo est retournée de 180°
        public void demandeAide(int cote)
        {
            Point p = new Point(WidthWindow / 2, HeightWindow / 2);
            VideoModele vm = Services.instance.getVideoAide();
            int o = 0;
            if (cote == -1)
            {
                o = 180;
            }
            if (vm != null)
            {
                VideoVM video = new VideoVM(vm, p, o, this);
                ouvrirMedia(video);
            }
        }

        public void initOuvertureTablette(MediaVM media)
        {
            if (mediaTablette != null)
            {
                mediaTablette.ouvertTablette = false;
            }
            mediaTablette = media;
        }

        public void initOuvertureTbi(MediaVM media)
        {
            if (mediaTbi != null)
            {
                mediaTbi.ouvertTbi = false;
            }
            mediaTbi = media;
        }

        /*
         * Methode utilisée par le code behind de Consultation.xaml pour initialiser la taille de la carte à celle de la fenêtre
         */
        public void tailleFenetre(double width, double height)
        {
            //On récupère la taille de la fenêtre
            WidthWindow = width;
            HeightWindow = height;
            //On essaye de mettre la carte au format de la fenêtre
            WidthGridMap = WidthWindow;
            HeightGridMap = HeightWindow;
            //Mais la carte refuse de se déformer si elle n'a pas la bonne résolution !!!
            //Donc il faut attendre d'avoir l'ActualWidth pour placer les POI
        }

        /*
         * 
         * 
         *  GESTION DU CHANGEMENT DE CARTE EN FONCTION DU NIVEAU DE ZOOM
         * 
         * 
         */

        /*
         * On change le fond de la carte en fonction du niveau de zoom, on en profite pour modifier l'indicateur de niveau
         * On joue sur la Visibility pour éviter de recharger à chaque fois les cartes (ce qui entrainerait une perte en fluidité de l'application)
         */
        public void refreshCarte()
        {
            if (ratio < 1.8)
            {
                VisibiliteCarte1 = System.Windows.Visibility.Visible;
                VisibiliteCarte2 = System.Windows.Visibility.Hidden;
                VisibiliteCarte3 = System.Windows.Visibility.Hidden;
                AffichageNiveau = new Uri("/Prototype1Table;component/Resources/niv1.png", UriKind.Relative);
            }
            if (ratio >= 1.75 && ratio < 3)
            {
                VisibiliteCarte1 = System.Windows.Visibility.Hidden;
                VisibiliteCarte2 = System.Windows.Visibility.Visible;
                VisibiliteCarte3 = System.Windows.Visibility.Hidden;
                AffichageNiveau = new Uri("/Prototype1Table;component/Resources/niv2.png", UriKind.Relative);
            }
            if (ratio >= 3)
            {
                VisibiliteCarte1 = System.Windows.Visibility.Hidden;
                VisibiliteCarte2 = System.Windows.Visibility.Hidden;
                VisibiliteCarte3 = System.Windows.Visibility.Visible;
                AffichageNiveau = new Uri("/Prototype1Table;component/Resources/niv3.png", UriKind.Relative);
            }
        }

        /*
         * 
         * 
         *  GESTION DE LA TAILLE DES POI EN FONCTION DU ZOOM VIA DES EVENTS CONTENANT LE RATIO DE LA CARTE
         * 
         * 
         */ 

        //Classe d'évènement pour signaler un changement dans le ratio (zoom ou dézoom de la carte)
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

        public event EventHandler<RatioChangedEventArgs> RatioChanged;

        //Code générique récupéré sur la documentation Microsoft (seuls les noms ont été changés)
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

        #region ICommand Commandes

        //Commande liée au bouton de retour au menu
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

        //Commande liée au bouton de retour à l'état initial
        private ICommand retourInitialCommande;
        public ICommand RetourInitialCommande
        {
            get
            {
                return retourInitialCommande;
            }
            set
            {
                retourInitialCommande = value;
            }
        }

        //Commande liée au bouton de fermeture des médias
        private ICommand _fermetureMedia;
        public ICommand FermetureMedia
        {
            get
            {
                return _fermetureMedia;
            }
            set
            {
                _fermetureMedia = value;
            }
        }

        //Commande liée au bouton d'aide à droite
        private ICommand _demandeAideDroite;
        public ICommand DemandeAideDroite
        {
            get
            {
                return _demandeAideDroite;
            }
            set
            {
                _demandeAideDroite = value;
            }
        }

        //Commande liée au bouton d'aide à gauche
        private ICommand _demandeAideGauche;
        public ICommand DemandeAideGauche
        {
            get
            {
                return _demandeAideGauche;
            }
            set
            {
                _demandeAideGauche = value;
            }
        }

        #endregion
    }
}
