using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using System.Windows.Input;
using System.Windows;
using Modele;
using System.Windows.Media.Imaging;

namespace Prototype1Table.VueModele
{
    class MainWindowVM : VueModeleBase
    {
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

        public static RemoteServicePxS connexion;

        public bool IsDesignTime
        {
            get
            {
                return (Application.Current == null) ||
                       (Application.Current.GetType() == typeof(Application));
            }
        }

        public MainWindowVM()
        {
            if (IsDesignTime == false)
            {
                VueCourante = new MenuVM(this);
                Vue.Menu view = new Vue.Menu();
                view.DataContext = VueCourante;
                App.Current.MainWindow.Content = view;

                connexion = new RemoteServicePxS();
                connexion.lancerServeur();
            }
        }

        public void lancementConsultation(string chemin)
        {
            List<MediaModele> l = new List<MediaModele>();
            l.Add(new MediaModele(Commun.Types.image, "C:\\Users\\Cedric\\Source\\Repos\\Arch-Image2\\Vue\\Resources\\Archives_departementales\\RECENSEMENT\\TRANS_LA_FORET_1846\\FRAD035_31_6M_33903_0001_P"));
            l.Add(new MediaModele(Commun.Types.image, "C:\\Users\\Cedric\\Source\\Repos\\Arch-Image2\\Vue\\Resources\\Archives_departementales\\RECENSEMENT\\TRANS_LA_FORET_1846\\FRAD035_31_6M_33903_0001_P"));
            PoiModele poi = new PoiModele(560, 500, l, " ");

            ConsultationVM vue = new ConsultationVM(this, chemin);
            ConteneurPoiVM cont = new ConteneurPoiVM(poi, vue);
            vue.ListePois.Add(cont);

            PoiConsultationVM poiVM = new PoiConsultationVM(cont, poi, 1);

            VueCourante = vue;
            Vue.Consultation view = new Vue.Consultation();
            view.DataContext = VueCourante;
            App.Current.MainWindow.Content = view;
        }
        
        // Une surcharge de la méthode pour ne pas avoir à recharger les niveaux de carte lorsque l'on revient à l'état initial
        public void lancementConsultation(string chemin, BitmapImage carte1, BitmapImage carte2, BitmapImage carte3)
        {
            VueCourante = new ConsultationVM(this, chemin, carte1, carte2, carte3);
            Vue.Consultation view = new Vue.Consultation();
            view.DataContext = VueCourante;
            App.Current.MainWindow.Content = view;
        }

        public void lancementCreation(string chemin)
        {

            VueCourante = new CreationVM(this, chemin);
            Vue.Creation view = new Vue.Creation();
            view.DataContext = VueCourante;
            App.Current.MainWindow.Content = view;    
        }

        public void lancementMenu()
        {
            VueCourante = new MenuVM(this);
            Vue.Menu view = new Vue.Menu();
            view.DataContext = VueCourante;
            App.Current.MainWindow.Content = view;
        }

        public void fermeture()
        {
            //Arret du serveur
            connexion.stopServeur();

            //Fermeture de l'application
            Application app = Application.Current;
            app.Shutdown();
        }
       
    }
}
