using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;

namespace Modele
{
    public class MsvipModele
    {
        private List<VitrineLightModele> _listeVitrines;
        public List<VitrineLightModele> listeVitrines { get { return _listeVitrines; } }//chemin des vitrines
        private VitrineModele vitrineCourante;
        
        private string cheminVitrine;
        public string CheminVitrine
        {
            get { return cheminVitrine; }
            set { cheminVitrine = value; }
        }


        public MsvipModele()
        {
            vitrineCourante = null;
            CheminVitrine = "..\\..\\..\\Vitrines";
            string[] cheminVitrines = Services.instance.getContenu(CheminVitrine, Types.vitrine);
            _listeVitrines = new List<VitrineLightModele>();
            foreach (string c in cheminVitrines)
            {
                _listeVitrines.Add(new VitrineLightModele(c));
            }
        }

        public void lancerVitrine(string vitrine)
        {
            vitrineCourante = new VitrineModele(vitrine);
        }

        public VitrineLightModele ajouterVitrine(String s)
        {
            VitrineLightModele vlm = new VitrineLightModele(s);
            _listeVitrines.Add(vlm);
            return vlm;
        }
    }
}
