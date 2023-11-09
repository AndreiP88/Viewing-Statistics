﻿using libData;
using libTime;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;


namespace libSql
{
    public class ValueShifts
    {
        public ValueShifts()
        {
            
        }
        public List<User> LoadShiftsForSelectedMonth(List<User> listUsers, DateTime selectDate, int countShifts, bool givenShiftNumber = true)
        {
            List<User> usersList = listUsers;

            ValueDateTime timeValues = new ValueDateTime();

            int countDaysFromSellectedDate = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            for (int currentDay = 0; currentDay < countDaysFromSellectedDate; currentDay++)
            {
                for (int currentShift = 1; currentShift <= countShifts; currentShift++)
                {
                    DateTime currentDate = selectDate.AddDays(currentDay);

                    List<User> loadedList = LoadOrders(currentDate, currentShift, givenShiftNumber);

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

        /// <summary>
        /// Загрузка смен за указанный период
        /// </summary>
        /// <param name="listUsers"></param>
        /// <param name="selectDate"></param>
        /// <param name="countShifts"></param>
        /// <returns></returns>
        public List<User> LoadShifts(List<User> listUsers, DateTime selectDate, int countShifts, bool givenShiftNumber = true)
        {
            List<User> usersList = listUsers;

            DateTime date = DateTime.Now;

            int period = (date - selectDate).Days;

            if (date.Hour < 8)
            {
                period++;
            }

            for (int currentDay = 0; currentDay <= period; currentDay++)
            {
                for (int currentShift = 1; currentShift <= countShifts; currentShift++)
                {
                    DateTime currentDate = selectDate.AddDays(currentDay);

                    List<User> loadedList = LoadOrders(currentDate, currentShift, givenShiftNumber);

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

        public List<User> LoadOrders(DateTime currentDate, int currentShift, bool givenShiftNumber = true)
        {
            //List<User> usersList = LoadOrdersFromFactjob(currentDate, currentShift, givenShiftNumber);
            //List<User> usersList = LoadOrdersForFBC(currentDate, currentShift, givenShiftNumber);
            List<User> usersList = LoadOrdersFromFBCBrigade(currentDate, currentShift, givenShiftNumber);

            return usersList;
        }

        public List<User> LoadOrdersFromFBCBrigade(DateTime currentDate, int currentShift, bool givenShiftNumber = true)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<User> usersList = new List<User>();

            string dateShift = currentDate.ToString("dd.MM.yyyy");

            string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);
            string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);

            string cLine = @"AND fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                             AND fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) 
                             AND fbc_brigade.shift_no = @shiftNum ";

            if (!givenShiftNumber)
            {
                startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);
                endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);

                cLine = @"AND fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                          AND fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) ";
            }

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
	                        man_planjob_list.id_man_planjob_list, 
                            man_factjob.id_fbc_brigade,
	                        idletime_directory.idletime_name
                        FROM
	                        dbo.fbc_brigade
	                        FULL OUTER JOIN
	                        dbo.man_factjob
	                        ON 
		                        (
			                        man_factjob.date_begin >= fbc_brigade.date_begin AND
			                        man_factjob.date_begin <= ISNULL( fbc_brigade.date_end, GETDATE( ) ) AND
			                        man_factjob.id_common_employee = fbc_brigade.id_common_employee
		                        )
	                        INNER JOIN
	                        dbo.man_planjob_list
	                        ON 
		                        man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
	                        INNER JOIN
	                        dbo.man_order_job_item
	                        ON 
		                        man_planjob_list.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                        FULL OUTER JOIN
	                        dbo.man_planjob
	                        ON 
		                        man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item
	                        FULL OUTER JOIN
	                        dbo.man_idletime
	                        ON 
		                        man_order_job_item.id_man_order_job = man_idletime.id_man_order_job
	                        INNER JOIN
	                        dbo.man_order_job
	                        ON 
		                        man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                        FULL OUTER JOIN
	                        dbo.order_head
	                        ON 
		                        man_order_job.id_order_head = order_head.id_order_head
	                        LEFT JOIN
	                        dbo.idletime_directory
	                        ON 
		                        man_idletime.id_idletime = idletime_directory.id_idletime_directory
	                        FULL OUTER JOIN
	                        dbo.common_ul_directory
	                        ON 
		                        order_head.id_customer = common_ul_directory.id_common_ul_directory
                        WHERE
                            man_factjob.id_equip IS NOT NULL AND
                            man_factjob.date_begin IS NOT NULL " +
                            cLine +
                        @"ORDER BY
	                        man_factjob.date_begin ASC"
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

                    string orderNum = "";
                    if (!DBNull.Value.Equals(sqlReader["order_num"]))
                    {
                        orderNum = sqlReader["order_num"].ToString();
                    }

                    string ulName = "";
                    if (!DBNull.Value.Equals(sqlReader["ul_name"]))
                    {
                        ulName = sqlReader["ul_name"].ToString();
                    }

                    int factOut = 0;
                    if (!DBNull.Value.Equals(sqlReader["fact_out_qty"]))
                    {
                        factOut = Convert.ToInt32(sqlReader["fact_out_qty"]);
                    }

                    int planOut = 0;
                    if (!DBNull.Value.Equals(sqlReader["plan_out_qty"]))
                    {
                        planOut = Convert.ToInt32(sqlReader["plan_out_qty"]);
                    }

                    int normTime = 0;
                    if (!DBNull.Value.Equals(sqlReader["normtime"]))
                    {
                        normTime = Convert.ToInt32(sqlReader["normtime"]);
                    }

                    int idFBCBrigade = -1;
                    if (!DBNull.Value.Equals(sqlReader["id_fbc_brigade"]))
                    {
                        idFBCBrigade = Convert.ToInt32(sqlReader["id_fbc_brigade"]);
                    }

                    /*if (DBNull.Value.Equals(sqlReader["id_norm_operation"]))
                    {
                        ulName = sqlReader["idletime_name"].ToString();
                    }*/

                    string idletimeName = "";
                    if (!DBNull.Value.Equals(sqlReader["idletime_name"]))
                    {
                        idletimeName = sqlReader["idletime_name"].ToString();
                    }

                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        orderNum,
                        ulName,
                        Convert.ToInt32(sqlReader["status"]),
                        Convert.ToInt32(sqlReader["flags"]),
                        sqlReader["date_begin"].ToString(),
                        sqlReader["date_end"].ToString(),
                        Convert.ToInt32(sqlReader["duration"]),
                        //Convert.ToInt32(sqlReader["fact_out_qty"]),
                        factOut,
                        planOut,
                        normTime,
                        Convert.ToInt32(sqlReader["id_man_order_job_item"]),
                        idFBCBrigade,
                        idletimeName
                    ));
                }
                connection.Close();
            }
            return usersList;
        }

        public List<User> LoadOrdersFromFactjob(DateTime currentDate, int currentShift, bool givenShiftNumber = true)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<User> usersList = new List<User>();

            string dateShift = currentDate.ToString("dd.MM.yyyy");

            string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDate(currentDate, currentShift);
            string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDate(currentDate, currentShift);

            string cLine = @"man_factjob.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) AND 
                             man_factjob.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) AND
                             man_factjob.shift_num = @shiftNum AND ";

            if (!givenShiftNumber)
            {
                startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateOnlyTime(currentDate, currentShift);
                endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateOnlyTime(currentDate, currentShift);
                //почему date_end???
                cLine = @"man_factjob.date_end >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) AND 
                          man_factjob.date_end <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) AND ";
            }

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
	                        man_planjob_list.id_man_planjob_list,
                            man_factjob.id_fbc_brigade
                        FROM
	                        dbo.man_factjob
                        INNER JOIN
	                        dbo.man_planjob_list
	                    ON 
		                    man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
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
                            man_factjob.date_begin IS NOT NULL AND " +
                            cLine +
                            @"man_factjob.id_common_employee IS NOT NULL AND
                            man_factjob.id_equip IS NOT NULL AND
                            --man_factjob.fact_out_qty IS NOT NULL AND
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

                    int factOut = 0;

                    if (!DBNull.Value.Equals(sqlReader["fact_out_qty"]))
                    {
                        factOut = Convert.ToInt32(sqlReader["fact_out_qty"]);
                    }

                    int idFBCBrigade = -1;

                    if (!DBNull.Value.Equals(sqlReader["id_fbc_brigade"]))
                    {
                        idFBCBrigade = Convert.ToInt32(sqlReader["id_fbc_brigade"]);
                    }

                    string idletimeName = "";
                    if (!DBNull.Value.Equals(sqlReader["idletime_name"]))
                    {
                        idletimeName = sqlReader["idletime_name"].ToString();
                    }

                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        sqlReader["order_num"].ToString(),
                        sqlReader["ul_name"].ToString(),
                        Convert.ToInt32(sqlReader["status"]),
                        Convert.ToInt32(sqlReader["flags"]),
                        sqlReader["date_begin"].ToString(),
                        sqlReader["date_end"].ToString(),
                        Convert.ToInt32(sqlReader["duration"]),
                        //Convert.ToInt32(sqlReader["fact_out_qty"]),
                        factOut,
                        Convert.ToInt32(sqlReader["plan_out_qty"]),
                        Convert.ToInt32(sqlReader["normtime"]),
                        Convert.ToInt32(sqlReader["id_man_order_job_item"]),
                        idFBCBrigade,
                        idletimeName
                    ));
                }

                connection.Close();
            }

            return usersList;
        }

        public List<User> LoadOrdersForFBC(DateTime currentDate, int currentShift, bool givenShiftNumber = true)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<User> usersList = new List<User>();

            string dateShift = currentDate.ToString("dd.MM.yyyy");

            string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);
            string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);

            string cLine = @"fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) AND 
                             fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) AND 
                             fbc_brigade.shift_no = @shiftNum AND ";

            if (!givenShiftNumber)
            {
                startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);
                endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);

                cLine = @"fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) AND 
                          fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) AND ";
            }

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
	                        dbo.fbc_brigade
	                        INNER JOIN
	                        dbo.man_factjob
	                        ON 
		                        man_factjob.id_fbc_brigade = fbc_brigade.id_fbc_brigade
	                        INNER JOIN
	                        dbo.man_planjob_list
	                        ON 
		                        man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
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
                            man_factjob.date_begin IS NOT NULL AND " +
                            cLine +
                            @"man_factjob.id_common_employee IS NOT NULL AND
                            man_factjob.id_equip IS NOT NULL AND
                            --man_factjob.fact_out_qty IS NOT NULL AND
                            man_planjob_list.normtime IS NOT NULL AND
                            man_planjob_list.plan_out_qty IS NOT NULL
                        ORDER BY
                            man_factjob.date_begin"
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

                    int factOut = 0;

                    if (!DBNull.Value.Equals(sqlReader["fact_out_qty"]))
                    {
                        factOut = Convert.ToInt32(sqlReader["fact_out_qty"]);
                    }

                    int idFBCBrigade = -1;

                    if (!DBNull.Value.Equals(sqlReader["id_fbc_brigade"]))
                    {
                        idFBCBrigade = Convert.ToInt32(sqlReader["id_fbc_brigade"]);
                    }

                    string idletimeName = "";
                    if (!DBNull.Value.Equals(sqlReader["idletime_name"]))
                    {
                        idletimeName = sqlReader["idletime_name"].ToString();
                    }

                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        sqlReader["order_num"].ToString(),
                        sqlReader["ul_name"].ToString(),
                        Convert.ToInt32(sqlReader["status"]),
                        Convert.ToInt32(sqlReader["flags"]),
                        sqlReader["date_begin"].ToString(),
                        sqlReader["date_end"].ToString(),
                        Convert.ToInt32(sqlReader["duration"]),
                        //Convert.ToInt32(sqlReader["fact_out_qty"]),
                        factOut,
                        Convert.ToInt32(sqlReader["plan_out_qty"]),
                        Convert.ToInt32(sqlReader["normtime"]),
                        Convert.ToInt32(sqlReader["id_man_order_job_item"]),
                        idFBCBrigade,
                        idletimeName
                    ));
                }

                connection.Close();
            }

            return usersList;
        }

        public bool CheckShiftIsActive(int idFBCBrigade)
        {
            bool result = false;

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            date_begin,
                            date_end,
                            shift_no
                        FROM
	                        dbo.fbc_brigade
                        WHERE
                            id_fbc_brigade = @idFBCBrigade"

                };
                Command.Parameters.AddWithValue("@idFBCBrigade", idFBCBrigade);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    if (DBNull.Value.Equals(sqlReader["date_end"]))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }

                connection.Close();
            }

            return result;
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
                            eff_output_coeff IS NOT NULL AND
                            normtime IS NOT NULL"

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

        public int GetAmountDoneFromPreviousShifts(int idManOrderJobItem, DateTime currentDate, int currentShift)
        {
            int result = 0;

            ValueDateTime time = new ValueDateTime();

            string startShiftForDB = time.StartShiftPlanedDateTimeForDataBase(currentDate, currentShift);

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
	                        man_factjob.flags,
	                        man_factjob.fact_out_qty
                        FROM
	                        dbo.man_factjob
                        INNER JOIN
	                        dbo.man_planjob_list
                        ON 
		                        man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
                        WHERE
	                        --man_factjob.id_man_planjob_list = @idManOrderJobItem AND
	                        id_man_order_job_item = @idManOrderJobItem AND
	                        date_begin < CONVERT ( VARCHAR ( 24 ), @startShiftForDB, 21 ) AND
	                        fact_out_qty IS NOT NULL
                        ORDER BY date_begin"

                };
                Command.Parameters.AddWithValue("@startShiftForDB", startShiftForDB);
                Command.Parameters.AddWithValue("@idManOrderJobItem", idManOrderJobItem);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    if (Convert.ToInt32(sqlReader["flags"]) != 576)
                    {
                        result += Convert.ToInt32(sqlReader["fact_out_qty"]);
                    }
                    else
                    {
                        
                    }
                }

                connection.Close();
            }

            return result;
        }

        public int GetAmountDoneFromPreviousShifts(int idManOrderJobItem, string startOrderDateTime)
        {
            int result = 0;

            //ValueDateTime time = new ValueDateTime();

            //string startOrder = time.

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
	                        man_factjob.flags,
	                        man_factjob.fact_out_qty
                        FROM
	                        dbo.man_factjob
                        INNER JOIN
	                        dbo.man_planjob_list
                        ON 
		                        man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
                        WHERE
	                        --man_factjob.id_man_planjob_list = @idManOrderJobItem AND
	                        id_man_order_job_item = @idManOrderJobItem AND
	                        date_begin < CONVERT ( VARCHAR ( 24 ), @startShiftForDB, 21 ) AND
	                        fact_out_qty IS NOT NULL
                        ORDER BY date_begin"

                };
                Command.Parameters.AddWithValue("@startShiftForDB", startOrderDateTime);
                Command.Parameters.AddWithValue("@idManOrderJobItem", idManOrderJobItem);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    if (Convert.ToInt32(sqlReader["flags"]) != 576)
                    {
                        result += Convert.ToInt32(sqlReader["fact_out_qty"]);
                    }
                    else
                    {

                    }
                }

                connection.Close();
            }

            return result;
        }

        //
    }
}
