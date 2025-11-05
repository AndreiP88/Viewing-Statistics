using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace libSql
{
    public class ValueEquips
    {
        public Dictionary<int, string> LoadMachine()
        {
            Dictionary<int, string> machines = new Dictionary<int, string>();

            //try
            {
                using (SqlConnection connection = DBConnection.GetDBConnection())
                {
                    connection.Open();
                    SqlCommand Command = new SqlCommand
                    {
                        Connection = connection,

                        CommandText =
                            @"SELECT
	                            id_common_equip_directory,
	                            equip_name
                            FROM
	                            dbo.common_equip_directory"
                    };
                    //Command.Parameters.AddWithValue("@order_num", "%" + textBox1.Text + "%");

                    DbDataReader sqlReader = Command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        machines.Add(Convert.ToInt32(sqlReader["id_common_equip_directory"]), sqlReader["equip_name"].ToString());
                    }

                    connection.Close();
                }
            }
            //catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Ошибка подключения equip");
            }

            return machines;
        }
    }
}
