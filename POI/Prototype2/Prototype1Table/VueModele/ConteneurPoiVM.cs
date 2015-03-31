using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using Commun;
using System.Windows.Input;
using System.Windows;

namespace Prototype1Table.VueModele
{
    class ConteneurPoiVM : VueModeleBase
    {
        private PoiModele modelePoi;
        public PoiModele ModelePoi
        {
            get
            {
                return modelePoi;
            }
        }
        private ConsultationVM consultation;

        public ConsultationVM getConsultationVM()
        {
            return consultation;
        }

        private int initialX;
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

        private int initialY;
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

        VueModeleBase vueCourante;
        public VueModeleBase VueCourante
        {
            get { return vueCourante; }
            set
            {
                vueCourante = value;
                RaisePropertyChanged("VueCourante");
            }
        }

        public bool IsDesignTime
        {
            get
            {
                return (Application.Current == null) ||
                       (Application.Current.GetType() == typeof(Application));
            }
        }

        public ConteneurPoiVM(PoiModele p, ConsultationVM c)
        {
            if (IsDesignTime == false)
            {
                
                modelePoi = p;
                consultation = c;
                VueCourante = new PoiConsultationVM(this, modelePoi);

                // Initialisation des coordonnées initiales
                initialX = modelePoi.Position.X;
                initialY = modelePoi.Position.Y;
                X = initialX;
                Y = initialY;

                c.RatioChanged += ConsultationHandleRatioChanged;
            }
            //initialiserPositions();
        }

        public void ouverturePoi()
        {
            VueCourante = new CouronneVM(this, modelePoi);
        }

        public void fermeturePoi()
        {
            VueCourante = new PoiConsultationVM(this, modelePoi);
        }

        public void ouvrirMedia(MediaVM m)
        {
            consultation.ouvrirMedia(m);
        }

        /*
         * Methode pour gerer les evenements signalant une modification du ratio de largeur de la carte 
         */
        private void ConsultationHandleRatioChanged(object sender, ConsultationVM.RatioChangedEventArgs e)
        {
            double tmpX = initialX * e.Ratio;
            double tmpY = initialY * e.Ratio;
            X = (int)tmpX;
            Y = (int)tmpY;
        }
    }
}
