﻿using Prototype1Table.VueModele;
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
            ZoomManager.matriceTransformation = ImageTransform.Matrix;
            ZoomManager.zoomRatio = 1;
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

            ZoomManager.zoomRatio *= scale.X;

            Matrix m = ImageTransform.Matrix; 
            Matrix baseMatrix = m;
            baseMatrix.Invert();

            // Find center of element and then transform to get current location of center
            FrameworkElement fe = e.Source as FrameworkElement;
            Point center; 
            
            if (scale.X > 1) 
            {
                center = e.ManipulationOrigin;
            }
            else
            {
                center = new Point(fe.ActualWidth / 2, fe.ActualHeight / 2);
            }
            
            
            center = m.Transform(center);

            // Update matrix to reflect translation
            m.Translate(trans.X, trans.Y);
            m.ScaleAt(scale.X, scale.Y, center.X, center.Y);
            
            Matrix transfo = Matrix.Multiply(baseMatrix, m);
            ZoomManager.matriceTransformation = Matrix.Multiply(ZoomManager.matriceTransformation, transfo);

            ImageTransform.Matrix = m;

            updateAllPOISize(scale.X, scale.Y);

            e.Handled = false;
        }

        private void updateAllPOISize(double ratioWidth, double ratioHeight)
        {
            var vm = this.consultationVM;

            for (int i = 0; i < vm.ListePois.Count; i++)
            {
                var poi = vm.ListePois.ElementAt(i).VueCourante;
                if (poi is PoiConsultationVM)
                { 
                    PoiConsultationVM poiClosed = ((PoiConsultationVM)poi);
                    poiClosed.HeightPoi /= ratioHeight;
                    poiClosed.WidthPoi /= ratioWidth;
                }
                else if (poi is CouronneVM)
                {
                    CouronneVM poiOpened = ((CouronneVM)poi);
                    poiOpened.WidthCouronne /= ratioWidth;
                }
            }
        }
    }
}
