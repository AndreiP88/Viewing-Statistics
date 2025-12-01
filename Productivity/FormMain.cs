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
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using System.Linq;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Common;
using System.Data.SqlClient;

using ExportExcel;
using System.Xml.Linq;
using System.Reflection;
using Microsoft.SqlServer.Server;

namespace Productivity
{
    public partial class FormMain : MetroSetForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        CancellationTokenSource cancelTokenSource;

        const int PageShiftDetails = 0;
        const int PageMonthDetails = 1;
        const int PageStatistic = 2;
        const int PageStatisticYear = 3;
        const int PageStatisticUser = 4;
        const int PagePlanOrders = 5;
        const int PageOrderDetailss = 6;
        const int PageSettings = 7;
        const int PageParametersMonitor = 8;

        int metroSetTabControlPreviousIndex = -1;
        int comboBoxSelectViewsPreviousIndex = -1;
        bool loadCategoryList = true;
        bool loadPageList = true;
        bool loadParameter = true;
        bool loadShiftsParameter = true;
        bool loadCategoryStatistic = false;
        bool loadCategoryYearStatistic = false;
        bool loadUserStatistic = false;
        bool loadPlan = false;

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> usersShort = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();
        Dictionary<string, int> rowIndexes = new Dictionary<string, int>();

        List<User> usersList;
        List<ViewPath> viewsList;
        List<PageView> pages;
        List<OrderHeadSearch> ordersHead;
        List<UserWorkingOutput> userWorkingOutputs = new List<UserWorkingOutput>();
        List<User> userListForYear;
        List<Equip> equipListForSelectedCategory;

        List<Category> categoriesList;

        List<ViewYearStatistic> viewYearStatistics = new List<ViewYearStatistic>();
        List<List<UserWorkingOutput>> userWorkingOutputsForYear = new List<List<UserWorkingOutput>>();

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

                users?.Clear();
                usersShort?.Clear();

                users = usersValue.LoadAllUsersNames();
                usersShort = usersValue.LoadAllUsersNames(true);
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

                machines?.Clear();

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

        private void LoadCategoryToStatisticComboBox()
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            categoriesList?.Clear();
            categoriesList = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            loadCategoryList = true;

            comboBoxStatCategory.Items.Clear();

            for (int i = 0; i < categoriesList.Count; i++)
            {
                comboBoxStatCategory.Items.Add(categoriesList[i].Name);
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
            comboBoxStatYear.Items.Clear();
            comboBoxStatYearSelectYear.Items.Clear();
            comboBoxUserYears.Items.Clear();

            for (int i = yearStart; i <= yearEnd; i++)
            {
                comboBox3.Items.Add(i.ToString());
                comboBoxStatYear.Items.Add(i.ToString());
                comboBoxStatYearSelectYear.Items.Add(i.ToString());
                comboBoxUserYears.Items.Add(i.ToString());
            }
        }

        private void AddMonthToComboBox()
        {
            comboBox2.Items.Clear();
            comboBoxStatMonth.Items.Clear();

            string[] month = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

            comboBox2.Items.AddRange(month);
            comboBoxStatMonth.Items.AddRange(month);
        }

        private void SelectCurrentMonth()
        {
            comboBox3.Text = DateTime.Now.Year.ToString();
            comboBoxStatYear.Text = DateTime.Now.Year.ToString();
            comboBoxStatYearSelectYear.Text = DateTime.Now.Year.ToString();
            comboBoxUserYears.Text = DateTime.Now.Year.ToString();

            comboBox2.SelectedIndex = DateTime.Now.Month - 1;
            comboBoxStatMonth.SelectedIndex = DateTime.Now.Month - 1;
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

            int w = 65;//(width - 560) / (days);

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

            indexColumn = dataGridView1.Columns.Add(@"colGroup", @"Простои, смен");
            dataGridView1.Columns[indexColumn].Width = 100;
            dataGridView1.Columns[indexColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            indexColumn = dataGridView1.Columns.Add(@"colGroup", @"Простои, часов");
            dataGridView1.Columns[indexColumn].Width = 100;
            dataGridView1.Columns[indexColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            indexColumn = dataGridView1.Columns.Add(@"colGroup", @"Выработка");
            dataGridView1.Columns[indexColumn].Width = 140;
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

            AddCellToGrid(0, days * countShifts + 4, 2);
            dataGridView1.Rows[0].Cells[days * countShifts + 4].Value = "Простои";

            AddCellToGrid(0, days * countShifts + 6, 3);
            dataGridView1.Rows[0].Cells[days * countShifts + 6].Value = "Производительность";

            for (int i = 2; i <= days * countShifts; i+=countShifts)
            {
                AddCellToGrid(0, i, countShifts);

                dataGridView1.Rows[0].Cells[i].Value = ((i - 2 + countShifts) / countShifts).ToString("D2") + "." + month.ToString("D2");
                dataGridView1.Rows[0].Cells[i + 1].Value = ((i + 1 - 2 + countShifts) / countShifts).ToString("D2") + "." + month.ToString("D2");
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
            dataGridView1.Rows[1].Cells[days * countShifts + 4].Value = "Смен";

            AddCellToGrid(1, days * countShifts + 5);
            dataGridView1.Rows[1].Cells[days * countShifts + 5].Value = "Часов";

            AddCellToGrid(1, days * countShifts + 6);
            dataGridView1.Rows[1].Cells[days * countShifts + 6].Value = "Выработка";

            AddCellToGrid(1, days * countShifts + 7);
            dataGridView1.Rows[1].Cells[days * countShifts + 7].Value = "Отклонение";

            AddCellToGrid(1, days * countShifts + 8);
            dataGridView1.Rows[1].Cells[days * countShifts + 8].Value = "%";

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

        private async Task ChangeDateAsync()
        {
            if (!loadShiftsParameter)
            {
                if (comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
                {
                    await UpdateStatistics();
                }
            }
        }

        private DateTime ReturnDateFromInputParameter(int year, int month)
        {
            DateTime result = DateTime.MinValue.AddYears(year - 1).AddMonths(month - 1);

            return result;
        }

        private async Task LoadStartsValuesAsync()
        {
            INISettings settings = new INISettings();

            LoadAllUsers();
            LoadMachine();

            loadShiftsParameter = true;

            AddYearsToComboBox(2015, DateTime.Now.Year);
            AddMonthToComboBox();

            SelectCurrentMonth();

            LoadCountShiftsToComboBox();

            LoadCategoryToStatisticComboBox();

            comboBoxStatCategory.SelectedIndex = 0;
            
            //Update Later
            comboBox4.SelectedIndex = 0;

            metroSetTabControl1.SelectTab(settings.GetLastTabIndex());

            loadShiftsParameter = false;

            await TabControlSelectedIndexChangedAsync(metroSetTabControl1.SelectedIndex);
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

        private async Task LoadUsersList(List<Category> categoryEquips, DateTime date)
        {
            try
            {
                usersList = new List<User>();

                ValueUsers usersValue = new ValueUsers();

                List<int> equips = CategoryEquipToListSelectedEquip(categoryEquips);
                
                //usersList = usersValue.LoadUsersList(equips, date);
                usersList = await usersValue.LoadUsersListFromSelectMonth(equips, date);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
                LogWrite(ex);
            }
        }

        private async Task<List<User>> LoadShiftsListAsync()
        {
            ValueShifts valueShifts = new ValueShifts();

            INISettings settings = new INISettings();

            int countShifts = settings.GetCountShifts();
            bool givenShiftNumber = settings.GetGivenShiftNumber();

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            //usersList = await valueShifts.LoadShiftsForSelectedMonth(usersList, selectDate, countShifts, givenShiftNumber);
            //сдесь проверить usersList, не пустой ли?
            return await valueShifts.LoadShiftsForSelectedMonth(usersList, selectDate, countShifts, givenShiftNumber);
        }
        private bool IsThereEquipForUser(List<UserShift> shifts, int equip)
        {
            for (int i = 0; i < shifts.Count; i++)
            {
                for (int j = 0; j < shifts[i].Orders.Count; j++)
                {
                    if (shifts[i].Orders[j].IdEquip == equip)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private async Task AddUsersToListViewAsync(CancellationToken token)
        {
            await Task.Run(() =>
            {
                ValueCategoryes valueCategoryes = new ValueCategoryes();
                INISettings settings = new INISettings();

                List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

                List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

                bool viewAllEquipsForUser = settings.GetLoadAllEquipForUser();
                int selectedIndex = 0;

                Invoke(new Action(() =>
                {
                    rowIndexes.Clear();

                    selectedIndex = comboBox4.SelectedIndex;
                }));

                if (token.IsCancellationRequested)
                {
                    return;
                }

                if (selectedIndex == 0)
                {
                    for (int i = 0; i < equips.Count; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

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

                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        Invoke(new Action(() =>
                        {
                            AddItemToGrid("e" + equips[i], "", machine, Color.Gray);
                        }));

                        for (int j = 0; j < usersList.Count; j++)
                        {
                            if (token.IsCancellationRequested)
                            {
                                break;
                            }

                            if (IsThereEquipForUser(usersList[j].Shifts, equips[i]))
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

                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }

                                Invoke(new Action(() =>
                                {
                                    AddItemToGrid(CreateNameListViewItem(equips[i], usersList[j].Id), countUserForCurrentEquip.ToString(), user, color);
                                }));
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
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        for (int j = 0; j < usersList.Count; j++)
                        {
                            if (token.IsCancellationRequested)
                            {
                                break;
                            }

                            for (int k = 0; k < usersList[j].Shifts.Count; k++)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    break;
                                }

                                for (int l = 0; l < usersList[j].Shifts[k].Orders.Count; l++)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        break;
                                    }

                                    if (!usersCurrent.Contains(usersList[j].Id) && IsThereEquipForUser(usersList[j].Shifts, equips[i]))
                                    {
                                        usersCurrent.Add(usersList[j].Id);
                                    }

                                    if (viewAllEquipsForUser && !equipsCurrent.Contains(usersList[j].Shifts[k].Orders[l].IdEquip))
                                    {
                                        equipsCurrent.Add(usersList[j].Shifts[k].Orders[l].IdEquip);
                                    }
                                }
                            }

                            /*if (!usersCurrent.Contains(usersList[j].Id) && IsThereEquipForUser(usersList[j].Shifts, equips[i]))
                            {
                                usersCurrent.Add(usersList[j].Id);
                            }

                            if (viewAllEquipsForUser && !equipsCurrent.Contains(usersList[j].Equip))
                            {
                                equipsCurrent.Add(usersList[j].Equip);
                            }*/
                        }
                    }

                    if (!viewAllEquipsForUser)
                    {
                        equipsCurrent = equips;
                    }

                    for (int i = 0; i < usersCurrent.Count; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        string user = "";

                        if (users.ContainsKey(usersCurrent[i]))
                        {
                            user += users[usersCurrent[i]];
                        }
                        else
                        {
                            user += "Работник " + usersCurrent[i];
                        }

                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        Invoke(new Action(() =>
                        {
                            AddItemToGrid("u" + usersCurrent[i], "", user, Color.Gray);
                        }));

                        int countEquipForCurrentUser = 0;

                        for (int j = 0; j < equipsCurrent.Count; j++)
                        {
                            if (token.IsCancellationRequested)
                            {
                                break;
                            }

                            int index = usersList.FindIndex((v) => v.Id == usersCurrent[i]);

                            if (index >= 0)
                            {
                                if (IsThereEquipForUser(usersList[index].Shifts, equipsCurrent[j]))
                                {
                                    countEquipForCurrentUser++;

                                    string machine = "    ";

                                    if (machines.ContainsKey(equipsCurrent[j]))
                                    {
                                        machine += machines[equipsCurrent[j]];
                                    }
                                    else
                                    {
                                        machine += "Оборудование " + equipsCurrent[j];
                                    }

                                    Color color = Color.White;

                                    if (countEquipForCurrentUser % 2 == 0)
                                    {
                                        color = Color.LightGray;
                                    }

                                    if (token.IsCancellationRequested)
                                    {
                                        break;
                                    }

                                    Invoke(new Action(() =>
                                    {
                                        AddItemToGrid(CreateNameListViewItem(equipsCurrent[j], usersList[index].Id), countEquipForCurrentUser.ToString(), machine, color);
                                    }));
                                }
                            }
                        }
                    }
                }
            }, token);
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

        private void StartAddingWorkingTimeToListView(CancellationToken token)
        {
            Task taskDetails = new Task(() => AddWorkingTimeUsersToListView(token), token);
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
                //LoadShiftsList();
            }));

            List<WorkingOut> equipsListWorkingOut = new List<WorkingOut>();
            List<WorkingOut> usersListWorkingOut = new List<WorkingOut>();

            List<WorkingOut> listWorkingOut = new List<WorkingOut>();

            //List<int> usersCurrent = new List<int>();
            INISettings settings = new INISettings();
            ValueDateTime timeValues = new ValueDateTime();
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

            bool viewAllEquipsForUser = settings.GetLoadAllEquipForUser();

            int fullOutput = settings.GetNormTime();
            int countShifts = settings.GetCountShifts();
            
            bool calculateShiftsInIdletime = settings.GetCalculateShiftsInIdletime();

            bool viewPercentWorkingOut;
            int indexViewValue = -1;

            Invoke(new Action(() =>
            {
                indexViewValue = comboBox7.SelectedIndex;
            }));

            switch (indexViewValue)
            {
                case 0:
                    viewPercentWorkingOut = false;
                    break;
                case 1:
                    viewPercentWorkingOut = true;
                    break;
                default:
                    viewPercentWorkingOut = false;
                    break;
            }

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
                            List<int> currentEquipsList = new List<int>();

                            //Сделать выборку оборудования выбранного участка, если включена данная опция???
                            for (int k = 0; k < shift.Orders.Count; k++)
                            {
                                if (!currentEquipsList.Contains(shift.Orders[k].IdEquip))
                                {
                                    if (viewAllEquipsForUser)
                                    {
                                        currentEquipsList.Add(shift.Orders[k].IdEquip);
                                    }
                                    else
                                    {
                                        if (equips.Contains(shift.Orders[k].IdEquip))
                                        {
                                            currentEquipsList.Add(shift.Orders[k].IdEquip);
                                        }
                                    }
                                }
                            }

                            for (int k = 0; k < currentEquipsList.Count; k++)
                            {
                                float timeWorkigOut = CalculateWorkTime(shift.Orders, currentEquipsList[k]);
                                float timeBacklog = 0;
                                bool isThereOrdersInWorking = IsThereOrdersInWorking(shift.Orders, currentEquipsList[k]);
                                bool isThereOrdersInWorkingForAllEuips = IsThereOrdersInWorkingForAllEquips(shift.Orders);

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

                                    int indexListWorkingOut = listWorkingOut.FindIndex(
                                                    (v) => v.Id == usersList[i].Id &&
                                                           v.Equip == currentEquipsList[k]
                                                           );

                                    if (indexListWorkingOut != -1)
                                    {
                                        listWorkingOut[indexListWorkingOut].WorkingOutSumm += timeWorkigOut;
                                        listWorkingOut[indexListWorkingOut].WorkingOutBacklog+= timeBacklog;
                                        listWorkingOut[indexListWorkingOut].NumberOfShiftsWorked++;

                                        if (!isThereOrdersInWorking)
                                        {
                                            listWorkingOut[indexListWorkingOut].NumberOfIdleShifts++;
                                        }
                                    }
                                    else
                                    {
                                        listWorkingOut.Add(new WorkingOut(usersList[i].Id, currentEquipsList[k]));

                                        listWorkingOut[listWorkingOut.Count - 1].WorkingOutSumm = timeWorkigOut;
                                        listWorkingOut[listWorkingOut.Count - 1].WorkingOutBacklog = timeBacklog;
                                        listWorkingOut[listWorkingOut.Count - 1].NumberOfShiftsWorked = 1;

                                        if (!isThereOrdersInWorking)
                                        {
                                            listWorkingOut[listWorkingOut.Count - 1].NumberOfIdleShifts = 1;
                                        }
                                    }
                                }

                                //Выработка для оборудования
                                int indexEquipsList = equipsListWorkingOut.FindIndex(
                                                    (v) => v.Id == currentEquipsList[k]
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

                                            if (!isThereOrdersInWorking)
                                            {
                                                if (equipsListWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].NumberOfIdleShifts != 0)
                                                {
                                                    equipsListWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].NumberOfIdleShifts = 1;
                                                }
                                            }
                                            else
                                            {
                                                equipsListWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].NumberOfIdleShifts = 0;
                                            }
                                        }
                                        else
                                        {
                                            equipsListWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].NumberOfIdleShifts = 0;
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
                                                equipsListWorkingOut[indexEquipsList].WorkingOutList[equipsListWorkingOut[indexEquipsList].WorkingOutList.Count - 1].NumberOfIdleShifts = 1;
                                            }
                                            else
                                            {
                                                equipsListWorkingOut[indexEquipsList].WorkingOutList[equipsListWorkingOut[indexEquipsList].WorkingOutList.Count - 1].NumberOfIdleShifts = 0;
                                            }

