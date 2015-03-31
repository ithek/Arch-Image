using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Input;
using System.Windows;

namespace Prototype1Table
{
    /// <summary>
    /// Filters blob and tags on PixelSense 
    /// </summary>
    public class SurfaceFingerFilter
    {
        //static SurfaceFingerFilter filter = new SurfaceFingerFilter();

        private static bool filter(System.Windows.Input.TouchEventArgs e)
        {
            return !InteractiveSurface.PrimarySurfaceDevice.IsFingerRecognitionSupported || e.TouchDevice.GetIsFingerRecognized();
        }


        public static bool GetFilter(DependencyObject obj)
        {
            return (bool)obj.GetValue(FilterProperty);
        }

        public static void SetFilter(DependencyObject obj, bool value)
        {
            obj.SetValue(FilterProperty, value);
        }

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.RegisterAttached("Filter", typeof(bool), typeof(SurfaceFingerFilter), new UIPropertyMetadata(false, onFilterChanged));

        static void onFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs de)
        {
            var element = d as UIElement;
            if (element == null || !(bool)de.NewValue)
                return;

            element.PreviewTouchDown += (s, e) => e.Handled = !filter(e);
            element.PreviewTouchMove += (s, e) => e.Handled = !filter(e);
            element.PreviewTouchUp += (s, e) => e.Handled = !filter(e);
        }
    }
}
