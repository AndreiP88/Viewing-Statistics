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
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection;
using System.Threading;

namespace Productivity
{
    public partial class FormMain : MetroSetForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        CancellationTokenSource cancelTokenSource;

        bool viewAllEquipsForUser = false;
        int countShifts = 2;

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();

        List<User> usersList;
        List<Equips> equipsList;

        private void LoadAllUsers()
        {
            try
            {
                ValueUsers usersValue = new ValueUsers();

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
                ValueEquips equipsValue = new ValueEquips();

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

        private void CreateColomnsToListView(int days, int month)
        {
            listView2.Items.Clear();
            listView2.Columns.Clear();

            int width = listView2.Width;

            int w = 50;//(width - 560) / (days);

            listView2.Columns.Add("№", 40, HorizontalAlignment.Center);
            listView2.Columns.Add("Имя", 300);

            listView3.Columns.Add("№", 40, HorizontalAlignment.Center);
            listView3.Columns.Add("Имя", 300);

            for (int i = 1; i <= days; i++)
            {
                listView3.Columns.Add(i.ToString("D2") + "." + month.ToString("D2"), w * 2, HorizontalAlignment.Center);

                for (int j = 1; j <= countShifts; j++)
                {
                    listView2.Columns.Add(i.ToString("D2") + "." + month.ToString("D2"), w, HorizontalAlignment.Center);
                    

                    /*if (j == 1)
                    {
                        listView2.Columns.Add(i.ToString("D2"), w, HorizontalAlignment.Right);
                    }
                    if (j == countShifts)
                    {
                        listView2.Columns.Add(month.ToString("D2"), w, HorizontalAlignment.Left);
                    }*/

                    /*dataGridView1.Columns.Add(i.ToString("D2") + ": " + j.ToString(), i.ToString("D2") + ": " + j.ToString());
                    dataGridView1.Columns[i].Width = w;*/


                }
            }

            listView2.Columns.Add("Выработка", 90, HorizontalAlignment.Center);
            listView2.Columns.Add("Отставание", 95, HorizontalAlignment.Center);
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

                ValueUsers usersValue = new ValueUsers();

                usersList = usersValue.LoadUsersList(equips, date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void LoadShiftsFromUser()
        {
            /*equipsList = new List<Equips>();*/

            ValueDateTime timeValues = new ValueDateTime();
            ValueShifts valueShifts = new ValueShifts();

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            usersList = valueShifts.LoadShifts(usersList, selectDate, countShifts);
        }

        private void LoadShifts()
        {
            try
            {
                /*equipsList = new List<Equips>();*/

                ValueDateTime timeValues = new ValueDateTime();

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

                                /*int indexEquipsList = equipsList.FindIndex(
                                                    (v) => v.Equip == loadEquip &&
                                                           v.ShiftDate == dateShift &&
                                                           v.ShiftNumber == currentShift
                                                           );

                                if (indexEquipsList == -1)
                                {
                                    equipsList.Add(new Equips(
                                        loadEquip,
                                        dateShift,
                                        currentShift
                                        ));
                                }*/
                            }

                            connection.Close();
                        }

                        //AddWorkTimeToLV(currentDay, currentShift);
                        //AddWorkTimeEquipToLV(currentDay, currentShift);
                        //AddWorkTimeUserToLV(currentDay, currentShift);
                    }
                }

                StartAddingWorkingTimeToListView();
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
        }

        private void AddItemToListView(string name, string text, string subText, int countDays, Color color)
        {
            ListViewItem item = new ListViewItem();

            item.Name = name;
            item.Text = text;
            item.SubItems.Add(subText);

            for (int j = 1; j <= countDays * countShifts; j++)
            {
                item.SubItems.Add("");
            }

            item.BackColor = color;

            if (text == "")
                item.Font = new Font(item.Font, FontStyle.Bold);

            listView2.Items.Add(item);
            /*
            dataGridView1.Rows.Add(name);
            //dataGridView1.Rows[i-1].SetValues(i.ToString(), j.ToString(),1,3);
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Value = text;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Value = subText;
            //dataGridView1.Rows[2].Cells[3].Style.;
            */
            
        }
        private void AddShiftNumbersToListView(int days)
        {
            ListViewItem item = new ListViewItem();

            item.Name = "";
            item.Text = "";
            item.SubItems.Add("");

            for (int i = 1; i <= days; i++)
            {
                for (int j = 1; j <= countShifts; j++)
                {
                    item.SubItems.Add(j.ToString());
                }
            }

            item.Font = new Font(item.Font, FontStyle.Bold);

            listView2.Items.Add(item);
        }

        private string CreateNameListViewItem(int equip, int user)
        {
            return "e" + equip + "u" + user;
        }

        private void StartAddingWorkingTimeToListView()
        {
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
            }

            cancelTokenSource = new CancellationTokenSource();

            Task taskDetails = new Task(() => AddWorkingTimeUsersToListView(cancelTokenSource.Token), cancelTokenSource.Token);
            taskDetails.Start();
            //AddWorkingTimeUsersToListView(cancelTokenSource.Token);

            Task taskEquips = new Task(() => AddWorkingTimeEquipsToListView(cancelTokenSource.Token), cancelTokenSource.Token);
            //taskEquips.Start();
            //AddWorkingTimeEquipsToListView
        }

