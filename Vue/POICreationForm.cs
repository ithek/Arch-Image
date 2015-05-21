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
        private ArchImage Archimage;
        private NavigationPage navigationPage;

        public POICreationForm(double x, double y, ArchImage arch)
        {
            InitializeComponent();
            this.data = new POICreationData(x, y);
            
            this.archimage = arch;
            this.archimage.SewelisAccess.chargerListePersonnes();
        }

        public POICreationForm(double left, double top, ArchImage archimage, NavigationPage navigationPage)
        {
            InitializeComponent();
            this.data = new POICreationData(left, top);
            this.left = left;
            this.top = top;
            this.archimage = archimage;
            this.archimage.SewelisAccess.chargerListePersonnes();
            this.navigationPage = navigationPage;
        }

        private void parseUserInput()
        {
            //this.data.IdPersonne = this.nameTextBox.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.parseUserInput();   

            //TODO : Gérer quand la personne n'existe pas
            if (data.IdPersonne != null)
                this.archimage.creerPOI(this.data);

            //TODO mettre à jour la vue ?
            this.Close();
            this.navigationPage.UpdateUI();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listeBoxPersonnes.DataSource != null)
            {
                Personne personne = listePersonnes[this.listeBoxPersonnes.SelectedIndex];

                this.nameLabel.Text = "Nom : " + personne.Nom;
                this.prenomLabel.Text = "Prénom : " + personne.Prenom;
                this.dateNaissanceLabel.Text = "Date de naissance : " + personne.DateNaissance;

                data.IdPersonne = personne.Id;
                data.Nom = personne.Nom;
            }
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.archimage.SewelisAccess.CanSearch)
            {
                this.nameLabel.Text = "Nom : ";
                this.prenomLabel.Text = "Prénom : ";
                this.dateNaissanceLabel.Text = "Date de naissance : ";
                this.listeBoxPersonnes.DataSource = null;
                data.IdPersonne = null;
                data.Nom = null;

                this.pictureBox1.Visible = true;

                rechercherPersonne_Delegate d = null;
                d = new rechercherPersonne_Delegate(rechercherPersonne);
 
                IAsyncResult R = null;
                R = d.BeginInvoke(this.nameTextBox.Text, new AsyncCallback(finRecherchePersonne), null); //invoking the method
            }
        }

        public void rechercherPersonne(string nom)
        {
            listePersonnes = this.archimage.SewelisAccess.recherchePersonnes(this.nameTextBox.Text);

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
                //listeBoxPersonnes.DataSource = listeNoms;
            }

            DispatcherOperation op = System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,        
                (Action)delegate()         
                {
                    this.pictureBox1.Visible = false;
                    listeBoxPersonnes.DataSource = listeNoms;
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
    }
}
