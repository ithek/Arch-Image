using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Modele_Controleur;
using Prototype1Table.VueModele;
using Modele;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Vue
{
    /// <summary>
    /// Logique d'interaction pour MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {        
        private ArchImage archimage
        {
            get;
            set;
        }

        public MainMenuPage(ArchImage arch)
        {
            InitializeComponent();

            this.archimage = arch;

            this.updateUI();
        }

        public void updateUI()
        {
            this.updateButtonVisibility();
        }

        public void updateButtonVisibility()
        {
            //TODO le premier couillon qui voudrait plutôt faire ça par Binding comme j'en avais l'intention au début est le bienvenu.
            var visibility = (this.archimage.Utilisateur is Auteur) ? Visibility.Hidden : Visibility.Visible;
            connexionTile.Visibility = visibility;
            inscriptionTile.Visibility = visibility;
        }

        private void MapTile_Click(object sender, RoutedEventArgs e)
        {
            getMainWindow().Content = new MapPage(this.archimage);
        }

        private void RMTile_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.REGISTRE_MATRICULE);
        }

        private void TablesRMTile_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.REGISTRE_MATRICULE);
        }

        private void NMDTile_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.NAISSANCE_MARIAGE_DECES);
        }

        private void TSATile_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.TSA);
        }

        private void RecensementTile_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.RECENSEMENT);
        }

        private void TablesDecenalesTile_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.TABLES_DECENNALES);
        }

        private void StartNavigation(Categorie c)
        {
            MainWindow main = getMainWindow();
            try
            {
                this.archimage.Navigation(c);
                
                main.Content = new NavigationPage(this.archimage);
            }
            catch (DirectoryNotFoundException ex)
            {
                ExceptionManager.DirectoryNotFound(ex);
            }
            catch (FileNotFoundException ex)
            {
                ExceptionManager.EmptyBook(ex);
            }
        }
        
        private void ConnexionTile_Click(object sender, RoutedEventArgs e)
        {
            flyoutInscription.IsOpen = false;
            flyoutConnexion.IsOpen = true;   
        }

        private void InscriptionTile_Click(object sender, RoutedEventArgs e)
        {
            flyoutConnexion.IsOpen = false;
            flyoutInscription.IsOpen = true;
        }

        private void RestoreSessionTile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow main = getMainWindow();

                loadLastSessionDoc();

                main.Content = new NavigationPage(this.archimage);
            }
            catch (FileNotFoundException ex)
            {
                ExceptionManager.SaveNotFound(ex);
            }
        }

        private void loadLastSessionDoc()
        {
            //Opens file and deserializes the object from it.
            Stream stream = File.Open(ArchImage.PATH_TO_SESSION_SAVE, FileMode.Open);
            
            BinaryFormatter formatter = new BinaryFormatter();

            Document docSessionPrecedente = (Document)formatter.Deserialize(stream);
            stream.Close();

            this.archimage.Navigation(docSessionPrecedente);  
        }

        private MainWindow getMainWindow() {
            return ((MainWindow)System.Windows.Application.Current.MainWindow);
        }

        private async void okConnexionButton_Click(object sender, EventArgs e)
        {
            if(identifiantConnexionTextBox.Text.Equals(""))
            {
                connexionLabel.Content = "Veuillez indiquer votre nom d'utilisateur.";
            }
            else if(passwordConnexionTextBox.Password.Equals(""))
            {
                connexionLabel.Content = "Veuillez indiquer votre mot de passe.";
            }
            else
            {
                EtatConnexion etat = this.archimage.MySQLAccess.connexion(identifiantConnexionTextBox.Text, passwordConnexionTextBox.Password);
                if (etat == EtatConnexion.OK)
                {
                    this.archimage.Utilisateur = new Auteur();
                    
                    
                    flyoutConnexion.IsOpen = false;
                   
                    await this.getMainWindow().ShowMessageAsync("Succès", "Vous êtes maintenant connecté et pouvez annoter les documents !");
                }
                else
                {
                    await this.getMainWindow().ShowMessageAsync("Erreur", "Impossible d'effectuer la connexion.");
                }
            }

            updateUI();
        }

        private void annulerConnexionButton_Click(object sender, EventArgs e)
        {
            flyoutConnexion.IsOpen = false;
        }

        private async void okInscriptionButton_Click(object sender, EventArgs e)
        {
            if (identifiantInscriptionTextBox.Text.Equals(""))
            {
                inscriptionLabel.Content = "Veuillez indiquer votre nom d'utilisateur.";
            }
            else if (passwordInscriptionTextBox.Password.Equals(""))
            {
                inscriptionLabel.Content = "Veuillez indiquer votre mot de passe.";
            }
            else if (emailTextBox.Text.Equals(""))
            {
                inscriptionLabel.Content = "Veuillez indiquer votre email.";
            }
            else
            {
                EtatInscription etat = this.archimage.MySQLAccess.inscription(identifiantInscriptionTextBox.Text, passwordInscriptionTextBox.Password, emailTextBox.Text);
                if (etat == EtatInscription.OK)
                {
                    flyoutInscription.IsOpen = false;
                    await this.getMainWindow().ShowMessageAsync("Succès", "Vous êtes maintenant inscrit !");
                }
                else
                {
                    await this.getMainWindow().ShowMessageAsync("Erreur", "Impossible de créer le compte (identifiant déjà choisi ?).");
                }
            }
        }

        private void annulerInscriptionButton_Click(object sender, EventArgs e)
        {
            flyoutInscription.IsOpen = false;
        }
    }
}
