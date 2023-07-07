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
using System.Runtime.InteropServices;
using OrderManager;

namespace Productivity
{
    public partial class FormMain : MetroSetForm
    {
        public FormMain()
        {
            InitializeComponent();
        }


        /*
         * 
         * Сделать прокрутку и сделать перерисовку ширины столбцов в зависимости от нижней таблицы 
         * Добавить таймер
         listView4.Left = listView3.TopItem.Position.X - 4;
         listView4.Width = listView3.Width;
         */

        CancellationTokenSource cancelTokenSource;

        int metroSetTabControlPreviousIndex = -1;

        bool viewAllEquipsForUser = true;
        int countShifts = 2;

        int fullOutput = 650;

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();
        Dictionary<string, int> rowIndexes = new Dictionary<string, int>();

        List<User> usersList;

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

                foreach(KeyValuePair<int, string> equip in machines)
                {
                    ListViewItem item = new ListViewItem();

                    item.Name = equip.Key.ToString();
                    item.Text = (ListViewEquips.Items.Count + 1).ToString("D2");
                    item.SubItems.Add(equip.Value);

                    ListViewEquips.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void LoadCheckedEquipsFromIniFile()
        {
            INISettings iniSettings = new INISettings();

            string equipStr = iniSettings.GetViewedEquipment();

            string[] equipsArray = equipStr.Split(new char[] { ';' });

            foreach(string equip in  equipsArray)
            {
                int index = ListViewEquips.Items.IndexOfKey(equip);

                if (index != -1)
                {
                    ListViewEquips.Items[index].Checked = true;
                }
            }
        }

        private void SaveCheckedEquipsToIniFile()
        {
            INISettings iniSettings = new INISettings();

            List<string> selectedEquips = new List<string>();

            for (int i = 0; i < ListViewEquips.Items.Count; i++)
            {
                if (ListViewEquips.Items[i].Checked)
                {
                    selectedEquips.Add(ListViewEquips.Items[i].Name);
                }
            }

            string outputStr = "";

            for (int i = 0; i < selectedEquips.Count; i++)
            {
                if (i <  selectedEquips.Count - 1)
                {
                    outputStr += selectedEquips[i] + ";";
                }
                else
                {
                    outputStr += selectedEquips[i];
                }
            }

            iniSettings.SetViewedEquipment(outputStr);
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

            for (int i = 1; i <= days; i++)
            {
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

            CreateColomnsToDataGrid(days, month);
        }

        private void CreateColomnsToDataGrid(int days, int month)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            int width = dataGridView1.Width;

            int w = 50;//(width - 560) / (days);

            DataGridViewColumn pColumn;
            string strTemp;

            dataGridView1.Columns.Add(@"colGroup", @"");
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Columns.Add(@"colTask", @"Task");
            dataGridView1.Columns[1].Width = 300;

            pColumn = dataGridView1.Columns["colTask"];
            pColumn.Frozen = true;

            for (int i = 0; i < days * countShifts; i++)
            {
                strTemp = "col" + i.ToString();
                dataGridView1.Columns.Add(@strTemp, i.ToString());
                pColumn = dataGridView1.Columns[strTemp];
                pColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                pColumn.Width = w;
                pColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dataGridView1.Columns.Add(@"colGroup", @"Выработка");
            dataGridView1.Columns[days * 2 + 2].Width = 100;
            dataGridView1.Columns[days * 2 + 2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns.Add(@"colTask", @"Отставание");
            dataGridView1.Columns[days * 2 + 3].Width = 100;
            dataGridView1.Columns[days * 2 + 3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Frozen = true;

            AddCellToGrid(0, 0, 2);
            dataGridView1.Rows[0].Cells[0].Value = "";

            AddCellToGrid(0, days * countShifts + 2, 2);
            dataGridView1.Rows[0].Cells[days * countShifts + 2].Value = "";

            for (int i = 2; i <= days * 2; i+=2)
            {
                AddCellToGrid(0, i, countShifts);

                dataGridView1.Rows[0].Cells[i].Value = (i / 2).ToString("D2") + "." + month.ToString("D2");
            }

            dataGridView1.Rows.Add();
            dataGridView1.Rows[1].Frozen = true;

            AddCellToGrid(1, 0, 2);
            dataGridView1.Rows[1].Cells[0].Value = "Имя";

            AddCellToGrid(1, days * countShifts + 2);
            dataGridView1.Rows[1].Cells[days * countShifts + 2].Value = "Выработка";

            AddCellToGrid(1, days * countShifts + 3);
            dataGridView1.Rows[1].Cells[days * countShifts + 3].Value = "Отставание";

            for (int i = 2; i <= days * countShifts + 1; i += 2)
            {
                for (int j = 1; j <= countShifts; j++)
                {
                    int n = i + j - 1;

                    AddCellToGrid(1, n);

                    dataGridView1.Rows[1].Cells[n].Value = (j).ToString();
                }
            }
        }

        private void AddCellToGrid(int indexRow, int indexCell, int collSpan = 1)
        {
            HMergedCell pCell;

            //int nOffset = indexCell;

            for (int j = indexCell; j < indexCell + collSpan; j++)
            {
                dataGridView1.Rows[indexRow].Cells[j] = new HMergedCell();
                pCell = (HMergedCell)dataGridView1.Rows[indexRow].Cells[j];
                pCell.LeftColumn = indexCell;
                pCell.RightColumn = indexCell + collSpan - 1;
            }
            //nOffset += collSpan + 1;
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
            
            metroSetTabControl1.SelectTab(1);
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

                //usersList = usersValue.LoadUsersList(equips, date);
                usersList = usersValue.LoadUsersListFromSelectMonth(equips, date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void LoadShifts()
        {
            /*equipsList = new List<Equips>();*/

            ValueDateTime timeValues = new ValueDateTime();
            ValueShifts valueShifts = new ValueShifts();

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            usersList = valueShifts.LoadShifts(usersList, selectDate, countShifts);

            StartAddingWorkingTimeToListView();
        }

        private void AddUsersToListView(int countDaysFromSellectedMonth)
        {
            List<int> equips = GetSelectegEquipsList();
            rowIndexes.Clear();

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
                    AddItemToGrid("e" + equips[i], "", machine, Color.Gray);

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
                            AddItemToGrid(CreateNameListViewItem(equips[i], usersList[j].Id), countUserForCurrentEquip.ToString(), user, color);
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
                    AddItemToGrid("u" + usersCurrent[i], "", user, Color.Gray);

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
                            AddItemToGrid(CreateNameListViewItem(equipsCurrent[j], usersList[index].Id), countEquipForCurrentUser.ToString(), machine, color);
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

            for (int j = 1; j <= countDays * countShifts + 2; j++)
            {
                item.SubItems.Add("");
            }

            item.BackColor = color;

            if (text == "")
                item.Font = new Font(item.Font, FontStyle.Bold);

            listView2.Items.Add(item);
        }

        private void AddItemToGrid(string name, string text, string subText, Color color, int colSpan = 1)
        {
            int indexRow = dataGridView1.Rows.Add();

            if (!rowIndexes.ContainsKey(name))
            {
                rowIndexes.Add(name, indexRow);
            }

            dataGridView1.Rows[indexRow].Cells[0].Value = text;
            dataGridView1.Rows[indexRow].Cells[1].Value = subText;
            dataGridView1.Rows[indexRow].DefaultCellStyle.BackColor = color;

            if (text == "")
            {
                dataGridView1.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;
                dataGridView1.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            }
            else
            {
                dataGridView1.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;
            }
            

            //AddCellToGrid();

            /*ListViewItem item = new ListViewItem();

            item.Name = name;
            item.Text = text;
            item.SubItems.Add(subText);

            for (int j = 1; j <= countDays * countShifts + 2; j++)
            {
                item.SubItems.Add("");
            }

            item.BackColor = color;

            if (text == "")
                item.Font = new Font(item.Font, FontStyle.Bold);

            listView2.Items.Add(item);*/
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

            //Task taskEquips = new Task(() => AddWorkingTimeEquipsToListView(cancelTokenSource.Token), cancelTokenSource.Token);
            //taskEquips.Start();
            //AddWorkingTimeEquipsToListView
        }

        private void AddWorkingTimeUsersToListView(CancellationToken token)
        {
            List<WorkingOut> equipsListWorkingOut = new List<WorkingOut>();
            List<WorkingOut> usersListWorkingOut = new List<WorkingOut>();
            //List<int> usersCurrent = new List<int>();

            ValueDateTime timeValues = new ValueDateTime();

            for (int i = 0; i < usersList.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (usersList[i].Shifts != null)
                {
                    for(int j = 0; j < usersList[i].Shifts.Count; j++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        int countDaysFromMonth = CountDaysFromMonth(usersList[i].Shifts[j].ShiftDate);

                        if (usersList[i].Shifts[j].Orders != null)
                        {
                            int day = Convert.ToDateTime(usersList[i].Shifts[j].ShiftDate).Day;
                            int shiftNumber = usersList[i].Shifts[j].ShiftNumber;

                            int timeWorkigOut = CalculateWorkTime(usersList[i].Shifts[j].Orders);
                            int timeBacklog = fullOutput - timeWorkigOut;

                            usersList[i].WorkingOutUser += timeWorkigOut;
                            usersList[i].WorkingOutBacklog += timeBacklog;

                            float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);
                            
                            //Выработка для оборудования
                            int indexEquipsList = equipsListWorkingOut.FindIndex(
                                                    (v) => v.Id == usersList[i].Equip
                                                           );

                            if (indexEquipsList != -1)
                            {
                                int indexEquipsListWOut = equipsListWorkingOut[indexEquipsList].WorkingOutList.FindIndex(
                                                    (v) => v.ShiftDate == usersList[i].Shifts[j].ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                                if (indexEquipsListWOut != -1)
                                {
                                    equipsListWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].WorkingOut += timeWorkigOut;
                                }
                                else
                                {
                                    equipsListWorkingOut[indexEquipsList].WorkingOutList.Add(new WorkingOutValue(
                                    usersList[i].Shifts[j].ShiftDate,
                                    shiftNumber,
                                    timeWorkigOut
                                    ));

                                    //equipsList[indexEquipsList].EquipsWOut[equipsList[indexEquipsList].EquipsWOut.Count - 1].WorkingOut 
                                }

                                equipsListWorkingOut[indexEquipsList].WorkingOutSumm += timeWorkigOut;
                                equipsListWorkingOut[indexEquipsList].WorkingOutBacklog += timeBacklog;

                            }
                            else
                            {
                                equipsListWorkingOut.Add(new WorkingOut(
                                    usersList[i].Equip
                                    ));

                                equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList = new List<WorkingOutValue>
                                {
                                    new WorkingOutValue(
                                        usersList[i].Shifts[j].ShiftDate,
                                        shiftNumber,
                                        timeWorkigOut
                                    )
                                };

                                equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                            }

                            //Выработка для сотрудника
                            int indexUserList = usersListWorkingOut.FindIndex(
                                                    (v) => v.Id == usersList[i].Id
                                                           );

                            if (indexUserList != -1)
                            {
                                int indexUserListWOut = usersListWorkingOut[indexUserList].WorkingOutList.FindIndex(
                                                    (v) => v.ShiftDate == usersList[i].Shifts[j].ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                                if (indexUserListWOut != -1)
                                {
                                    usersListWorkingOut[indexUserList].WorkingOutList[indexUserListWOut].WorkingOut += timeWorkigOut;
                                }
                                else
                                {
                                    usersListWorkingOut[indexUserList].WorkingOutList.Add(new WorkingOutValue(
                                    usersList[i].Shifts[j].ShiftDate,
                                    shiftNumber,
                                    timeWorkigOut
                                    ));

                                    //equipsList[indexEquipsList].EquipsWOut[equipsList[indexEquipsList].EquipsWOut.Count - 1].WorkingOut 
                                }

                                usersListWorkingOut[indexUserList].WorkingOutSumm += timeWorkigOut;
                                //usersListWorkingOut[indexUserList].WorkingOutBacklog += timeBacklog;
                                usersListWorkingOut[indexUserList].WorkingOutBacklog += fullOutput - timeWorkigOut;
                            }
                            else
                            {
                                usersListWorkingOut.Add(new WorkingOut(
                                    usersList[i].Id
                                    ));

                                usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutList = new List<WorkingOutValue>
                                {
                                    new WorkingOutValue(
                                        usersList[i].Shifts[j].ShiftDate,
                                        shiftNumber,
                                        timeWorkigOut
                                    )
                                };

                                usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                //usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                                usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutBacklog += fullOutput - timeWorkigOut;
                            }

                            Invoke(new Action(() =>
                            {
                                int index = listView2.Items.IndexOfKey(CreateNameListViewItem(usersList[i].Equip, usersList[i].Id));

                                if (index >= 0)
                                {
                                    ListViewItem item = listView2.Items[index];

                                    if (item != null)
                                    {
                                        item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                                        //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                                        item.SubItems[countDaysFromMonth * countShifts + 2].Text = timeValues.MinuteToTimeString(usersList[i].WorkingOutUser);
                                        item.SubItems[countDaysFromMonth * countShifts + 3].Text = timeValues.MinuteToTimeString(usersList[i].WorkingOutBacklog);
                                    }
                                }
                                
                                string key = CreateNameListViewItem(usersList[i].Equip, usersList[i].Id);

                                if (rowIndexes.ContainsKey(key))
                                {
                                    int indexRow = rowIndexes[key];

                                    dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                                    dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = timeValues.MinuteToTimeString(usersList[i].WorkingOutUser);
                                    dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(usersList[i].WorkingOutBacklog);
                                }
                            }));
                        }
                    }
                }
            }

            for (int i = 0; i < equipsListWorkingOut.Count; i++)
            {
                int countDaysFromMonth = 0;

                for (int j = 0; j < equipsListWorkingOut[i].WorkingOutList.Count; j++)
                {
                    //MessageBox.Show(equipsList.Count + ", " + equipsList[i].Equip + ", " + equipsList[i].WorkingOut);
                    int day = Convert.ToDateTime(equipsListWorkingOut[i].WorkingOutList[j].ShiftDate).Day;
                    int shiftNumber = equipsListWorkingOut[i].WorkingOutList[j].ShiftNumber;

                    countDaysFromMonth = CountDaysFromMonth(equipsListWorkingOut[i].WorkingOutList[j].ShiftDate);

                    int timeWorkigOut = equipsListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        int index = listView2.Items.IndexOfKey("e" + equipsListWorkingOut[i].Id);

                        if (index >= 0)
                        {
                            ListViewItem item = listView2.Items[index];

                            if (item != null)
                            {
                                item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                                //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                            }
                        }

                        string key = "e" + equipsListWorkingOut[i].Id;

                        if (rowIndexes.ContainsKey(key))
                        {
                            int indexRow = rowIndexes[key];

                            dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                        }
                    }));
                }

                int fullTimeWorkigOut = equipsListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    int index = listView2.Items.IndexOfKey("e" + equipsListWorkingOut[i].Id);

                    if (index >= 0)
                    {
                        ListViewItem item = listView2.Items[index];

                        if (item != null)
                        {
                            item.SubItems[countDaysFromMonth * countShifts + 2].Text = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutSumm);
                            //item.SubItems[countDaysFromMonth * countShifts + 3].Text = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                            item.SubItems[countDaysFromMonth * countShifts + 3].Text = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutList.Count * fullOutput - equipsListWorkingOut[i].WorkingOutSumm);
                        }
                    }

                    string key = "e" + equipsListWorkingOut[i].Id;

                    if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutList.Count * fullOutput - equipsListWorkingOut[i].WorkingOutSumm);
                    }

                }));
            }

