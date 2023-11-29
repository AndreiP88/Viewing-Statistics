using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using libData;
using libINIFile;
using libSql;
using libTime;
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
        int comboBoxSelectViewsPreviousIndex = -1;
        bool loadCategoryList = true;
        bool loadPageList = true;
        bool loadParameter = true;
        bool loadShiftsParameter = true;

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();
        Dictionary<string, int> rowIndexes = new Dictionary<string, int>();

        List<User> usersList;
        List<ViewPath> viewsList;
        List<Page> pages;
        List<OrderHeadSearch> ordersHead;

        DateTime lastTimeUpdateShiftStatistic = DateTime.Now;

        private void StartDowloadUpdater()
        {
            string fileTemp = "Update.exe";

            string link = "https://drive.google.com/uc?export=download&id=1gNtqWboCGjdZ_FKPS-eKoHuVZr9mdgDH";

            var task = Task.Run(() => DowloadUpdater(link, fileTemp));

        }

        private void DowloadUpdater(string link, string path)
        {
            FileDownloader downloader = new FileDownloader();

            try
            {
                downloader.DownloadFile(link, path);
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Ошибка подключения", "Ошибка", MessageBoxButtons.OK);
                LogWrite(ex);
            }

            Invoke(new Action(() =>
            {
                //MessageBox.Show(currentDateV.ToString() + " " + lastDateV.ToString());

            }));
        }

        private void CreateFolder()
        {
            string path = @"TempDownload";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden | FileAttributes.System;
            }
        }

        private void LogWrite(Exception ex)
        {
            Logger.WriteLine(ex.StackTrace + ", " + ex.Message);
        }

        private void StartTaskUpdateApplication()
        {
            var task = Task.Run(() => StartUpdateApplication());
        }

        private void StartUpdateApplication()
        {
            try
            {
                var p = new Process();
                p.StartInfo.FileName = "Updater.exe";
                p.StartInfo.Arguments = "update";

                p.Start();
            }
            catch(Exception ex)
            {
                LogWrite(ex);
            }
        }

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
                LogWrite(ex);
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
                LogWrite(ex);
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
                valueCategoryes.ChangeCategoryID(i, i - 1);
            }

            ini.DeleteSection(sectionLast);

            LoadCategoryToListView();
            listViewEquips.Items.Clear();
        }

        private void MoveCategoryUp(int categoryId)
        {
            ValueCategoryes categoryes = new ValueCategoryes();

            categoryes.SwapCategory(categoryId, categoryId - 1);

            LoadCategoryToListView();
        }

        private void MoveCategoryDown(int categoryId)
        {
            ValueCategoryes categoryes = new ValueCategoryes();

            categoryes.SwapCategory(categoryId, categoryId + 1);

            LoadCategoryToListView();
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

            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

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

            int indexColumn;

            indexColumn = dataGridView1.Columns.Add(@"colGroup", @"Норма смен");
            dataGridView1.Columns[indexColumn].Width = 100;
            dataGridView1.Columns[indexColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            indexColumn = dataGridView1.Columns.Add(@"colGroup", @"Норма часов");
            dataGridView1.Columns[indexColumn].Width = 100;
            dataGridView1.Columns[indexColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            indexColumn = dataGridView1.Columns.Add(@"colGroup", @"Выработка");
            dataGridView1.Columns[indexColumn].Width = 100;
            dataGridView1.Columns[indexColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            indexColumn = dataGridView1.Columns.Add(@"colTask", @"Отставание");
            dataGridView1.Columns[indexColumn].Width = 100;
            dataGridView1.Columns[indexColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            indexColumn = dataGridView1.Columns.Add(@"colTask", @"Эффективность");
            dataGridView1.Columns[indexColumn].Width = 100;
            dataGridView1.Columns[indexColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Frozen = true;

            AddCellToGrid(0, 0, 2);
            dataGridView1.Rows[0].Cells[0].Value = "";

            AddCellToGrid(0, days * countShifts + 2, 2);
            dataGridView1.Rows[0].Cells[days * countShifts + 2].Value = "Отработано";

            AddCellToGrid(0, days * countShifts + 4, 3);
            dataGridView1.Rows[0].Cells[days * countShifts + 4].Value = "Производительность";

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
            dataGridView1.Rows[1].Cells[days * countShifts + 2].Value = "Смен";

            AddCellToGrid(1, days * countShifts + 3);
            dataGridView1.Rows[1].Cells[days * countShifts + 3].Value = "Часов";

            AddCellToGrid(1, days * countShifts + 4);
            dataGridView1.Rows[1].Cells[days * countShifts + 4].Value = "Выработка";

            AddCellToGrid(1, days * countShifts + 5);
            dataGridView1.Rows[1].Cells[days * countShifts + 5].Value = "Отставание";

            AddCellToGrid(1, days * countShifts + 6);
            dataGridView1.Rows[1].Cells[days * countShifts + 6].Value = "%";

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

        private void AddCellToGrid(DataGridView dataGrid, int indexRow, int indexCell, int collSpan = 1)
        {
            HMergedCell pCell;

            //int nOffset = indexCell;

            for (int j = indexCell; j < indexCell + collSpan; j++)
            {
                dataGrid.Rows[indexRow].Cells[j] = new HMergedCell();
                pCell = (HMergedCell)dataGrid.Rows[indexRow].Cells[j];
                pCell.LeftColumn = indexCell;
                pCell.RightColumn = indexCell + collSpan - 1;
            }
            //nOffset += collSpan + 1;
        }

        private void ChangeDate()
        {
            if (!loadShiftsParameter)
            {
                if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
                {
                    UpdateStatistics();
                }
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

            loadShiftsParameter = true;

            AddYearsToComboBox(2015, 2050);
            AddMonthToComboBox();

            SelectCurrentMonth();

            LoadCountShiftsToComboBox();

            //Update Later
            comboBox4.SelectedIndex = 0;

            metroSetTabControl1.SelectTab(settings.GetLastTabIndex());

            loadShiftsParameter = false;

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
                LogWrite(ex);
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
                LogWrite(ex);
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
                LogWrite(ex);
            }
        }

        private void LoadShiftsList()
        {
            ValueShifts valueShifts = new ValueShifts();

            INISettings settings = new INISettings();

            int countShifts = settings.GetCountShifts();
            bool givenShiftNumber = settings.GetGivenShiftNumber();

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            usersList = valueShifts.LoadShiftsForSelectedMonth(usersList, selectDate, countShifts, givenShiftNumber);
        }

        private void LoadShifts()
        {
            //LoadShiftsList();

            StartAddingWorkingTimeToListView();
        }

        private void AddUsersToListView()
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
            Invoke(new Action(() =>
            {
                LoadShiftsList();
            }));

            List<WorkingOut> equipsListWorkingOut = new List<WorkingOut>();
            List<WorkingOut> usersListWorkingOut = new List<WorkingOut>();
            //List<int> usersCurrent = new List<int>();
            INISettings settings = new INISettings();
            ValueDateTime timeValues = new ValueDateTime();

            int fullOutput = settings.GetNormTime();
            int countShifts = settings.GetCountShifts();
            
            bool calculateShiftsInIdletime = settings.GetCalculateShiftsInIdletime();

            for (int i = 0; i < usersList.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                int numberOfShiftsWorked = 0;

                if (usersList[i].Shifts != null)
                {
                    for(int j = 0; j < usersList[i].Shifts.Count; j++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        UserShift shift = usersList[i].Shifts[j];

                        int countDaysFromMonth = CountDaysFromMonth(shift.ShiftDate);

                        DateTime shiftDate = Convert.ToDateTime(shift.ShiftDate);
                        int day = shiftDate.Day;
                        int shiftNumber = shift.ShiftNumber;

                        bool currentShift;// = CheckCurrentShift(shiftDate, shiftNumber);

                        if(shift.ShiftDateEnd == "")
                        {
                            currentShift = true;
                        }
                        else
                        {
                            currentShift = false;
                        }

                        //Выработка 
                        if (shift.Orders != null)// && !currentShift)
                        {
                            float timeWorkigOut = CalculateWorkTime(shift.Orders);
                            float timeBacklog = 0;
                            bool isThereOrdersInWorking = IsThereOrdersInWorking(shift.Orders);

                            if (calculateShiftsInIdletime)
                            {
                                timeBacklog = fullOutput - timeWorkigOut;
                            }
                            else
                            {
                                if (isThereOrdersInWorking)
                                {
                                    timeBacklog = fullOutput - timeWorkigOut;
                                }
                            }

                            if (!currentShift)
                            {
                                usersList[i].WorkingOutUser += timeWorkigOut;
                                usersList[i].WorkingOutBacklog += timeBacklog;
                                numberOfShiftsWorked++;
                            }

                            float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

                            //Выработка для оборудования
                            int indexEquipsList = equipsListWorkingOut.FindIndex(
                                                (v) => v.Id == usersList[i].Equip
                                                       );

                            if (indexEquipsList != -1)
                            {
                                int indexEquipsListWOut = equipsListWorkingOut[indexEquipsList].WorkingOutList.FindIndex(
                                                    (v) => v.ShiftDate == shift.ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                                if (indexEquipsListWOut != -1)
                                {
                                    if (!currentShift)
                                    {
                                        equipsListWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].WorkingOut += timeWorkigOut;
                                    }
                                }
                                else
                                {
                                    equipsListWorkingOut[indexEquipsList].WorkingOutList.Add(new WorkingOutValue(
                                    shift.ShiftDate,
                                    shiftNumber,
                                    timeWorkigOut
                                    ));

                                    if (!currentShift)
                                    {
                                        equipsListWorkingOut[indexEquipsList].NumberOfShiftsWorked++;

                                        if (!isThereOrdersInWorking)
                                        {
                                            equipsListWorkingOut[indexEquipsList].NumberOfIdleShifts++;
                                        }
                                    }

                                    //equipsList[indexEquipsList].EquipsWOut[equipsList[indexEquipsList].EquipsWOut.Count - 1].WorkingOut 
                                }

                                if (!currentShift)
                                {
                                    equipsListWorkingOut[indexEquipsList].WorkingOutSumm += timeWorkigOut;
                                    equipsListWorkingOut[indexEquipsList].WorkingOutBacklog += timeBacklog;
                                }
                            }
                            else
                            {
                                equipsListWorkingOut.Add(new WorkingOut(
                                    usersList[i].Equip
                                    ));

                                equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList = new List<WorkingOutValue>
                                {
                                    new WorkingOutValue(
                                        shift.ShiftDate,
                                        shiftNumber,
                                        timeWorkigOut
                                    )
                                };

                                if (!currentShift)
                                {
                                    equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                    equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                                    equipsListWorkingOut[equipsListWorkingOut.Count - 1].NumberOfShiftsWorked++;

                                    if (!isThereOrdersInWorking)
                                    {
                                        equipsListWorkingOut[equipsListWorkingOut.Count - 1].NumberOfIdleShifts++;
                                    }
                                }
                            }

                            //Выработка для сотрудника
                            int indexUserList = usersListWorkingOut.FindIndex(
                                                    (v) => v.Id == usersList[i].Id
                                                           );

                            if (indexUserList != -1)
                            {
                                int indexUserListWOut = usersListWorkingOut[indexUserList].WorkingOutList.FindIndex(
                                                    (v) => v.ShiftDate == shift.ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                                if (indexUserListWOut != -1)
                                {
                                    if (!currentShift)
                                    {
                                        usersListWorkingOut[indexUserList].WorkingOutList[indexUserListWOut].WorkingOut += timeWorkigOut;
                                    }
                                }
                                else
                                {
                                    usersListWorkingOut[indexUserList].WorkingOutList.Add(new WorkingOutValue(
                                    shift.ShiftDate,
                                    shiftNumber,
                                    timeWorkigOut
                                    ));

                                    if (!currentShift)
                                    {
                                        usersListWorkingOut[indexUserList].NumberOfShiftsWorked++;

                                        if (!isThereOrdersInWorking)
                                        {
                                            usersListWorkingOut[indexUserList].NumberOfIdleShifts++;
                                        }
                                    }

                                    //equipsList[indexEquipsList].EquipsWOut[equipsList[indexEquipsList].EquipsWOut.Count - 1].WorkingOut 
                                }

                                if (!currentShift)
                                {
                                    usersListWorkingOut[indexUserList].WorkingOutSumm += timeWorkigOut;
                                    usersListWorkingOut[indexUserList].WorkingOutBacklog += timeBacklog;
                                    //usersListWorkingOut[indexUserList].WorkingOutBacklog += fullOutput - timeWorkigOut;
                                }
                            }
                            else
                            {
                                usersListWorkingOut.Add(new WorkingOut(
                                    usersList[i].Id
                                    ));

                                usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutList = new List<WorkingOutValue>
                                    {
                                        new WorkingOutValue(
                                            shift.ShiftDate,
                                            shiftNumber,
                                            timeWorkigOut
                                        )
                                    };

                                if (!currentShift)
                                {
                                    usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                    usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                                    //usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutBacklog += fullOutput - timeWorkigOut;
                                    usersListWorkingOut[usersListWorkingOut.Count - 1].NumberOfShiftsWorked++;

                                    if (!isThereOrdersInWorking)
                                    {
                                        usersListWorkingOut[usersListWorkingOut.Count - 1].NumberOfIdleShifts++;
                                    }
                                }
                            }

                            Invoke(new Action(() =>
                            {
                                string key = CreateNameListViewItem(usersList[i].Equip, usersList[i].Id);

                                if (rowIndexes.ContainsKey(key))
                                {
                                    int indexRow = rowIndexes[key];

                                    dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                                    dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = numberOfShiftsWorked;// usersList[i].Shifts.Count;
                                    //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString((int)Math.Round(usersList[i].WorkingOutUser + usersList[i].WorkingOutBacklog));
                                    dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(fullOutput * numberOfShiftsWorked);
                                    dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = timeValues.MinuteToTimeString((int)Math.Round(usersList[i].WorkingOutUser));
                                    dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 5].Value = timeValues.MinuteToTimeString((int)Math.Round(usersList[i].WorkingOutBacklog));
                                    dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = (usersList[i].WorkingOutUser / (usersList[i].WorkingOutUser + usersList[i].WorkingOutBacklog)).ToString("P1");
                                }
                            }));
                        }
                    }
                }
            }

            AddListEquipsWorkingOutToDataGridView(equipsListWorkingOut);
            AddListUsersWorkingOutToDataGridView(usersListWorkingOut);
        }

        private void AddListEquipsWorkingOutToDataGridView(List<WorkingOut> equipsListWorkingOut)
        {
            ValueDateTime timeValues = new ValueDateTime();
            INISettings settings = new INISettings();

            int fullOutput = settings.GetNormTime();
            int countShifts = settings.GetCountShifts();
            bool calculateShiftsInIdletime = settings.GetCalculateShiftsInIdletime();

            for (int i = 0; i < equipsListWorkingOut.Count; i++)
            {
                int countDaysFromMonth = 0;

                int numberOfShiftsWorkedEquips = equipsListWorkingOut[i].NumberOfShiftsWorked;
                int numberOfIdleShiftsEquips;

                if (!calculateShiftsInIdletime)
                {
                    numberOfIdleShiftsEquips = equipsListWorkingOut[i].NumberOfIdleShifts;
                }
                else
                {
                    numberOfIdleShiftsEquips = 0;
                }

                for (int j = 0; j < equipsListWorkingOut[i].WorkingOutList.Count; j++)
                {
                    //MessageBox.Show(equipsList.Count + ", " + equipsList[i].Equip + ", " + equipsList[i].WorkingOut);
                    int day = Convert.ToDateTime(equipsListWorkingOut[i].WorkingOutList[j].ShiftDate).Day;
                    int shiftNumber = equipsListWorkingOut[i].WorkingOutList[j].ShiftNumber;

                    countDaysFromMonth = CountDaysFromMonth(equipsListWorkingOut[i].WorkingOutList[j].ShiftDate);

                    float timeWorkigOut = equipsListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "e" + equipsListWorkingOut[i].Id;

                        if (rowIndexes.ContainsKey(key))
                        {
                            int indexRow = rowIndexes[key];

                            dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                        }
                    }));
                }

                //float fullTimeWorkigOut = equipsListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    string key = "e" + equipsListWorkingOut[i].Id;

                    if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = numberOfShiftsWorkedEquips;
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(numberOfShiftsWorkedEquips * fullOutput);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = timeValues.MinuteToTimeString((int)Math.Round(equipsListWorkingOut[i].WorkingOutSumm));
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 5].Value = timeValues.MinuteToTimeString((numberOfShiftsWorkedEquips - numberOfIdleShiftsEquips) * fullOutput - (int)Math.Round(equipsListWorkingOut[i].WorkingOutSumm));
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = (equipsListWorkingOut[i].WorkingOutSumm / ((numberOfShiftsWorkedEquips - numberOfIdleShiftsEquips) * fullOutput)).ToString("P1");
                    }

                }));
            }
        }

        private void AddListUsersWorkingOutToDataGridView(List<WorkingOut> usersListWorkingOut)
        {
            ValueDateTime timeValues = new ValueDateTime();
            INISettings settings = new INISettings();

            int fullOutput = settings.GetNormTime();
            int countShifts = settings.GetCountShifts();
            bool calculateShiftsInIdletime = settings.GetCalculateShiftsInIdletime();

            for (int i = 0; i < usersListWorkingOut.Count; i++)
            {
                int countDaysFromMonth = 0;

                int numberOfShiftsWorkedUsers = usersListWorkingOut[i].NumberOfShiftsWorked;
                int numberOfIdleShiftsUsers;

                if (!calculateShiftsInIdletime)
                {
                    numberOfIdleShiftsUsers = usersListWorkingOut[i].NumberOfIdleShifts;
                }
                else
                {
                    numberOfIdleShiftsUsers = 0;
                }

                for (int j = 0; j < usersListWorkingOut[i].WorkingOutList.Count; j++)
                {
                    //MessageBox.Show(equipsList.Count + ", " + equipsList[i].Equip + ", " + equipsList[i].WorkingOut);
                    int day = Convert.ToDateTime(usersListWorkingOut[i].WorkingOutList[j].ShiftDate).Day;
                    int shiftNumber = usersListWorkingOut[i].WorkingOutList[j].ShiftNumber;

                    countDaysFromMonth = CountDaysFromMonth(usersListWorkingOut[i].WorkingOutList[j].ShiftDate);

                    float timeWorkigOut = usersListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "u" + usersListWorkingOut[i].Id;

                        if (rowIndexes.ContainsKey(key))
                        {
                            int indexRow = rowIndexes[key];

                            dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                        }
                    }));
                }

                //float fullTimeWorkigOut = usersListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    string key = "u" + usersListWorkingOut[i].Id;

                    if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = numberOfShiftsWorkedUsers;
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString((int)numberOfShiftsWorkedUsers * fullOutput);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = timeValues.MinuteToTimeString((int)Math.Round(usersListWorkingOut[i].WorkingOutSumm));
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 5].Value = timeValues.MinuteToTimeString((numberOfShiftsWorkedUsers - numberOfIdleShiftsUsers) * fullOutput - (int)Math.Round(usersListWorkingOut[i].WorkingOutSumm));
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = (usersListWorkingOut[i].WorkingOutSumm / ((numberOfShiftsWorkedUsers - numberOfIdleShiftsUsers) * fullOutput)).ToString("P1");
                    }
                }));
            }
        }
        private bool IsThereOrdersInWorking(List<UserShiftOrder> orders)
        {
            bool result = false;

            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].IdletimeName == "")
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        private float CalculateWorkTime(List<UserShiftOrder> order)
        {
            float workingOut = 0;

            for (int i = 0; i < order.Count; i++)
            {
                workingOut += CalculateWorkTimeForOneOrder(order[i]);
            }

            return workingOut;
        }

        private float CalculateWorkTimeForOneOrder(UserShiftOrder order)
        {
            float workingOut = 0;

            /*if (order.Flags == 576)
            {
                workingOut += order.Normtime;
            }
            else
            {
                if (order.Normtime > 0)
                {
                    float norm = (float)order.PlanOutQty / (float)order.Normtime;

                    if (norm > 0)
                    {
                        workingOut += order.FactOutQty / norm;
                    }
                }
            }*/

            if (order.Normtime > 0)
            {
                if (order.PlanOutQty > 0)
                {
                    workingOut += ((float)order.FactOutQty * (float)order.Normtime) / (float)order.PlanOutQty;
                }
                else
                {
                    if (order.FactOutQty > 0)
                    {
                        workingOut += (float)order.Normtime;
                    }
                }
            }

            return workingOut;
        }

        private float GetPercentWorkingOut(int targetWorkingOut, float facticalWorkingOut)
        {
            float result;

            result = facticalWorkingOut / targetWorkingOut;

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

            AddUsersToListView();

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

            if (dateTime.Hour >= 20 && dateTime.Hour <= 23 || dateTime.Hour >= 0 && dateTime.Hour < 8)
            {
                if (comboBox1.Items.Count > 1)
                    comboBox1.SelectedIndex = 1;
            }
            else
            {
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }

            if (dateTime.Hour >= 0 && dateTime.Hour < 8)
            {
                dateTimePicker1.Value = dateTime.AddDays(-1);
            }
            else
            {
                dateTimePicker1.Value = dateTime;
            }
        }

        private bool CheckCurrentShift(DateTime dateTime, int shift) 
        {
            bool result = false;

            DateTime currentTime = DateTime.Now;

            if (currentTime.ToString("dd.MM.yyyy") == dateTime.ToString("dd.MM.yyyy"))
            {
                if (shift == 1)
                {
                    if (currentTime.Hour >= 8 && currentTime.Hour < 20)
                    {
                        result = true;
                        //MessageBox.Show(1.ToString());
                    }
                }
                if (shift == 2)
                {
                    if (currentTime.Hour >= 20 && currentTime.Hour <= 23)
                    {
                        result = true;
                        //MessageBox.Show(2.ToString());
                    }
                }
                /*else
                {
                    result = false;
                    MessageBox.Show(3.ToString());
                }*/
            }
            else if (currentTime.AddDays(-1).ToString("dd.MM.yyyy") == dateTime.ToString("dd.MM.yyyy"))
            {
                if (shift == 1)
                {
                    result = false;
                    //MessageBox.Show(4.ToString());
                }
                else if (shift == 2)
                {
                    if (currentTime.Hour >= 0 && currentTime.Hour < 8)
                    {
                        result = true;
                        //MessageBox.Show(5.ToString());
                    } 
                }
            } 


            /*if (shift == 1)
            {
                if (currentTime.ToString("dd.MM.yyyy") == dateTime.ToString("dd.MM.yyyy"))
                    result = true;
                else
                    result = false;
            }
            else
            {
                if (currentTime.ToString("dd.MM.yyyy") == dateTime.ToString("dd.MM.yyyy"))
                    if (currentTime.Hour >= 20 && currentTime.Hour < 0)
                        result = true;
                    else
                        result = false;
                else if (currentTime.ToString("dd.MM.yyyy") == dateTime.AddDays(1).ToString("dd.MM.yyyy"))
                    if (currentTime.Hour >= 0 && currentTime.Hour < 8)
                        result = true;
                    else
                        result = false;
            }*/
            //MessageBox.Show(currentTime.ToString("dd.MM.yyyy") + " == " + dateTime.ToString("dd.MM.yyyy") + " - " + result.ToString() + ", Время: " + currentTime.Hour + ", Смена: " + shift);
            return result;
        }

        private int AddDinnerTimeToWorkingOut(DateTime selectDate, string firstTime, string secondTime)
        {
            int result = 0;

            DateTime firstDateTime = Convert.ToDateTime(firstTime);
            DateTime secondDateTime = Convert.ToDateTime(secondTime);

            string[] breakeTimesDay = { "11:30", "30", "15:30", "30", "18:00", "10", "23:30", "30" };
            string[] breakeTimesNight = { "03:30", "30", "06:00", "10" };

            for (int i = 0; i < breakeTimesDay.Length; i += 2)
            {
                DateTime breakeDateTime = Convert.ToDateTime(selectDate.ToString("dd.MM.yyyy") + " " + breakeTimesDay[i] + ":00");
                int breakTime = Convert.ToInt32(breakeTimesDay[i + 1]);

                if (firstDateTime < breakeDateTime && breakeDateTime < secondDateTime)
                {
                    result += breakTime;
                }
            }

            for (int i = 0; i < breakeTimesNight.Length; i += 2)
            {
                DateTime breakeDateTime = Convert.ToDateTime(selectDate.AddDays(1).ToString("dd.MM.yyyy") + " " + breakeTimesNight[i] + ":00");
                int breakTime = Convert.ToInt32(breakeTimesNight[i + 1]);

                if (firstDateTime < breakeDateTime && breakeDateTime < secondDateTime)
                {
                    result += breakTime;
                }
            }

            return result;
        }

        private int AddDinnerTimeToWorkingOut(string firstTime, string secondTime)
        {
            int result = 0;

            DateTime firstDateTime = Convert.ToDateTime(firstTime);
            DateTime secondDateTime = Convert.ToDateTime(secondTime);

            DateTime firstDate = new DateTime(firstDateTime.Year, firstDateTime.Month, firstDateTime.Day);
            DateTime secondDate = new DateTime(secondDateTime.Year, secondDateTime.Month, secondDateTime.Day);

            string[] breakeTimes = { "11:30", "30", "15:30", "30", "18:00", "10", "23:30", "30", "03:30", "30", "06:00", "10" };

            int dayDifference = (secondDate - firstDate).Days;
            
            for (int day = 0; day <= dayDifference; day++)
            {
                DateTime selectDate = firstDateTime.AddDays(day);

                for (int i = 0; i < breakeTimes.Length; i += 2)
                {
                    DateTime breakeDateTime = Convert.ToDateTime(selectDate.ToString("dd.MM.yyyy") + " " + breakeTimes[i] + ":00");
                    int breakTime = Convert.ToInt32(breakeTimes[i + 1]);

                    if (firstDateTime < breakeDateTime && breakeDateTime < secondDateTime)
                    {
                        result += breakTime;
                    }
                }
            }

            return result;
        }

        private void CreateColomnsToDataGridForOneShift()
        {
            INISettings settings = new INISettings();

            int countShifts = settings.GetCountShifts();

            dataGridViewOneShift.Rows.Clear();
            dataGridViewOneShift.Columns.Clear();

            dataGridViewOneShift.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridViewOneShift.AllowUserToResizeColumns = false;
            dataGridViewOneShift.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewOneShift.AllowUserToResizeRows = false;
            dataGridViewOneShift.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            DataGridViewContentAlignment right = DataGridViewContentAlignment.MiddleRight;
            DataGridViewContentAlignment left = DataGridViewContentAlignment.MiddleLeft;
            DataGridViewContentAlignment center = DataGridViewContentAlignment.MiddleCenter;

            string[] colNames = { "№", "Имя", "Заказ", "Заказчик", "Операция", "Остаток | Тираж", "Дано времени", "Начало", "Завершение", "Продолжительность", "Планируемое время завершения", "Отклонение", "Сделано", "Выработка" };
            int[] colWidth = { 30, 300, 100, 250, 200, 140, 70, 170, 170, 80, 150, 80, 80, 80 };
            DataGridViewContentAlignment[] colAligment = { right, left, left, left, left, left, center, left, left, center, left, center, left, center};

            for (int i = 0; i < colNames.Length; i++)
            {
                int indexCol = dataGridViewOneShift.Columns.Add(colNames[i], colNames[i]);
                dataGridViewOneShift.Columns[indexCol].Width = colWidth[i];
                dataGridViewOneShift.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewOneShift.Columns[indexCol].DefaultCellStyle.Alignment = colAligment[i];
            }

            dataGridViewOneShift.Columns[0].Frozen = true;
            dataGridViewOneShift.Columns[1].Frozen = true;

            dataGridViewOneShift.Rows.Add();
            dataGridViewOneShift.Rows[0].Height = 60;
            dataGridViewOneShift.Rows[0].Frozen = true;

            for (int i = 0; i < colNames.Length; i++)
            {
                AddCellToGrid(dataGridViewOneShift, 0, i);
                dataGridViewOneShift.Rows[0].Cells[i].Value = colNames[i];
            }
        }

        private bool isLastRecordOfOrder(List<UserShiftOrder> userShiftOrders)
        {
            bool result = true;

            for (int i = 1; i < userShiftOrders.Count; i++)
            {
                if (userShiftOrders[i].IdletimeName == "")
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private bool AreThereAnyMoreOrders(List <UserShiftOrder> userShiftOrders, int idManOrderJobItem)
        {
            bool result = false;

            for (int i = 0; i < userShiftOrders.Count; i++)
            {
                if (idManOrderJobItem != userShiftOrders[i].IdManOrderJobItem)
                {
                    if (userShiftOrders[i].IdletimeName == "")
                    {
                        result = false;
                        break;
                    }
                }

                if (idManOrderJobItem == userShiftOrders[i].IdManOrderJobItem)
                {
                    result = true;
                    break;
                }
            }
            
            return result;
        }

        private List<UserShiftOrder> SelectedFullStepsForCurrentOrder(List<UserShiftOrder> userShiftOrders, int idManOrderJobItem)
        {
            List<UserShiftOrder> userShiftOrder = new List<UserShiftOrder>();
            for (int i = 0; i < userShiftOrders.Count; i++)
            {
                if (userShiftOrders[i].IdManOrderJobItem == idManOrderJobItem)
                {
                    userShiftOrder.Add(userShiftOrders[i]);
                }
                else
                {
                    if (AreThereAnyMoreOrders(userShiftOrders.GetRange(i, userShiftOrders.Count - i), idManOrderJobItem) && i != 0)
                    {
                        userShiftOrder.Add(userShiftOrders[i]);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return userShiftOrder;
        }

        private void LoadOrdersSelectedDateAndShift(DateTime selectDate, int selectShift)
        {
            LoadOrdersSelectedDateAndShiftDetails(selectDate, selectShift);
            //LoadOrdersSelectedDateAndShiftCompact(selectDate, selectShift);
        }

        private void LoadOrdersSelectedDateAndShiftDetails(DateTime selectDate, int selectShift)
        {
            ChangeStateTimer();

            CreateColomnsToDataGridForOneShift();

            ValueShifts shifts = new ValueShifts();
            ValueDateTime time = new ValueDateTime();
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            INISettings settings = new INISettings();

            bool givenShiftNumber = settings.GetGivenShiftNumber();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

            string timeStartShift = time.StartShiftPlanedDateTime(selectDate, selectShift);
            string timeEndShift = time.EndShiftPlanedDateTime(timeStartShift);

            List<User> usersShiftList = shifts.LoadOrders(selectDate, selectShift, givenShiftNumber);
            
            List<int> usersCurrent = new List<int>();

            //Пока так, потом сделаю отдельную выборку оборудования с сортировкой, либо без сортирвки а в порядке загрузки
            //если бы БД адекватно хранила индексы смены, а не херила их после редактирования записи, то было бы проще привязываться к смене в fbc_brigade
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

            for (int i = 0; i < usersCurrent.Count; i++)
            {
                int indexRow = dataGridViewOneShift.Rows.Add();

                dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (i + 1).ToString();
                dataGridViewOneShift.Rows[indexRow].Cells[1].Value = users[usersCurrent[i]];
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOneShift.Font, FontStyle.Bold);
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = Color.Gray;
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                float userWorkingOut = 0;
                int userDone = 0;
                int dinnerTime = 0;

                int idletime = 0;
                //int indexRowForUser = listView1.Items.Count - 1;

                //Сделать детальное отображение выполняемых заказов
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    if (usersShiftList[j].Id == usersCurrent[i])
                    {
                        UserShift userShift = usersShiftList[j].Shifts[0];

                        int currentStep = 0;
                        int countOrder = 0;
                        int countOperation = 0;

                        dataGridViewOneShift.Rows[indexRow].Cells[7].Value = userShift.ShiftDateBegin;
                        dataGridViewOneShift.Rows[indexRow].Cells[8].Value = userShift.ShiftDateEnd;

                        while (currentStep < userShift.Orders.Count)
                        {
                            countOperation++;
                            countOrder++;

                            bool isMakeready = false;

                            List<UserShiftOrder> userShiftOrders = SelectedFullStepsForCurrentOrder(userShift.Orders.GetRange(currentStep, userShift.Orders.Count - currentStep), userShift.Orders[currentStep].IdManOrderJobItem);

                            for (int l = 0; l < userShiftOrders.Count; l++)
                            {
                                UserShiftOrder order = userShiftOrders[l];
                                ViewOrder view = new ViewOrder();

                                string orderStartTime = userShiftOrders[0].DateBegin;
                                string orderEndTime = userShiftOrders[l].DateEnd;

                                view.WorkingOut += CalculateWorkTimeForOneOrder(order);

                                if (order.Flags != 576)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        view.Done += order.FactOutQty;
                                    }
                                        
                                    view.Amount = order.PlanOutQty;
                                    view.NormTimeWork = order.Normtime;
                                    view.IdletimeName = "работа";
                                }
                                
                                if (order.Flags == 576)
                                {
                                    view.NormTimeMakeReady = order.Normtime;
                                    view.IdletimeName = "приладка";
                                }

                                if (order.IdletimeName != "")
                                {
                                    countOrder--;

                                    view.IdletimeName = order.IdletimeName;

                                    if (order.FactOutQty > 0)
                                    {
                                        //idletime += order.Normtime
                                        //Поскольку время планового простоя учитывается в выработке, то не смысла в этой записи
                                    }
                                }

                                string lastTimeEndPlanedOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut));
                                view.Duration += order.Duration;

                                userWorkingOut += view.WorkingOut;
                                userDone += view.Done;

                                //string lastTimeEndPlanedOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut));

                                int orderPreviousAmount = shifts.GetAmountDoneFromPreviousShifts(userShift.Orders[currentStep].IdManOrderJobItem, order.DateBegin);
                                int lastAmount = view.Amount - orderPreviousAmount;

                                int[] normtime = shifts.GetNormTimeForOrder(order.IdManOrderJobItem);
                                int normTimeFull = 0;
                                int normTimeGeneral = 0;

                                float lastTime = 0;

                                if (normtime[1] > 0)
                                {
                                    float norm = view.Amount / normtime[1];

                                    if (norm > 0)
                                    {
                                        lastTime = lastAmount / norm;
                                    }
                                }

                                if (orderPreviousAmount > 0)
                                {
                                    normTimeGeneral = (int)lastTime;
                                    normTimeFull = (int)lastTime;
                                }
                                else
                                {
                                    if (isMakeready)
                                    {
                                        normTimeGeneral = normtime[1];
                                    }
                                    else
                                    {
                                        normTimeGeneral = normtime[0] + normtime[1];
                                    }
                                    
                                    normTimeFull = view.NormTimeMakeReady + view.NormTimeWork;
                                }

                                if (order.Status == 2)
                                {
                                    //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                    //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, time.DateTimeAmountMunutes(order.DateBegin, (int)Math.Round(userWorkingOut)));
                                    //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, order.DateEnd);
                                    dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, order.DateEnd);
                                    view.TimePlanedEndOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut) + dinnerTime + idletime);

                                    view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                    view.TimeEnd = order.DateEnd + " ";
                                }

                                if (order.Status != 2)
                                {
                                    //timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull);

                                    //if (user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateBegin == user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd)
                                    if (order.DateBegin == order.DateEnd)
                                    {
                                        if (shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                        {
                                            string factTimeEndOrder = time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral);

                                            if (time.StringToDateTime(factTimeEndOrder) < DateTime.Now)
                                            {
                                                factTimeEndOrder = DateTime.Now.ToString();
                                            }

                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, DateTime.Now.ToString());
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral));
                                            //dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral));
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, factTimeEndOrder);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeGeneral + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, DateTime.Now.ToString());
                                            view.Duration = time.DateDifferenceToMinutes(DateTime.Now.ToString(), order.DateBegin);
                                            view.TimeEnd = "выполняется ";
                                        }
                                        else
                                        {
                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                            //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, time.DateTimeAmountMunutes(order.DateBegin, (int)Math.Round(view.WorkingOut)));
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, order.DateEnd);
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, order.DateEnd);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(view.WorkingOut) + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            view.TimeEnd = order.DateEnd + " ";

                                            /*dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, timeEndShift);
                                            timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull + dinnerTime);
                                            differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, timeEndShift);
                                            duration = time.DateDifferenceToMinutes(timeEndShift, firstTimeBegin);
                                            timeEnd = timeEndShift; */
                                        }
                                    }
                                    else
                                    {
                                        if (shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                        {
                                            int normTimeCurrent;

                                            if (order.IdletimeName == "")
                                            {
                                                if (isLastRecordOfOrder(userShiftOrders.GetRange(l, userShiftOrders.Count - l)))
                                                {
                                                    normTimeCurrent = (int)Math.Round(view.WorkingOut);
                                                }
                                                else
                                                {
                                                    normTimeCurrent = normTimeGeneral;
                                                }
                                            }
                                            else
                                            {
                                                normTimeCurrent = (int)Math.Round(view.WorkingOut);
                                            }

                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, DateTime.Now.ToString());
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, time.DateTimeAmountMunutes(orderStartTime, normTimeGeneral));
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, time.DateTimeAmountMunutes(orderStartTime, normTimeCurrent));
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeCurrent + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            //view.Duration = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateBegin);
                                            view.TimeEnd = order.DateEnd + " ";
                                        }
                                        else
                                        {
                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, time.DateTimeAmountMunutes(orderStartTime, (int)Math.Round(view.WorkingOut)));
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, orderEndTime);
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, orderEndTime);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(view.WorkingOut) + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            view.TimeEnd = order.DateEnd + " ";
                                        }
                                    }
                                }

                                if (order.Flags == 576)
                                {
                                    isMakeready = true;
                                }

                                indexRow = dataGridViewOneShift.Rows.Add();

                                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                                if (l == 0)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (countOrder).ToString();
                                    }
                                    
                                    dataGridViewOneShift.Rows[indexRow].Cells[1].Value = "    " + machines[usersShiftList[j].Equip];
                                    dataGridViewOneShift.Rows[indexRow].Cells[2].Value = order.OrderNumber;
                                    dataGridViewOneShift.Rows[indexRow].Cells[3].Value = order.OrderName;
                                }

                                dataGridViewOneShift.Rows[indexRow].Cells[4].Value = view.IdletimeName;

                                if (order.Flags == 576 || order.IdletimeName != "")
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[5].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");
                                    dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(order.Normtime);
                                }
                                else
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastAmount.ToString("N0") + " | " + view.Amount.ToString("N0");

                                    if (order.DateBegin == order.DateEnd)
                                    {
                                        if (shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                        {
                                            dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeGeneral);
                                        }
                                        else
                                        {
                                            dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeFull);
                                        }
                                    }
                                    else
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeGeneral);
                                    }
                                }

                                if (order.IdletimeName == "")
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[10].Value = view.TimePlanedEndOrder;

                                    if (l == userShiftOrders.Count - 1)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                    }
                                }
                                else
                                {
                                    if ((currentStep == userShift.Orders.Count - 1) && !shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                    }

                                    if (order.IdletimeName != "" && order.FactOutQty > 0)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[10].Value = view.TimePlanedEndOrder;
                                        dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                    }
                                }

                                if (view.Done > 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.Done.ToString("N0");
                                }

                                //тут еще ничего не работает
                                //dataGridViewOneShift.Rows[indexRow].Cells[4].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");

                                dataGridViewOneShift.Rows[indexRow].Cells[7].Value = order.DateBegin;
                                //dataGridViewOneShift.Rows[indexRow].Cells[8].Value = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd;
                                dataGridViewOneShift.Rows[indexRow].Cells[8].Value = view.TimeEnd;
                                dataGridViewOneShift.Rows[indexRow].Cells[9].Value = time.MinuteToTimeString(view.Duration);
                                
                                //dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(view.DifferentTime);
                                //dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.Done.ToString("N0");
                                dataGridViewOneShift.Rows[indexRow].Cells[13].Value = time.MinuteToTimeString((int)Math.Round(view.WorkingOut));

                                if (view.DifferentTime >= 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[11].Style.ForeColor = Color.Green;
                                }
                                else
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[11].Style.ForeColor = Color.Red;
                                }

                                Color color = Color.White;

                                if (countOperation % 2 == 0)
                                {
                                    color = Color.Gainsboro;
                                }

                                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = color;
                            }

                            currentStep += userShiftOrders.Count;
                        }
                    }
                }

                indexRow = dataGridViewOneShift.Rows.Add();

                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOneShift.Font, FontStyle.Bold);
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = Color.Silver;
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                dataGridViewOneShift.Rows[indexRow].Cells[12].Value = userDone.ToString("N0");
                dataGridViewOneShift.Rows[indexRow].Cells[13].Value = time.MinuteToTimeString((int)Math.Round(userWorkingOut));
            }
        }

        private void LoadOrdersSelectedDateAndShiftCompact(DateTime selectDate, int selectShift)
        {
            ChangeStateTimer();

            CreateColomnsToDataGridForOneShift();

            ValueShifts shifts = new ValueShifts();
            ValueDateTime time = new ValueDateTime();
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            INISettings settings = new INISettings();

            bool givenShiftNumber = settings.GetGivenShiftNumber();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

            string timeStartShift = time.StartShiftPlanedDateTime(selectDate, selectShift);
            string timeEndShift = time.EndShiftPlanedDateTime(timeStartShift);

            List<User> usersShiftList = shifts.LoadOrders(selectDate, selectShift, givenShiftNumber);

            List<int> usersCurrent = new List<int>();

            //Пока так, потом сделаю отдельную выборку оборудования с сортировкой, либо без сортирвки а в порядке загрузки
            //если бы БД адекватно хранила индексы смены, а не херила их после редактирования записи, то было бы проще привязываться к смене в fbc_brigade
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

            for (int i = 0; i < usersCurrent.Count; i++)
            {
                int indexRow = dataGridViewOneShift.Rows.Add();

                dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (i + 1).ToString();
                dataGridViewOneShift.Rows[indexRow].Cells[1].Value = users[usersCurrent[i]];
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOneShift.Font, FontStyle.Bold);
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = Color.Gray;
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                float userWorkingOut = 0;
                int userDone = 0;
                int dinnerTime = 0;
                //int indexRowForUser = listView1.Items.Count - 1;

                //Сделать детальное отображение выполняемых заказов
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    if (usersShiftList[j].Id == usersCurrent[i])
                    {
                        User user = usersShiftList[j];
                        UserShift userShift = user.Shifts[0];

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

                            string firstTimeBegin = user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateBegin;
                            string lastTimeBegin = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateBegin;
                            string firstTimeEnd = user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateEnd;
                            string lastTimeEnd = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd;

                            //dinnerTime += AddDinnerTimeToWorkingOut(firstTimeBegin, lastTimeEnd);

                            string operation = "";

                            float workingOut = 0;
                            int done = 0;
                            int duration = 0;
                            int amount = 0;
                            string timeEnd = "";
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
                                    operation = "работа";
                                }

                                if (orderCur.Flags == 576)
                                {
                                    normTimeMakeReady = orderCur.Normtime;
                                    operation = "приладка";
                                }

                                duration += orderCur.Duration;
                            }

                            if (order.IdletimeName != "")
                            {
                                operation = order.IdletimeName;
                            }

                            string lastTimeEndPlanedOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut));

                            int lastAmount = amount - orderPreviousAmount;
                            //MessageBox.Show(lastAmount + " = " + order.PlanOutQty + " - " + orderPreviousAmount);

                            userWorkingOut += workingOut;
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

                                dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, lastTimeEnd);
                                timePlanedEndOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut) + dinnerTime);

                                differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, lastTimeEnd);
                                timeEnd = lastTimeEnd + " ";
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

                                //timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull);

                                //if (user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateBegin == user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd)
                                if (lastTimeBegin == lastTimeEnd)
                                {
                                    if (shifts.CheckShiftIsActive(order.IDFBCBrigade))
                                    {
                                        dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, DateTime.Now.ToString());
                                        timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull + dinnerTime);
                                        differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, DateTime.Now.ToString());
                                        duration = time.DateDifferenceToMinutes(DateTime.Now.ToString(), firstTimeBegin);
                                        timeEnd = "выполняется ";
                                    }
                                    else
                                    {
                                        dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, lastTimeEnd);
                                        timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(workingOut) + dinnerTime);
                                        differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, lastTimeEnd);
                                        timeEnd = lastTimeEnd + " ";

                                        /*dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, timeEndShift);
                                        timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull + dinnerTime);
                                        differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, timeEndShift);
                                        duration = time.DateDifferenceToMinutes(timeEndShift, firstTimeBegin);
                                        timeEnd = timeEndShift;*/

                                        //delete
                                        /*dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, timeEndShift);
                                        timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, normTimeFull + dinnerTime);
                                        differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, timeEndShift);
                                        duration = time.DateDifferenceToMinutes(timeEndShift, firstTimeBegin);
                                        timeEnd = timeEndShift;*/
                                    }
                                }
                                else
                                {
                                    dinnerTime += AddDinnerTimeToWorkingOut(selectDate, firstTimeBegin, lastTimeEnd);
                                    timePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(workingOut) + dinnerTime);
                                    differentTime = time.DateDifferenceToMinutes(timePlanedEndOrder, lastTimeEnd);
                                    timeEnd = lastTimeEnd + " ";
                                }
                            }
                            
                            indexRow = dataGridViewOneShift.Rows.Add();

                            dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                            dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (k + 1).ToString();
                            dataGridViewOneShift.Rows[indexRow].Cells[1].Value = "    " + machines[user.Equip];
                            dataGridViewOneShift.Rows[indexRow].Cells[2].Value = order.OrderNumber;
                            dataGridViewOneShift.Rows[indexRow].Cells[3].Value = order.OrderName;
                            dataGridViewOneShift.Rows[indexRow].Cells[4].Value = operation;
                            dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastAmount.ToString("N0") + " | " + amount.ToString("N0");
                            dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeFull);
                            dataGridViewOneShift.Rows[indexRow].Cells[7].Value = firstTimeBegin;
                            //dataGridViewOneShift.Rows[indexRow].Cells[8].Value = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd;
                            dataGridViewOneShift.Rows[indexRow].Cells[8].Value = timeEnd;
                            dataGridViewOneShift.Rows[indexRow].Cells[9].Value = time.MinuteToTimeString(duration);
                            dataGridViewOneShift.Rows[indexRow].Cells[10].Value = timePlanedEndOrder;
                            dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(differentTime);
                            dataGridViewOneShift.Rows[indexRow].Cells[12].Value = done.ToString("N0");
                            dataGridViewOneShift.Rows[indexRow].Cells[13].Value = time.MinuteToTimeString((int)Math.Round(workingOut));

                            if (differentTime >= 0)
                            {
                                dataGridViewOneShift.Rows[indexRow].Cells[11].Style.ForeColor = Color.Green;
                            }
                            else
                            {
                                dataGridViewOneShift.Rows[indexRow].Cells[11].Style.ForeColor = Color.Red;
                            }
                        }
                    }
                }

                indexRow = dataGridViewOneShift.Rows.Add();

                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOneShift.Font, FontStyle.Bold);
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.BackColor = Color.Silver;
                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                dataGridViewOneShift.Rows[indexRow].Cells[12].Value = userDone.ToString("N0");
                dataGridViewOneShift.Rows[indexRow].Cells[13].Value = time.MinuteToTimeString((int)Math.Round(userWorkingOut));
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            ReloadOrdersSelectedDate();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadOrdersSelectedDate();
        }

        private void ReloadOrdersSelectedDate()
        {
            DateTime selectDate = dateTimePicker1.Value;
            int selectShift = comboBox1.SelectedIndex + 1;

            if (!loadParameter)
            {
                LoadOrdersSelectedDateAndShift(selectDate, selectShift);
            }
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            StartTaskUpdateApplication();
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
            SaveParameterBeforeClosing();

            TabControlSelectedIndexChanged(metroSetTabControl1.SelectedIndex);

            metroSetTabControlPreviousIndex = metroSetTabControl1.SelectedIndex;
            comboBoxSelectViewsPreviousIndex = -1;
        }

        private void SaveParameterBeforeClosing()
        {
            if (metroSetTabControlPreviousIndex == 3)
            {
                SaveCategoryToIniFile();
                SaveParameterToIniFile();

                LoadCountShiftsToComboBox();

                listViewEquips.Items.Clear();
            }

            if (metroSetTabControlPreviousIndex == 4)
            {
                SaveViewParameter();
            }
        }

        private void LoadSelectedOrderSearched(int index)
        {
            CreateColomnsToDataGridForSearchedOrder();

            LoadSearchedOrderToDataGridView(index);
        }

        private void ClearColomnsFromDataGridForSearchedOrder()
        {
            dataGridViewOrderDetails.Rows.Clear();
            dataGridViewOrderDetails.Columns.Clear();
        }

        private void CreateColomnsToDataGridForSearchedOrder()
        {
            INISettings settings = new INISettings();

            ClearColomnsFromDataGridForSearchedOrder();

            dataGridViewOrderDetails.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            //dataGridViewOneShift.AllowUserToResizeColumns = false;
            dataGridViewOrderDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewOrderDetails.AllowUserToResizeRows = false;
            dataGridViewOrderDetails.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            string[] colNames = { "№", "Имя", "Операция", "Тираж", "Начало", "Завершение", "Продолжительность", "Сделано" };
            int[] colWidth = { 30, 300, 160, 100, 180, 180, 100, 100 };
            DataGridViewContentAlignment[] colAligment = { DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft,
                DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleCenter,
                DataGridViewContentAlignment.MiddleLeft };

            for (int i = 0; i < colNames.Length; i++)
            {
                int indexCol = dataGridViewOrderDetails.Columns.Add(colNames[i], colNames[i]);
                dataGridViewOrderDetails.Columns[indexCol].Width = colWidth[i];
                dataGridViewOrderDetails.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewOrderDetails.Columns[indexCol].DefaultCellStyle.Alignment = colAligment[i];
            }

            dataGridViewOrderDetails.Columns[0].Frozen = true;
            dataGridViewOrderDetails.Columns[1].Frozen = true;

            dataGridViewOrderDetails.Rows.Add();
            dataGridViewOrderDetails.Rows[0].Height = 60;
            dataGridViewOrderDetails.Rows[0].Frozen = true;

            for (int i = 0; i < colNames.Length; i++)
            {
                AddCellToGrid(dataGridViewOrderDetails, 0, i);
                dataGridViewOrderDetails.Rows[0].Cells[i].Value = colNames[i];
            }
        }

        private void LoadSearchedOrderToDataGridView(int index)
        {
            ValueOrderSearch valueOrderSearch = new ValueOrderSearch();
            ValueDateTime time = new ValueDateTime();

            List<OrderSearchValue> orderSearch = valueOrderSearch.OrdersListFromIDHead(ordersHead[index].IDOrderHead);

            List<int> equips = new List<int>();

            int indexRow;

            for (int i = 0; i < orderSearch.Count; i++)
            {
                if (!equips.Contains(orderSearch[i].IDEquip))
                {
                    equips.Add(orderSearch[i].IDEquip);
                }
            }

            for (int i = 0; i < equips.Count; i++)
            {
                indexRow = dataGridViewOrderDetails.Rows.Add();

                dataGridViewOrderDetails.Rows[indexRow].Cells[0].Value = (i + 1).ToString();
                dataGridViewOrderDetails.Rows[indexRow].Cells[1].Value = machines[equips[i]];
                dataGridViewOrderDetails.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOrderDetails.Font, FontStyle.Bold);
                dataGridViewOrderDetails.Rows[indexRow].DefaultCellStyle.BackColor = Color.Gray;
                dataGridViewOrderDetails.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                List<int> idManOrderItem = new List<int>();

                int countOperationForCurrentEquip = 0;
                int idManOrderJobItem = -1;

                int fullPlanOutQTY = 0;
                int fullFactOutQTY = 0;

                string operationNumber = "";
                string userName = "";
                string planOutQTY = "";

                int _idUser = -1;
                int _planQTY = -1;

                for (int j = 0; j < orderSearch.Count; j++)
                {
                    if (orderSearch[j].IDEquip == equips[i] && orderSearch[j].Idletime == "")
                    {
                        if (!idManOrderItem.Contains(orderSearch[j].IDManOrderJobItem))
                        {
                            idManOrderItem.Add(orderSearch[j].IDManOrderJobItem);
                        }

                        if (_idUser == orderSearch[j].IDEmployee)
                        {
                            userName = "";
                            operationNumber = "";
                        }
                        else
                        {
                            countOperationForCurrentEquip++;

                            _idUser = orderSearch[j].IDEmployee;
                            userName = "    " + users[orderSearch[j].IDEmployee];

                            operationNumber = countOperationForCurrentEquip.ToString();
                        }

                        if (idManOrderJobItem == orderSearch[j].IDManOrderJobItem)
                        {
                            if (_planQTY == orderSearch[j].PlanOutQTY)
                            {
                                planOutQTY = "";
                            }
                            else
                            {
                                planOutQTY = orderSearch[j].PlanOutQTY.ToString("N0");
                                _planQTY = orderSearch[j].PlanOutQTY;
                            }
                        }
                        else
                        {
                            planOutQTY = orderSearch[j].PlanOutQTY.ToString("N0");
                            _planQTY = orderSearch[j].PlanOutQTY;
                            idManOrderJobItem = orderSearch[j].IDManOrderJobItem;
                        }

                        /*if (idManOrderJobItem == orderSearch[j].IDManOrderJobItem && orderSearch[j].PlanOutQTY == _planQTY)
                        {
                            planOutQTY = "";
                        }
                        else
                        {
                            idManOrderJobItem = orderSearch[j].IDManOrderJobItem;
                            planOutQTY = orderSearch[j].PlanOutQTY.ToString("N0");
                        }*/

                        //_planQTY = orderSearch[j].PlanOutQTY;

                        string operation = "";

                        if (orderSearch[j].Idletime == "")
                        {
                            if (orderSearch[j].Flags != 576)
                            {
                                operation = "работа";

                                fullFactOutQTY += orderSearch[j].FactOutQTY;
                            }

                            if (orderSearch[j].Flags == 576)
                            {
                                operation = "приладка";
                            }

                            /*if (orderSearch[j].Idletime != "")
                            {
                                operation = orderSearch[j].Idletime;
                            }*/

                            indexRow = dataGridViewOrderDetails.Rows.Add();

                            dataGridViewOrderDetails.Rows[indexRow].Cells[0].Value = operationNumber;
                            dataGridViewOrderDetails.Rows[indexRow].Cells[1].Value = userName;
                            dataGridViewOrderDetails.Rows[indexRow].Cells[2].Value = operation;
                            dataGridViewOrderDetails.Rows[indexRow].Cells[3].Value = planOutQTY;
                            dataGridViewOrderDetails.Rows[indexRow].Cells[4].Value = orderSearch[j].DateBegin;
                            dataGridViewOrderDetails.Rows[indexRow].Cells[5].Value = orderSearch[j].DateEnd;
                            dataGridViewOrderDetails.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(orderSearch[j].Duration);
                            dataGridViewOrderDetails.Rows[indexRow].Cells[7].Value = orderSearch[j].FactOutQTY.ToString("N0");

                            Color color = Color.White;

                            if (countOperationForCurrentEquip % 2 == 0)
                            {
                                color = Color.Gainsboro;
                            }

                            dataGridViewOrderDetails.Rows[indexRow].DefaultCellStyle.BackColor = color;
                        }
                    }
                }

                for (int l = 0; l < idManOrderItem.Count; l++)
                {
                    List<int> _planOutQTY = new List<int>();

                    for (int j = 0; j < orderSearch.Count; j++)
                    {
                        if (idManOrderItem[l] == orderSearch[j].IDManOrderJobItem)
                        {
                            if (orderSearch[j].Flags != 576)
                            {
                                if (!_planOutQTY.Contains(orderSearch[j].PlanOutQTY))
                                {
                                    fullPlanOutQTY += orderSearch[j].PlanOutQTY;
                                    _planOutQTY.Add(orderSearch[j].PlanOutQTY);
                                }
                                //break;
                                //Console.WriteLine(fullPlanOutQTY + " + " + orderSearch[j].PlanOutQTY);
                            }
                        }
                    }
                }
                

                indexRow = dataGridViewOrderDetails.Rows.Add();

                dataGridViewOrderDetails.Rows[indexRow].Cells[3].Value = fullPlanOutQTY.ToString("N0");
                dataGridViewOrderDetails.Rows[indexRow].Cells[7].Value = fullFactOutQTY.ToString("N0");

                dataGridViewOrderDetails.Rows[indexRow].DefaultCellStyle.Font = new Font(dataGridViewOrderDetails.Font, FontStyle.Bold);
                dataGridViewOrderDetails.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;
                dataGridViewOrderDetails.Rows[indexRow].DefaultCellStyle.BackColor = Color.Silver;
            }
        }

        private int GetAmountOrder()
        {
            int result = 0;



            return result;
        }

        private void LoadCountShiftsToComboBox()
        {
            INISettings settings = new INISettings();

            int countShifts = settings.GetCountShifts();

            comboBox1.Items.Clear();

            for (int i = 1; i <= countShifts; i++)
            {
                comboBox1.Items.Add(i + " смена");
            }
        }

        private void TabControlSelectedIndexChanged(int selectedIndex)
        {
            INISettings settings = new INISettings();

            if (selectedIndex == 0)
            {
                loadParameter = true;
                comboBox1.SelectedIndex = 0;

                metroSetSwitch1.Switched = settings.GetLoadCurrentShift();
                metroSetSwitch2.Switched = settings.GetAutoUpdateStatistic();
                formattedNumericUpDown4.Value = settings.GetPeriodAutoUpdateStatistic();
                loadParameter = false;

                ReloadOrdersSelectedDate();
            }

            if (selectedIndex == 1)
            {
                //UpdateStatistics();
                ChangeDate();
            }

            if (selectedIndex == 3)
            {
                LoadCategoryToListView();
                LoadParameterFromIniFile();
            }

            if (selectedIndex == 4)
            {
                LoadViewList();
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

            bool givenShiftNumber = settings.GetGivenShiftNumber();

            metroSetCheckBox2.Checked = givenShiftNumber;
            
            bool calculateShiftsInIdletime = settings.GetCalculateShiftsInIdletime();

            metroSetCheckBox6.Checked = !calculateShiftsInIdletime;
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

            bool givenShiftNumber = metroSetCheckBox2.Checked;

            settings.SetGivenShiftNumber(givenShiftNumber);

            bool calculateShiftsInIdletime = !metroSetCheckBox6.Checked;

            settings.SetCalculateShiftsInIdletime(calculateShiftsInIdletime);
        }

        private void LoadViewList()
        {
            ValueView view = new ValueView();

            viewsList = view.LoadViewList();

            comboBox5.Items.Clear();

            for (int i = 0; i < viewsList.Count; i++)
            {
                comboBox5.Items.Add(viewsList[i].Name);
            }

            comboBox5.Items.Add("новый");

            if (comboBoxSelectViewsPreviousIndex == -1)
            {
                comboBox5.SelectedIndex = 0;
            }
            else
            {
                //comboBox5.SelectedIndex = comboBoxSelectViewsPreviousIndex;
            }
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

            ChangeStateTimer();

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
            DateTime currentDateTime = DateTime.Now;

            if (metroSetSwitch2.Switched)
            {
                int updatePeriod = (int)formattedNumericUpDown4.Value;
                int lostMin = lastTimeUpdateShiftStatistic.AddMinutes(updatePeriod).Subtract(currentDateTime).Minutes + 1;

                button2.Text = "Обновление (" + lostMin + ")";

                if (lastTimeUpdateShiftStatistic.AddMinutes(updatePeriod) <= currentDateTime)
                {
                    LoadOrdersForSelectedDate();
                }
            }
            
            if (currentDateTime.Second == 0)
            {
                StartTaskUpdateApplication();
            }
        }

        private void ChangeStateTimer()
        {
            if (metroSetSwitch2.Switched && CheckCurrentShift(dateTimePicker1.Value, comboBox1.SelectedIndex + 1))
            {
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;

                button2.Text = "Обновление";
            }
        }

        private void metroSetSwitch2_SwitchedChanged(object sender)
        {
            INISettings settings = new INISettings();

            if (metroSetSwitch2.Switched)
            {
                //timer1.Enabled = true;

                formattedNumericUpDown4.Visible = true;
                metroSetLabel3.Visible = true;
            }
            else
            {
                //timer1.Enabled = false;

                formattedNumericUpDown4.Visible = false;
                metroSetLabel3.Visible = false;
            }

            ChangeStateTimer();

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



        private void tableLayoutPanel12_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPages();

            comboBoxSelectViewsPreviousIndex = comboBox5.SelectedIndex;
        }

        private void LoadPages()
        {
            ClearParameterViewMonitor();

            if (comboBox5.SelectedIndex < comboBox5.Items.Count - 1)
            {
                string path = viewsList[comboBox5.SelectedIndex].Path;

                label3.Text = path;

                if (File.Exists(path))
                {
                    label3.ForeColor = Color.DarkGreen;

                    ValuePagesList valuePagesList = new ValuePagesList(path);

                    pages?.Clear();

                    pages = valuePagesList.LoadPagesList();

                    LoadViewParameter(path);
                    LaodPagesToListView(pages);
                }
                else
                {
                    label3.ForeColor = Color.DarkRed;
                }
            }
            else
            {
                label3.Text = "";

                FormAddEditViewingSource fm = new FormAddEditViewingSource();
                fm.ShowDialog();

                if (fm.NewValue)
                {
                    LoadViewList();
                    comboBox5.SelectedIndex = comboBox5.Items.Count - 2;
                }
                else
                {
                    comboBox5.SelectedIndex = comboBoxSelectViewsPreviousIndex;
                }
            }
        }

        private void LaodPagesToListView(List<Page> pages)
        {
            loadPageList = true;

            for (int i = 0; i < pages.Count; i++)
            {
                ListViewItem item = new ListViewItem();

                item.Name = pages[i].Id.ToString();
                item.Text = (listViewPages.Items.Count + 1).ToString();
                item.SubItems.Add(pages[i].Name);

                item.Checked = pages[i].ActivePage;

                listViewPages.Items.Add(item);

            }

            loadPageList = false;
        }

        private void LoadViewParameter(string path)
        {
            INIView view = new INIView(path);

            formattedNumericUpDown5.Value = view.GetPeriod();
            formattedNumericUpDown6.Value = view.GetWidthNumberCol();
            formattedNumericUpDown7.Value = view.GetWidthNameCol();
            formattedNumericUpDown8.Value = view.GetWidthWorkingOutCol();
            formattedNumericUpDown9.Value = view.GetWidthResultsCol();

            metroSetCheckBox3.Checked = view.GetViewCurrentDay();
            metroSetCheckBox4.Checked = view.GetColWorksOutAutoWidth();
            metroSetCheckBox5.Checked = view.GetAutoAddDays();
        }

        private void SaveViewParameter()
        {
            if (comboBox5.SelectedIndex < viewsList.Count)
            {
                string path = viewsList[comboBox5.SelectedIndex].Path;

                if (File.Exists(path))
                {
                    INIView view = new INIView(path);

                    view.SetPeriod(formattedNumericUpDown5.Value);
                    view.SetWidthNumberCol(formattedNumericUpDown6.Value);
                    view.SetWidthNameCol(formattedNumericUpDown7.Value);
                    view.SetWidthWorkingOutCol(formattedNumericUpDown8.Value);
                    view.SetWidthResultsCol(formattedNumericUpDown9.Value);

                    view.SetViewCurrentDay(metroSetCheckBox3.Checked);
                    view.SetColWorksOutAutoWidth(metroSetCheckBox4.Checked);
                    view.SetAutoAddDays(metroSetCheckBox5.Checked);
                }
            }
        }

        private void ClearParameterViewMonitor()
        {
            listViewPages.Items.Clear();

            formattedNumericUpDown5.Value = 0;
            formattedNumericUpDown6.Value = 0;
            formattedNumericUpDown7.Value = 0;
            formattedNumericUpDown8.Value = 0;
            formattedNumericUpDown9.Value = 0;

            metroSetCheckBox3.Checked = false;
            metroSetCheckBox4.Checked = false;
            metroSetCheckBox5.Checked = false;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveParameterBeforeClosing();
        }

        private void buttonViewAdd_Click(object sender, EventArgs e)
        {
            string path = viewsList[comboBox5.SelectedIndex].Path;

            FormAddEditViewing fm = new FormAddEditViewing(path);
            fm.ShowDialog();

            if (fm.NewValue)
            {
                LoadPages();
            }
        }

        private void buttonViewEdit_Click(object sender, EventArgs e)
        {
            LoadPageForEdit();
        }
        
        private void LoadPageForEdit()
        {
            if (listViewPages.SelectedItems.Count > 0)
            {
                string path = viewsList[comboBox5.SelectedIndex].Path;

                FormAddEditViewing fm = new FormAddEditViewing(path, Convert.ToInt32(listViewPages.Items[listViewPages.SelectedIndices[0]].Name));
                fm.ShowDialog();

                if (fm.NewValue)
                {
                    LoadPages();
                }
            }
        }

        private void buttonViewUp_Click(object sender, EventArgs e)
        {
            if (listViewPages.SelectedItems.Count > 0 && listViewPages.SelectedIndices[0] != 0)
            {
                string path = viewsList[comboBox5.SelectedIndex].Path;

                ValuePagesList valuePages = new ValuePagesList(path);

                int selectIndex = Convert.ToInt32(listViewPages.Items[listViewPages.SelectedIndices[0]].Name);

                int secondIndex = selectIndex - 1;

                valuePages.SwapPage(selectIndex, secondIndex);

                LoadPages();
            }
        }

        private void buttonViewDown_Click(object sender, EventArgs e)
        {
            if (listViewPages.SelectedItems.Count > 0 && listViewPages.SelectedIndices[0] != listViewPages.Items.Count - 1)
            {
                string path = viewsList[comboBox5.SelectedIndex].Path;

                ValuePagesList valuePages = new ValuePagesList(path);

                int selectIndex = Convert.ToInt32(listViewPages.Items[listViewPages.SelectedIndices[0]].Name);

                int secondIndex = selectIndex + 1;

                valuePages.SwapPage(selectIndex, secondIndex);

                LoadPages();
            }
        }

        private void buttonViewDel_Click(object sender, EventArgs e)
        {
            if (listViewPages.SelectedItems.Count > 0 && listViewPages.SelectedIndices[0] != 0)
            {
                DialogResult result;

                result = MessageBox.Show("Вы действительно хотите удалить: " + listViewPages.SelectedItems[0].SubItems[1].Text + "?", "Удаление страницы", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    string path = viewsList[comboBox5.SelectedIndex].Path;

                    ValuePagesList valuePages = new ValuePagesList(path);

                    int selectIndex = Convert.ToInt32(listViewPages.Items[listViewPages.SelectedIndices[0]].Name);

                    valuePages.DeletePage(selectIndex);

                    LoadPages();
                }
            }
        }

        private void listViewPages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPages.SelectedIndices.Count > 0)
            {
                buttonViewEdit.Enabled = true;
                buttonViewDel.Enabled = true;

                if (listViewPages.SelectedIndices[0] == 0)
                {
                    buttonViewUp.Enabled = false;
                }
                else
                {
                    buttonViewUp.Enabled = true;
                }

                if (listViewPages.SelectedIndices[0] == listViewPages.Items.Count - 1)
                {
                    buttonViewDown.Enabled = false;
                }
                else
                {
                    buttonViewDown.Enabled = true;
                }
            }
            else
            {
                buttonViewEdit.Enabled = false;
                buttonViewDel.Enabled = false;
                buttonViewUp.Enabled = false;
                buttonViewDown.Enabled = false;
            }
        }

        private void listViewPages_DoubleClick(object sender, EventArgs e)
        {
            LoadPageForEdit();
        }

        private void listViewPages_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!loadPageList)
            {
                string path = viewsList[comboBox5.SelectedIndex].Path;

                ValuePagesList valuePages = new ValuePagesList(path);

                int selectIndex = Convert.ToInt32(e.Item.Name);

                valuePages.ChangeActivePage(selectIndex);

                //LoadPages();
            }
        }

        private void buttonSettingsSave_Click(object sender, EventArgs e)
        {
            SaveViewParameter();
        }

        private void buttonSettingsReload_Click(object sender, EventArgs e)
        {
            LoadPages();
        }

        private void buttonEditViewPath_Click(object sender, EventArgs e)
        {
            FormAddEditViewingSource fm = new FormAddEditViewingSource(comboBox5.SelectedIndex + 1);
            fm.ShowDialog();

            if (fm.NewValue)
            {
                LoadViewList();
                comboBox5.SelectedIndex = comboBoxSelectViewsPreviousIndex;
            }
            else
            {
                //comboBox5.SelectedIndex = comboBoxSelectViewsPreviousIndex;
            }
        }

        private void buttonDelViewPath_Click(object sender, EventArgs e)
        {
            DialogResult result;

            result = MessageBox.Show("Вы действительно хотите удалить: " + comboBox5.Text + "?", "Удаление источника", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ValueView valueView = new ValueView();

                valueView.DeleteViewSource(comboBox5.SelectedIndex + 1);

                LoadViewList();

                if (comboBoxSelectViewsPreviousIndex > 0)
                {
                    comboBox5.SelectedIndex = comboBoxSelectViewsPreviousIndex - 1;
                }
                else
                {
                    comboBox5.SelectedIndex = 0;
                }
            }
        }

        private void StartSearch()
        {
            ValueOrderSearch valueOrder = new ValueOrderSearch();

            ordersHead?.Clear();
            comboBox6.Items.Clear();
            ClearColomnsFromDataGridForSearchedOrder();

            if (textBox1.Text != "")
            {
                ordersHead = valueOrder.SearchOrderHeadIndexes(textBox1.Text);

                if (ordersHead.Count == 0)
                {
                    comboBox6.Items.Add("<ничего не найдено>");
                }

                if (ordersHead.Count > 1)
                {
                    comboBox6.Items.Add("<найдено заказов: " + ordersHead.Count + ">");
                }

                for (int i = 0; i < ordersHead.Count; i++)
                {
                    comboBox6.Items.Add(ordersHead[i].OrderNumber + ": " + ordersHead[i].OrderCustomer);
                }

                comboBox6.SelectedIndex = 0;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            StartSearch();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedOrderIndex = -1;

            if (ordersHead.Count > 0)
            {
                if (ordersHead.Count == 1)
                {
                    selectedOrderIndex = comboBox6.SelectedIndex;
                }

                if (ordersHead.Count > 1 && comboBox6.SelectedIndex > 0)
                {
                    selectedOrderIndex = comboBox6.SelectedIndex - 1;
                }

                if (selectedOrderIndex >= 0)
                {
                    LoadSelectedOrderSearched(selectedOrderIndex);
                }
                else
                {
                    ClearColomnsFromDataGridForSearchedOrder();
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                StartSearch(); ;
            }
        }
    }
}
