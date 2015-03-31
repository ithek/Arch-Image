using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Modele
{
    public class VitrineLightModele
    {
        public string Chemin { get; private set; }
        public Uri Apercu { get; private set; }

        public VitrineLightModele(string c)
        {
            Chemin = c;
            String[] resultat = Directory.GetFiles(c, "apercu.*");

            if (resultat.Length > 0)
            {
                string adresseCourante = Directory.GetCurrentDirectory();
                string combinaison = System.IO.Path.Combine(adresseCourante, resultat[0]);
                Apercu =new Uri(combinaison);
            }
            else
            {
                //Mise en place de l'aperçu par defaut
                Apercu = new Uri(@"/Prototype1Table;component/Resources/vitrineParDefaut.png", UriKind.RelativeOrAbsolute);
            }
        }
        
    }
}
