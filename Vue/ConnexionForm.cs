using Modele_Controleur;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




using System.Windows;


namespace Vue
{
    public partial class ConnexionForm : Form
    {
        private ArchImage archimage;
        private MainMenuPage menu;
        public ConnexionForm(ArchImage a, MainMenuPage m)
        {
            InitializeComponent();
            this.archimage = a;
            this.menu = m;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            EtatConnexion etat = this.archimage.MySQLAccess.connexion(identifiantTextBox.Text, passwordTextBox.Text);
            if (etat == EtatConnexion.OK)
            {
                this.archimage.Utilisateur = new Auteur();
                this.Close();
                this.menu.updateUI();
                System.Windows.MessageBox.Show("Vous êtes maintenant connecté et pouvez modifier les documents !");
            }
            else
            {
                System.Windows.MessageBox.Show("Impossible d'effectuer la connexion");
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
