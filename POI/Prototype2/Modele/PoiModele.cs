using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Modele
{
    public class PoiModele
    {
        private Point position;
        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        private String nom;
        public String Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public String chemin { get; set; }

        private CouronneModele couronne;
        public CouronneModele Couronne
        {
            get { return couronne; }
        }
        private int niveau;
        public int Niveau
        {
            get { return niveau; }
            set { niveau = value; }
        }

        public PoiModele(String c, int niveau)
        {
            chemin = c;
            Niveau = niveau;
            position = Services.instance.getPosition(c);
            couronne = new CouronneModele(c);
        }

        public PoiModele(String c,int niveau, int x, int y)
        {
            Services.instance.creerDossier(c);
            Services.instance.creerFichier(c + "\\position.txt");
            Services.instance.setPosition(new Point(x, y),1, c);
            chemin = c;
            Niveau = niveau;
            position = new Point(x, y); ;
            couronne = new CouronneModele(c);
        }

        public PoiModele(int x, int y,List<MediaModele> media, string c,String name)
        {
            chemin = c;
            Niveau = 1;
            position = new Point(x, y); ;
            couronne = new CouronneModele(media);
            nom = name;
        }

        public void enregistrerPosition(double ratio)
        {
            Services.instance.setPosition(position,ratio, chemin);
        }
    }
}
