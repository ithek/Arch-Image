using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
namespace Modele
{
    public class MediaModele
    {
        private Types type;
        public Types Type
        {
            get { return type; }
        }

        private String chemin;
        public String Chemin
        {
            get { return chemin; }
            set { chemin = value; }
        }

        private String nom;

        public MediaModele(Types t, String c)
        {
            type = t;
            chemin = c;
            //initialisation du nom
        }

        public String getInfos()
        {
            throw new NotImplementedException();
        }
    }
}
