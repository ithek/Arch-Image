using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Zeroconf;

/**
 * 
 * \file IZeroConf.cs
 * \brief Interface pour ajouter une nouvelle couche d'abstraction de ZeroConf
 * \author MSVIP Team
 * \version Prototype 0
 * \date 14/12/2012
 * 
 * Interface pour ajouter une nouvelle couche d'abstraction de ZeroConf
 */

namespace msvipConnexionDLL.interfaces
{

    /**
     * 
     * \interface IZeroConf
     * \brief Ajout d'une couche au-dessus de Mono.Zeroconf
     * 
     */
    public interface IZeroConf
    {
        /**
         * \fn void enregistrementService(string name)
         * \brief Permet d'enregistrer un service
         * 
         * Permet d'enregistrer un service proposé par l'application
         * 
         * \param[in] name le nom du service propose
         */
        void enregistrementService(string name);

        /**
         * \fn void fermetureService()
         * \brief Permet d'enlever un service
         * 
         * Permet d'enlever un service proposé par l'application
         * 
         */
        void fermetureService();

        /**
         * \fn List<IResolvableService> rechercheServices()
         * \brief Recherche tous les services disponibles
         * 
         * Recherche tous les services disponibles et les retournes sous forme de liste
         * 
         * \return les services disponibles
         */
        List<IResolvableService> rechercheServices();

        /**
         * \fn IResolvableService rechercheServices()
         * \brief Recherche le service de nom passer en parametre
         * 
         * Recherche tous le service de nom donne en parametre
         * 
         * \param[in] name le nom du service recherche
         * 
         * \return le service
         */
        IResolvableService rechercheService(string name);
    }
}