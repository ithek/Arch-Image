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

        public String AnneeRegistreMatricule
        {
            get;
            set;
        }

        public String NumeroRegistreMatricule
        {
            get;
            set;
        }

        public String DateNaissance
        {
            get;
            set;
        }

        public String LieuNaissance
        {
            get;
            set;
        }

        public String DateDeces
        {
            get;
            set;
        }

        public String LieuDeces
        {
            get;
            set;
        }

        public String DateMariage
        {
            get;
            set;
        }

        public String LieuMariage
        {
            get;
            set;
        }

        public String NomConjoint
        {
            get;
            set;
        }

        public String PrenomConjoint
        {
            get;
            set;
        }

        public String PrenomPere
        {
            get;
            set;
        }

        public String NomMere
        {
            get;
            set;
        }

        public String PrenomMere
        {
            get;
            set;
        }

        public Personne(String nom, String prenom, String initiale, String anneeRM, String numeroRM, String ddn, String lieuNaissance, String dateDeces, String lieuDeces, String dateMariage, String lieuMariage, String nomConjoint, String prenomConjoint, String prenomPere, String nomMere, String prenomMere, String id)
        {
            Nom = nom;
            Initiale = initiale;
            Prenom = prenom;
            AnneeRegistreMatricule = anneeRM;
            NumeroRegistreMatricule = numeroRM;
            DateNaissance = ddn;
            LieuNaissance = lieuNaissance;
            DateDeces = dateDeces;
            LieuDeces = lieuDeces;
            DateMariage = dateMariage;
            LieuMariage = lieuMariage;
            NomConjoint = nomConjoint;
            PrenomConjoint = prenomConjoint;
            PrenomPere = prenomPere;
            NomMere = nomMere;
            PrenomMere = prenomMere;
            Id = id;
        }

        public Personne(String nom, String prenom, String initiale, String anneeRM, String numeroRM, String ddn, String lieuNaissance, String dateDeces, String lieuDeces, String dateMariage, String lieuMariage, String nomConjoint, String prenomConjoint, String prenomPere, String nomMere, String prenomMere)
        {
            Nom = nom;
            Initiale = initiale;
            Prenom = prenom;
            AnneeRegistreMatricule = anneeRM;
            NumeroRegistreMatricule = numeroRM;
            DateNaissance = ddn;
            LieuNaissance = lieuNaissance;
            DateDeces = dateDeces;
            LieuDeces = lieuDeces;
            DateMariage = dateMariage;
            LieuMariage = lieuMariage;
            NomConjoint = nomConjoint;
            PrenomConjoint = prenomConjoint;
            PrenomPere = prenomPere;
            NomMere = nomMere;
            PrenomMere = prenomMere;
        }
    }
}
