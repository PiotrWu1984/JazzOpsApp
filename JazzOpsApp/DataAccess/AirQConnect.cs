using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace JazzOpsApp.DataAccess
{
    public class AirQConnect
    {
        private MySqlConnection connection;
        
        public AirQConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AirQMySql"].ConnectionString; 

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Select statement
        public List<string[]> Select(string query)
        {
            //Create a list to store the result
            List<string[]> list = new List<string[]>();

            //Open connection
            if (OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    //make string[] the size of the row being returned
                    string[] row = new string[dataReader.FieldCount];
                    //fill sting[] with row data
                    for (int i = 0; i < row.Length; i++)
                        row[i] = dataReader[i].ToString();
                    //add string[] to return list
                    list.Add(row);
                }

                //close stuff
                dataReader.Close();
                CloseConnection();

                //return list to be displayed
                return list;
            }
            else
                return list;
        }
    }
}
