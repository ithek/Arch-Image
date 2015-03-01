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

        public POICreationForm(double x, double y, ArchImage arch)
        {
            InitializeComponent();
            this.data = new POICreationData(x, y);
            
            this.archimage = arch;
        }

        private void parseUserInput()
        {
            this.data.name = this.nameTextBox.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.parseUserInput();   

            this.Hide();
            this.archimage.creerPOI(this.data);
            //TODO mettre à jour la vue ?
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
