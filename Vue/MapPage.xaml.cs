using Modele_Controleur;
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
using Prototype1Table.VueModele;
using Modele;
using Commun;
using Prototype1Table.Vue;
using System.Text.RegularExpressions;
using System.Windows.Threading;

namespace Vue
{
    /// <summary>
    /// Logique d'interaction pour MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        private ArchImage arch;
        public MapPage(ArchImage a)
        {
            InitializeComponent();

            this.arch = a;
            Document d = new Document("../../Resources/map.png");
            arch.DocumentCourant = d;

            initTouchManagement();

            loadCurrentPOI();
        }

        private ConsultationVM vue;

        public delegate void getPOI_Delegate();

        public void getPOI()
        {
            arch.getPOI();
        }

        private void initTouchManagement()
        {
            ScreenTouchManager touch = new ScreenTouchManager(mapGrid);
            this.MapRectangle.ManipulationStarting += touch.Image_ManipulationStarting;
            this.MapRectangle.ManipulationDelta += touch.Image_ManipulationDelta;
            this.DataContext = touch;
        }

        public void finGetPOI(IAsyncResult R)
        {
            List<POICreationData> listePOIs = arch.DocumentCourant.POIs;
            vue = new ConsultationVM(" ");
            PoiModele poiMod = null;

            //Binding
            DispatcherOperation op = System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)delegate()
                {
                    PoisItemControl.DataContext = vue;
                    ScatterMedias.DataContext = vue;
                    MapRectangle.DataContext = vue;
                    
                    foreach (POICreationData poi in listePOIs)
                    {
                        //On initialise les Documents présents dans les caroussels
                        List<MediaModele> listMedia = new List<MediaModele>();
                        List<Document> listDoc = arch.SewelisAccess.getListDocs(poi);
                        foreach (Document doc in listDoc)
                        {
                            String cMiniature = findMiniature(doc.CheminAcces);
                            Console.WriteLine(cMiniature);
                            listMedia.Add(new MediaModele(Types.image, "../../Resources/" + doc.CheminAcces, "../../Resources/"+cMiniature));
                        }

                        poiMod = new PoiModele((int)poi.posX, (int)poi.posY, listMedia, poi.Id, poi.Nom);

                        ConteneurPoiVM cont = new ConteneurPoiVM(poiMod, vue);
                        cont.fermeturePoi(); //Pour afficher les noms sur les POI
                        vue.ListePois.Add(cont);
                        PoiConsultationVM poiVM = new PoiConsultationVM(cont, poiMod, poi.Nom);
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

        private string findMiniature(string path)
        {
            Regex myRegex1 = new System.Text.RegularExpressions.Regex(@"/[A-Z]*_[0-9]*\.JPG$");
            String nomFichier = System.IO.Path.GetFileName(path);
            String pathAModif =  myRegex1.Replace(path, "/Miniatures/"+nomFichier);

            Regex myRegex2 = new System.Text.RegularExpressions.Regex(@"/");
            return myRegex2.Replace(pathAModif, "\\");
        }

        private void loadCurrentPOI()
        {
            vue = new ConsultationVM(" ");
            PoisItemControl.DataContext = vue;
            ScatterMedias.DataContext = vue;
            //Initialisation
            getPOI_Delegate d = null;
            d = new getPOI_Delegate(getPOI);

            IAsyncResult R = null;
            R = d.BeginInvoke(new AsyncCallback(finGetPOI), null); //invoking the method          
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).Content = new MainMenuPage(this.arch);
        }

        private void mapGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(System.Windows.Shapes.Path))
            {
                
                //vue.fermerMedia(i);
            }
        }
    }
}
