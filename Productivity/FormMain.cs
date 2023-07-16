using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using OrderManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Policy;

namespace Productivity
{
    public partial class FormMain : MetroSetForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        CancellationTokenSource cancelTokenSource;

        int metroSetTabControlPreviousIndex = -1;
        bool loadCategoryList = true;

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();
        Dictionary<string, int> rowIndexes = new Dictionary<string, int>();

        List<User> usersList;

        DateTime lastTimeUpdateShiftStatistic = DateTime.Now;

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


        private void LoadCategoryToListView()
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();
            List<Category> categories = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            loadCategoryList = true;

            listViewCategory.Items.Clear();

            for (int i = 0; i < categories.Count; i++)
            {
                ListViewItem item = new ListViewItem();

                item.Name = categories[i].Id.ToString();
                item.Text = (listViewCategory.Items.Count + 1).ToString();
                item.SubItems.Add(categories[i].Name);

                item.Checked = categories[i].Selected;

                listViewCategory.Items.Add(item);
            }

            loadCategoryList = false;
        }

        private void LoadEquipsFromCategoryToListView(int idCategory)
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();
            List<Category> categories = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            listViewEquips.Items.Clear();

            int index = categories.FindIndex((v) => v.Id == idCategory);

            if (index != -1)
            {
                for (int i = 0; i < categories[index].Equips.Count; i++)
                {
                    ListViewItem item = new ListViewItem();

                    string equip = "";

                    if (machines.ContainsKey(categories[index].Equips[i].Id))
                    {
                        equip = machines[categories[index].Equips[i].Id];
                    }

                    item.Name = categories[index].Equips[i].Id.ToString();
                    item.Text = (listViewEquips.Items.Count + 1).ToString();
                    item.SubItems.Add(equip);

                    item.Checked = categories[index].Equips[i].Selected;

                    listViewEquips.Items.Add(item);
                }
            }
        }

        private void SaveCategoryToIniFile()
        {
            IniFile ini = new IniFile("settings.ini");

            for (int i = 0; i < listViewCategory.Items.Count; i++)
            {
                string name = listViewCategory.Items[i].SubItems[1].Text;

                bool selected = listViewCategory.Items[i].Checked;

                string section = "category_" + listViewCategory.Items[i].Name;

                ini.Write("name", name, section);
                ini.Write("selected", selected.ToString(), section);
            }
        }

        private void SaveEquipsFromCategoryToIniFile(int idCategory)
        {
            IniFile ini = new IniFile("settings.ini");

            string outputStr = "";

            for (int i = 0; i < listViewEquips.Items.Count; i++)
            {
                int selected = 0;

                if (listViewEquips.Items[i].Checked)
                {
                    selected = 1;
                }
                else
                {
                    selected = 0;
                }

                if (i < listViewEquips.Items.Count - 1)
                {
                    outputStr += listViewEquips.Items[i].Name + ":" + selected + ";";
                }
                else
                {
                    outputStr += listViewEquips.Items[i].Name + ":" + selected;
                }
            }

            string section = "category_" + idCategory;

            ini.Write("equips", outputStr, section);
        }

        private void AddNewCategory(string name)
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();
            List<Category> categories = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            IniFile ini = new IniFile("settings.ini");

            int id = categories.Count + 1;

            while (true)
            {
                if (categories.FindIndex((v) => v.Id == id) == -1)
                {
                    break;
                }
                else
                {
                    id++;
                }
            }

            string section = "category_" + id;

            ini.Write("name", name, section);
            ini.Write("selected", false.ToString(), section);
            ini.Write("equips", "", section);

            LoadCategoryToListView();
        }

        private void EditNameCategory(int categoryId, string newName)
        {
            IniFile ini = new IniFile("settings.ini");

            string section = "category_" + categoryId;

            ini.Write("name", newName, section);

            LoadCategoryToListView();
        }

        private void DeleteCategory(int categoryId)
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();
            IniFile ini = new IniFile("settings.ini");

            string section = "category_" + categoryId;

            //ini.DeleteSection(section);

            List<Category> categories = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            string sectionLast = "category_" + categories.Count;

            for (int i = categoryId + 1; i <= categories.Count; i++)
            {
                ChangeCategoryID(i, i - 1);
            }

            ini.DeleteSection(sectionLast);

            LoadCategoryToListView();
            listViewEquips.Items.Clear();
        }

        private void MoveCategoryUp(int categoryId)
        {
            SwapCategory(categoryId, categoryId - 1);

            LoadCategoryToListView();
        }

        private void MoveCategoryDown(int categoryId)
        {
            SwapCategory(categoryId, categoryId + 1);

            LoadCategoryToListView();
        }

        private void SwapCategory(int firstCategory, int secondCategory)
        {
            IniFile ini = new IniFile("settings.ini");

            string firstSection = "category_" + firstCategory;
            string secondSection = "category_" + secondCategory;

            string firstName = "";
            bool firstSelected = false;
            string firstEquipsStr = "";

            if (ini.KeyExists("name", firstSection))
                firstName = ini.ReadString("name", firstSection);

            if (ini.KeyExists("selected", firstSection))
                firstSelected = ini.ReadBool("selected", firstSection);

            if (ini.KeyExists("equips", firstSection))
                firstEquipsStr = ini.ReadString("equips", firstSection);

            string secondName = "";
            bool secondSelected = false;
            string secondEquipsStr = "";

            if (ini.KeyExists("name", secondSection))
                secondName = ini.ReadString("name", secondSection);

            if (ini.KeyExists("selected", secondSection))
                secondSelected = ini.ReadBool("selected", secondSection);

            if (ini.KeyExists("equips", secondSection))
                secondEquipsStr = ini.ReadString("equips", secondSection);

            //ini.DeleteSection(oldSection);

            ini.Write("name", secondName, firstSection);
            ini.Write("selected", secondSelected.ToString(), firstSection);
            ini.Write("equips", secondEquipsStr, firstSection);

            ini.Write("name", firstName, secondSection);
            ini.Write("selected", firstSelected.ToString(), secondSection);
            ini.Write("equips", firstEquipsStr, secondSection);
        }

        private void ChangeCategoryID(int oldCategoryId, int newCategoryId)
        {
            IniFile ini = new IniFile("settings.ini");

            string oldSection = "category_" + oldCategoryId;
            string newSection = "category_" + newCategoryId;

            string name = "";
            bool selected = false;
            string equipsStr = "";

            if (ini.KeyExists("name", oldSection))
                name = ini.ReadString("name", oldSection);

            if (ini.KeyExists("selected", oldSection))
                selected = ini.ReadBool("selected", oldSection);

            if (ini.KeyExists("equips", oldSection))
                equipsStr = ini.ReadString("equips", oldSection);

            ini.DeleteSection(oldSection);

            ini.Write("name", name, newSection);
            ini.Write("selected", selected.ToString(), newSection);
            ini.Write("equips", equipsStr, newSection);
        }

        private void AddNewEquips(List<string> equipsList)
        {
            loadCategoryList = true;

            for (int i = 0; i < equipsList.Count; i++)
            {
                string name = "";

                if (machines.ContainsKey(Convert.ToInt32(equipsList[i])))
                {
                    name = machines[Convert.ToInt32(equipsList[i])];
                }

                ListViewItem item = new ListViewItem();

                item.Name = equipsList[i];
                item.Text = (listViewEquips.Items.Count + 1).ToString();
                item.SubItems.Add(name);

                listViewEquips.Items.Add(item);
            }

            loadCategoryList = false;

            int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

            SaveEquipsFromCategoryToIniFile(idCategory);
        }

        private void DeleteEquip(int index)
        {
            if (listViewEquips.SelectedItems.Count > 0)
            {
                listViewEquips.Items.RemoveAt(index);
            }

            int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

            SaveEquipsFromCategoryToIniFile(idCategory);
        }

        private void MoveEquipUp(int index)
        {
            SwapEquips(index, index - 1);

            int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

            SaveEquipsFromCategoryToIniFile(idCategory);
        }

        private void MoveEquipDown(int index)
        {
            SwapEquips(index, index + 1);

            int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

            SaveEquipsFromCategoryToIniFile(idCategory);
        }

        private void SwapEquips(int firstEquip, int secondEquip)
        {
            /*ListViewItem firstItem = listViewEquips.Items[firstEquip];
            ListViewItem secondItem = listViewEquips.Items[secondEquip];

            listViewEquips.Items.Remove(firstItem);
            listViewEquips.Items.Remove(secondItem);

            listViewEquips.Items.Insert(secondEquip, firstItem);
            listViewEquips.Items.Insert(firstEquip, secondItem);*/

            string firstName = listViewEquips.Items[firstEquip].Name;
            string firstSubName = listViewEquips.Items[firstEquip].SubItems[1].Text;
            bool firstSelected = listViewEquips.Items[firstEquip].Checked;

            string secondName = listViewEquips.Items[secondEquip].Name;
            string seconSubName = listViewEquips.Items[secondEquip].SubItems[1].Text;
            bool secondSelected = listViewEquips.Items[secondEquip].Checked;

            listViewEquips.Items[firstEquip].Name = secondName;
            listViewEquips.Items[firstEquip].SubItems[1].Text = seconSubName;
            listViewEquips.Items[firstEquip].Checked = secondSelected;

            listViewEquips.Items[secondEquip].Name = firstName;
            listViewEquips.Items[secondEquip].SubItems[1].Text = firstSubName;
            listViewEquips.Items[secondEquip].Checked = firstSelected;
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

        private void CreateColomnsToDataGrid(int days, int month)
        {
            INISettings settings= new INISettings();

            int countShifts = settings.GetCountShifts();

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
            dataGridView1.Columns[days * countShifts + 2].Width = 100;
            dataGridView1.Columns[days * countShifts + 2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns.Add(@"colTask", @"Отставание");
            dataGridView1.Columns[days * countShifts + 3].Width = 100;
            dataGridView1.Columns[days * countShifts + 3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Frozen = true;

            AddCellToGrid(0, 0, 2);
            dataGridView1.Rows[0].Cells[0].Value = "";

            AddCellToGrid(0, days * countShifts + 2, 2);
            dataGridView1.Rows[0].Cells[days * countShifts + 2].Value = "";

            for (int i = 2; i <= days * countShifts; i+=countShifts)
            {
                AddCellToGrid(0, i, countShifts);

                dataGridView1.Rows[0].Cells[i].Value = ((i - 2 + countShifts) / countShifts).ToString("D2") + "." + month.ToString("D2");
            }

            dataGridView1.Rows.Add();
            dataGridView1.Rows[1].Frozen = true;

            AddCellToGrid(1, 0, 2);
            dataGridView1.Rows[1].Cells[0].Value = "Имя";

            AddCellToGrid(1, days * countShifts + 2);
            dataGridView1.Rows[1].Cells[days * countShifts + 2].Value = "Выработка";

            AddCellToGrid(1, days * countShifts + 3);
            dataGridView1.Rows[1].Cells[days * countShifts + 3].Value = "Отставание";

            for (int i = 2; i <= days * countShifts + 1; i += countShifts)
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
            INISettings settings = new INISettings();

            LoadAllUsers();
            LoadMachine();

            AddYearsToComboBox(2015, 2050);
            AddMonthToComboBox();

            SelectCurrentMonth();

            //Update Later
            comboBox4.SelectedIndex = 0;

            metroSetTabControl1.SelectTab(settings.GetLastTabIndex());

            TabControlSelectedIndexChanged(metroSetTabControl1.SelectedIndex);
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

        private List<int> CategoryEquipToListSelectedEquip(List<Category> categories)
        {
            List<int> equips = new List<int>();

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i].Selected)
                {
                    for (int j = 0; j < categories[i].Equips.Count; j++)
                    {
                        if (categories[i].Equips[j].Selected)
                        {
                            equips.Add(categories[i].Equips[j].Id);
                        }
                    }
                }
            }

            return equips;
        }

        private void LoadUsersList(List<Category> categoryEquips, DateTime date)
        {
            try
            {
                usersList = new List<User>();

                ValueUsers usersValue = new ValueUsers();

                List<int> equips = CategoryEquipToListSelectedEquip(categoryEquips);

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
            ValueShifts valueShifts = new ValueShifts();

            INISettings settings = new INISettings();

            int countShifts = settings.GetCountShifts();

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            usersList = valueShifts.LoadShifts(usersList, selectDate, countShifts);

            StartAddingWorkingTimeToListView();
        }

        private void AddUsersToListView(int countDaysFromSellectedMonth)
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();
            INISettings settings = new INISettings();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

            bool viewAllEquipsForUser = settings.GetLoadAllEquipForUser();

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

                            AddItemToGrid(CreateNameListViewItem(equips[i], usersList[j].Id), countUserForCurrentEquip.ToString(), user, color);
                        }
                    }
                }
            }
            else
            {
                List<int> usersCurrent = new List<int>();
                List<int> equipsCurrent = new List<int>();

                for (int i = 0; i < equips.Count; i++)
                {
                    for (int j = 0; j < usersList.Count; j++)
                    {
                        if (!usersCurrent.Contains(usersList[j].Id) && usersList[j].Equip == equips[i])
                        {
                            usersCurrent.Add(usersList[j].Id);
                        }

                        if (viewAllEquipsForUser && !equipsCurrent.Contains(usersList[j].Equip))
                        {
                            equipsCurrent.Add(usersList[j].Equip);
                        }
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

                            AddItemToGrid(CreateNameListViewItem(equipsCurrent[j], usersList[index].Id), countEquipForCurrentUser.ToString(), machine, color);
                        }
                    }
                }
            }
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
            INISettings settings = new INISettings();
            ValueDateTime timeValues = new ValueDateTime();

            int fullOutput = settings.GetNormTime();
            int countShifts = settings.GetCountShifts();

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

        private int CalculateWorkTimeN(List<UserShiftOrder> order)
        {
            float workingOut = 0;

            /*for (int i = 0; i < order.Count; i++)
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
            }*/

            for (int i = 0; i < order.Count; i++)
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

            return (int)workingOut;
        }

        private int CalculateWorkTimeForOneOrder(UserShiftOrder order)
        {
            float workingOut = 0;

            if (order.Flags == 576)
            {
                workingOut += order.Normtime;
            }

            if (order.Flags == 512 || order.Flags == 544)
            {
                if (order.Normtime > 0)
                {
                    float norm = order.PlanOutQty / order.Normtime;

                    if (norm > 0)
                    {
                        workingOut += order.FactOutQty / norm;
                    }
                }
            }

            /*if (order.Normtime > 0)
            {
                float norm = order.PlanOutQty / order.Normtime;

                if (norm > 0)
                {
                    workingOut += order.FactOutQty / norm;
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

        private int CountDaysFromMonth(string date)
        {
            DateTime selectDate = Convert.ToDateTime(date);

            int countDaysFromSellectedMonth = DateTime.DaysInMonth(selectDate.Year, selectDate.Month);

            return countDaysFromSellectedMonth;
        }


        private void UpdateStatistics()
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            int countDaysFromSellectedMonth = DateTime.DaysInMonth(year, month);

            CreateColomnsToDataGrid(countDaysFromSellectedMonth, month);

            //List<int> equips = GetSelectegEquipsList();
            List<Category> categoryEquips = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            LoadUsersList(categoryEquips, selectDate);

            AddUsersToListView(countDaysFromSellectedMonth);

            LoadShifts();
        }

        private void EnabledButtonsForCategory(int item, int itemsCount)
        {
            if (item >= 0)
            {
                buttonCatEdit.Enabled = true;
                buttonCatDelete.Enabled = true;
                buttonEquipAdd.Enabled = true;

                if (item == 0)
                {
                    buttonCatUp.Enabled = false;
                }
                else
                {
                    buttonCatUp.Enabled = true;
                }

                if (item == itemsCount - 1)
                {
                    buttonCatDown.Enabled = false;
                }
                else
                {
                    buttonCatDown.Enabled = true;
                }
            }
            else
            {
                buttonCatEdit.Enabled = false;
                buttonCatDelete.Enabled = false;
                buttonCatUp.Enabled = false;
                buttonCatDown.Enabled = false;
                buttonEquipAdd.Enabled = false;
            }
        }

        private void EnabledButtonsForEquips(int item, int itemsCount)
        {
            if (item >= 0)
            {
                buttonEquipDel.Enabled = true;

                if (item == 0)
                {
                    buttonEquipUp.Enabled = false;
                }
                else
                {
                    buttonEquipUp.Enabled = true;
                }

                if (item == itemsCount - 1)
                {
                    buttonEquipDown.Enabled = false;
                }
                else
                {
                    buttonEquipDown.Enabled = true;
                }
            }
            else
            {
                buttonEquipDel.Enabled = false;
                buttonEquipUp.Enabled = false;
                buttonEquipDown.Enabled = false;
            }
        }

        private void SelectCurrentShift()
        {
            DateTime dateTime = DateTime.Now;

            if (dateTime.Hour < 8 || dateTime.Hour >= 20)
            {
                comboBox1.SelectedIndex = 1;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }

            if (dateTime.Hour < 8 && dateTime.Hour >= 0)
            {
                dateTimePicker1.Value = dateTime.AddDays(-1);
            }
            else
            {
                dateTimePicker1.Value = dateTime;
            }
        }

        private void LoadOrdersSelectedDateAndShift(DateTime selectDate, int selectShift)
        {
            listView1.Items.Clear();

            ValueShifts shifts = new ValueShifts();
            ValueDateTime time = new ValueDateTime();
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

            string timeStartShift = time.StartShiftPlanedDateTime(selectDate, selectShift);

            List<User> usersShiftList = shifts.LoadOrders(selectDate, selectShift);

            List<int> usersCurrent = new List<int>();

            //Пока так, потом сделаю отдельную выборку оборудования с сортировкой, либо без сортирвки а в порядке загрузки
            for (int i = 0; i < equips.Count; i++)
            {
                for (int j = 0; j < usersShiftList.Count; j++)
                {

                    if (usersShiftList[j].Equip == equips[i])
                    {
                        if (!usersCurrent.Contains(usersShiftList[j].Id))
                        {
                            usersCurrent.Add(usersShiftList[j].Id);
                        }
                    }
                }
            }

            /*for (int i = 0; i < usersShiftList.Count; i++)
            {
                if (!usersCurrent.Contains(usersShiftList[i].Id))
                {
                    usersCurrent.Add(usersShiftList[i].Id);
                }
            }*/

            for (int i = 0; i < usersCurrent.Count; i++)
            {
                ListViewItem item = new ListViewItem();

                item.Name = "u" + usersCurrent[i].ToString();
                item.Text = (i + 1).ToString();
                item.SubItems.Add(users[usersCurrent[i]]);
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");
                item.SubItems.Add("");

                item.BackColor = Color.Gray;
                item.Font = new Font(listView1.Font, FontStyle.Bold);

                listView1.Items.Add(item);

                float userWorkingOut = 0;
                int userDone = 0;
                int indexRowForUser = listView1.Items.Count - 1;

                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    if (usersShiftList[j].Id == usersCurrent[i])
                    {
                        User user = usersShiftList[j];
                        List<int> ordersIdManOrderJobItem = new List<int>();

                        //List<UserShiftOrder> ordersIdManOrderJobItem = new List<UserShiftOrder>();

                        for (int k = 0; k < user.Shifts[0].Orders.Count; k++)
                        {
                            if (!ordersIdManOrderJobItem.Contains(user.Shifts[0].Orders[k].IdManOrderJobItem))
                            {
                                ordersIdManOrderJobItem.Add(user.Shifts[0].Orders[k].IdManOrderJobItem);
                            }
                        }

                        for (int k = 0; k < ordersIdManOrderJobItem.Count; k++)
                        {
                            List<int> indexesUserShiftsOrders = new List<int>();

                            for (int l = 0; l < user.Shifts[0].Orders.Count; l++)
                            {
                                if (user.Shifts[0].Orders[l].IdManOrderJobItem == ordersIdManOrderJobItem[k])
                                {
                                    indexesUserShiftsOrders.Add(l);
                                }
                            }

                            UserShiftOrder order = user.Shifts[0].Orders[indexesUserShiftsOrders[0]];

                            int orderPreviousAmount = shifts.GetAmountDoneFromPreviousShifts(ordersIdManOrderJobItem[k], order.DateBegin);

                            float workingOut = 0;
                            int done = 0;
                            int duration = 0;
                            int amount = 0;
                            string timePlanedEndOrder = "";
                            int normTimeMakeReady = 0;
                            int normTimeWork = 0;
                            int differentTime = 0;

                            for (int l = 0; l < indexesUserShiftsOrders.Count; l++)
                            {
                                UserShiftOrder orderCur = user.Shifts[0].Orders[indexesUserShiftsOrders[l]];

                                workingOut += CalculateWorkTimeForOneOrder(orderCur);

                                if (orderCur.Flags != 576)
                                {
                                    done += orderCur.FactOutQty;
                                    amount = orderCur.PlanOutQty;
                                    normTimeWork = orderCur.Normtime;
                                }

                                if (orderCur.Flags == 576)
                                {
                                    normTimeMakeReady = orderCur.Normtime;
                                }

                                duration += orderCur.Duration;
                            }

                            string lastTimeEndPlanedOrder = time.DateTimeAmountMunutes(timeStartShift, (int)userWorkingOut);

                            int lastAmount = amount - orderPreviousAmount;
                            //MessageBox.Show(lastAmount + " = " + order.PlanOutQty + " - " + orderPreviousAmount);

                            userWorkingOut += (int)workingOut;
                            userDone += done;

                            int[] normtime = shifts.GetNormTimeForOrder(order.IdManOrderJobItem);
                            int normTimeFull = 0;

                            float lastTime = 0;

                            if (normtime[1] > 0)
                            {
                                float norm = amount / normtime[1];

                                if (norm > 0)
                                {
                                    lastTime = lastAmount / norm;
                                }
                            }

                            if (order.Status == 2)
                            {
                                if (orderPreviousAmount > 0)
                                {
                                    normTimeFull = (int)lastTime;
                                }
                                else
                                {
                                    normTimeFull = normTimeMakeReady + normTimeWork;
                                }

                                timePlanedEndOrder = time.DateTimeAmountMunutes(timeStartShift, (int)userWorkingOut);

                                differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd);
                            }

                            if (order.Status != 2)
                            {
                                if (orderPreviousAmount > 0)
                                {
                                    normTimeFull = (int)lastTime;
                                }
                                else
                                {
                                    normTimeFull = normtime[0] + normtime[1];
                                }
                                
                                timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull);

                                differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, DateTime.Now.ToString());
                            }

                            ListViewItem subItem = new ListViewItem();

                            subItem.Name = user.Id.ToString();
                            subItem.Text = (k + 1).ToString();
                            //subItem.SubItems.Add("");
                            subItem.SubItems.Add("    " + machines[user.Equip]);
                            subItem.SubItems.Add(order.OrderNumber);
                            subItem.SubItems.Add(order.OrderName);
                            subItem.SubItems.Add(lastAmount.ToString("N0") + " | " + amount.ToString("N0"));
                            subItem.SubItems.Add(time.MinuteToTimeString(normTimeFull));
                            subItem.SubItems.Add(user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateBegin);
                            subItem.SubItems.Add(user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd);
                            subItem.SubItems.Add(time.MinuteToTimeString(duration));
                            subItem.SubItems.Add(timePlanedEndOrder);
                            subItem.SubItems.Add(time.MinuteToTimeString(differentTime));
                            subItem.SubItems.Add(done.ToString("N0"));
                            subItem.SubItems.Add(time.MinuteToTimeString((int)workingOut));

                            listView1.Items.Add(subItem);
                        }
                    }
                }

                listView1.Items[indexRowForUser].SubItems[11].Text = userDone.ToString("N0");
                listView1.Items[indexRowForUser].SubItems[12].Text = time.MinuteToTimeString((int)userWorkingOut);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectDate = dateTimePicker1.Value;
            int selectShift = comboBox1.SelectedIndex + 1;

            LoadOrdersSelectedDateAndShift(selectDate, selectShift);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime selectDate = dateTimePicker1.Value;
            int selectShift = comboBox1.SelectedIndex + 1;

            LoadOrdersSelectedDateAndShift(selectDate, selectShift);
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
                SaveCategoryToIniFile();
                SaveParameterToIniFile();

                listViewEquips.Items.Clear();
            }

            TabControlSelectedIndexChanged(metroSetTabControl1.SelectedIndex);

            metroSetTabControlPreviousIndex = metroSetTabControl1.SelectedIndex;
        }

        private void TabControlSelectedIndexChanged(int selectedIndex)
        {
            INISettings settings = new INISettings();

            if (selectedIndex == 0)
            {
                metroSetSwitch1.Switched = settings.GetLoadCurrentShift();
                metroSetSwitch2.Switched = settings.GetAutoUpdateStatistic();
                formattedNumericUpDown4.Value = settings.GetPeriodAutoUpdateStatistic();
            }

            if (selectedIndex == 1)
            {
                UpdateStatistics();
            }

            if (selectedIndex == 2)
            {
                LoadCategoryToListView();
                LoadParameterFromIniFile();
            }

            settings.SetLastTabIndex(selectedIndex);
        }

        private void LoadParameterFromIniFile()
        {
            INISettings settings = new INISettings();
            ValueDateTime time = new ValueDateTime();

            int normTime = settings.GetNormTime();

            int[] hoursAndMinutes = time.TotalMinutesToHoursAndMinutes(normTime);

            int hours = hoursAndMinutes[0];
            int minutes = hoursAndMinutes[1];

            formattedNumericUpDown1.Value = hours;
            formattedNumericUpDown2.Value = minutes;

            int countShifts = settings.GetCountShifts();

            formattedNumericUpDown3.Value = countShifts;

            bool loadAllEquipForUser = settings.GetLoadAllEquipForUser();

            metroSetCheckBox1.Checked = loadAllEquipForUser;
        }

        private void SaveParameterToIniFile()
        {
            INISettings settings = new INISettings();
            ValueDateTime time = new ValueDateTime();

            int hours = (int)formattedNumericUpDown1.Value;
            int minutes = (int)formattedNumericUpDown2.Value;

            int normTime = time.HoursAndMinutesToTotalMinutes(hours, minutes);

            settings.SetNormTime(normTime);

            int countShifts = (int)formattedNumericUpDown3.Value;

            settings.SetCountShifts(countShifts);

            bool loadAllEquipForUser = metroSetCheckBox1.Checked;

            settings.SetLoadAllEquipForUser(loadAllEquipForUser);
        }

        private void listViewCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCategory.SelectedItems.Count > 0)
            {
                int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

                //LoadEquipsFromCategoryToListView(idCategory);
                EnabledButtonsForCategory(listViewCategory.SelectedIndices[0], listViewCategory.Items.Count);
            }
            else
            {
                EnabledButtonsForCategory(-1, listViewCategory.Items.Count);
                EnabledButtonsForEquips(-1, listViewEquips.Items.Count);
                listViewEquips.Items.Clear();
            }
        }

        private void listViewCategory_Click(object sender, EventArgs e)
        {
            if (listViewCategory.SelectedItems.Count > 0)
            {
                int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

                LoadEquipsFromCategoryToListView(idCategory);
            }

            //EnabledButtonsForCategory(listViewCategory.SelectedIndices[0], listViewCategory.Items.Count);
        }

        private void buttonAddCat_Click(object sender, EventArgs e)
        {
            FormAddEditCategory fm = new FormAddEditCategory();
            fm.ShowDialog();

            if (fm.NewValue)
            {
                AddNewCategory(fm.NameCategory);
            }
        }

        private void buttonCatEdit_Click(object sender, EventArgs e)
        {
            if (listViewCategory.SelectedItems.Count > 0)
            {
                int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

                FormAddEditCategory fm = new FormAddEditCategory(listViewCategory.SelectedItems[0].SubItems[1].Text);
                fm.ShowDialog();

                if (fm.NewValue)
                {
                    EditNameCategory(idCategory, fm.NameCategory);
                }
            }
        }

        private void buttonCatDelete_Click(object sender, EventArgs e)
        {
            if (listViewCategory.SelectedItems.Count > 0)
            {
                DialogResult result;

                result = MessageBox.Show("Вы действительно хотите удалить участок: " + listViewCategory.SelectedItems[0].SubItems[1].Text + "?", "Удаление участка", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

                    DeleteCategory(idCategory);
                }
            }
        }

        private void buttonCatUp_Click(object sender, EventArgs e)
        {
            if (listViewCategory.SelectedItems.Count > 0)
            {
                int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

                MoveCategoryUp(idCategory);

                int index = listViewCategory.Items.IndexOfKey(idCategory.ToString());

                listViewCategory.Items[index - 1].Selected = true;
            }
        }

        private void buttonCatDown_Click(object sender, EventArgs e)
        {
            if (listViewCategory.SelectedItems.Count > 0)
            {
                int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

                MoveCategoryDown(idCategory);

                int index = listViewCategory.Items.IndexOfKey(idCategory.ToString());

                listViewCategory.Items[index + 1].Selected = true;
            }
        }

        private void listViewEquips_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEquips.SelectedItems.Count > 0)
            {
                EnabledButtonsForEquips(listViewEquips.SelectedIndices[0], listViewEquips.Items.Count);
            }
            else
            {
                EnabledButtonsForEquips(-1, listViewEquips.Items.Count);
            }
        }

        private void listViewEquips_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!loadCategoryList && listViewCategory.SelectedItems.Count > 0)
            {
                int idCategory = Convert.ToInt32(listViewCategory.SelectedItems[0].Name);

                SaveEquipsFromCategoryToIniFile(idCategory);
            }
            //MessageBox.Show("test");
        }

        private void buttonEquipAdd_Click(object sender, EventArgs e)
        {
            FormAddEquips fm = new FormAddEquips();
            fm.ShowDialog();

            if (fm.NewValue)
            {
                AddNewEquips(fm.EquipsList);
            }
        }

        private void buttonEquipDel_Click(object sender, EventArgs e)
        {
            if (listViewCategory.SelectedItems.Count > 0)
            {
                DialogResult result;

                result = MessageBox.Show("Вы действительно хотите удалить: " + listViewEquips.SelectedItems[0].SubItems[1].Text + "?", "Удаление оборудования", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteEquip(listViewEquips.SelectedIndices[0]);
                }
            }
        }

        private void buttonEquipUp_Click(object sender, EventArgs e)
        {
            if (listViewEquips.SelectedItems.Count > 0)
            {
                int index = listViewEquips.SelectedIndices[0];

                MoveEquipUp(index);

                listViewEquips.Items[index - 1].Selected = true;
            }
        }

        private void buttonEquipDown_Click(object sender, EventArgs e)
        {
            if (listViewEquips.SelectedItems.Count > 0)
            {
                int index = listViewEquips.SelectedIndices[0];

                MoveEquipDown(index);

                listViewEquips.Items[index + 1].Selected = true;
            }
        }

        private void listViewCategory_SizeChanged(object sender, EventArgs e)
        {
            listViewCategory.Columns[1].Width = listViewCategory.Width - 65;
            listViewCategory.Refresh();
        }

        private void listViewEquips_SizeChanged(object sender, EventArgs e)
        {
            listViewEquips.Columns[1].Width = listViewEquips.Width - 65;
            listViewEquips.Refresh();
        }

        private void metroSetSwitch1_SwitchedChanged(object sender)
        {
            INISettings settings = new INISettings();

            if (metroSetSwitch1.Switched)
            {
                SelectCurrentShift();

                dateTimePicker1.Enabled = false;
                comboBox1.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = true;
                comboBox1.Enabled = true;
            }

            settings.SetLoadCurrentShift(metroSetSwitch1.Switched);
        }

        private void LoadOrdersForSelectedDate()
        {
            if (metroSetSwitch1.Switched)
            {
                SelectCurrentShift();
            }

            DateTime selectDate = dateTimePicker1.Value;
            int selectShift = comboBox1.SelectedIndex + 1;

            LoadOrdersSelectedDateAndShift(selectDate, selectShift);

            lastTimeUpdateShiftStatistic = DateTime.Now;
        }

        private void metroSetButton1_Click(object sender, EventArgs e)
        {
            LoadOrdersForSelectedDate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (metroSetSwitch2.Switched)
            {
                int updatePeriod = (int)formattedNumericUpDown4.Value;
                int lostMin = lastTimeUpdateShiftStatistic.AddMinutes(updatePeriod).Subtract(DateTime.Now).Minutes + 1;

                button2.Text = "Обновление (" + lostMin + ")";

                if (lastTimeUpdateShiftStatistic.AddMinutes(updatePeriod) <= DateTime.Now)
                {
                    LoadOrdersForSelectedDate();
                }
            }
        }

        private void metroSetSwitch2_SwitchedChanged(object sender)
        {
            INISettings settings = new INISettings();

            if (metroSetSwitch2.Switched)
            {
                timer1.Enabled = true;

                formattedNumericUpDown4.Visible = true;
                metroSetLabel3.Visible = true;
            }
            else
            {
                timer1.Enabled = false;

                formattedNumericUpDown4.Visible = false;
                metroSetLabel3.Visible = false;

                button2.Text = "Обновление";
            }

            settings.SetAutoUpdateStatistic(metroSetSwitch2.Switched);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadOrdersForSelectedDate();
        }

        private void formattedNumericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            INISettings settings = new INISettings();

            settings.SetPeriodAutoUpdateStatistic((int)formattedNumericUpDown4.Value);
        }
    }
}
