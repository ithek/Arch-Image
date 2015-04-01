using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using System.Windows;

namespace Prototype1Table.VueModele
{
    public class ImageVM : MediaVM
    {
        public ImageVM(MediaModele m, Point p, double o,ConsultationVM c) : base(m, p, o,c) {}
    }
}
