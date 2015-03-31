/**
 * \file RemoteServicePxs.cs
 * \brief Serveur PixelSense
 * \author MSVIP Team
 * \version Prototype 0
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.ServiceModel;
using Mono.Zeroconf;
using msvipConnexionDLL.implementations;
using msvipConnexionDLL.interfaces;
using Commun;

namespace Modele
{
    /**
     * \class RemoteServicePxs
     * \brief Serveur PixelSense
     */
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class RemoteServicePxS : IRemoteServicePxS
    {
        /**!< Hachage statique contenant les types de peripheriques ainsi que les semaphores des clients */
        private static Dictionary<ClientInformation.TypePeriph, Dictionary<string, IContractCallBack>> clients = new Dictionary<ClientInformation.TypePeriph, Dictionary<string, IContractCallBack>>();

        /**!< Couche supplementaire de Mono.ZeroConf */
        private IZeroConf conf;

        /**!< IP du serveur */
        private IPAddress ipaddr;

        /**!< Objet qui represente le serveur*/
        private ServiceHost host;

        /**!<  Nom du serveur*/
        private string name;

        /**!<  Compteur du nombre de périphériques*/
        private static int compteur = 0;


        //Evenement pour signaler l'inscription ou la desincription d'un client
        public static event EventHandler NombreClientsChanged;
        public delegate void ChangedEventHandler(object sender, EventArgs e);

        protected void OnNombreClientsChanged(EventArgs e)
        {
            if (NombreClientsChanged != null)
                NombreClientsChanged(this, e);
        }

        /**!<  Compteur du nombre de Tablette*/
        public int nombreTablette
        {
            get { return clients[ClientInformation.TypePeriph.Tablette].Count; }
        }

        /**!<  Compteur du nombre de TBI*/
        public int nombreTbi
        {
            get { return clients[ClientInformation.TypePeriph.Tbi].Count; }
        }

        /**
         * \fn public RemoteServicePxS(short port,string name)
         * \brief Constructeur de la classe
         * \param port le port d'ecoute du serveur
         * \name le nom du serveur
         */
        public RemoteServicePxS(short port, string name)
        {
            conf = new ZeroConf(port, "_daap._tcp", "local");

            this.name = name;

            conf.enregistrementService(name);

            //Ajout des deux dictionnaires clients
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tablette))
            {
                clients.Add(ClientInformation.TypePeriph.Tablette, new Dictionary<string, IContractCallBack>());
            }
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tbi))
            {
                clients.Add(ClientInformation.TypePeriph.Tbi, new Dictionary<string, IContractCallBack>());
            }
        }

        /**
         * \fn public RemoteServicePxS(short port)
         * \brief Constructeur de la classe
         * \param port le port d'ecoute du serveur
         */
        public RemoteServicePxS(short port)
        {
            conf = new ZeroConf(port, "_daap._tcp", "local");

            this.name = "msvip";

            conf.enregistrementService("msvip");

            //Ajout des deux dictionnaires clients
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tablette))
            {
                clients.Add(ClientInformation.TypePeriph.Tablette, new Dictionary<string, IContractCallBack>());
            }
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tbi))
            {
                clients.Add(ClientInformation.TypePeriph.Tbi, new Dictionary<string, IContractCallBack>());
            }
        }

        /**
         * \fn public RemoteServicePxS(string name)
         * \brief Constructeur de la classe
         * \param name le noim du serveur
         */
        public RemoteServicePxS(string name)
        {
            conf = new ZeroConf(8000, "_daap._tcp", "local");

            this.name = name;

            conf.enregistrementService(name);

            //Ajout des deux dictionnaires clients
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tablette))
            {
                clients.Add(ClientInformation.TypePeriph.Tablette, new Dictionary<string, IContractCallBack>());
            }
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tbi))
            {
                clients.Add(ClientInformation.TypePeriph.Tbi, new Dictionary<string, IContractCallBack>());
            }
        }

        /**
         * \fn  public RemoteServicePxS()
         * \brief Constructeur de la classe
         */
        public RemoteServicePxS()
        {
            conf = new ZeroConf(8000, "_daap._tcp", "local");

            this.name = "msvip";

            conf.enregistrementService("msvip");

            //Ajout des deux dictionnaires clients
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tablette))
            {
                clients.Add(ClientInformation.TypePeriph.Tablette, new Dictionary<string, IContractCallBack>());
            }
            if (!clients.ContainsKey(ClientInformation.TypePeriph.Tbi))
            {
                clients.Add(ClientInformation.TypePeriph.Tbi, new Dictionary<string, IContractCallBack>());
            }
        }

        /**
         * \fn public void lancerServeur()
         * \brief Lance le serveur
         */
        public void lancerServeur()
        {
            //recherche son propre service afin de connaitre son IP
            IResolvableService s = conf.rechercheService("msvip");
            ipaddr = null;

            ipaddr = new IPAddress(s.HostEntry.AddressList[0].GetAddressBytes());

            //Publication de la classe en multithread
            host = new ServiceHost(typeof(RemoteServicePxS));

            host.AddServiceEndpoint(typeof(IRemoteServicePxS), new NetTcpBinding("netTcp"), "net.tcp://" + ipaddr.ToString() + ":" + s.Port);

            //Ouverture du canal de communication
            host.Open();
        }

        /**
         * \fn public void stopServeur()
         * \brief Stoppe le serveur
         */
        public void stopServeur()
        {
            //Envoi d'un message de fermeture à tous les périphériques connectés
            sendCommande(ClientInformation.TypePeriph.Tablette, Commande.typeCommande.finConnexion, Types.vitrine, "", 0, false);
            sendCommande(ClientInformation.TypePeriph.Tbi, Commande.typeCommande.finConnexion, Types.vitrine, "", 0, false);

            //Fermeture du canal de communication
            host.Close();

            //Desinscription du serveur
            conf.fermetureService();
        }

        /**
         * \fn public void sendCommande(ClientInformation.TypePeriph tp, Commande.typeCommande tc, string file)
         * \brief Envoie une commande a un client
         * \param tp le type du peripherique client
         * \param tc le type de commande
         * \param file le chemin du fichier a ouvrir
         */
        public void sendCommande(ClientInformation.TypePeriph tp, Commande.typeCommande tc, Types type, string file, double nbr, bool bl)
        {
            //si il existe au moins un peripherique du type tp
            if (clients[tp].Count > 0)
            {
                Dictionary<string, IContractCallBack> dico;
                clients.TryGetValue(tp, out dico);

                // pour chaque peripherique de ce type on recupere le semaphore
                foreach (IContractCallBack contract in dico.Values)
                {
                    contract.Commande(tc, type, file, nbr,bl);
                }
            }
        }

        #region IRemoteServicePxS Members


        /**
         * \fn IContractCallBack Callback
         * \brief Retourne l'interface de Callback de l'objet appelant
         */
        IContractCallBack Callback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IContractCallBack>();
            }
        }


        /**
         * \fn public virtual void enregistrerClient(ClientInformation ci)
         * \brief Enregistre un client
         * \param ci les informations du client
         */
        public virtual string enregistrerClient(ClientInformation.TypePeriph type)
        {
            string name;
            RemoteServicePxS.compteur = RemoteServicePxS.compteur + 1;
            Console.WriteLine("Valeur du compteur : " + compteur);
            switch(type)
            {
                case ClientInformation.TypePeriph.Tablette:
                    name = "tab" + compteur.ToString();
                    break;
                case ClientInformation.TypePeriph.Tbi:
                    name = "tbi" + compteur.ToString();
                    break;
                default:
                    name = "tab" + compteur.ToString();
                    break;
            }

            Console.WriteLine("Enregistrement client {0}", name);

            //on ajoute ce client a la liste
            Dictionary<string, IContractCallBack> dicoTmp;
            clients.TryGetValue(type, out dicoTmp);
            dicoTmp.Add(name, Callback);

            //On informe la table du changement
            OnNombreClientsChanged(EventArgs.Empty);

            return name;
        }



        /**
         * \fn public virtual void supprimerClient(ClientInformation ci)
         * \brief Supprimer un client
         * \param ci les informations du client
         */
        public virtual void supprimerClient(ClientInformation ci)
        {
            Console.WriteLine("Suppression client {0}", ci.name);

            if (clients[ci.periph].ContainsKey(ci.name))
            {
                clients[ci.periph].Remove(ci.name);
            }

            OnNombreClientsChanged(EventArgs.Empty);
        }

        #endregion
    }
}
