using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using Modele;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using Microsoft.Surface.Presentation.Controls;

namespace Prototype1Table.VueModele
{
    class CouronneVM : VueModeleBase
    {
        public bool finInitialisation;//permet d'attendre la fin de l'initialisation pour activer le comportement des apercus
        public List<ApercuVM> apercus { get; set; }//liste des apercus de la couronne

        protected ApercuVM futurSensHoraire;      //prochain apercu à faire apparaitre lors d'une rotation en sens Horaire
        protected ApercuVM futurSensAntiHoraire;  //prochain apercu à faire apparaitre lors d'une rotation anti-horaire

        protected int nbApercus;   //Parametre : nombre d'apercus manipulables dans la couronne au lancement
        protected double zonePile;    //Parametre : taille en degré de la zone de représentation de la pile
        protected double pileG;       //position angulaire du bord gauche de la pile
        protected double pileD;       //position angulaire du bord droit de la pile
        protected int nbPile;      //nombre d'apercus stockées dans la pile à tout instant
        protected bool etatCouronne; //definit l'etat de la couronne : vrai quand nbApercus sont visibles, faux quand nbApercus+1 sont visibles

        public bool simple;//definit si la couronne possede une pile ou non
        public ApercuVM apercuCourant;//apercu en cours de manipulation dans la couronne

        public ObservableCollection<MediaVM> mediasOuverts { get; private set; }// médias ouverts sur la table depuis cette couronne

        public static int diametre { get; set; }
        public static int diametreEllipse { get; set; }
        public static int tailleCouronne { get; set; }

        protected PoiModele modelePoi;
        protected ConteneurPoiVM conteneur;
        public ConteneurPoiVM Conteneur
        {
            get { return conteneur; }
        }

        protected static double initialWidth = 4000;
        protected Double widthCouronne;
        public Double WidthCouronne
        {
            get { return widthCouronne; }
            set
            {
                widthCouronne = value;
                RaisePropertyChanged("WidthCouronne");
            }
        }

        protected double initialMarge = -2500;
        protected double margeCouronne;
        public double MargeCouronne
        {
            get { return margeCouronne; }
            set
            {
                margeCouronne = value;
                RaisePropertyChanged("MargeCouronne");
            }
        }

        protected Boolean _menuEnvoiVisible;
        public Boolean menuEnvoiVisible{
            get { return _menuEnvoiVisible;}
            set {
                _menuEnvoiVisible = value;
                RaisePropertyChanged("menuEnvoiVisible");
            }
        }

        public CouronneVM(ConteneurPoiVM c, PoiModele m)
        {
            finInitialisation = false;
            diametre = 240;
            tailleCouronne = 4000;
            // tailleCouronne limite la zone de déplacement des aperçus

            modelePoi = m;
            conteneur = c;
            LancementCommande = new RelaiCommande(new Action(lancement));
            RotationEllipse = new RelaiCommande<ContainerManipulationDeltaEventArgs>(new Action<ContainerManipulationDeltaEventArgs>(rotation));

            mediasOuverts = new ObservableCollection<MediaVM>();

            if (modelePoi.Couronne.medias.Count > 8)
            {
                initialiseCouronne();
                simple = false;
            }
            else
            {
                initialiseCouronneSimple();
                simple = true;
            }

            //Pour la gestion du zoom
            c.getConsultationVM().RatioChanged += HandleRatioChanged;
            double ratio = c.getConsultationVM().Ratio;
            WidthCouronne = ratio * initialWidth;
            MargeCouronne = ratio * initialMarge;
        }


        protected void initialiseCouronne()
        {
            apercus = new List<ApercuVM>();
            int i = -1;
            int j = 0;

            nbApercus = 7;
            nbPile = (modelePoi.Couronne.medias.Count - nbApercus - 1);
            zonePile = 45;
            pileG = (270 - (zonePile / 2));
            pileD = (270 + (zonePile / 2));
            etatCouronne = false;
            
            foreach (MediaModele mediaM in modelePoi.Couronne.medias)
            {
                var apercu = new ApercuVM(mediaM, this);
                apercus.Add(apercu);
                apercu.index = i + j + 1;
                apercu.masquePartiel = false;
                //placement initial
                if (i+1 < nbApercus)
                {
                    apercu.visible = true;
                    apercu.angle = ApercuVM.modulo(i * 360 / (nbApercus + 1));
                    i++;
                }
                else
                {
                    apercu.visible = false;
                    apercu.angle = ApercuVM.modulo(pileG + j * zonePile / nbPile);
                    j++;
                }
            }
            futurSensHoraire = apercus[apercus.Count - 1];
            futurSensAntiHoraire = apercus[nbApercus];

            finInitialisation = true;
        }

