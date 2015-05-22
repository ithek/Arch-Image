using Prototype1Table.VueModele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Vue
{
    class ScreenTouchManager
    {
        public MatrixTransform ImageTransform
        {
            get;
            set;
        }

        private Grid ManipContainer;

        private ConsultationVM consultationVM;

        public ScreenTouchManager(Grid theGrid, ConsultationVM cVM)
        {
            this.ImageTransform = new MatrixTransform();
            this.ManipContainer = theGrid;
            this.consultationVM = cVM;
        }

        public void Image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            // Ask for manipulations to be reported relative to the grid
            e.ManipulationContainer = this.ManipContainer;
            e.Handled = false;
        }

        public void Image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            ManipulationDelta md = e.DeltaManipulation;
            Vector trans = md.Translation;
            Vector scale = md.Scale;

            Matrix m = ImageTransform.Matrix;

            // Find center of element and then transform to get current location of center
            FrameworkElement fe = e.Source as FrameworkElement;
            Point center = new Point(fe.ActualWidth / 2, fe.ActualHeight / 2);
            center = m.Transform(center);

            // Update matrix to reflect translation
            m.Translate(trans.X, trans.Y);
            m.ScaleAt(scale.X, scale.Y, center.X, center.Y);
            ImageTransform.Matrix = m;

            updateAllPOISize(scale.X, scale.Y);

            e.Handled = false;
        }

        private void updateAllPOISize(double ratioWidth, double ratioHeight)
        {
            var vm = this.consultationVM;

            
            string TODO = "";//TODO
            if (vm == null) MessageBox.Show("vm, ");
            if (vm.ListePois == null) MessageBox.Show("vm.ListePois, ");
            if (vm.ListePois.Count == null) MessageBox.Show( "vm.ListePois.Count, ");
            if (vm.ListePois.ElementAt(0) == null) MessageBox.Show("vm.ListePois.ElementAt(0), ");
            if (vm.ListePois.ElementAt(0).VueCourante == null)MessageBox.Show("vm.ListePois.ElementAt(0).VueCourante, ");
            if (((PoiConsultationVM)vm.ListePois.ElementAt(0).VueCourante) == null) MessageBox.Show("((PoiConsultationVM)vm.ListePois.ElementAt(0).VueCourante), ");
            
         

            for (int i = 0; i < vm.ListePois.Count; i++)
            {
                PoiConsultationVM poi = ((PoiConsultationVM)vm.ListePois.ElementAt(i).VueCourante);
                poi.HeightPoi /= ratioHeight;
                poi.WidthPoi /= ratioWidth;   
            }
        }
    }
}
