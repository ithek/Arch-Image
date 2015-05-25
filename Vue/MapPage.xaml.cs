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
        private const string SWITCH_BUTTON_TEXT_MANIP = "Utiliser points d'intérêts";
        
        private const string SWITCH_BUTTON_TEXT_POI = "Naviguer";

        private ArchImage arch;
        public MapPage(ArchImage a)
        {
            InitializeComponent();

            UpdateSwitchModeButton();

            this.arch = a;
            Document d = new Document("../../Resources/map.png");
            arch.DocumentCourant = d;

            this.vue = new ConsultationVM(" ");

            initTouchManagement();

            loadCurrentPOI();
        }

        private ConsultationVM vue;

        private ConsultationVM vueScatterView;

        PoiConsultationVM poiVM;

        public delegate void getPOI_Delegate();

        public void getPOI()
        {
            arch.getPOI();
        }

        private void initTouchManagement()
        {
            ScreenTouchManager touch = new ScreenTouchManager(mapGrid, this.vue);
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
                            listMedia.Add(new MediaModele(Types.image, "../../Resources/" + doc.CheminAcces, "../../Resources/"+cMiniature));
                        }

                        poiMod = new PoiModele((int)poi.posX, (int)poi.posY, listMedia, poi.Id, poi.Nom);

                        ConteneurPoiVM cont = new ConteneurPoiVM(poiMod, vue);
                        cont.fermeturePoi(); //Pour afficher les noms sur les POI
                        vue.ListePois.Add(cont);
                        poiVM = new PoiConsultationVM(cont, poiMod, poi.Nom);
                        finGetPOIDocOuvert();
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

        public void finGetPOIDocOuvert()
        {
            Console.WriteLine("================");
            vueScatterView = new ConsultationVM(" ");
            foreach (POICreationData poiDocParent in arch.SewelisAccess.getPOI(arch.DocumentCourant))
            {
                List<Document> listeDoc = arch.SewelisAccess.getListDocs(poiDocParent);
                PoiModele poiMod = null;

                CanvasMedias.DataContext = vue;

                Console.WriteLine("DATA CONTEXT OK");
                foreach (Document docPOI in listeDoc) {

                    Console.WriteLine(docPOI.CheminAcces);
                    List<POICreationData> listePOIs = arch.SewelisAccess.getPOI(docPOI);

                    foreach (POICreationData poi in listePOIs)
                    {
                        Console.WriteLine(poi.Personne);
                        //On initialise les Documents présents dans les caroussels
                        List<MediaModele> listMedia = new List<MediaModele>();
                        List<Document> listDoc = arch.SewelisAccess.getListDocs(poi);
                        foreach (Document doc in listDoc)
                        {
                            String cMiniature = findMiniaturePOIonDoc(doc.Categorie.ToString());
                            listMedia.Add(new MediaModele(Types.image, "../../Resources/" + doc.CheminAcces, cMiniature));
                        }

                        poiMod = new PoiModele((int)poi.posX, (int)poi.posY, listMedia, poi.Id, poi.Nom);

                        ConteneurPoiVM cont = new ConteneurPoiVM(poiMod, vueScatterView);
                        cont.fermeturePoi(); //Pour afficher les noms sur les POI
                        vueScatterView.ListePois.Add(cont);
                        PoiConsultationVM poiVMScatterView = new PoiConsultationVM(cont, poiMod, poi.Nom);
                    }
                }
            }
            poiVM.setConteneurScatterView(vueScatterView);
        }

        private string findMiniature(string path)
        {
            Regex myRegex1 = new System.Text.RegularExpressions.Regex(@"/[A-Z]*_[0-9]*\.JPG$");
            String nomFichier = System.IO.Path.GetFileName(path);
            String pathAModif =  myRegex1.Replace(path, "/Miniatures/"+nomFichier);

            Regex myRegex2 = new System.Text.RegularExpressions.Regex(@"/");
            return myRegex2.Replace(pathAModif, "\\");
        }

        private string findMiniaturePOIonDoc(string categorie)  //TODO clean
        {
            switch (categorie)
            {
                case "REGISTRE_MATRICULE":
                    return "../../Resources/Miniatures/RM.png";
                case "NAISSANCE_MARIAGE_DECES":
                    return "../../Resources/Miniatures/NMD.png";
                case "TABLE_REGISTRE_MATRICULE":
                    return "../../Resources/Miniatures/TRM.png";
                case "RECENSEMENT":
                    return "../../Resources/Miniatures/REC.png";
                case "TABLES_DECENNALES":
                    return "../../Resources/Miniatures/TD.png";
                case "TSA":
                    return "../../Resources/Miniatures/TSA.png";
                default:
                    return "../../Resources/Miniatures/TSA.png";
            }
        }

        private void loadCurrentPOI()
        {
            vue = new ConsultationVM(" ");
            vueScatterView = new ConsultationVM("");
            
            PoisItemControl.DataContext = vue;
            ScatterMedias.DataContext = vue;
            MapRectangle.DataContext = vue;
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

        private void UpdateSwitchModeButton()
        {
            this.SwitchModeButton.Title = MapRectangle.IsManipulationEnabled ?
                SWITCH_BUTTON_TEXT_MANIP : SWITCH_BUTTON_TEXT_POI;
        }

        private void SwitchModeButton_Click(object sender, RoutedEventArgs e)
        {
            MapRectangle.IsManipulationEnabled = !MapRectangle.IsManipulationEnabled;
            this.UpdateSwitchModeButton();
        }

        //Récupère le type des documents
        private int categoryName(String path)
        {
            Regex rMat = new System.Text.RegularExpressions.Regex("REGISTRES_MILITAIRES");
            Regex nmd = new System.Text.RegularExpressions.Regex("NMD");
            Regex tRMat = new System.Text.RegularExpressions.Regex("TABLES_RMM");
            Regex recensement = new System.Text.RegularExpressions.Regex("RECENSEMENT");
            Regex tDecen = new System.Text.RegularExpressions.Regex("TABLES_DECENNALES");
            Regex tsa = new System.Text.RegularExpressions.Regex("TSA");

            if (rMat.IsMatch(path))
                return 0;
            if (nmd.IsMatch(path))
                return 1;
            if (tRMat.IsMatch(path))
                return 2;
            if (recensement.IsMatch(path))
                return 3;
            if (tDecen.IsMatch(path))
                return 4;
            if (tsa.IsMatch(path))
                return 5;
            else
                return -1;
        }

        //Remplace le caractère "/" par "\" pour les comparaisons de chemin d'accès
        private string changeChar(string chaine)
        {
            Regex myRegex = new System.Text.RegularExpressions.Regex(@"/");
            return myRegex.Replace(chaine, "\\"); //renvoi la chaine modifiée
        }

        private void newBackgroundButton_Click(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            if (e.OriginalSource.GetType() == typeof(System.Windows.Controls.MediaElement))
            {
                if (this.vue.mediasOuverts.Count == 0)
                {
                    // TODO was here before for unclear reasons, remove completely if useless : e.Handled = false;
                    return;
                }

                MediaVM media = this.vueScatterView.mediasOuverts.ElementAt(0);

                //Pour récupérer les types des documents.
                String chemin = media.cheminMedia.OriginalString;
                int categorie = categoryName(chemin);

                //Pour récupérer le numéro de la page
                String cheminLivre = System.IO.Path.GetDirectoryName(chemin);

                //On remplace les / par des \
                chemin = changeChar(chemin);
                Console.WriteLine(chemin);

                int noPage = System.IO.Directory.EnumerateFiles(cheminLivre).ToList().IndexOf(chemin) + 1;

                //Pour récupérer le numéro du livre
                String type = System.IO.Path.GetDirectoryName(cheminLivre);
                cheminLivre = changeChar(cheminLivre);

                int noLivre = System.IO.Directory.EnumerateDirectories(type).ToList().IndexOf(cheminLivre) + 1;

                //Créer un nouveau document en rappelant Sewelis pour connaitre les POI sur le doc.
                Document d = new Document((Categorie)categorie, chemin, noLivre, noPage);
                d.POIs = this.arch.SewelisAccess.getPOI(d);
                this.arch.Navigation(d);
                Application.Current.MainWindow.Content = new NavigationPage(arch);

            }
        }
    }
}
