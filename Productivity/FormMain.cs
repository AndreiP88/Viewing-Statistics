using MetroSet_UI.Forms;
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

namespace Productivity
{
    public partial class FormMain : MetroSetForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        bool viewAllEquipsForUser = false;
        int countShifts = 2;

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();

        List<User> usersList;

        private void LoadAllUsers()
        {
            try
            {
                UsersValue usersValue = new UsersValue();

                users = usersValue.LoadAllUsersNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void LoadMachine()
        {
            try
            {
                EquipsValue equipsValue = new EquipsValue();

                machines = equipsValue.LoadMachine();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }
        private void AddYearsToComboBox(int yearStart, int yearEnd)
        {
            comboBox3.Items.Clear();

            for (int i = yearStart; i <= yearEnd; i++)
            {
                comboBox3.Items.Add(i.ToString());
            }
        }

        private void AddMonthToComboBox()
        {
            comboBox2.Items.Clear();

            string[] month = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

            comboBox2.Items.AddRange(month);
        }

        private void SelectCurrentMonth()
        {
            comboBox3.Text = DateTime.Now.Year.ToString();
            comboBox2.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void SelectPreviewMonth()
        {
            comboBox3.Text = DateTime.Now.AddMonths(-1).Year.ToString();
            comboBox2.SelectedIndex = DateTime.Now.AddMonths(-1).Month;
        }

        private void CreateColomnsToListView(int days)
        {
            listView2.Items.Clear();
            listView2.Columns.Clear();

            int width = listView2.Width;

            int w = (width - 440) / days;

            listView2.Columns.Add("№", 40, HorizontalAlignment.Center);
            listView2.Columns.Add("Имя", 300);

            for (int i = 1; i <= days; i++)
            {
                listView2.Columns.Add(i.ToString(), w, HorizontalAlignment.Center);
            }

            listView2.Columns.Add("ИТОГ", 80, HorizontalAlignment.Center);
        }

        private void ChangeDate()
        {
            if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
            {
                UpdateStatistics();
            }
        }

        private DateTime ReturnDateFromInputParameter(int year, int month)
        {
            DateTime result = DateTime.MinValue.AddYears(year - 1).AddMonths(month - 1);

            return result;
        }

        private void LoadStartsValues()
        {
            LoadAllUsers();
            LoadMachine();

            AddYearsToComboBox(2015, 2050);
            AddMonthToComboBox();

            SelectCurrentMonth();

            //Update Later
            comboBox4.SelectedIndex = 0;
        }

        private int GetYearFromComboBox()
        {
            int result = 0;

            try
            {
                result = Convert.ToInt32(comboBox3.Text);
            }
            catch (Exception ex)
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

        private void LoadUsersList(List<int> equips, DateTime date)
        {
            try
            {
                usersList = new List<User>();

                UsersValue usersValue = new UsersValue();

                usersList = usersValue.LoadUsersList(equips, date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void LoadShifts()
        {
            try
            {
                DateTimeValues timeValues = new DateTimeValues();

                int year = GetYearFromComboBox();
                int month = GetMonthFromComboBox();

                DateTime selectDate = ReturnDateFromInputParameter(year, month);

                int countDaysFromSellectedDate = DateTime.DaysInMonth(year, month);

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
	                                    common_employee.employee_lastname, 
	                                    common_employee.employee_firstname, 
	                                    common_employee.employee_middlename, 
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка.");
            }
        }

        private void AddUsersToListView(int countDaysFromSellectedMonth)
        {
            List<int> equips = GetSelectegEquipsList();

            if (comboBox4.SelectedIndex == 0)
            {
                for(int i = 0; i < equips.Count; i++)
                {
                    int countUserForCurrentEquip = 0;
                    string machine = "";

                    if (machines.ContainsKey(equips[i]))
                    {
                        machine = machines[equips[i]];
                    }
                    else
                    {
                        machine = "Оборудование " + machines[i];
                    }

                    AddItemToListView("e" + equips[i], "", machine, countDaysFromSellectedMonth, Color.Gray);

                    for (int j = 0; j < usersList.Count; j++)
                    {
                        if (usersList[j].Equip == equips[i])
                        {
                            countUserForCurrentEquip++;
                            string user = "    ";

                            if (users.ContainsKey(usersList[j].Id))
                            {
                                user += users[usersList[j].Id];
                            }
                            else
                            {
                                user += "Работник " + usersList[j].Id;
                            }

                            Color color = Color.White;

                            if (countUserForCurrentEquip % 2 == 0)
                            {
                                color = Color.LightGray;
                            }

                            AddItemToListView(CreateNameListViewItem(equips[i], usersList[j].Id), countUserForCurrentEquip.ToString(), user, countDaysFromSellectedMonth, color);
                        }
                    }
                }
            }
            else
            {
                List<int> usersCurrent = new List<int>();
                List<int> equipsCurrent = new List<int>();

                for (int i = 0; i < usersList.Count; i++)
                {
                    if (!usersCurrent.Contains(usersList[i].Id))
                    {
                        usersCurrent.Add(usersList[i].Id);
                    }

                    if (viewAllEquipsForUser && !equipsCurrent.Contains(usersList[i].Equip))
                    {
                        equipsCurrent.Add(usersList[i].Equip);
                    }
                }

                if (!viewAllEquipsForUser)
                {
                    equipsCurrent = equips;
                }

                for (int i = 0; i < usersCurrent.Count; i++)
                {
                    string user = "";

                    if (users.ContainsKey(usersCurrent[i]))
                    {
                        user += users[usersCurrent[i]];
                    }
                    else
                    {
                        user += "Работник " + usersCurrent[i];
                    }

                    AddItemToListView("u" + usersCurrent[i], "", user, countDaysFromSellectedMonth, Color.Gray);

                    int countEquipForCurrentUser = 0;

                    for (int j = 0; j < equipsCurrent.Count; j++)
                    {
                        int index = usersList.FindIndex((v) => v.Id == usersCurrent[i] &&
                                                               v.Equip == equipsCurrent[j]);

                        if(index >= 0)
                        {
                            countEquipForCurrentUser++;

                            string machine = "    ";

                            if (machines.ContainsKey(equipsCurrent[j]))
                            {
                                machine += machines[equipsCurrent[j]];
                            }
                            else
                            {
                                machine += "Оборудование " + machines[j];
                            }

                            Color color = Color.White;

                            if (countEquipForCurrentUser % 2 == 0)
                            {
                                color = Color.LightGray;
                            }

                            AddItemToListView(CreateNameListViewItem(equipsCurrent[j], usersList[index].Id), countEquipForCurrentUser.ToString(), machine, countDaysFromSellectedMonth, color);
                        }
                    }
                }
            }


            /*for (int i = 0; i < usersList.Count; i++)
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

                for (int j = 1; j <= countDaysFromSellectedMonth; j++)
                {
                    item.SubItems.Add("");
                }

                listView2.Items.Add(item);
            }*/
        }

        private void AddItemToListView(string name, string text, string subText, int countDays, Color color)
        {
            ListViewItem item = new ListViewItem();

            item.Name = name;
            item.Text = text;
            item.SubItems.Add(subText);

            for (int j = 1; j <= countDays; j++)
            {
                item.SubItems.Add("");
            }

            item.BackColor = color;

            if (text == "")
                item.Font = new Font(item.Font, FontStyle.Bold);

            listView2.Items.Add(item);
        }

        private string CreateNameListViewItem(int equip, int user)
        {
            return "e" + equip + "u" + user;
        }

        private void AddWorkTimeToLV(int day, int shifNum)
        {
            DateTimeValues timeValues = new DateTimeValues();

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

                    //int index = listView2.Items.IndexOfKey(usersList[i].Id.ToString());
                    int index = listView2.Items.IndexOfKey(CreateNameListViewItem(usersList[i].Equip, usersList[i].Id));

                    if (index >= 0 && indexFromUserListShifts >= 0)
                    {
                        if (usersList[i].Shifts[indexFromUserListShifts].Orders != null)
                        {
                            int timeWorkigOut = CalculateWorkTime(usersList[i].Shifts[indexFromUserListShifts].Orders);
                            float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                            ListViewItem item = listView2.Items[index];

                            if (item != null)
                            {
                                item.SubItems[day + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
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

            for (int i = 0; i < order.Count; i++)
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

        private void UpdateStatistics()
        {
            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            int countDaysFromSellectedMonth = DateTime.DaysInMonth(year, month);

            CreateColomnsToListView(countDaysFromSellectedMonth);



            List<int> equips = GetSelectegEquipsList();

            LoadUsersList(equips, selectDate);

            AddUsersToListView(countDaysFromSellectedMonth);

            LoadShifts();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadStartsValues();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDate();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDate();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDate();
        }
    }
}
