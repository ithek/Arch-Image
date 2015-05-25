using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Commun;
using System.Windows.Input;
using msvipConnexionDLL.implementations;
using Modele;


using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Text.RegularExpressions;
using Microsoft.Surface;

namespace Prototype1Table.VueModele
{
    public class ApercuVM : VueModeleBase
    {
        private Modele.MediaModele modele;
        private CouronneVM couronneParent;

        public int index { get; set; } //rang dans la liste d'apercus de sa couronne
        public double orientation {
            get { return modulo(angle-90);}
        }

        public Uri cheminMiniature { get; private set; }

        private double saveAngle;//permet de memoriser l'angle auquel était l'apercu au moment ou on le sort de la couronne
        private Boolean sortiCouronne;//permet de savoir si l'apercu est sorti de la couronne (il est alors décoréllé de la rotation de la couronne)

        private Point _pos; //position de l'apercu dans le repère de la couronne
        public Point pos
        {
            get { return _pos; }
            set
            {
                double ancienAngle = angle;
                _pos = value;
                double delta = angle - ancienAngle;

                //on applique un modulo sur delta
                //(si l'ecart à 0 est trop grand, c'est qu'il faut prendre le modulo)
                if (delta > 40)
                    delta -= 360;
                else if (delta < -40)
                    delta += 360;

                if (!sortiCouronne)
                {
                    if (distance < 170 && distance > 60)
                    {
                        couronneParent.placerApercus(this, delta);
                        //angle = angle;
                    }
                    else
                    {
                        saveAngle = ancienAngle;
                        sortiCouronne = true;
                    }
                }

                //pour l'affichage du menu de connexion
                if (distance < 90)
                    couronneParent.menuEnvoiVisible = true;
                else
                    couronneParent.menuEnvoiVisible = false;

                RaisePropertyChanged("orientation");
            }
        }

        public double angle //angle que forme l'apercu avec le centre de la couronne
        {
            get
            {
                return modulo(toDeg(Math.Atan2((pos.Y - CouronneVM.tailleCouronne / 2), (pos.X - CouronneVM.tailleCouronne / 2))));
            }
            set
            {
                _pos.X = ((Math.Cos(toRad(value))) * CouronneVM.diametre / 2) + CouronneVM.tailleCouronne / 2;
                _pos.Y = ((Math.Sin(toRad(value))) * CouronneVM.diametre / 2) + CouronneVM.tailleCouronne / 2;
                RaisePropertyChanged("angle");
                RaisePropertyChanged("pos");
                RaisePropertyChanged("orientation");
                RaisePropertyChanged("A");
                RaisePropertyChanged("B");
                RaisePropertyChanged("C");
                RaisePropertyChanged("D");
                RaisePropertyChanged("E");
                RaisePropertyChanged("F");
            }
        }

        public double distance //distance entre l'apercu et le centre de la couronne
        {
            get
            {
                double decalage = CouronneVM.tailleCouronne/2.0;
                return (Math.Sqrt((pos.X - decalage) * (pos.X - decalage) + (pos.Y - decalage) * (pos.Y - decalage)));
            }
        }

        private Boolean _visible;//définit si l'apercu est manipulable ou dans la "pile d'apercus"
        public Boolean visible
        {
            get { return _visible; }
            set { 
                _visible = value;
                RaisePropertyChanged("visible");
            }
        }

        public string Nom { get; private set; }//nom du média correspondant

        //demi-angle occupé par l'apercu
        public double demiLargeurAng
        {
            get
            {
                if (couronneParent.simple) {
                    return toRad(20);
                }
                else
                {
                    if (angle >= 225 && angle <= 247.5)
                        return toRad(20 - ((angle - 225) * (20 - 2) / (247.5 - 225)));
                    else
                    {
                        if (angle >= 292.5 && angle <= 315)
                            return toRad((angle - 292.5) * (20 - 2) / (247.5 - 225));
                        else
                        {
                            if (visible)
                                return toRad(20);
                            else
                                return toRad(2);
                        }
                    }
                }
            }
        }

        /*
         *  points du masque de la miniature
         *  les Cos et Sin sont inversés pour ne pas avoir de -Pi/2
         */
        public Point A
        {
            get
            {
                return new Point((Math.Sin(demiLargeurAng) * 75) + 47.8,
                                 (Math.Cos(demiLargeurAng) * 75) - 69.3);
            }
        }
        public Point B
        {
            get
            {
                return new Point((Math.Sin(demiLargeurAng) * 125) + 47.8,
                                 (Math.Cos(demiLargeurAng) * 125) - 69.3);
            }
        }
        public Point C
        {
            get
            {
                return new Point((Math.Sin(-demiLargeurAng) * 125) + 47.8,
                                 (Math.Cos(-demiLargeurAng) * 125) - 69.3);
            }
        }
        public Point D
        {
            get
            {
                return new Point((Math.Sin(-demiLargeurAng) * 75) + 47.8,
                                 (Math.Cos(-demiLargeurAng) * 75) - 69.3);
            }
        }
        public Point E
        {
            get
            {
                return new Point((Math.Sin(demiLargeurAng) * 138) + 47.8,
                                 (Math.Cos(demiLargeurAng) * 138) - 69.3);
            }
        }
        public Point F
        {
            get
            {
                return new Point((Math.Sin(-demiLargeurAng) * 138) + 47.8,
                                 (Math.Cos(-demiLargeurAng) * 138) - 69.3);
            }
        }

