using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using Commun;

namespace Prototype1Table.VueModele
{
    public class MsvipVM : VueModeleBase
    {
        private MsvipModele modele;
        private VitrineVM vitrineCourante;
        public List<VitrineLightModele> liste_vitrines { get { return modele.listeVitrines; } }

        public MsvipVM()
        {
            modele = new MsvipModele();
        }
    }
}
