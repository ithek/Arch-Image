using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using Commun;
using System.Windows;
using System.IO;
using System.Diagnostics;
using Modele;
using msvipConnexionDLL.implementations;

namespace Prototype1TBI.VueModeles
{
    class ImageVM : MediaVM
    {
        private int _orientation;
        public int orientation
        {
            get { return _orientation; }
            set { _orientation = value; RaisePropertyChanged("orientation"); }
        }

        public ImageVM(Uri u) : base(u) 
        {
            largeurImage = largeurMedia;
            hauteurImage = hauteurMedia;
        }

        public void tournerGauche()
        {
            orientation += 90;
        }

        public void tournerDroite()
        {
            orientation -= 90;
        }


        private int _largeurImage;
        public int largeurImage
        {
            get { return _largeurImage; }
            set { _largeurImage = value; RaisePropertyChanged("largeurImage"); }
        }


        private int _hauteurImage;
        public int hauteurImage
        {
            get { return _hauteurImage; }
            set { _hauteurImage = value; RaisePropertyChanged("hauteurImage"); }
        }



        private ICommand _rotationGauche;
        public ICommand rotationGauche
        {
            get
            {
                if (_rotationGauche == null)
                    _rotationGauche = new RelaiCommande(tournerGauche);
                return _rotationGauche;
            }
        }

        private ICommand _rotationDroite;
        public ICommand rotationDroite
        {
            get
            {
                if (_rotationDroite == null)
                    _rotationDroite = new RelaiCommande(tournerDroite);
                return _rotationDroite;
            }
        }
    }
}
