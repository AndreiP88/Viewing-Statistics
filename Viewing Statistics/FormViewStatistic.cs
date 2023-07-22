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

namespace Viewing_Statistics
{
    public partial class Form1 : MetroSetForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, string> machines = new Dictionary<int, string>();

        List<User> usersList;

        List<Page> pages;

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
            INISettings ini = new INISettings();

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
                            string nameMediaFile = ini.ReadString("nameMediaFile", sections[i]);

                            pageList.Add(new Page(
                                idCategory,
                                typePage,
                                name,
                                timeForView,
                                categoryesNames,
                                categoryAndEquips,
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

        private void UpdatePagesListsFromFile()
        {
            pages = LoadPagesList();

            AddTabPageFromPageList(pages);
        }

        private void AddTabPageFromPageList(List<Page> pageList)
        {
            for(int i = 0; i < pageList.Count; i++)
            {
                TabPage page = new TabPage();
                page.Name = pageList[i].Id.ToString();
                page.Text = pageList[i].Name;
                page.Controls.Add(CreatGridView(i));

                metroSetTabControl1.TabPages.Add(page);
            }
        }

        private void ViewStatistic(Page page)
        {

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

        private DataGridView CreatGridView(int index)
        {
            DataGridView gridView = new DataGridView();
            gridView.Name = "gridView" + index;

            INISettings settings = new INISettings();

            int countShifts = settings.GetCountShifts();

            gridView.Rows.Clear();
            gridView.Columns.Clear();

            int width = gridView.Width;

            int w = 50;//(width - 560) / (days);

            DataGridViewColumn pColumn;
            string strTemp;

            gridView.Columns.Add(@"colGroup", @"");
            gridView.Columns[0].Width = 40;
            gridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            gridView.Columns.Add(@"colTask", @"Task");
            gridView.Columns[1].Width = 300;

            pColumn = gridView.Columns["colTask"];
            pColumn.Frozen = true;

            gridView.Rows.Add();
            gridView.Rows[0].Frozen = true;

            //AddCellToGrid(gridView, 0, 0, 2);
            gridView.Rows[0].Cells[0].Value = "";

            for (int i = 2; i <= countShifts; i += countShifts)
            {
                //AddCellToGrid(gridView, 0, i, countShifts);

                //gridView.Rows[0].Cells[i].Value = ((i - 2 + countShifts) / countShifts).ToString("D2");
            }

            gridView.Rows.Add();
            gridView.Rows[1].Frozen = true;


            for (int i = 2; i <= countShifts + 1; i += countShifts)
            {
                for (int j = 1; j <= countShifts; j++)
                {
                    int n = i + j - 1;

                    //AddCellToGrid(gridView, 1, n);

                    //gridView.Rows[1].Cells[n].Value = (j).ToString();
                }
            }

            return gridView;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartCheckUpdate();
            UpdatePagesListsFromFile();
        }
    }
}
