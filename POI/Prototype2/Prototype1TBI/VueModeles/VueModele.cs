using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Commun;
using Modele;
using msvipConnexionDLL.implementations;
using System.Threading;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.ServiceModel;

namespace Prototype1TBI.VueModeles
{
    class VueModele : VueModeleBase
    {
        private ClientPeriph _connexion;
        public ClientPeriph connexion
        {
            get { return _connexion; }
            set { _connexion = value; }
        }

        public MediaVM mediaCourant { get; private set; }


        private System.Windows.Visibility _mediaVisible;
        public System.Windows.Visibility mediaVisible
        {
            get { return _mediaVisible; }
            set
            {
                _mediaVisible = value;
                RaisePropertyChanged("mediaVisible");
            }
        }

        /* Taille de la fenetre */
        public int largeurFenetre { get; set; }
        public int hauteurFenetre { get; set; }

        private ICommand _Fermeture;
        public ICommand Fermeture
        {
            get
            {
                if (_Fermeture == null)
                    _Fermeture = new RelaiCommande(fermerApplication);
                return _Fermeture;
            }
        }

        private ICommand _FermetureAttente;
        public ICommand FermetureAttente
        {
            get
            {
                if (_FermetureAttente == null)
                    _FermetureAttente = new RelaiCommande(fermerAttenteApplication);
                return _FermetureAttente;
            }
        }

        private bool attenteConnexion;
        public bool AttenteConnexion
        {
            get { return attenteConnexion; }
            set
            {
                attenteConnexion = value;
                RaisePropertyChanged("AttenteConnexion");
            }
        }

        public VueModele()
        {
            //Initialisation tablette
            AttenteConnexion = true;

            //Lancement du Thread responsable de la connexion à la table
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += connexionTable;
            worker.RunWorkerCompleted += connexionTableComplete;
            worker.RunWorkerAsync();

            //Initialement le media est caché
            mediaCourant = new MediaVM();
            mediaCourant.mediaVisible = System.Windows.Visibility.Hidden;

            //Initialisation de la commande
            LancementCommande = new RelaiCommande<bool>(new Action<bool>(lancement));
        }

        /* Methode assuarant la connexion */
        public void connexionTable(object o, DoWorkEventArgs ea)
        {
            //Creation de la partie connexion
            connexion = new ClientPeriph(ClientInformation.TypePeriph.Tbi);
            bool connexionEtablie = false;
            while (!connexionEtablie)
            {
                try
                {
                    connexion.lancerClient("msvip");
                    connexionEtablie = true;
                }
                catch (EndpointNotFoundException)
                {

                }
            }

            //Abonnement a l'evenement
            connexion.RaiseCommandeEvent += traitementCommande;
        }

        public void connexionTableComplete(object o, RunWorkerCompletedEventArgs ea)
        {
            //Fin du processus de connexion
            AttenteConnexion = false;
        }

        /* On initialise la taille du média courant et on le rend visible */
        public void initMediaCourant()
        {
            mediaCourant.hauteurMedia = hauteurFenetre - 120;
            mediaCourant.largeurMedia = largeurFenetre;
            mediaCourant.orientationMedia = 0;
            if (mediaCourant is ImageVM)
            {
                ImageVM media = (ImageVM)mediaCourant;
                media.largeurImage = media.largeurMedia;
                media.hauteurImage = media.hauteurMedia;
            }
            //mediaCourant.centre = new Point((double)largeurFenetre / 2, (double)hauteurFenetre / 2);
            mediaCourant.mediaVisible = System.Windows.Visibility.Visible;
        }

        public void tailleFenetre(int width, int height)
        {
            largeurFenetre = width;
            hauteurFenetre = height;
        }

        public void lancement(bool lancementTabChef)
        {
            App.Current.MainWindow = new MainWindow();
            App.Current.MainWindow.DataContext = this;
            App.Current.MainWindow.Show();
        }

        public void fermerAttenteApplication()
        {
            Application.Current.Shutdown();
        }

        public void fermerApplication()
        {
            connexion.stopClient();
            Application app = Application.Current;
            app.Shutdown();
        }

        public void fermerImage()
        {
            mediaVisible = System.Windows.Visibility.Hidden;
        }

