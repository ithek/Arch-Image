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

        public POICreationForm(double x, double y, ArchImage arch)
        {
            InitializeComponent();
            this.data = new POICreationData(x, y);
            
            this.archimage = arch;
            this.archimage.SewelisAccess.chargerListePersonnes();
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
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Personne personne = listePersonnes[this.listeBoxPersonnes.SelectedIndex];

            this.nameLabel.Text = "Nom : " + personne.Nom;
            this.prenomLabel.Text = "Prénom : " + personne.Prenom;
            this.dateNaissanceLabel.Text = "Date de naissance : " + personne.DateNaissance;

            data.IdPersonne = personne.Id;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            this.nameLabel.Text = "Nom : ";
            this.prenomLabel.Text = "Prénom : ";
            this.dateNaissanceLabel.Text = "Date de naissance : ";
            data.IdPersonne = null;

            List<String> listeNoms = new List<String>();
            listePersonnes = this.archimage.SewelisAccess.recherchePersonnes(this.nameTextBox.Text);

            if (listePersonnes != null)
            {
                foreach (Personne personne in listePersonnes)
                {

                    listeNoms.Add(personne.Nom);
                }
                listeBoxPersonnes.DataSource = listeNoms;
            }
        }
    }
}
