using Modele_Controleur;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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


namespace Vue
{
    /// <summary>
    /// Logique d'interaction pour NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {
        /**
         * The string to display on the switch-mode button when it's possible to move/zoom the image, but impossible to create POI.
         */
        private const string SWITCH_BUTTON_TEXT_MANIP = "Indiquer zones";

        /**
         * The string to display on the switch-mode button when it's possible to create POI, but impossible to move/zoom the image.
         */
        private const string SWITCH_BUTTON_TEXT_POI = "Naviguer";

        private bool drawingRectangleForNewPOI;
        private Point rectanglePOIStart;
        
        private ArchImage Archimage
        {
            get;
            set;
        }
        
        public NavigationPage(ArchImage a)
        {
            InitializeComponent();

            this.initTouchManagement();
            
            this.Archimage = a;

            this.drawingRectangleForNewPOI = false;

            this.UpdateUI();
        }

        private void updateAuthorPrivileges()
        {
            bool modeAuteur = this.Archimage.Utilisateur is Auteur;

            this.SwitchModeButton.Visibility = modeAuteur ? Visibility.Visible : Visibility.Hidden;
            if (modeAuteur)
            {
                //TODO est-ce qu'on ne cumulerait pas les handlers (1 fois par update) avec += ? s'il était possible de se déconnecter, faudrait-il faire autant de fois les -= pour perdre à nouveau les privilèges ?
                this.MouseMove += this.OnMouseMove;
                this.MouseRightButtonDown += this.OnMouseRightButtonDown;
                this.MouseRightButtonUp += this.OnMouseRightButtonUp;
            }
            else
            {
                this.MouseMove -= this.OnMouseMove;
                this.MouseRightButtonDown -= this.OnMouseRightButtonDown;
                this.MouseRightButtonUp -= this.OnMouseRightButtonUp;
            }
        }

        private void initTouchManagement()
        {
            ScreenTouchManager touchManager = new ScreenTouchManager(this.theGrid);
            this.RectangleContainingBackgroundImage.ManipulationStarting += touchManager.Image_ManipulationStarting;
            this.RectangleContainingBackgroundImage.ManipulationDelta += touchManager.Image_ManipulationDelta;
            this.DataContext = touchManager;
        }

        public void UpdateUI()
        {
            UpdateBackground();
            UpdateSlider();//TODO use binding instead
            UpdateButtons();
            doStuffWithPOI();
            this.updateAuthorPrivileges();
        }

        private void doStuffWithPOI()
        {
            List<POICreationData> listePOIs = Archimage.DocumentCourant.POIs;
            ConsultationVM vue = new ConsultationVM(" ");
            PoiModele poiMod = null;
            List<MediaModele> listMedia = new List<MediaModele>();

            PoisItemControl.DataContext = vue;
            ScatterMedias.DataContext = vue;

            foreach (POICreationData poi in listePOIs)
            {
                List<Document> listDoc = Archimage.SewelisAccess.getListDocs(poi);
                foreach (Document doc in listDoc)
                {
                    listMedia.Add(new MediaModele(Types.image, "C:/Users/Cedric/Source/Repos/Arch-Image2/POI/Prototype2/Vitrines/Insa.vitrine/niveau3/Salle_Mac.poi/PicToShare.diaporama/affichage.jpg")); // TO DO : rajouter les véritables chemin d'accès.
                }

                poiMod = new PoiModele((int)poi.posX, (int)poi.posY, listMedia, poi.IdPersonne, poi.IdPersonne); // TO DO : a modifier lorsque les noms seront implémentés
                
                ConteneurPoiVM cont = new ConteneurPoiVM(poiMod, vue);
                cont.fermeturePoi(); //Pour afficher les noms sur les POI
                vue.ListePois.Add(cont);
                PoiConsultationVM poiVM = new PoiConsultationVM(cont, poiMod, poi.IdPersonne);
            }
        }

        private void UpdateButtons()
        {
            this.NextCategoryName.Text = GetDisplayableName(Archimage.GetNext(Archimage.DocumentCourant.Categorie));
            this.PrevCategoryName.Text = GetDisplayableName(Archimage.GetPrev(Archimage.DocumentCourant.Categorie));
            this.UpdateSwitchModeButton();
        }

        private void UpdateSwitchModeButton()
        {
            this.SwitchModeButton.Content = RectangleContainingBackgroundImage.IsManipulationEnabled ?
                SWITCH_BUTTON_TEXT_MANIP : SWITCH_BUTTON_TEXT_POI;
        }

        private void UpdateBackground()
        {
            ImageSource source = new BitmapImage(new Uri(this.Archimage.DocumentCourant.CheminAcces, UriKind.Relative));
            BackgroundImage.ImageSource = source;
        }
        private void UpdateSlider()
        {
            DocSlider.Value = this.Archimage.DocumentCourant.Position; 
            DocSlider.Maximum = this.Archimage.GetNbDocInCurrentBook();
            SliderInfoTextBlock.Text = Archimage.DocumentCourant.Position + "/" + Archimage.GetNbDocInCurrentBook();
        }