            for (int i = 0; i < usersListWorkingOut.Count; i++)
            {
                int countDaysFromMonth = 0;

                for (int j = 0; j < usersListWorkingOut[i].WorkingOutList.Count; j++)
                {
                    //MessageBox.Show(equipsList.Count + ", " + equipsList[i].Equip + ", " + equipsList[i].WorkingOut);
                    int day = Convert.ToDateTime(usersListWorkingOut[i].WorkingOutList[j].ShiftDate).Day;
                    int shiftNumber = usersListWorkingOut[i].WorkingOutList[j].ShiftNumber;

                    countDaysFromMonth = CountDaysFromMonth(usersListWorkingOut[i].WorkingOutList[j].ShiftDate);

                    int timeWorkigOut = usersListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        int index = listView2.Items.IndexOfKey("u" + usersListWorkingOut[i].Id);

                        if (index >= 0)
                        {
                            ListViewItem item = listView2.Items[index];

                            if (item != null)
                            {
                                item.SubItems[(day - 1) * countShifts + shiftNumber + 1].Text = timeValues.MinuteToTimeString(timeWorkigOut);
                                //item.SubItems[day + 1].Text = percentWorkingOut.ToString("P1");
                            }
                        }

                        string key = "u" + usersListWorkingOut[i].Id;

                        if (rowIndexes.ContainsKey(key))
                        {
                            int indexRow = rowIndexes[key];

                            dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                        }
                    }));
                }

                int fullTimeWorkigOut = usersListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    int index = listView2.Items.IndexOfKey("u" + usersListWorkingOut[i].Id);

                    if (index >= 0)
                    {
                        ListViewItem item = listView2.Items[index];

                        if (item != null)
                        {
                            item.SubItems[countDaysFromMonth * countShifts + 2].Text = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutSumm);
                            //item.SubItems[countDaysFromMonth * countShifts + 3].Text = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutBacklog);
                            item.SubItems[countDaysFromMonth * countShifts + 3].Text = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutList.Count * fullOutput - usersListWorkingOut[i].WorkingOutSumm);
                        }
                    }

                    string key = "u" + usersListWorkingOut[i].Id;

                    if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutList.Count * fullOutput - usersListWorkingOut[i].WorkingOutSumm);
                    }
                }));
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
            INISettings iniSettings = new INISettings();

            List<int> equips = new List<int>();

            string equipStr = iniSettings.GetViewedEquipment();

            //var numbers = sNumbers?.Split(',')?.Select(Int32.Parse)?.ToList();
            equips = equipStr?.Split(';')?.Select(Int32.Parse)?.ToList();

            //equips.AddRange(equipsArray);

            /*equips.Add(9);
            equips.Add(15);
            equips.Add(38);
            equips.Add(8);
            equips.Add(11);
            equips.Add(13);
            equips.Add(3);
            equips.Add(4);
            equips.Add(5);*/

            return equips;
        }

        private int CountDaysFromMonth(string date)
        {
            DateTime selectDate = Convert.ToDateTime(date);

            int countDaysFromSellectedMonth = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            return countDaysFromSellectedMonth;
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

        private void LoadOrdersSelectedDateAndShift()
        {
            listView1.Items.Clear();

            ValueShifts shifts = new ValueShifts();

            DateTime selectDate = dateTimePicker1.Value;
            int selectShift = comboBox1.SelectedIndex + 1;

            List<User> usersShiftList = shifts.LoadOrders(selectDate, selectShift);

            for(int i = 0; i < usersShiftList.Count; i++)
            {
                User user = usersShiftList[i];

                for (int j = 0; j < user.Shifts[0].Orders.Count; j++)
                {
                    ListViewItem item = new ListViewItem();

                    item.Name = user.Id.ToString();
                    item.Text = listView1.Items.Count.ToString();
                    item.SubItems.Add(users[user.Id]);
                    item.SubItems.Add(machines[user.Equip]);
                    item.SubItems.Add(user.Shifts[0].Orders[j].OrderNumber);
                    item.SubItems.Add(user.Shifts[0].Orders[j].OrderName);
                    item.SubItems.Add(user.Shifts[0].Orders[j].DateBegin.ToString());
                    item.SubItems.Add(user.Shifts[0].Orders[j].Normtime.ToString());
                    item.SubItems.Add(user.Shifts[0].Orders[j].FactOutQty.ToString());
                    item.SubItems.Add(user.Shifts[0].Orders[j].PlanOutQty.ToString());

                    listView1.Items.Add(item);
                }
                

                
            }

            
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
            ChangeDate();
        }

        private void metroSetTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (metroSetTabControlPreviousIndex == 2)
            {
                SaveCheckedEquipsToIniFile();
            }

            if (metroSetTabControl1.SelectedIndex == 1)
            {
                UpdateStatistics();
            }

            if (metroSetTabControl1.SelectedIndex == 2)
            {
                LoadCheckedEquipsFromIniFile();
            }

            

            metroSetTabControlPreviousIndex = metroSetTabControl1.SelectedIndex;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrdersSelectedDateAndShift();
        }
    }
}
