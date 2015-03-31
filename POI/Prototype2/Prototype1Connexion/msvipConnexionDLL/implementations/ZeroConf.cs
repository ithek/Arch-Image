using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Zeroconf;

/**
 * 
 * \file ZeroConf.cs
 * \brief Implementation de la couche supplementaire à Mono.Zeroconf
 * \author Equipe MSVIP
 * \version Prototype 0
 * \date 14/12/2012
 * 
 * Implementation de la couche supplementaire à Mono.Zeroconf
 */

namespace msvipConnexionDLL.implementations
{
    using interfaces;
    using System.Threading;

    /**
     * 
     * \class ZeroConf
     * \brief Ajout d'une couche au-dessus de Mono.Zeroconf
     * 
     */
    public class ZeroConf : IZeroConf
    {
        /**
         * !< Le service enregistre
         */
        private RegisterService service;

        /**
         * !< Permet de recherche les service
         */
        private IServiceBrowser browser;

        /**
         * !< Le type du registre de connexion
         */
        private string regType;

        /**
         * !< Le domaine d'ecoute
         */
        private string domain;

        /**
         * !< Le port d'ecoute du service
         */
        private short port;

        /**
         * !< Le semaphore de bloquage recherche service
         */
        private Semaphore sema;

        /**
         * \fn ZeroConf(short port, string regType, string domain)
         * \brief constructeur de la classe
         * 
         * \param[in] port le port d'ecoute du service 
         * \param[in] regType le type de registre 
         * \param[in] domain le domaine d'ecoute 
         */
        public ZeroConf(short port, string regType, string domain)
        {
            initZeroConf(port, regType, domain);
        }

        /**
        * \fn ZeroConf(short port)
        * \brief constructeur de la classe
        * 
        * \param[in] port le port d'ecoute du service 
        */
        public ZeroConf(short port)
        {
            initZeroConf(port, "_daap._tcp", "local");
        }

        /**
        * \fn ZeroConf()
        * \brief constructeur de la classe
        */
        public ZeroConf()
        {
            initZeroConf(6060, "_daap._tcp", "local");
        }

        /**
         * \fn private void initZeroConf(short port, string regType, string domain)
         * \brief Initialise les attributs de la classe
         * \param[in] port le port du service
         * \param[in] regType le type de registre
         * \param[in] domain le domaine d'ecoute
         */
        private void initZeroConf(short port, string regType, string domain)
        {
            this.regType = regType;
            this.domain = domain;
            this.port = port;
            this.sema = new Semaphore(0, 10);
        }

        /**
         * \fn public void enregistrementService(string name)
         * \brief Permet d'enregistrer un service
         * 
         * Permet d'enregistrer un service proposé par l'application
         * 
         * \param[in] name le nom du service propose
         */
        public void enregistrementService(string name)
        {
            service = new RegisterService();
            service.Name = name;
            service.RegType = regType + ".";
            service.ReplyDomain = this.domain;
            service.Port = port;

            service.Register();
        }

        /**
         * \fn void fermetureService(string name)
         * \brief Permet d'enlever un service
         * 
         * Permet d'enlever un service proposé par l'application
         * 
         * \param[in] name le nom du service a enlever
         */
        public void fermetureService()
        {
            service.Dispose();
        }

        /**
         * \fn public List<IResolvableService> rechercheServices()
         * \brief Recherche tous les services disponibles
         * 
         * Recherche tous les services disponibles et les retournes sous form de liste
         * 
         * \return les services disponibles
         */
        public List<IResolvableService> rechercheServices()
        {
            browser = new ServiceBrowser();

            List<IResolvableService> listServices = new List<IResolvableService>();

            // definition de l'action lors de la detection d'un service
            browser.ServiceAdded += delegate(object o, ServiceBrowseEventArgs args1)
            {
                args1.Service.Resolved += delegate(object o1, ServiceResolvedEventArgs args2)
                {
                    IResolvableService s = (IResolvableService)args2.Service;
                    listServices.Add(s);
                };
                args1.Service.Resolve();
            };

            // 
            // Trigger the request
            //
            browser.Browse(regType, domain);

            while (listServices.Count == 0);

            return listServices;
        }

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
        public IResolvableService rechercheService(string name)
        {
            browser = new ServiceBrowser();

            IResolvableService service = null;

            // definition de l'action lors de la detection d'un service
            browser.ServiceAdded += delegate(object o, ServiceBrowseEventArgs args1)
            {
                args1.Service.Resolved += delegate(object o1, ServiceResolvedEventArgs args2)
                {
                    IResolvableService s = (IResolvableService)args2.Service;
                    if (s.Name.CompareTo(name) == 0)
                    {
                        service = s;
                        sema.Release();
                    }
                };
                args1.Service.Resolve();
            };

            // 
            // Trigger the request
            //
            browser.Browse(regType, domain);

            sema.WaitOne();

            return service;
        }
    }
}