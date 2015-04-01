using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using Modele;
using System.Text.RegularExpressions;
namespace Prototype1Table.VueModele
{
    public class VitrineVM : VueModeleBase
    {
        private VitrineLightModele modele;
        public string Chemin { get { return modele.Chemin; } }
        public string Nom { get; private set;}
        public Uri Apercu { get { return modele.Apercu; } }

        public VitrineVM(VitrineLightModele m)
        {
            modele = m;
            string s = modele.Chemin;
            Nom = "_erreur";
            
            Match match = Regex.Match(modele.Chemin, @"([^\\]+).vitrine$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                Nom = match.Groups[1].Value;
            }
        }
    }
}
