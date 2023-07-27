﻿using MetroSet_UI.Forms;
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

        List<User> usersList;

        List<Page> pages;

        int countOutValue = 3;

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

        private void StartCheckUpdate()
        {
            CreateFolder();

            string pathTemp = @"TempDownload";

            string fileTemp = "changlog.txt";

            string link = "https://drive.google.com/uc?export=download&id=1gMfdWRsRONkljPkjlQt90vwPTF3kqL9w";

            var task = Task.Run(() => CheckUpdate(link, pathTemp + "\\" + fileTemp));

        }

        private void CheckUpdate(string link, string path)
        {
            FileDownloader downloader = new FileDownloader();
            INIView ini = new INIView();

            string[] chLog = null;

            int lastDateV = 0;
            int currentDateV = 0;

            string lastDateVersion = ini.GetLastDateVersion();
            string currentDateVersion = "";

            try
            {
                var p = new Process();
                p.StartInfo.FileName = "Update.exe";
                p.StartInfo.Arguments = "update";

                downloader.DownloadFile(link, path);
                //downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
                //downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;

                chLog = File.ReadAllLines(path, Encoding.UTF8);
                currentDateVersion = chLog[0].Substring(7);

                if (currentDateVersion != "")
                    currentDateV = Convert.ToInt32(currentDateVersion);

                if (lastDateVersion != "")
                {
                    lastDateV = Convert.ToInt32(lastDateVersion);


                    if (currentDateV > lastDateV)
                    {
                        p.Start();
                    }
                }
                else
                {
                    p.Start();
                }

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
            }
        }

        private void LoadUsersList(List<int> equips, DateTime startDate, int countDays)
        {
            try
            {
                usersList = new List<User>();

                ValueUsers usersValue = new ValueUsers();

                //usersList = usersValue.LoadUsersList(equips, date);
                //usersList = usersValue.LoadUsersListFromLastAnyDays(equips, countDays);
                usersList = usersValue.LoadUsersListFromLastAnyDays(equips, startDate, countDays);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void LoadShiftsForUsersList(DateTime startDate, int countDays)
        {
            try
            {
                ValueShifts valueShifts = new ValueShifts();
                INIView view = new INIView();

                int countShifts = view.GetCountShifts();

                usersList = valueShifts.LoadShifts(usersList, startDate, countDays, countShifts);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }

        private void ReloadDataFromBase(List<Page> pagesList, DateTime startDate, int countDays)
        {
            List<int> equips = GetAllEquipsFromPagesList(pagesList);

            LoadAllUsers();
            LoadMachine();
            LoadUsersList(equips, startDate, countDays);
            LoadShiftsForUsersList(startDate, countDays);
        }

        private List<Page> LoadPagesList()
        {
            List<Page> pageList = new List<Page>();

            string startStrSection = "page_";

            IniFile ini = new IniFile("view.ini");

            string[] sections = ini.GetAllSections();

            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].StartsWith(startStrSection))
                {
                    if (ini.KeyExists("activePage", sections[i]))
                    {
                        if (ini.ReadBool("activePage", sections[i]))
                        {
                            int idCategory = Convert.ToInt32(sections[i].Substring(startStrSection.Length));

                            string name = ini.ReadString("name", sections[i]);
                            int typePage = ini.ReadInt("typePage", sections[i]);
                            int timeForView = ini.ReadInt("timeForView", sections[i]);
                            List<string> categoryesNames = ini.ReadString("categoryesNames", sections[i])?.Split(';')?.ToList();
                            List<string> categoryAndEquips = ini.ReadString("categoryAndEquips", sections[i])?.Split(';')?.ToList();
                            int typeLoad = ini.ReadInt("typeLoad", sections[i]);
                            string nameMediaFile = ini.ReadString("nameMediaFile", sections[i]);

                            pageList.Add(new Page(
                                idCategory,
                                typePage,
                                name,
                                timeForView,
                                categoryesNames,
                                categoryAndEquips,
                                typeLoad,
                                nameMediaFile
                                ));
                        }
                    }

                    //equipsForCategory = equipsStr?.Split(';')?.Select(Int32.Parse)?.ToList();

                    //List<string> strings = equipsStr?.Split(';')?.ToList();
                }
            }
            //categoryes.Add(Convert.ToInt32(sections[i].Substring(startStrSection.Length)));
            return pageList;
        }

        private List<int> GetAllEquipsFromPagesList(List<Page> pageList)
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

        private void UpdatePagesListsFromFile()
        {
            INIView view = new INIView();

            int period = view.GetPeriod();

            DateTime startDate = GetStartDate(period);

            pages = LoadPagesList();

            ReloadDataFromBase(pages, startDate, period);

            AddTabPageFromPageList(pages, period);
        }

        private void AddTabPageFromPageList(List<Page> pageList, int period)
        {
            for(int i = 0; i < pageList.Count; i++)
            {
                TabPage page = new TabPage();
                page.Name = pageList[i].Id.ToString();
                page.Text = pageList[i].Name;

                if (pageList[i].TypePage == 0)
                {
                    DateTime startPeriod = GetStartDate(period);

                    DoubleBufferedDataGridView dataGrid = CreatGridView(i, startPeriod, period);

                    ViewStatistic(dataGrid, pageList[i], period);

                    page.Controls.Add(dataGrid);
                }

                

                metroSetTabControl1.TabPages.Add(page);
            }
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

        private void ViewStatistic(DoubleBufferedDataGridView dataGrid, Page page, int period)
        {
            AddUsersToGridView(dataGrid, page);
            StartAddingWorkingTimeToListView(dataGrid, period);
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

        private DoubleBufferedDataGridView CreatGridView(int indexPage, DateTime startPeriod, int period)
        {
            DoubleBufferedDataGridView gridView = new DoubleBufferedDataGridView
            {
                Name = "gridView" + indexPage,
                Dock = DockStyle.Fill,
                ColumnHeadersVisible = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            string[] colCaption = { "Т", "%", "П" };

            INIView view = new INIView();

            int countShifts = view.GetCountShifts();

            gridView.Rows.Clear();
            gridView.Columns.Clear();

            List<string> nameCol = new List<string>();

            int width = gridView.Width;

            int w = view.GetWidthWorkingOutCol();

            int indexCol;

            indexCol = gridView.Columns.Add(@"index", @"");
            gridView.Columns[indexCol].Width = 45;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridView.Columns[indexCol].Frozen = true;

            indexCol = gridView.Columns.Add(@"name", @"");
            gridView.Columns[indexCol].Width = 300;
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
                gridView.Columns[indexCol].Width = w;
                gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                gridView.Columns[indexCol].Frozen = false;
            }

            indexCol = gridView.Columns.Add(@"totalWOut0", @"");
            gridView.Columns[indexCol].Width = 90;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridView.Columns[indexCol].Frozen = false;

            indexCol = gridView.Columns.Add(@"totalWOut1", @"");
            gridView.Columns[indexCol].Width = 90;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridView.Columns[indexCol].Frozen = false;

            indexCol = gridView.Columns.Add(@"totalWOut2", @"");
            gridView.Columns[indexCol].Width = 90;
            gridView.Columns[indexCol].SortMode = DataGridViewColumnSortMode.NotSortable;
            gridView.Columns[indexCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridView.Columns[indexCol].Frozen = false;

            int indexRow;

            indexRow = gridView.Rows.Add();
            gridView.Rows[indexRow].Frozen = true;

            for (int i = 2; i <= countShifts * period * countOutValue; i += countShifts * countOutValue)
            {
                AddCellToGrid(gridView, indexRow, i, countShifts * countOutValue);

                //gridView.Rows[indexRow].Cells[i].Value = ((i - 2 + countShifts * countOutValue) / (countShifts * countOutValue)).ToString("D2");
                gridView.Rows[indexRow].Cells[i].Value = nameCol[((i - 2) + countShifts * countOutValue) / (countShifts * countOutValue) - 1];
            }

            indexRow = gridView.Rows.Add();
            gridView.Rows[indexRow].Frozen = true;

            for (int i = 2; i <= countShifts * period * countOutValue + 1; i += countOutValue * countShifts)
            {
                for (int j = 1; j <= countShifts; j++)
                {
                    int n = i + (j - 1) * countOutValue;

                    AddCellToGrid(gridView, indexRow, n, countOutValue);

                    gridView.Rows[indexRow].Cells[n].Value = (j).ToString();
                }
            }

            indexRow = gridView.Rows.Add();
            gridView.Rows[indexRow].Frozen = true;

            for (int i = 2; i <= countShifts * period * countOutValue + 1; i += countOutValue)
            {
                for (int j = 2; j <= countShifts; j++)
                {
                    for (int k = 1; k <= countOutValue; k++)
                    {
                        int n = i + (k - 1);

                        AddCellToGrid(gridView, indexRow, n);

                        gridView.Rows[indexRow].Cells[n].Value = colCaption[k - 1];
                    }
                }
            }

            return gridView;
        }

        private void AddUsersToGridView(DoubleBufferedDataGridView dataGrid, Page page)
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
                        machine = "Оборудование " + machines[i];
                    }

                    AddItemToGrid(dataGrid, "e" + equips[i], "", machine, Color.Gray);

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

                            AddItemToGrid(dataGrid, CreateNameListViewItem(equips[i], usersList[j].Id), countUserForCurrentEquip.ToString(), user, color);
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

                    AddItemToGrid(dataGrid, "u" + usersCurrent[i], "", user, Color.Gray);

                    int countEquipForCurrentUser = 0;

                    for (int j = 0; j < equipsCurrent.Count; j++)
                    {
                        int index = usersList.FindIndex((v) => v.Id == usersCurrent[i] &&
                                                               v.Equip == equipsCurrent[j]);

                        if (index >= 0)
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

                            AddItemToGrid(dataGrid, CreateNameListViewItem(equipsCurrent[j], usersList[index].Id), countEquipForCurrentUser.ToString(), machine, color);
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

        private void StartAddingWorkingTimeToListView(DoubleBufferedDataGridView dataGrid, int period)
        {
            if (cancelTokenSource != null)
            {
                cancelTokenSource.Cancel();
            }

            cancelTokenSource = new CancellationTokenSource();
            AddWorkingTimeUsersToListView(cancelTokenSource.Token, dataGrid, period);
            Task taskDetails = new Task(() => AddWorkingTimeUsersToListView(cancelTokenSource.Token, dataGrid, period), cancelTokenSource.Token);
            //taskDetails.Start();
        }

        private void AddWorkingTimeUsersToListView(CancellationToken token, DoubleBufferedDataGridView dataGrid, int period)
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

                        if (usersList[i].Shifts[j].Orders != null)
                        {
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

                                /*DataGridViewRow row;
                                row = dataGrid.Rows[key];

                                int indexRow = row.Index;*/
                                int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                                int indexCol = GetDataGridColumnIndexFromKey(dataGrid, usersList[i].Shifts[j].ShiftDate);

                                if (indexRow != -1)
                                {
                                    //dataGrid.Rows[indexRow].Cells[(day) * countShifts * countOutValue + shiftNumber * countOutValue - 1].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                                    dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue)].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                                    dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + 1].Value = percentWorkingOut.ToString("P1");
                                    dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + 2].Value = GetBonusWorkingOut(timeWorkigOut);
                                    dataGrid.Rows[indexRow].Cells[period * countOutValue * countShifts + 2].Value = timeValues.MinuteToTimeString(usersList[i].WorkingOutUser);
                                    //dataGrid.Rows[indexRow].Cells[days * countShifts + 3].Value = timeValues.MinuteToTimeString(usersList[i].WorkingOutBacklog);
                                }
                            }));
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
                    int timeWorkigOut = equipsListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "e" + equipsListWorkingOut[i].Id;

                        int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                        int indexCol = GetDataGridColumnIndexFromKey(dataGrid, equipsListWorkingOut[i].WorkingOutList[j].ShiftDate);

                        if (indexRow != -1)
                        {
                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue)].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + 1].Value = percentWorkingOut.ToString("P1");
                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + 2].Value = GetBonusWorkingOut(timeWorkigOut);
                        }
                    }));
                }

                int fullTimeWorkigOut = equipsListWorkingOut[i].WorkingOutSumm;

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
                    int timeWorkigOut = usersListWorkingOut[i].WorkingOutList[j].WorkingOut;

                    float percentWorkingOut = GetPercentWorkingOut(650, timeWorkigOut);

                    Invoke(new Action(() =>
                    {
                        string key = "u" + usersListWorkingOut[i].Id;

                        int indexRow = GetDataGridRowIndexFromKey(dataGrid, key);
                        int indexCol = GetDataGridColumnIndexFromKey(dataGrid, usersListWorkingOut[i].WorkingOutList[j].ShiftDate);

                        if (indexRow != -1)
                        {
                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue)].Value = timeValues.MinuteToTimeString(timeWorkigOut);
                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + 1].Value = percentWorkingOut.ToString("P1");
                            dataGrid.Rows[indexRow].Cells[indexCol + ((shiftNumber - 1) * countOutValue) + 2].Value = GetBonusWorkingOut(timeWorkigOut);
                        }
                    }));
                }

                int fullTimeWorkigOut = usersListWorkingOut[i].WorkingOutSumm;

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

        private string GetBonusWorkingOut(int wOut)
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

            if (wOut < 600)
            {
                return "";
            }
            else
            {
                return result.ToString("P0");
            }
        }

        private string CreateNameListViewItem(int equip, int user)
        {
            return "e" + equip + "u" + user;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartCheckUpdate();
            UpdatePagesListsFromFile();
        }
    }
}
