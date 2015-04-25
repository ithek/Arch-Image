using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Modele_Controleur
{
    public class MySQLAccess
    {
        public Modele_Controleur.EtatConnexion connexion(string nom, string mdp)
        {
            //Established a connection with the database
            using (SqlConnection myLoginConnection = new SqlConnection(@"Data Source=USER-PC\;Initial Catalog=TestDB;Integrated Security=True"))
            {
                //Open the connection
                myLoginConnection.Open();


                using (SqlCommand myLoginCommand = new SqlCommand("SELECT * FROM Table_Utilisateurs WHERE UserName = @UserName AND Password = @Password", myLoginConnection))
                {
                    myLoginCommand.Parameters.AddWithValue("UserName", nom);
                    myLoginCommand.Parameters.AddWithValue("Password", mdp);

                    SqlDataReader myLoginReader = myLoginCommand.ExecuteReader();

                    //if the data matches the rows (username, password), then you enter to the page
                    if (myLoginReader.Read())
                    {
                        myLoginConnection.Close();
                        myLoginReader.Close();
                        return Modele_Controleur.EtatConnexion.OK;
                    }
                    else
                    {
                        myLoginConnection.Close();
                        myLoginReader.Close();
                        return Modele_Controleur.EtatConnexion.ERREUR_MDP;
                    }
                }
            }
        }

        public EtatInscription inscription(string nom, string mdp, string email)
        {
            //Established a connection with the database
            using (SqlConnection myRegisterConnection = new SqlConnection(@"Data Source=USER-PC\;Initial Catalog=TestDB;Integrated Security=True"))
            {
                //Open the connection
                myRegisterConnection.Open();

                //Create a command where each control is equal to the values in the table
                using (SqlCommand myRegisterCommand = new SqlCommand("INSERT INTO [Table_Utilisateurs] values (@UserName, @Email, @Password)", myRegisterConnection))
                {
                    myRegisterCommand.Parameters.AddWithValue("UserName", nom);
                    myRegisterCommand.Parameters.AddWithValue("Email", email);
                    myRegisterCommand.Parameters.AddWithValue("Password", mdp);

                    myRegisterCommand.ExecuteNonQuery();
                }
                //Close connection 
                myRegisterConnection.Close();

                return EtatInscription.OK;
            }
        }
    }
}
