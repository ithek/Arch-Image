using Modele_Controleur;
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

        private ArchImage Archimage
        {
            get;
            set;
        }

        public NavigationPage(ArchImage a)
        {
            InitializeComponent();

            this.Archimage = a;

            this.UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateBackground();
            UpdateSlider();//TODO use binding instead
        }

        private void UpdateBackground()
        {
            MainWindow mainWindow = ((MainWindow)System.Windows.Application.Current.MainWindow);
            var background = new ImageBrush();
            background.ImageSource = new BitmapImage(new Uri(this.Archimage.DocumentCourant.CheminAcces, UriKind.Relative));
            background.Stretch = Stretch.UniformToFill;
            mainWindow.Background = background;
        }
        private void UpdateSlider()
        {
            DocSlider.Maximum = this.Archimage.GetNbDocInCurrentCategory();
            DocSlider.Value = this.Archimage.DocumentCourant.Position;
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

        private void DocSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.Archimage != null && this.Archimage.DocumentCourant.Position != (int)DocSlider.Value)
            {
                this.Archimage.UtiliserDoc((int)DocSlider.Value);
                this.UpdateUI();
            }
        }
    }
}
