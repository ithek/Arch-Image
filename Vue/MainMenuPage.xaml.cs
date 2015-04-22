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
        }


        private void Todo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented :(");
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
            MainWindow main = ((MainWindow)System.Windows.Application.Current.MainWindow);
            try
            {
                this.archimage.Navigation(c);

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

        private void ConnexionButton_Click(object sender, RoutedEventArgs e)
        {
            Todo(sender, e);
        }

        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            Todo(sender, e);
        }
    }
}