                                            /*if (!isThereOrdersInWorking)
                                            {
                                                equipsListWorkingOut[indexEquipsList].NumberOfIdleShifts++;
                                            }*/
                                        }
                                        else
                                        {
                                            equipsListWorkingOut[indexEquipsList].WorkingOutList[equipsListWorkingOut[indexEquipsList].WorkingOutList.Count - 1].NumberOfIdleShifts = 0;
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
                                        currentEquipsList[k]
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
                                            equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList[equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList.Count - 1].NumberOfIdleShifts = 1;
                                        }
                                        else
                                        {
                                            equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList[equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList.Count - 1].NumberOfIdleShifts = 0;
                                        }

                                        /*if (!isThereOrdersInWorking)
                                        {
                                            equipsListWorkingOut[equipsListWorkingOut.Count - 1].NumberOfIdleShifts++;
                                        }*/
                                    }
                                    else
                                    {
                                        equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList[equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutList.Count - 1].NumberOfIdleShifts = 0;
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

                                            if (!isThereOrdersInWorkingForAllEuips)
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

                                        if (!isThereOrdersInWorkingForAllEuips)
                                        {
                                            usersListWorkingOut[usersListWorkingOut.Count - 1].NumberOfIdleShifts++;
                                        }
                                    }
                                }

                                Invoke(new Action(() =>
                                {
                                    string key = CreateNameListViewItem(currentEquipsList[k], usersList[i].Id);

                                    int indexListWorkingOut = listWorkingOut.FindIndex(
                                                    (v) => v.Id == usersList[i].Id &&
                                                           v.Equip == currentEquipsList[k]
                                                           );

                                    float workingOutUser = 0;
                                    float workingOutBacklog = 0;
                                    int numberOfShifts = 0;
                                    int numberOfIdleShifts = 0;

                                    if (indexListWorkingOut != -1)
                                    {
                                        workingOutUser = listWorkingOut[indexListWorkingOut].WorkingOutSumm;
                                        workingOutBacklog = listWorkingOut[indexListWorkingOut].WorkingOutBacklog;
                                        numberOfShifts = listWorkingOut[indexListWorkingOut].NumberOfShiftsWorked;
                                        numberOfIdleShifts = listWorkingOut[indexListWorkingOut].NumberOfIdleShifts;
                                    }

                                    if (rowIndexes.ContainsKey(key))
                                    {
                                        int indexRow = rowIndexes[key];

                                        string vOutValue = "";

                                        if (!viewPercentWorkingOut)
                                        {
                                            vOutValue = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                                        }
                                        else
                                        {
                                            vOutValue = (timeWorkigOut / fullOutput).ToString("P1");
                                        }

                                        dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = vOutValue;

                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = numberOfShifts;// usersList[i].Shifts.Count;
                                                                                                                                              //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString((int)Math.Round(usersList[i].WorkingOutUser + usersList[i].WorkingOutBacklog));
                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString((fullOutput + 30) * numberOfShifts);

                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = numberOfIdleShifts;
                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 5].Value = timeValues.MinuteToTimeString((fullOutput + 30) * numberOfIdleShifts);

                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = timeValues.MinuteToTimeString((int)Math.Round(workingOutUser)) + " / " + timeValues.MinuteToTimeString(fullOutput * numberOfShifts);
                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 7].Value = timeValues.MinuteToTimeString((int)Math.Round(-workingOutBacklog));
                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 8].Value = (workingOutUser / (workingOutUser + workingOutBacklog)).ToString("P1");

                                        /*dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = timeValues.MinuteToTimeString((int)Math.Round(usersList[i].WorkingOutUser));
                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 5].Value = timeValues.MinuteToTimeString((int)Math.Round(usersList[i].WorkingOutBacklog));
                                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = (usersList[i].WorkingOutUser / (usersList[i].WorkingOutUser + usersList[i].WorkingOutBacklog)).ToString("P1");*/
                                    }
                                }));
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < equipsListWorkingOut.Count; i++)
            {
                for (int j = 0; j < equipsListWorkingOut[i].WorkingOutList.Count; j++)
                {
                    equipsListWorkingOut[i].NumberOfIdleShifts += equipsListWorkingOut[i].WorkingOutList[j].NumberOfIdleShifts;
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

                bool viewPercentVorkingOut;
                int indexViewValue = -1;

                Invoke(new Action(() =>
                {
                    indexViewValue = comboBox7.SelectedIndex;
                }));

                switch (indexViewValue)
                {
                    case 0:
                        viewPercentVorkingOut = false;
                        break;
                    case 1:
                        viewPercentVorkingOut = true;
                        break;
                    default:
                        viewPercentVorkingOut = false;
                        break;
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

                            string vOutValue = "";

                            if (!viewPercentVorkingOut)
                            {
                                vOutValue = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                            }
                            else
                            {
                                vOutValue = (timeWorkigOut / fullOutput).ToString("P1");
                            }

                            dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = vOutValue;
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
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(numberOfShiftsWorkedEquips * (fullOutput + 30));

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = numberOfIdleShiftsEquips;
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 5].Value = timeValues.MinuteToTimeString(numberOfIdleShiftsEquips * (fullOutput + 30));

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = timeValues.MinuteToTimeString((int)Math.Round(equipsListWorkingOut[i].WorkingOutSumm)) + " / " + timeValues.MinuteToTimeString(numberOfShiftsWorkedEquips * fullOutput);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 7].Value = timeValues.MinuteToTimeString((int)Math.Round(equipsListWorkingOut[i].WorkingOutSumm) - (numberOfShiftsWorkedEquips - numberOfIdleShiftsEquips) * fullOutput);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 8].Value = (equipsListWorkingOut[i].WorkingOutSumm / ((numberOfShiftsWorkedEquips - numberOfIdleShiftsEquips) * fullOutput)).ToString("P1");
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

                bool viewPercentVorkingOut;
                int indexViewValue = -1;

                Invoke(new Action(() =>
                {
                    indexViewValue = comboBox7.SelectedIndex;
                }));

                switch (indexViewValue)
                {
                    case 0:
                        viewPercentVorkingOut = false;
                        break;
                    case 1:
                        viewPercentVorkingOut = true;
                        break;
                    default:
                        viewPercentVorkingOut = false;
                        break;
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

                            string vOutValue = "";

                            if (!viewPercentVorkingOut)
                            {
                                vOutValue = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                            }
                            else
                            {
                                vOutValue = (timeWorkigOut / fullOutput).ToString("P1");
                            }

                            dataGridView1.Rows[indexRow].Cells[(day - 1) * countShifts + shiftNumber + 1].Value = vOutValue;
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
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString((int)numberOfShiftsWorkedUsers * (fullOutput + 30));

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 4].Value = numberOfIdleShiftsUsers;
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 5].Value = timeValues.MinuteToTimeString((int)numberOfIdleShiftsUsers * (fullOutput + 30));

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = timeValues.MinuteToTimeString((int)Math.Round(usersListWorkingOut[i].WorkingOutSumm)) + " / " + timeValues.MinuteToTimeString((int)numberOfShiftsWorkedUsers * fullOutput);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 6].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 7].Value = timeValues.MinuteToTimeString((int)Math.Round(usersListWorkingOut[i].WorkingOutSumm) - (numberOfShiftsWorkedUsers - numberOfIdleShiftsUsers) * fullOutput);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 8].Value = (usersListWorkingOut[i].WorkingOutSumm / ((numberOfShiftsWorkedUsers - numberOfIdleShiftsUsers) * fullOutput)).ToString("P1");
                    }
                }));
            }
        }

        private bool IsThereOrdersInWorking(List<UserShiftOrder> orders, int equip)
        {
            bool result = false;

            if (orders.Count == 1 && orders[0].IdManOrderJobItem == -1)
            {
                //if (orders[0].IdEquip == equip)
                {
                    return false;
                }
            }

            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].IdEquip == equip)
                {
                    if (orders[i].IdletimeName == "" || orders[i].FactOutQty > 0)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        private bool IsThereOrdersInWorkingOLD(List<UserShiftOrder> orders, int equip)
        {
            bool result = false;

            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].IdletimeName == "" && orders[i].IdEquip == equip)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private bool IsThereOrdersInWorkingForAllEquips(List<UserShiftOrder> orders)
        {
            bool result = false;

            if (orders.Count == 1 && orders[0].IdManOrderJobItem == -1)
            {
                return false;
            }

            for (int i = 0; i < orders.Count; i++)
            {
                /*if (orders[i].IdManOrderJobItem == -1)
                {
                    result = false;
                    break;
                }*/

                if (orders[i].IdletimeName == "" || orders[i].FactOutQty > 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private float CalculateWorkTime(List<UserShiftOrder> order, int equip)
        {
            float workingOut = 0;

            for (int i = 0; i < order.Count; i++)
            {
                if (order[i].IdEquip == equip)
                {
                    workingOut += CalculateWorkTimeForOneOrder(order[i]);
                }
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

        private async Task UpdateStatistics()
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox7.Enabled = false;
            button1.Enabled = false;

            int year = GetYearFromComboBox();
            int month = GetMonthFromComboBox();

            DateTime selectDate = ReturnDateFromInputParameter(year, month);

            int countDaysFromSellectedMonth = DateTime.DaysInMonth(year, month);

            CreateColomnsToDataGrid(countDaysFromSellectedMonth, month);

            cancelTokenSource?.Cancel();
            Thread.Sleep(300);

            cancelTokenSource = new CancellationTokenSource();

            //List<int> equips = GetSelectegEquipsList();
            List<Category> categoryEquips = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            Stopwatch stopwatch = new Stopwatch();
            
            Console.WriteLine("Start LoadUsersList: " + DateTime.Now.ToString("R"));
            stopwatch.Start();
            await LoadUsersList(categoryEquips, selectDate);
            stopwatch.Stop();
            Console.WriteLine("LoadUsersList: " + stopwatch.ElapsedMilliseconds);

            Console.WriteLine("Start LoadShiftsList: ");
            stopwatch.Start();
            usersList = await LoadShiftsListAsync();
            stopwatch.Stop();
            Console.WriteLine("LoadShiftsList: " + stopwatch.ElapsedMilliseconds + ", usersList = " + usersList.Count);

            Console.WriteLine("Start AddUsersToListView: ");
            //LoadShifts();
            stopwatch.Start();
            await AddUsersToListViewAsync(cancelTokenSource.Token);
            stopwatch.Stop();
            Console.WriteLine("AddUsersToListView: " + stopwatch.ElapsedMilliseconds);

            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox7.Enabled = true;
            button1.Enabled = true;

            if (comboBox7.SelectedIndex == -1)
            {
                comboBox7.SelectedIndex = 0;
            }
            else
            {
                Console.WriteLine("Start LoadShifts: ");
                //LoadShifts();
                stopwatch.Start();
                StartAddingWorkingTimeToListView(cancelTokenSource.Token);
                stopwatch.Stop();
                Console.WriteLine("LoadShifts: " + stopwatch.ElapsedMilliseconds);
                
            }
            Console.WriteLine("End: " + DateTime.Now.ToString("R"));
            //LoadShifts();
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
                        secondDateTime.AddMinutes(breakTime);
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

            string[] colNames = { "№", "Имя", "Заказ", "Заказчик", "Операция", "Тираж", "Дано времени", "Начало", "Завершение", "Продолжительность", "Планируемое время завершения", "Отклонение", "Сделано", "Выработка", "Примечания" };
            int[] colWidth = { 30, 280, 100, 280, 240, 140, 70, 135, 135, 80, 135, 90, 80, 80, 900 };
            DataGridViewContentAlignment[] colAligment = { right, left, left, left, left, left, center, left, left, center, left, center, left, center, left};

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

        private bool IsLastRecordOfOrder(List<UserShiftOrder> userShiftOrders)
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

        private async Task LoadOrdersSelectedDateAndShiftAsync(DateTime selectDate, int selectShift)
        {
            await LoadOrdersSelectedDateAndShiftDetailsAsync(selectDate, selectShift);
            //LoadOrdersSelectedDateAndShiftCompact(selectDate, selectShift);
        }

        private async Task LoadOrdersSelectedDateAndShiftDetailsAsync(DateTime selectDate, int selectShift)
        {
            ChangeStateTimer();

            CreateColomnsToDataGridForOneShift();

            ValueShifts shifts = new ValueShifts();
            ValueDateTime time = new ValueDateTime();
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            INISettings settings = new INISettings();

            bool givenShiftNumber = settings.GetGivenShiftNumber();
            int normTime = settings.GetNormTime();

            List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> equips = CategoryEquipToListSelectedEquip(categoryEquip);

            string timeStartShift = time.StartShiftPlanedDateTime(selectDate, selectShift);
            string timeEndShift = time.EndShiftPlanedDateTime(timeStartShift);

            List<User> usersShiftList = await shifts.LoadOrdersAsync(selectDate, selectShift, givenShiftNumber);
            
            List<int> usersCurrent = new List<int>();

            //Пока так, потом сделаю отдельную выборку оборудования с сортировкой, либо без сортирвки а в порядке загрузки
            //если бы БД адекватно хранила индексы смены, а не херила их после редактирования записи, то было бы проще привязываться к смене в fbc_brigade
            /*for (int i = 0; i < equips.Count; i++)
            {
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    for(int k = 0; k < usersShiftList[j].Shifts.Count; k++)
                    {
                        for(int l = 0; l < usersShiftList[j].Shifts[k].Orders.Count; l++)
                        {
                            if (usersShiftList[j].Shifts[k].Orders[l].IdEquip == equips[i])
                            {
                                if (!usersCurrent.Contains(usersShiftList[j].Id))
                                {
                                    usersCurrent.Add(usersShiftList[j].Id);
                                }
                            }
                        }
                    }
                }
            }*/

            for (int i = 0; i < equips.Count; i++)
            {
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    for (int k = 0; k < usersShiftList[j].Shifts.Count; k++)
                    {
                        for (int l = 0; l < usersShiftList[j].Shifts[k].Equips.Count; l++)
                        {
                            if (usersShiftList[j].Shifts[k].Equips[l] == equips[i])
                            {
                                if (!usersCurrent.Contains(usersShiftList[j].Id))
                                {
                                    usersCurrent.Add(usersShiftList[j].Id);
                                }
                            }
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

                dataGridViewOneShift.Rows[indexRow].Cells[7].Style.Font = new Font(dataGridViewOneShift.Font, FontStyle.Underline);
                dataGridViewOneShift.Rows[indexRow].Cells[8].Style.Font = new Font(dataGridViewOneShift.Font, FontStyle.Underline);
                dataGridViewOneShift.Rows[indexRow].Cells[9].Style.Font = new Font(dataGridViewOneShift.Font, FontStyle.Underline);

                float userWorkingOut = 0;
                float userDone = 0;
                int dinnerTime = 0;

                int idletime = 0;
                //int indexRowForUser = listView1.Items.Count - 1;

                //Сделать детальное отображение выполняемых заказов
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    if (usersShiftList[j].Id == usersCurrent[i])
                    {
                        UserShift userShift = usersShiftList[j].Shifts[0];

                        string startShift = userShift.ShiftDateBegin == "" ? string.Empty : Convert.ToDateTime(userShift.ShiftDateBegin).ToString("dd.MM.yyyy HH:mm");
                        string endShift = userShift.ShiftDateEnd == "" ? string.Empty : Convert.ToDateTime(userShift.ShiftDateEnd).ToString("dd.MM.yyyy HH:mm");

                        int durationShift = 0;

                        if (endShift != "")
                        {
                            durationShift = time.DateDifferenceToMinutes(endShift, startShift);
                        }
                        else
                        {
                            durationShift = time.DateDifferenceToMinutes(DateTime.Now.ToString("dd.MM.yyyy HH:mm"), startShift);
                        }

                        dataGridViewOneShift.Rows[indexRow].Cells[7].Value = startShift;
                        dataGridViewOneShift.Rows[indexRow].Cells[8].Value = endShift;
                        dataGridViewOneShift.Rows[indexRow].Cells[9].Value = time.MinuteToTimeString(durationShift);

                        int currentStep = 0;
                        int countOrder = 0;
                        int countOperation = 0;

                        float currentMakereadyWorkingOut = 0;

                        //dataGridViewOneShift.Rows[indexRow].Cells[7].Value = userShift.ShiftDateBegin;
                        //dataGridViewOneShift.Rows[indexRow].Cells[8].Value = userShift.ShiftDateEnd;

                        while (currentStep < userShift.Orders.Count)
                        {
                            countOperation++;
                            //countOrder++;

                            bool isMakeready = false;

                            List<UserShiftOrder> userShiftOrders = SelectedFullStepsForCurrentOrder(userShift.Orders.GetRange(currentStep, userShift.Orders.Count - currentStep), userShift.Orders[currentStep].IdManOrderJobItem);

                            for (int l = 0; l < userShiftOrders.Count; l++)
                            {
                                UserShiftOrder order = userShiftOrders[l];
                                ViewOrder view = new ViewOrder();

                                //может сразу при запросе считаьь времия на приладку делить время на количество приладко???
                                Normtime normtime = shifts.GetNormTimeForOrder(order.IdManOrderJobItem);

                                string orderStartTime = userShiftOrders[0].DateBegin;
                                string orderEndTime = userShiftOrders[l].DateEnd;

                                //string timeBegin = Convert.ToDateTime(order.DateBegin).ToString("dd.MM.yyyy HH:mm");
                                //string timeEnd = Convert.ToDateTime(order.DateEnd).ToString("dd.MM.yyyy HH:mm");

                                string timeBegin = order.DateBegin == "" ? string.Empty : Convert.ToDateTime(order.DateBegin).ToString("dd.MM.yyyy HH:mm");
                                string timeEnd = order.DateEnd == "" ? string.Empty : Convert.ToDateTime(order.DateEnd).ToString("dd.MM.yyyy HH:mm");

                                if (timeBegin == "")
                                {
                                    break;
                                }

                                view.TimeBegin = timeBegin + "";

                                view.WorkingOut += CalculateWorkTimeForOneOrder(order);

                                //view.Amount = order.PlanOutQty;
                                //if (order.Flags != 576)
                                if (order.OperationType == 1)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        view.Done += order.FactOutQty;
                                    }
                                        
                                    view.Amount = order.PlanOutQty;
                                    view.NormTimeWork = order.Normtime;
                                    view.IdletimeName = "работа";
                                }

                                //if (order.Flags == 576)
                                if (order.OperationType == 0)
                                {
                                    view.Amount = normtime.PlanOutQtyWork;

                                    view.DoneMakeReady += order.FactOutQty;

                                    view.MakeReady = order.PlanOutQty;
                                    view.NormTimeMakeReady = order.Normtime;
                                    view.IdletimeName = "приладка";

                                    currentMakereadyWorkingOut = view.WorkingOut;
                                }

                                if (order.IdletimeName != "")
                                {
                                    //countOrder -= 1;

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

                                float orderPreviousAmount = shifts.GetAmountDoneFromPreviousShifts(userShift.Orders[currentStep].IdManOrderJobItem, order.DateBegin, 1);
                                float lastAmount = (view.Amount - orderPreviousAmount) < 0 ? 0 : (view.Amount - orderPreviousAmount);

                                float orderPreviousMakeReady = shifts.GetAmountDoneFromPreviousShifts(userShift.Orders[currentStep].IdManOrderJobItem, order.DateBegin, 0);
                                float lastMakeReady = orderPreviousMakeReady == 0 ? 1 : (normtime.PlanOutQtyMakeReady - orderPreviousMakeReady);
                                Console.WriteLine("!!!!!!!!!!orderPreviousMakeReady " + orderPreviousMakeReady + ", lastMakeReady " + lastMakeReady + ", normtime.PlanOutQtyMakeReady " + normtime.PlanOutQtyMakeReady);
                                int normTimeFull = 0;
                                int normTimeGeneral = 0;

                                float lastTimeWork = 0;
                                float lastTimeMakeReady = (normtime.PlanNormtimeMakeReady / (normtime.PlanOutQtyMakeReady == 0 ? 1 : normtime.PlanOutQtyMakeReady)) * lastMakeReady;

                                Console.WriteLine(order.OrderNumber + ": " + order.OrderName + " | " + order.IdManOrderJobItem + " - " + normtime.PlanNormtimeMakeReady + " / " + normtime.PlanOutQtyMakeReady + " * " + lastMakeReady + " = " + lastTimeMakeReady + "\nРабота: " + view.NormTimeWork + " == " + normtime.PlanNormtimeWork + "\nПриладка: " + view.NormTimeMakeReady + " == " + normtime.PlanNormtimeMakeReady +
                                    "\nТираж: " + view.Amount + " Сделано: " + view.Done + "\nПрил: " + view.MakeReady + " Сделано: " + view.DoneMakeReady);

                                //Сделать подсчёт оставшегося времени с учетом оставшейся части приладки view.NormTimeMakeReady * lastMakeReady
                                //Что делает GetNormTimeForOrder?
                                //Возможно переделать
                                if (normtime.PlanNormtimeWork > 0)
                                {
                                    float norm = view.Amount / normtime.PlanNormtimeWork;

                                    if (norm > 0)
                                    {
                                        lastTimeWork = lastAmount / norm;
                                    }
                                }

                                if (orderPreviousAmount > 0)//заказ уже прилажен и начато выполнение
                                {
                                    normTimeGeneral = (int)lastTimeWork;
                                    normTimeFull = (int)lastTimeWork;
                                }
                                else
                                {
                                    if (isMakeready)
                                    {
                                        normTimeGeneral = (int)normtime.PlanNormtimeWork;
                                    }
                                    else
                                    {
                                        normTimeGeneral = (int)lastTimeMakeReady + (int)normtime.PlanNormtimeWork;
                                    }

                                    normTimeFull = view.NormTimeMakeReady + view.NormTimeWork;
                                }

                                ///////TEST
                                //float fullLastTime = (normtime.PlanNormtimeMakeReady / (normtime.PlanOutQtyMakeReady == 0 ? 1 : normtime.PlanOutQtyMakeReady)) * (1 - orderPreviousMakeReady) + lastTimeWork;
                                string currentOperation = "";
                                string noteOperation = order.IdletimeNote == "" ? "" : order.IdletimeNote + "; ";

                                noteOperation += order.Note == "" ? "" : order.Note + "; ";//order.IdletimeNote;

                                if (order.ProblemName != "")
                                {
                                    noteOperation += "Проблема: " + order.ProblemName + "; Причина: " + order.ProblemCause + "; Действия: " + order.ProblemAction + "; Задержка: " + time.MinuteToTimeString(order.ProblemDelay);
                                }

                                if (order.Status == 2)
                                {
                                    //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                    //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, time.DateTimeAmountMunutes(order.DateBegin, (int)Math.Round(userWorkingOut)));
                                    //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, order.DateEnd);
                                    dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, order.DateEnd);
                                    view.TimePlanedEndOrder = time.DateTimeAmountMunutes(timeStartShift, (int)Math.Round(userWorkingOut) + dinnerTime + idletime);

                                    view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                    
                                    /*timeBegin = Convert.ToDateTime(order.DateBegin).ToString("dd.MM.yyyy HH:mm");
                                    timeEnd = Convert.ToDateTime(order.DateEnd).ToString("dd.MM.yyyy HH:mm");*/

                                    view.TimeEnd = timeEnd + "";
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

                                            int dinnerForCorrentTimeBegin = AddDinnerTimeToWorkingOut(timeStartShift, DateTime.Now.ToString());
                                            int currentTimeBegin = time.DateDifferenceToMinutes(DateTime.Now.ToString(), lastTimeEndPlanedOrder) - (dinnerForCorrentTimeBegin + idletime);

                                            if (lastTimeMakeReady > 0)
                                            {
                                                if (lastTimeMakeReady > currentTimeBegin)
                                                {
                                                    currentOperation = "Выполняется приладка. Осталось: " + time.MinuteToTimeString((int)lastTimeMakeReady - currentTimeBegin) + " из " + time.MinuteToTimeString((int)lastTimeMakeReady) + "; ";
                                                }
                                                else
                                                {
                                                    currentOperation = "Приладка завершена. ";
                                                }
                                            }

                                            //Тест расчета планируемой выработки при указании части приладки больше остатка
                                            float timeMakeready = lastTimeMakeReady;

                                            if (currentMakereadyWorkingOut > lastTimeMakeReady && lastTimeMakeReady > 0)
                                            {
                                                timeMakeready = currentMakereadyWorkingOut;
                                            }

                                            //if (lastTimeMakeReady < currentTimeBegin)
                                            if (timeMakeready < currentTimeBegin)
                                            {
                                                float norm = view.Amount / normtime.PlanNormtimeWork;
                                                //float workCount = (currentTimeBegin - lastTimeMakeReady) * norm;
                                                

                                                float workCount = (currentTimeBegin - timeMakeready) * norm;

                                                currentOperation += "Выполняется работа. Сделано: " + workCount.ToString("N0") + " из остатка " + lastAmount.ToString("N0") + " шт. ";//Норма: " + (norm * 60).ToString("N0") + " шт/ч; ";// + " время текущего заказа: " + currentTimeBegin + " lastTimeMakeReady: " + lastTimeMakeReady + " lastTimeEndPlanedOrder: " + lastTimeEndPlanedOrder;

                                                float workSumm = orderPreviousAmount + workCount;

                                                if (orderPreviousAmount > 0)
                                                {
                                                    currentOperation += "Сумма: " + (orderPreviousAmount + workCount).ToString("N0") + " из тиража " + view.Amount.ToString("N0") + " шт. ";
                                                }
                                            }

                                            currentOperation += "Норма: " + (view.Amount / normtime.PlanNormtimeWork * 60).ToString("N0") + " шт/ч; ";

                                            Console.WriteLine("TEST: view.Amount / normtime.PlanNormtimeWork = (" + view.Amount + " / " + normtime.PlanNormtimeWork + ") = " + view.Amount / normtime.PlanNormtimeWork +
                                                "; workCount = (currentTimeBegin - lastTimeMakeReady) * norm = (" + currentTimeBegin + " - " + lastTimeMakeReady + ") * (" + view.Amount + " / " + normtime.PlanNormtimeWork + ") = " + (currentTimeBegin  -  lastTimeMakeReady) * (view.Amount / normtime.PlanNormtimeWork) + 
                                                 "\n---------------------------------");

                                            Console.WriteLine("TEST2: currentMakereadyWorkingOut = " + currentMakereadyWorkingOut + "; lastTimeMakeReady < currentTimeBegin " +
                                                lastTimeMakeReady + " < " + currentTimeBegin +
                                                 "\n---------------------------------");
                                        }
                                        else
                                        {
                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                            //dinnerTime += AddDinnerTimeToWorkingOut(order.DateBegin, time.DateTimeAmountMunutes(order.DateBegin, (int)Math.Round(view.WorkingOut)));
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, order.DateEnd);
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, order.DateEnd);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(view.WorkingOut) + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            view.TimeEnd = timeEnd + " ";

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

                                            //
                                            //
                                            // поправить это условие, для незавершенного заказа и незавершенной смены показывает планируемое время завершения заказа общее
                                            if (order.IdletimeName == "")
                                            {
                                                if (IsLastRecordOfOrder(userShiftOrders.GetRange(l, userShiftOrders.Count - l)))
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
                                            view.TimeEnd = timeEnd + " ";
                                        }
                                        else
                                        {
                                            //dinnerTime += AddDinnerTimeToWorkingOut(selectDate, order.DateBegin, order.DateEnd);
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, time.DateTimeAmountMunutes(orderStartTime, (int)Math.Round(view.WorkingOut)));
                                            //dinnerTime += AddDinnerTimeToWorkingOut(orderStartTime, orderEndTime);
                                            dinnerTime = AddDinnerTimeToWorkingOut(timeStartShift, orderEndTime);
                                            view.TimePlanedEndOrder = time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)Math.Round(view.WorkingOut) + dinnerTime + idletime);
                                            view.DifferentTime = time.DateDifferenceToMinutes(view.TimePlanedEndOrder, order.DateEnd);
                                            view.TimeEnd = timeEnd + " ";
                                        }
                                    }
                                }

                                //if (order.Flags == 576)
                                if (order.OperationType == 0)
                                {
                                    isMakeready = true;
                                }
                                else
                                {
                                    currentMakereadyWorkingOut = 0;
                                }

                                indexRow = dataGridViewOneShift.Rows.Add();

                                ////TEST/////
                                ///

                                string description = "";

                                if (order.IdletimeName == "")
                                {
                                    description = currentOperation;
                                    //dataGridViewOneShift.Rows[indexRow].Cells[14].Value = currentOperation;// + " -_-_-_-_- Last time: " + fullLastTime + " |||| Приладка:" + lastTimeMakeReady + " Работа: " + lastTimeWork + " Завершения: " + time.DateTimeAmountMunutes(lastTimeEndPlanedOrder, (int)lastTimeWork + (int)lastTimeMakeReady + dinnerTime);
                                }



                                dataGridViewOneShift.Rows[indexRow].Cells[14].Value = description + noteOperation;

                                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                                if (l == 0)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        countOrder++;

                                        dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (countOrder).ToString();
                                    }
                                    
                                    dataGridViewOneShift.Rows[indexRow].Cells[1].Value = "    " + machines[order.IdEquip];
                                    dataGridViewOneShift.Rows[indexRow].Cells[2].Value = order.OrderNumber;
                                    dataGridViewOneShift.Rows[indexRow].Cells[3].Value = order.OrderName;
                                }

                                dataGridViewOneShift.Rows[indexRow].Cells[4].Value = view.IdletimeName;

                                //if (order.Flags == 576 || order.IdletimeName != "")
                                //Подумать(
                                if (order.OperationType == -1 && order.IdletimeName != "")
                                {
                                    //dataGridViewOneShift.Rows[indexRow].Cells[5].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");
                                    if (order.Normtime > 0)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(order.Normtime);
                                    }
                                }
                                else if (order.OperationType == 0)
                                {
                                    if (lastMakeReady != order.PlanOutQty)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastMakeReady.ToString("P0") + " / " + order.PlanOutQty.ToString("P0");
                                    }
                                    
                                    dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString((int)lastTimeMakeReady);
                                }
                                else if (order.OperationType == 1)
                                {
                                    if (lastAmount != view.Amount)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastAmount.ToString("N0") + " / " + view.Amount.ToString("N0");
                                    }
                                    else
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[5].Value = view.Amount.ToString("N0");
                                    }

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

                                if (view.DoneMakeReady > 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.DoneMakeReady.ToString("P0");
                                }

                                if (view.Done > 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.Done.ToString("N0");
                                }

                                //тут еще ничего не работает
                                //dataGridViewOneShift.Rows[indexRow].Cells[4].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");

                                dataGridViewOneShift.Rows[indexRow].Cells[7].Value = view.TimeBegin;
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
                dataGridViewOneShift.Rows[indexRow].Cells[14].Value = (userWorkingOut / normTime).ToString("P1");
            }
        }

        private async Task LoadOrdersSelectedDateAndShiftDetailsOLDAsync(DateTime selectDate, int selectShift)
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

            List<User> usersShiftList = await shifts.LoadOrdersAsync(selectDate, selectShift, givenShiftNumber);

            List<int> usersCurrent = new List<int>();

            //Пока так, потом сделаю отдельную выборку оборудования с сортировкой, либо без сортирвки а в порядке загрузки
            //если бы БД адекватно хранила индексы смены, а не херила их после редактирования записи, то было бы проще привязываться к смене в fbc_brigade
            for (int i = 0; i < equips.Count; i++)
            {
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    for (int k = 0; k < usersShiftList[j].Shifts.Count; k++)
                    {
                        for (int l = 0; l < usersShiftList[j].Shifts[k].Orders.Count; l++)
                        {
                            if (usersShiftList[j].Shifts[k].Orders[l].IdEquip == equips[i])
                            {
                                if (!usersCurrent.Contains(usersShiftList[j].Id))
                                {
                                    usersCurrent.Add(usersShiftList[j].Id);
                                }
                            }
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
                float userDone = 0;
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

                        //dataGridViewOneShift.Rows[indexRow].Cells[7].Value = userShift.ShiftDateBegin;
                        //dataGridViewOneShift.Rows[indexRow].Cells[8].Value = userShift.ShiftDateEnd;

                        while (currentStep < userShift.Orders.Count)
                        {
                            countOperation++;
                            //countOrder++;

                            bool isMakeready = false;

                            List<UserShiftOrder> userShiftOrders = SelectedFullStepsForCurrentOrder(userShift.Orders.GetRange(currentStep, userShift.Orders.Count - currentStep), userShift.Orders[currentStep].IdManOrderJobItem);

                            for (int l = 0; l < userShiftOrders.Count; l++)
                            {
                                UserShiftOrder order = userShiftOrders[l];
                                ViewOrder view = new ViewOrder();

                                string orderStartTime = userShiftOrders[0].DateBegin;
                                string orderEndTime = userShiftOrders[l].DateEnd;

                                view.WorkingOut += CalculateWorkTimeForOneOrder(order);

                                //if (order.Flags != 576)
                                if (order.OperationType == 1)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        view.Done += order.FactOutQty;
                                    }

                                    view.Amount = order.PlanOutQty;
                                    view.NormTimeWork = order.Normtime;
                                    view.IdletimeName = "работа";
                                }

                                //if (order.Flags == 576)
                                if (order.OperationType == 0)
                                {
                                    view.DoneMakeReady += order.FactOutQty;

                                    view.MakeReady = order.PlanOutQty;
                                    view.NormTimeMakeReady = order.Normtime;
                                    view.IdletimeName = "приладка";
                                }

                                if (order.IdletimeName != "")
                                {
                                    //countOrder -= 1;

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

                                float orderPreviousAmount = shifts.GetAmountDoneFromPreviousShifts(userShift.Orders[currentStep].IdManOrderJobItem, order.DateBegin, 1);
                                float lastAmount = view.Amount - orderPreviousAmount;

                                float orderPreviousMakeReady = shifts.GetAmountDoneFromPreviousShifts(userShift.Orders[currentStep].IdManOrderJobItem, order.DateBegin, 0);
                                float lastMakeReady = view.MakeReady - orderPreviousMakeReady;

                                int[] normtime = shifts.GetNormTimeForOrder(order.IdManOrderJobItem, 11111111);
                                int normTimeFull = 0;
                                int normTimeGeneral = 0;

                                float lastTime = 0;


                                //Сделать подсчёт оставшегося времени с учетом оставшейся части приладки view.NormTimeMakeReady * lastMakeReady
                                //Что делает GetNormTimeForOrder?
                                //Возможно переделать
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

                                            //
                                            //
                                            // поправить это условие, для незавершенного заказа и незавершенной смены показывает планируемое время завершения заказа общее
                                            if (order.IdletimeName == "")
                                            {
                                                if (IsLastRecordOfOrder(userShiftOrders.GetRange(l, userShiftOrders.Count - l)))
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

                                //if (order.Flags == 576)
                                if (order.OperationType == 0)
                                {
                                    isMakeready = true;
                                }

                                indexRow = dataGridViewOneShift.Rows.Add();

                                dataGridViewOneShift.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;

                                if (l == 0)
                                {
                                    if (order.IdletimeName == "")
                                    {
                                        countOrder++;

                                        dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (countOrder).ToString();
                                    }

                                    dataGridViewOneShift.Rows[indexRow].Cells[1].Value = "    " + machines[order.IdEquip];
                                    dataGridViewOneShift.Rows[indexRow].Cells[2].Value = order.OrderNumber;
                                    dataGridViewOneShift.Rows[indexRow].Cells[3].Value = order.OrderName;
                                }

                                dataGridViewOneShift.Rows[indexRow].Cells[4].Value = view.IdletimeName;

                                //if (order.Flags == 576 || order.IdletimeName != "")
                                //Подумать(
                                if (order.OperationType == -1 && order.IdletimeName != "")
                                {
                                    //dataGridViewOneShift.Rows[indexRow].Cells[5].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");
                                    if (order.Normtime > 0)
                                    {
                                        dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(order.Normtime);
                                    }
                                }
                                else if (order.OperationType == 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastMakeReady.ToString("P0") + " | " + order.PlanOutQty.ToString("P0");
                                    dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(order.Normtime);
                                }
                                else if (order.OperationType == 1)
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

                                if (view.DoneMakeReady > 0)
                                {
                                    dataGridViewOneShift.Rows[indexRow].Cells[12].Value = view.DoneMakeReady.ToString("P0");
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

        private async Task LoadOrdersSelectedDateAndShiftCompactAsync(DateTime selectDate, int selectShift)
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

            List<User> usersShiftList = await shifts.LoadOrdersAsync(selectDate, selectShift, givenShiftNumber);

            List<int> usersCurrent = new List<int>();

            //Пока так, потом сделаю отдельную выборку оборудования с сортировкой, либо без сортирвки а в порядке загрузки
            //если бы БД адекватно хранила индексы смены, а не херила их после редактирования записи, то было бы проще привязываться к смене в fbc_brigade
            for (int i = 0; i < equips.Count; i++)
            {
                for (int j = 0; j < usersShiftList.Count; j++)
                {
                    for (int k = 0; k < usersShiftList[j].Shifts.Count; k++)
                    {
                        for (int l = 0; l < usersShiftList[j].Shifts[k].Orders.Count; l++)
                        {
                            if (usersShiftList[j].Shifts[k].Orders[l].IdEquip == equips[i])
                            {
                                if (!usersCurrent.Contains(usersShiftList[j].Id))
                                {
                                    usersCurrent.Add(usersShiftList[j].Id);
                                }
                            }
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
                float userDone = 0;
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

                            float orderPreviousAmount = shifts.GetAmountDoneFromPreviousShifts(ordersIdManOrderJobItem[k], order.DateBegin, 1);

                            string firstTimeBegin = user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateBegin;
                            string lastTimeBegin = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateBegin;
                            string firstTimeEnd = user.Shifts[0].Orders[indexesUserShiftsOrders[0]].DateEnd;
                            string lastTimeEnd = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd;

                            //dinnerTime += AddDinnerTimeToWorkingOut(firstTimeBegin, lastTimeEnd);

                            string operation = "";

                            float workingOut = 0;
                            float done = 0;
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
                                    amount = (int)orderCur.PlanOutQty;
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

                            float lastAmount = amount - orderPreviousAmount;
                            //MessageBox.Show(lastAmount + " = " + order.PlanOutQty + " - " + orderPreviousAmount);

                            userWorkingOut += workingOut;
                            userDone += done;

                            int[] normtime = shifts.GetNormTimeForOrder(order.IdManOrderJobItem, 111111);
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
                            dataGridViewOneShift.Rows[indexRow].Cells[1].Value = "    " + machines[order.IdEquip];
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

        private async void dateTimePicker1_ValueChangedAsync(object sender, EventArgs e)
        {
            await ReloadOrdersSelectedDateAsync();
        }
        private async void comboBox1_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            await ReloadOrdersSelectedDateAsync();
        }

        private async Task ReloadOrdersSelectedDateAsync()
        {
            DateTime selectDate = dateTimePicker1.Value;
            int selectShift = comboBox1.SelectedIndex + 1;

            if (!loadParameter)
            {
                await LoadOrdersSelectedDateAndShiftAsync(selectDate, selectShift);
            }
        }
        private async void FormMain_LoadAsync(object sender, EventArgs e)
        {
            StartTaskUpdateApplication();
            await LoadStartsValuesAsync();
        }

        private async void comboBox2_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            await ChangeDateAsync();
        }

        private async void comboBox3_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            await ChangeDateAsync();
        }

        private async void comboBox4_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            await ChangeDateAsync();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            await ChangeDateAsync();
        }

        private async void metroSetTabControl1_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            SaveParameterBeforeClosing();

            await TabControlSelectedIndexChangedAsync(metroSetTabControl1.SelectedIndex);

            metroSetTabControlPreviousIndex = metroSetTabControl1.SelectedIndex;
            comboBoxSelectViewsPreviousIndex = -1;
        }

        private void SaveParameterBeforeClosing()
        {
            if (metroSetTabControlPreviousIndex == PageSettings)
            {
                SaveCategoryToIniFile();
                SaveParameterToIniFile();

                LoadCountShiftsToComboBox();

                listViewEquips.Items.Clear();
            }

            if (metroSetTabControlPreviousIndex == PageParametersMonitor)
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
                            //if (orderSearch[j].Flags != 576)
                            if (orderSearch[j].OperationType == 1)
                            {
                                operation = "работа";

                                fullFactOutQTY += orderSearch[j].FactOutQTY;
                            }

                            //if (orderSearch[j].Flags == 576)
                            if (orderSearch[j].OperationType == 0)
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
                            //if (orderSearch[j].Flags != 576)
                            if (orderSearch[j].OperationType == 1)
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

        private async Task TabControlSelectedIndexChangedAsync(int selectedIndex)
        {
            INISettings settings = new INISettings();

            cancelTokenSource?.Cancel();

            if (selectedIndex == PageShiftDetails)
            {
                loadParameter = true;
                comboBox1.SelectedIndex = 0;

                metroSetSwitch1.Switched = settings.GetLoadCurrentShift();
                metroSetSwitch2.Switched = settings.GetAutoUpdateStatistic();
                formattedNumericUpDown4.Value = settings.GetPeriodAutoUpdateStatistic();
                loadParameter = false;

                await ReloadOrdersSelectedDateAsync();
            }

            if (selectedIndex == PageMonthDetails)
            {
                //UpdateStatistics();

                await ChangeDateAsync();
            }

            if (selectedIndex == PageStatistic)
            {
                //UpdateStatistics();
                //ChangeIncomingValuesForCategoryStatistic();
                SelectCategoryStatistic();
            }

            if (selectedIndex == PageStatisticYear)
            {
                //UpdateStatistics();
                //ChangeIncomingValuesForCategoryStatistic();
                SelectCategoryStatisticYear();
            }
            
            if (selectedIndex == PageStatisticUser)
            {
                SelectUsersStatistic();
            }

            if (selectedIndex == PagePlanOrders)
            {
                SelectPlanOrders();
            }

            if (selectedIndex == PageSettings)
            {
                LoadCategoryToListView();
                LoadParameterFromIniFile();
            }

            if (selectedIndex == PageParametersMonitor)
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
                buttonPreview.Enabled = false;
                buttonNext.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = true;
                comboBox1.Enabled = true;
                buttonPreview.Enabled = true;
                buttonNext.Enabled = true;
            }

            ChangeStateTimer();

            settings.SetLoadCurrentShift(metroSetSwitch1.Switched);
        }

        private async Task LoadOrdersForSelectedDateAsync()
        {
            if (metroSetSwitch1.Switched)
            {
                SelectCurrentShift();
            }

            DateTime selectDate = dateTimePicker1.Value;
            int selectShift = comboBox1.SelectedIndex + 1;

            await LoadOrdersSelectedDateAndShiftAsync(selectDate, selectShift);

            lastTimeUpdateShiftStatistic = DateTime.Now;
        }

        private async Task metroSetButton1_ClickAsync(object sender, EventArgs e)
        {
            await LoadOrdersForSelectedDateAsync();
        }

        private async void timer1_TickAsync(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;

            if (metroSetSwitch2.Switched)
            {
                int updatePeriod = (int)formattedNumericUpDown4.Value;
                int lostMin = lastTimeUpdateShiftStatistic.AddMinutes(updatePeriod).Subtract(currentDateTime).Minutes + 1;

                button2.Text = "Обновление (" + lostMin + ")";

                if (lastTimeUpdateShiftStatistic.AddMinutes(updatePeriod) <= currentDateTime)
                {
                    await LoadOrdersForSelectedDateAsync();
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

        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            await LoadOrdersForSelectedDateAsync();
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

        private void LaodPagesToListView(List<PageView> pages)
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

        private async Task NextShiftAsync()
        {
            loadParameter = true;

            if (comboBox1.SelectedIndex == 0)
            {
                comboBox1.SelectedIndex = 1;
            }
            else
            {
                dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
                comboBox1.SelectedIndex = 0;
            }

            loadParameter = false;

            await LoadOrdersForSelectedDateAsync();
        }

        private async Task PreviewShiftAsync()
        {
            loadParameter = true;

            if (comboBox1.SelectedIndex == 1)
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
                comboBox1.SelectedIndex = 1;
            }
            loadParameter = false;

            await LoadOrdersForSelectedDateAsync();
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

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            PreviewShiftAsync();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextShiftAsync();
        }

        private string SelectValueFromDictionary(Dictionary<int, string> dictionary, int index)
        {
            string value = "";

            int count = 0;

            if (dictionary.ContainsKey(index))
            {
                value = dictionary[index];
            }
            else
            {
                if (count <= 10)
                {
                    count++;

                    LoadAllUsers();
                    LoadMachine();

                    SelectValueFromDictionary(dictionary, index);
                }
                else
                {
                    value = dictionary.ToString() + " " + index;
                }

            }

            return value;
        }

        private void ClearAll()
        {
            ListViewWorkingOutCategory.Items.Clear();
            labelCategoryStatisticValue.Text = "";
            chart1.Series.Clear();
        }

        private void DrawDiagram(List<float> yValues, List<string> xValues, bool hourValue)
        {
            chart1.Series.Clear();
            // Форматировать диаграмму
            //chart1.BackColor = Color.Gray;
            //chart1.BackSecondaryColor = Color.WhiteSmoke;
            chart1.BackGradientStyle = GradientStyle.DiagonalRight;

            chart1.BorderlineDashStyle = ChartDashStyle.Solid;
            //chart1.BorderlineColor = Color.Gray;
            chart1.BorderSkin.SkinStyle = BorderSkinStyle.None;

            // Форматировать область диаграммы
            chart1.ChartAreas[0].BackColor = Color.Transparent;

            // Добавить и форматировать заголовок
            /*chart1.Titles.Add("Диаграммы");
            chart1.Titles[0].Font = new Font("Utopia", 16);*/

            chart1.Series.Add(new Series("ColumnSeries")
            {
                ChartType = SeriesChartType.Column,
                //ChartType = SeriesChartType.Pie,
                LabelBackColor = Color.Transparent,
                IsVisibleInLegend = false

            });

            chart1.Series["ColumnSeries"].Points.DataBindXY(xValues, yValues);

            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -30;
            //chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //chart1.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            chart1.ChartAreas[0].AxisX.Interval = 1;

            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{N0}";

            chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = !hourValue;

            //chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
            chart1.AlignDataPointsByAxisLabel();
        }

        private void DrawDiagram(Chart chart, List<float> yValues, List<string> xValues, string diagramName = null)
        {
            chart.Series.Clear();
            // Форматировать диаграмму
            //chart.BackColor = Color.Gray;
            //chart.BackSecondaryColor = Color.WhiteSmoke;
            chart.BackGradientStyle = GradientStyle.DiagonalRight;

            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            //chart.BorderlineColor = Color.Gray;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;

            // Форматировать область диаграммы
            chart.ChartAreas[0].BackColor = Color.Transparent;

            // Добавить и форматировать заголовок
            if (diagramName != null)
            {
                chart.Titles.Add(diagramName);
                chart.Titles[0].Font = new Font("Consolas", 12);
            }
            
            chart.Series.Add(new Series("ColumnSeries")
            {
                ChartType = SeriesChartType.Column,
                //ChartType = SeriesChartType.Pie,
                LabelBackColor = Color.Transparent,
                IsVisibleInLegend = false

            });

            chart.Series["ColumnSeries"].Points.DataBindXY(xValues, yValues);

            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -30;
            //chart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //chart.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "{N0}";

            //chart.ChartAreas[0].Area3DStyle.Enable3D = true;
            chart.AlignDataPointsByAxisLabel();
        }

        private void AddUsersASToListView(CancellationToken token, List<int> usersList)
        {
            ValueUsers valueUsers = new ValueUsers();

            for (int i = 0; i < usersList.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                ListViewItem item = new ListViewItem();

                item.Name = usersList[i].ToString();
                item.Text = (ListViewWorkingOutCategory.Items.Count + 1).ToString();
                item.SubItems.Add(valueUsers.GetUserNameFromID(usersList[i]));
                item.SubItems.Add("");
                item.SubItems.Add("");

                Invoke(new Action(() =>
                {
                    ListViewWorkingOutCategory.Items.Add(item);
                }));
            }
        }

        private void AddUserWorkingOutputValues(CancellationToken token, int typeValueLoad)
        {
            ValueDateTime time = new ValueDateTime();

            List<UserWorkingOutput> userOutputs = userWorkingOutputs;
            List<string> names = new List<string>();
            List<float> values = new List<float>();
            float summWorkingOut = 0;

            Invoke(new Action(() =>
            {
                ClearAll();
            }));

            switch (typeValueLoad)
            {
                case 0:
                    userOutputs.Sort((b1, b2) => b2.Amount.CompareTo(b1.Amount));
                    labelCategoryStatisticCaption.Text = "Всего сделано продукции:";
                    break;
                case 1:
                    userOutputs.Sort((b1, b2) => b2.Percent.CompareTo(b1.Percent));
                    labelCategoryStatisticCaption.Text = "Средняя выработка:";
                    break;
                case 2:
                    userOutputs.Sort((b1, b2) => b2.Makeready.CompareTo(b1.Makeready));
                    labelCategoryStatisticCaption.Text = "Всего сделано приладок:";
                    break;
                case 3:
                    userOutputs.Sort((b1, b2) => b2.MakereadyTime.CompareTo(b1.MakereadyTime));
                    labelCategoryStatisticCaption.Text = "Сумма времени приладок:";
                    break;
                default:
                    break;
            }

            for (int i = 0; i < userOutputs.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                float wOutValue = 0;

                switch (typeValueLoad)
                {
                    case 0:
                        wOutValue = userOutputs[i].Amount;
                        values.Add(wOutValue);
                        break;
                    case 1:
                        wOutValue = userOutputs[i].Percent;
                        values.Add(wOutValue * 100);
                        break;
                    case 2:
                        wOutValue = userOutputs[i].Makeready;
                        values.Add(wOutValue);
                        break;
                    case 3:
                        wOutValue = userOutputs[i].MakereadyTime;
                        values.Add(wOutValue);
                        break;
                    default:
                        break;
                }

                ListViewItem item = new ListViewItem();

                int inrdexCurrentItem = ListViewWorkingOutCategory.Items.Count;

                item.Name = userOutputs[i].UserID.ToString();
                item.Text = (inrdexCurrentItem + 1).ToString();
                item.SubItems.Add(userOutputs[i].UserName);
                item.SubItems.Add(userOutputs[i].CountShifts.ToString());

                if (typeValueLoad == 1)
                {
                    item.SubItems.Add(wOutValue.ToString("P1"));
                }
                else if (typeValueLoad == 3)
                {
                    item.SubItems.Add(time.MinuteToTimeString((int)wOutValue));
                }
                else
                {
                    item.SubItems.Add(wOutValue.ToString("N0"));
                }
                
                item.SubItems.Add("");
                item.SubItems.Add(userOutputs[i].Bonus.ToString("P0"));

                Invoke(new Action(() =>
                {
                    ListViewWorkingOutCategory.Items.Add(item);
                }));

                summWorkingOut += wOutValue;

                string add = "";

                if (names.Contains(userOutputs[i].UserName))
                {
                    add += " ";
                }

                names.Add(userOutputs[i].UserName + add);
            }

            for (int i = 0; i < userOutputs.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                float wOutValue = 0;

                switch (typeValueLoad)
                {
                    case 0:
                        wOutValue = userOutputs[i].Amount;
                        break;
                    case 1:
                        wOutValue = userOutputs[i].Percent;
                        break;
                    case 2:
                        wOutValue = userOutputs[i].Makeready;
                        break;
                    case 3:
                        wOutValue = userOutputs[i].MakereadyTime;
                        break;
                    default:
                        break;
                }

                Invoke(new Action(() =>
                {
                    int index = ListViewWorkingOutCategory.Items.IndexOfKey(userOutputs[i].UserID.ToString());

                    if (index >= 0)
                    {
                        ListViewItem item = ListViewWorkingOutCategory.Items[index];

                        if (item != null)
                        {
                            item.SubItems[4].Text = ((float)wOutValue / summWorkingOut).ToString("P2");
                        }
                    }
                }));
            }

            if (!token.IsCancellationRequested)
            {
                bool hourValue = false;

                switch (typeValueLoad)
                {
                    case 0:
                        labelCategoryStatisticValue.Text = summWorkingOut.ToString("N0");
                        hourValue = false;
                        break;
                    case 1:
                        labelCategoryStatisticValue.Text = (summWorkingOut / userOutputs.Count).ToString("P1");
                        hourValue = false;
                        break;
                    case 2:
                        labelCategoryStatisticValue.Text = summWorkingOut.ToString("N0");
                        hourValue = false;
                        break;
                    case 3:
                        labelCategoryStatisticValue.Text = time.MinuteToTimeString((int)summWorkingOut);
                        hourValue = true;
                        break;
                    default:
                        break;
                }

                Invoke(new Action(() =>
                {
                    DrawDiagram(values, names, hourValue);

                    //label2.Text = summWorkingOut.ToString("N0");
                }));
            }
        }

        private Task<List<int>> LoadUserListFromMonthAS(CancellationToken token, List<int> equips, DateTime date, bool includeASuserID = false)
        {
            ValueUsers valueUsers = new ValueUsers();

            List<int> usersListAS = valueUsers.LoadUsersListOnlyIDFromSelectMonth(equips, date);

            return Task.FromResult(usersListAS);
        }

        private void StartLoadingStatisticForCategory()
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            DateTime date;
            date = DateTime.MinValue.AddYears(Convert.ToInt32(comboBoxStatYear.Text) - 1).AddMonths(comboBoxStatMonth.SelectedIndex);

            int typeValueLoad = comboBoxStatType.SelectedIndex;

            //Task task = new Task(() => LoadUsersFromBase(token, date));
            Task task = new Task(() => LoadUsersFromBase(cancelTokenSource.Token, date), cancelTokenSource.Token);
            //LoadUsersFromBase(cancelTokenSource.Token, date);

            task.Start();
        }

        private async void LoadUsersFromBase(CancellationToken token, DateTime date)
        {
            CalculateWorkingOutput workingOutSum = new CalculateWorkingOutput();
            ValueUsers valueUsers = new ValueUsers();

            userWorkingOutputs?.Clear();

            //List<string> usersNames = new List<string>();
            //List<float> workingOut = new List<float>();
            int category = -1;
            int categoryComboBoxSelectedIndex = -1;

            Invoke(new Action(() =>
            {
                categoryComboBoxSelectedIndex = comboBoxStatCategory.SelectedIndex;
            }));

            if (categoryComboBoxSelectedIndex < 1)
            {
                Invoke(new Action(() =>
                {
                    ClearAll();
                }));
                
                return;
            }

            Invoke(new Action(() =>
            {
                loadCategoryStatistic = true;
                button4.Text = "Отмена";

                comboBoxStatYear.Enabled = false;
                comboBoxStatMonth.Enabled = false;
                comboBoxStatCategory.Enabled = false;
                comboBoxStatType.Enabled = false;
            }));

            category = categoryComboBoxSelectedIndex - 1;

            List<int> equipsListForCategory = new List<int>();
            List <Equip> equips = categoriesList[category].Equips;

            foreach (Equip equip in equips)
            {
                if (equip.Selected)
                {
                    equipsListForCategory.Add(equip.Id);
                }
            }

            List<int> usersList = await LoadUserListFromMonthAS(token, equipsListForCategory, date, true);

            Invoke(new Action(() =>
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = usersList.Count;
                progressBar1.Value = 0;
            }));

            for (int i = 0; i < usersList.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                string name = SelectValueFromDictionary(usersShort, usersList[i]);

                UserWorkingOutput userWorkingOutput = workingOutSum.FullWorkingOutput(usersList[i], date, token, equipsListForCategory);

                userWorkingOutputs.Add(new UserWorkingOutput(
                    usersList[i],
                    name,
                    userWorkingOutput.Amount,
                    userWorkingOutput.Worktime,
                    userWorkingOutput.Percent,
                    userWorkingOutput.Makeready,
                    userWorkingOutput.MakereadyTime,
                    userWorkingOutput.Bonus,
                    userWorkingOutput.CountShifts
                    ));

                Invoke(new Action(() =>
                {
                    progressBar1.Value++;
                }));
            }

            Invoke(new Action(() =>
            {
                loadCategoryStatistic = false;
                button4.Text = "Обновить";

                comboBoxStatYear.Enabled = true;
                comboBoxStatMonth.Enabled = true;
                comboBoxStatCategory.Enabled = true;
                comboBoxStatType.Enabled = true;

                progressBar1.Value = 0;

                if (comboBoxStatType.SelectedIndex == -1)
                {
                    comboBoxStatType.SelectedIndex = 0;
                }
                else
                {
                    ChangeTypeWorkingOutput();
                }
            }));
        }
        private void ChangeIncomingValuesForCategoryStatistic()
        {
            if (!loadShiftsParameter)
            {
                if (comboBoxStatYear.SelectedIndex != -1 && comboBoxStatMonth.SelectedIndex != -1 && comboBoxStatCategory.SelectedIndex != -1)
                {
                    StartLoadingStatisticForCategory();
                }
            }
        }

        private void ChangeTypeWorkingOutput()
        {
            /*cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();*/

            AddUserWorkingOutputValues(cancelTokenSource.Token, comboBoxStatType.SelectedIndex);
        }
        private void SelectCategoryStatistic()
        {
            LoadCategoryList(comboBoxStatCategory, new List<string> { "<выберите участок>" });
        }




        //Итоги участка за год

        private void ClearAllForYearStatistic()
        {
            dataGridViewYearStatistic.DataSource = null;
            //dataGridViewYearStatistic.Invalidate();
            dataGridViewYearStatistic.Refresh();

            dataGridViewYearStatistic.Rows.Clear();
            dataGridViewYearStatistic.Columns.Clear();

            dataGridViewYearStatistic.Refresh();

            labelYearStatisticCaption.Text = "";
            chartStatYear.Series.Clear();
        }
        private void ChangeTypeWorkingOutputYearStatistic()
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            int statYearTypeViewSelectedIndex = comboBoxStatYearTypeViewSelect.SelectedIndex;

            Task task = new Task(() => AddUserWorkingOutputValuesForYearStatistic(cancelTokenSource.Token, statYearTypeViewSelectedIndex), cancelTokenSource.Token);
            //AddUserWorkingOutputValuesForYearStatistic(cancelTokenSource.Token, statYearTypeViewSelectedIndex);

            task.Start();
        }
        private void ChangeIncomingValuesForCategoryYearStatistic()
        {
            if (!loadShiftsParameter)
            {
                if (comboBoxStatYearCategorySelect.SelectedIndex != -1 && comboBoxStatYearCategorySelect.SelectedIndex != -1 && comboBoxStatYearEquipSelect.SelectedIndex > 0)
                {
                    StartLoadingStatisticYearForCategory();
                }
                else
                {
                    ClearAllForYearStatistic();
                }
            }
        }
        private void SelectCategoryStatisticYear()
        {
            LoadCategoryList(comboBoxStatYearCategorySelect, new List<string> { "<выберите участок>" });
        }

        private Task<List<int>> LoadUserListFromYear(CancellationToken token, List<int> equips, DateTime date)
        {
            ValueUsers valueUsers = new ValueUsers();

            List<int> usersListAS = valueUsers.LoadUsersListOnlyIDFromSelectYear(equips, date);

            return Task.FromResult(usersListAS);
        }

        private void StartLoadingStatisticYearForCategory()
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            DateTime date;
            date = DateTime.MinValue.AddYears(Convert.ToInt32(comboBoxStatYearSelectYear.Text) - 1);

            int typeValueLoad = comboBoxStatYearTypeViewSelect.SelectedIndex;

            ClearAllForYearStatistic();

            //Task task = new Task(() => LoadUsersFromBase(token, date));
            Task task = new Task(() => LoadUsersFromBaseForYearStatistic(cancelTokenSource.Token, date), cancelTokenSource.Token);
            //LoadUsersFromBaseForYearStatistic(cancelTokenSource.Token, date);

            task.Start();
        }

        private async void LoadUsersFromBaseForYearStatistic(CancellationToken token, DateTime date)
        {
            CalculateWorkingOutput workingOutSum = new CalculateWorkingOutput();
            ValueUsers valueUsers = new ValueUsers();

            //List<string> usersNames = new List<string>();
            //List<float> workingOut = new List<float>();
            int categoryComboBoxSelectedIndex = -1;
            int equipComboBoxSelectedindex = -1;

            Invoke(new Action(() =>
            {
                categoryComboBoxSelectedIndex = comboBoxStatYearCategorySelect.SelectedIndex;
                equipComboBoxSelectedindex = comboBoxStatYearEquipSelect.SelectedIndex;
            }));

            if (categoryComboBoxSelectedIndex < 1 || equipComboBoxSelectedindex < 1)
            {
                Invoke(new Action(() =>
                {
                    //ClearAllForYearStatistic();
                }));

                return;
            }

            Invoke(new Action(() =>
            {
                loadCategoryStatistic = true;
                buttonStatYearUpdate.Text = "Отмена";

                buttonExcelExportyearStatistic.Enabled = false;
                comboBoxStatYearSelectYear.Enabled = false;
                comboBoxStatYearCategorySelect.Enabled = false;
                comboBoxStatYearEquipSelect.Enabled = false;
                comboBoxStatYearTypeViewSelect.Enabled = false;
            }));

            userWorkingOutputsForYear?.Clear();

            //viewYearStatistics?.Clear();
            viewYearStatistics = new List<ViewYearStatistic>();

            List<int> equipsListForCategory = new List<int>();
            
            if (equipComboBoxSelectedindex > 1)
            {
                equipsListForCategory.Add(equipListForSelectedCategory[equipComboBoxSelectedindex - 2].Id);
            }
            else
            {
                List<Equip> equips = categoriesList[categoryComboBoxSelectedIndex - 1].Equips;

                foreach (Equip equip in equips)
                {
                    if (equip.Selected)
                    {
                        equipsListForCategory.Add(equip.Id);
                    }
                }
            }

            List<int> usersList = await LoadUserListFromYear(token, equipsListForCategory, date);

            Invoke(new Action(() =>
            {
                progressBarStatYear.Minimum = 0;
                progressBarStatYear.Maximum = usersList.Count * 12;
                progressBarStatYear.Value = 0;
            }));

            for (int m  = 0; m < 12;  m++)
            {
                DateTime currentDate = date.AddMonths(m);

                if (currentDate > DateTime.Now)
                {
                    break;
                }

                userWorkingOutputsForYear.Add(new List<UserWorkingOutput>());

                for (int i = 0; i < usersList.Count; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    string name = SelectValueFromDictionary(usersShort, usersList[i]);

                    UserWorkingOutput userWorkingOutput = workingOutSum.FullWorkingOutput(usersList[i], currentDate, token, equipsListForCategory);

                    userWorkingOutputsForYear[m].Add(new UserWorkingOutput(
                        usersList[i],
                        name,
                        userWorkingOutput.Amount,
                        userWorkingOutput.Worktime,
                        userWorkingOutput.Percent,
                        userWorkingOutput.Makeready,
                        userWorkingOutput.MakereadyTime,
                        userWorkingOutput.Bonus,
                        userWorkingOutput.CountShifts
                        ));

                    if (viewYearStatistics.FindIndex(x => x.Id == usersList[i]) == -1)
                    {
                        viewYearStatistics.Add(new ViewYearStatistic(usersList[i], name));
                    }

                    viewYearStatistics.Sort((b1, b2) => b1.Name.CompareTo(b2.Name));

                    Invoke(new Action(() =>
                    {
                        progressBarStatYear.Value++;
                    }));
                }
            }

            //Sort


            viewYearStatistics.Add(new ViewYearStatistic(0, "ИТОГ:"));

            Invoke(new Action(() =>
            {
                loadCategoryStatistic = false;
                buttonStatYearUpdate.Text = "Обновить";

                buttonExcelExportyearStatistic.Enabled = true;
                comboBoxStatYearSelectYear.Enabled = true;
                comboBoxStatYearCategorySelect.Enabled = true;
                comboBoxStatYearEquipSelect.Enabled = true;
                comboBoxStatYearTypeViewSelect.Enabled = true;

                progressBarStatYear.Value = 0;

                if (comboBoxStatYearTypeViewSelect.SelectedIndex == -1)
                {
                    comboBoxStatYearTypeViewSelect.SelectedIndex = 0;
                }
                else
                {
                    ChangeTypeWorkingOutputYearStatistic();
                }
            }));
        }

        private List<ViewYearStatistic> NewListAndAddHeader()
        {
            List<ViewYearStatistic> result = new List<ViewYearStatistic>();

            result.Add(new ViewYearStatistic(0, "ФИО"));

            int index = result.Count - 1;

            result[index].M01 = "Январь";
            result[index].M02 = "Февраль";
            result[index].M03 = "Март";
            result[index].M04 = "Апрель";
            result[index].M05 = "Май";
            result[index].M06 = "Июнь";
            result[index].M07 = "Июль";
            result[index].M08 = "Август";
            result[index].M09 = "Сентябрь";
            result[index].M10 = "Октябрь";
            result[index].M11 = "Ноябрь";
            result[index].M12 = "Декабрь";
            result[index].TotallOutput = "Сумма";

            return result;
        }

        private void AddUserWorkingOutputValuesForYearStatistic(CancellationToken token, int typeValueLoad)
        {
            ValueDateTime time = new ValueDateTime();

            List<List<UserWorkingOutput>> userOutputs = userWorkingOutputsForYear;
            List<string> names = new List<string>();
            List<float> values = new List<float>();
            List<float> countWorkingMonthsForCurrentYear = new List<float>();
            
            int countWorkingUsersForCurrentYear = 0;

            int lastCalculatingMonth = 0;

            float summWorkingOut = 0;
            float summWOPercent = 0;
            object summWorkingOutResult = null;

            int summIndex = viewYearStatistics.Count - 1;

            Invoke(new Action(() =>
            {
                ClearAllForYearStatistic();

                switch (typeValueLoad)
                {
                    case 0:
                        labelYearStatisticCaption.Text = "Всего сделано продукции:";
                        break;
                    case 1:
                        labelYearStatisticCaption.Text = "Средняя выработка:";
                        break;
                    case 2:
                        labelYearStatisticCaption.Text = "Всего сделано приладок:";
                        break;
                    case 3:
                        labelYearStatisticCaption.Text = "Сумма времени приладок:";
                        break;
                    default:
                        break;
                }

            }));

            /*for (int m = 0; m < userOutputs.Count; m++)
            {
                viewYearStatistics[m].TotallOutput = "";
            }*/

            for (int m = 0; m < userOutputs.Count; m++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                lastCalculatingMonth++;

                int countWorkingUsersForCurrentMonth = 0;

                float totalOutputMonth = 0;
                object totalOutputMonthResult = null;

                List<UserWorkingOutput> output = userOutputs[m];

                for (int j = 0; j < output.Count; j++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    if (m == 0)
                    {
                        values.Add(0);
                        countWorkingMonthsForCurrentYear.Add(0);

                        string add = "";

                        if (names.Contains(output[j].UserName))
                        {
                            add += " ";
                        }

                        names.Add(output[j].UserName + add);
                    }

                    float wOutValue = 0;
                    object wOutResult = null;

                    int index = viewYearStatistics.FindIndex(x => x.Id == output[j].UserID);

                    if (index != -1)
                    {
                        if (output[j].CountShifts > 0 && output[j].Percent > 0)
                        {
                            countWorkingUsersForCurrentMonth++;
                        }

                        switch (typeValueLoad)
                        {
                            case 0:
                                wOutValue = output[j].Amount;
                                wOutResult = wOutValue;
                                values[j] += (wOutValue);
                                viewYearStatistics[index].TotallOutput = values[j];
                                break;
                            case 1:
                                wOutValue = output[j].Percent;
                                wOutResult = wOutValue;
                                values[j] += (wOutValue * 100);

                                if (wOutValue > 0)
                                {
                                    countWorkingMonthsForCurrentYear[j] += 1;
                                }

                                if (countWorkingMonthsForCurrentYear[j] > 0)
                                {
                                    viewYearStatistics[index].TotallOutput = (values[j] / 100 / countWorkingMonthsForCurrentYear[j]);
                                }

                                break;
                            case 2:
                                wOutValue = output[j].Makeready;
                                wOutResult = wOutValue;
                                values[j] += (wOutValue);
                                viewYearStatistics[index].TotallOutput = values[j];
                                break;
                            case 3:
                                wOutValue = output[j].MakereadyTime;
                                wOutResult = time.MinuteToTimeString((int)wOutValue);
                                values[j] += (wOutValue);
                                viewYearStatistics[index].TotallOutput = time.MinuteToTimeString((int)values[j]);
                                break;
                            case 4:
                                wOutValue = output[j].CountShifts;
                                wOutResult = wOutValue;
                                values[j] += (wOutValue);
                                viewYearStatistics[index].TotallOutput = values[j];
                                break;
                            case 5:
                                wOutValue = output[j].Worktime;
                                wOutResult = time.MinuteToTimeString((int)wOutValue);
                                values[j] += (wOutValue);
                                viewYearStatistics[index].TotallOutput = time.MinuteToTimeString((int)values[j]);
                                break;
                            case 6:
                                wOutValue = output[j].Bonus;
                                wOutResult = wOutValue;
                                values[j] += (wOutValue * 100);
                                viewYearStatistics[index].TotallOutput = values[j] / 100;
                                break;
                            default:
                                break;
                        }

                        viewYearStatistics[index].EnterMonthValue(m, wOutResult);

                        /*switch (typeValueLoad)
                        {
                            case 0:
                                viewYearStatistics[index].TotallOutput = values[j].ToString("N0");
                                break;
                            case 1:
                                viewYearStatistics[index].TotallOutput = values[j].ToString("P1");
                                break;
                            case 2:
                                viewYearStatistics[index].TotallOutput = values[j].ToString("N0");
                                break;
                            case 3:
                                viewYearStatistics[index].TotallOutput = time.MinuteToTimeString((int)values[j]);
                                break;
                            default:
                                break;
                        }*/

                        totalOutputMonth += wOutValue;
                    }
                }

                summWorkingOut += totalOutputMonth;
                summWOPercent += totalOutputMonth / countWorkingUsersForCurrentMonth;

                if (countWorkingUsersForCurrentMonth > countWorkingUsersForCurrentYear)
                {
                    countWorkingUsersForCurrentYear = countWorkingUsersForCurrentMonth;
                }

                switch (typeValueLoad)
                {
                    case 0:
                        totalOutputMonthResult = totalOutputMonth;
                        break;
                    case 1:
                        totalOutputMonthResult = totalOutputMonth / countWorkingUsersForCurrentMonth;
                        break;
                    case 2:
                        totalOutputMonthResult = totalOutputMonth;
                        break;
                    case 3:
                        totalOutputMonthResult = time.MinuteToTimeString((int)totalOutputMonth);
                        break;
                    case 5:
                        totalOutputMonthResult = time.MinuteToTimeString((int)totalOutputMonth);
                        break;
                    default:
                        totalOutputMonthResult = totalOutputMonth;
                        break;
                }

                viewYearStatistics[summIndex].EnterMonthValue(m, totalOutputMonthResult);


                /*Invoke(new Action(() =>
                {
                    //dataGridViewYearStatistic.DataSource = null;
                    dataGridViewYearStatistic.DataSource = viewYearStatistics;
                    dataGridViewYearStatistic.Invalidate();
                    dataGridViewYearStatistic.Refresh();
                }));*/
            }

            string formatValue = "";

            switch (typeValueLoad)
            {
                case 0:
                    summWorkingOutResult = summWorkingOut;
                    formatValue = "N0";
                    break;
                case 1:
                    summWorkingOutResult = summWOPercent / lastCalculatingMonth;
                    formatValue = "P2";
                    break;
                case 2:
                    summWorkingOutResult = summWorkingOut;
                    formatValue = "N0";
                    break;
                case 3:
                    summWorkingOutResult = time.MinuteToTimeString((int)summWorkingOut);
                    formatValue = "";
                    break;
                case 5:
                    summWorkingOutResult = time.MinuteToTimeString((int)summWorkingOut);
                    formatValue = "";
                    break;
                case 6:
                    summWorkingOutResult = summWorkingOut;
                    formatValue = "P0";
                    break;
                default:
                    summWorkingOutResult = summWorkingOut;
                    formatValue = "N0";
                    break;
            }

            viewYearStatistics[summIndex].TotallOutput = summWorkingOutResult;

            Invoke(new Action(() =>
            {
                CreatColumnHeaderNamesForYearStatistic(dataGridViewYearStatistic);
                SetFormatValueForColumns(dataGridViewYearStatistic, formatValue);

                dataGridViewYearStatistic.DataSource = null;
                dataGridViewYearStatistic.DataSource = viewYearStatistics;

                dataGridViewYearStatistic.Invalidate();
                dataGridViewYearStatistic.Refresh();

                for (int i = 0; i < dataGridViewYearStatistic.Rows.Count - 1; i++)
                {
                    if (i % 2 == 0)
                    {
                        dataGridViewYearStatistic.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                    else
                    {
                        dataGridViewYearStatistic.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }

                dataGridViewYearStatistic.Rows[dataGridViewYearStatistic.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Gray;
            }));

            if (!token.IsCancellationRequested)
            {
                

                /*Invoke(new Action(() =>
                {
                    bool hourValue = false;

                    switch (typeValueLoad)
                    {
                        case 0:
                            labelYearStatisticValue.Text = summWorkingOut.ToString("N0");
                            hourValue = false;
                            break;
                        case 1:
                            labelYearStatisticValue.Text = (summWorkingOut / userOutputs.Count).ToString("P1");
                            hourValue = false;
                            break;
                        case 2:
                            labelYearStatisticValue.Text = summWorkingOut.ToString("N0");
                            hourValue = false;
                            break;
                        case 3:
                            labelYearStatisticValue.Text = time.MinuteToTimeString((int)summWorkingOut);
                            hourValue = true;
                            break;
                        default:
                            break;
                    }

                    DrawDiagram(chartStatYear, values, names, "");

                    //label2.Text = summWorkingOut.ToString("N0");
                }));*/
            }
        }

        private void CreatColumnHeaderNamesForYearStatistic(DataGridView dataGrid)
        {
            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "",
                Visible = false
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "ФИО"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M01",
                HeaderText = "Январь"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M02",
                HeaderText = "Февраль"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M03",
                HeaderText = "Март"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M04",
                HeaderText = "Апрель"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M05",
                HeaderText = "Май"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M06",
                HeaderText = "Июнь"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M07",
                HeaderText = "Июль"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M08",
                HeaderText = "Август"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M09",
                HeaderText = "Сентябрь"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M10",
                HeaderText = "Октябрь"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M11",
                HeaderText = "Ноябрь"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "M12",
                HeaderText = "Декабрь"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotallOutput",
                HeaderText = "Сумма"
            });
        }

        private void SetFormatValueForColumns(DataGridView dataGridView, string formatValue)
        {
            for (int i = 2; i  < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].DefaultCellStyle.Format = formatValue;
            }
        }

        private void ExportWOutYearToExcel(DataGridView dataGridView)
        {
            using (Excel excel = new Excel())
            {
                Document doc = new Document(excel.Document);

                int startTablePositionX = 4;
                int startTablePositionY = 1;

                doc[1, 2] = comboBoxStatYearSelectYear.Text + " год";
                doc[1, 4] = comboBoxStatYearTypeViewSelect.Text;

                doc[3, 2] = comboBoxStatYearCategorySelect.Text;

                if (comboBoxStatYearEquipSelect.SelectedIndex <= 1)
                {
                    for (int i = 2; i < comboBoxStatYearEquipSelect.Items.Count; i++)
                    {
                        doc[i + 1, 4] = comboBoxStatYearEquipSelect.Items[i].ToString();

                        doc.SetRange(i + 1, 4, i + 1, 6);
                        doc.Merge();

                        startTablePositionX++;
                    }
                }
                else
                {
                    doc[3, 4] = comboBoxStatYearEquipSelect.Text;

                    startTablePositionX++;
                }

                doc.SetRange(1, 2, 1, 3);
                doc.Merge();

                doc.SetRange(1, 4, 1, 6);
                doc.Merge();

                doc.SetRange(1, 2, 1, 6);
                doc.DrawThinLineRange();

                doc.SetRange(3, 2, 3, 3);
                doc.Merge();
                doc.DrawThinLineRange();

                doc.SetRange(3, 4, 3, 6);
                doc.Merge();

                doc.SetRange(3, 4, startTablePositionX - 2, 6);
                doc.DrawThinLineRange();

                int x = startTablePositionX;
                int y = startTablePositionY;
                int maxColumnCount = y;

                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (y > startTablePositionY)
                    {
                        doc.SetRange(x, y, x, y);
                        doc.SetColumnWidth(column.Width / 7);

                        doc[x, y] = column.HeaderText;
                    }
                    else
                    {
                        doc.SetRange(x, y, x, y);
                        doc.SetColumnWidth(4);

                        doc[x, y] = "№";
                    }

                    y++;
                }

                x++;
                y = startTablePositionY;

                foreach (DataGridViewRow item in dataGridView.Rows)
                {
                    foreach (DataGridViewCell value in item.Cells)
                    {
                        if (y > 1)
                        {
                            doc[x, y] = value.Value;
                        }
                        else
                        {
                            if ((x - startTablePositionX) < dataGridView.Rows.Count)
                            {
                                doc[x, y] = (x - startTablePositionX).ToString("N0");
                            }
                        }
                            
                        y++;
                    }

                    if (x >= 1)
                    {
                        doc.SetRange(x, 1, x, y - 1);
                        doc.SetBackColorRange(item.DefaultCellStyle.BackColor);
                        //doc.DrawThinLineRange();
                    }

                    if (maxColumnCount != y)
                        maxColumnCount = y;

                    x++;
                    y = startTablePositionY;
                }

                doc.SetRange(startTablePositionX, 1, startTablePositionX, maxColumnCount - 1);
                doc.SetFontBoldRange(true);
                doc.SetHorizontalAligmentRange(RangeAligment.Center);
                doc.SetBackColorRange(Color.Gray);

                doc.SetRange(x - 1, 1, x - 1, maxColumnCount - 1);
                doc.SetFontBoldRange(true);

                doc.SetRange(startTablePositionX, 1, x - 1, maxColumnCount - 1);
                doc.DrawThinLineRange();

                doc.SetRange(startTablePositionX, maxColumnCount - 1, x - 1, maxColumnCount - 1);
                doc.SetFontBoldRange(true);

                doc.SetRange(1, 1, 1, 1);
            }
        }



















        private void StartLoadingStatisticForUser()
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            DateTime date;
            date = DateTime.MinValue.AddYears(Convert.ToInt32(comboBoxUserYears.Text) - 1);

            int typeValueLoad = comboBoxStatType.SelectedIndex;
            bool loaddStatisticFromAllEquips = !metroSetSwitchUserStatistic.Switched;

            //Task task = new Task(() => LoadUsersFromBase(token, date));
            Task task = new Task(() => LoadingStatisticForUser(cancelTokenSource.Token, date, loaddStatisticFromAllEquips), cancelTokenSource.Token);
            //LoadUsersFromBase(cancelTokenSource.Token, date);

            task.Start();
        }

        private void LoadingStatisticForUser(CancellationToken token, DateTime date, bool loaddStatisticFromAllEquips = true)
        {
            CalculateWorkingOutput workingOutSum = new CalculateWorkingOutput();
            ValueDateTime time = new ValueDateTime();
            ValueUsers valueUsers = new ValueUsers();

            List<string> monthNames = new List<string>();

            int category = -1;
            int usersComboBoxSelectedIndex = -1;
            int userID = -1;
            string userName = "";

            Invoke(new Action(() =>
            {
                category = comboBoxUserCategory.SelectedIndex;
                usersComboBoxSelectedIndex = comboBoxUserNames.SelectedIndex;
                userName = comboBoxUserNames.Text;
            }));

            if (usersComboBoxSelectedIndex < 1)
            {
                Invoke(new Action(() =>
                {
                    ResetUserStatisc(false);
                }));

                return;
            }

            Invoke(new Action(() =>
            {
                ResetUserStatisc(false);

                loadUserStatistic = true;
                buttonUpdateUserStatistic.Text = "Отмена";

                comboBoxUserYears.Enabled = false;
                comboBoxUserCategory.Enabled = false;
                comboBoxUserNames.Enabled = false;

                progressBarUserStatistic.Minimum = 0;
                progressBarUserStatistic.Maximum = 12;
                progressBarUserStatistic.Value = 0;
            }));

            userID = userListForYear[usersComboBoxSelectedIndex - 1].Id;

            List<int> equipsListForCategory = null;

            if (!loaddStatisticFromAllEquips)
            {
                equipsListForCategory = new List<int>();

                List<Equip> equips = categoriesList[category].Equips;

                foreach (Equip equip in equips)
                {
                    if (equip.Selected)
                    {
                        equipsListForCategory.Add(equip.Id);
                    }
                }
            }
            
            List<float> amountYear = new List<float>();
            List<float> wOutputYear = new List<float>();
            List<float> makereadyYear = new List<float>();
            List<float> makereadyTimeYear = new List<float>();
            List<float> worktimeYear = new List<float>();
            List<float> bonusYear = new List<float>();
            List<float> countShifts = new List<float>();

            int countActiveMonth = 0;

            for (int i = 0; i < 12; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                monthNames.Add(CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU").DateTimeFormat.GetMonthName(i + 1));

                DateTime currentDateTime = date.AddMonths(i);

                UserWorkingOutput userWorkingOutput = workingOutSum.FullWorkingOutput(userID, currentDateTime, token, equipsListForCategory);
                
                if (token.IsCancellationRequested)
                {
                    break;
                }

                /*userWorkingOutputs.Add(new UserWorkingOutput(
                    userID,
                    userName,
                    userWorkingOutput.Amount,
                    userWorkingOutput.Worktime,
                    userWorkingOutput.Percent,
                    userWorkingOutput.Makeready,
                    userWorkingOutput.Bonus
                    ));*/

                amountYear.Add(userWorkingOutput.Amount);
                wOutputYear.Add(userWorkingOutput.Percent * 100);
                makereadyYear.Add(userWorkingOutput.Makeready);
                makereadyTimeYear.Add(userWorkingOutput.MakereadyTime);
                worktimeYear.Add(userWorkingOutput.Worktime);
                bonusYear.Add(userWorkingOutput.Bonus);
                countShifts.Add(userWorkingOutput.CountShifts);

                if (userWorkingOutput.Amount > 0 || userWorkingOutput.Worktime > 0)
                {
                    countActiveMonth++;
                }

                ListViewItem itemCurrent = new ListViewItem();

                itemCurrent.Name = i.ToString();
                itemCurrent.Text = (i + 1).ToString();
                itemCurrent.SubItems.Add(monthNames[i]);
                itemCurrent.SubItems.Add(userWorkingOutput.CountShifts.ToString("N0"));
                itemCurrent.SubItems.Add(userWorkingOutput.Amount.ToString("N0"));
                itemCurrent.SubItems.Add(time.MinuteToTimeString((int)userWorkingOutput.Worktime));
                itemCurrent.SubItems.Add(userWorkingOutput.Percent.ToString("P2"));
                itemCurrent.SubItems.Add(userWorkingOutput.Makeready.ToString("N0") + " (" + time.MinuteToTimeString((int)userWorkingOutput.MakereadyTime) + ")");
                itemCurrent.SubItems.Add(userWorkingOutput.Bonus.ToString("P0"));

                Invoke(new Action(() =>
                {
                    ListViewUserWorking.Items.Add(itemCurrent);
                }));

                Invoke(new Action(() =>
                {
                    progressBarUserStatistic.Value++;
                }));
            }

            if (!token.IsCancellationRequested)
            {
                ListViewItem item = new ListViewItem();

                item.Name = "sum";
                item.Text = "";
                item.SubItems.Add("ИТОГ");
                item.SubItems.Add(countShifts.Sum().ToString("N0"));
                item.SubItems.Add(amountYear.Sum().ToString("N0"));
                item.SubItems.Add(time.MinuteToTimeString((int)worktimeYear.Sum()));
                item.SubItems.Add((wOutputYear.Sum() / countActiveMonth / 100).ToString("P2"));
                item.SubItems.Add(makereadyYear.Sum().ToString("N0") + " (" + time.MinuteToTimeString((int)makereadyTimeYear.Sum()) + ")");
                item.SubItems.Add(bonusYear.Sum().ToString("P0"));

                item.Font = new Font(ListView.DefaultFont, FontStyle.Bold);

                Invoke(new Action(() =>
                {
                    ListViewUserWorking.Items.Add(item);
                }));

                Invoke(new Action(() =>
                {
                    DrawDiagram(chartUserAmount, amountYear, monthNames, "Количество выполненной продукци");
                    DrawDiagram(chartUserWorkingOutput, wOutputYear, monthNames, "Средняя выработка, %");
                    DrawDiagram(chartUserMakereadyCount, makereadyYear, monthNames, "Количество приладок");
                }));
            }

            Invoke(new Action(() =>
            {
                loadUserStatistic = false;
                buttonUpdateUserStatistic.Text = "Обновить";

                comboBoxUserYears.Enabled = true;
                comboBoxUserCategory.Enabled = true;
                comboBoxUserNames.Enabled = true;

                progressBarUserStatistic.Value = 0;
            }));
        }

        private void LoadUserList()
        {
            userListForYear?.Clear();
            userListForYear = new List<User>();

            ValueUsers valueUsers = new ValueUsers();

            int category = -1;
            DateTime date = DateTime.MinValue;

            Invoke(new Action(() =>
            {
                //comboBoxUserNames.Items.Clear();
                category = comboBoxUserCategory.SelectedIndex;
                date = date.AddYears(Convert.ToInt32(comboBoxUserYears.Text) - 1);
            }));

            List<int> equipsListForCategory = new List<int>();
            List<Equip> equips = categoriesList[category].Equips;

            foreach (Equip equip in equips)
            {
                equipsListForCategory.Add(equip.Id);
            }

            List<int> usersFromCategory = valueUsers.LoadUsersListOnlyIDFromSelectYear(equipsListForCategory, date);

            comboBoxUserNames.Items.Add("<выберите сотрудника>");

            foreach (int user in usersFromCategory)
            {
                string name = SelectValueFromDictionary(users, user);

                userListForYear.Add(new User (user, name));
                
                //comboBoxUserNames.Items.Add(name);
            }

            userListForYear.Sort((b1, b2) => b1.Name.CompareTo(b2.Name));

            foreach (User user in userListForYear)
            {
                comboBoxUserNames.Items.Add(user.Name);
            }

            comboBoxUserNames.SelectedIndex = 0;
        }
        private void LoadCategoryList(ComboBox comboBox, List<string> startCategoryes = null)
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();

            categoriesList?.Clear();
            categoriesList = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            loadCategoryList = true;

            comboBox.Items.Clear();

            if (startCategoryes != null)
            {
                comboBox.Items.AddRange(startCategoryes.ToArray());
            }

            for (int i = 0; i < categoriesList.Count; i++)
            {
                comboBox.Items.Add(categoriesList[i].Name);
            }

            comboBox.SelectedIndex = 0;

            loadCategoryList = false;
        }

        private void ResetUserStatisc(bool clearUserList = true)
        {
            if (clearUserList)
                comboBoxUserNames.Items.Clear();

            chartUserAmount.Titles.Clear();
            chartUserWorkingOutput.Titles.Clear();
            chartUserMakereadyCount.Titles.Clear();

            chartUserAmount.Series.Clear();
            chartUserWorkingOutput.Series.Clear();
            chartUserMakereadyCount.Series.Clear();

            ListViewUserWorking.Items.Clear();
        }
        private void ChangeCategoryValuesForUsersStatistic()
        {
            if (!loadShiftsParameter)
            {
                if (comboBoxUserYears.SelectedIndex != -1 && comboBoxUserCategory.SelectedIndex != -1)
                {
                    LoadUserList();
                }
            }
        }

        private void SelectUsersStatistic()
        {
            LoadCategoryList(comboBoxUserCategory);
        }

        /*private void ChangeTypeWorkingOutput()
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            AddUserWorkingOutputValues(cancelTokenSource.Token, comboBoxStatType.SelectedIndex);
        }*/

        //Загрузка плана
        private void SelectPlanOrders()
        {
            LoadCategoryList(comboBoxPlanCategory, new List<string> { "<выберие участок>", "<показать все оборудование>"});
        }
        /// <summary>
        /// Загрузка списка оборудования для выбранной категории
        /// </summary>
        /// <param name="index">Индекс выбранной категории</param>
        /// <param name="comboBoxEquips">Первые записи в списке оборудования</param>
        /// <param name="positionAllEquipsLoad">Индекс из списка категорий для загрузки всего доступного оборудования</param>
        /// <param name="startPositionEquipsList">Индекс из списка категорий начиная с которого оборудование загружается для выбранной категории</param>
        private void LoadEquipList(int index, ComboBox comboBoxEquips, List<string> firstItems, int positionAllEquipsLoad = 1, int startPositionEquipsList = 2)
        {
            equipListForSelectedCategory?.Clear();
            equipListForSelectedCategory = new List<Equip>();

            comboBoxEquips.Items?.Clear();

            if (index < 1)
            {
                return;
            }

            comboBoxEquips.Items.AddRange(firstItems.ToArray());

            if (index == positionAllEquipsLoad)
            {
                for (int i = 0; i < machines.Count; i++)
                {
                    equipListForSelectedCategory.Add(new Equip(machines.ElementAt(i).Key, machines.ElementAt(i).Value));
                    comboBoxEquips.Items.Add(machines.ElementAt(i).Value);
                }
            }

            if (index >= startPositionEquipsList)
            {
                List<Equip> equips = categoriesList[index - startPositionEquipsList].Equips;

                foreach (Equip equip in equips)
                {
                    string nameEquip = SelectValueFromDictionary(machines, equip.Id);
                    equipListForSelectedCategory.Add(new Equip(equip.Id, nameEquip));
                    comboBoxEquips.Items.Add(nameEquip/* + " - " + equip.Id*/);
                }
            }

            if (comboBoxEquips.Items.Count > 0)
                comboBoxEquips.SelectedIndex = 0;
        }

        private void SelectEquipForLoadPlan()
        {
            int indexSelectCombobBoxEquip = comboBoxPlanEquips.SelectedIndex;

            if (indexSelectCombobBoxEquip < 1)
            {
                listViewPlan.Items.Clear();

                return;
            }

            int eqipId = equipListForSelectedCategory[indexSelectCombobBoxEquip - 1].Id;
            bool loadAllOrders = metroSetSwitch3.Switched;

            StartLoading(eqipId, loadAllOrders);
        }
        private void StartLoading(int idMachine, bool loadAllOrders)
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            Task task = new Task(() => LoadPlan(cancelTokenSource.Token, idMachine, loadAllOrders), cancelTokenSource.Token);
            task.Start();

            //LoadPlan(cancelTokenSource.Token, idMachine, loadAllOrders);
        }

        private void LoadPlan(CancellationToken token, int idMachine, bool loadAllOrders)
        {
            List<OrdersLoad> orders = new List<OrdersLoad>();

            Invoke(new Action(() =>
            {
                orders?.Clear();
                //orderNumbers.Clear();
                listViewPlan.Items.Clear();
            }));

            int lastItemIndex = -1;

            try
            {
                using (SqlConnection Connect = DBConnection.GetDBConnection())
                {
                    Connect.Open();
                    SqlCommand Command = new SqlCommand
                    {
                        Connection = Connect,
                        CommandText = @"SELECT
	                                        man_planjob.id_man_planjob, 
	                                        man_planjob.date_begin, 
	                                        man_planjob.date_end, 
	                                        man_planjob.status, 
	                                        man_planjob.flags, 
	                                        man_planjob.id_equip, 
	                                        man_planjob_list.plan_out_qty, 
	                                        man_planjob_list.normtime, 
	                                        order_head.order_num, 
	                                        order_head.order_name, 
	                                        common_ul_directory.ul_name, 
	                                        common_equip_directory.equip_name, 
	                                        man_planjob_list.id_norm_operation, 
	                                        man_idletime.idletime_type, 
	                                        man_idletime.id_idletime, 
	                                        idletime_directory.idletime_name, 
	                                        man_idletime.id_man_idletime, 
	                                        order_head.id_order_head,
                                            norm_operation_table.ord
                                        FROM
	                                        dbo.man_planjob
	                                        INNER JOIN
	                                        dbo.man_planjob_list
	                                        ON 
		                                        man_planjob.id_man_order_job_item = man_planjob_list.id_man_order_job_item
	                                        LEFT JOIN
	                                        dbo.man_order_job_item
	                                        ON 
		                                        man_planjob.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                                        LEFT JOIN
	                                        dbo.man_order_job
	                                        ON 
		                                        man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                                        LEFT JOIN
	                                        dbo.order_head
	                                        ON 
		                                        man_order_job.id_order_head = order_head.id_order_head
	                                        LEFT JOIN
	                                        dbo.common_ul_directory
	                                        ON 
		                                        order_head.id_customer = common_ul_directory.id_common_ul_directory
	                                        LEFT JOIN
	                                        dbo.common_equip_directory
	                                        ON 
		                                        man_order_job.id_equip = common_equip_directory.id_common_equip_directory
	                                        LEFT JOIN
	                                        dbo.man_idletime
	                                        ON 
		                                        man_order_job.id_man_order_job = man_idletime.id_man_order_job
	                                        LEFT JOIN
	                                        dbo.idletime_directory
	                                        ON 
		                                        man_idletime.id_idletime = idletime_directory.id_idletime_directory
                                            LEFT JOIN
	                                        dbo.norm_operation_table
	                                        ON 
		                                        man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
                                        WHERE
	                                        man_planjob.status <> 2 AND
	                                        man_planjob.flags <> 1 AND
                                            plan_out_qty IS NOT NULL AND
	                                        man_planjob.id_equip = @idMachine
                                        ORDER BY
	                                        man_planjob.date_begin ASC"
                    };
                    Command.Parameters.AddWithValue("@idMachine", idMachine);

                    DbDataReader sqlReader = Command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        string operationStatus = "";

                        if ((int)sqlReader["flags"] == 8 || (int)sqlReader["flags"] == 10 || (int)sqlReader["flags"] == 40)
                        {
                            operationStatus = "В плане";
                        }

                        if ((int)sqlReader["flags"] == 0 || (int)sqlReader["flags"] == 32)
                        {
                            operationStatus = "В очереди";
                        }

                        if ((int)sqlReader["status"] == 1 || (int)sqlReader["status"] == 2 || (int)sqlReader["status"] == 3)
                        {
                            operationStatus = "В работе";
                        }

                        int idManPlanJob = Convert.ToInt32(sqlReader["id_man_planjob"]);

                        if (!DBNull.Value.Equals(sqlReader["order_num"]))
                        {
                            //подумать над реализацией
                            int lastIutemIndex = orders.Count - 1;
                            int itemIndex = orders.FindIndex((v) => v.IDManPlanJob == idManPlanJob);

                            if (itemIndex == -1)
                            {
                                orders.Add(new OrdersLoad(
                                        0,
                                        idManPlanJob,
                                        sqlReader["date_begin"].ToString(),
                                        sqlReader["date_end"].ToString(),
                                        sqlReader["order_num"].ToString(),
                                        sqlReader["ul_name"].ToString(),
                                        sqlReader["order_name"].ToString(),
                                        0,
                                        0,
                                        0,
                                        operationStatus,
                                        sqlReader["id_order_head"].ToString()
                                    ));

                                itemIndex = orders.Count - 1;
                            }

                            if ((int)sqlReader["ord"] == 0)
                            {
                                orders[itemIndex].makereadyTime = Convert.ToInt32(sqlReader["normtime"]) / Convert.ToInt32(sqlReader["plan_out_qty"]);
                            }

                            if ((int)sqlReader["ord"] == 1)
                            {
                                orders[itemIndex].workTime = Convert.ToInt32(sqlReader["normtime"]);
                                orders[itemIndex].amountOfOrder = Convert.ToInt32(sqlReader["plan_out_qty"]);
                            }

                            if (orders[orders.Count - 1].IDManPlanJob != idManPlanJob)
                            {
                                //AddOrderToListView(itemIndex, orders[itemIndex], token);
                                lastItemIndex = itemIndex;
                            }
                        }
                        else
                        {
                            int itemIndex = orders.FindIndex((v) => v.IDManPlanJob == idManPlanJob);

                            if (itemIndex == -1)
                            {
                                orders.Add(new OrdersLoad(
                                        1,
                                        idManPlanJob,
                                        sqlReader["date_begin"].ToString(),
                                        sqlReader["date_end"].ToString(),
                                        "",
                                        "",
                                        sqlReader["idletime_name"].ToString(),
                                        0,
                                        Convert.ToInt32(sqlReader["normtime"]),
                                        0,
                                        operationStatus,
                                        ""
                                    ));

                                itemIndex = orders.Count - 1;
                            }

                            //AddOrderToListView(itemIndex, orders[itemIndex], token);
                        }

                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                    }

                    Connect.Close();

                    for (int i = 0; i < orders.Count; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        if (loadAllOrders)
                        {
                            AddOrderToListView(i, orders[i], token);
                        }
                        else
                        {
                            if (orders[i].stamp != "В очереди")
                            {
                                AddOrderToListView(i, orders[i], token);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                Logger.WriteLine(ex.Message);
            }
        }

        private void AddOrderToListView(int index, OrdersLoad order, CancellationToken token)
        {
            ValueDateTime timeOperations = new ValueDateTime();

            ListViewItem item = new ListViewItem();

            item.Name = index.ToString();
            item.Text = (index + 1).ToString();
            item.SubItems.Add(Convert.ToDateTime(order.TimeStartOrder).ToString("dd.MM.yyyy HH:mm"));
            item.SubItems.Add(Convert.ToDateTime(order.TimeEndOrder).ToString("dd.MM.yyyy HH:mm"));
            item.SubItems.Add(order.numberOfOrder.ToString());
            item.SubItems.Add(order.nameCustomer.ToString());
            item.SubItems.Add(order.nameItem.ToString());
            item.SubItems.Add(timeOperations.MinuteToTimeString(order.makereadyTime, true));
            item.SubItems.Add(timeOperations.MinuteToTimeString(order.workTime, true));
            item.SubItems.Add(order.amountOfOrder.ToString("N0"));
            item.SubItems.Add(order.stamp);

            try
            {
                Invoke(new Action(() =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        listViewPlan.Items.Add(item);

                        //label1.Text = $"Загружено заказов: {index + 1}";
                    }
                }));
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
            }
        }

        private void ExportWOutToExcel(DataGridView dataGridView)
        {
            using (Excel excel = new Excel())
            {
                Document doc = CreateColumnName(excel, dataGridView);//new Document(excel.Document);

                /*doc[1, 1] = "X";
                 doc[1, 2] = "Y";*/

                int x = 1;
                int y = 1;
                int maxColumnCount = y;

                foreach (DataGridViewRow item in dataGridView.Rows)
                {
                    foreach (DataGridViewCell value in item.Cells)
                    {
                        if (x < 3) break;

                        doc[x, y++] = value.Value;

                        if (maxColumnCount != y)
                            maxColumnCount = y;
                    }

                    if (x >= 3)
                    {
                        doc.SetRange(x, 1, x, y);
                        doc.SetBackColorRange(dataGridView.Rows[x - 1].DefaultCellStyle.BackColor);
                        doc.DrawThinLineRange();
                    }

                    x++;
                    y = 1;
                }

                doc.SetRange(1, 3, x, maxColumnCount);
                doc.SetHorizontalAligmentRange(RangeAligment.Center);
                doc.SetRange(1, 1, 1, 1);
            }
        }

        private Document CreateColumnName(Excel excel, DataGridView dataGridView)
        {
            using (excel)
            {
                Document result = new Document(excel.Document);

                /*doc[1, 1] = "X";
                 doc[1, 2] = "Y";*/

                /*for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    result.SetRange(1, i + 1, 1, i + 1);
                    result.SetColumnWidth(dataGridView.Columns[i].Width / 8);

                    if (i > 2 &&  i < dataGridView.ColumnCount - 6 && i % 2 == 0)
                    {
                        result.SetRange(1, i - 1, 1, i);
                        result.Merge();
                        result.SetHorizontalAligmentRange(RangeAligment.Center);
                    }
                }*/

                /*for (int i = 3; i < dataGridView.ColumnCount - 6; i++)
                {
                    //idc < 2 || idc > dataGridView1.Columns.Count - 8
                    if (i % 2 == 0)
                    {
                        result.SetRange(1, i - 1, 1, i);
                        result.Merge();
                        result.SetHorizontalAligmentRange(RangeAligment.Center);
                    }
                }*/

                int x = 1;
                int y = 1;
                int maxColumn = y;

                foreach (DataGridViewRow item in dataGridView.Rows)
                {
                    foreach (DataGridViewCell value in item.Cells)
                    {
                        if (x == 1)
                        {
                            result.SetRange(1, y, 1, y);
                            result.SetColumnWidth(dataGridView.Columns[y - 1].Width / 8);

                            if (y < dataGridView.ColumnCount - 6 && y % 2 == 0)
                            {
                                result.SetRange(1, y - 1, 1, y);
                                result.Merge();
                                //result.SetHorizontalAligmentRange(RangeAligment.Center);
                            }
                        }

                        result[x, y++] = value.Value;
                    }

                    result.SetRange(x, 1, x, y);
                    //result.SetBackColorRange(dataGridView.Rows[x - 1].DefaultCellStyle.BackColor);
                    result.SetBackColorRange(Color.Silver);
                    result.DrawThinLineRange();

                    x++;
                    y = 1;

                    if (x == 3) break;
                }

                int firstColOut = dataGridView.ColumnCount - 6;

                result.SetRange(1, firstColOut, 1, firstColOut + 1);
                result.Merge();

                result.SetRange(1, firstColOut + 2, 1, firstColOut + 3);
                result.Merge();

                result.SetRange(1, firstColOut + 4, 1, firstColOut + 6);
                result.Merge();

                result.SetRange(2, 1, 2, 2);
                result.SetHorizontalAligmentRange(RangeAligment.Center);
                result.Merge();

                /*result.SetRange(2, 1, x, 2);
                result.SetHorizontalAligmentRange(RangeAligment.Left);
                result.SetRange(1, 1, 1, 1);*/

                return result;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (loadCategoryStatistic)
            {
                cancelTokenSource?.Cancel();

                Thread.Sleep(100);

                ClearAll();
            }
            else
            {
                ChangeIncomingValuesForCategoryStatistic();
            }
        }

        private void comboBoxStatYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeIncomingValuesForCategoryStatistic();
        }

        private void comboBoxStatMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeIncomingValuesForCategoryStatistic();
        }

        private void comboBoxStatCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeIncomingValuesForCategoryStatistic();
        }

        private void comboBoxStatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeTypeWorkingOutput();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int idc, idr;
            idc = dataGridView1.CurrentCell.ColumnIndex;
            idr = dataGridView1.CurrentCell.RowIndex;

            if (idc < 2 || idc > dataGridView1.Columns.Count - 8)
            {
                return;
            }

            if (idr < 2)
            {
                return;
            }

            var val = dataGridView1[idc, idr].Value;

            if (val == null)
            {
                return;
            }

            

            string sDate = dataGridView1[idc, 0].Value.ToString();
            int sShift = Convert.ToInt32(dataGridView1[idc, 1].Value);
            string userAndEquip = rowIndexes.ElementAt(idr - 2).Key;
            string value = dataGridView1[idc, idr].Value.ToString();

            DateTime date = Convert.ToDateTime(sDate + "." + comboBox3.Text);

            FormViewShiftDetails form = new FormViewShiftDetails(date, sShift, userAndEquip);
            form.ShowDialog();

            //MessageBox.Show("Date:" + sDate + ", Shift:" + sShift + ", rowIndexesValue:" + userAndEquip + ", Val: " + value);
        }

        private void comboBoxUserCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetUserStatisc();
            LoadUserList();
        }

        private void comboBoxUserYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetUserStatisc();
            ChangeCategoryValuesForUsersStatistic();
        }

        private void comboBoxUserNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            StartLoadingStatisticForUser();
        }

        private void buttonUpdateUserStatistic_Click(object sender, EventArgs e)
        {
            if (loadUserStatistic)
            {
                cancelTokenSource?.Cancel();
                
                Thread.Sleep(100);

                ResetUserStatisc(false);
            }
            else
            {
                StartLoadingStatisticForUser();
            }
        }

        private void comboBoxPlanCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlanCategory.SelectedIndex < 1)
            {
                listViewPlan.Items.Clear();
            }

            LoadEquipList(comboBoxPlanCategory.SelectedIndex, comboBoxPlanEquips, new List<string> { "<выберите оборудование>" });
        }

        private void comboBoxPlanEquips_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectEquipForLoadPlan();
        }

        private void metroSetSwitch3_SwitchedChanged(object sender)
        {
            SelectEquipForLoadPlan();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SelectEquipForLoadPlan();
        }

        private void metroSetSwitchUserStatistic_SwitchedChanged(object sender)
        {
            StartLoadingStatisticForUser();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            cancelTokenSource?.Cancel();

            cancelTokenSource = new CancellationTokenSource();

            StartAddingWorkingTimeToListView(cancelTokenSource.Token);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ExportWOutToExcel(dataGridView1);
        }

        private void comboBoxStatYearCategorySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxStatYearCategorySelect.SelectedIndex == 0)
            {
                ClearAllForYearStatistic();
            }

            LoadEquipList(comboBoxStatYearCategorySelect.SelectedIndex, comboBoxStatYearEquipSelect, new List<string> { "<выберите оборудование для загрузки>", "<выбор всего оборудования участка>" }, -1, 1);
        }

        private void comboBoxStatYearEquipSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeIncomingValuesForCategoryYearStatistic();
        }

        private void buttonStatYearUpdate_Click(object sender, EventArgs e)
        {
            if (loadCategoryStatistic)
            {
                cancelTokenSource?.Cancel();

                Thread.Sleep(100);

                ClearAllForYearStatistic();
            }
            else
            {
                ChangeIncomingValuesForCategoryYearStatistic();
            }
        }

        private void comboBoxStatYearTypeViewSelect_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            ChangeTypeWorkingOutputYearStatistic();
        }

        private void comboBoxStatYearSelectYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeIncomingValuesForCategoryYearStatistic();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ExportWOutYearToExcel(dataGridViewYearStatistic);
        }
    }
}
