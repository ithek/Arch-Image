using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using System.Windows;
using System.Windows.Input;
using Commun;
using System.Windows.Controls;
using msvipConnexionDLL.implementations;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace Prototype1Table.VueModele
{
    class DiaporamaVM : MediaVM
    {
        public DiaporamaVM(MediaModele m, Point p, double o, ConsultationVM c) : base(m, p, o, c) 
        {
            numero = 0;
            SuivantCommande = new RelaiCommande(new Action(diapoSuivante));
            PrecedentCommande = new RelaiCommande(new Action(diapoPrecedente));
            DiaporamaModele mod = (DiaporamaModele)modele;
            
            //Recuperation de la premiere image
            Image tmp = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(obtenirAdresseAbsolue(mod.images[0].Chemin), UriKind.Absolute);
            bi.EndInit();
            tmp.Stretch = Stretch.Uniform;
            tmp.Source = bi;
           
            DiapoCourante = tmp;
        }

        /*
        * Constructeur de DiaporamaVM ne servant que pour le Menu
        */
        public DiaporamaVM(MediaModele m): base(m)
        {
            numero = 0;
            SuivantCommande = new RelaiCommande(new Action(diapoSuivante));
            PrecedentCommande = new RelaiCommande(new Action(diapoPrecedente));
            DiaporamaModele mod = (DiaporamaModele)modele;

            //Recuperation de la premiere image
            Image tmp = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(obtenirAdresseAbsolue(mod.images[0].Chemin), UriKind.Absolute);
            bi.EndInit();
            tmp.Stretch = Stretch.Uniform;
            tmp.Source = bi;

            DiapoCourante = tmp;
        }

        private Image diapoCourante;
        public Image DiapoCourante
        {
            get { return diapoCourante; }
            set
            {
                diapoCourante = value;
                RaisePropertyChanged("DiapoCourante");
            }
        }

        private int numero;
        public int Numero
        {
            get { return numero + 1; }
        }

        public int NombreTotal
        {
            get
            {
                DiaporamaModele mod = (DiaporamaModele)modele;
                return mod.images.Count;
            }
        }

        private string obtenirAdresseAbsolue(string adresseRelative)
        {
            //Accès à un chemin relatif eu chemin actuel
            string adresseCourante = Directory.GetCurrentDirectory();
            string combinaison = System.IO.Path.Combine(adresseCourante, adresseRelative);
            return System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath);
        }


        public void diapoSuivante()
        {
            DiaporamaModele mod = (DiaporamaModele)modele;
            if (++numero == mod.images.Count) numero = 0;
            RaisePropertyChanged("Numero");

            Image tmp = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(obtenirAdresseAbsolue(mod.images[numero].Chemin), UriKind.Absolute);
            bi.EndInit();
            tmp.Stretch = Stretch.Uniform;
            tmp.Source = bi;

            //Changement
            DiapoCourante = tmp;

            //Envoi de la commande
            if (ouvertTbi)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tbi,
                    msvipConnexionDLL.implementations.Commande.typeCommande.diapoSuivante,
                    Types.diaporama, "", numero, false);
            }
            if (ouvertTablette)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tablette,
                    msvipConnexionDLL.implementations.Commande.typeCommande.diapoSuivante,
                    Types.diaporama, "", numero, false);
            }
        }

        public void diapoPrecedente()
        {
            DiaporamaModele mod = (DiaporamaModele)modele;
            if (--numero == -1) numero = mod.images.Count - 1;
            RaisePropertyChanged("Numero");

            Image tmp = new Image();
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(obtenirAdresseAbsolue(mod.images[numero].Chemin), UriKind.Absolute);
            bi.EndInit();
            tmp.Stretch = Stretch.Uniform;
            tmp.Source = bi;
           
            //Changement
            DiapoCourante = tmp;

            //Envoi de la commande
            if (ouvertTbi)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tbi,
                    msvipConnexionDLL.implementations.Commande.typeCommande.diapoPrecedente,
                    Types.diaporama, "", numero, false);
            }
            if (ouvertTablette)
            {
                MainWindowVM.connexion.sendCommande(
                    msvipConnexionDLL.implementations.ClientInformation.TypePeriph.Tablette,
                    msvipConnexionDLL.implementations.Commande.typeCommande.diapoPrecedente,
                    Types.diaporama, "", numero, false);
            }
        }

        private ICommand suivantCommande;
        public ICommand SuivantCommande
        {
            get
            {
                return suivantCommande;
            }
            set
            {
                suivantCommande = value;
            }
        }

        private ICommand precedentCommande;
        public ICommand PrecedentCommande
        {
            get
            {
                return precedentCommande;
            }
            set
            {
                precedentCommande = value;
            }
        }

        //Surcharge pour permettre la synchronisation
        public override void envoiTbi()
        {
            //Init
            consultation.initOuvertureTbi(this);
            //Envoi commande dans consultationVM 
            ouvertTbi = true;
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tbi, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin, numero, false);
        }

        public override void envoiTablettes()
        {   
            //Init
            consultation.initOuvertureTablette(this);
            //Envoi commande dans consultationVM 
            ouvertTablette = true;
            MainWindowVM.connexion.sendCommande(ClientInformation.TypePeriph.Tablette, Commande.typeCommande.lancerMedia, modele.Type, modele.Chemin, numero, false);
        }
    }
}
