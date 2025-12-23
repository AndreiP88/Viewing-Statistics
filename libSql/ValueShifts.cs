using libData;
using libTime;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace libSql
{
    public class ValueShifts
    {
        public ValueShifts()
        {
        }
        public async Task<List<User>> LoadShiftsForSelectedMonth(List<User> listUsers, DateTime selectDate, int countShifts, bool givenShiftNumber = true)
        {
            List<User> usersList = listUsers;

            ValueDateTime timeValues = new ValueDateTime();

            int countDaysFromSellectedDate = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            DateTime startMonth = DateTime.MinValue.AddYears(selectDate.Year - 1).AddMonths(selectDate.Month - 1);
            
            for (int currentDay = 0; currentDay < countDaysFromSellectedDate; currentDay++)
            {
                for (int currentShift = 1; currentShift <= countShifts; currentShift++)
                {
                    DateTime currentDate = startMonth.AddDays(currentDay);

                    List<User> loadedList = await LoadOrdersAsync(currentDate, currentShift, givenShiftNumber);
                    
                    for (int i = 0; i < loadedList.Count; i++)
                    {
                        int indexFromUserList = usersList.FindIndex((v) => v.Id == loadedList[i].Id);
                        
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

        /*public List<ShiftsDetails> LoadShiftsForSelectedMonthLight(List<int> listUsers, DateTime selectDate, int countShifts, int shiftNormTime = 650, bool givenShiftNumber = true, List<int> equipListAS = null)
        {
            List<ShiftsDetails> shiftsList = new List<ShiftsDetails>();

            ValueDateTime timeValues = new ValueDateTime();

            int countDaysFromSellectedDate = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            DateTime startMonth = DateTime.MinValue.AddYears(selectDate.Year - 1).AddMonths(selectDate.Month - 1);

            List<DateTime> dateList = new List<DateTime>();

            for (int currentDay = 0; currentDay < countDaysFromSellectedDate; currentDay++)
            {
                for (int currentShift = 1; currentShift <= countShifts; currentShift++)
                {
                    DateTime currentDate = startMonth.AddDays(currentDay);

                    ShiftsDetails loadedShift = LoadOrdersLight(listUsers, currentDate, currentShift, shiftNormTime, givenShiftNumber, equipListAS);

                    shiftsList.Add(loadedShift);
                }
            }

            return shiftsList;
        }*/

        /// <summary>
        /// Получить выработку за выбранный месяц с выборкой только отработанных смен
        /// </summary>
        /// <param name="listUsers"></param>
        /// <param name="selectDate"></param>
        /// <param name="countShifts"></param>
        /// <param name="shiftNormTime"></param>
        /// <param name="givenShiftNumber"></param>
        /// <param name="equipListAS"></param>
        /// <returns></returns>
        public List<ShiftsDetails> LoadShiftsForSelectedMonthFromFBCBrigadeListLight(List<int> listUsers, DateTime selectDate, int countShifts, int shiftNormTime = 650, bool givenShiftNumber = true, List<int> equipListAS = null)
        {
            List<ShiftsDetails> shiftsList = new List<ShiftsDetails>();

            //ValueDateTime timeValues = new ValueDateTime();

            List<ShiftsList> shifts = LoadShiftsList(listUsers, selectDate);

            //int countDaysFromSellectedDate = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            foreach (ShiftsList shift in shifts)
            {
                //ShiftsDetails loadedShift = LoadOrdersLight(listUsers, shift.ShiftDate, shift.ShiftNumber, shiftNormTime, givenShiftNumber, equipListAS);
                ShiftsDetails loadedShift = LoadOrdersLight(listUsers, shift, shiftNormTime, equipListAS);

                shiftsList.Add(loadedShift);
            }

            return shiftsList;
        }

        /// <summary>
        /// Загрузка смен за указанный период
        /// </summary>
        /// <param name="listUsers"></param>
        /// <param name="selectDate"></param>
        /// <param name="countShifts"></param>
        /// <returns></returns>
        public async Task<List<User>> LoadShiftsAsync(List<User> listUsers, DateTime selectDate, int countShifts, bool givenShiftNumber = true)
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

                    List<User> loadedList = await LoadOrdersAsync(currentDate, currentShift, givenShiftNumber);
                    
                    for (int i = 0; i < loadedList.Count; i++)
                    {
                        int indexFromUserList = usersList.FindIndex((v) => v.Id == loadedList[i].Id);

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
        /// Список смен для списка пользователей за указанный месяц
        /// </summary>
        /// <param name="listUsers"></param>
        /// <param name="currentMonth"></param>
        /// <returns></returns>
        /// Сделать загрузку оборудования по умолчанию
        public List<ShiftsList> LoadShiftsList(List<int> listUsers, DateTime currentMonth)
        {
            List<ShiftsList> shiftsList = new List<ShiftsList>();

            DateTime startMonth = DateTime.MinValue.AddYears(currentMonth.Year - 1).AddMonths(currentMonth.Month - 1);
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);

            string startDateTime = startMonth.ToString("yyyy-MM-dd") + "T00:00:00.000";
            string endDateTime = endMonth.ToString("yyyy-MM-dd") + "T23:59:59.000";

            string usersStr = "id_common_employee = " + listUsers[0];

            for (int i = 1; i < listUsers.Count; i++)
            {
                usersStr += " OR id_common_employee = " + listUsers[i];
            }

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
	                        id_common_employee, 
	                        shift_no, 
	                        date_begin, 
	                        date_end,
                            equip_id
                        FROM
	                        dbo.fbc_brigade
                        WHERE 
                            fbc_brigade.date_begin IS NOT NULL AND 
                            fbc_brigade.date_end IS NOT NULL AND 
                            date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                            AND date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) 
                            AND (" + usersStr + @")"
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);


                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    string loadDateBegin = sqlReader["date_begin"] == DBNull.Value ? string.Empty : sqlReader["date_begin"].ToString();
                    DateTime shiftDate = Convert.ToDateTime(loadDateBegin);

                    shiftsList.Add(new ShiftsList(
                        sqlReader["id_common_employee"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["id_common_employee"]),
                        shiftDate,
                        sqlReader["shift_no"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["shift_no"]),
                        sqlReader["date_begin"] == DBNull.Value ? string.Empty : sqlReader["date_begin"].ToString(),
                        sqlReader["date_end"] == DBNull.Value ? string.Empty : sqlReader["date_end"].ToString()
                        ));
                }
                connection.Close();
            }

            return shiftsList;
        }

        public async Task<List<User>> LoadOrdersAsync(DateTime currentDate, int currentShift, bool givenShiftNumber = true, int onlyOneUserID = -1, int onlyOneEquipID = -1)
        {
            //List<User> usersList = LoadOrdersFromFactjob(currentDate, currentShift, givenShiftNumber);
            //List<User> usersList = LoadOrdersForFBC(currentDate, currentShift, givenShiftNumber);
            List<User> usersList = await LoadOrdersFromFBCBrigadeAsync(currentDate, currentShift, givenShiftNumber, onlyOneUserID, onlyOneEquipID);

            return usersList;
        }

        /// <summary>
        /// Загрузка списка заказов для списка смен, игнорируя смены без открытых заказо/простоев
        /// </summary>
        /// <param name="currentDate">Дата смены</param>
        /// <param name="currentShift">Номер смены</param>
        /// <param name="givenShiftNumber">Учитывать номер сены при выборке (не обязательно, по умолчанию = true)</param>
        /// <param name="onlyOneUserID">Указать индекс пользователя (не обязательно, по умолчанию = -1)</param>
        /// <param name="onlyOneEquipID">Указать индекс оборудования (не обязательно, по умолчанию = -1)</param>
        /// <returns>List<User></returns>
        public async Task<List<User>> LoadOrdersFromShiftListAsync(DateTime currentDate, int currentShift, bool givenShiftNumber = true, int onlyOneUserID = -1, int onlyOneEquipID = -1)
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

            if (onlyOneUserID != -1)
            {
                cLine += @"AND man_factjob.id_common_employee = @user ";
            }

            if (onlyOneEquipID != -1)
            {
                cLine += @"AND man_factjob.id_equip = @equip ";
            }

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                await connection.OpenAsync();
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
	                        idletime_directory.idletime_name, 
	                        fbc_brigade.date_begin AS shif_date_begin, 
	                        fbc_brigade.date_end AS shif_date_end, 
	                        norm_operation_table.ord AS operationType, 
	                        LTRIM(RTRIM(REPLACE(CAST(common_note.note AS NVARCHAR(MAX)), '  ', ' '))) AS note, 
	                        man_idletime.idletime_name AS idletimeNote, 
	                        tqm_problem.problem_name, 
	                        man_factjob_problem.cause, 
	                        man_factjob_problem.actions, 
	                        man_factjob_problem.caused_delay, 
	                        idletime_directory.ord
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
	                        LEFT JOIN
	                        dbo.norm_operation_table
	                        ON 
		                        man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
	                        LEFT JOIN
	                        dbo.common_note
	                        ON 
		                        man_factjob.id_man_factjob = common_note.objectid AND
		                        common_note.objecttype = 64
	                        LEFT JOIN
	                        dbo.man_factjob_problem
	                        ON 
		                        man_factjob.id_man_factjob = man_factjob_problem.id_man_factjob
	                        LEFT JOIN
	                        dbo.tqm_problem
	                        ON 
		                        man_factjob_problem.id_tqm_problem = tqm_problem.id_tqm_problem
                        WHERE
                            man_factjob.id_equip IS NOT NULL AND
                            man_factjob.date_begin IS NOT NULL " +
                            cLine +
                        @"ORDER BY
	                        man_factjob.date_begin ASC, man_factjob.id_man_factjob ASC"
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);
                Command.Parameters.AddWithValue("@user", onlyOneUserID);
                Command.Parameters.AddWithValue("@equip", onlyOneEquipID);

                DbDataReader sqlReader = await Command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);
                    int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);

                    string shiftDateBegin = sqlReader["shif_date_begin"].ToString();

                    string shiftDateEnd = "";
                    if (!DBNull.Value.Equals(sqlReader["shif_date_end"]))
                    {
                        shiftDateEnd = sqlReader["shif_date_end"].ToString();
                    }

                    /*int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser &&
                                                                       v.Equip == loadEquip);*/

                    int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser);

                    if (indexFromUserList == -1)
                    {
                        usersList.Add(new User(
                            loadUser
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
                        currentShift,
                        shiftDateBegin,
                        shiftDateEnd
                        ));

                        indexShift = usersList[indexFromUserList].Shifts.Count - 1;

                        usersList[indexFromUserList].Shifts[indexShift].Orders = new List<UserShiftOrder>();

                        if (!usersList[indexFromUserList].Shifts[indexShift].Equips.Contains(loadEquip))
                        {
                            usersList[indexFromUserList].Shifts[indexShift].Equips.Add(loadEquip);
                        }
                    }

                    string orderNum = sqlReader["order_num"] == DBNull.Value ? string.Empty : sqlReader["order_num"].ToString();
                    string ulName = sqlReader["ul_name"] == DBNull.Value ? string.Empty : sqlReader["ul_name"].ToString();
                    float factOut = sqlReader["fact_out_qty"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["fact_out_qty"]);
                    float planOut = sqlReader["plan_out_qty"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["plan_out_qty"]);
                    int normTime = sqlReader["normtime"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["normtime"]);
                    int idFBCBrigade = sqlReader["id_fbc_brigade"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["id_fbc_brigade"]);
                    string idletimeName = sqlReader["idletime_name"] == DBNull.Value ? string.Empty : sqlReader["idletime_name"].ToString();
                    int operationType = sqlReader["operationType"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["operationType"]);

                    string note = sqlReader["note"] == DBNull.Value ? string.Empty : sqlReader["note"].ToString();
                    string idletimeNote = sqlReader["idletimeNote"] == DBNull.Value ? string.Empty : sqlReader["idletimeNote"].ToString();
                    string problemName = sqlReader["problem_name"] == DBNull.Value ? string.Empty : sqlReader["problem_name"].ToString();
                    string problemCause = sqlReader["cause"] == DBNull.Value ? string.Empty : sqlReader["cause"].ToString();
                    string problemAction = sqlReader["actions"] == DBNull.Value ? string.Empty : sqlReader["actions"].ToString();
                    int problemDelay = sqlReader["caused_delay"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["caused_delay"]);

                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        loadEquip,
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
                        idletimeName,
                        operationType,
                        note,
                        idletimeNote,
                        problemName,
                        problemCause,
                        problemAction,
                        problemDelay
                    ));
                }
                connection.Close();
            }
            
            return usersList;
        }
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Сделать загрузку отдельно списка смен с добавлением оборудования
        /// <summary>
        /// Загрузка списка заказов для списка смен, загружая смены без открытых заказо/простоев
        /// Очень долгая загрузка
        /// </summary>
        /// <param name="currentDate">Дата смены</param>
        /// <param name="currentShift">Номер смены</param>
        /// <param name="givenShiftNumber">Учитывать номер сены при выборке (не обязательно, по умолчанию = true)</param>
        /// <param name="onlyOneUserID">Указать индекс пользователя (не обязательно, по умолчанию = -1)</param>
        /// <param name="onlyOneEquipID">Указать индекс оборудования (не обязательно, по умолчанию = -1)</param>
        /// <returns>List<User></returns>
        public async Task<List<User>> LoadOrdersFromFBCBrigadeAsync(DateTime currentDate, int currentShift, bool givenShiftNumber = true, int onlyOneUserID = -1, int onlyOneEquipID = -1)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<User> usersList = new List<User>();

            string dateShift = currentDate.ToString("dd.MM.yyyy");

            string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);
            string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);

            string cLine = @"fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                             AND fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) 
                             AND fbc_brigade.shift_no = @shiftNum ";

            if (!givenShiftNumber)
            {
                startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);
                endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);

                cLine = @"fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                          AND fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) ";
            }

            if (onlyOneUserID != -1)
            {
                cLine += @"AND man_factjob.id_common_employee = @user ";
            }

            if (onlyOneEquipID != -1)
            {
                cLine += @"AND man_factjob.id_equip = @equip ";
            }

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                await connection.OpenAsync();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
	                        order_head.id_order_head, 
	                        man_planjob.id_man_planjob, 
	                        man_planjob.status, 
	                        man_factjob.id_common_employee, 
                            CASE WHEN man_factjob.id_equip IS NOT NULL THEN man_factjob.id_equip ELSE fbc_brigade.equip_id END AS id_equip,
	                        --man_factjob.id_equip, 
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
	                        idletime_directory.idletime_name, 
	                        fbc_brigade.id_common_employee AS shift_user_id, 
	                        fbc_brigade.date_begin AS shif_date_begin, 
	                        fbc_brigade.date_end AS shif_date_end, 
	                        norm_operation_table.ord AS operationType, 
	                        LTRIM(RTRIM(REPLACE(CAST(common_note.note AS NVARCHAR(MAX)), '  ', ' '))) AS note, 
	                        man_idletime.idletime_name AS idletimeNote, 
	                        tqm_problem.problem_name, 
	                        man_factjob_problem.cause, 
	                        man_factjob_problem.actions, 
	                        man_factjob_problem.caused_delay, 
	                        idletime_directory.ord
                        FROM
	                        dbo.fbc_brigade
	                    LEFT JOIN
	                    dbo.man_factjob
	                    ON 
		                    (
			                    man_factjob.date_begin >= fbc_brigade.date_begin AND
			                    man_factjob.date_begin <= ISNULL( fbc_brigade.date_end, GETDATE( ) ) AND
			                    man_factjob.id_common_employee = fbc_brigade.id_common_employee
		                    )
	                    LEFT JOIN
	                    dbo.man_planjob_list
	                    ON 
		                    man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
	                    LEFT JOIN
	                    dbo.man_order_job_item
	                    ON 
		                    man_planjob_list.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                    LEFT JOIN
	                    dbo.common_note
	                    ON 
		                    man_factjob.id_man_factjob = common_note.objectid AND
		                    common_note.objecttype = 64
	                    LEFT JOIN
	                    dbo.man_factjob_problem
	                    ON 
		                    man_factjob.id_man_factjob = man_factjob_problem.id_man_factjob
	                    LEFT JOIN
	                    dbo.man_planjob
	                    ON 
		                    man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item
	                    LEFT JOIN
	                    dbo.man_idletime
	                    ON 
		                    man_order_job_item.id_man_order_job = man_idletime.id_man_order_job
	                    LEFT JOIN
	                    dbo.man_order_job
	                    ON 
		                    man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                    LEFT JOIN
	                    dbo.order_head
	                    ON 
		                    man_order_job.id_order_head = order_head.id_order_head
	                    LEFT JOIN
	                    dbo.idletime_directory
	                    ON 
		                    man_idletime.id_idletime = idletime_directory.id_idletime_directory
	                    LEFT JOIN
	                    dbo.common_ul_directory
	                    ON 
		                    order_head.id_customer = common_ul_directory.id_common_ul_directory
	                    LEFT JOIN
	                    dbo.norm_operation_table
	                    ON 
		                    man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
	                    LEFT JOIN
	                    dbo.tqm_problem
	                    ON 
		                    man_factjob_problem.id_tqm_problem = tqm_problem.id_tqm_problem
                        WHERE " +
                            cLine +
                        @"ORDER BY
	                        man_factjob.date_begin ASC, man_factjob.id_man_factjob ASC"
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);
                Command.Parameters.AddWithValue("@user", onlyOneUserID);
                Command.Parameters.AddWithValue("@equip", onlyOneEquipID);

                DbDataReader sqlReader = await Command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    int loadUser = Convert.ToInt32(sqlReader["shift_user_id"]);
                    //int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);
                    int loadEquip = sqlReader["id_equip"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["id_equip"]);

                    string shiftDateBegin = sqlReader["shif_date_begin"].ToString();

                    string shiftDateEnd = "";
                    if (!DBNull.Value.Equals(sqlReader["shif_date_end"]))
                    {
                        shiftDateEnd = sqlReader["shif_date_end"].ToString();
                    }

                    /*int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser &&
                                                                       v.Equip == loadEquip);*/

                    int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser);

                    if (indexFromUserList == -1)
                    {
                        usersList.Add(new User(
                            loadUser
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
                        currentShift,
                        shiftDateBegin,
                        shiftDateEnd
                        ));

                        indexShift = usersList[indexFromUserList].Shifts.Count - 1;

                        usersList[indexFromUserList].Shifts[indexShift].Orders = new List<UserShiftOrder>();

                        if (!usersList[indexFromUserList].Shifts[indexShift].Equips.Contains(loadEquip))
                        {
                            usersList[indexFromUserList].Shifts[indexShift].Equips.Add(loadEquip);
                        }
                    }

                    string orderNum = sqlReader["order_num"] == DBNull.Value ? string.Empty : sqlReader["order_num"].ToString();
                    string ulName = sqlReader["ul_name"] == DBNull.Value ? string.Empty : sqlReader["ul_name"].ToString();
                    float factOut = sqlReader["fact_out_qty"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["fact_out_qty"]);
                    float planOut = sqlReader["plan_out_qty"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["plan_out_qty"]);
                    int normTime = sqlReader["normtime"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["normtime"]);
                    int idFBCBrigade = sqlReader["id_fbc_brigade"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["id_fbc_brigade"]);
                    string idletimeName = sqlReader["idletime_name"] == DBNull.Value ? string.Empty : sqlReader["idletime_name"].ToString();
                    int operationType = sqlReader["operationType"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["operationType"]);

                    string note = sqlReader["note"] == DBNull.Value ? string.Empty : sqlReader["note"].ToString();
                    string idletimeNote = sqlReader["idletimeNote"] == DBNull.Value ? string.Empty : sqlReader["idletimeNote"].ToString();
                    string problemName = sqlReader["problem_name"] == DBNull.Value ? string.Empty : sqlReader["problem_name"].ToString();
                    string problemCause = sqlReader["cause"] == DBNull.Value ? string.Empty : sqlReader["cause"].ToString();
                    string problemAction = sqlReader["actions"] == DBNull.Value ? string.Empty : sqlReader["actions"].ToString();
                    int problemDelay = sqlReader["caused_delay"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["caused_delay"]);

                    int status = sqlReader["status"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["status"]);
                    int flags = sqlReader["flags"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["flags"]);
                    string date_begin = sqlReader["date_begin"] == DBNull.Value ? string.Empty : sqlReader["date_begin"].ToString();
                    string date_end = sqlReader["date_end"] == DBNull.Value ? string.Empty : sqlReader["date_end"].ToString();
                    int duration = sqlReader["duration"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["duration"]);
                    int id_man_order_job_item = sqlReader["id_man_order_job_item"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["id_man_order_job_item"]);


                    if (loadEquip != -1)
                    {
                        usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        loadEquip,
                        orderNum,
                        ulName,
                        status,
                        flags,
                        date_begin,
                        date_end,
                        duration,
                        //Convert.ToInt32(sqlReader["fact_out_qty"]),
                        factOut,
                        planOut,
                        normTime,
                        id_man_order_job_item,
                        idFBCBrigade,
                        idletimeName,
                        operationType,
                        note,
                        idletimeNote,
                        problemName,
                        problemCause,
                        problemAction,
                        problemDelay
                    ));
                    }
                }
                connection.Close();
            }

            return usersList;
        }

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Версия где загружаются пустые смены, но очееень долго
        /// <summary>
        /// Загрузка списка заказов для списка смен, загружая смены без открытых заказо/простоев
        /// Очень долгая загрузка
        /// </summary>
        /// <param name="currentDate">Дата смены</param>
        /// <param name="currentShift">Номер смены</param>
        /// <param name="givenShiftNumber">Учитывать номер сены при выборке (не обязательно, по умолчанию = true)</param>
        /// <param name="onlyOneUserID">Указать индекс пользователя (не обязательно, по умолчанию = -1)</param>
        /// <param name="onlyOneEquipID">Указать индекс оборудования (не обязательно, по умолчанию = -1)</param>
        /// <returns>List<User></returns>
        public async Task<List<User>> LoadOrdersFromFBCBrigadeAsyncOLD(DateTime currentDate, int currentShift, bool givenShiftNumber = true, int onlyOneUserID = -1, int onlyOneEquipID = -1)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<User> usersList = new List<User>();

            string dateShift = currentDate.ToString("dd.MM.yyyy");

            string startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);
            string endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateForFBC(currentDate, currentShift);

            string cLine = @"fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                             AND fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) 
                             AND fbc_brigade.shift_no = @shiftNum ";

            if (!givenShiftNumber)
            {
                startDateTime = timeValues.SelectStartDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);
                endDateTime = timeValues.SelectEndDateTimeFromShiftNumberAndDateOnlyTimeForFBC(currentDate, currentShift);

                cLine = @"fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                          AND fbc_brigade.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) ";
            }

            if (onlyOneUserID != -1)
            {
                cLine += @"AND man_factjob.id_common_employee = @user ";
            }

            if (onlyOneEquipID != -1)
            {
                cLine += @"AND man_factjob.id_equip = @equip ";
            }

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                await connection.OpenAsync();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
	                        order_head.id_order_head, 
	                        man_planjob.id_man_planjob, 
	                        man_planjob.status, 
	                        man_factjob.id_common_employee, 
                            CASE WHEN man_factjob.id_equip IS NOT NULL THEN man_factjob.id_equip ELSE fbc_brigade.equip_id END AS id_equip,
	                        --man_factjob.id_equip, 
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
	                        idletime_directory.idletime_name, 
	                        fbc_brigade.id_common_employee AS shift_user_id, 
	                        fbc_brigade.date_begin AS shif_date_begin, 
	                        fbc_brigade.date_end AS shif_date_end, 
	                        norm_operation_table.ord AS operationType, 
	                        LTRIM(RTRIM(REPLACE(CAST(common_note.note AS NVARCHAR(MAX)), '  ', ' '))) AS note, 
	                        man_idletime.idletime_name AS idletimeNote, 
	                        tqm_problem.problem_name, 
	                        man_factjob_problem.cause, 
	                        man_factjob_problem.actions, 
	                        man_factjob_problem.caused_delay, 
	                        idletime_directory.ord
                        FROM
	                        dbo.fbc_brigade
	                    LEFT JOIN
	                    dbo.man_factjob
	                    ON 
		                    (
			                    man_factjob.date_begin >= fbc_brigade.date_begin AND
			                    man_factjob.date_begin <= ISNULL( fbc_brigade.date_end, GETDATE( ) ) AND
			                    man_factjob.id_common_employee = fbc_brigade.id_common_employee
		                    )
	                    LEFT JOIN
	                    dbo.man_planjob_list
	                    ON 
		                    man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
	                    LEFT JOIN
	                    dbo.man_order_job_item
	                    ON 
		                    man_planjob_list.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                    LEFT JOIN
	                    dbo.common_note
	                    ON 
		                    man_factjob.id_man_factjob = common_note.objectid AND
		                    common_note.objecttype = 64
	                    LEFT JOIN
	                    dbo.man_factjob_problem
	                    ON 
		                    man_factjob.id_man_factjob = man_factjob_problem.id_man_factjob
	                    LEFT JOIN
	                    dbo.man_planjob
	                    ON 
		                    man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item
	                    LEFT JOIN
	                    dbo.man_idletime
	                    ON 
		                    man_order_job_item.id_man_order_job = man_idletime.id_man_order_job
	                    LEFT JOIN
	                    dbo.man_order_job
	                    ON 
		                    man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                    LEFT JOIN
	                    dbo.order_head
	                    ON 
		                    man_order_job.id_order_head = order_head.id_order_head
	                    LEFT JOIN
	                    dbo.idletime_directory
	                    ON 
		                    man_idletime.id_idletime = idletime_directory.id_idletime_directory
	                    LEFT JOIN
	                    dbo.common_ul_directory
	                    ON 
		                    order_head.id_customer = common_ul_directory.id_common_ul_directory
	                    LEFT JOIN
	                    dbo.norm_operation_table
	                    ON 
		                    man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
	                    LEFT JOIN
	                    dbo.tqm_problem
	                    ON 
		                    man_factjob_problem.id_tqm_problem = tqm_problem.id_tqm_problem
                        WHERE " +
                            cLine +
                        @"ORDER BY
	                        man_factjob.date_begin ASC, man_factjob.id_man_factjob ASC"
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);
                Command.Parameters.AddWithValue("@user", onlyOneUserID);
                Command.Parameters.AddWithValue("@equip", onlyOneEquipID);

                DbDataReader sqlReader = await Command.ExecuteReaderAsync();

                while (await sqlReader.ReadAsync())
                {
                    int loadUser = Convert.ToInt32(sqlReader["shift_user_id"]);
                    //int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);
                    int loadEquip = sqlReader["id_equip"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["id_equip"]);

                    string shiftDateBegin = sqlReader["shif_date_begin"].ToString();

                    string shiftDateEnd = "";
                    if (!DBNull.Value.Equals(sqlReader["shif_date_end"]))
                    {
                        shiftDateEnd = sqlReader["shif_date_end"].ToString();
                    }

                    /*int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser &&
                                                                       v.Equip == loadEquip);*/

                    int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser);

                    if (indexFromUserList == -1)
                    {
                        usersList.Add(new User(
                            loadUser
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
                        currentShift,
                        shiftDateBegin,
                        shiftDateEnd
                        ));

                        indexShift = usersList[indexFromUserList].Shifts.Count - 1;

                        usersList[indexFromUserList].Shifts[indexShift].Orders = new List<UserShiftOrder>();

                        if (!usersList[indexFromUserList].Shifts[indexShift].Equips.Contains(loadEquip))
                        {
                            usersList[indexFromUserList].Shifts[indexShift].Equips.Add(loadEquip);
                        }
                    }

                    string orderNum = sqlReader["order_num"] == DBNull.Value ? string.Empty : sqlReader["order_num"].ToString();
                    string ulName = sqlReader["ul_name"] == DBNull.Value ? string.Empty : sqlReader["ul_name"].ToString();
                    float factOut = sqlReader["fact_out_qty"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["fact_out_qty"]);
                    float planOut = sqlReader["plan_out_qty"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["plan_out_qty"]);
                    int normTime = sqlReader["normtime"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["normtime"]);
                    int idFBCBrigade = sqlReader["id_fbc_brigade"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["id_fbc_brigade"]);
                    string idletimeName = sqlReader["idletime_name"] == DBNull.Value ? string.Empty : sqlReader["idletime_name"].ToString();
                    int operationType = sqlReader["operationType"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["operationType"]);

                    string note = sqlReader["note"] == DBNull.Value ? string.Empty : sqlReader["note"].ToString();
                    string idletimeNote = sqlReader["idletimeNote"] == DBNull.Value ? string.Empty : sqlReader["idletimeNote"].ToString();
                    string problemName = sqlReader["problem_name"] == DBNull.Value ? string.Empty : sqlReader["problem_name"].ToString();
                    string problemCause = sqlReader["cause"] == DBNull.Value ? string.Empty : sqlReader["cause"].ToString();
                    string problemAction = sqlReader["actions"] == DBNull.Value ? string.Empty : sqlReader["actions"].ToString();
                    int problemDelay = sqlReader["caused_delay"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["caused_delay"]);

                    int status = sqlReader["status"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["status"]);
                    int flags = sqlReader["flags"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["flags"]);
                    string date_begin = sqlReader["date_begin"] == DBNull.Value ? string.Empty : sqlReader["date_begin"].ToString();
                    string date_end = sqlReader["date_end"] == DBNull.Value ? string.Empty : sqlReader["date_end"].ToString();
                    int duration = sqlReader["duration"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["duration"]);
                    int id_man_order_job_item = sqlReader["id_man_order_job_item"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["id_man_order_job_item"]);


                    if (loadEquip != -1)
                    {
                        usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        loadEquip,
                        orderNum,
                        ulName,
                        status,
                        flags,
                        date_begin,
                        date_end,
                        duration,
                        //Convert.ToInt32(sqlReader["fact_out_qty"]),
                        factOut,
                        planOut,
                        normTime,
                        id_man_order_job_item,
                        idFBCBrigade,
                        idletimeName,
                        operationType,
                        note,
                        idletimeNote,
                        problemName,
                        problemCause,
                        problemAction,
                        problemDelay
                    ));
                    }
                }
                connection.Close();
            }

            return usersList;
        }

        /*public ShiftsDetails LoadOrdersLight(List<int> listUsers, DateTime currentDate, int currentShift, int shiftNormTime = 650, bool givenShiftNumber = true, List<int> equipListAS = null)
        {
            //List<User> usersList = LoadOrdersFromFactjob(currentDate, currentShift, givenShiftNumber);
            //List<User> usersList = LoadOrdersForFBC(currentDate, currentShift, givenShiftNumber);
            //ShiftsDetails shiftsList = LoadOrdersFromFBCBrigadeLight(listUsers, currentDate, currentShift, shiftNormTime, givenShiftNumber, equipListAS);
            ShiftsDetails shiftsList = LoadOrdersFromFBCBrigadeLight(listUsers, currentDate, currentShift, shiftNormTime, givenShiftNumber, equipListAS);

            return shiftsList;
        }*/

        public ShiftsDetails LoadOrdersLight(List<int> listUsers, ShiftsList currentShift, int shiftNormTime, List<int> equipListAS = null)
        {
            //List<User> usersList = LoadOrdersFromFactjob(currentDate, currentShift, givenShiftNumber);
            //List<User> usersList = LoadOrdersForFBC(currentDate, currentShift, givenShiftNumber);
            //ShiftsDetails shiftsList = LoadOrdersFromFBCBrigadeLight(listUsers, currentDate, currentShift, shiftNormTime, givenShiftNumber, equipListAS);
            
            
            //ShiftsDetails shiftsList = LoadOrdersFromFBCBrigadeLight(listUsers, currentShift, shiftNormTime, equipListAS);
            ShiftsDetails shiftsList = LoadOrdersFromShiftsListLight(listUsers, currentShift, shiftNormTime, equipListAS);
            
            return shiftsList;
        }

        /*public ShiftsDetails LoadOrdersFromFBCBrigadeLight(List<int> listUsers, ShiftsList currentShift, int shiftNormTime, List<int> equipListAS = null)
        {
            ValueDateTime timeValues = new ValueDateTime();
            CalculateWorkingOutput calculateWorking = new CalculateWorkingOutput();

            int isShiftActive = 0;
            int workingAmount = 0;
            float workingOutput = -1;
            float workingPercent = -1;
            int makereadyCount = -1;
            float makeReadyWorkTime = -1;
            float bonus = -1;

            string dateShift = currentShift.ShiftDate.ToString("dd.MM.yyyy");

            string startDateTime = currentShift.ShiftDateBegin;
            string endDateTime = currentShift.ShiftDateEnd;
            int shiftNumber = currentShift.ShiftNumber;

            string cLine = @"AND fbc_brigade.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                             AND fbc_brigade.date_end <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) 
                             AND fbc_brigade.shift_no = @shiftNum ";

            string usersStr = "";

            if (listUsers != null)
            {
                usersStr = "AND (man_factjob.id_common_employee = " + listUsers[0];

                for (int i = 1; i < listUsers.Count; i++)
                {
                    usersStr += " OR man_factjob.id_common_employee = " + listUsers[i];
                }

                usersStr += ")";
            }

            //string usersStr = "man_factjob.id_common_employee = " + userId;

            string equipsStr = "";

            if (equipListAS != null)
            {
                equipsStr = "AND (man_factjob.id_equip = " + equipListAS[0];

                for (int i = 1; i < equipListAS.Count; i++)
                {
                    equipsStr += " OR man_factjob.id_equip = " + equipListAS[i];
                }

                equipsStr += ")";
            }

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            SUM(CASE WHEN norm_operation_table.ord = 1 THEN fact_out_qty ELSE 0 END) AS amount,
	                        SUM(CASE WHEN plan_out_qty = 0 THEN CASE WHEN fact_out_qty = 1 THEN fact_out_qty * normtime END ELSE fact_out_qty * normtime / plan_out_qty END) AS workingOutput,
	                        SUM(CASE WHEN plan_out_qty = 0 THEN CASE WHEN fact_out_qty = 1 THEN fact_out_qty * normtime END ELSE fact_out_qty * normtime / plan_out_qty END) / @shiftNormTime AS workingPercent,
	                        --SUM(fact_out_qty * normtime / plan_out_qty) / @shiftNormTime AS workingPercent,
                            SUM(CASE WHEN norm_operation_table.ord = 0 THEN 1 ELSE 0 END) AS makereadyCount,
                            SUM ( CASE WHEN norm_operation_table.ord = 0 THEN fact_out_qty * normtime / plan_out_qty ELSE 0 END ) AS makereadyWorkTime,
                            CASE WHEN (COUNT (id_man_factjob)) > 0 THEN 1 ELSE 0 END AS isShiftActive
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
	                    LEFT JOIN
	                    dbo.norm_operation_table
	                    ON 
		                    man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
                        WHERE
	                        fbc_brigade.date_begin IS NOT NULL 
                            AND fbc_brigade.date_end IS NOT NULL "
                            + cLine
                            + usersStr 
                            + equipsStr
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", shiftNumber);
                Command.Parameters.AddWithValue("@shiftNormTime", shiftNormTime);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    workingAmount = sqlReader["amount"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["amount"]);
                    workingOutput = sqlReader["workingOutput"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["workingOutput"]);
                    workingPercent = sqlReader["workingPercent"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["workingPercent"]);
                    makereadyCount = sqlReader["makereadyCount"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["makereadyCount"]);
                    makeReadyWorkTime = sqlReader["makereadyWorkTime"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["makereadyWorkTime"]);
                    isShiftActive = sqlReader["isShiftActive"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["isShiftActive"]);
                }
                connection.Close();
            }

            bonus = calculateWorking.GetBonusWorkingOutF((int)workingOutput);

            *//*if (listUsers[0] == 112)
        {
            Console.WriteLine(dateShift + " workingOutput: " + timeValues.MinuteToTimeString((int)workingOutput) + " (" + workingOutput + ")" + ". workingPercent: " + workingPercent * 100);
        }*//*

            ShiftsDetails shiftsDetails = new ShiftsDetails(
                    isShiftActive,
                    -1,
                    -1,
                    workingOutput,
                    -1,
                    makereadyCount,
                    makeReadyWorkTime,
                    workingAmount,
                    workingPercent,
                    bonus
                    );

            return shiftsDetails;
        }*/

        public ShiftsDetails LoadOrdersFromShiftsListLight(List<int> listUsers, ShiftsList currentShift, int shiftNormTime, List<int> equipListAS = null)
        {
            ValueDateTime timeValues = new ValueDateTime();
            CalculateWorkingOutput calculateWorking = new CalculateWorkingOutput();

            int isShiftActive = 0;
            int workingAmount = 0;
            float workingOutput = -1;
            float workingPercent = -1;
            int makereadyCount = -1;
            float makeReadyWorkTime = -1;
            float bonus = -1;

            string dateShift = currentShift.ShiftDate.ToString("dd.MM.yyyy");

            string startDateTime = currentShift.ShiftDateBegin;
            string endDateTime = currentShift.ShiftDateEnd;
            int shiftNumber = currentShift.ShiftNumber;

            string cLine = @"man_factjob.date_begin >= CONVERT ( VARCHAR ( 24 ), @startDate, 21 ) 
                             AND man_factjob.date_begin <= CONVERT ( VARCHAR ( 24 ), @endDate, 21 ) ";

            string usersStr = "";

            if (listUsers != null)
            {
                usersStr = "AND (man_factjob.id_common_employee = " + listUsers[0];

                for (int i = 1; i < listUsers.Count; i++)
                {
                    usersStr += " OR man_factjob.id_common_employee = " + listUsers[i];
                }

                usersStr += ")";
            }

            //string usersStr = "man_factjob.id_common_employee = " + userId;

            string equipsStr = "";

            if (equipListAS != null)
            {
                equipsStr = "AND (man_factjob.id_equip = " + equipListAS[0];

                for (int i = 1; i < equipListAS.Count; i++)
                {
                    equipsStr += " OR man_factjob.id_equip = " + equipListAS[i];
                }

                equipsStr += ")";
            }

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            SUM(CASE WHEN norm_operation_table.ord = 1 THEN fact_out_qty ELSE 0 END) AS amount,
	                        SUM(CASE WHEN plan_out_qty = 0 THEN CASE WHEN fact_out_qty = 1 THEN fact_out_qty * normtime END ELSE fact_out_qty * normtime / plan_out_qty END) AS workingOutput,
	                        SUM(CASE WHEN plan_out_qty = 0 THEN CASE WHEN fact_out_qty = 1 THEN fact_out_qty * normtime END ELSE fact_out_qty * normtime / plan_out_qty END) / @shiftNormTime AS workingPercent,
	                        --SUM(fact_out_qty * normtime / plan_out_qty) / @shiftNormTime AS workingPercent,
                            SUM(CASE WHEN norm_operation_table.ord = 0 THEN 1 ELSE 0 END) AS makereadyCount,
                            SUM ( CASE WHEN norm_operation_table.ord = 0 THEN fact_out_qty * normtime / plan_out_qty ELSE 0 END ) AS makereadyWorkTime,
                            CASE WHEN (COUNT (id_man_factjob)) > 0 THEN 1 ELSE 0 END AS isShiftActive
                        FROM
	                        dbo.man_factjob
	                    INNER JOIN
	                        dbo.man_planjob_list
	                    ON 
		                    man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
	                    LEFT JOIN
	                        dbo.norm_operation_table
	                    ON 
		                    man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
                        WHERE "
                            + cLine
                            + usersStr
                            + equipsStr
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", shiftNumber);
                Command.Parameters.AddWithValue("@shiftNormTime", shiftNormTime);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    workingAmount = sqlReader["amount"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["amount"]);
                    workingOutput = sqlReader["workingOutput"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["workingOutput"]);
                    workingPercent = sqlReader["workingPercent"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["workingPercent"]);
                    makereadyCount = sqlReader["makereadyCount"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["makereadyCount"]);
                    makeReadyWorkTime = sqlReader["makereadyWorkTime"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["makereadyWorkTime"]);
                    isShiftActive = sqlReader["isShiftActive"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["isShiftActive"]);
                }
                connection.Close();
            }

            bonus = calculateWorking.GetBonusWorkingOutF((int)workingOutput);

            /*if (listUsers[0] == 112)
        {
            Console.WriteLine(dateShift + " workingOutput: " + timeValues.MinuteToTimeString((int)workingOutput) + " (" + workingOutput + ")" + ". workingPercent: " + workingPercent * 100);
        }*/

            ShiftsDetails shiftsDetails = new ShiftsDetails(
                    isShiftActive,
                    -1,
                    -1,
                    workingOutput,
                    -1,
                    makereadyCount,
                    makeReadyWorkTime,
                    workingAmount,
                    workingPercent,
                    bonus
                    );

            return shiftsDetails;
        }

        public ShiftsDetails LoadOrdersFromFBCBrigadeLight(List<int> listUsers, DateTime currentDate, int currentShift, int shiftNormTime = 650, bool givenShiftNumber = true, List<int> equipListAS = null)
        {
            ValueDateTime timeValues = new ValueDateTime();
            CalculateWorkingOutput calculateWorking = new CalculateWorkingOutput();

            int isShiftActive = 0;
            int workingAmount = 0;
            float workingOutput = -1;
            float workingPercent = -1;
            int makereadyCount = -1;
            float makeReadyWorkTime = -1;
            float bonus = -1;

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

            string usersStr = "man_factjob.id_common_employee = " + listUsers[0];

            for (int i = 1; i < listUsers.Count; i++)
            {
                usersStr += " OR man_factjob.id_common_employee = " + listUsers[i];
            }

            //string usersStr = "man_factjob.id_common_employee = " + userId;

            string equipsStr = "";

            if (equipListAS != null)
            {
                equipsStr = "AND (man_factjob.id_equip = " + equipListAS[0];

                for (int i = 1; i < equipListAS.Count; i++)
                {
                    equipsStr += " OR man_factjob.id_equip = " + equipListAS[i];
                }

                equipsStr += ")";
            }

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            SUM(CASE WHEN norm_operation_table.ord = 1 THEN fact_out_qty ELSE 0 END) AS amount,
	                        SUM(CASE WHEN plan_out_qty = 0 THEN CASE WHEN fact_out_qty = 1 THEN fact_out_qty * normtime END ELSE fact_out_qty * normtime / plan_out_qty END) AS workingOutput,
	                        SUM(CASE WHEN plan_out_qty = 0 THEN CASE WHEN fact_out_qty = 1 THEN fact_out_qty * normtime END ELSE fact_out_qty * normtime / plan_out_qty END) / @shiftNormTime AS workingPercent,
	                        --SUM(fact_out_qty * normtime / plan_out_qty) / @shiftNormTime AS workingPercent,
                            SUM(CASE WHEN norm_operation_table.ord = 0 THEN 1 ELSE 0 END) AS makereadyCount,
                            SUM ( CASE WHEN norm_operation_table.ord = 0 THEN fact_out_qty * normtime / plan_out_qty ELSE 0 END ) AS makereadyWorkTime,
                            CASE WHEN (COUNT (id_man_factjob)) > 0 THEN 1 ELSE 0 END AS isShiftActive
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
	                    LEFT JOIN
	                    dbo.norm_operation_table
	                    ON 
		                    man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
                        WHERE
	                        fbc_brigade.date_begin IS NOT NULL 
                            AND fbc_brigade.date_end IS NOT NULL "
                            + cLine +
	                        @"AND (" + usersStr + @")"
	                        + equipsStr
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);
                Command.Parameters.AddWithValue("@shiftNormTime", shiftNormTime);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    workingAmount = sqlReader["amount"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["amount"]);
                    workingOutput = sqlReader["workingOutput"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["workingOutput"]);
                    workingPercent = sqlReader["workingPercent"] == DBNull.Value ? 0 : (float)Convert.ToDecimal(sqlReader["workingPercent"]);
                    makereadyCount = sqlReader["makereadyCount"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["makereadyCount"]);
                    makeReadyWorkTime = sqlReader["makereadyWorkTime"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["makereadyWorkTime"]);
                    isShiftActive = sqlReader["isShiftActive"] == DBNull.Value ? 0 : (int)Convert.ToInt32(sqlReader["isShiftActive"]);
                }
                connection.Close();
            }

            bonus = calculateWorking.GetBonusWorkingOutF((int)workingOutput);

                /*if (listUsers[0] == 112)
            {
                Console.WriteLine(dateShift + " workingOutput: " + timeValues.MinuteToTimeString((int)workingOutput) + " (" + workingOutput + ")" + ". workingPercent: " + workingPercent * 100);
            }*/

            ShiftsDetails shiftsDetails = new ShiftsDetails(
                    isShiftActive,
                    -1,
                    -1,
                    workingOutput,
                    -1,
                    makereadyCount,
                    makeReadyWorkTime,
                    workingAmount,
                    workingPercent,
                    bonus
                    );
            
            return shiftsDetails;
        }

        /*public List<User> LoadOrdersFromFactjob(DateTime currentDate, int currentShift, bool givenShiftNumber = true)
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
                            man_factjob.id_fbc_brigade,
                            fbc_brigade.date_begin 'shif_date_begin',
                            fbc_brigade.date_end 'shif_date_end'
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
                        ORDER BY man_factjob.date_begin, man_factjob.id_man_factjob ASC"
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);
                    int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);

                    string shiftDateBegin = sqlReader["shif_date_begin"].ToString();

                    string shiftDateEnd = "";
                    if (!DBNull.Value.Equals(sqlReader["shif_date_end"]))
                    {
                        shiftDateEnd = sqlReader["shif_date_end"].ToString();
                    }

                    int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser);

                    if (indexFromUserList == -1)
                    {
                        usersList.Add(new User(
                            loadUser
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
                        currentShift,
                        shiftDateBegin,
                        shiftDateEnd
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

                    int operationType = sqlReader["operationType"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["operationType"]);

                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        loadEquip,
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
                        idletimeName,
                        operationType
                    ));
                }

                connection.Close();
            }

            return usersList;
        }*/

        /*public List<User> LoadOrdersForFBC(DateTime currentDate, int currentShift, bool givenShiftNumber = true)
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
	                        man_planjob_list.id_man_planjob_list,
                            fbc_brigade.date_begin 'shif_date_begin',
                            fbc_brigade.date_end 'shif_date_end'
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
                            man_factjob.date_begin ASC, man_factjob.id_man_factjob ASC"
                };
                Command.Parameters.AddWithValue("@startDate", startDateTime);
                Command.Parameters.AddWithValue("@endDate", endDateTime);
                Command.Parameters.AddWithValue("@shiftNum", currentShift);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    int loadUser = Convert.ToInt32(sqlReader["id_common_employee"]);
                    int loadEquip = Convert.ToInt32(sqlReader["id_equip"]);

                    string shiftDateBegin = sqlReader["shif_date_begin"].ToString();

                    string shiftDateEnd = "";
                    if (!DBNull.Value.Equals(sqlReader["shif_date_end"]))
                    {
                        shiftDateEnd = sqlReader["shif_date_end"].ToString();
                    }

                    int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser);

                    if (indexFromUserList == -1)
                    {
                        usersList.Add(new User(
                            loadUser
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
                        currentShift,
                        shiftDateBegin,
                        shiftDateEnd
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

                    int operationType = sqlReader["operationType"] == DBNull.Value ? -1 : Convert.ToInt32(sqlReader["operationType"]);


                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new UserShiftOrder(
                        loadEquip,
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
                        idletimeName,
                        operationType
                    ));
                }

                connection.Close();
            }

            return usersList;
        }*/

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

        public Normtime GetNormTimeForOrder(int idManOrderJobItem)
        {
            Normtime normtime = new Normtime(idManOrderJobItem);

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            plan_out_qty,
	                        normtime,
                            norm_operation_table.ord
                        FROM
	                        dbo.man_planjob_list
	                        INNER JOIN
	                        dbo.norm_operation_table
	                        ON 
		                        man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
                        WHERE
                            id_man_order_job_item = @idManOrderJobItem AND
                            normtime IS NOT NULL"
                };
                Command.Parameters.AddWithValue("@idManOrderJobItem", idManOrderJobItem);

                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    if (Convert.ToInt32(sqlReader["ord"]) == 0)
                    {
                        normtime.PlanOutQtyMakeReady = (float)Convert.ToDecimal(sqlReader["plan_out_qty"]);
                        normtime.PlanNormtimeMakeReady = Convert.ToInt32(sqlReader["normtime"]);
                    }
                    else
                    {
                        normtime.PlanOutQtyWork = (float)Convert.ToDecimal(sqlReader["plan_out_qty"]);
                        normtime.PlanNormtimeWork = Convert.ToInt32(sqlReader["normtime"]);
                    }
                }

                connection.Close();
            }

            return normtime;
        }

        public int[] GetNormTimeForOrder(int idManOrderJobItem, int idddddd)
        {
            int[] result = { 0, 0 };
            Normtime normtime = new Normtime(idManOrderJobItem);

            using (SqlConnection connection = DBConnection.GetDBConnection())
            {
                connection.Open();
                SqlCommand Command = new SqlCommand
                {
                    Connection = connection,

                    CommandText =
                        @"SELECT
                            plan_out_qty,
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
                        normtime.PlanOutQtyMakeReady = Convert.ToInt32(sqlReader["plan_out_qty"]);
                        normtime.PlanNormtimeMakeReady = Convert.ToInt32(sqlReader["normtime"]);
                    }
                    else
                    {
                        result[1] = Convert.ToInt32(sqlReader["normtime"]);
                        normtime.PlanOutQtyWork = Convert.ToInt32(sqlReader["plan_out_qty"]);
                        normtime.PlanNormtimeWork = Convert.ToInt32(sqlReader["normtime"]);
                    }
                }

                connection.Close();
            }

            return result;
        }

        /*public int GetAmountDoneFromPreviousShifts(int idManOrderJobItem, DateTime currentDate, int currentShift)
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
        }*/

        public float GetAmountDoneFromPreviousShifts(int idManOrderJobItem, string startOrderDateTime, int operationType)
        {
            float result = 0;

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
	                        man_factjob.fact_out_qty,
                            norm_operation_table.ord 'operationType'
                        FROM
	                        dbo.man_factjob
                        INNER JOIN
	                        dbo.man_planjob_list
                            ON 
		                    man_factjob.id_man_planjob_list = man_planjob_list.id_man_planjob_list
                        LEFT JOIN
	                        dbo.norm_operation_table
	                        ON 
		                    man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
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
                    //if (Convert.ToInt32(sqlReader["flags"]) != 576)
                    if (Convert.ToInt32(sqlReader["operationType"]) == operationType)
                    {
                        result += (float)Convert.ToDouble(sqlReader["fact_out_qty"]);
                    }
                    //else
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
