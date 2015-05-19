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

namespace Vue
{
    public partial class InscriptionForm : Form
    {
        private ArchImage arch;
        public InscriptionForm(ArchImage a)
        {
            this.arch = a;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            EtatInscription etat = this.arch.MySQLAccess.inscription(identifiantTextBox.Text, passwordTextBox.Text, emailTextBox.Text);
            if (etat == EtatInscription.OK)
            {
                MessageBox.Show("Inscription effectuée");
                this.Close();
            }
            else
            {
                MessageBox.Show("Impossible de créer le compte (identifiant déjà choisi ?)");
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
