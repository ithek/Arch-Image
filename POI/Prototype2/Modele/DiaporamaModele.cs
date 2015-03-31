using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;

namespace Modele
{
    public class DiaporamaModele : MediaModele
    {
        private List<ImageModele> _images;
        public List<ImageModele> images
        {
            get { return _images; }
        }

        public DiaporamaModele(String c) : base(Types.diaporama,c) 
        {
            _images = new List<ImageModele>();

            //Recherche des images
            String[] cheminMedias = Services.instance.getContenu(c, Types.image);
            foreach (String ci in cheminMedias)
            {
                _images.Add(new ImageModele(ci));
            }
        }
    }
}
