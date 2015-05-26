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
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Collections;
using System.Timers;


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
        private const string SWITCH_BUTTON_TEXT_MANIP = "Utiliser points d'intérêts";

        /**
         * The string to display on the switch-mode button when it's possible to create POI, but impossible to move/zoom the image.
         */
        private const string SWITCH_BUTTON_TEXT_POI = "Naviguer";

        private bool drawingRectangleForNewPOI;
        private Point rectanglePOIStart;

        private ConsultationVM vue;

        private List<Personne> listePersonnes;
        private POICreationData poi;
        private Timer timer;
        private int tootalsecs = 1;

        private List<TextBox> listeTextBox;
        private List<Label> listeLabel;

        private ArchImage Archimage
        {
            get;
            set;
        }
        
        public NavigationPage(ArchImage a)
        {
            InitializeComponent();

            this.initForms();

            this.Archimage = a;

            this.drawingRectangleForNewPOI = false;

            this.vue = new ConsultationVM(" ");
            this.initTouchManagement();            

            this.UpdateUI();
        }

        private void initForms() 
        {
            this.listeTextBox = new List<TextBox>();
            this.listeTextBox.Add(nomTextBox);
            this.listeTextBox.Add(prenomTextBox);
            this.listeTextBox.Add(initialeTextBox);
            this.listeTextBox.Add(anneeRMTextBox);
            this.listeTextBox.Add(numeroRMTextBox);
            this.listeTextBox.Add(dateNaissanceTextBox);
            this.listeTextBox.Add(lieuNaissanceTextBox);
            this.listeTextBox.Add(dateDecesTextBox);
            this.listeTextBox.Add(lieuDecesTextBox);
            this.listeTextBox.Add(dateMariageTextBox);
            this.listeTextBox.Add(lieuMariageTextBox);
            this.listeTextBox.Add(nomConjointTextBox);
            this.listeTextBox.Add(prenomConjointTextBox);
            this.listeTextBox.Add(prenomPereTextBox);
            this.listeTextBox.Add(nomMereTextBox);
            this.listeTextBox.Add(prenomMereTextBox);

            this.listeLabel = new List<Label>();
            this.listeLabel.Add(nomLabel);
            this.listeLabel.Add(prenomLabel);
            this.listeLabel.Add(initialeLabel);
            this.listeLabel.Add(anneeRMLabel);
            this.listeLabel.Add(numeroRMLabel);
            this.listeLabel.Add(dateNaissanceLabel);
            this.listeLabel.Add(lieuNaissanceLabel);
            this.listeLabel.Add(dateDecesLabel);
            this.listeLabel.Add(lieuDecesLabel);
            this.listeLabel.Add(dateMariageLabel);
            this.listeLabel.Add(lieuMariageLabel);
            this.listeLabel.Add(nomConjointLabel);
            this.listeLabel.Add(prenomConjointLabel);
            this.listeLabel.Add(prenomPereLabel);
            this.listeLabel.Add(nomMereLabel);
            this.listeLabel.Add(prenomMereLabel);
        }

        private void updateAuthorPrivileges()
        {
            bool modeAuteur = this.Archimage.Utilisateur is Auteur;

            var mouseRightButtonUPListener = this;
            var mouseRightButtonDOWNListener = this.RectangleContainingBackgroundImage;
            var mouseMoveListener = this;

            if (modeAuteur)
            {
                //TODO est-ce qu'on ne cumulerait pas les handlers (1 fois par update) avec += ? s'il était possible de se déconnecter, faudrait-il faire autant de fois les -= pour perdre à nouveau les privilèges ?
                mouseMoveListener.MouseMove += this.OnMouseMove;
                mouseRightButtonDOWNListener.MouseRightButtonDown += this.OnMouseRightButtonDown;
                mouseRightButtonUPListener.MouseRightButtonUp += this.OnMouseRightButtonUp;
            }
            else
            {
                mouseMoveListener.MouseMove -= this.OnMouseMove;
                mouseRightButtonDOWNListener.MouseRightButtonDown -= this.OnMouseRightButtonDown;
                mouseRightButtonUPListener.MouseRightButtonUp -= this.OnMouseRightButtonUp;
            }
        }

        private void initTouchManagement()
        {
            var dc = RectangleContainingBackgroundImage.DataContext;
            if (dc is ScreenTouchManager) // remove previous eventual listener
            {
                ScreenTouchManager oldListener = (ScreenTouchManager) dc;
                this.RectangleContainingBackgroundImage.ManipulationStarting -= oldListener.Image_ManipulationStarting;
                this.RectangleContainingBackgroundImage.ManipulationDelta -= oldListener.Image_ManipulationDelta;
            }

            ScreenTouchManager touchManager = new ScreenTouchManager(this.theGrid, this.vue);
            this.RectangleContainingBackgroundImage.ManipulationStarting += touchManager.Image_ManipulationStarting;
            this.RectangleContainingBackgroundImage.ManipulationDelta += touchManager.Image_ManipulationDelta;
            this.DataContext = touchManager;
        }

        public void UpdateUI()
        {
            UpdateBackground();
            UpdateSlider();//TODO use binding instead
            UpdateButtons();
            loadCurrentPOI();
            this.updateAuthorPrivileges();
        }

        public delegate void getPOI_Delegate();

        public void getPOI()
        {
            Archimage.getPOI();
        }

        public void finGetPOI(IAsyncResult R)
        {
            List<POICreationData> listePOIs = Archimage.DocumentCourant.POIs;
            PoiModele poiMod = null;

            //Binding
            DispatcherOperation op = System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)delegate()
                {
                    PoisItemControl.DataContext = vue;
                    ScatterMedias.DataContext = vue;
                    foreach (POICreationData poi in listePOIs)
                    {
                        //On initialise les Documents présents dans les caroussels
                        List<MediaModele> listMedia = new List<MediaModele>();
                        List<Document> listDoc = Archimage.SewelisAccess.getListDocs(poi);
                        foreach (Document doc in listDoc)
                        {
                            String cMiniature = findMiniature(doc.Categorie.ToString());
                            listMedia.Add(new MediaModele(Types.image, "../../Resources/" + doc.CheminAcces, cMiniature));
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

        private void loadCurrentPOI()
        {
            vue.ListePois.Clear();
            vue.mediasOuverts.Clear();
            PoisItemControl.DataContext = vue;
            ScatterMedias.DataContext = vue;
            //Initialisation
            getPOI_Delegate d = null;
            d = new getPOI_Delegate(getPOI);

            IAsyncResult R = null;
            R = d.BeginInvoke(new AsyncCallback(finGetPOI), null); //invoking the method          
        }

        private string findMiniature(string categorie)  //TODO clean
        {
            switch (categorie)
            {
                case "REGISTRE_MATRICULE" : 
                    return "../../Resources/Miniatures/RM.png";
                case "NAISSANCE_MARIAGE_DECES" :
                    return "../../Resources/Miniatures/NMD.png";
                case "TABLE_REGISTRE_MATRICULE" :
                    return "../../Resources/Miniatures/TRM.png";
                case "RECENSEMENT" :
                    return "../../Resources/Miniatures/REC.png";
                case "TABLES_DECENNALES" :
                    return "../../Resources/Miniatures/TD.png";
                case "TSA" :
                    return "../../Resources/Miniatures/TSA.png";
                default :
                    return "../../Resources/Miniatures/TSA.png";
            }
        }

        private void UpdateButtons()
        {
            this.NextCategoryButton.Title = GetDisplayableName(Archimage.GetNext(Archimage.DocumentCourant.Categorie));
            this.PreviousCategoryButton.Title = GetDisplayableName(Archimage.GetPrev(Archimage.DocumentCourant.Categorie));
            this.UpdateSwitchModeButton();
        }

        private void UpdateSwitchModeButton()
        {
            this.SwitchModeButton.Title = RectangleContainingBackgroundImage.IsManipulationEnabled ?
                SWITCH_BUTTON_TEXT_MANIP : SWITCH_BUTTON_TEXT_POI;
        }

        private void UpdateBackground()
        {
            ImageSource source = new BitmapImage(new Uri(this.Archimage.DocumentCourant.CheminAcces, UriKind.Relative));
            
            var img = new ImageBrush(source);
            img.Stretch = Stretch.Uniform;
            RectangleContainingBackgroundImage.Background = img;

            initTouchManagement();//TODO decommenté = Zoom réinitialisé à chaque changement de doc, mais qui rendent les POI minuscules sur zoom
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
                    res = "REC";
                    break;

                case Categorie.REGISTRE_MATRICULE :
                    res = "TM";
                    break;

                case Categorie.TABLE_REGISTRE_MATRICULE :
                    res = "TRM";
                    break;

                case Categorie.TABLES_DECENNALES :
                    res = "TD";
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
            if (!this.drawingRectangleForNewPOI)
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
        }

        private void endRectangle(MouseButtonEventArgs e)
        {
            if (this.drawingRectangleForNewPOI)
            {
                this.drawingRectangleForNewPOI = false;
                theGrid.ReleaseMouseCapture();

                // Hide the drag selection box.
                newPOISelectionRectangle.Visibility = Visibility.Collapsed;

                Point rectanglePOIEnd = e.GetPosition(theGrid);


                //TODO corriger (peut-être) les valeurs
                var poiPos = calculatePOIPos(rectanglePOIStart, rectanglePOIEnd);

                this.Archimage.SewelisAccess.chargerListePersonnes();
                List<String> listeNoms = new List<String>();
                listeNoms.Insert(0, "<Nouvelle personne>");
                listBoxNoms.ItemsSource = listeNoms;
                poi = new POICreationData(poiPos.X , poiPos.Y);
                annotationLabel.Content = "";
                flyoutAnnotation.IsOpen = true;
            }
        }

        /**
         * Returns the center of the provided selection rectangle (0;0 being the top-left of the whole thing), considering current zoom and position
         */
        private Point calculatePOIPos(Point rectanglePOIStart, Point rectanglePOIEnd)
        {
            double relativeX = ((rectanglePOIStart.X + rectanglePOIEnd.X) / 2);
            double relativeY = ((rectanglePOIStart.Y + rectanglePOIEnd.Y) / 2);

            return new Point(ZoomManager.posX + (relativeX/ZoomManager.zoomRatio),
                             ZoomManager.posY + (relativeY/ZoomManager.zoomRatio));
            
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
            if(nmd.IsMatch(path))
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

        //Fonction de remplacement d'image de fond lors d'un clic sur un apercu du caroussel
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
                   
                MediaVM media = this.vue.mediasOuverts.ElementAt(0);

                //Pour récupérer les types des documents.
                String chemin = media.cheminMedia.OriginalString;
                int categorie = categoryName(chemin);                

                //Pour récupérer le numéro de la page
                String cheminLivre = System.IO.Path.GetDirectoryName(chemin);

                //On remplace les / par des \
                chemin = changeChar(chemin);

                int noPage = System.IO.Directory.EnumerateFiles(cheminLivre).ToList().IndexOf(chemin)+1;

                //Pour récupérer le numéro du livre
                String type = System.IO.Path.GetDirectoryName(cheminLivre);
                cheminLivre = changeChar(cheminLivre);

                int noLivre = System.IO.Directory.EnumerateDirectories(type).ToList().IndexOf(cheminLivre)+1;

                //Créer un nouveau document en rappelant Sewelis pour connaitre les POI sur le doc.
                Document d = new Document((Categorie)categorie, chemin, noLivre, noPage);
                d.POIs = this.Archimage.SewelisAccess.getPOI(d);
                this.Archimage.Navigation(d);

                this.UpdateUI();
            }
        }

        private void okAnnotationButton_Click(object sender, RoutedEventArgs e)
        {
            if (poi.Personne != null)
            {
                creerPOI_Delegate d = new creerPOI_Delegate(creerPOI);

                IAsyncResult R = null;
                R = d.BeginInvoke(new AsyncCallback(finCreerPOI), null); //invoking the method
                flyoutAnnotation.IsOpen = false;
            }
            else if(this.nomTextBox.Text.Equals(""))
            {
                annotationLabel.Content = "Veuillez indiquer au moins le nom.";
            }
            else if (!this.nomTextBox.Text.Equals(""))
            {
                String nom, prenom, initiale, anneeRM, numeroRM, ddn, lieuNaissance, dateDeces, lieuDeces, dateMariage, lieuMariage, nomConjoint, prenomConjoint, prenomPere, nomMere, prenomMere;
                nom = this.nomTextBox.Text;
                prenom = this.prenomTextBox.Text;
                initiale = this.initialeTextBox.Text;
                anneeRM = this.anneeRMTextBox.Text;
                numeroRM = this.numeroRMTextBox.Text;
                ddn = this.dateNaissanceTextBox.Text;
                lieuNaissance = this.lieuNaissanceTextBox.Text;
                dateDeces = this.dateDecesTextBox.Text;
                lieuDeces = this.lieuDecesTextBox.Text;
                dateMariage = this.dateMariageTextBox.Text;
                lieuMariage = this.lieuMariageTextBox.Text;
                nomConjoint = this.nomConjointTextBox.Text;
                prenomConjoint = this.prenomConjointTextBox.Text;
                prenomPere = this.prenomPereTextBox.Text;
                nomMere = this.nomMereTextBox.Text;
                prenomMere = this.prenomMereTextBox.Text;

                Personne p = new Personne(nom, prenom, initiale, anneeRM, numeroRM, ddn, lieuNaissance, dateDeces, lieuDeces, dateMariage, lieuMariage, nomConjoint, prenomConjoint, prenomPere, nomMere, prenomMere);

                this.poi.Personne = this.Archimage.SewelisAccess.ajouterPersonne(p);

                creerPOI_Delegate d = new creerPOI_Delegate(creerPOI);

                IAsyncResult R = null;
                R = d.BeginInvoke(new AsyncCallback(finCreerPOI), null); //invoking the method

                flyoutAnnotation.IsOpen = false;
            }
        }

        private void annulerAnnotationButton_Click(object sender, RoutedEventArgs e)
        {
            flyoutAnnotation.IsOpen = false;
        }

        private void listBoxNoms_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (listBoxNoms.ItemsSource != null && listBoxNoms.SelectedIndex > 0)
            {
                switchOnLabel();
                Personne personne = listePersonnes[listBoxNoms.SelectedIndex - 1];
                this.nomLabel.Content = personne.Nom;
                this.prenomLabel.Content = personne.Prenom;
                this.initialeLabel.Content = personne.Initiale;
                this.anneeRMLabel.Content = personne.AnneeRegistreMatricule;
                this.numeroRMLabel.Content = personne.NumeroRegistreMatricule;
                this.dateNaissanceLabel.Content = personne.DateNaissance;
                this.lieuNaissanceLabel.Content = personne.LieuNaissance;
                this.dateDecesLabel.Content = personne.DateDeces;
                this.lieuDecesLabel.Content = personne.LieuDeces;
                this.dateMariageLabel.Content = personne.DateMariage ;
                this.lieuMariageLabel.Content = personne.LieuMariage;
                this.nomConjointLabel.Content = personne.NomConjoint;
                this.prenomConjointLabel.Content = personne.PrenomConjoint;
                this.prenomPereLabel.Content = personne.PrenomPere;
                this.nomMereLabel.Content = personne.NomMere;
                this.prenomMereLabel.Content = personne.PrenomMere;

                poi.Personne = personne;
            }
            else if (listBoxNoms.SelectedIndex == 0)
            {
                switchOnTextBox();
            }
        }

        private void nomRechercheTextBox_TextChanged(object sender, EventArgs e)
        {
            tootalsecs = 1;
            countdownTimer();

            listeTextBox.ForEach(delegate(TextBox t)
            {
                t.Text = "";
            });

            this.listBoxNoms.ItemsSource = null;
            poi.Personne = null;

            if (!this.nomRechercheTextBox.Text.Equals(""))
            {
                lock (progressRing)
                {
                    progressRing.IsActive = true;
                }
            }
            else
            {
                if (timer != null)
                    timer.Stop();

                List<String> listeNoms = new List<String>();
                listeNoms.Insert(0, "<Nouvelle personne>");
                lock (listBoxNoms)
                {
                    listBoxNoms.ItemsSource = listeNoms;
                }
            }
        }

        public void rechercherPersonne(string nom)
        {
            listePersonnes = this.Archimage.SewelisAccess.recherchePersonnes(nom);
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
                    lock (progressRing)
                    {
                        progressRing.IsActive = false;
                    }
                    lock (listBoxNoms)
                    {
                        listeNoms.Insert(0, "<Nouvelle personne>");
                        listBoxNoms.ItemsSource = listeNoms;
                        HashSet<String> set = new HashSet<String>((IEnumerable<String>)listBoxNoms.ItemsSource);
                        listBoxNoms.ItemsSource = set.ToList();
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

        public delegate void rechercherPersonne_Delegate(string s);


        public void creerPOI()
        {
            this.Archimage.creerPOI(this.poi);
        }

        public void finCreerPOI(IAsyncResult R)
        {
            DispatcherOperation op = System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)delegate()
                {
                    this.UpdateUI();
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

        public delegate void creerPOI_Delegate();

        public void switchOnLabel()
        {
            if(nomLabel.Visibility == Visibility.Hidden)
            {
                listeTextBox.ForEach(delegate(TextBox t)
                {
                    t.Visibility = Visibility.Collapsed;
                });listeLabel.ForEach(delegate(Label l)
                {
                    l.Visibility = Visibility.Visible;
                });
            }
        }
        
        public void switchOnTextBox()
        {
            if (nomTextBox.Visibility == Visibility.Collapsed)
            {   
                listeLabel.ForEach(delegate(Label l) {
    		        l.Visibility = Visibility.Hidden;
    	        });
                listeTextBox.ForEach(delegate(TextBox t) {
                    t.Visibility = Visibility.Visible;
    	        });
            }
        }

        private void countdownTimer()
        {
            if (timer != null)
                timer.Stop();

            var startTime = DateTime.Now;

            timer = new Timer() { Interval = 1000 };
            timer.Elapsed += (obj, args) =>
            {
            if (tootalsecs==0)
            {
                timer.Stop();
                DispatcherOperation op = System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (Action)delegate()
                {
                    if (!this.nomRechercheTextBox.Text.Equals(""))
                    {
                        rechercherPersonne_Delegate d = new rechercherPersonne_Delegate(rechercherPersonne);

                        IAsyncResult R = null;
                        R = d.BeginInvoke(this.nomRechercheTextBox.Text, new AsyncCallback(finRecherchePersonne), null); //invoking the method
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
            else
            {
                tootalsecs--;
            }
        };
                timer.Start();
        }      
	}
}
