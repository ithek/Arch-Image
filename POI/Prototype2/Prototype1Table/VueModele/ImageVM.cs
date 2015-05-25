using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using System.Windows;
using System.Collections.ObjectModel;

namespace Prototype1Table.VueModele
{
    public class ImageVM : MediaVM
    {
        public ImageVM(MediaModele m, Point p, double o, ConsultationVM c) : base(m, p, o,c) 
        {
            
        }

        public ImageVM(MediaModele m, Point p, double o, ConsultationVM c, ConsultationVM vueScatterView) : base(m, p, o, c, vueScatterView)
        {

        }
    }
}
