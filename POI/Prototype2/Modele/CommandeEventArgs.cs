using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commun;
using msvipConnexionDLL.implementations;

namespace Modele
{
    //Classe permettant d'avoir des EventArgs avec un message
    public class CommandeEventArgs : EventArgs
    {
        public CommandeEventArgs(Commande.typeCommande c, Types t, string s, double n, bool b)
        {
            commande = c;
            type = t;
            msg = s;
            nombre = n;
            booleen = b;
        }


        private string msg;
        public string Message
        {
            get { return msg; }
        }

        private double nombre;
        public double Nombre
        {
            get { return nombre; }
        }

        private bool booleen;
        public bool Booleen
        {
            get { return booleen; }
        }

        private Types type;
        public Types Type
        {
            get { return type; }
        }

        private Commande.typeCommande commande;
        public Commande.typeCommande Commande
        {
            get { return commande; }
        }
    }
}
