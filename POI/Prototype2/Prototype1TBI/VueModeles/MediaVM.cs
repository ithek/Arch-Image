using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using Commun;
using System.Windows;

namespace Prototype1TBI.VueModeles
{
    class MediaVM : VueModeleBase
    {
        private Uri _myUri;
        public Uri myUri
        {
            get { return _myUri; }
            set
            {
                _myUri = value;
                RaisePropertyChanged("myUri");
            }
        }

        public MediaVM(Uri u)
        {
            myUri = u;
        }

        public MediaVM() { }

        public virtual void fermeture()
        {
            Console.WriteLine("Fermeture du media");
        }

        /* Tout ce qui concerne la vue du média */
        private int _largeurMedia;
        public int largeurMedia
        {
            get { return _largeurMedia; }
            set { _largeurMedia = value; RaisePropertyChanged("largeurMedia"); }
        }

        private int _hauteurMedia;
        public int hauteurMedia
        {
            get { return _hauteurMedia; }
            set { _hauteurMedia = value; RaisePropertyChanged("hauteurMedia"); }
        }

        private int _orientationMedia;
        public int orientationMedia
        {
            get { return _orientationMedia; }
            set { _orientationMedia = value; RaisePropertyChanged("orientationMedia"); }
        }

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

        private Point _centre;
        public Point centre
        {
            get { return _centre; }
            set { _centre = value; RaisePropertyChanged("centre"); }
        }
    }
}
