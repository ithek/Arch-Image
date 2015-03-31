using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Commun;

namespace Modele
{
    public class VitrineModele
    {
        private String chemin;
        public String Chemin
        {
            get { return chemin; }
        }

        private List<PoiModele> listePois;
        public List<PoiModele> ListePois
        {
            get { return listePois; }
        }

        // Uri de la carte du niveau de zoom 1
        private Uri _uriMapLvl1;
        public Uri uriMapLvl1
        {
            get { return _uriMapLvl1; }
        }
        // String contenant le chemin de la carte de niveau 1
        private string _pathlvl1;
        public string pathlvl1
        {
            get { return _pathlvl1; }
        }


        // Uri de la carte du niveau de zoom 2
        private Uri _uriMapLvl2;
        public Uri uriMapLvl2
        {
            get { return _uriMapLvl2; }
        }
        // String contenant le chemin de la carte de niveau 2
        private string _pathlvl2;
        public string pathlvl2
        {
            get { return _pathlvl2; }
        }


        // Uri de la carte du niveau de zoom 3
        private Uri _uriMapLvl3;
        public Uri uriMapLvl3
        {
            get { return _uriMapLvl3; }
        }
        // String contenant le chemin de la carte de niveau 3
        private string _pathlvl3;
        public string pathlvl3
        {
            get { return _pathlvl3; }
        }


        public VitrineModele(String p)
        {
            chemin = p;
            listePois = new List<PoiModele>();
            String[] carte;
            string adresseCourante;
            string combinaison;

            // Niveau 1
            String nv1 = p + "\\niveau1";
            // On récupère les POI de niveau 1
            String[] chemins_niv1 = Services.instance.getContenu(nv1, Types.poi);
            foreach (String c in chemins_niv1)
            {
                listePois.Add(new PoiModele(c, 1));
            }
            // On recupere l'Uri de la carte de niveau 1
            try
            {
                carte = Directory.GetFiles(nv1, "carte.*");
                adresseCourante = Directory.GetCurrentDirectory();
                combinaison = System.IO.Path.Combine(adresseCourante, carte[0]);
                _uriMapLvl1 = new Uri(System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath));
                _pathlvl1 = carte[0];
            }
            catch (IndexOutOfRangeException e)
            {
                // Pour le cas ou on oublie de mettre une carte dans le dossier, on regarde si il y en a une à la racine
                // si ce n'est pas le cas ...
                carte = Directory.GetFiles(chemin, "carte.*");
                adresseCourante = Directory.GetCurrentDirectory();
                combinaison = System.IO.Path.Combine(adresseCourante, carte[0]);
                _uriMapLvl1 = new Uri(System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath));
                _pathlvl1 = carte[0];
            }


            // Niveau 2
            String nv2 = p + "\\niveau2";
            // On récupère les POI de niveau 2
            String[] chemins_niv2 = Services.instance.getContenu(nv2, Types.poi);
            foreach (String c in chemins_niv2)
            {
                listePois.Add(new PoiModele(c, 2));
            }
            // On recupere l'Uri de la carte de niveau 2
            try
            {
                carte = Directory.GetFiles(nv2, "carte.*");
                adresseCourante = Directory.GetCurrentDirectory();
                combinaison = System.IO.Path.Combine(adresseCourante, carte[0]);
                _uriMapLvl2 = new Uri(System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath));
                _pathlvl2 = carte[0];
            }
            catch (IndexOutOfRangeException e)
            {
                // Pour le cas ou on oublie de mettre une carte dans le dossier, on regarde si il y en a une à la racine
                // si ce n'est pas le cas ...
                carte = Directory.GetFiles(chemin, "carte.*");
                adresseCourante = Directory.GetCurrentDirectory();
                combinaison = System.IO.Path.Combine(adresseCourante, carte[0]);
                _uriMapLvl2 = new Uri(System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath));
                _pathlvl2 = carte[0];
            }

            // Niveau 3
            String nv3 = p + "\\niveau3";
            // On récupère les POI de niveau 3
            String[] chemins_niv3 = Services.instance.getContenu(nv3, Types.poi);
            foreach (String c in chemins_niv3)
            {
                listePois.Add(new PoiModele(c, 3));
            }
            // On recupere l'Uri de la carte de niveau 3
            try
            {
                carte = Directory.GetFiles(nv3, "carte.*");
                adresseCourante = Directory.GetCurrentDirectory();
                combinaison = System.IO.Path.Combine(adresseCourante, carte[0]);
                _uriMapLvl3 = new Uri(System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath));
                _pathlvl3 = carte[0];
            }
            catch (IndexOutOfRangeException e)
            {
                // Pour le cas ou on oublie de mettre une carte dans le dossier, on regarde si il y en a une à la racine
                // si ce n'est pas le cas ...
                carte = Directory.GetFiles(chemin, "carte.*");
                adresseCourante = Directory.GetCurrentDirectory();
                combinaison = System.IO.Path.Combine(adresseCourante, carte[0]);
                _uriMapLvl3 = new Uri(System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath));
                _pathlvl3 = carte[0];
            }
        }
    }
}
