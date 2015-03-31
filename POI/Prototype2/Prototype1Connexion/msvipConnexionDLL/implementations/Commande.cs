/**
 * \file Commande.cs
 * \brief Commande
 * \author MSVIP Team
 * \version Prototype 0
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using msvipConnexionDLL.interfaces;
using System.Runtime.Serialization;

namespace msvipConnexionDLL.implementations
{
    /**
     * \class Commande
     * \brief Creation d'une commande
     */
    public class Commande
    {
        // Pour executer une commande systeme, on pourrait faire une Hashtable<typeCommande,string>

        /**!< Chemin du fichier a ouvrir */
        public string file;

        /**
         * \enum typeCommande
         * \brief Differents types de commandes
         */
        public enum typeCommande
        {
            lancerMedia,  /**!< Lancement d'un media */
            fermetureMedia, /**!< Fermeture d'un media */
            lectureVideo, /**!< Lecture d'une video */
            pauseVideo, /**!< Mise en pause d'une video */
            debutVideo, /**!< Retour au debut de la video */
            diapoSuivante, /**!< Diapositive suivante */
            diapoPrecedente, /**!< Diapositive precedente */
            allerA, /**!< Aller a une position precise de la video */
            finConnexion /**!< Informe les peripheriques connectes de la fin de la connexion */ 
            /* possibilité d'ajouter des commandes, à modifier. exemple passer des diapos*/
        }

        /**!< Type de la commande */
        public typeCommande tc;

        /**!< Objet gerant l'execution de la commande */
        private IExec exec;

        /**
         * \fn public Commande(typeCommande tc, string file)
         * \brief Constructeur de Commande
         * \pararm tc le type de la commande
         * \param file le chemin du fichier a ouvrir
         */
        public Commande(typeCommande tc, string file)
        {
            this.exec = new Exec();
            this.file = file;
            this.tc = tc;
        }

        /**
         * \fn public void run()
         * \brief Execute la commande
         */
        public void run()
        {
            switch (tc)
            {
                /*ajouter les différentes comandes à éxécuter*/
                case typeCommande.lancerMedia:
                    exec.lancerFichier(file);
                    break;
                default: break;
            }
        }

    }
}
