using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using System.Windows;
using Prototype1Table.Vue;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Timers;
using System.Windows.Input;
using Commun;
using System.Windows.Controls.Primitives;
using Microsoft.Surface.Presentation.Controls.Primitives;
using Microsoft.Surface.Presentation.Controls;
using msvipConnexionDLL.implementations;

namespace Prototype1Table.VueModele
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public class VideoVM : MediaVM
    {
        private MediaElement videoPlayer;
        public MediaElement VideoPlayer
        {
            get { return videoPlayer; }
            set { videoPlayer = value; }
        }

        private bool videoIsPlaying;
        public bool VideoIsPlaying 
        {
            get { return videoIsPlaying; }
            set { 
                videoIsPlaying = value;
                RaisePropertyChanged("VideoIsPlaying");
            }
        }

        private Slider seekBar;
        public Slider SeekBar
        {
            get { return seekBar; }
            set { seekBar = value; }
        }

        private DispatcherTimer timer;

        public DateTime? PlayStarted { get; set; }


        public VideoVM(MediaModele m, Point p, double o, ConsultationVM c) : base(m, p, o,c) 
        {
            //Initialisation du Video Player
            VideoPlayer = new MediaElement();
            VideoPlayer.Source = cheminMedia;
            VideoPlayer.ScrubbingEnabled = true;
            VideoPlayer.Margin = new Thickness(3);
            VideoPlayer.Loaded += new RoutedEventHandler(VideoPlayer_Loaded);
            VideoPlayer.MediaOpened += new RoutedEventHandler(VideoPlayer_MediaOpened);
            VideoPlayer.LoadedBehavior = MediaState.Manual;
            VideoPlayer.UnloadedBehavior = MediaState.Close;

            //Initialisation du slider
            SeekBar = new SurfaceSlider();
            SeekBar.AddHandler(SurfaceThumb.DragStartedEvent, new DragStartedEventHandler(seekBar_DragStarted));
            SeekBar.AddHandler(SurfaceThumb.DragCompletedEvent, new DragCompletedEventHandler(seekBar_DragCompleted));            

            //Initialisation des commandes
            PlayCommande = new RelaiCommande(new Action(PlayButton_Click));
            PlaySmallCommande = new RelaiCommande(new Action(PlayButtonSmall_Click));
            PauseCommande = new RelaiCommande(new Action(PauseButton_Click));
            RewindCommande = new RelaiCommande(new Action(RewindButton_Click));

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
        }


        #region Slider
        void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = VideoPlayer.NaturalDuration.TimeSpan;
                SeekBar.Maximum = ts.TotalSeconds;
                SeekBar.SmallChange = 1;
                SeekBar.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            timer.Start();
        }

        private void seekBar_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void seekBar_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            double val = SeekBar.Value;
            VideoPlayer.Position = TimeSpan.FromSeconds(val);
            if (ouvertTbi)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tbi,
                    msvipConnexionDLL.implementations.Commande.typeCommande.allerA,
                    Types.video,"",
                    val,false);
            }
            if (ouvertTablette)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tablette,
                    msvipConnexionDLL.implementations.Commande.typeCommande.allerA,
                    Types.video, "",
                    val,false);
            }
        }

        bool isDragging = false;

        void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                seekBar.Value = VideoPlayer.Position.TotalSeconds;
            }
        }

        #endregion

        private void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Play();
            ((MediaElement)sender).Position = new TimeSpan(0, 0, 0, 1);
            ((MediaElement)sender).Pause();

            videoPlayer.MediaEnded += delegate(object o, RoutedEventArgs args)
            {
                videoPlayer.Position = new TimeSpan(0, 0, 0, 0);
                videoPlayer.Play();
            };
        }

        public event ChangedEventHandler OnVideoPlayerPlayed;
        protected virtual void OnPlayed(EventArgs e)
        {
            PlayStarted = DateTime.Now;
            if (OnVideoPlayerPlayed != null)
                OnVideoPlayerPlayed(this, e);
        }
        public event ChangedEventHandler OnVideoPlayerStopped;
        protected virtual void OnStopped(EventArgs e)
        {
            PlayStarted = null;
            if (OnVideoPlayerStopped != null)
                OnVideoPlayerStopped(this, e);
        }
        public event ChangedEventHandler OnVideoPlayerRewind;
        protected virtual void OnRewind(EventArgs e)
        {
            if (OnVideoPlayerRewind != null)
                OnVideoPlayerRewind(this, e);
        }

        public override void fermeture()
        {
            Console.WriteLine("Arret de la video");
            StopVideo();
        }
        
        public void StopVideo()
        {
            videoPlayer.Pause();
            VideoIsPlaying = false;
            if (ouvertTbi)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tbi,
                    msvipConnexionDLL.implementations.Commande.typeCommande.pauseVideo,
                    Types.video,"",0,false);
            }
            if (ouvertTablette)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tablette,
                    msvipConnexionDLL.implementations.Commande.typeCommande.pauseVideo,
                    Types.video,"",0,false);
            }
            //OnStopped(EventArgs.Empty);
        }

        public void PlayVideo()
        {
            videoPlayer.Play();
            VideoIsPlaying = true;
            if (ouvertTbi)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tbi,
                    msvipConnexionDLL.implementations.Commande.typeCommande.lectureVideo,
                    Types.video,"",0,false);
            }
            if (ouvertTablette)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tablette,
                    msvipConnexionDLL.implementations.Commande.typeCommande.lectureVideo,
                    Types.video,"",0,false);
            }
            //OnPlayed(EventArgs.Empty);
        }

        public void RewindVideo()
        {
            videoPlayer.Position = new TimeSpan(0, 0, 0, 0);
            if (ouvertTbi)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tbi,
                    msvipConnexionDLL.implementations.Commande.typeCommande.debutVideo,
                    Types.video, "", 0,false);
            }
            if (ouvertTablette)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tablette,
                    msvipConnexionDLL.implementations.Commande.typeCommande.debutVideo,
                    Types.video, "", 0,false);
            }
            //OnRewind(EventArgs.Empty);
        }

        public void PlayButton_Click()
        {
            PlayVideo();
        }

        private void RewindButton_Click()
        {
            RewindVideo();
        }

        private void PauseButton_Click()
        {
            StopVideo();
        }

        private void PlayButtonSmall_Click()
        {
            PlayVideo();
        }

        #region ICommand Commandes

        private ICommand playCommande;
        public ICommand PlayCommande
        {
            get
            {
                return playCommande;
            }
            set
            {
                playCommande = value;
            }
        }

        private ICommand playSmallCommande;
        public ICommand PlaySmallCommande
        {
            get
            {
                return playSmallCommande;
            }
            set
            {
                playSmallCommande = value;
            }
        }

        private ICommand pauseCommande;
        public ICommand PauseCommande
        {
            get
            {
                return pauseCommande;
            }
            set
            {
                pauseCommande = value;
            }
        }

        private ICommand rewindCommande;
        public ICommand RewindCommande
        {
            get
            {
                return rewindCommande;
            }
            set
            {
                rewindCommande = value;
            }
        }

        #endregion

        //Surcharge pour permettre la synchronisation
        public override void envoiTbi()
        {
            //Init
            consultation.initOuvertureTbi(this);
            //Envoi commande dans consultationVM  
            ouvertTbi = true;
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tbi, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin, SeekBar.Value,VideoIsPlaying);
        }

        public override void envoiTablettes()
        {
            //Init
            consultation.initOuvertureTablette(this);
            //Envoi commande dans consultationVM 
            ouvertTablette = true;
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tablette, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin, SeekBar.Value,VideoIsPlaying);
        }
    }
}
