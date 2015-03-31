/**
 * \file IRemoteServicePxS
 * \brief Serveur PixelSense
 * \author MSVIP Team
 * \version Prototype 0
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using msvipConnexionDLL.implementations;
using System.ServiceModel.Channels;
using Commun;

namespace msvipConnexionDLL.interfaces
{
    /**
     * \interface IRemoteServicePxS
     * \brief Serveur PixelSense
     */
    [ServiceContract(CallbackContract = typeof(IContractCallBack))]
    public interface IRemoteServicePxS
    {
        /**
         * \fn void enregistrerClient(ClientInformation.TypePeriph type))
         * \brief Enregistre un client
         * \param type le type du peripherique
         */
        [OperationContract]
        string enregistrerClient(ClientInformation.TypePeriph type);


        /**
         * \fn void supprimerClient(ClientInformation ci)
         * \brief Supprime un client
         * \param ci les informations du client
         */
        [OperationContract]
        void supprimerClient(ClientInformation ci);

        /**
         * \fn void lancerServeur()
         * \brief Lance le serveur
         */
        void lancerServeur();

        /**
         * \fn void sendCommande(ClientInformation.TypePeriph tp, Commande.typeCommande tc, string file, double nbr)
         * \brief Envoie une commande au client
         * \param tp le type de peripherique client
         * \param tc le type de commande
         * \param file le fichier a ouvrir
         * \param nbr un nombre
         * \param bl un boolean
         */
        void sendCommande(ClientInformation.TypePeriph tp, Commande.typeCommande tc, Types type, string file, double nbr, bool bl);

        /**
         * \fn void stopServeur()
         * \brief Stoppe le serveur
         */
        void stopServeur();
    }

    public interface IContractCallBack
    {
        [OperationContract(IsOneWay = true)]
        void Commande(Commande.typeCommande tc, Types type, string str, double nbr, bool bl);
    }
}
