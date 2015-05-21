using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modele_Controleur
{
    [Serializable()]
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

        public Personne(String nom, String prenom, String initiale, String ddn, String id)
        {
            Nom = nom;
            Initiale = initiale;
            Prenom = prenom;
            DateNaissance = ddn;
            Id = id;
        }

        public Personne(String nom, String prenom, String initiale, String ddn)
        {
            Nom = nom;
            Initiale = initiale;
            Prenom = prenom;
            DateNaissance = ddn;
        }
    }
}
