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

        private List<int> LoadAListOfUsersFromTheSelectedRangeForTheSelectedEquipmentList(List<int> equips, DateTime date)
        {
            List<int> usersList = new List<int>();

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

                            if(!usersList.Contains(loadUser)) 
                            {
                                usersList.Add(loadUser);
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

            return usersList;
        }

        private void LoadShifts()
        {
            try
            {
                int countShifts = 2;

                int year = GetYearFromComboBox();
                int month = GetMonthFromComboBox();

                DateTime selectDate = ReturnDateFromInputParameter(year, month);

                int countDaysFromSellectedDate = DateTime.DaysInMonth(year, month);

                for (int i = 0; i < countDaysFromSellectedDate; i++)
                {
                    for(int j = 1; j <= countShifts; j++)
                    {
                        DateTime caurrentDaet = selectDate.AddDays(i);

                        string startDateTime = SelectStartDateTimeFromShiftNumberAndDate(caurrentDaet, j);
                        string endDateTime = SelectEndDateTimeFromShiftNumberAndDate(selectDate, j);


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка " + ex.Message, "Ошибка");
            }
        }

        private List<int> GetSelectegEquipsList()
        {
            List<int> equips = new List<int>();

            equips.Add(9);
            equips.Add(15);
            equips.Add(38);

            return equips;

        }

        private void CreateColomnsToListView(int days)
        {
            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("№", 40, HorizontalAlignment.Center);
            listView1.Columns.Add("Имя", 260);

            for(int i =  1; i <= days; i++)
            {
                listView1.Columns.Add(i.ToString(), 40, HorizontalAlignment.Center);
            }

            listView1.Columns.Add("ИТОГ", 80, HorizontalAlignment.Center);
        }

        private void AddUsersToListView(List<int> usersList)
        {
            for (int i = 0; i < usersList.Count; i++)
            {
                string user = "";

                if (users.ContainsKey(usersList[i]))
                {
                    user = users[usersList[i]];
                }
                else
                {
                    user = "Работник " + usersList[i];
                }

                ListViewItem item = new ListViewItem();

                item.Name = usersList[i].ToString();
                item.Text = (i + 1).ToString();
                item.SubItems.Add(user);

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
                MessageBox.Show("Ошибка получения года " + ex, "Ошибка");
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
                MessageBox.Show("Ошибка получения месяца " + ex, "Ошибка");
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

            List<int> usersList = LoadAListOfUsersFromTheSelectedRangeForTheSelectedEquipmentList(equips, selectDate);

            AddUsersToListView(usersList);
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