        protected void initialiseCouronneSimple()
        {
            apercus = new List<ApercuVM>();
            int i = 0;

            nbApercus = modelePoi.Couronne.medias.Count;
            nbPile = 0;//car inutile dans le cas d'une couronne sans pile
            zonePile = 0;
            pileD = 0;
            pileG = 0;
            etatCouronne = true;

            foreach (MediaModele mediaM in modelePoi.Couronne.medias)
            {
                var apercu = new ApercuVM(mediaM, this);
                apercus.Add(apercu);
                apercu.index = i;
                apercu.masquePartiel = false;
                //placement initial
                if (i < nbApercus)
                {
                    apercu.visible = true;
                    apercu.angle = ApercuVM.modulo(i * 360 / nbApercus);
                    i++;
                }
            }
            futurSensHoraire = null;
            futurSensAntiHoraire = null;


            finInitialisation = true;
        }

        public void placerApercus(ApercuVM aCourant, double delta)
        {
            apercuCourant = aCourant;

            if (apercus.Count < nbApercus + 1)
                placerApercusSimple(aCourant, delta);
            else if (finInitialisation)
            {

                //-------GESTION DU COMPORTEMENT-------
                ApercuVM apercu1 = apercus[ApercuVM.modulo(futurSensAntiHoraire.index - 1, apercus.Count)];
                ApercuVM apercu2 = apercus[ApercuVM.modulo(futurSensHoraire.index + 1, apercus.Count)];

                double angle2 = ApercuVM.modulo(apercu2.angle + delta);
                double angle1 = ApercuVM.modulo(apercu1.angle + delta);

                // apparition d'un apercu de l'autre côté au début du masquage de apercu1
                if (!apercu1.masquePartiel){
                    if (angle1 >= pileG - zonePile / 2)
                    {
                        futurSensHoraire.visible = true;
                        futurSensHoraire.masquePartiel = true;
                        futurSensHoraire.angle = pileD;
                        futurSensAntiHoraire.masquePartiel = true;
                        apercu1.masquePartiel = true;

                        Console.WriteLine("masquage de " + apercu1.index + " - apparition de " + futurSensHoraire.index);
                        futurSensHoraire = apercus[ApercuVM.modulo(futurSensHoraire.index - 1, apercus.Count)];
                        nbPile--;
                    }
                }
                //changement de l'etat de l'apercu lorsque celui ci sort de la zone de masquage
                else if (angle1 < pileG - zonePile / 2)
                {
                    apercu1.masquePartiel = false;
                    Console.WriteLine(apercu1.index + " sort de la zone de masquage cote G");
                }

                if (!apercu2.masquePartiel)
                {
                    if ((angle2 <= pileD + zonePile / 2) && angle2 > pileG)
                    {
                        futurSensAntiHoraire.visible = true;
                        futurSensAntiHoraire.masquePartiel = true;
                        futurSensAntiHoraire.angle = pileG;
                        futurSensHoraire.masquePartiel = true;
                        apercu2.masquePartiel = true;

                        Console.WriteLine("masquage de " + apercu2.index + " - apparition de " + futurSensAntiHoraire.index);
                        futurSensAntiHoraire = apercus[ApercuVM.modulo(futurSensAntiHoraire.index + 1, apercus.Count)];
                        nbPile--;
                    }
                }
                else if (angle2 > pileD + zonePile/2)
                {
                    apercu2.masquePartiel = false;
                    Console.WriteLine(apercu2.index + " sort de la zone de masquage cote D");
                }
                    
                //disparition de l'apercu lors de l'arrivee sur la pile
                if (apercu1.angle + delta> pileG)
                {
                    apercu1.visible = false;
                    apercu1.masquePartiel = true;
                    futurSensAntiHoraire = apercu1;
                    nbPile++;
                    Console.WriteLine(apercu1.index + "arrive sur la pile cote G");
                }
                else if (angle2 < pileD && angle2 > pileG)
                {
                    apercu2.visible = false;
                    apercu2.masquePartiel = true;
                    futurSensHoraire = apercu2;
                    nbPile++;
                    Console.WriteLine(apercu1.index + "arrive sur la pile cote D");
                }

                

                //-------GESTION DE L'AFFICHAGE/ROTATION------
                #region v1
                int i;
                int z = 0;

                //apercus visibles
                for (i = ApercuVM.modulo(futurSensHoraire.index + 1, apercus.Count);
                    i != futurSensAntiHoraire.index;
                    i = ApercuVM.modulo(i + 1, apercus.Count))
                {
                    if (apercus[i] != aCourant)
                    {
                        //disparition a gauche
                        if (apercus[i].angle > 225 && apercus[i].angle < 270)
                            apercus[i].angle += delta / 2;
                        else if (apercus[i].angle > 270 && apercus[i].angle < 315)
                        {
                            //disparition a droite
                            apercus[i].angle += delta / 2;
                        }
                        else
                        {
                            //autres miniatures
                            apercus[i].angle += delta;
                        }
                    }
                }

                //pile d'apercus
                for (i = futurSensAntiHoraire.index;
                    i != ApercuVM.modulo(futurSensHoraire.index + 1, apercus.Count);
                    i = ApercuVM.modulo(i + 1, apercus.Count))
                {
                    z++;
                    apercus[i].angle += delta / (nbPile+1);
                }
                #endregion

                consolidation();
            }
        }


