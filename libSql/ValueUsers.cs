using libData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace libSql
{
    public class ValueUsers
    {
        /// <summary>
        /// Получить список сотрудников с оборудованием за указанное количство дней
        /// </summary>
        /// <param name="equips"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<User>> LoadUsersListFromLastAnyDaysAsync (List<int> equips, int days)
        {
            /*string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";*/

            DateTime date = DateTime.Now;
            DateTime dateStart;
            DateTime dateEnd;

            if (date.Hour >= 8 && date.Hour <= 23)
            {
                //dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- days - 1);
                dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - days);
                //dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- 1);
                dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day);
            }
            else //if (date.Hour >= 0 && date.Hour < 8)
            {
                //dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- (days + 1) - 1);
                dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - (days + 1));
                //dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- 1 - 1);
                dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - 1);
            }

            string startDate = dateStart.ToString("yyyy-MM-dd") + "T07:40:00.000";
            string endDate = dateEnd.ToString("yyyy-MM-dd") + "T07:10:00.000";

            List<int> users = LoadUsersIncludedAllEquips(equips, startDate, endDate);

            List<User> usersList = await LoadUsersList(users, startDate, endDate);
            
            return usersList;
        }

        public async Task<List<User>> LoadUsersListFromLastAnyDays(List<int> equips, DateTime startDatePeriod)
        {
            /*string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";*/

            DateTime date = DateTime.Now;

            int period = (date - startDatePeriod).Days + 1;

            DateTime dateStart = startDatePeriod;
            //DateTime dateEnd = DateTime.MinValue.AddYears(startDatePeriod.Year - 1).AddMonths(startDatePeriod.Month - 1).AddDays(startDatePeriod.Day + period + 1);
            //DateTime dateEnd = DateTime.MinValue.AddYears(startDatePeriod.Year - 1).AddMonths(startDatePeriod.Month - 1).AddDays(period + 1);
            DateTime dateEnd = DateTime.MinValue.AddYears(startDatePeriod.Year - 1).AddMonths(startDatePeriod.Month - 1).AddDays(startDatePeriod.Day + period - 1);

            /*if (date.Hour >= 8 && date.Hour <= 23)
            {
                //dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- days - 1);
                dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - days);
                //dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- 1);
                dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day);
            }
            else //if (date.Hour >= 0 && date.Hour < 8)
            {
                //dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- (days + 1) - 1);
                dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - (days + 1));
                //dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(- 1 - 1);
                dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - 1);
            }*/

            string startDate = dateStart.ToString("yyyy-MM-dd") + "T07:40:00.000";
            string endDate = dateEnd.ToString("yyyy-MM-dd") + "T07:30:00.000";

            List<int> users = LoadUsersIncludedAllEquips(equips, startDate, endDate);

            List<User> usersList = await LoadUsersList(users, startDate, endDate);

            return usersList;
        }

        /// <summary>
        /// Получить список сотрудников с оборудованием за выбранный месяц
        /// </summary>
        /// <param name="equips"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<User>> LoadUsersListFromSelectMonth(List<int> equips, DateTime date)
        {
            /*string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";*/

            DateTime dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1);
            DateTime dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month);
            
            string startDate = dateStart.ToString("yyyy-MM-dd") + "T07:40:00.000";
            string endDate = dateEnd.ToString("yyyy-MM-dd") + "T07:10:00.000";

            List<int> users = LoadUsersIncludedAllEquips(equips, startDate, endDate);

            List<User> usersList = await LoadUsersList(users, startDate, endDate);

            return usersList;
        }

        /// <summary>
        /// Получить индексы всех сотрудников указанного месяца
        /// </summary>
        /// <param name="equips"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<int> LoadUsersListOnlyIDFromSelectMonth(List<int> equips, DateTime date)
        {
            /*string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";*/

            DateTime dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1);
            DateTime dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month);

            string startDate = dateStart.ToString("yyyy-MM-dd") + "T07:40:00.000";
            string endDate = dateEnd.ToString("yyyy-MM-dd") + "T07:10:00.000";

            List<int> users = LoadUsersIncludedAllEquips(equips, startDate, endDate);

            return users;
        }

        /// <summary>
        /// Получить индексы всех сотрудников указанного года
        /// </summary>
        /// <param name="equips"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<int> LoadUsersListOnlyIDFromSelectYear(List<int> equips, DateTime date)
        {
            /*string startDate = date.ToString("yyyy-MM") + "-01T07:40:00.000";
            string endDate = date.AddMonths(1).ToString("yyyy-MM") + "-01T07:10:00.000";*/

            DateTime dateStart = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(0);
            DateTime dateEnd = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(12);

            string startDate = dateStart.ToString("yyyy-MM-dd") + "T07:40:00.000";
            string endDate = dateEnd.ToString("yyyy-MM-dd") + "T07:10:00.000";

            List<int> users = LoadUsersIncludedAllEquips(equips, startDate, endDate);

            return users;
        }

        private async Task<List<User>> LoadUsersList(List<int> users, string startDate, string endDate)
        {
            List<User> usersList = new List<User>();

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                await connection.OpenAsync();
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

                DbDataReader sqlReader = await Command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);
                    int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);

                    if (users.Contains(loadUser))
                    {
                        if (usersList.FindIndex((v) => v.Id == loadUser) == -1)
                        {
                            usersList.Add(new User(loadUser));
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

        public Dictionary<int, string> LoadAllUsersNames(bool shortName = false)
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
                        string name;

                        if (shortName)
                        {
                            name = sqlReader["employee_firstname"].ToString() + " " +
                            sqlReader["employee_lastname"].ToString();
                        }
                        else
                        {
                            name = sqlReader["employee_lastname"].ToString() + " " +
                            sqlReader["employee_firstname"].ToString() + " " +
                            sqlReader["employee_middlename"].ToString();
                        }

                        users.Add(Convert.ToInt32(sqlReader["id_common_employee"]), name);
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

        public string GetUserNameFromID(int indexUserADFromASBase)
        {
            string name = "";

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
	                            * 
                            FROM
	                            dbo.common_employee
                            WHERE
                                id_common_employee = @userID"

                    };
                    Command.Parameters.AddWithValue("@userID", indexUserADFromASBase);

                    DbDataReader sqlReader = Command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        name = sqlReader["employee_firstname"].ToString() + " " +
                            sqlReader["employee_lastname"].ToString();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return name;
        }
    }
}
