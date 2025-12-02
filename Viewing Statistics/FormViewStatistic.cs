using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libData;
using libINIFile;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using libSql;
using libTime;
using System.Threading;

namespace Viewing_Statistics
{
    public partial class Form1 : MetroSetForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        CancellationTokenSource cancelTokenSource;

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();

        List<User> usersList;// = new List<User>();
        List<User> usersListPreviewMonth;
        List<User> usersListCurrentMonth;

        List<PageView> pages;// = new List<Page>();

        DateTime timeLastChengePage;

        //int countOutValue = 3;

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
            catch
            {
                //MessageBox.Show("Ошибка подключения", "Ошибка", MessageBoxButtons.OK);
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
            catch (Exception ex)
            {
                LogWrite(ex);
            }
        }

        private void LoadAllUsers()
        {
            try
            {
                ValueUsers usersValue = new ValueUsers();

                users.Clear();

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

                machines.Clear();

                machines = equipsValue.LoadMachine();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
                LogWrite(ex);
            }
        }

        private async Task LoadUsersListAsync(List<int> equips, DateTime startDate)
        {
            try
            {
                usersList?.Clear();
                //usersListMonth?.Clear();

                //usersList = new List<User>();

                ValueUsers usersValue = new ValueUsers();

                DateTime startMonth = DateTime.MinValue.AddYears(startDate.Year - 1).AddMonths(startDate.Month - 1);

                //usersList = usersValue.LoadUsersList(equips, date);
                //usersList = usersValue.LoadUsersListFromLastAnyDays(equips, countDays);
                usersList = await usersValue.LoadUsersListFromLastAnyDays(equips, startMonth.AddMonths(-1));
                
                //usersList = usersValue.LoadUsersListFromLastAnyDays(equips, startDate);
                usersListPreviewMonth = await usersValue.LoadUsersListFromSelectMonth(equips, startDate.AddMonths(-1));
                usersListCurrentMonth = await usersValue.LoadUsersListFromSelectMonth(equips, startDate);

                //MessageBox.Show(usersList.Count + ", " + startDate.AddMonths(-1));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
                LogWrite(ex);
            }
        }

        private async Task LoadShiftsForUsersListAsync(DateTime startDate)
        {
            try
            {
                ValueShifts valueShifts = new ValueShifts();
                INIView view = new INIView();

                int countShifts = view.GetCountShifts();
                bool givenShiftNumber = view.GetGivenShiftNumber();
                DateTime date = DateTime.Now;

                DateTime startMonth = DateTime.MinValue.AddYears(startDate.Year - 1).AddMonths(startDate.Month - 1);

                usersList = await valueShifts.LoadShiftsAsync(usersList, startMonth.AddMonths(-1), countShifts, givenShiftNumber);
                
                //usersList = valueShifts.LoadShifts(usersList, startDate, countShifts, givenShiftNumber);
                usersListPreviewMonth = await valueShifts.LoadShiftsForSelectedMonth(usersListPreviewMonth, startDate.AddMonths(-1), countShifts, givenShiftNumber);
                usersListCurrentMonth = await valueShifts.LoadShiftsForSelectedMonth(usersListCurrentMonth, startDate, countShifts, givenShiftNumber);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
                LogWrite(ex);
            }
        }

        private async Task ReloadDataFromBaseAsync(List<PageView> pagesList, DateTime startDate)
        {
            List<int> equips = GetAllEquipsFromPagesList(pagesList);

            DisposingAllControlsFromTabPages();
            
            LoadAllUsers();
            LoadMachine();
            await LoadUsersListAsync(equips, startDate);
            await LoadShiftsForUsersListAsync(startDate);
        }

        private void DisposingAllControlsFromTabPages()
        {
            for (int i = 0; i < metroSetTabControl1.TabPages.Count; i++)
            {
                Control.ControlCollection controls = metroSetTabControl1.TabPages[i].Controls;

                foreach(Control control in controls)
                {
                    control.Dispose();
                }
            }
        }

        private List<PageView> LoadPagesList()
        {
            ValuePagesList valuePagesList = new ValuePagesList();

            List<PageView> pageList = valuePagesList.LoadPagesList(false);

            return pageList;
        }

