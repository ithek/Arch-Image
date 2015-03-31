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
using Microsoft.Surface.Presentation.Input;
using Prototype1Table.VueModele;

namespace Prototype1Table.Vue
{
    /// <summary>
    /// Logique d'interaction pour Creation.xaml
    /// </summary>
    public partial class Creation : UserControl
    {
        List<PoiCreation> listPoi;

        public Creation()
        {
            listPoi = new List<PoiCreation>();
            InitializeComponent();
        }

        private void Fenetre_Loaded(object sender, RoutedEventArgs e)
        {
            ((CreationVM)(DataContext)).tailleFenetre(this.ActualWidth, this.ActualHeight);
            item.Center = new Point(this.ActualWidth / 2, (this.ActualHeight / 2));
            Console.WriteLine("Consultation : height = " + ActualHeight + " ; width = " + ActualWidth);
        }

        private void PoiCreation_TouchDown(object sender, TouchEventArgs e)
        {
            PoiCreation poi = sender as PoiCreation;
            
            if (e.TouchDevice.GetIsFingerRecognized() && e.TouchDevice.Captured != poi && !listPoi.Contains(poi))
            {
                listPoi.Add(poi);
                e.TouchDevice.Capture(poi);
                e.Handled = true;
            }

        }

        private void PoiCreation_TouchUp(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice.GetIsFingerRecognized())
            {
                PoiCreation poi = sender as PoiCreation;
                listPoi.Remove(poi);
                poi.ReleaseTouchCapture(e.TouchDevice);
                poi.ReleaseAllTouchCaptures();
            }
            ((CreationVM)(DataContext)).sauvegarderPosition();
            e.Handled = false;
        }
    }
}
