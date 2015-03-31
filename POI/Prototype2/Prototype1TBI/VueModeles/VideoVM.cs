using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Input;
using Modele;
using Commun;

namespace Prototype1TBI.VueModeles
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    class VideoVM : MediaVM
    {
        /* Propre au fonctionnement de la vidéo */
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
            set
            {
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


        public VideoVM(Uri u, double temps, bool playing)
            : base(u)
        {
            //Initialisation du Video Player
            VideoPlayer = new MediaElement();
            VideoPlayer.Source = myUri;
            VideoPlayer.ScrubbingEnabled = true;
            VideoPlayer.Margin = new Thickness(3);
            VideoPlayer.Loaded += new RoutedEventHandler(VideoPlayer_Loaded);
            VideoPlayer.MediaOpened += new RoutedEventHandler(VideoPlayer_MediaOpened);
            VideoPlayer.LoadedBehavior = MediaState.Manual;
            VideoPlayer.UnloadedBehavior = MediaState.Close;
            videoPlayer.MediaEnded += delegate(object o, RoutedEventArgs args)
            {
                videoPlayer.Position = new TimeSpan(0, 0, 0, 0);
                videoPlayer.Play();
            };

            //Coupure du son et de l'image pour le chargement
            VideoPlayer.IsMuted = true;
            VideoPlayer.Visibility = Visibility.Hidden;

            //Initialisation du slider
            SeekBar = new Slider();
            SeekBar.AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(seekBar_DragStarted));
            SeekBar.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(seekBar_DragCompleted));

            //Initialisation des commandes
            PlayCommande = new RelaiCommande(new Action(PlayButton_Click));
            PlaySmallCommande = new RelaiCommande(new Action(PlayButtonSmall_Click));
            PauseCommande = new RelaiCommande(new Action(PauseButton_Click));
            RewindCommande = new RelaiCommande(new Action(RewindButton_Click));

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);

            //Initialisation de variables
            playingInit = playing;
            tempsInit = temps;
        }

        private bool playingInit;
        private double tempsInit;

        #region Slider
        void VideoPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            //Positionnement de la video à l'endroit indique
            if ((int)tempsInit != 0)
            {
                VideoPlayer.Position = TimeSpan.FromSeconds(tempsInit);
                seekBar.Value = VideoPlayer.Position.TotalSeconds;
            }
            else
            {
                VideoPlayer.Position = new TimeSpan(0, 0, 0, 1);
            }

            if (!playingInit)
            {
                ((MediaElement)sender).Pause();
            }
            else
            {
                VideoIsPlaying = true;
            }


            //Initialisation du seeker
            if (VideoPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = VideoPlayer.NaturalDuration.TimeSpan;
                SeekBar.Maximum = ts.TotalSeconds;
                SeekBar.SmallChange = 1;
                SeekBar.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            timer.Start();

            //Mise en route du son et de l'image
            VideoPlayer.IsMuted = false;
            VideoPlayer.Visibility = Visibility.Visible;
        }

        private void seekBar_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void seekBar_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            VideoPlayer.Position = TimeSpan.FromSeconds(SeekBar.Value);
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
            VideoPlayer.Play();
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
            Application.Current.Dispatcher.Invoke((Action)(() => StopVideo()));
            //StopVideo();
        }

        public void StopVideo()
        {
            videoPlayer.Pause();
            VideoIsPlaying = false;
            OnStopped(EventArgs.Empty);
        }

        public void PlayVideo()
        {
            videoPlayer.Play();
            VideoIsPlaying = true;
            OnPlayed(EventArgs.Empty);
        }

        public void RewindVideo()
        {
            videoPlayer.Position = new TimeSpan(0, 0, 0, 0);
            OnRewind(EventArgs.Empty);
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
    }
}
