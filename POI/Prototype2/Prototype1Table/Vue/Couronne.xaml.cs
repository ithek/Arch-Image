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


using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace Prototype1Table.Vue
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class Couronne : Grid
    {
        public Couronne()
        {
            InitializeComponent();
        }

        public void apercu_TapGesture(object sender, MouseEventArgs e)
        {
            //Point pRelatif = e.TouchDevice.GetTouchPoint(this).Position;
            Point pRelatif = e.GetPosition(ScatterV);
            Point pAbs = e.GetPosition((System.Windows.IInputElement)this.Parent);
            //Point pRelatif = e.TouchDevice.GetTouchPoint(ScatterV).Position;
            //Point pAbs = e.TouchDevice.GetTouchPoint((System.Windows.IInputElement)this.Parent).Position;
            ((VueModele.CouronneVM)(this.DataContext)).tapCouronneAction2(pRelatif, pAbs);
        }

        public void apercu_TapGesture(object sender, TouchEventArgs e)
        {
            //Point pRelatif = e.TouchDevice.GetTouchPoint(this).Position;
            //Point pRelatif = e.GetPosition(ScatterV);
            //Point pAbs = e.GetPosition((System.Windows.IInputElement)this.Parent);
            Point pRelatif = e.TouchDevice.GetTouchPoint(ScatterV).Position;
            Point pAbs = e.TouchDevice.GetTouchPoint((System.Windows.IInputElement)this.Parent).Position;
            ((VueModele.CouronneVM)(this.DataContext)).tapCouronneAction(pRelatif, pAbs);
        }

        private void ScatterView_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            //Point pAbs = e.GetPosition((System.Windows.IInputElement)this.Parent);
            //((VueModele.CouronneVM)(this.DataContext)).upCouronneAction(pAbs, 90.0);
            Point pAbs = e.TouchDevice.GetTouchPoint((System.Windows.IInputElement)this.Parent).Position;
            double orientation = e.TouchDevice.GetOrientation((System.Windows.IInputElement)this.Parent);
            ((VueModele.CouronneVM)(this.DataContext)).upCouronneAction(pAbs, orientation);
        }
    }
}
