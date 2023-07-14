using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Productivity
{
    public class ValueShifts
    {
        public ValueShifts()
        {
            
        }
        public List<User> LoadShifts(List<User> listUsers, DateTime selectDate, int countShifts)
        {
            List<User> usersList = listUsers;

            ValueDateTime timeValues = new ValueDateTime();

            int countDaysFromSellectedDate = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            for (int currentDay = 0; currentDay < countDaysFromSellectedDate; currentDay++)
            {
                for (int currentShift = 1; currentShift <= countShifts; currentShift++)
                {
                    DateTime currentDate = selectDate.AddDays(currentDay);

                    List<User> loadedList = LoadOrders(currentDate, currentShift);

                    for (int i = 0; i < loadedList.Count; i++)
                    {
                        int indexFromUserList = usersList.FindIndex((v) => v.Id == loadedList[i].Id &&
                                                                           v.Equip == loadedList[i].Equip);

                        if (indexFromUserList != -1)
                        {
                            usersList[indexFromUserList].Shifts.AddRange(loadedList[i].Shifts);
                            /*for (int j = 0; j < loadedList[i].Shifts.Count; j++)
                            {
                                usersList[indexFromUserList].Shifts.Add(loadedList[i].Shifts[j]);
                            }*/
                        }
                    }
                }
            }

            return usersList;
        }

        public List<User> LoadOrders(DateTime currentDate, int currentShift)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<User> usersList = new List<User>();

            string dateShift = currentDate.ToString("dd.MM.yyyy");

            string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDate(currentDate, currentShift);
            string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDate(currentDate, currentShift);

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
	                        order_head.id_order_head, 
	                        man_planjob.id_man_planjob, 
	                        man_planjob.status,  
                            man_factjob.id_common_employee,
                            man_factjob.id_equip,
	                        common_equip_directory.equip_name, 
	                        man_factjob.shift_num, 
	                        order_head.order_num, 
	                        common_ul_directory.ul_name, 
	                        order_head.order_name, 
	                        man_factjob.date_begin, 
	                        man_factjob.date_end, 
	                        man_factjob.duration, 
	                        man_factjob.fact_out_qty, 
	                        man_factjob.flags, 
	                        man_planjob_list.plan_out_qty, 
	                        man_planjob_list.normtime, 
	                        man_factjob.norm_time, 
	                        man_factjob.id_man_factjob, 
	                        man_planjob_list.id_norm_operation, 
	                        man_planjob_list.id_man_order_job_item, 
	                        man_planjob_list.id_man_planjob_list
                        FROM
	                        dbo.man_factjob
                        INNER JOIN
	                        dbo.man_planjob_list
	                    ON 
		                    man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
	                    RIGHT JOIN
	                        dbo.fbc_brigade
	                    ON 
		                    man_factjob.id_fbc_brigade = fbc_brigade.id_fbc_brigade
	                    RIGHT JOIN
	                        dbo.common_employee
	                    ON 
		                    man_factjob.id_common_employee = common_employee.id_common_employee
	                    INNER JOIN
	                        dbo.common_equip_directory
	                    ON 
		                    man_factjob.id_equip = common_equip_directory.id_common_equip_directory
	                    LEFT JOIN
	                        dbo.man_order_job_item
	                    ON 
		                    man_planjob_list.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                    INNER JOIN
	                        dbo.man_order_job
	                    ON 
		                    man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                    INNER JOIN
	                        dbo.order_head
	                    ON 
		                    man_order_job.id_order_head = order_head.id_order_head
	                    LEFT JOIN
	                        dbo.common_ul_directory
	                    ON 
		                    order_head.id_customer = common_ul_directory.id_common_ul_directory
	                    LEFT JOIN
	                        dbo.man_planjob
	                    ON 
		                    man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item
                        WHERE
                            man_factjob.date_begin IS NOT NULL AND 
                            man_factjob.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) AND
                            man_factjob.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) AND
                            man_factjob.shift_num = @shiftNum AND
                            man_factjob.id_common_employee IS NOT NULL AND
                            man_factjob.id_equip IS NOT NULL AND
                            man_factjob.fact_out_qty IS NOT NULL AND
                            man_planjob_list.normtime IS NOT NULL AND
                            man_planjob_list.plan_out_qty IS NOT NULL
                        ORDER BY man_factjob.date_begin"
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);
                    int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);

                    int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser &&
                                                                       v.Equip == loadEquip);

                    if (indexFromUserList == -1)
                    {
                        usersList.Add(new User(
                            loadUser,
                            loadEquip
                        ));

                        indexFromUserList = usersList.Count - 1;

                        usersList[indexFromUserList].Shifts = new List<UserShift>();
                    }

                    int indexFromUserListShifts = usersList[indexFromUserList].Shifts.FindIndex(
                                                        (v) => v.ShiftDate == dateShift &&
                                                               v.ShiftNumber == currentShift);

                    int indexShift = indexFromUserListShifts;

                    if (indexFromUserListShifts == -1)
                    {
                        usersList[indexFromUserList].Shifts.Add(new UserShift(
                        dateShift,
                        currentShift
                        ));

                        indexShift = usersList[indexFromUserList].Shifts.Count - 1;

                        usersList[indexFromUserList].Shifts[indexShift].Orders = new List<UserShiftOrder>();
                    }

                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        sqlReader["order_num"].ToString(),
                        sqlReader["ul_name"].ToString(),
                        Convert.ToInt32(sqlReader["status"]),
                        Convert.ToInt32(sqlReader["flags"]),
                        sqlReader["date_begin"].ToString(),
                        sqlReader["date_end"].ToString(),
                        Convert.ToInt32(sqlReader["duration"]),
                        Convert.ToInt32(sqlReader["fact_out_qty"]),
                        Convert.ToInt32(sqlReader["plan_out_qty"]),
                        Convert.ToInt32(sqlReader["normtime"]),
                        Convert.ToInt32(sqlReader["id_man_order_job_item"])
                    ));
                }

                connection.Close();
            }

            return usersList;
        }

        public int[] GetNormTimeForOrder(int idManOrderJobItem)
        {
            int[] result = { 0, 0 };

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            normtime,
                            eff_output_coeff
                        FROM
	                        dbo.man_planjob_list
                        WHERE
                            id_man_order_job_item = @idManOrderJobItem AND
                            eff_output_coeff IS NOT NULL"

                };
                Command.Parameters.AddWithValue("@idManOrderJobItem", idManOrderJobItem);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    if (Convert.ToInt32(sqlReader["eff_output_coeff"]) == 0)
                    {
                        result[0] = Convert.ToInt32(sqlReader["normtime"]);
                    }
                    else
                    {
                        result[1] = Convert.ToInt32(sqlReader["normtime"]);
                    }
                }

                connection.Close();
            }

            return result;
        }











        /*public List<User> LoadOrdersFromdateAndShift(DateTime currentDate, int currentShift)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<int> indexesFactJob = new List<int>();
            List<User> usersList = new List<User>();

            string dateShift = currentDate.ToString("dd.MM.yyyy");

            string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDate(currentDate, currentShift);
            string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDate(currentDate, currentShift);

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            man_factjob.id_man_factjob
                        FROM
	                        dbo.man_factjob
                        WHERE
                            man_factjob.date_begin IS NOT NULL AND 
                            man_factjob.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) AND
                            man_factjob.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) AND
                            man_factjob.shift_num = @shiftNum AND
                            man_factjob.id_common_employee IS NOT NULL AND
                            man_factjob.id_equip IS NOT NULL AND
                            man_factjob.fact_out_qty IS NOT NULL"

                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    indexesFactJob.Add(Convert.ToInt32(sqlReader["id_man_factjob"]));
                }

                connection.Close();
            }

            for (int i = 0; i < indexesFactJob.Count; i++)
            {
                using (SqlConnection connection = DBConnection.GetDBConnection())
                {
                    connection.Open();
                    SqlCommand Command = new SqlCommand
                    {
                        Connection = connection,

                        CommandText =
                            @"SELECT
	                            order_head.id_order_head, 
	                            man_planjob.id_man_planjob, 
	                            man_planjob.status,  
                                man_factjob.id_common_employee,
                                man_factjob.id_equip,
	                            common_equip_directory.equip_name, 
	                            man_factjob.shift_num, 
	                            order_head.order_num, 
	                            common_ul_directory.ul_name, 
	                            order_head.order_name, 
	                            man_factjob.date_begin, 
	                            man_factjob.date_end, 
	                            man_factjob.duration, 
	                            man_factjob.fact_out_qty, 
	                            man_factjob.flags, 
	                            man_planjob_list.plan_out_qty, 
	                            man_planjob_list.normtime, 
	                            man_factjob.norm_time, 
	                            man_factjob.id_man_factjob, 
	                            man_planjob_list.id_norm_operation, 
	                            man_planjob_list.id_man_order_job_item, 
	                            man_planjob_list.id_man_planjob_list
                            FROM
	                            dbo.man_factjob
                            INNER JOIN
	                            dbo.man_planjob_list
	                        ON 
		                        man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
	                        RIGHT JOIN
	                            dbo.fbc_brigade
	                        ON 
		                        man_factjob.id_fbc_brigade = fbc_brigade.id_fbc_brigade
	                        RIGHT JOIN
	                            dbo.common_employee
	                        ON 
		                        man_factjob.id_common_employee = common_employee.id_common_employee
	                        INNER JOIN
	                            dbo.common_equip_directory
	                        ON 
		                        man_factjob.id_equip = common_equip_directory.id_common_equip_directory
	                        LEFT JOIN
	                            dbo.man_order_job_item
	                        ON 
		                        man_planjob_list.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                        INNER JOIN
	                            dbo.man_order_job
	                        ON 
		                        man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                        INNER JOIN
	                            dbo.order_head
	                        ON 
		                        man_order_job.id_order_head = order_head.id_order_head
	                        LEFT JOIN
	                            dbo.common_ul_directory
	                        ON 
		                        order_head.id_customer = common_ul_directory.id_common_ul_directory
	                        LEFT JOIN
	                            dbo.man_planjob
	                        ON 
		                        man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item
                            WHERE
                                man_factjob.id_man_factjob = @indexFactJob AND
                                man_planjob_list.normtime IS NOT NULL AND
                                man_planjob_list.plan_out_qty IS NOT NULL"
                    };
                    Command.Parameters.AddWithValue("@indexFactJob", indexesFactJob[i]);

                    DbDataReader sqlReader = Command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);
                        int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);

                        int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser &&
                                                                           v.Equip == loadEquip);

                        if (indexFromUserList == -1)
                        {
                            usersList.Add(new User(
                                loadUser,
                                loadEquip
                            ));

                            indexFromUserList = usersList.Count - 1;

                            usersList[indexFromUserList].Shifts = new List<UserShift>();
                        }

                        int indexFromUserListShifts = usersList[indexFromUserList].Shifts.FindIndex(
                                                            (v) => v.ShiftDate == dateShift &&
                                                                   v.ShiftNumber == currentShift);

                        int indexShift = indexFromUserListShifts;

                        if (indexFromUserListShifts == -1)
                        {
                            usersList[indexFromUserList].Shifts.Add(new UserShift(
                            dateShift,
                            currentShift
                            ));

                            indexShift = usersList[indexFromUserList].Shifts.Count - 1;

                            usersList[indexFromUserList].Shifts[indexShift].Orders = new List<UserShiftOrder>();
                        }

                        usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                            sqlReader["order_num"].ToString(),
                            sqlReader["ul_name"].ToString(),
                            Convert.ToInt32(sqlReader["status"]),
                            Convert.ToInt32(sqlReader["flags"]),
                            sqlReader["date_begin"].ToString(),
                            sqlReader["date_end"].ToString(),
                            Convert.ToInt32(sqlReader["duration"]),
                            Convert.ToInt32(sqlReader["fact_out_qty"]),
                            Convert.ToInt32(sqlReader["plan_out_qty"]),
                            Convert.ToInt32(sqlReader["normtime"]),
                            Convert.ToInt32(sqlReader["id_man_order_job_item"])
                        ));
                    }

                    connection.Close();
                }
            }

            return usersList;
        }*/

        /*public List<User> LoadShifts2(List<User> listUsers, DateTime selectDate, int countShifts)
        {
            *//*equipsList = new List<Equips>();*//*
            List<User> usersList = listUsers;

            ValueDateTime timeValues = new ValueDateTime();

            *//*int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);*//*

            int countDaysFromSellectedDate = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            for (int currentDay = 0; currentDay < countDaysFromSellectedDate; currentDay++)
            {
                for (int currentShift = 1; currentShift <= countShifts; currentShift++)
                {
                    DateTime currentDate = selectDate.AddDays(currentDay);

                    string dateShift = currentDate.ToString("dd.MM.yyyy");

                    string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDate(currentDate, currentShift);
                    string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDate(currentDate, currentShift);

                    using (SqlConnection connection = DBConnection.GetDBConnection())
                    {
                        connection.Open();
                        SqlCommand Command = new SqlCommand
                        {
                            Connection = connection,

                            CommandText =
                                @"SELECT
	                                    order_head.id_order_head, 
	                                    man_planjob.id_man_planjob, 
	                                    man_planjob.status,  
                                        man_factjob.id_common_employee,
                                        man_factjob.id_equip,
	                                    common_equip_directory.equip_name, 
	                                    man_factjob.shift_num, 
	                                    order_head.order_num, 
	                                    common_ul_directory.ul_name, 
	                                    order_head.order_name, 
	                                    man_factjob.date_begin, 
	                                    man_factjob.date_end, 
	                                    man_factjob.duration, 
	                                    man_factjob.fact_out_qty, 
	                                    man_factjob.flags, 
	                                    man_planjob_list.plan_out_qty, 
	                                    man_planjob_list.normtime, 
	                                    man_factjob.norm_time, 
	                                    man_factjob.id_man_factjob, 
	                                    man_planjob_list.id_norm_operation, 
	                                    man_planjob_list.id_man_order_job_item, 
	                                    man_planjob_list.id_man_planjob_list
                                    FROM
	                                    dbo.man_factjob
                                    INNER JOIN
	                                dbo.man_planjob_list
	                                ON 
		                                man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
	                                RIGHT JOIN
	                                dbo.fbc_brigade
	                                ON 
		                                man_factjob.id_fbc_brigade = fbc_brigade.id_fbc_brigade
	                                RIGHT JOIN
	                                dbo.common_employee
	                                ON 
		                                man_factjob.id_common_employee = common_employee.id_common_employee
	                                INNER JOIN
	                                dbo.common_equip_directory
	                                ON 
		                                man_factjob.id_equip = common_equip_directory.id_common_equip_directory
	                                LEFT JOIN
	                                dbo.man_order_job_item
	                                ON 
		                                man_planjob_list.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                                INNER JOIN
	                                dbo.man_order_job
	                                ON 
		                                man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                                INNER JOIN
	                                dbo.order_head
	                                ON 
		                                man_order_job.id_order_head = order_head.id_order_head
	                                LEFT JOIN
	                                dbo.common_ul_directory
	                                ON 
		                                order_head.id_customer = common_ul_directory.id_common_ul_directory
	                                LEFT JOIN
	                                dbo.man_planjob
	                                ON 
		                                man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item
                                    WHERE
                                        man_factjob.date_begin IS NOT NULL AND 
	                                    man_factjob.date_begin >= CONVERT ( VARCHAR ( 32 ), @startDate, 21 ) AND
	                                    man_factjob.date_begin <= CONVERT ( VARCHAR ( 32 ), @endDate, 21 ) AND
                                        man_factjob.shift_num = @shiftNum AND
                                        man_factjob.id_common_employee IS NOT NULL AND
                                        man_factjob.id_equip IS NOT NULL AND
                                        man_factjob.fact_out_qty IS NOT NULL AND
                                        man_planjob_list.normtime IS NOT NULL AND
                                        man_planjob_list.plan_out_qty IS NOT NULL"
                        };
                        Command.Parameters.AddWithValue("@startDate", startDateTime);
                        Command.Parameters.AddWithValue("@endDate", endDateTime);
                        Command.Parameters.AddWithValue("@shiftNum", currentShift);

                        DbDataReader sqlReader = Command.ExecuteReader();

                        while (sqlReader.Read())
                        {
                            int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);
                            int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);

                            int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser &&
                                                                               v.Equip == loadEquip);

                            if (indexFromUserList != -1)
                            {
                                int indexFromUserListShifts = usersList[indexFromUserList].Shifts.FindIndex(
                                                                (v) => v.ShiftDate == dateShift &&
                                                                       v.ShiftNumber == currentShift);

                                int indexShift = indexFromUserListShifts;

                                if (indexFromUserListShifts == -1)
                                {
                                    usersList[indexFromUserList].Shifts.Add(new UserShift(
                                    dateShift,
                                    currentShift
                                    ));

                                    indexShift = usersList[indexFromUserList].Shifts.Count - 1;

                                    usersList[indexFromUserList].Shifts[indexShift].Orders = new List<UserShiftOrder>();
                                }

                                usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                                    sqlReader["order_num"].ToString(),
                                    sqlReader["order_name"].ToString(),
                                    Convert.ToInt32(sqlReader["status"]),
                                    Convert.ToInt32(sqlReader["flags"]),
                                    sqlReader["date_begin"].ToString(),
                                    sqlReader["date_end"].ToString(),
                                    Convert.ToInt32(sqlReader["duration"]),
                                    Convert.ToInt32(sqlReader["fact_out_qty"]),
                                    Convert.ToInt32(sqlReader["plan_out_qty"]),
                                    Convert.ToInt32(sqlReader["normtime"]),
                                    Convert.ToInt32(sqlReader["id_man_order_job_item"])
                                ));
                            }
                        }

                        connection.Close();
                    }
                }
            }

            return usersList;
        }*/
    }
}
