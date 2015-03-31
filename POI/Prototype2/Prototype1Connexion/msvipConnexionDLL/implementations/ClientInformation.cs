/**
 * \file ClientInformation.cs
 * \brief Informations du client
 * \author MSVIP Team
 * \version Prototype 0
 */

using System.Runtime.Serialization;
using System.Net;

namespace msvipConnexionDLL.implementations
{
    /**
     * \class ClientInformation
     * \brief Informations du client
     */
    [DataContract]
    public class ClientInformation
    {
        /**
         * \enum TypePeriph
         * \brief Differents types de peripheriques
         */
        public enum TypePeriph { 
            Table, /**!< Table PixelSense */
            Tablette,  /**!< Tablette */
            Tbi  /**!< Tableau blanc interactif */
        };

        /**!< Nom du client */
        private string _name;

        /**!< Propriete accedant a l'attribut _name */
        [DataMember]
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != null)
                {
                    _name = value;
                }
            }
        }

        /**!< Port d'ecoute du client */
        private int _port;

        /**!< Propriete accedant a l'attribut _port */
        [DataMember]
        public int port
        {
            get
            {
                return _port;
            }
            set
            {
               _port = value;
            }
        }

        /**!< Adresse IP du client */
        private IPAddress _ip;

        /**!< Propriete accedant a l'attribut _ip */
        [DataMember]
        public IPAddress ip
        {
            get
            {
                return _ip;
            }
            set
            {
                if (value != null)
                {
                    _ip = value;
                }
            }
        }

        /**!< Type du peripherique client */
        private TypePeriph _periph;

        /**!< Propriete accedant a l'attribut _periph */
        [DataMember]
        public TypePeriph periph
        {
            get
            {
                return _periph;
            }
            set
            {
                _periph = value;
            }
        }
    }
}