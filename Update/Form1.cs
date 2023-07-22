using libINIFile;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using File = System.IO.File;

namespace Update
{
    public partial class Form1 : Form
    {
        bool update = false;

        public Form1(string[] args)
        {
            InitializeComponent();

            if (args.Length > 0)
            {
                string param = args[0].Replace("-", "");

                if (param == "update")
                {
                    update = true;
                }
                else
                {
                    update = false;
                }

            }
        }

        bool openApplication = false;
        string currentVersion = "";

        private void CreateFolder()
        {
            string path = @"TempDownload";
            if (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden | FileAttributes.System;
            }
        }

        private void StartDownload(string link, string path)
        {
            //cancelTokenSource = new CancellationTokenSource();

            var task = Task.Run(() => Download(link, path));

            /*CancellationToken token = cancelTokenSource.Token;

            Task task = new Task(() => LoadDetailsMount(token));

            task.Start();*/
        }

        private void Download(string link, string path)
        {
            FileDownloader downloader = new FileDownloader();

            try
            {
                downloader.DownloadFile(link, path);
                Invoke(new Action(() =>
                {
                    downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
                    downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;

                }));
                /*downloader.DownloadProgressChanged += Downloader_DownloadProgressChanged;
                downloader.DownloadFileCompleted += Downloader_DownloadFileCompleted;*/
            }
            catch
            {
                MessageBox.Show("Ошибка подключения", "Ошибка", MessageBoxButtons.OK);
            }
        }

        private void Downloader_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Загрузка завершена");

            throw new NotImplementedException();
        }

        private void Downloader_DownloadProgressChanged(object sender, FileDownloader.DownloadProgress progress)
        {
            progressBar1.Value = progress.ProgressPercentage;

            throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            INISettings ini = new INISettings();

            CreateFolder();

            string path = @"TempDownload";

            string fileCL = "changlog.txt";

            StartDownload("https://drive.google.com/uc?export=download&id=1gMfdWRsRONkljPkjlQt90vwPTF3kqL9w", path + "\\" + fileCL);

            //webBrowser1.Url = new Uri("https://drive.google.com/uc?export=download&id=1YYbr30wiiSSwETsH8GIPFulWpebS6LeM");

            string[] chLog = File.ReadAllLines(path + "\\" + fileCL, Encoding.UTF8);

            currentVersion = chLog[0].Substring(7);
            //MessageBox.Show(currentVersion);

            textBox1.Lines = chLog;
            //textBox1.Text = File.ReadAllText(path + "\\" + fileCL, Encoding.Unicode);

            textBox1.DeselectAll();

            bool autoUpdate = Convert.ToBoolean(ini.GetAutoUpdate());

            if (autoUpdate && update)
            {
                DownloadProductivity();

                DownloadViewingStatistic();

                Close();
            }
        }

        private void CloseApp(string appName)
        {
            Process[] myProcList = Process.GetProcessesByName(appName);
            foreach (Process Target in myProcList)
            {
                Target.Kill();
            }

            Thread.Sleep(150);

            if (myProcList.Length != 0)
                openApplication = true;
            else
                openApplication = false;

            /*var processArray = Process.GetProcesses();
            var process = processArray.FirstOrDefault(p => p.ProcessName == appName);
            process?.Kill();*/
        }

        private void OpenApp(string appName)
        {
            Process.Start(appName);
        }

        private void DownloadProductivity()
        {
            INISettings ini = new INISettings();

            string path = @"TempDownload";

            string fileAPP = "Productivity.exe";

            Download("https://drive.google.com/uc?export=download&id=1gAr0D-ufN96cdoK9P7hOMatYuYzQx574", path + "\\" + fileAPP);

            CloseApp(Path.GetFileNameWithoutExtension(fileAPP));

            CopyFile(path + "\\" + fileAPP, fileAPP);

            ini.SetLastDateVersion(currentVersion);

            if (openApplication)
                OpenApp(fileAPP);
        }

        private void DownloadViewingStatistic()
        {
            INISettings ini = new INISettings();

            string path = @"TempDownload";

            string fileAPP = "Viewing Statistics.exe";

            Download("https://drive.google.com/uc?export=download&id=1gL4VfGAzx5AsPjGOmrELe09FNZbHrERE", path + "\\" + fileAPP);

            CloseApp(Path.GetFileNameWithoutExtension(fileAPP));

            CopyFile(path + "\\" + fileAPP, fileAPP);

            ini.SetLastDateVersion(currentVersion);

            if (openApplication)
                OpenApp(fileAPP);
        }

        private void CopyFile(string source, string dest)
        {
            if (File.Exists(source))
                File.Copy(source, dest, true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DownloadProductivity();

            DownloadViewingStatistic();

            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
