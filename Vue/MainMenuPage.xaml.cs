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
            this.FavorisButton.Visibility = (this.archimage.Utilisateur is Auteur) ? Visibility.Visible : Visibility.Hidden;
        }


        private void Todo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented :(");
        }

        private void MapButton_Click(object sender, RoutedEventArgs e)
        {
            getMainWindow().Content = new MapPage(this.archimage);
        }

        private void RMButton_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.REGISTRE_MATRICULE);
        }

        private void NMDButton_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.NAISSANCE_MARIAGE_DECES);
        }

        private void RecensementButton_Click(object sender, RoutedEventArgs e)
        {
            StartNavigation(Categorie.RECENSEMENT);
        }

        private void StartNavigation(Categorie c)
        {
            MainWindow main = getMainWindow();
            try
            {
                this.archimage.Navigation(c);

                doStuffWithPOI();
                
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
        //TODO !!! Changer le nom une fois que le code POI est fonctionnel et clair. Appeler cette fonction où nécessaire et faire attention aux redondances.
        private void doStuffWithPOI()
        {
            List<POICreationData> listePOIs = archimage.DocumentCourant.POIs;
            ConsultationVM vue = new ConsultationVM(" ");
            PoiModele poiMod;
            List<MediaModele> listMedia = new List<MediaModele>();

            foreach (POICreationData poi in listePOIs)
            {
                poiMod = new PoiModele((int)poi.posX, (int)poi.posY, listMedia, poi.name);
                ConteneurPoiVM cont = new ConteneurPoiVM(poiMod, vue);
                vue.ListePois.Add(cont);
                PoiConsultationVM poiVM = new PoiConsultationVM(cont, poiMod, poi.name);
            }
            Console.WriteLine("Création POI logiques OK");
        }

        private void ConnexionButton_Click(object sender, RoutedEventArgs e)
        {
            new ConnexionForm(this.archimage, this).Show();
        }

        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            new InscriptionForm().Show();
        }

        private void RestoreSessionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow main = getMainWindow();

                loadLastSessionDoc();

                doStuffWithPOI();
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
    }
}
