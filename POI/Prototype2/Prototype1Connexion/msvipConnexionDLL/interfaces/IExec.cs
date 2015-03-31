/**
 * \file IExec.cs
 * \brief Interface IExec
 * \author MSVIP Team
 * \version Prototype 0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace msvipConnexionDLL.interfaces
{
    /**
     * \interface IExec
     * \brief Execution d'une commande
     */
    public interface IExec
    {
        /**
         * \fn void executer(string commande, string arguments)
         * \brief Execute une commande systeme avec differents parametres
         * \param commande la commande a executer
         * \param arguments une chaine contenant les arguments de la commande separes par un espace
         */
        void executer(string commande, string arguments);

        /**
         * \fn void lancerFichier(string fichier)
         * \brief Lance un fichier avec le programme associe a son extension dans le shell windows
         * \param fichier le chemin vers le fichier a ouvrir
         */
        void lancerFichier(string fichier);
    }
}
