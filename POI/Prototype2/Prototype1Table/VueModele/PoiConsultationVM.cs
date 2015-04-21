using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using Modele;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Media;

namespace Prototype1Table.VueModele
{
    public class PoiConsultationVM : VueModeleBase
    {

        private PoiModele modele;
        private ConteneurPoiVM conteneur;

        private static double initialWidth = 150;
        private Double widthPoi;
        public Double WidthPoi
        {
            get { return widthPoi; }
            set
            {
                widthPoi = value;
                RaisePropertyChanged("WidthPoi");
            }
        }

        private static double initialHeight = 150;
        private Double heightPoi;
        public Double HeightPoi
        {
            get { return heightPoi; }
            set
            {
                heightPoi = value;
                RaisePropertyChanged("HeightPoi");
            }
        }

        private static double initialMargePoi = -75;
        private Double margePoi;
        public Double MargePoi
        {
            get { return margePoi; }
            set
            {
                margePoi = value;
                RaisePropertyChanged("MargePoi");
            }
        }

        private System.Windows.Visibility _poiVisible;
        public System.Windows.Visibility poiVisible
        {
            get { return _poiVisible; }
            set
            {
                _poiVisible = value;
                RaisePropertyChanged("poiVisible");
            }
        }

        private string _Nom;
        public string Nom
        {
            get { return _Nom; }
            set
            {
                _Nom = value;
                RaisePropertyChanged("Nom");
                RaisePropertyChanged("RotationText");
            }
        }

        private System.Windows.Media.Brush _brushCouleur;
        public System.Windows.Media.Brush BrushCouleur
        {
            get { return _brushCouleur; }
            set
            {
                _brushCouleur = value;
                RaisePropertyChanged("BrushCouleur");
            }
        }

        // Les 3 couleurs de brush en static pour etre accessibles par tous les POI
        private static System.Windows.Media.Brush c1;
        private static System.Windows.Media.Brush c2;
        private static System.Windows.Media.Brush c3;

        public double RotationText { get { return -90 - Nom.Length * 8; } }

        // Constructeur static pour initialiser les variables static
        static PoiConsultationVM()
        {
            BrushConverter converter = new BrushConverter();
            c1 = (System.Windows.Media.Brush)converter.ConvertFromString("#bb00727f");
            c2 = (System.Windows.Media.Brush)converter.ConvertFromString("#bb1ea8b2");
            c3 = (System.Windows.Media.Brush)converter.ConvertFromString("#bbbce8ec");
        }

        public PoiConsultationVM(ConteneurPoiVM c, PoiModele pm)
        {
            modele = pm;
            conteneur = c;
            double ratio = c.getConsultationVM().Ratio;
            HeightPoi = initialHeight * ratio;
            WidthPoi = initialWidth * ratio;
            MargePoi = initialMargePoi * ratio;

            string nomPoi = "_";
            Match match = Regex.Match(modele.chemin, @"\\([A-Za-z_0-9]+)\.poi$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                nomPoi = match.Groups[1].Value;
            }
            _Nom = nomPoi;

            //On change la visibilité du POI en fonction de ce qu'il est où non dans le bon niveau de zoom
            //A l'ouverture de la vitrine, seuls les POI du niveau 1 sont sensés être visibles (le ratio de ConsultationVM est initialisée à 1 donc ok
            //Les POIs de niveau 1 sont visibles a tous les niveaux dans une idée où plus on zoom plus un a du détail via de nouveaux niveaux de POIs
            //Les niveaux de zoom sont fixés arbitrairement pour le moment
            switch (modele.Niveau)
            {
                case 1:
                    poiVisible = System.Windows.Visibility.Visible;
                    BrushCouleur = c1;
                    break;
                case 2:
                    BrushCouleur = c2;
                    if (ratio > 1.75)
                    {
                        poiVisible = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        poiVisible = System.Windows.Visibility.Hidden;
                    }
                    break;
                case 3:
                    BrushCouleur = c3;
                    if (ratio > 3)
                    {
                        poiVisible = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        poiVisible = System.Windows.Visibility.Hidden;
                    }
                    break;
                default:
                    poiVisible = System.Windows.Visibility.Hidden;
                    BrushCouleur = c1;
                    break;
            }

            c.getConsultationVM().RatioChanged += HandleRatioChanged;

            LancementCommande = new RelaiCommande(new Action(lancement));
        }

        public PoiConsultationVM(ConteneurPoiVM c, PoiModele pm,string n)
        {
            modele = pm;
            conteneur = c;
            double ratio = c.getConsultationVM().Ratio;
            HeightPoi = initialHeight * ratio;
            WidthPoi = initialWidth * ratio;
            MargePoi = initialMargePoi * ratio;
                        
            _Nom = n;

            poiVisible = System.Windows.Visibility.Visible;
            BrushCouleur = c1;

            c.getConsultationVM().RatioChanged += HandleRatioChanged;

            LancementCommande = new RelaiCommande(new Action(lancement));
        }

        public void lancement()
        {
            conteneur.ouverturePoi();
        }
        
        /*
         * Changement de la taille du POI en fonction du ratio passé en argument de l'évènement. 
         */
        private void HandleRatioChanged(object sender, ConsultationVM.RatioChangedEventArgs e)
        {
            WidthPoi = initialWidth * e.Ratio;
            HeightPoi = initialHeight * e.Ratio;
            MargePoi = initialMargePoi * e.Ratio;

            //On change la visibilité du POI en fonction de ce qu'il est où non dans le bon niveau de zoom
            //Les niveaux de zoom sont fixés arbitrairement pour le moment
            switch (modele.Niveau)
            {
                case 1:
                    poiVisible = System.Windows.Visibility.Visible;
                    break;
                case 2:
                    if (e.Ratio > 1.75)
                    {
                        poiVisible = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        poiVisible = System.Windows.Visibility.Hidden;
                    }
                    break;
                case 3:
                    if (e.Ratio > 3)
                    {
                        poiVisible = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        poiVisible = System.Windows.Visibility.Hidden;
                    }
                    break;
                default:
                    poiVisible = System.Windows.Visibility.Hidden;
                    break;
            }
        }

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
    }
}
