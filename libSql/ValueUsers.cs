using libData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace libSql
{
    public class ValueUsers
    {
        //Не используемый метод
        public List<User> LoadUsersList(List<int> equips, DateTime date)
        {
            List<User> usersList = new List<User>();

            string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";

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
                    int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);

                    if (equips.Contains(loadEquip))
                    {
                        //int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);

                        if (usersList.FindIndex((v) => v.Id == loadUser && v.Equip == loadEquip) == -1)
                        //if (usersList.FindIndex((v) => v.Id == loadUser) == -1)
                        {
                            usersList.Add(new User(loadUser, loadEquip));
                            usersList[usersList.Count - 1].Shifts = new List<UserShift>();
                        }
                    }
                }

                connection.Close();
            }

            return usersList;
        }//

        /// <summary>
        /// Получить список сотрудников с оборудованием за выбранный месяц
        /// </summary>
        /// <param name="equips"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<User> LoadUsersListFromSelectMonth(List<int> equips, DateTime date)
        {
            /*string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";*/

            DateTime dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1);
            DateTime dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month);

            string startDate = dateStart.ToString("yyyy-MM-dd") + "T07:40:00.000";
            string endDate = dateEnd.ToString("yyyy-MM-dd") + "T07:10:00.000";

            List<int> users = LoadUsersIncludedAllEquips(equips, startDate, endDate);

            List<User> usersList = LoadUsersList(users, startDate, endDate);

            return usersList;
        }

        private List<User> LoadUsersList(List<int> users, string startDate, string endDate)
        {
            List<User> usersList = new List<User>();

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
                    int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);

                    if (users.Contains(loadUser))
                    {
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

            return usersList;
        }

        private List<int> LoadUsersIncludedAllEquips(List<int> equips, string startDate, string endDate)
        {
            List<int> users = new List<int>();

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

                        if (!users.Contains(loadUser))
                        {
                            users.Add(loadUser);
                        }
                    }
                }

                connection.Close();
            }

            return users;
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
                //MessageBox.Show(ex.Message, "Ошибка подключения");
            }

            return users;
        }
    }
}