        public bool masquePartiel { get; set; }

        public ApercuVM(MediaModele m, CouronneVM c)
        {
            couronneParent = c;
            pos = new Point(100, 100);
            modele = m;
            visible = true;

            string cheminMin = "_";

            Match match = Regex.Match(modele.Chemin, @"^(.+\\+)([\w\s\-]+)(\.[\w]+)$", RegexOptions.IgnoreCase);
            if (match.Success)
                Nom = match.Groups[2].Value;
            switch (m.Type)
            {
                case (Commun.Types.image):
                    if (match.Success)
                    {
                        cheminMin = match.Groups[1].Value + "miniatures\\" + match.Groups[2].Value + match.Groups[3].Value;
                        //si 
                        if (!System.IO.File.Exists(cheminMin))
                            cheminMin = "..\\..\\Resources\\image.png";
                    }
                    else
                        cheminMin = "..\\..\\Resources\\image.png";

                    break;
                case (Commun.Types.diaporama):
                    cheminMin = "..\\..\\Resources\\diaporama.png";
                    break;
                case (Commun.Types.video):
                    cheminMin = "..\\..\\Resources\\video.png";
                    break;
            }
            String finalCheminMiniature = m.CheminMiniature;
            System.Text.RegularExpressions.Regex myRegex = new System.Text.RegularExpressions.Regex(@"/");
            finalCheminMiniature = myRegex.Replace(finalCheminMiniature, "\\");
            cheminMiniature = new Uri(finalCheminMiniature,UriKind.Relative);
            
            sortiCouronne = false;
            saveAngle = 0;
        }

        public void tapApercuAction(Point p1)
        {
            Random r =new Random();
            //pour le faire apparaitre n'importe où :
            Point p = new Point(r.Next(200, 1720), r.Next(200, 880));

            //pour le faire apparaître près du point de contact :
            //p.Offset(r.Next(-200,200), r.Next(-200,200));
            
            double o = r.Next(0,360);
            ouvertureMedia(p, o);
        }


       //gestion du comportement à effectuer au relachement de l'apercu
        public void relacheApercuAction(Point p, double o)
        {
            //ouverture de média si il est à l'exterieur de la couronne
            if (distance > 170)
            {
                //position et orientation du contact en fonction de l'orientation de la table
                if (ApplicationServices.InitialOrientation == UserOrientation.Top)
                {
                    p.X = 1920 - p.X;
                    p.Y = 1080 - p.Y;
                    o += 180;
                }

                //orientation du doigt en fonction de l'orientation de la table
                ouvertureMedia(p, (((o+90) % 360) + 360) % 360);
            }
            else
            {
                //envoi du média si il est à l'interieur
                if (distance < 60)
                {
                    if ((pos.X - CouronneVM.tailleCouronne / 2) >= 0)
                        envoiTablette();
                    else
                        envoiTbi();
                }
            }


            if (sortiCouronne)
            {
                angle = saveAngle;
                sortiCouronne = false;
            }

            //replacer l'aperçu dans la couronne
            angle = angle;

            //masquer le menu connexion
            couronneParent.menuEnvoiVisible = false;
        }



        public void ouvertureMedia(Point p, double o)
        {
            switch (modele.Type)
            {
                case Types.image:
                    couronneParent.ouvrirMedia(new ImageVM(modele, p, o,couronneParent.Conteneur.getConsultationVM(), couronneParent.Conteneur.VueScatterView));
                    break;
                case Types.video:
                    couronneParent.ouvrirMedia(new VideoVM(modele, p, o,couronneParent.Conteneur.getConsultationVM()));
                    break;
                case Types.diaporama:
                    couronneParent.ouvrirMedia(new DiaporamaVM(modele, p, o, couronneParent.Conteneur.getConsultationVM()));
                    break;
                default:
                    break;
            }
        }


        public void envoiTbi()
        {
            (new System.Media.SoundPlayer("..//..//Resources//son.wav")).Play();
            couronneParent.Conteneur.getConsultationVM().initOuvertureTbi(null);
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tbi, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin,0,false);
        }

        public void envoiTablette()
        {
            (new System.Media.SoundPlayer("..//..//Resources//son.wav")).Play();
            couronneParent.Conteneur.getConsultationVM().initOuvertureTablette(null);
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tablette, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin,0,false);
        }


        #region fonctions utilitaires
        public static double toRad(double angle)
        {
            return angle * (Math.PI / 180);
        }

        public static double toDeg(double angle)
        {
            return angle * (180 / Math.PI);
        }

        public static double modulo(double a)
        {
            return ((a % 360) + 360) % 360;
        }
        public static double modulo(double a, int m)
        {
            return ((a % m) + m) % m;
        }
        public static int modulo(int a, int m)
        {
            return ((a % m) + m) % m;
        }
        #endregion
    }
}
