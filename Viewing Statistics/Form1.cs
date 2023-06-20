using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.InteropServices.ComTypes;

namespace Viewing_Statistics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();

        List<User> usersList;

        private void LoadUsers()
        {
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
            catch(ArgumentException e)
            {
                MessageBox.Show("Ошибка подключения", e.Message);
            }
        }

        private void LoadMachine()
        {
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
            catch (ArgumentException e)
            {
                MessageBox.Show("Ошибка подключения", e.Message);
            }
        }

        private void AddYearsToComboBox(int yearStart, int yearEnd)
        {
            comboBox1.Items.Clear();

            for (int i = yearStart; i <= yearEnd; i++)
            {
                comboBox1.Items.Add(i.ToString());
            }
        }

        private void AddMonthToComboBox()
        {
            comboBox2.Items.Clear();

            string[] month = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

            comboBox2.Items.AddRange(month);
        }

        private void SelectCurrentDate()
        {
            comboBox1.Text = DateTime.Now.Year.ToString();
            comboBox2.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void SelectPreviewMonth()
        {
            comboBox1.Text = DateTime.Now.AddMonths(-1).Year.ToString();
            comboBox2.SelectedIndex = DateTime.Now.AddMonths(-1).Month;
        }

        private string SelectStartDateTimeFromShiftNumberAndDate(DateTime date, int shiftNumber)
        {
            string result = "";

            if(shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T07:00:00.000";
            }
            else
            {
                result = date.ToString("yyyy-MM-dd") + "T19:00:00.000";
            }

            return result;
        }

        private string SelectEndDateTimeFromShiftNumberAndDate(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T21:00:00.000";
            }
            else
            {
                result = date.AddDays(1).ToString("yyyy-MM-dd") + "T09:00:00.000";
            }

            return result;
        }

        private void LoadUsersList(List<int> equips, DateTime date)
        {
            usersList = new List<User>();

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

                            if(usersList.FindIndex((v) => v.Id == loadUser) == -1)
                            {
                                usersList.Add(new User(loadUser));
                                usersList[usersList.Count - 1].Shifts = new List<UserShift>();
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Ошибка подключения", e.Message);
            }
        }

        private void LoadShifts()
        {
            //try
            {
                int countShifts = 2;

                int year = GetYearFromComboBox();
                int month = GetMonthFromComboBox();

                DateTime selectDate = ReturnDateFromInputParameter(year, month);

                int countDaysFromSellectedDate = DateTime.DaysInMonth(year, month);

                for (int currentDay = 0; currentDay < countDaysFromSellectedDate; currentDay++)
                {
                    for(int currentShift = 1; currentShift <= countShifts; currentShift++)
                    {
                        DateTime currentDate = selectDate.AddDays(currentDay);

                        string dateShift = currentDate.ToString("dd.MM.yyyy");

                        string startDateTime = SelectStartDateTimeFromShiftNumberAndDate(currentDate, currentShift);
                        string endDateTime = SelectEndDateTimeFromShiftNumberAndDate(currentDate, currentShift);

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
	                                    common_employee.employee_lastname, 
	                                    common_employee.employee_firstname, 
	                                    common_employee.employee_middlename, 
                                        man_factjob.id_common_employee,
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

                                int indexFromUserList = usersList.FindIndex((v) => v.Id == loadUser);
                                
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

                                        usersList[indexFromUserList].Shifts[indexShift].Orders = new List<Order>();
                                    }

                                    usersList[indexFromUserList].Shifts[indexShift].Orders.Add(new Order(
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

                        AddWorkTimeToLV(currentDay, currentShift);
                    }
                }
            }
            //catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Ошибка. LoadShifts");
            }
        }

        private void AddWorkTimeToLV(int day, int shifNum)
        {
            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            string dateShift = day.ToString("D2") + "." + month.ToString("D2") + "." + year.ToString();

            for (int i = 0; i < usersList.Count; i++)
            {
                //int indexFromUserList = usersList.FindIndex((v) => v.Id == user);

                int indexFromUserListShifts = -1;

                if (usersList[i].Shifts != null)
                {
                    indexFromUserListShifts = usersList[i].Shifts.FindIndex(
                                        (v) => v.ShiftDate == dateShift &&
                                               v.ShiftNumber == shifNum);

                    int index = listView1.Items.IndexOfKey(usersList[i].Id.ToString());

                    if (index >= 0 && indexFromUserListShifts >= 0)
                    {
                        if(usersList[i].Shifts[indexFromUserListShifts].Orders != null)
                        {
                            int timeWorkigOut = CalculateWorkTime(usersList[i].Shifts[indexFromUserListShifts].Orders);
                            float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                            ListViewItem item = listView1.Items[index];

                            if (item != null)
                            {
                                item.SubItems[day + 1].Text = MinuteToTimeString(timeWorkigOut);
                                //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                            }
                        }
                        
                    }
                }

                
            }
        }

        private int CalculateWorkTime(List<Order> order)
        {
            int workingOut = 0;

            for (int i = 0;  i < order.Count; i++)
            {
                if (order[i].Flags == 576)
                {
                    workingOut += order[i].Normtime;
                }

                if (order[i].Flags == 512 || order[i].Flags == 544)
                {
                    if (order[i].Normtime > 0)
                    {
                        int norm = order[i].PlanOutQty / order[i].Normtime;

                        if (norm > 0)
                        {
                            workingOut += order[i].FactOutQty / norm;
                        }
                    }
                }
            }

            return workingOut;
        }

        private float GetPercentWorkingOut(int targetWorkingOut, int facticalWorkingOut)
        {
            float result;

            result = (float)facticalWorkingOut / targetWorkingOut;

            return result;
        }

        private List<int> GetSelectegEquipsList()
        {
            List<int> equips = new List<int>();

            equips.Add(9);
            equips.Add(15);
            equips.Add(38);

            return equips;

        }

        public string MinuteToTimeString(int totalMinutes)
        {
            string result = "00:00";

            int absMinutes = Math.Abs(totalMinutes);

            int hours = 0;
            int minutes = absMinutes % 60;

            if (absMinutes >= 60)
            {
                hours = absMinutes / 60;
            }

            result = hours.ToString("D2") + ":" + minutes.ToString("D2");

            return result;
        }

        private void CreateColomnsToListView(int days)
        {
            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("№", 40, HorizontalAlignment.Center);
            listView1.Columns.Add("Имя", 300);

            for(int i =  1; i <= days; i++)
            {
                listView1.Columns.Add(i.ToString(), 50, HorizontalAlignment.Center);
            }

            listView1.Columns.Add("ИТОГ", 80, HorizontalAlignment.Center);
        }

        private void AddUsersToListView(int countDaysFromSellectedDate)
        {
            for (int i = 0; i < usersList.Count; i++)
            {
                string user = "";

                if (users.ContainsKey(usersList[i].Id))
                {
                    user = users[usersList[i].Id];
                }
                else
                {
                    user = "Работник " + usersList[i].Id;
                }

                ListViewItem item = new ListViewItem();

                item.Name = usersList[i].Id.ToString();
                item.Text = (i + 1).ToString();
                item.SubItems.Add(user);

                for(int j = 1; j <= countDaysFromSellectedDate; j++)
                {
                    item.SubItems.Add("");
                }

                listView1.Items.Add(item);
            }
        }

        private DateTime ReturnDateFromInputParameter(int year, int month)
        {
            DateTime result = DateTime.MinValue.AddYears(year - 1).AddMonths(month - 1);

            return result;
        }

        private void ChangeDate()
        {
            if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                UpdateStatistics();
            }
        }

        private int GetYearFromComboBox()
        {
            int result = 0;

            try
            {
                result = Convert.ToInt32(comboBox1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка получения года " + ex.Message, "Ошибка");
            }

            return result;
        }

        private int GetMonthFromComboBox()
        {
            int result = 0;

            try
            {
                result = comboBox2.SelectedIndex + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка получения месяца " + ex.Message, "Ошибка");
            }

            return result;
        }

        private void UpdateStatistics()
        {
            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            int countDaysFromSellectedDate = DateTime.DaysInMonth(year, month);

            CreateColomnsToListView(countDaysFromSellectedDate);



            List<int> equips = GetSelectegEquipsList();

            LoadUsersList(equips, selectDate);

            AddUsersToListView(countDaysFromSellectedDate);

            LoadShifts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUsers();
            LoadMachine();

            AddYearsToComboBox(2015, 2050);
            AddMonthToComboBox();

            SelectCurrentDate();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDate();
        }
    }
}
