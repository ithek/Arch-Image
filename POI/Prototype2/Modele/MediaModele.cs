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

        //On rajoute le chemin vers les miniatures
        private String cheminMiniature;
        public String CheminMiniature
        {
            get { return cheminMiniature; }
            set { cheminMiniature = value; }
        }

        private String nom;

        public MediaModele(Types t, String c)
        {
            type = t;
            chemin = c;
            //initialisation du nom
        }

        public MediaModele(Types t, String c, String cMiniature)
        {
            type = t;
            chemin = c;
            this.CheminMiniature = cMiniature;
        }

        public String getInfos()
        {
            throw new NotImplementedException();
        }
    }
}
