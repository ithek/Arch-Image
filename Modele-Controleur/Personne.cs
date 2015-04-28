using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modele_Controleur
{
    public class Personne
    {
        public String Id
        {
            get;
            set;
        }

        public String Nom
        {
            get;
            set;
        }

        public String Prenom
        {
            get;
            set;
        }

        public String Initiale
        {
            get;
            set;
        }

        public String DateNaissance
        {
            get;
            set;
        }

        public Personne(String nom, String prenom, String ddn, String id)
        {
            Nom = nom;
            Prenom = prenom;
            DateNaissance = ddn;
            Id = id;
        }
    }
}
