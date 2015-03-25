﻿using Modele_Controleur;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


namespace Vue
{
    /// <summary>
    /// Logique d'interaction pour NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {
        private bool drawingRectangleForNewPOI;
        private Point rectanglePOIStart;
        
        private ArchImage Archimage
        {
            get;
            set;
        }

        private SwipeManager Swipe
        {
            get;
            set;
        }

        public NavigationPage(ArchImage a)
        {
            InitializeComponent();

            this.Archimage = a;
            this.drawingRectangleForNewPOI = false;
            this.Swipe = new SwipeManager(a, this);

            this.TouchDown += new EventHandler<TouchEventArgs>(Swipe.TouchDown);
            this.TouchMove += new EventHandler<TouchEventArgs>(Swipe.TouchMove); 

            this.UpdateUI();
        }

        public void UpdateUI()
        {
            UpdateBackground();
            UpdateSlider();//TODO use binding instead
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            this.NextCategoryName.Text = GetDisplayableName(Archimage.getPrev(Archimage.DocumentCourant.Categorie));
            this.PrevCategoryName.Text = GetDisplayableName(Archimage.GetNext(Archimage.DocumentCourant.Categorie));        
        }

        private void UpdateBackground()
        {
            MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);
            var background = new ImageBrush();
            background.ImageSource = new BitmapImage(new Uri(this.Archimage.DocumentCourant.CheminAcces, UriKind.Relative));
            background.Stretch = Stretch.Uniform;
            mainWindow.Background = background;
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
            this.Archimage.DocumentSuivant();
            this.UpdateUI();
        }

        private void PreviousDocButton_Click(object sender, RoutedEventArgs e)
        {
            this.Archimage.DocumentPrecedent();
            this.UpdateUI();
        }

        private void PreviousCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Archimage.CategoriePrecedente();
            this.UpdateUI();
        }

        private void NextCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Archimage.CategorieSuivante();
            this.UpdateUI();
        }

        private async void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            bool isHold = await this.TouchHold(TimeSpan.FromSeconds(1));
            if (isHold)
            {
                this.startRectangle(e);
            }
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

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.drawingRectangleForNewPOI)
            {
                this.endRectangle(e);
            }

            this.Swipe.HasRealeasedTouch();
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

            new POICreationForm(left, top, Archimage).Show();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
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
	}
}
