using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using Modele;
using System.Windows.Input;
using System.Windows;
using msvipConnexionDLL.implementations;
using System.Collections.ObjectModel;

namespace Prototype1Table.VueModele
{
    public class MediaVM : VueModeleBase
    {
        //private String nom;
        //private CouronneVM couronne;
        protected ConsultationVM consultation;

        protected MediaModele modele;

        public Point position { get; set; }
        public double orientation { get; set; }

        //Liste des POI de la vitrine ouverte
        private ObservableCollection<ConteneurPoiVM> listePois;
        public ObservableCollection<ConteneurPoiVM> ListePois
        {
            get { return listePois; }
        }

        public ConsultationVM VueScatterView
        {
            get;
            set;
        }

        public Uri cheminMedia { get { return new Uri(modele.Chemin, UriKind.Relative); }}

        public bool ouvertTablette;
        public bool ouvertTbi;

        public RelaiCommande EnvoiTbi { get; set; }
        public RelaiCommande EnvoiTablettes { get; set; }

        public MediaVM(MediaModele m, Point p, double o, ConsultationVM c)
        {
            modele = m;
            ouvertTablette = false;
            ouvertTbi = false;
            listePois = c.ListePois;
            position = p;
            orientation = o;
            consultation = c;
            EnvoiTbi = new RelaiCommande(new Action(envoiTbi));
            EnvoiTablettes = new RelaiCommande(new Action(envoiTablettes));
        }

        public MediaVM(MediaModele m, Point p, double o, ConsultationVM c, ConsultationVM vueScatterView)
        {
            modele = m;
            ouvertTablette = false;
            ouvertTbi = false;
            if(vueScatterView != null)
                listePois = vueScatterView.ListePois;
            position = p;
            orientation = o;
            consultation = c;
            VueScatterView = vueScatterView;
            EnvoiTbi = new RelaiCommande(new Action(envoiTbi));
            EnvoiTablettes = new RelaiCommande(new Action(envoiTablettes));
        }

        /*
         * Constructeur de MediaVM ne servant que pour le Menu
         */
        public MediaVM(MediaModele m)
        {
            modele = m;
        }

        public virtual void fermeture()
        {
            Console.WriteLine("Fermeture du media");
        }

        public String getInfos()
        {
            throw new NotImplementedException();
        }




        public virtual void envoiTbi()
        {
            //Init
            consultation.initOuvertureTbi(this);
            //Envoi commande dans consultationVM 
            ouvertTbi = true;
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tbi, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin,0,false);
        }

        public virtual void envoiTablettes()
        {
            //Init
            consultation.initOuvertureTablette(this);
            //Envoi commande dans consultationVM 
            ouvertTablette = true;
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tablette, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin, 0, false);
        }
    }
}
