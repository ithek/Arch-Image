using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Commun;

namespace Modele
{
    public class CouronneModele
    {
        private List<MediaModele> _medias;
        private List<MediaModele> media;
        public List<MediaModele> medias
        {
            get { return _medias; }
            set { _medias = value; }
        }

        public CouronneModele(String chemin)
        {
            _medias = new List<MediaModele>();

            //Recherche des images
            String[] cheminMedias = Services.instance.getContenu(chemin, Types.image);
            foreach (String c in cheminMedias)
            {
                _medias.Add(new ImageModele(c));
            }

            //Recherche des videos
            cheminMedias = Services.instance.getContenu(chemin, Types.video);
            foreach (String c in cheminMedias)
            {
                _medias.Add(new VideoModele(c));
            }

            //Recherche des diaporamas
            cheminMedias = Services.instance.getContenu(chemin, Types.diaporama);
            foreach (String c in cheminMedias)
            {
                _medias.Add(new DiaporamaModele(c));
            }

        }

        public CouronneModele(List<MediaModele> media)
        {
            this.medias = media;
        }
    }
}
