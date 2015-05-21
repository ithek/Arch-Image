﻿using System;
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
        }

        private void parseUserInput()
        {
            //this.data.IdPersonne = this.nameTextBox.Text;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.parseUserInput();   

            if (data.Personne != null)
                this.archimage.creerPOI(this.data);
            else if(this.nameTextBox.Text != null)
            {
                string nom = this.nameTextBox.Text;
                string prenom = this.prenomTextBox.Text;
                string initiale = this.initialeTextBox.Text;
                string ddn = this.dateNaissanceTextBox.Text;
                Personne p = new Personne(nom, prenom, initiale, ddn);
                
                this.data.Personne = this.archimage.SewelisAccess.ajouterPersonne(p);
                this.archimage.creerPOI(this.data);
            }

            this.Close();
            this.navigationPage.UpdateUI();
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
            if (this.archimage.SewelisAccess.CanSearch)
            {
                this.nameTextBox.Text = "";
                this.prenomTextBox.Text = "";
                this.initialeTextBox.Text = "";
                this.dateNaissanceTextBox.Text = "";
                this.listeBoxPersonnes.DataSource = null;
                data.Personne = null;

                this.chargementPictureBox.Visible = true;

                rechercherPersonne_Delegate d = null;
                d = new rechercherPersonne_Delegate(rechercherPersonne);
 
                IAsyncResult R = null;
                R = d.BeginInvoke(this.nameSearchTextBox.Text, new AsyncCallback(finRecherchePersonne), null); //invoking the method
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
                    this.chargementPictureBox.Visible = false;
                    listeNoms.Insert(0, "<Nouvelle personne>");
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