        private void AddWorkingTimeUsersToListView(CancellationToken token)
        {
            equipsList = new List<Equips>();

            ValueDateTime timeValues = new ValueDateTime();

            for (int i = 0; i < usersList.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (usersList[i].Shifts != null)
                {
                    int userShiftWorkingOut = 0;

                    for(int j = 0; j < usersList[i].Shifts.Count; j++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        if (usersList[i].Shifts[j].Orders != null)
                        {
                            int day = Convert.ToDateTime(usersList[i].Shifts[j].ShiftDate).Day;
                            int shiftNumber = usersList[i].Shifts[j].ShiftNumber;

                            int timeWorkigOut = CalculateWorkTime(usersList[i].Shifts[j].Orders);

                            userShiftWorkingOut += timeWorkigOut;

                            float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                            int indexEquipsList = equipsList.FindIndex(
                                                    (v) => v.Equip == usersList[i].Equip &&
                                                           v.ShiftDate == usersList[i].Shifts[j].ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                            if (indexEquipsList != -1)
                            {
                                equipsList[indexEquipsList].WorkingOut += timeWorkigOut;
                            }
                            else
                            {
                                equipsList.Add(new Equips(
                                    usersList[i].Equip,
                                    usersList[i].Shifts[j].ShiftDate,
                                    shiftNumber
                                    ));

                                equipsList[equipsList.Count - 1].WorkingOut = timeWorkigOut;
                            }

                            Invoke(new Action(() =>
                            {
                                int index = listView2.Items.IndexOfKey(CreateNameListViewItem(usersList[i].Equip, usersList[i].Id));

                                ListViewItem item = listView2.Items[index];

                                if (item != null)
                                {
                                    item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                                    //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                                }
                            }));
                        }
                    }
                }
            }

            for (int i = 0; i < equipsList.Count; i++)
            {
                //MessageBox.Show(equipsList.Count + ", " + equipsList[i].Equip + ", " + equipsList[i].WorkingOut);
                int day = Convert.ToDateTime(equipsList[i].ShiftDate).Day;
                int shiftNumber = equipsList[i].ShiftNumber;

                int timeWorkigOut = equipsList[i].WorkingOut;

                float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                Invoke(new Action(() =>
                {
                    int index = listView2.Items.IndexOfKey("e" + equipsList[i].Equip);

                    ListViewItem item = listView2.Items[index];

                    if (item != null)
                    {
                        item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                        //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                    }
                }));
            }
        }

        private void AddWorkingTimeEquipsToListView(CancellationToken token)
        {
            equipsList = new List<Equips>();

            ValueDateTime timeValues = new ValueDateTime();

            for (int i = 0; i < usersList.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (usersList[i].Shifts != null)
                {
                    for (int j = 0; j < usersList[i].Shifts.Count; j++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        if (usersList[i].Shifts[j].Orders != null)
                        {
                            int day = Convert.ToDateTime(usersList[i].Shifts[j].ShiftDate).Day;
                            int shiftNumber = usersList[i].Shifts[j].ShiftNumber;

                            int timeWorkigOut = CalculateWorkTime(usersList[i].Shifts[j].Orders);

                            int indexEquipsList = equipsList.FindIndex(
                                                    (v) => v.Equip == usersList[i].Equip &&
                                                           v.ShiftDate == usersList[i].Shifts[j].ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                            if (indexEquipsList != -1)
                            {
                                equipsList[indexEquipsList].WorkingOut += timeWorkigOut;
                            }
                            else
                            {
                                equipsList.Add(new Equips(
                                    usersList[i].Equip,
                                    usersList[i].Shifts[j].ShiftDate,
                                    shiftNumber
                                    ));

                                equipsList[equipsList.Count - 1].WorkingOut = timeWorkigOut;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < equipsList.Count; i++)
            {
                //MessageBox.Show(equipsList.Count + ", " + equipsList[i].Equip + ", " + equipsList[i].WorkingOut);
                int day = Convert.ToDateTime(equipsList[i].ShiftDate).Day;
                int shiftNumber = equipsList[i].ShiftNumber;

                int timeWorkigOut = equipsList[i].WorkingOut;

                float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                Invoke(new Action(() =>
                {
                    int index = listView2.Items.IndexOfKey("e" + equipsList[i].Equip);

                    ListViewItem item = listView2.Items[index];

                    if (item != null)
                    {
                        item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                        //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                    }
                }));
            }
















            /*for (int i = 0; i < equipsList.Count; i++)
            {
                int day = Convert.ToDateTime(equipsList[i].ShiftDate).Day;
                int shiftNumber = equipsList[i].ShiftNumber;

                int timeWorkigOut = equipsList[i].WorkingOut;

                float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                Invoke(new Action(() =>
                {
                    int index = listView2.Items.IndexOfKey("e" + equipsList[i].Equip);

                    ListViewItem item = listView2.Items[index];

                    if (item != null)
                    {
                        item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                        //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                    }
                }));
            }*/













            /*List<int> equips = GetSelectegEquipsList();

            for (int j = 0; j < equips.Count; j++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                //int timeWorkigOut = 0;

                for (int i = 0; i < usersList.Count; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }
                    int timeWorkigOut = 0;

                    if (usersList[i].Equip == equips[j])
                    {
                        if (usersList[i].Shifts != null)
                        {
                            for (int k = 0; k < usersList[i].Shifts.Count; k++)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }

                                if (usersList[i].Shifts[k].Orders != null)
                                {
                                    int day = Convert.ToDateTime(usersList[i].Shifts[k].ShiftDate).Day;
                                    int shiftNumber = usersList[i].Shifts[k].ShiftNumber;

                                    timeWorkigOut += CalculateWorkTime(usersList[i].Shifts[k].Orders);

                                    float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                                    Invoke(new Action(() =>
                                    {
                                        int index = listView2.Items.IndexOfKey("e" + usersList[i].Equip);

                                        ListViewItem item = listView2.Items[index];

                                        if (item != null)
                                        {
                                            item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                                            //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                                        }
                                    }));
                                }
                            }
                        }
                    }
                }
            }*/
        }














        private void AddWorkTimeToLV(int day, int shifNum)
        {
            ValueDateTime timeValues = new ValueDateTime();

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
                                item.SubItems[(day - 1) * countShifts + shifNum + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                                //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                            }
                        }

                    }
                }

            }
        }

        

        private void AddWorkTimeUserToLV(int day, int shifNum)
        {
            ValueDateTime timeValues = new ValueDateTime();

            List<int> equips = GetSelectegEquipsList();

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            string dateShift = day.ToString("D2") + "." + month.ToString("D2") + "." + year.ToString();

            int indexFromUserListShifts = -1;

            List<int> usersCurrent = new List<int>();

            for (int i = 0; i < usersList.Count; i++)
            {
                if (!usersCurrent.Contains(usersList[i].Id))
                {
                    usersCurrent.Add(usersList[i].Id);
                }
            }

            for (int j = 0; j < usersCurrent.Count; j++)
            {
                int timeWorkigOut = 0;

                for (int i = 0; i < usersList.Count; i++)
                {
                    if (usersList[i].Shifts != null)
                    {
                        indexFromUserListShifts = usersList[i].Shifts.FindIndex(
                                            (v) => v.ShiftDate == dateShift &&
                                                   v.ShiftNumber == shifNum);

                        //int index = listView2.Items.IndexOfKey(usersList[i].Id.ToString());
                        int index = listView2.Items.IndexOfKey("u" + usersList[i].Id);
                        //MessageBox.Show(index.ToString());
                        if (index >= 0 && indexFromUserListShifts >= 0 && usersList[i].Id == usersCurrent[j])
                        {
                            if (usersList[i].Shifts[indexFromUserListShifts].Orders != null)
                            {
                                timeWorkigOut += CalculateWorkTime(usersList[i].Shifts[indexFromUserListShifts].Orders);
                                float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                                ListViewItem item = listView2.Items[index];

                                if (item != null)
                                {
                                    item.SubItems[(day - 1) * countShifts + shifNum + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                                    //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                                }
                            }
                        }
                    }
                }
            }
        }

        private int CalculateWorkTime(List<UserShiftOrder> order)
        {
            float workingOut = 0;

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
                        float norm = order[i].PlanOutQty / order[i].Normtime;

                        if (norm > 0)
                        {
                            workingOut += order[i].FactOutQty / norm;
                        }
                    }
                }
            }

            /*for (int i = 0; i < order.Count; i++)
            {
                if (order[i].Normtime > 0)
                {
                    float norm = order[i].PlanOutQty / order[i].Normtime;

                    if (norm > 0)
                    {
                        workingOut += order[i].FactOutQty / norm;
                    }
                }
            }*/

            return (int)workingOut;
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
            equips.Add(8);
            equips.Add(11);
            equips.Add(13);
            equips.Add(3);
            equips.Add(4);
            equips.Add(5);

            return equips;

        }

        private void UpdateStatistics()
        {
            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            int countDaysFromSellectedMonth = DateTime.DaysInMonth(year, month);

            CreateColomnsToListView(countDaysFromSellectedMonth, month);
            AddShiftNumbersToListView(countDaysFromSellectedMonth);



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

        private void button1_Click(object sender, EventArgs e)
        {
            //int s = listView2.Items[0].SubItems[0].Bounds.Location.X;

            listView3.TopItem.Position = new Point(listView2.TopItem.Position.X, listView2.TopItem.Position.Y);

            listView2.TopItem.Position = new Point(0, listView2.TopItem.Position.Y - 500);


            //MessageBox.Show(listView2.TopItem.Position.X.ToString());
        }
    }
}
