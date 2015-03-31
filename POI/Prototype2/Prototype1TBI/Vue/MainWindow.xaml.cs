using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prototype1TBI.VueModeles;

namespace Prototype1TBI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((VueModele)(DataContext)).tailleFenetre((int)this.ActualWidth, (int)this.ActualHeight);
            scatterView.Center = new Point(this.ActualWidth / 2, (this.ActualHeight / 2));
        }

        public void openPicture(object sender, EventArgs e)
        {
            Process.Start("D:/Prototype1/Prototype1TBI/Resources/insa.jpg");
        }

        public void openPDF(object sender, EventArgs e)
        {
            Process.Start("D:/Prototype1/Prototype1TBI/Resources/rapport_conception.pdf");
        }
    }
}
