using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modele_Controleur
{
    [Serializable()]
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

        public string Id
        {
            get;
            set;
        }

        public string IdPersonne
        {
            get;
            set;
        }

        public POICreationData(double x, double y)
        {
            this.posX = x;
            this.posY = y;
        }

        public POICreationData(double x, double y, string poiId)
        {
            this.posX = x;
            this.posY = y;
            this.Id = poiId;
        }

        public POICreationData(double x, double y, string poiId, string nom)
        {
            this.posX = x;
            this.posY = y;
            this.Id = poiId;
            this.IdPersonne = nom;
        }
    }
}
