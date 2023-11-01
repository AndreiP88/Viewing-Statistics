using System.Data.SqlClient;

namespace libSql
{
    internal class DBSQLUtils
    {
        public static SqlConnection
        GetDBConnection(string host, string database, string username, string password)
        {
            // Connection String.
            string connString = "Data Source = " + host + "; Initial Catalog = " + database + "; Persist Security Info = True; User ID = " + username + "; Password = " + password;

            //string connString = "Server = localhost; Database = asystem; Trusted_Connection = True";

            //string connectionString = @"Data Source = SRV-ACS\DSACS; Initial Catalog = asystem; Persist Security Info = True; User ID = ds; Password = 1";

            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }
    }
}
