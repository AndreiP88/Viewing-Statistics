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
        bool loadCategoryList = true;

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

                /*foreach(KeyValuePair<int, string> equip in machines)
                {
                    ListViewItem item = new ListViewItem();

                    item.Name = equip.Key.ToString();
                    item.Text = (ListViewEquips.Items.Count + 1).ToString("D2");
                    item.SubItems.Add(equip.Value);

                    ListViewEquips.Items.Add(item);
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        /*private void LoadCheckedEquipsFromIniFile()
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
        }*/

        /*private void SaveCheckedEquipsToIniFile()
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
        }*/

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




            /*ListViewItem item = new ListViewItem();

            int id = listViewCategory.Items.Count + 1;

            while (true)
            {
                if (listViewCategory.Items.IndexOfKey(id.ToString()) == -1)
                {
                    break;
                }
                else
                {
                    id++;
                }
            }

            item.Name = id.ToString();
            item.Text = (listViewCategory.Items.Count + 1).ToString();
            item.SubItems.Add(name);

            item.Checked = false;

            listViewCategory.Items.Add(item);

            SaveCategoryToIniFile();*/
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
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

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

        private float GetPercentWorkingOut(int targetWorkingOut, int facticalWorkingOut)
        {
            float result;

            result = (float)facticalWorkingOut / targetWorkingOut;

            return result;
        }

        /*private List<int> GetSelectegEquipsList()
        {
            INISettings iniSettings = new INISettings();

            List<int> equips = new List<int>();

            string equipStr = iniSettings.GetViewedEquipment();

            //var numbers = sNumbers?.Split(',')?.Select(Int32.Parse)?.ToList();
            equips = equipStr?.Split(';')?.Select(Int32.Parse)?.ToList();

            //equips.AddRange(equipsArray);

            *//*equips.Add(9);
            equips.Add(15);
            equips.Add(38);
            equips.Add(8);
            equips.Add(11);
            equips.Add(13);
            equips.Add(3);
            equips.Add(4);
            equips.Add(5);*//*

            return equips;
        }*/

        /*private List<Category> GetSelectedCategoriesAndEquipsList()
        {
            List<Category> categories = new List<Category>();

            string startStrSection = "category_";

            IniFile ini = new IniFile("settings.ini");

            string[] sections = ini.GetAllSections();

            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].StartsWith(startStrSection))
                {
                    string name = "";
                    bool selected = false;
                    string equipsStr = "";
                    List<Equip> equipsForCategory = new List<Equip>();

                    if (ini.KeyExists("name", sections[i]))
                        name = ini.ReadString("name", sections[i]);

                    if (ini.KeyExists("selected", sections[i]))
                        selected = ini.ReadBool("selected", sections[i]);

                    if (ini.KeyExists("equips", sections[i]))
                    {
                        equipsStr = ini.ReadString("equips", sections[i]);

                        //equipsForCategory = equipsStr?.Split(';')?.Select(Int32.Parse)?.ToList();

                        List<string> strings = equipsStr?.Split(';')?.ToList();

                        for (int j = 0; j < strings.Count; j++)
                        {
                            bool selectedEquip;

                            string[] stringsEquip = strings[j]?.Split(':');

                            int idEquip = Convert.ToInt32(stringsEquip[0]);

                            if (stringsEquip[1] == "1")
                            {
                                selectedEquip = true;
                            }
                            else
                            {
                                selectedEquip = false;
                            }

                            equipsForCategory.Add(new Equip(
                                idEquip,
                                selectedEquip
                                ));
                        }
                    }

                    int idCategory = Convert.ToInt32(sections[i].Substring(startStrSection.Length));

                    categories.Add(new Category(
                        idCategory,
                        name,
                        selected,
                        equipsForCategory
                        ));
                }
                //categoryes.Add(Convert.ToInt32(sections[i].Substring(startStrSection.Length)));
            }

            categories.Sort((v, s) => v.Id.CompareTo(s.Id));

            return categories;
        }*/

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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadOrdersSelectedDateAndShift();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrdersSelectedDateAndShift();
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

                listViewEquips.Items.Clear();
            }

            if (metroSetTabControl1.SelectedIndex == 1)
            {
                UpdateStatistics();
            }

            if (metroSetTabControl1.SelectedIndex == 2)
            {
                LoadCategoryToListView();
            }

            metroSetTabControlPreviousIndex = metroSetTabControl1.SelectedIndex;
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
                EnabledButtonsForCategory(-1, listViewEquips.Items.Count);
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
    }
}