        /**
         * Returns the name of the Categorie c that will be displayed to the user
         */
        private string GetDisplayableName(Categorie c)
        {
            string res;

            switch (c)
            {
                case Categorie.NAISSANCE_MARIAGE_DECES :
                    res = "NMD";
                    break;

                case Categorie.RECENSEMENT:
                    res = "Recensement";
                    break;

                case Categorie.REGISTRE_MATRICULE :
                    res = "Registres matricules";
                    break;

                case Categorie.TABLE_REGISTRE_MATRICULE :
                    res = "Tables RM";
                    break;

                case Categorie.TABLES_DECENNALES :
                    res = "Décès";
                    break;

                case Categorie.TSA :
                    res = "TSA";
                    break;

                default :
                    res = "Unknown category";
                    break;
            }
            return res; 
        }

        private void NextDocButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Archimage.DocumentSuivant();
                this.UpdateUI();
            }
            catch (FileNotFoundException ex)
            {
                ExceptionManager.EmptyBook(ex);
            }
        }

        private void PreviousDocButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Archimage.DocumentPrecedent();
                this.UpdateUI();
            }
            catch (FileNotFoundException ex)
            {
                ExceptionManager.EmptyBook(ex);
            }
        }

        private void PreviousCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Archimage.CategoriePrecedente();
            }
            catch (DirectoryNotFoundException ex)
            {
                ExceptionManager.DirectoryNotFound(ex);
            }
            catch (FileNotFoundException ex)
            {
                ExceptionManager.EmptyBook(ex);
            }
            this.UpdateUI();
        }

        private void NextCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Archimage.CategorieSuivante();
            }
            catch (DirectoryNotFoundException ex)
            {
                ExceptionManager.DirectoryNotFound(ex);
            }
            catch (FileNotFoundException ex)
            {
                ExceptionManager.EmptyBook(ex);
            }
            this.UpdateUI();
        }

        private void startRectangle(MouseButtonEventArgs e)
        {
            // Capture and track the mouse.
            this.drawingRectangleForNewPOI = true;
            rectanglePOIStart = e.GetPosition(theGrid);
            theGrid.CaptureMouse();

            // Initial placement of the drag selection box.         
            Canvas.SetLeft(newPOISelectionRectangle, rectanglePOIStart.X);
            Canvas.SetTop(newPOISelectionRectangle, rectanglePOIStart.Y);
            newPOISelectionRectangle.Width = 0;
            newPOISelectionRectangle.Height = 0;

            // Make the drag selection box visible.
            newPOISelectionRectangle.Visibility = Visibility.Visible;
        }

        private void endRectangle(MouseButtonEventArgs e)
        {
            this.drawingRectangleForNewPOI = false;
            theGrid.ReleaseMouseCapture();

            // Hide the drag selection box.
            newPOISelectionRectangle.Visibility = Visibility.Collapsed;

            Point rectanglePOIEnd = e.GetPosition(theGrid);

            //TODO formulaire, puis POI entre rectanglePOIStart et rectanglePOIEnd 

            //TODO corriger (peut-être) les valeurs
            double left = ((rectanglePOIStart.X + rectanglePOIEnd.X)/2);
            double top =  ((rectanglePOIStart.Y + rectanglePOIEnd.Y)/2);

            new POICreationForm(left, top, Archimage, this).Show();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
           e.Handled = true;
           if (drawingRectangleForNewPOI)
            {
                // When the mouse is held down, reposition the drag selection box.

                Point mousePos = e.GetPosition(theGrid);

                if (rectanglePOIStart.X < mousePos.X)
                {
                    Canvas.SetLeft(newPOISelectionRectangle, rectanglePOIStart.X);
                    newPOISelectionRectangle.Width = mousePos.X - rectanglePOIStart.X;
                }
                else
                {
                    Canvas.SetLeft(newPOISelectionRectangle, mousePos.X);
                    newPOISelectionRectangle.Width = rectanglePOIStart.X - mousePos.X;
                }

                if (rectanglePOIStart.Y < mousePos.Y)
                {
                    Canvas.SetTop(newPOISelectionRectangle, rectanglePOIStart.Y);
                    newPOISelectionRectangle.Height = mousePos.Y - rectanglePOIStart.Y;
                }
                else
                {
                    Canvas.SetTop(newPOISelectionRectangle, mousePos.Y);
                    newPOISelectionRectangle.Height = rectanglePOIStart.Y - mousePos.Y;
                }
            }
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.endRectangle(e);
        }

        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.startRectangle(e);
        }

        private void DocSlider_UserModif()
        {
            this.Archimage.UtiliserDoc((int)DocSlider.Value);
            this.UpdateUI();
        }

        private void DocSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            DocSlider_UserModif();
        }

        private void DocSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            DocSlider_UserModif();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = ((MainWindow)System.Windows.Application.Current.MainWindow);
            main.Content = new MainMenuPage(this.Archimage);
        }

        private void SwitchModeButton_Click(object sender, RoutedEventArgs e)
        {
            RectangleContainingBackgroundImage.IsManipulationEnabled = ! RectangleContainingBackgroundImage.IsManipulationEnabled;
            this.UpdateSwitchModeButton();
        }
	}
}
