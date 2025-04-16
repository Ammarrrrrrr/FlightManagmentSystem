using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace FlightManagmentSystem
{

    internal class Database
    {
        static readonly string server = "localhost";
        static readonly string user = "root";
        static readonly string pass = "";
        static readonly string db = "flight_management";
        public static string conStr = "server='" + server + "'; user= '" + user + "'; database= '" + db + "'; password='" + pass + "'";
        public MySqlConnection mySqlConnection = new MySqlConnection(conStr);
        public bool connect_db()
        {
            try
            {
                mySqlConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool close_db()
        {
            try
            {
                mySqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
