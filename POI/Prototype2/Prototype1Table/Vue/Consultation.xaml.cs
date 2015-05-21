using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Prototype1Table.Vue
{
    /// <summary>
    /// Logique d'interaction pour Consultation.xaml
    /// </summary>
    public partial class Consultation : UserControl
    {
        public Consultation()
        {
            InitializeComponent();
        }
        
        private void Fenetre_Loaded(object sender, RoutedEventArgs e)
        {
            ((ConsultationVM)(DataContext)).tailleFenetre(this.ActualWidth, this.ActualHeight);
            item.Center = new Point(this.ActualWidth / 2, (this.ActualHeight / 2));
        }

        private void onClickClose(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(System.Windows.Shapes.Path))
            {
                
            }
        }
    }
}
