using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modele;
using System.Windows;
using System.Windows.Input;
using Commun;
using System.Windows.Controls;
using System.IO;

namespace Prototype1Tablette.VueModeles
{
    class DiaporamaVM : MediaVM
    {
        public DiaporamaVM(Uri u, double numDiapo)
            : base(u)
        {
            //Recuperation des URIs des elements du diaporama
            Diapos = new List<Uri>();
            String[] chemins = Services.instance.getContenu(u.OriginalString, Types.image);
            foreach (String c in chemins)
            {
                diapos.Add(new Uri(obtenirAdresseAbsolue(c), UriKind.Absolute));
            }

            //Initialisation
            numero = (int)numDiapo;
            SuivantCommande = new RelaiCommande(new Action(diapoSuivante));
            PrecedentCommande = new RelaiCommande(new Action(diapoPrecedente));
            DiapoCourante = diapos[numero];
        }

        private List<Uri> diapos;
        public List<Uri> Diapos
        {
            get { return diapos; }
            set { diapos = value; }
        }

        private Uri diapoCourante;
        public Uri DiapoCourante
        {
            get { return diapoCourante; }
            set
            {
                diapoCourante = value;
                RaisePropertyChanged("DiapoCourante");
            }
        }

        private int numero;

        private string obtenirAdresseAbsolue(string adresseRelative)
        {
            //Accès à un chemin relatif eu chemin actuel
            string adresseCourante = Directory.GetCurrentDirectory();
            string combinaison = System.IO.Path.Combine(adresseCourante, adresseRelative);
            return System.IO.Path.GetFullPath((new Uri(combinaison)).LocalPath);
        }

        public void diapoSuivante()
        {
            if (++numero == Diapos.Count) numero = 0;
            DiapoCourante = Diapos[numero];
        }

        public void diapoPrecedente()
        {
            if (--numero == -1) numero = Diapos.Count - 1;
            DiapoCourante = Diapos[numero];
        }

        public void allerA(int numDiapo)
        {
            numero = numDiapo;
            DiapoCourante = Diapos[numero];
        }

        private ICommand suivantCommande;
        public ICommand SuivantCommande
        {
            get
            {
                return suivantCommande;
            }
            set
            {
                suivantCommande = value;
            }
        }

        private ICommand precedentCommande;
        public ICommand PrecedentCommande
        {
            get
            {
                return precedentCommande;
            }
            set
            {
                precedentCommande = value;
            }
        }
    }
}
