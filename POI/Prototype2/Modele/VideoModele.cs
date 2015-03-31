using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;

namespace Modele
{
    public class VideoModele : MediaModele
    {
        public VideoModele(String c) : base(Types.video,c) { }
    }
}
