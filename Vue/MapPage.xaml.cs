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
            initTouchManagement();
        }

        private void initTouchManagement()
        {
            ScreenTouchManager touch = new ScreenTouchManager(mapGrid);
            this.MapRectangle.ManipulationStarting += touch.Image_ManipulationStarting;
            this.MapRectangle.ManipulationDelta += touch.Image_ManipulationDelta;
            this.DataContext = touch;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).Content = new MainMenuPage(this.arch);
        }
    }
}
