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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArchImage archimage
        {
            get;
            set;
        }

        public MainWindow()
        {
            InitializeComponent();

            this.archimage = new ArchImage();

            //FIXME mettre le contenu de la page principale (bouton map, RM, connexion, favoris, etc) dans une Page et la charger plutot qu'en dur.

        }


        private void Todo(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not yet implemented :(");
        }

        private void RMButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = ((MainWindow)System.Windows.Application.Current.MainWindow);
            this.archimage.Navigation(Categorie.REGISTRE_MATRICULE);
            main.Content = new NavigationPage(this.archimage);
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
