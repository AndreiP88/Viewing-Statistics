using System.Data.SqlClient;

namespace libSql
{
    public class DBConnection
    {
        public static SqlConnection GetDBConnection()
        {
            string host = "SRV-ACS\\DSACS";
            string database = "asystem";
            string username = "ds";
            string password = "1";

            return DBSQLUtils.GetDBConnection(host, database, username, password);
        }

        public static SqlConnection GetDBConnection(string host, string database, string username, string password)
        {
            /*string host = "25.21.38.172";
            int port = 3309;
            string database = "order_manager";
            string username = "oxyfox";
            string password = "root";*/

            return DBSQLUtils.GetDBConnection(host, database, username, password);
        }

        public bool IsServerConnected()
        {
            using (SqlConnection Connect = GetDBConnection())
            {
                try
                {
                    Connect.Open();
                    Connect.Close();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
    }
}
