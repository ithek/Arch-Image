using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using Modele;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.IO;

namespace Prototype1Table.VueModele
{
    class MenuVM : VueModeleBase
    {
        private MainWindowVM mainWindow;
        private MsvipModele modele;

        private string vitrineSelectionnee;
        public ObservableCollection<VitrineVM> liste_vitrines { get; set; }
        
        private bool modeEdition;
        public bool ModeEdition
        {
            get { return modeEdition; }
            set 
            {
                modeEdition = value;
                RaisePropertyChanged("ModeEdition");
            }
        }

        public bool MenuCreateVisible
        {
            get { return menuCreate == Visibility.Visible; }
        }

        private Visibility menuCreate;
        public Visibility MenuCreate
        {
            get { return menuCreate; }
            set
            {
                menuCreate = value;
                RaisePropertyChanged("MenuCreate");
                RaisePropertyChanged("MenuCreateVisible");
            }

        }

        private String message;
        public String Message
        {
            get { return message; }
            set
            {
                message = value;
                RaisePropertyChanged("Message");
            }
        }

        private Visibility affichageErreurLancement;
        public Visibility AffichageErreurLancement
        {
            get { return affichageErreurLancement; }
            set
            {
                affichageErreurLancement = value;
                RaisePropertyChanged("AffichageErreurLancement");
            }

        }

        private Visibility affichageErreurCreation;
        public Visibility AffichageErreurCreation
        {
            get { return affichageErreurCreation; }
            set
            {
                affichageErreurCreation = value;
                RaisePropertyChanged("AffichageErreurCreation");
            }
        }

        private Visibility aideVisible;
        public Visibility AideVisible
        {
            get { return aideVisible; }
            set
            {
                aideVisible = value;
                RaisePropertyChanged("AideVisible");
            }
        }

        private DiaporamaVM cheminAide;
        public DiaporamaVM CheminAide
        {
            get { return cheminAide; }
            set
            {
                cheminAide = value;
                RaisePropertyChanged("CheminAide");
            }
        }

        private String messageErreur;
        public String MessageErreur
        {
            get { return messageErreur; }
            set
            {
                messageErreur = value;
                RaisePropertyChanged("MessageErreur");
            }
        }

        private Object selectedSnapshotValue;
        public Object SelectedSnapshotValue
        {
            get { return selectedSnapshotValue; }
            set
            {
                if (selectedSnapshotValue != value)
                {
                    selectedSnapshotValue = value;
                    RaisePropertyChanged("SelectedSnapshotValue");
                }
            }
        }

