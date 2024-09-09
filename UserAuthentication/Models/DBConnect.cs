using MySql.Data.MySqlClient;
using System.Data;

namespace UserAuthentication.Models
{
    
    public class DBConnect
    {
        private string server_name = "localhost";
        private string uid = "authuser";
        private string password = "authpassword";
        private string database_name = "userauth";
        MySqlConnection conn = null!;

        public DBConnect()
        {
            this.ConnectToDatabase();
        }

        /// <summary>
        /// Connects to the MySQL database and opens the connection. Should be okay to keep it open for the program duration.
        /// </summary>
        public void ConnectToDatabase ()
        {
            try
            {
                if (conn == null)
                {
                    this.conn = new MySqlConnection($"server={server_name};uid={uid};pwd={password};database={database_name}");
                    conn.Open();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// Closes the MySQL connection, but only if it's currently open.
    /// </summary>
    public void CloseConnection ()
        {
            try
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

        }
}