        private List<int> GetAllEquipsFromPagesList(List<PageView> pageList)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < pageList.Count; i++)
            {
                if (pageList[i].TypePage == 0)
                {
                    for (int j = 0; j < pageList[i].CategoryAndEquips.Count; j++)
                    {
                        if (pageList[i].CategoryAndEquips[j] != "")
                        {
                            int[] item = pageList[i].CategoryAndEquips[j]?.Split(':')?.Select(Int32.Parse)?.ToArray<int>();

                            result.Add(item[1]);
                        }
                    }
                }
            }

            return result;
        }

        private async Task SelectNextPageAsync()
        {
            DateTime currentTime = DateTime.Now;
            int currentPage = metroSetTabControl1.SelectedIndex;
            if (currentPage >= 0)
            {
                if (timeLastChengePage.AddSeconds(pages[currentPage].TimeForView) <= currentTime)
                {
                    if (currentPage < pages.Count - 1)
                    {
                        metroSetTabControl1.SelectedIndex = currentPage + 1;
                        timeLastChengePage = currentTime;
                    }
                    else
                    {
                        await UpdatePagesListsFromFileAsync();
                        metroSetTabControl1.SelectedIndex = 0;
                        //timeLastChengePage = currentTime;
                    }
                }
            }
        }

        private int GetMinCountOutValue(List<PageView> pageList)
        {
            int countOutValue = 3;

            ValuePagesList valuePagesList = new ValuePagesList();

            for (int i = 0; i < pageList.Count; i++)
            {
                int countValue = valuePagesList.GetCountOutValue(pageList[i].OutValues);

                if (countValue < countOutValue)
                {
                    countOutValue = countValue;
                }
            }

            return countOutValue;
        }

        private int GetPeriodForView(int countOutValue)
        {
            INIView view = new INIView();

            int period = view.GetPeriod();

            bool autoDayAdded = view.GetAutoAddDays();

            if (autoDayAdded)
            {
                int width = metroSetTabControl1.Width;

                int countShifts = view.GetCountShifts();
                int wColNum = view.GetWidthNumberCol();
                int wColName = view.GetWidthNameCol();
                int wColVal = view.GetWidthWorkingOutCol();
                int wColResults = view.GetWidthResultsCol();

                //Сделать
                int fullWidthColForDay = wColVal * countOutValue * countShifts;
                int widthForColsVal = width - (wColNum + wColName + wColResults * countOutValue * 2);
                //MessageBox.Show(width + "");
                period = widthForColsVal / fullWidthColForDay;
            }

            return period;
        }

        private async Task UpdatePagesListsFromFileAsync()
        {
            INIView view = new INIView();

            pages?.Clear();

            pages = LoadPagesList();

            int countOutValue = GetMinCountOutValue(pages);
            
            //int period = view.GetPeriod();
            int period = GetPeriodForView(countOutValue);

            DateTime startDate = GetStartDate(period);

            await ReloadDataFromBaseAsync(pages, startDate);

            AddTabPageFromPageList(pages);

            //
        }

        private void AddTabPageFromPageList(List<PageView> pageList)
        {
            ValuePagesList valuePagesList = new ValuePagesList();

            metroSetTabControl1?.TabPages?.Clear();

            for (int i = 0; i < pageList.Count; i++)
            {
                TabPage page = new TabPage();
                page.Name = pageList[i].Id.ToString();
                page.Text = pageList[i].Name;

                if (pageList[i].TypePage == 0)
                {
                    int countOutValue = valuePagesList.GetCountOutValue(pageList[i].OutValues);

                    int period = GetPeriodForView(countOutValue);

                    DoubleBufferedDataGridView dataGrid = CreatGridView(i, period, countOutValue, pageList[i].OutValues);
                    
                    ViewStatistic(dataGrid, pageList[i], period, countOutValue, pageList[i].OutValues);

                    page.Controls.Add(dataGrid);
                }

                if (pageList[i].TypePage == 1)
                {
                    PictureBox pictureBox = CreatePictureBox(i, pageList[i].NameMediaFile);

                    page.Controls.Add(pictureBox);
                }

                metroSetTabControl1.TabPages.Add(page);
            }

            timeLastChengePage = DateTime.Now;
        }

        private DateTime GetStartDate(int period)
        {
            DateTime date = DateTime.Now;
            DateTime selectDate;

            if (date.Hour >= 8 && date.Hour <= 23)
            {
                selectDate = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - period);
            }
            else //if (date.Hour >= 0 && date.Hour < 8)
            {
                selectDate = DateTime.MinValue.AddYears(date.Year - 1).AddMonths(date.Month - 1).AddDays(date.Day - 1 - period);
            }
            
            return selectDate;
        }

        private void ViewStatistic(DoubleBufferedDataGridView dataGrid, PageView page, int period, int countOutValue, List<string> outValues)
        {
            AddUsersToGridView(dataGrid, page);
            StartAddingWorkingTimeToListView(dataGrid, period, countOutValue, outValues);
        }

        private void AddCellToGrid(DoubleBufferedDataGridView dataGrid, int indexRow, int indexCell, int collSpan = 1)
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

        private PictureBox CreatePictureBox(int index, string fileName)
        {
            string path = Application.StartupPath + "\\src\\" + fileName;

            PictureBox pictureBox = new PictureBox
            {
                Name = "pictureBox" + index,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            if (File.Exists(path))
            {
                pictureBox.Image = Image.FromFile(path);
            }

            return pictureBox;
        }

        private DoubleBufferedDataGridView CreatGridView(int indexPage, int period, int countOutValue, List<string> outValues)
        {
            DoubleBufferedDataGridView gridView = new DoubleBufferedDataGridView
            {
                Name = "gridView" + indexPage,
                Dock = DockStyle.Fill,
                ColumnHeadersVisible = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White
            };

            DateTime startPeriod = GetStartDate(period);

            string[] colCaption = { "Т", "%", "П" };

            INIView view = new INIView();

            int countShifts = view.GetCountShifts();

            gridView.Rows.Clear();
            gridView.Columns.Clear();

            List<string> nameCol = new List<string>();

            int width = metroSetTabControl1.Width;

            int wColNum = view.GetWidthNumberCol();
            int wColName = view.GetWidthNameCol();
            int wColVal = view.GetWidthWorkingOutCol();
            int wColResults = view.GetWidthResultsCol();

            bool autoWidthColVal = view.GetColWorksOutAutoWidth();

            if (autoWidthColVal)
            {
                int wForValue = width - (wColNum + wColName + wColResults * countOutValue * 2) - 8;
                int wTemp = wForValue / (period * countOutValue * 2);

                if (wTemp >= wColVal)
                    wColVal = wForValue / (period * countOutValue * 2);
            }

            int indexCol;

            indexCol = gridView.Columns.Add(@"index", @"");
            gridView.Columns[indexCol].Width = wColNum;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridView.Columns[indexCol].Frozen = true;

            indexCol = gridView.Columns.Add(@"name", @"");
            gridView.Columns[indexCol].Width = wColName;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridView.Columns[indexCol].Frozen = true;

            for (int i = 0; i < countShifts * period * countOutValue; i++)
            {
                if (i % (countShifts * countOutValue) == 0 || i == 0)
                {
                    int n = (i + countShifts * countOutValue) / (countShifts * countOutValue) - 1;

                    nameCol.Add(startPeriod.AddDays(n).ToString("dd.MM.yyyy"));

                    indexCol = gridView.Columns.Add(nameCol[nameCol.Count - 1], @"");
                }
                else
                {
                    indexCol = gridView.Columns.Add(@"col" + i, @"");
                }

                //indexCol = gridView.Columns.Add(nameCol[nameCol.Count - 1], @"");
                gridView.Columns[indexCol].Width = wColVal;
                gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gridView.Columns[indexCol].Frozen = false;
            }

            for (int i = 0; i < countOutValue * 2; i++)
            {
                if (i % (countOutValue) == 0 || i == 0)
                {
                    int n = i / countOutValue;

                    //nameCol.Add(startPeriod.AddDays(n).ToString("dd.MM.yyyy"));

                    indexCol = gridView.Columns.Add(startPeriod.AddMonths(n - 1).ToString("MM.yyyy"), @"");
                }
                else
                {
                    indexCol = gridView.Columns.Add(@"colRes" + i, @"");
                }

                //indexCol = gridView.Columns.Add(nameCol[nameCol.Count - 1], @"");
                gridView.Columns[indexCol].Width = wColResults;
                gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gridView.Columns[indexCol].Frozen = false;
            }

            /*indexCol = gridView.Columns.Add(@"totalWOut0", @"");
            gridView.Columns[indexCol].Width = wColResults;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridView.Columns[indexCol].Frozen = false;

            indexCol = gridView.Columns.Add(@"totalWOut1", @"");
            gridView.Columns[indexCol].Width = wColResults;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridView.Columns[indexCol].Frozen = false;

            indexCol = gridView.Columns.Add(@"totalWOut2", @"");
            gridView.Columns[indexCol].Width = wColResults;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridView.Columns[indexCol].Frozen = false;*/

            int indexRow;

            //Строка с датой
            indexRow = gridView.Rows.Add();
            gridView.Rows[indexRow].Frozen = true;

            AddCellToGrid(gridView, indexRow, 0, 2);
            gridView.Rows[indexRow].Cells[0].Value = "";

            for (int i = 2; i <= countShifts * period * countOutValue; i += countShifts * countOutValue)
            {
                AddCellToGrid(gridView, indexRow, i, countShifts * countOutValue);

                //gridView.Rows[indexRow].Cells[i].Value = ((i - 2 + countShifts * countOutValue) / (countShifts * countOutValue)).ToString("D2");
                gridView.Rows[indexRow].Cells[i].Value = nameCol[((i - 2) + countShifts * countOutValue) / (countShifts * countOutValue) - 1];
            }

            AddCellToGrid(gridView, indexRow, countShifts * period * countOutValue + 2, countOutValue * 2);
            gridView.Rows[indexRow].Cells[countShifts * period * countOutValue + 2].Value = "Итоги за период";

            //Строка с номерами смен
            indexRow = gridView.Rows.Add();
            gridView.Rows[indexRow].Frozen = true;

            AddCellToGrid(gridView, indexRow, 0, 2);
            gridView.Rows[indexRow].Cells[0].Value = "";

            for (int i = 2; i <= countShifts * period * countOutValue + 1; i += countOutValue * countShifts)
            {
                for (int j = 1; j <= countShifts; j++)
                {
                    int n = i + (j - 1) * countOutValue;

                    AddCellToGrid(gridView, indexRow, n, countOutValue);

                    gridView.Rows[indexRow].Cells[n].Value = (j).ToString();
                }
            }

            AddCellToGrid(gridView, indexRow, countShifts * period * countOutValue + 2, countOutValue);
            gridView.Rows[indexRow].Cells[countShifts * period * countOutValue + 2].Value = startPeriod.AddMonths(-1).ToString("MM.yyyy");

            AddCellToGrid(gridView, indexRow, countShifts * period * countOutValue + countOutValue + 2, countOutValue);
            gridView.Rows[indexRow].Cells[countShifts * period * countOutValue + countOutValue + 2].Value = startPeriod.ToString("MM.yyyy");

            //Строка с названиями столбцов "Т, %, П"
            indexRow = gridView.Rows.Add();
            gridView.Rows[indexRow].Frozen = true;

            AddCellToGrid(gridView, indexRow, 0, 2);
            gridView.Rows[indexRow].Cells[0].Value = "";

            for (int i = 2; i <= countShifts * period * countOutValue + 1; i += countOutValue)
            {
                for (int j = 2; j <= countShifts; j++)
                {
                    for (int k = 0; k < countOutValue; k++)
                    {
                        int n = i + k;

                        AddCellToGrid(gridView, indexRow, n);

                        gridView.Rows[indexRow].Cells[n].Value = colCaption[k];

                        for (int v = k; v < outValues.Count; v++)
                        {
                            if (outValues[v] != "0")
                            {
                                gridView.Rows[indexRow].Cells[n].Value = colCaption[v];
                                break;
                            }
                        }
                    }
                }
            }

            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < countOutValue; k++)
                {
                    int n = j * countOutValue + countShifts * period * countOutValue + 2 + k;

                    AddCellToGrid(gridView, indexRow, n);

                    gridView.Rows[indexRow].Cells[n].Value = colCaption[k];

                    for (int v = k; v < outValues.Count; v++)
                    {
                        if (outValues[v] != "0")
                        {
                            gridView.Rows[indexRow].Cells[n].Value = colCaption[v];
                            break;
                        }
                    }
                }
            }

            //AddCellToGrid(gridView, indexRow, countShifts * period * countOutValue + 2, 3);
            //gridView.Rows[indexRow].Cells[countShifts * period * countOutValue + 2].Value = "";

            return gridView;
        }

        private bool CheckUserShiftsFromViewPeriod(DataGridView dataGrid, List<UserShift> userShifts)
        {
            bool result = false;

            for (int i = 0; i < userShifts.Count; i++)
            {
                if (GetDataGridColumnIndexFromKey(dataGrid, userShifts[i].ShiftDate) != -1)
                {
                    result = true;
                    break;
                }
            }

            return result;
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

        private void AddUsersToGridView(DoubleBufferedDataGridView dataGrid, PageView page)
        {
            //ValueCategoryes valueCategoryes = new ValueCategoryes();
            INIView view = new INIView();

            bool viewAllEquipsForUser = view.GetLoadAllEquipForUser();

            //List<Category> categoryEquip = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            List<int> category = new List<int>();
            List<int> equips = new List<int>();

            List<string> equipsString = page.CategoryAndEquips;
            
            for (int i = 0; i < equipsString.Count; i++)
            {
                int[] item = equipsString[i]?.Split(':')?.Select(Int32.Parse)?.ToArray<int>();

                category.Add(item[0]);
                equips.Add(item[1]);
            }

            //rowIndexes.Clear();

            if (page.TypeLoad == 0)
            {
                for (int i = 0; i < equips.Count; i++)
                {
                    int countUserForCurrentEquip = 0;
                    string machine = "";

                    if (machines.ContainsKey(equips[i]))
                    {
                        machine = machines[equips[i]];
                    }
                    else
                    {
                        machine = "Оборудование " + equips[i];
                    }

                    AddItemToGrid(dataGrid, "e" + equips[i], "", machine, Color.Gray);
                    
                    for (int j = 0; j < usersList.Count; j++)
                    {
                        if (IsThereEquipForUser(usersList[j].Shifts, equips[i]))
                        {
                            //if (CheckUserShiftsFromViewPeriod(dataGrid, usersList[j].Shifts))
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

                                AddItemToGrid(dataGrid, CreateNameListViewItem(equips[i], usersList[j].Id), countUserForCurrentEquip.ToString(), user, color);
                            }
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
                        //Новая проерка, два цикла
                        for (int k = 0; k < usersList[j].Shifts.Count; k++)
                        {
                            for (int l = 0; l < usersList[j].Shifts[k].Orders.Count; l++)
                            {
                                //if (CheckUserShiftsFromViewPeriod(dataGrid, usersList[j].Shifts))
                                {
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
                        }

                        /*if (CheckUserShiftsFromViewPeriod(dataGrid, usersList[j].Shifts))
                        {
                            if (!usersCurrent.Contains(usersList[j].Id) && IsThereEquipForUser(usersList[j].Shifts, equips[i]))
                            {
                                usersCurrent.Add(usersList[j].Id);
                            }

                            if (viewAllEquipsForUser && !equipsCurrent.Contains(usersList[j].Equip))
                            {
                                equipsCurrent.Add(usersList[j].Equip);
                            }
                        }*/
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

                    AddItemToGrid(dataGrid, "u" + usersCurrent[i], "", user, Color.Gray);

                    int countEquipForCurrentUser = 0;

                    for (int j = 0; j < equipsCurrent.Count; j++)
                    {
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

                                AddItemToGrid(dataGrid, CreateNameListViewItem(equipsCurrent[j], usersList[index].Id), countEquipForCurrentUser.ToString(), machine, color);
                            }
                        }
                    }
                }
            }
        }

        private void AddItemToGrid(DoubleBufferedDataGridView dataGrid, string name, string text, string subText, Color color, int colSpan = 1)
        {
            int indexRow = dataGrid.Rows.Add();

            /*if (!rowIndexes.ContainsKey(name))
            {
                rowIndexes.Add(name, indexRow);
            }*/

            dataGrid.Rows[indexRow].HeaderCell.Value = name;

            dataGrid.Rows[indexRow].Cells[0].Value = text;
            dataGrid.Rows[indexRow].Cells[1].Value = subText;
            dataGrid.Rows[indexRow].DefaultCellStyle.BackColor = color;

            if (text == "")
            {
                dataGrid.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;
                dataGrid.Rows[indexRow].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
            }
            else
            {
                dataGrid.Rows[indexRow].DefaultCellStyle.ForeColor = Color.Black;
                dataGrid.Rows[indexRow].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            }
        }

        private void StartAddingWorkingTimeToListView(DoubleBufferedDataGridView dataGrid, int period, int countOutValue, List<string> outValues)
        {
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
            }

            DateTime previewMonth = GetStartDate(period).AddMonths(-1);
            DateTime currentMonth = GetStartDate(period);

            cancelTokenSource = new CancellationTokenSource();

            AddWorkingTimeUsersToListView(cancelTokenSource.Token, dataGrid, period, countOutValue, outValues);
            Task taskDetails = new Task(() => AddWorkingTimeUsersToListView(cancelTokenSource.Token, dataGrid, period, countOutValue, outValues), cancelTokenSource.Token);
            //taskDetails.Start();

            AddWorkingTimeMonthUsersToListView(cancelTokenSource.Token, dataGrid, usersListPreviewMonth, previewMonth, outValues);
            Task taskDetailsPreviewMonth = new Task(() => AddWorkingTimeMonthUsersToListView(cancelTokenSource.Token, dataGrid, usersListPreviewMonth, previewMonth, outValues), cancelTokenSource.Token);
            //taskDetailsPreviewMonth.Start();

            AddWorkingTimeMonthUsersToListView(cancelTokenSource.Token, dataGrid, usersListCurrentMonth, currentMonth, outValues);
            Task taskDetailsCurrentMonth = new Task(() => AddWorkingTimeMonthUsersToListView(cancelTokenSource.Token, dataGrid, usersListCurrentMonth, currentMonth, outValues), cancelTokenSource.Token);
            //taskDetailsCurrentMonth.Start();
        }

        private void AddWorkingTimeUsersToListView(CancellationToken token, DoubleBufferedDataGridView dataGrid, int period, int countOutValue, List<string> values)
        {
            List<WorkingOut> equipsListWorkingOut = new List<WorkingOut>();
            List<WorkingOut> usersListWorkingOut = new List<WorkingOut>();
            //List<int> usersCurrent = new List<int>();
            INIView view = new INIView();
            ValueDateTime timeValues = new ValueDateTime();

            int fullOutput = view.GetNormTime();
            int countShifts = view.GetCountShifts();
            
            for (int i = 0; i < usersList.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                //MessageBox.Show(usersList[i].Shifts.Count.ToString());
                if (usersList[i].Shifts != null)
                {
                    for (int j = 0; j < usersList[i].Shifts.Count; j++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        UserShift shift = usersList[i].Shifts[j];

                        if (shift.Orders != null)
                        {
                            int shiftNumber = shift.ShiftNumber;

                            List<int> currentEquipsList = new List<int>();

                            for (int k = 0; k < shift.Orders.Count; k++)
                            {
                                if (!currentEquipsList.Contains(shift.Orders[k].IdEquip))
                                {
                                    currentEquipsList.Add(shift.Orders[k].IdEquip);
                                }
                            }

                            for (int k = 0; k < currentEquipsList.Count; k++)
                            {
                                float timeWorkigOut = CalculateWorkTime(shift.Orders, currentEquipsList[k]);
                                float timeBacklog = fullOutput - timeWorkigOut;

                                usersList[i].WorkingOutUser += timeWorkigOut;
                                usersList[i].WorkingOutBacklog += timeBacklog;

                                float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

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
                                        equipsListWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].WorkingOut += timeWorkigOut;
                                    }
                                    else
                                    {
                                        equipsListWorkingOut[indexEquipsList].WorkingOutList.Add(new WorkingOutValue(
                                        shift.ShiftDate,
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
                                                        (v) => v.ShiftDate == shift.ShiftDate &&
                                                               v.ShiftNumber == shiftNumber
                                                               );

                                    if (indexUserListWOut != -1)
                                    {
                                        usersListWorkingOut[indexUserList].WorkingOutList[indexUserListWOut].WorkingOut += timeWorkigOut;
                                    }
                                    else
                                    {
                                        usersListWorkingOut[indexUserList].WorkingOutList.Add(new WorkingOutValue(
                                        shift.ShiftDate,
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
                                        shift.ShiftDate,
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
                                    string key = CreateNameListViewItem(currentEquipsList[k], usersList[i].Id);

                                    /*DataGridViewRow row;
                                    row = dataGrid.Rows[key];

                                    int indexRow = row.Index;*/
                                    int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                                    int indexCol = GetDataGridColumnIndexFromKey(dataGrid, shift.ShiftDate);

                                    if (indexRow != -1 && indexCol != -1)
                                    {
                                        int nextCol = 0;

                                        //dataGrid.Rows[indexRow].Cells[(day) * countShifts * countOutValue + shiftNumber * countOutValue - 1].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                                        if (values[0] == "1")
                                        {
                                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                                            nextCol++;
                                        }

                                        if (values[1] == "1")
                                        {
                                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = percentWorkingOut.ToString("P1");
                                            nextCol++;
                                        }

                                        if (values[2] == "1")
                                        {
                                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = GetBonusWorkingOut((int)Math.Round(timeWorkigOut));
                                            nextCol++;
                                        }

                                        //dataGrid.Rows[indexRow].Cells[period * countOutValue * countShifts + 2].Value = timeValues.MinuteToTimeString((int)Math.Round(usersList[i].WorkingOutUser));
                                        //dataGrid.Rows[indexRow].Cells[days * countShifts + 3].Value = timeValues.MinuteToTimeString(usersList[i].WorkingOutBacklog);
                                    }
                                }
                                ));
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < equipsListWorkingOut.Count; i++)
            {
                //int countDaysFromMonth = 0;

                for (int j = 0; j < equipsListWorkingOut[i].WorkingOutList.Count; j++)
                {
                    int shiftNumber = equipsListWorkingOut[i].WorkingOutList[j].ShiftNumber;
                    float timeWorkigOut = equipsListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "e" + equipsListWorkingOut[i].Id;

                        int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                        int indexCol = GetDataGridColumnIndexFromKey(dataGrid, equipsListWorkingOut[i].WorkingOutList[j].ShiftDate);

                        if (indexRow != -1 && indexCol != -1)
                        {
                            int nextCol = 0;

                            if (values[0] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                                nextCol++;
                            }

                            if (values[1] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = percentWorkingOut.ToString("P1");
                                nextCol++;
                            }

                            if (values[2] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = GetBonusWorkingOut((int)Math.Round(timeWorkigOut));
                                nextCol++;
                            }                                
                        }
                    }));
                }

                float fullTimeWorkigOut = equipsListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    string key = "e" + equipsListWorkingOut[i].Id;

                    /*if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[days * countShifts + 2].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[days * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutList.Count * fullOutput - equipsListWorkingOut[i].WorkingOutSumm);
                    }*/

                }));
            }

            for (int i = 0; i < usersListWorkingOut.Count; i++)
            {
                for (int j = 0; j < usersListWorkingOut[i].WorkingOutList.Count; j++)
                {
                    int shiftNumber = usersListWorkingOut[i].WorkingOutList[j].ShiftNumber;
                    float timeWorkigOut = usersListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(fullOutput, (int)timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "u" + usersListWorkingOut[i].Id;

                        int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                        int indexCol = GetDataGridColumnIndexFromKey(dataGrid, usersListWorkingOut[i].WorkingOutList[j].ShiftDate);

                        if (indexRow != -1 && indexCol != -1)
                        {
                            int nextCol = 0;

                            if (values[0] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                                nextCol++;
                            }

                            if (values[1] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = percentWorkingOut.ToString("P1");
                                nextCol++;
                            }

                            if (values[2] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + nextCol].Value = GetBonusWorkingOut((int)Math.Round(timeWorkigOut));
                                nextCol++;
                            }                                
                        }
                    }));
                }

                float fullTimeWorkigOut = usersListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    string key = "u" + usersListWorkingOut[i].Id;

                    /*if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutList.Count * fullOutput - usersListWorkingOut[i].WorkingOutSumm);
                    }*/
                }));
            }
        }

        private async Task<List<User>> LoadShiftsListMonthAsync(DateTime selectDate)
        {
            ValueShifts valueShifts = new ValueShifts();
            ValueUsers usersValue = new ValueUsers();

            INISettings settings = new INISettings();

            List<int> equips = GetAllEquipsFromPagesList(pages);

            int countShifts = settings.GetCountShifts();
            bool givenShiftNumber = settings.GetGivenShiftNumber();

            List<User> usersListMonth = new List<User>();

            usersListMonth = await usersValue.LoadUsersListFromSelectMonth(equips, selectDate);

            usersListMonth = await valueShifts.LoadShiftsForSelectedMonth(usersListMonth, selectDate, countShifts, givenShiftNumber);

            //MessageBox.Show(selectDate.ToString() + ": " + usersListMonth.Count + " = " + usersList.Count);

            return usersListMonth;
        }

        private void AddWorkingTimeMonthUsersToListView(CancellationToken token, DoubleBufferedDataGridView dataGrid, List<User> usersListMonth, DateTime selectDate, List<string> values)
        {
            List<WorkingOut> equipsListWorkingOut = new List<WorkingOut>();
            List<WorkingOut> usersListWorkingOut = new List<WorkingOut>();

            List<WorkingOut> listWorkingOut = new List<WorkingOut>();

            //List<int> usersCurrent = new List<int>();
            INIView view = new INIView();
            ValueDateTime timeValues = new ValueDateTime();

            int fullOutput = view.GetNormTime();
            int countShifts = view.GetCountShifts();

            bool calculateShiftsInIdletime = view.GetCalculateShiftsInIdletime();

            //List<User> usersListMonth = LoadShiftsListMonth(selectDate, period);
            //List<User> usersListMonth = LoadShiftsListMonth(selectDate, period);

            string selectMonth = selectDate.ToString("MM.yyyy");

            /*float totalTimeWorkigOut = 0;
            float totalPercentWorkingOut = 0;*/

            for (int i = 0; i < usersListMonth.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                float totalTimeWorkigOut = 0;
                //float totalPercentWorkingOut = 0;
                float totalBonusWorkingOut = 0;

                List<float> totalPercentWorkingOutList = new List<float>();

                //MessageBox.Show(usersListMonth[i].Shifts.Count.ToString());
                if (usersListMonth[i].Shifts != null)
                {
                    for (int j = 0; j < usersListMonth[i].Shifts.Count; j++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        UserShift shift = usersListMonth[i].Shifts[j];

                        bool currentShift;// = CheckCurrentShift(shiftDate, shiftNumber);

                        if (shift.ShiftDateEnd == "")
                        {
                            currentShift = true;
                        }
                        else
                        {
                            currentShift = false;
                        }

                        if (shift.Orders != null)
                        {
                            int shiftNumber = shift.ShiftNumber;

                            List<int> currentEquipsList = new List<int>();

                            for (int k = 0; k < shift.Orders.Count; k++)
                            {
                                if (!currentEquipsList.Contains(shift.Orders[k].IdEquip))
                                {
                                    currentEquipsList.Add(shift.Orders[k].IdEquip);
                                }
                            }

                            for (int k = 0; k < currentEquipsList.Count; k++)
                            {
                                float timeWorkigOut = CalculateWorkTime(shift.Orders, currentEquipsList[k]);
                                float timeBacklog = fullOutput - timeWorkigOut;

                                usersListMonth[i].WorkingOutUser += timeWorkigOut;
                                usersListMonth[i].WorkingOutBacklog += timeBacklog;

                                float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

                                bool isThereOrdersInWorking = IsThereOrdersInWorking(shift.Orders, currentEquipsList[k]);
                                bool isThereOrdersInWorkingForAllEuips = IsThereOrdersInWorkingForAllEquips(shift.Orders);

                                if (!currentShift)
                                {
                                    totalTimeWorkigOut += timeWorkigOut;
                                    //totalPercentWorkingOut += percentWorkingOut;
                                    totalBonusWorkingOut += CalculateBonusWorkingOut(timeWorkigOut);

                                    int indexListWorkingOut = listWorkingOut.FindIndex(
                                                    (v) => v.Id == usersListMonth[i].Id &&
                                                           v.Equip == currentEquipsList[k]
                                                           );

                                    if (indexListWorkingOut != -1)
                                    {
                                        listWorkingOut[indexListWorkingOut].WorkingOutSumm += timeWorkigOut;
                                        listWorkingOut[indexListWorkingOut].BonusWorkingOutSumm += CalculateBonusWorkingOut(timeWorkigOut);

                                        if (isThereOrdersInWorking)
                                        {
                                            listWorkingOut[indexListWorkingOut].PercentsWorkingOut.Add(percentWorkingOut);
                                        }
                                    }
                                    else
                                    {
                                        listWorkingOut.Add(new WorkingOut(usersListMonth[i].Id, currentEquipsList[k]));

                                        listWorkingOut[listWorkingOut.Count - 1].WorkingOutSumm = timeWorkigOut;
                                        listWorkingOut[listWorkingOut.Count - 1].BonusWorkingOutSumm = CalculateBonusWorkingOut(timeWorkigOut);

                                        if (isThereOrdersInWorking)
                                        {
                                            listWorkingOut[listWorkingOut.Count - 1].PercentsWorkingOut.Add(percentWorkingOut);
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
                                        }
                                    }
                                    else
                                    {
                                        if (!currentShift)
                                        {
                                            equipsListWorkingOut[indexEquipsList].WorkingOutList.Add(new WorkingOutValue(
                                                shift.ShiftDate,
                                                shiftNumber,
                                                timeWorkigOut
                                                ));

                                            equipsListWorkingOut[indexEquipsList].NumberOfShiftsWorked++;
                                            //numberOfShiftsWorked++;

                                            if (!isThereOrdersInWorking)
                                            {
                                                equipsListWorkingOut[indexEquipsList].NumberOfIdleShifts++;

                                                /*if (!calculateShiftsInIdletime)
                                                {
                                                    numberOfShiftsWorked--;
                                                }*/
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
                                    if (!currentShift)
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

                                        equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                        equipsListWorkingOut[equipsListWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                                        equipsListWorkingOut[equipsListWorkingOut.Count - 1].NumberOfShiftsWorked++;
                                        //numberOfShiftsWorked++;

                                        if (!isThereOrdersInWorking)
                                        {
                                            equipsListWorkingOut[equipsListWorkingOut.Count - 1].NumberOfIdleShifts++;

                                            /*if (!calculateShiftsInIdletime)
                                            {
                                                numberOfShiftsWorked--;
                                            }*/
                                        }
                                    }
                                }

                                //Выработка для сотрудника
                                int indexUserList = usersListWorkingOut.FindIndex(
                                                        (v) => v.Id == usersListMonth[i].Id
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
                                        if (!currentShift)
                                        {
                                            usersListWorkingOut[indexUserList].WorkingOutList.Add(new WorkingOutValue(
                                                shift.ShiftDate,
                                                shiftNumber,
                                                timeWorkigOut
                                                ));
                                            usersListWorkingOut[indexUserList].NumberOfShiftsWorked++;

                                            if (!isThereOrdersInWorkingForAllEuips)// && timeWorkigOut > 0)
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
                                    if (!currentShift)
                                    {
                                        usersListWorkingOut.Add(new WorkingOut(
                                            usersListMonth[i].Id
                                            ));

                                        usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutList = new List<WorkingOutValue>
                                        {
                                            new WorkingOutValue(
                                                shift.ShiftDate,
                                                shiftNumber,
                                                timeWorkigOut
                                            )
                                        };

                                        usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                        usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                                        //usersListWorkingOut[usersListWorkingOut.Count - 1].WorkingOutBacklog += fullOutput - timeWorkigOut;
                                        usersListWorkingOut[usersListWorkingOut.Count - 1].NumberOfShiftsWorked++;

                                        if (!isThereOrdersInWorkingForAllEuips)// && timeWorkigOut > 0)
                                        {
                                            usersListWorkingOut[usersListWorkingOut.Count - 1].NumberOfIdleShifts++;
                                        }
                                    }
                                }







                                Invoke(new Action(() =>
                                {
                                    string key = CreateNameListViewItem(currentEquipsList[k], usersListMonth[i].Id);

                                    /*DataGridViewRow row;
                                    row = dataGrid.Rows[key];

                                    int indexRow = row.Index;*/
                                    int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                                    int indexCol = GetDataGridColumnIndexFromKey(dataGrid, selectMonth);

                                    int indexListWorkingOut = listWorkingOut.FindIndex(
                                                    (v) => v.Id == usersListMonth[i].Id &&
                                                           v.Equip == currentEquipsList[k]
                                                           );

                                    float workigOutSumm = 0;
                                    float bonusWorkingOutSumm = 0;
                                    float percentWorkingOutAverage = 0;

                                    if (indexListWorkingOut != -1)
                                    {
                                        workigOutSumm = listWorkingOut[indexListWorkingOut].WorkingOutSumm;
                                        bonusWorkingOutSumm = listWorkingOut[indexListWorkingOut].BonusWorkingOutSumm;
                                        percentWorkingOutAverage = listWorkingOut[indexListWorkingOut].PercentsWorkingOut.Sum() / listWorkingOut[indexListWorkingOut].PercentsWorkingOut.Count;
                                    }

                                    if (indexRow != -1 && indexCol != -1)
                                    {
                                        int nextCol = 0;

                                        //dataGrid.Rows[indexRow].Cells[(day) * countShifts * countOutValue + shiftNumber * countOutValue - 1].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                                        if (values[0] == "1")
                                        {
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Style.ForeColor = Color.Gray;
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(workigOutSumm));
                                            nextCol++;
                                        }

                                        if (values[1] == "1")
                                        {
                                            //dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = (totalPercentWorkingOut / numberOfShiftsWorked).ToString("P1"); //(usersList[i].WorkingOutUser / (usersList[i].WorkingOutUser + usersList[i].WorkingOutBacklog))
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Style.ForeColor = Color.Gray;
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = percentWorkingOutAverage.ToString("P1");
                                            nextCol++;
                                        }

                                        if (values[2] == "1")
                                        {
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Bold);
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Style.ForeColor = Color.Gray;
                                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = bonusWorkingOutSumm.ToString("P0");
                                            nextCol++;
                                        }

                                        //dataGrid.Rows[indexRow].Cells[period * countOutValue * countShifts + 2].Value = timeValues.MinuteToTimeString((int)Math.Round(usersListMonth[i].WorkingOutUser));
                                        //dataGrid.Rows[indexRow].Cells[days * countShifts + 3].Value = timeValues.MinuteToTimeString(usersListMonth[i].WorkingOutBacklog);
                                    }
                                }
                                ));
                            }
                        }
                    }
                }

                //for (int l = 0; l < usersListWorkingOut.Count; l++)
                /*{
                    int indexUserList = usersListWorkingOut.FindIndex(
                                                    (v) => v.Id == usersListMonth[i].Id
                                                           );
                    if (indexUserList > -1)
                    {
                        numberOfShiftsWorked = usersListWorkingOut[indexUserList].NumberOfShiftsWorked - usersListWorkingOut[indexUserList].NumberOfIdleShifts;
                    }
                    else
                    {
                        numberOfShiftsWorked = 1;
                    }
                }*/

                //здесь была вставка выработким
            }

            for (int i = 0; i < equipsListWorkingOut.Count; i++)
            {
                //int countDaysFromMonth = 0;
                float totalTimeWorkigOut = 0;
                float totalPercentWorkingOut = 0;
                float totalBonusWorkingOut = 0;

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
                    float timeWorkigOut = equipsListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    totalTimeWorkigOut += timeWorkigOut;
                    totalPercentWorkingOut += GetPercentWorkingOut(fullOutput, timeWorkigOut);
                    totalBonusWorkingOut += CalculateBonusWorkingOut(timeWorkigOut);
                }

                Invoke(new Action(() =>
                {
                    string key = "e" + equipsListWorkingOut[i].Id;

                    int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                    int indexCol = GetDataGridColumnIndexFromKey(dataGrid, selectMonth);

                    if (indexRow != -1 && indexCol != -1)
                    {
                        int nextCol = 0;

                        if (values[0] == "1")
                        {
                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(totalTimeWorkigOut));
                            nextCol++;
                        }

                        if (values[1] == "1")
                        {
                             dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = (totalPercentWorkingOut / (numberOfShiftsWorkedEquips - numberOfIdleShiftsEquips)).ToString("P1");
                            nextCol++;
                        }

                        if (values[2] == "1")
                        {
                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = totalBonusWorkingOut.ToString("P0");
                            nextCol++;
                        }
                    }
                }));

                float fullTimeWorkigOut = equipsListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    //string key = "e" + equipsListWorkingOut[i].Id;

                    /*string key = CreateNameListViewItem(usersListMonth[i].Equip, usersListMonth[i].Id);

                    int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                    int indexCol = GetDataGridColumnIndexFromKey(dataGrid, selectMonth);*/

                    /*if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[days * countShifts + 2].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[days * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutList.Count * fullOutput - equipsListWorkingOut[i].WorkingOutSumm);
                    }*/

                }));
            }

            for (int i = 0; i < usersListWorkingOut.Count; i++)
            {
                float totalTimeWorkigOut = 0;
                float totalPercentWorkingOut = 0;
                float totalBonusWorkingOut = 0;

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
                    float timeWorkigOut = usersListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    totalTimeWorkigOut += timeWorkigOut;
                    totalPercentWorkingOut += GetPercentWorkingOut(fullOutput, (int)timeWorkigOut);
                    totalBonusWorkingOut += CalculateBonusWorkingOut(timeWorkigOut);
                }
                
                Invoke(new Action(() =>
                {
                    string key = "u" + usersListWorkingOut[i].Id;

                    int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                    int indexCol = GetDataGridColumnIndexFromKey(dataGrid, selectMonth);

                    if (indexRow != -1 && indexCol != -1)
                    {
                        int nextCol = 0;

                        if (values[0] == "1")
                        {
                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(totalTimeWorkigOut));
                            nextCol++;
                        }

                        if (values[1] == "1")
                        {
                            //
                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = (totalPercentWorkingOut / (numberOfShiftsWorkedUsers - numberOfIdleShiftsUsers)).ToString("P1");
                            nextCol++;
                        }

                        if (values[2] == "1")
                        {
                            dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = totalBonusWorkingOut.ToString("P0");
                            nextCol++;
                        }
                    }
                }));

                float fullTimeWorkigOut = usersListWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    string key = "u" + usersListWorkingOut[i].Id;

                    /*if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutList.Count * fullOutput - usersListWorkingOut[i].WorkingOutSumm);
                    }*/
                }));
            }
        }

        /*private void AddWorkingTimeMonthUsersToListViewOLD(CancellationToken token, DoubleBufferedDataGridView dataGrid, List<User> usersListMonth, DateTime selectDate, List<string> values)
        {
            List<WorkingOut> equipsListMonthWorkingOut = new List<WorkingOut>();
            List<WorkingOut> usersListMonthWorkingOut = new List<WorkingOut>();
            //List<int> usersCurrent = new List<int>();
            INIView view = new INIView();
            ValueDateTime timeValues = new ValueDateTime();

            int fullOutput = view.GetNormTime();
            int countShifts = view.GetCountShifts();

            //List<User> usersListMonth = LoadShiftsListMonth(selectDate, period);
            //List<User> usersListMonth = LoadShiftsListMonth(selectDate, period);

            string selectMonth = selectDate.ToString("MM.yyyy");

            *//*float totalTimeWorkigOut = 0;
            float totalPercentWorkingOut = 0;*//*

            for (int i = 0; i < usersListMonth.Count; i++)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                float totalTimeWorkigOut = 0;
                float totalPercentWorkingOut = 0;

                //MessageBox.Show(usersListMonth[i].Shifts.Count.ToString());
                if (usersListMonth[i].Shifts != null)
                {
                    for (int j = 0; j < usersListMonth[i].Shifts.Count; j++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        UserShift shift = usersListMonth[i].Shifts[j];

                        if (shift.Orders != null)
                        {
                            int shiftNumber = shift.ShiftNumber;

                            float timeWorkigOut = CalculateWorkTime(shift.Orders);
                            float timeBacklog = fullOutput - timeWorkigOut;

                            usersListMonth[i].WorkingOutUser += timeWorkigOut;
                            usersListMonth[i].WorkingOutBacklog += timeBacklog;

                            float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

                            totalTimeWorkigOut += timeWorkigOut;
                            totalPercentWorkingOut += percentWorkingOut;

                            //Выработка для оборудования
                            int indexEquipsList = equipsListMonthWorkingOut.FindIndex(
                                                    (v) => v.Id == usersListMonth[i].Equip
                                                           );

                            if (indexEquipsList != -1)
                            {
                                int indexEquipsListWOut = equipsListMonthWorkingOut[indexEquipsList].WorkingOutList.FindIndex(
                                                    (v) => v.ShiftDate == shift.ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                                if (indexEquipsListWOut != -1)
                                {
                                    equipsListMonthWorkingOut[indexEquipsList].WorkingOutList[indexEquipsListWOut].WorkingOut += timeWorkigOut;
                                }
                                else
                                {
                                    equipsListMonthWorkingOut[indexEquipsList].WorkingOutList.Add(new WorkingOutValue(
                                    shift.ShiftDate,
                                    shiftNumber,
                                    timeWorkigOut
                                    ));

                                    //equipsList[indexEquipsList].EquipsWOut[equipsList[indexEquipsList].EquipsWOut.Count - 1].WorkingOut 
                                }

                                equipsListMonthWorkingOut[indexEquipsList].WorkingOutSumm += timeWorkigOut;
                                equipsListMonthWorkingOut[indexEquipsList].WorkingOutBacklog += timeBacklog;

                            }
                            else
                            {
                                equipsListMonthWorkingOut.Add(new WorkingOut(
                                    usersListMonth[i].Equip
                                    ));

                                equipsListMonthWorkingOut[equipsListMonthWorkingOut.Count - 1].WorkingOutList = new List<WorkingOutValue>
                                {
                                    new WorkingOutValue(
                                        shift.ShiftDate,
                                        shiftNumber,
                                        timeWorkigOut
                                    )
                                };

                                equipsListMonthWorkingOut[equipsListMonthWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                equipsListMonthWorkingOut[equipsListMonthWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                            }

                            //Выработка для сотрудника
                            int indexUserList = usersListMonthWorkingOut.FindIndex(
                                                    (v) => v.Id == usersListMonth[i].Id
                                                           );

                            if (indexUserList != -1)
                            {
                                int indexUserListWOut = usersListMonthWorkingOut[indexUserList].WorkingOutList.FindIndex(
                                                    (v) => v.ShiftDate == shift.ShiftDate &&
                                                           v.ShiftNumber == shiftNumber
                                                           );

                                if (indexUserListWOut != -1)
                                {
                                    usersListMonthWorkingOut[indexUserList].WorkingOutList[indexUserListWOut].WorkingOut += timeWorkigOut;
                                }
                                else
                                {
                                    usersListMonthWorkingOut[indexUserList].WorkingOutList.Add(new WorkingOutValue(
                                    shift.ShiftDate,
                                    shiftNumber,
                                    timeWorkigOut
                                    ));

                                    //equipsList[indexEquipsList].EquipsWOut[equipsList[indexEquipsList].EquipsWOut.Count - 1].WorkingOut 
                                }

                                usersListMonthWorkingOut[indexUserList].WorkingOutSumm += timeWorkigOut;
                                //usersListMonthWorkingOut[indexUserList].WorkingOutBacklog += timeBacklog;
                                usersListMonthWorkingOut[indexUserList].WorkingOutBacklog += fullOutput - timeWorkigOut;
                            }
                            else
                            {
                                usersListMonthWorkingOut.Add(new WorkingOut(
                                    usersListMonth[i].Id
                                    ));

                                usersListMonthWorkingOut[usersListMonthWorkingOut.Count - 1].WorkingOutList = new List<WorkingOutValue>
                                {
                                    new WorkingOutValue(
                                        shift.ShiftDate,
                                        shiftNumber,
                                        timeWorkigOut
                                    )
                                };

                                usersListMonthWorkingOut[usersListMonthWorkingOut.Count - 1].WorkingOutSumm += timeWorkigOut;
                                //usersListMonthWorkingOut[usersListMonthWorkingOut.Count - 1].WorkingOutBacklog += timeBacklog;
                                usersListMonthWorkingOut[usersListMonthWorkingOut.Count - 1].WorkingOutBacklog += fullOutput - timeWorkigOut;
                            }

                            Invoke(new Action(() =>
                            {
                                string key = CreateNameListViewItem(usersListMonth[i].Equip, usersListMonth[i].Id);

                                *//*DataGridViewRow row;
                                row = dataGrid.Rows[key];

                                int indexRow = row.Index;*//*
                                int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                                int indexCol = GetDataGridColumnIndexFromKey(dataGrid, selectMonth);

                                if (indexRow != -1 && indexCol != -1)
                                {
                                    int nextCol = 0;

                                    //dataGrid.Rows[indexRow].Cells[(day) * countShifts * countOutValue + shiftNumber * countOutValue - 1].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                                    if (values[0] == "1")
                                    {
                                        dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(totalTimeWorkigOut));
                                        nextCol++;
                                    }

                                    if (values[1] == "1")
                                    {
                                        dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = totalPercentWorkingOut.ToString("P1");
                                        nextCol++;
                                    }

                                    if (values[2] == "1")
                                    {
                                        dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = GetBonusWorkingOut((int)Math.Round(totalTimeWorkigOut));
                                        nextCol++;
                                    }

                                    //dataGrid.Rows[indexRow].Cells[period * countOutValue * countShifts + 2].Value = timeValues.MinuteToTimeString((int)Math.Round(usersListMonth[i].WorkingOutUser));
                                    //dataGrid.Rows[indexRow].Cells[days * countShifts + 3].Value = timeValues.MinuteToTimeString(usersListMonth[i].WorkingOutBacklog);
                                }
                            }
                            ));
                        }
                    }
                }
            }

            for (int i = 0; i < equipsListMonthWorkingOut.Count; i++)
            {
                //int countDaysFromMonth = 0;
                float totalTimeWorkigOut = 0;
                float totalPercentWorkingOut = 0;
                float totalBonusWorkingOut = 0;

                for (int j = 0; j < equipsListMonthWorkingOut[i].WorkingOutList.Count; j++)
                {
                    int shiftNumber = equipsListMonthWorkingOut[i].WorkingOutList[j].ShiftNumber;
                    float timeWorkigOut = equipsListMonthWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(fullOutput, timeWorkigOut);

                    totalTimeWorkigOut += timeWorkigOut;
                    totalPercentWorkingOut += percentWorkingOut;
                    totalBonusWorkingOut += CalculateBonusWorkingOut(timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "e" + equipsListMonthWorkingOut[i].Id;

                        int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                        int indexCol = GetDataGridColumnIndexFromKey(dataGrid, selectMonth);

                        if (indexRow != -1 && indexCol != -1)
                        {
                            int nextCol = 0;

                            if (values[0] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(totalTimeWorkigOut));
                                nextCol++;
                            }

                            if (values[1] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = percentWorkingOut.ToString("P1");
                                nextCol++;
                            }

                            if (values[2] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = totalBonusWorkingOut.ToString("P0");
                                nextCol++;
                            }
                        }
                    }));
                }

                float fullTimeWorkigOut = equipsListMonthWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    string key = "e" + equipsListMonthWorkingOut[i].Id;

                    *//*if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[days * countShifts + 2].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[days * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutList.Count * fullOutput - equipsListWorkingOut[i].WorkingOutSumm);
                    }*//*

                }));
            }

            for (int i = 0; i < usersListMonthWorkingOut.Count; i++)
            {
                for (int j = 0; j < usersListMonthWorkingOut[i].WorkingOutList.Count; j++)
                {
                    int shiftNumber = usersListMonthWorkingOut[i].WorkingOutList[j].ShiftNumber;
                    float timeWorkigOut = usersListMonthWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(fullOutput, (int)timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "u" + usersListMonthWorkingOut[i].Id;

                        int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                        int indexCol = GetDataGridColumnIndexFromKey(dataGrid, selectMonth);

                        if (indexRow != -1 && indexCol != -1)
                        {
                            int nextCol = 0;

                            if (values[0] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = timeValues.MinuteToTimeString((int)Math.Round(timeWorkigOut));
                                nextCol++;
                            }

                            if (values[1] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = percentWorkingOut.ToString("P1");
                                nextCol++;
                            }

                            if (values[2] == "1")
                            {
                                dataGrid.Rows[indexRow].Cells[indexCol + nextCol].Value = GetBonusWorkingOut((int)Math.Round(timeWorkigOut));
                                nextCol++;
                            }
                        }
                    }));
                }

                float fullTimeWorkigOut = usersListMonthWorkingOut[i].WorkingOutSumm;

                Invoke(new Action(() =>
                {
                    string key = "u" + usersListMonthWorkingOut[i].Id;

                    *//*if (rowIndexes.ContainsKey(key))
                    {
                        int indexRow = rowIndexes[key];

                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 2].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutSumm);
                        //dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(equipsListWorkingOut[i].WorkingOutBacklog);
                        dataGridView1.Rows[indexRow].Cells[countDaysFromMonth * countShifts + 3].Value = timeValues.MinuteToTimeString(usersListWorkingOut[i].WorkingOutList.Count * fullOutput - usersListWorkingOut[i].WorkingOutSumm);
                    }*//*
                }));
            }
        }*/

        private int GetDataGridRowIndexFromKey(DataGridView dataGrid, string key)
        {
            int result = -1;

            for (int f = 0; f < dataGrid.Rows.Count; f++)
            {
                if (dataGrid.Rows[f].HeaderCell.Value != null)
                {
                    if (dataGrid.Rows[f].HeaderCell.Value.ToString() == key)
                    {
                        result = f;
                        break;
                    }
                }
            }

            return result;
        }

        private int GetDataGridColumnIndexFromKey(DataGridView dataGrid, string key)
        {
            int result = -1;

            DataGridViewColumn pColumn;
            pColumn = dataGrid.Columns[key];

            if (pColumn != null)
            {
                result = pColumn.Index;
            }

            return result;
        }

        private bool IsThereOrdersInWorking(List<UserShiftOrder> orders, int equip)
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

            result = (float)facticalWorkingOut / targetWorkingOut;

            return result;
        }

        private string GetBonusWorkingOut(float wOut)
        {
            float result = CalculateBonusWorkingOut(wOut);

            if (wOut < 600)
            {
                return "";
            }
            else
            {
                return result.ToString("P0");
            }
        }

        private float CalculateBonusWorkingOut(float wOut)
        {
            float result = 0;

            if (wOut < 600)
            {
                result = 0f;
            }
            if (wOut >= 600 && wOut < 630)
            {
                result = 0.1f;
            }
            else if (wOut >= 630 && wOut < 660)
            {
                result = 0.12f;
            }
            else if (wOut >= 660 && wOut < 720)
            {
                result = 0.15f;
            }
            else if (wOut >= 720)
            {
                result = 0.2f;
            }

            return result;
        }

        private string CreateNameListViewItem(int equip, int user)
        {
            return "e" + equip + "u" + user;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //StartCheckUpdate();
            StartTaskUpdateApplication();
            await UpdatePagesListsFromFileAsync();
            metroSetTabControl1.UseAnimation = false;
            metroSetTabControl1.AnimateEasingType = MetroSet_UI.Enums.EasingType.CubeOut;
            metroSetTabControl1.AnimateTime = 3000;
            timer1.Enabled = true;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await SelectNextPageAsync();

            DateTime currentDateTime = DateTime.Now;

            if (currentDateTime.Second == 0)
            {
                StartTaskUpdateApplication();
            }
        }
    }
}
