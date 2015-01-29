using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modele_Controleur
{
    public enum EtatInscription
    {
        OK,
        ERREUR_NOM,
        ERREUR_MDP,
        ERREUR_EMAIL,
        NOM_EXISTE_DEJA,
        EMAIL_EXISTE_DEJA,
    }
}
