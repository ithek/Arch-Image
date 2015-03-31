/**
 * \file ClientPeriph.cs
 * \brief Client sur un peripherique
 * \author MSVIP Team
 * \version Prototype 0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using msvipConnexionDLL.interfaces;
using msvipConnexionDLL.implementations;
using Mono.Zeroconf;
using System.Net;
using Commun;

namespace Modele
{
    /**
     * \class ClientPeriph
     * \brief Client sur un peripherique
     */
    public class ClientPeriph
    {
        /**!< Canal de communication client/serveur */
        private DuplexChannelFactory<IRemoteServicePxS> canalCommunication;

        /**!< Objet representant le serveur */
        private IRemoteServicePxS serveur;

        /**!< Informations du client*/
        private ClientInformation clientInfo;

        /**!< Couche supplementaire d'implementation de Mono.ZeroConf */
        private IZeroConf conf;

        /**!< Instance permettant le callback */
        private InstanceContext instanceContext;

        /**
         * \fn public ClientPeriph(ClientInformation.TypePeriph typePeriph)
         * \brief Constructeur de la classe
         * \param typePeriph le type de peripherique du client
         */
        public ClientPeriph(ClientInformation.TypePeriph typePeriph)
        {
            clientInfo = new ClientInformation();
            clientInfo.periph = typePeriph;

            conf = new ZeroConf(8000, "_daap._tcp", "local");

            // recherche de l'adresse IP du peripherique
            IPHostEntry host;

            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    clientInfo.ip = ip;
                }
            }

            instanceContext = new InstanceContext(new CallbackHandler(this));
        }

        /**
         * \fn public void lancerClient(string nameServeur)
         * \brief Lance le client
         * \param nameServeur le nom du serveur auquel se connecter
         */
        public void lancerClient(string nameServeur)
        {
            // recherche du serveur de nameServeur
            IResolvableService serveur1 = conf.rechercheService(nameServeur);

            // connexion au service distant
            canalCommunication = new DuplexChannelFactory<IRemoteServicePxS>(instanceContext, new NetTcpBinding("netTcp"), new EndpointAddress("net.tcp://" + serveur1.HostEntry.AddressList[0].ToString() + ":" + serveur1.Port));

            canalCommunication.Open();

            serveur = canalCommunication.CreateChannel();

            clientInfo.name = serveur.enregistrerClient(clientInfo.periph);
        }

        /**
         * \fn public void stopClient()
         * \brief Stoppe le client
         */
        public void stopClient()
        {
            serveur.supprimerClient(clientInfo);
            canalCommunication.Close();
        }

        /**
         * \fn public void fermetureCanal()
         * \brief Ferme la canal de communication avec la table
         */
        public void fermetureCanal()
        {
            canalCommunication.Close();
        }

        /** Evenement permettant de communiquer avec la classe au niveau superieur **/
        public delegate void CommandeEventHandler(object sender, CommandeEventArgs a);

        public event EventHandler<CommandeEventArgs> RaiseCommandeEvent;

        protected virtual void OnRaiseCommandeEvent(CommandeEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CommandeEventArgs> handler = RaiseCommandeEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        /* Interface de Callback */
        public class CallbackHandler : IContractCallBack
        {
            ClientPeriph client;

            public CallbackHandler(ClientPeriph c)
            {
                client = c;
            }

            public void Commande(Commande.typeCommande tc, Types type, String message, double number, bool booleen)
            {
                client.OnRaiseCommandeEvent(new CommandeEventArgs(tc,type,message,number,booleen));

                /*
                Commande c = new Commande(keyValue.Key, keyValue.Value);

                // execute la commande retournee par le serveur
                c.run();
                */
            }
            
        }
    }


}
