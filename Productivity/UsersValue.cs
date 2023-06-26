using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Productivity
{
    internal class UsersValue
    {
        public List<User> LoadUsersList(List<int> equips, DateTime date)
        {
            List<User> usersList = new List<User>();

            string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";

            try
            {
                using (SqlConnection connection = DBConnection.GetDBConnection())
                {
                    connection.Open();
                    SqlCommand Command = new SqlCommand
                    {
                        Connection = connection,

                        CommandText =
                            @"SELECT
	                            id_common_employee,
	                            id_equip
                            FROM
	                            dbo.man_factjob
                            WHERE
                                date_begin IS NOT NULL AND 
	                            date_begin >= CONVERT ( VARCHAR ( 32 ), @startDate, 21 ) AND
	                            date_begin <= CONVERT ( VARCHAR ( 32 ), @endDate, 21 ) AND
                                id_common_employee IS NOT NULL AND
                                id_equip IS NOT NULL"
                    };
                    Command.Parameters.AddWithValue("@startDate", startDate);
                    Command.Parameters.AddWithValue("@endDate", endDate);

                    DbDataReader sqlReader = Command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);

                        if (equips.Contains(loadEquip))
                        {
                            int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);

                            if (usersList.FindIndex((v) => v.Id == loadUser &&
                                                           v.Equip == loadEquip) == -1)
                            {
                                usersList.Add(new User(loadUser, loadEquip));
                                usersList[usersList.Count - 1].Shifts = new List<UserShift>();
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }

            return usersList;
        }

        public Dictionary<int, string> LoadAllUsersNames()
        {
            Dictionary<int, string> users = new Dictionary<int, string>();

            try
            {
                using (SqlConnection connection = DBConnection.GetDBConnection())
                {
                    connection.Open();
                    SqlCommand Command = new SqlCommand
                    {
                        Connection = connection,
                        //CommandText = @"SELECT * FROM dbo.order_head WHERE status = '1' AND order_num LIKE '@order_num'"
                        //CommandText = @"SELECT * FROM dbo.common_employee WHERE fire_date IS null"
                        CommandText =
                            @"SELECT
	                            * 
                            FROM
	                            dbo.common_employee"
                        /* WHERE
                             fire_date IS NULL"*/

                    };
                    //Command.Parameters.AddWithValue("@order_num", "%" + textBox1.Text + "%");

                    DbDataReader sqlReader = Command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        string fullName = sqlReader["employee_lastname"].ToString() + " " +
                            sqlReader["employee_firstname"].ToString() + " " +
                            sqlReader["employee_middlename"].ToString();

                        users.Add(Convert.ToInt32(sqlReader["id_common_employee"]), fullName);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }

            return users;
        }
    }
}
