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

        public ScreenTouchManager(Grid theGrid)
        {
            this.ImageTransform = new MatrixTransform();
            this.ManipContainer = theGrid;
        }

        public void Image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            // Ask for manipulations to be reported relative to the grid
            e.ManipulationContainer = this.ManipContainer;
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

            e.Handled = true;
        }
    }
}
