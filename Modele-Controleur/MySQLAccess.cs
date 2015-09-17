using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Modele_Controleur
{
    public class MySQLAccess
    {
        private static string conn = "Server=37.59.103.120;Port=443;Database=archimage;Uid=remote;Pwd=archimage35;";
        private MySqlConnection connect;

        public MySQLAccess()
        {
            //inscription("toto", "tata", "tototata");
            //EtatConnexion e = connexion("toto", "tata");
        }

        private void db_connection()
        {
            try
            {
                connect = new MySqlConnection(conn);
                connect.Open();
            }
            catch (MySqlException e)
            {
                throw;
            }
        }

        public Modele_Controleur.EtatConnexion connexion(string nom, string mdp)
        {
            /*db_connection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "Select * from Table_Utilisateurs where username=@user and password=@pass";
            cmd.Parameters.AddWithValue("@user", nom);
            cmd.Parameters.AddWithValue("@pass", mdp);
            cmd.Connection = connect;
            MySqlDataReader login = cmd.ExecuteReader();
            if (login.Read())
            {
                connect.Close();
                return EtatConnexion.OK;
            }
            else
            {
                connect.Close();
                return EtatConnexion.ERREUR_MDP;
            }*/
            return EtatConnexion.OK;
        }

        public EtatInscription inscription(string nom, string mdp, string email)
        {
            /*db_connection();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "INSERT INTO Table_Utilisateurs (username,password,email) values (@user,@pass,@email)";
            cmd.Parameters.AddWithValue("@user", nom);
            cmd.Parameters.AddWithValue("@pass", mdp);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Connection = connect;
            try
            {
                MySqlDataReader login = cmd.ExecuteReader();
                connect.Close();
                return EtatInscription.OK;
            }
            catch(Exception e)
            {
                connect.Close();
                return EtatInscription.ERREUR_MDP;
            }*/
            return EtatInscription.OK;
        }
    }
}