        private int nombrePeriph;
        public int NombrePeriph
        {
            get { return nombrePeriph; }
            set
            {
                nombrePeriph = value;
                RaisePropertyChanged("NombrePeriph");
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

        public MenuVM(MainWindowVM mw)
        {
            if (IsDesignTime == false)
            {
                modele = new MsvipModele();
                ModeEdition = false;
                MenuCreate = Visibility.Collapsed;
                AideVisible = Visibility.Collapsed;
                AffichageErreurLancement = Visibility.Collapsed;
                AffichageErreurCreation = Visibility.Collapsed;
                Message = null;
                MessageErreur = null;
                mainWindow = mw;
                RemoteServicePxS.NombreClientsChanged += new EventHandler(connexion_NombreClientsChanged);
                LancementCommande = new RelaiCommande<string>(new Action<string>(lancement));
                SelectionCommande = new RelaiCommande<string>(new Action<string>(selectionChangement));
                MenuCommande = new RelaiCommande(new Action(menuChangement));
                FermetureCommande = new RelaiCommande(new Action(fermeture));
                MenuCreationCommande = new RelaiCommande(new Action(menuCreation));
                CreationVitrineCommande = new RelaiCommande<string>(new Action<string>(creationVitrine));
                FermetureErreurCommande = new RelaiCommande(new Action(fermetureErreur));
                AideCommande = new RelaiCommande(new Action(aide));

                liste_vitrines = new ObservableCollection<VitrineVM>();
                foreach (VitrineLightModele v in modele.listeVitrines)
                {
                    liste_vitrines.Add(new VitrineVM(v));
                }
            }
        }

        public void lancement(string selection)
        {
            //Verification du contenu
            bool lancement = false;
            string[] res = Directory.GetFiles(selection, "carte.*",SearchOption.AllDirectories);
            if (res.Length > 2) lancement = true;
            else
            {
                res = Directory.GetFiles(selection, "carte.*", SearchOption.TopDirectoryOnly);
                if (res.Length == 1) lancement = true;
            }


            if (lancement)
            {
                vitrineSelectionnee = selection;
                (new System.Media.SoundPlayer("..//..//Resources//son.wav")).Play();
                if (ModeEdition)
                {   
                    mainWindow.lancementCreation(selection);
                }
                else
                {
                    mainWindow.lancementConsultation(selection);
                }
            }
            else
            {
                string message = "Il manque des cartes aux niveaux suivants : ";
                res = Directory.GetFiles(selection + "\\niveau1\\", "carte.*", SearchOption.TopDirectoryOnly);
                if (res.Length != 1) message += "1 ";
                res = Directory.GetFiles(selection + "\\niveau2\\", "carte.*", SearchOption.TopDirectoryOnly);
                if (res.Length != 1) message += "2 ";
                res = Directory.GetFiles(selection + "\\niveau3\\", "carte.*", SearchOption.TopDirectoryOnly);
                if (res.Length != 1) message += "3";
                SelectedSnapshotValue = null;
                MessageErreur = message;
                AffichageErreurLancement = Visibility.Visible;
            }
        }

        public void fermetureErreur()
        {
            AffichageErreurLancement = Visibility.Collapsed;
        }

        public void fermeture()
        {
            mainWindow.fermeture();
        }

        public void selectionChangement(string selection)
        {
            vitrineSelectionnee = selection;
        }

        public void menuChangement()
        {
            ModeEdition = !ModeEdition;
            if (!modeEdition)
            {
                MenuCreate = Visibility.Collapsed;
            }
        }

        public void menuCreation()
        {
            if (MenuCreate == Visibility.Collapsed)
            {
                MenuCreate = Visibility.Visible;
            }
            else
            {
                MenuCreate = Visibility.Collapsed;
                AffichageErreurCreation = Visibility.Collapsed;
            }
        }

        public void creationVitrine(string nom)
        {
            if (nom == null || nom == "")
            {
                Message = "Veuillez rentrer un nom pour la vitrine";
                AffichageErreurCreation = Visibility.Visible;
            }
            else
            {
                bool b = false;
                char s = new char();
                for (int i = 0; i < nom.Length; i++)
                {
                    if (char.IsControl(nom, i) || !Services.instance.caractereValide(nom[i]))
                    {
                        b = true;
                        s = nom[i];
                    }
                }
                if (b == true)
                {
                    Message = "Le caractère " + s + " ne peut pas être utilisé";
                    AffichageErreurCreation = Visibility.Visible;
                }
                else if (!verificationNom(nom))
                {
                    Message = "Nom de vitrine déjà utilisé";
                    AffichageErreurCreation = Visibility.Visible;
                }
                else
                {
                    MenuCreate = Visibility.Collapsed;

                    //Creation des dossiers
                    string dossierVitrine = modele.CheminVitrine + "\\" + nom + ".vitrine";
                    Services.instance.creerDossier(dossierVitrine);
                    Services.instance.creerDossier(dossierVitrine + "\\niveau1");
                    Services.instance.creerDossier(dossierVitrine + "\\niveau2");
                    Services.instance.creerDossier(dossierVitrine + "\\niveau3");

                    //Creation de la VueModele associe
                    liste_vitrines.Add(new VitrineVM(modele.ajouterVitrine(dossierVitrine)));
                    RaisePropertyChanged("liste_vitrines");
                }
            }

        }

        public void aide()
        {
            DiaporamaModele m = Services.instance.getDiapoAide();
            if (m != null)
            {
                CheminAide = new DiaporamaVM(m);
                if (AideVisible == Visibility.Collapsed)
                    AideVisible = Visibility.Visible;
                else
                    AideVisible = Visibility.Collapsed;
            }
            else
            {
                AffichageErreurCreation = Visibility.Visible;
                Message = "Aucun diaporama n'est présent dans le dossier \"Aide\"";
            }
        }

        public bool verificationNom(string s)
        {
            bool res = true;
            for (int i = 0; i < liste_vitrines.Count && res; i++)
            {
                if (s.Equals(liste_vitrines[i].Nom, StringComparison.InvariantCultureIgnoreCase))
                {
                    res = false;
                }
            }
            return res;
        }

        /* Modification du nombre de périphériques connectés */
        public void connexion_NombreClientsChanged(object sender, EventArgs e)
        {
            NombrePeriph = MainWindowVM.connexion.nombreTablette + MainWindowVM.connexion.nombreTbi;
        }

        #region ICommand Commandes

        private ICommand menuCommande;
        public ICommand MenuCommande
        {
            get
            {
                return menuCommande;
            }
            set
            {
                menuCommande = value;
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


        private ICommand selectionCommande;
        public ICommand SelectionCommande
        {
            get
            {
                return selectionCommande;
            }
            set
            {
                selectionCommande = value;
            }
        }

        private ICommand fermetureCommande;
        public ICommand FermetureCommande
        {
            get
            {
                return fermetureCommande;
            }
            set
            {
                fermetureCommande = value;
            }
        }

        private ICommand menuCreationCommande;
        public ICommand MenuCreationCommande
        {
            get
            {
                return menuCreationCommande;
            }
            set
            {
                menuCreationCommande = value;
            }
        }

        private ICommand creationVitrineCommande;
        public ICommand CreationVitrineCommande
        {
            get
            {
                return creationVitrineCommande;
            }
            set
            {
                creationVitrineCommande = value;
            }
        }

        private ICommand fermetureErreurCommande;
        public ICommand FermetureErreurCommande
        {
            get
            {
                return fermetureErreurCommande;
            }
            set
            {
                fermetureErreurCommande = value;
            }
        }

        private ICommand aideCommande;
        public ICommand AideCommande
        {
            get
            {
                return aideCommande;
            }
            set
            {
                aideCommande = value;
            }
        }

        #endregion





    }
}