        public void ouvrirMedia(String chemin, Types type, double nombre, bool booleen)
        {
            fermerMedia();

            Uri myUri;
            switch (type)
            {
                case Types.image:
                    myUri = new Uri(obtenirAdresseAbsolue(chemin), UriKind.Absolute);
                    /* On crée une ImageVM et on change mediaCourant pour une liste où il n'y a que cette image */
                    ImageVM mediaI = new ImageVM(myUri);
                    mediaCourant = mediaI;
                    initMediaCourant();
                    RaisePropertyChanged("mediaCourant");
                    break;
                case Types.video:
                    myUri = new Uri(chemin, UriKind.Relative);
                    /* On crée une VideoVM et on change mediaCourant pour une liste où il n'y a que cette video */
                    //VideoVM mediaV = new VideoVM(myUri);
                    VideoVM mediaV = null;
                    Application.Current.Dispatcher.Invoke((Action)(()=> mediaV = new VideoVM(myUri, nombre, booleen)) );
                    mediaCourant = mediaV;
                    initMediaCourant();
                    RaisePropertyChanged("mediaCourant");
                    break;
                case Types.diaporama:
                    myUri = new Uri(chemin, UriKind.Relative);
                    /* On crée un DiaporamaVM et on change mediaCourant pour une liste où il n'y a que ce diaporama */
                    DiaporamaVM mediaD = new DiaporamaVM(myUri,nombre);
                    mediaCourant = mediaD;
                    initMediaCourant();
                    RaisePropertyChanged("mediaCourant");
                    break;
                default:
                    break;
            }
        }


        private string obtenirAdresseAbsolue(string adresseRelative)
        {
            //Accès à un chemin relatif eu chemin actuel
            string adresseCourante = Directory.GetCurrentDirectory();
            string combinaison = System.IO.Path.Combine(adresseCourante, adresseRelative);
            return System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath);
        }

        /** Reception d'une commande **/
        void traitementCommande(object sender, CommandeEventArgs e)
        {
            switch (e.Commande)
            {
                case Commande.typeCommande.lancerMedia:
                    ouvrirMedia(e.Message, e.Type,e.Nombre,e.Booleen);
                    break;
                case Commande.typeCommande.fermetureMedia:
                    fermerMedia();
                    break;
                case Commande.typeCommande.lectureVideo:
                    Application.Current.Dispatcher.Invoke((Action)(() => lectureVideo()));
                    break;
                case Commande.typeCommande.pauseVideo:
                    Application.Current.Dispatcher.Invoke((Action)(() => pauseVideo()));
                    break;
                case Commande.typeCommande.debutVideo:
                    Application.Current.Dispatcher.Invoke((Action)(() => debutVideo()));
                    break;
                case Commande.typeCommande.allerA:
                    Application.Current.Dispatcher.Invoke((Action)(() => allerA(e.Nombre)));
                    break;
                case Commande.typeCommande.diapoSuivante:
                    diapoSuivante((int)e.Nombre);
                    break;
                case Commande.typeCommande.diapoPrecedente:
                    diapoPrecedente((int)e.Nombre);
                    break;
                case Commande.typeCommande.finConnexion:
                    fermerMedia();
                    connexion.fermetureCanal();
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += connexionTable;
                    worker.RunWorkerCompleted += connexionTableComplete;
                    AttenteConnexion = true;
                    worker.RunWorkerAsync();
                    break;
                default:
                    break;
            }

        }

        #region Traitement Commandes
        void lectureVideo()
        {
            VideoVM video = (VideoVM)mediaCourant;
            if (video != null)
            {
                video.PlayVideo();
            }
        }

        void pauseVideo()
        {
            VideoVM video = (VideoVM)mediaCourant;
            if (video != null)
            {
                Application.Current.Dispatcher.Invoke((Action)(() => video.StopVideo()));
            }
        }

        void debutVideo()
        {
            VideoVM video = (VideoVM)mediaCourant;
            if (video != null)
            {
                video.RewindVideo();
            }
        }

        void allerA(double nb)
        {
            VideoVM video = (VideoVM)mediaCourant;
            if (video != null)
            {
                video.VideoPlayer.Position = TimeSpan.FromSeconds(nb);
            }
        }

        void diapoSuivante(int numDiapo)
        {
            DiaporamaVM diapo = (DiaporamaVM)mediaCourant;
            if (diapo != null)
            {
                diapo.allerA(numDiapo);
            }
        }

        void diapoPrecedente(int numDiapo)
        {
            DiaporamaVM diapo = (DiaporamaVM)mediaCourant;
            if (diapo != null)
            {
                diapo.allerA(numDiapo);
            }
        }

        void fermerMedia()
        {
            mediaCourant.fermeture();
            mediaVisible = System.Windows.Visibility.Hidden;
        }
        #endregion Traitement Commandes

        #region ICommand Commandes

        private ICommand lancementCommande;
        public ICommand LancementCommande
        {
            get
            {
                return lancementCommande;
            }
            set
            {
                lancementCommande = value;
            }
        }
        #endregion
    }
}
