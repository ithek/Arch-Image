/**
 * \file Exec.cs
 * \brief Execution d'une commande
 * \author MSVIP Team
 * \version Prototype 0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace msvipConnexionDLL.implementations
{
    /**
     * \class Exec
     * \brief Objet gerant l'execution d'une commande
     */
    public class Exec : interfaces.IExec
    {
        /**
         * \fn public void executer(string commande, string arguments)
         * \brief Execute une commande systeme avec differents parametres
         * \param commande la commande a executer
         * \param arguments une chaine contenant les arguments de la commande separes par un espace
         */
        public void executer(string commande, string arguments)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = commande;
            proc.StartInfo.Arguments = arguments;
            proc.Start();

            proc.Close();
            proc.Dispose();
        }

        /**
         * \fn public void lancerFichier(string fichier)
         * \brief Lance un fichier avec le programme associe a son extension dans le shell windows
         * \param fichier le chemin vers le fichier a ouvrir
         */
        public void lancerFichier(string fichier)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fichier;
            proc.Start();

            proc.Close();
            proc.Dispose();
        }

        /* ajouter les implémentation des différentes commandes à exécuter*/
    }
}
