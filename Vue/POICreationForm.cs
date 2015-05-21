using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Modele_Controleur;
using System.Threading;
using System.Windows.Threading;

namespace Vue
{
    public partial class POICreationForm : Form
    {
        private ArchImage archimage
        {
            get;
            set;
        }

        private POICreationData data
        {
            get;
            set;
        }

        private List<Personne> listePersonnes;
        private double left;
        private double top;
        private NavigationPage navigationPage;

        public POICreationForm(double left, double top, ArchImage archimage, NavigationPage navigationPage)
        {
            InitializeComponent();
            this.data = new POICreationData(left, top);
            this.left = left;
            this.top = top;
            this.archimage = archimage;
            this.archimage.SewelisAccess.chargerListePersonnes();
            this.navigationPage = navigationPage;
            List<String> l = new List<String>();
            l.Add("<Nouvelle personne>");
            this.listeBoxPersonnes.DataSource = l;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void parseUserInput()
        {
            //this.data.IdPersonne = this.nameTextBox.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.parseUserInput();

            if (data.Personne != null)
            {
                creerPOI_Delegate d = new creerPOI_Delegate(creerPOI);

                IAsyncResult R = null;
                R = d.BeginInvoke(new AsyncCallback(finCreerPOI), null); //invoking the method
            }
            else if (!this.nameTextBox.Text.Equals(""))
            {
                string nom = this.nameTextBox.Text;
                string prenom = this.prenomTextBox.Text;
                string initiale = this.initialeTextBox.Text;
                string ddn = this.dateNaissanceTextBox.Text;
                Personne p = new Personne(nom, prenom, initiale, ddn);

                this.data.Personne = this.archimage.SewelisAccess.ajouterPersonne(p);

                creerPOI_Delegate d = new creerPOI_Delegate(creerPOI);

                IAsyncResult R = null;
                R = d.BeginInvoke(new AsyncCallback(finCreerPOI), null); //invoking the method
            }

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listeBoxPersonnes.DataSource != null && listeBoxPersonnes.SelectedIndex != 0)
            {
                Personne personne = listePersonnes[this.listeBoxPersonnes.SelectedIndex - 1];
                this.nameTextBox.Text = personne.Nom;
                this.prenomTextBox.Text = personne.Prenom;
                this.initialeTextBox.Text = personne.Initiale;
                this.dateNaissanceTextBox.Text = personne.DateNaissance;

                data.Personne = personne;
            }
            else if (listeBoxPersonnes.SelectedIndex == 0)
            {
                this.nameTextBox.Text = "";
                this.prenomTextBox.Text = "";
                this.initialeTextBox.Text = "";
                this.dateNaissanceTextBox.Text = "";
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.nameTextBox.Text = "";
            this.prenomTextBox.Text = "";
            this.initialeTextBox.Text = "";
            this.dateNaissanceTextBox.Text = "";
            this.listeBoxPersonnes.DataSource = null;
            data.Personne = null;

            if (!this.nameSearchTextBox.Text.Equals(""))
            {
                lock (chargementPictureBox)
                {
                    this.chargementPictureBox.Visible = true;
                }

                rechercherPersonne_Delegate d = new rechercherPersonne_Delegate(rechercherPersonne);

                IAsyncResult R = null;
                R = d.BeginInvoke(this.nameSearchTextBox.Text, new AsyncCallback(finRecherchePersonne), null); //invoking the method
            }
            else
            {
                List<String> listeNoms = new List<String>();
                listeNoms.Insert(0, "<Nouvelle personne>");
                lock (listeBoxPersonnes)
                {
                    listeBoxPersonnes.DataSource = listeNoms;
                }
            }
        }

        public void rechercherPersonne(string nom)
        {
            listePersonnes = this.archimage.SewelisAccess.recherchePersonnes(this.nameSearchTextBox.Text);

            List<String> listeNoms = new List<String>();

            if (listePersonnes != null)
            {
                foreach (Personne personne in listePersonnes)
                {
                    listeNoms.Add(personne.Nom);
                }
                listeBoxPersonnes.DataSource = listeNoms;
            }
        }

        public void finRecherchePersonne(IAsyncResult R)
        {
            List<String> listeNoms = new List<String>();

            if (listePersonnes != null)
            {
                foreach (Personne personne in listePersonnes)
                {
                    listeNoms.Add(personne.Nom);
                }
            }

            DispatcherOperation op = System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,        
                (Action)delegate()         
                {
                    lock (chargementPictureBox)
                    {
                        this.chargementPictureBox.Visible = false;
                    }
                    lock (listeBoxPersonnes)
                    {
                        listeNoms.Insert(0, "<Nouvelle personne>");
                        listeBoxPersonnes.DataSource = listeNoms;
                    }
                }
                );
            DispatcherOperationStatus status = op.Status;
            while (status != DispatcherOperationStatus.Completed)
            {
                status = op.Wait(TimeSpan.FromMilliseconds(1000));
                if (status == DispatcherOperationStatus.Aborted)
                {
                    // Alert Someone 
                }
            }
        }

        public delegate void rechercherPersonne_Delegate(string s);


        public void creerPOI()
        {
            this.archimage.creerPOI(this.data);
        }

        public void finCreerPOI(IAsyncResult R)
        {
            DispatcherOperation op = System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)delegate()
                {
                    this.navigationPage.UpdateUI();
                }
                );
            DispatcherOperationStatus status = op.Status;
            while (status != DispatcherOperationStatus.Completed)
            {
                status = op.Wait(TimeSpan.FromMilliseconds(1000));
                if (status == DispatcherOperationStatus.Aborted)
                {
                    // Alert Someone 
                }
            }
        }

        public delegate void creerPOI_Delegate();
    }
}
