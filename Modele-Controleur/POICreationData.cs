using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modele_Controleur
{
    public class POICreationData
    {
        public double posX
        {
            get;
            set;
        }
        
        public double posY
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public POICreationData(double x, double y)
        {
            this.posX = x;
            this.posY = y;
        }
    }
}