        public void consolidation()
        {
            int i;
            int j;
            for (i = ApercuVM.modulo(futurSensHoraire.index + 1, apercus.Count);
                    i != ApercuVM.modulo(futurSensAntiHoraire.index - 1, apercus.Count);
                    i = ApercuVM.modulo(i + 1, apercus.Count))
            {
                j = ApercuVM.modulo(i + 1, apercus.Count);
                if (!apercus[j].masquePartiel && !apercus[i].masquePartiel
                    && apercus[i] != apercuCourant && apercus[j] != apercuCourant)
                    apercus[j].angle = ApercuVM.modulo(apercus[i].angle + 45.0);
            }

            for (i = ApercuVM.modulo(futurSensAntiHoraire.index + 1, apercus.Count);
                   i != ApercuVM.modulo(futurSensHoraire.index - 1, apercus.Count);
                   i = ApercuVM.modulo(i + 1, apercus.Count))
            {
                j = ApercuVM.modulo(i + 1, apercus.Count);
                apercus[j].angle = ApercuVM.modulo(apercus[i].angle + 45.0 / nbPile);
            }

            i = ApercuVM.modulo(futurSensAntiHoraire.index, apercus.Count);
            j = ApercuVM.modulo(futurSensHoraire.index, apercus.Count);
            apercus[j].angle = ApercuVM.modulo(apercus[i].angle + 45.0);
        }




        public void placerApercusSimple(ApercuVM aCourant, double delta)
        {
            if (finInitialisation)
            {
                foreach (ApercuVM a in apercus)
                {
                    if (a != aCourant)
                        a.angle += delta;
                }
            }
        }

        /*
         * Changement de la taille du POI en fonction du ratio passé en argument de l'évènement. 
         */
        private void HandleRatioChanged(object sender, ConsultationVM.RatioChangedEventArgs e)
        {
            WidthCouronne = initialWidth * e.Ratio;
            MargeCouronne = e.Ratio * initialMarge;
            //On s'assure que les couronnes se ferment quand on change de niveau si les POIs
            //auquels elles sont liées disparaissent
            if (e.Ratio < 1.75)
            {
                if (modelePoi.Niveau != 1)
                {
                    conteneur.fermeturePoi();
                }
            }
            if (e.Ratio < 3)
            {
                if (modelePoi.Niveau != 2 && modelePoi.Niveau != 1)
                {
                    conteneur.fermeturePoi();
                }
            }
        }

        public ICommand LancementCommande { get; private set; }
        public void lancement()
        {
            conteneur.fermeturePoi();
        }

        
        public ICommand RotationEllipse { get; private set; }
        public void rotation(ContainerManipulationDeltaEventArgs e)
        {
            placerApercus(null, e.RotationalChange);
        }



        //gestion du Tap sur la couronne : transmission à l'apercu concerné
        public void tapCouronneAction(Point p, Point pAbs)
        {
            p.Offset( -tailleCouronne / 2, -tailleCouronne / 2);
            
            double angle = ApercuVM.modulo(toDeg(Math.Atan2(p.Y, p.X)));

            

            double seuil;

            foreach (ApercuVM ap in apercus)
            {
                if (ap.visible)
                {
                    seuil = ApercuVM.toDeg(ap.demiLargeurAng);
                    if ((ap.angle - seuil < angle) && (ap.angle + seuil > angle))
                    {
                        ap.tapApercuAction(pAbs);
                        break;
                    }
                }
            }
        }

        //gestion de TouchUp sur la couronne : transmission à l'apercu concerné
        public void upCouronneAction(Point p, double o)
        {
            if (apercuCourant != null)
            {
                apercuCourant.relacheApercuAction(p, o);
                apercuCourant = null;
            }
        }


        #region fonctions utilitaires
        private double toRad(double angle)
        {
            return angle * (Math.PI / 180);
        }

        private double toDeg(double angle)
        {
            return angle * (180 / Math.PI);
        }

        private int modulo(double a)
        {
            return (int)((a % 360) + 360) % 360;
        }
        #endregion


        public void ouvrirMedia(MediaVM m)
        {
            //commande transmise jusqu' a consultationVM
            conteneur.ouvrirMedia(m);
        }

    }
}
