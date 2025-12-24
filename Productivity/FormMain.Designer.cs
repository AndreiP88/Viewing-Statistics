using System.ComponentModel;
using System.Windows.Forms;

namespace Productivity
{
    public class FormattedNumericUpDown : System.Windows.Forms.NumericUpDown
    {
        [Browsable(true)]
        public string Format { get; set; }
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (Format != null && Format.Length > 0)
                    base.Text = base.Value.ToString(Format);
                else base.Text = base.Value.ToString();
            }
        }
    }

    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        public class MyListView : System.Windows.Forms.ListView
        {
            public MyListView()
            {
                DoubleBuffered = true;
            }
        }

        class DoubleBufferedDataGridView : DataGridView
        {
            protected override bool DoubleBuffered { get => true; }
        }


        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.metroSetTabControl1 = new MetroSet_UI.Controls.MetroSetTabControl();
            this.metroSetSetTabPage1 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridViewOneShift = new Productivity.FormMain.DoubleBufferedDataGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.metroSetSwitch1 = new MetroSet_UI.Controls.MetroSetSwitch();
            this.metroSetLabel1 = new MetroSet_UI.Controls.MetroSetLabel();
            this.metroSetSwitch2 = new MetroSet_UI.Controls.MetroSetSwitch();
            this.metroSetLabel2 = new MetroSet_UI.Controls.MetroSetLabel();
            this.metroSetLabel3 = new MetroSet_UI.Controls.MetroSetLabel();
            this.formattedNumericUpDown4 = new Productivity.FormattedNumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.metroSetSetTabPage2 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new Productivity.FormMain.DoubleBufferedDataGridView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.button6 = new System.Windows.Forms.Button();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.metroSetSetTabPage6 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxStatType = new System.Windows.Forms.ComboBox();
            this.comboBoxStatCategory = new System.Windows.Forms.ComboBox();
            this.comboBoxStatMonth = new System.Windows.Forms.ComboBox();
            this.comboBoxStatYear = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel21 = new System.Windows.Forms.TableLayoutPanel();
            this.ListViewWorkingOutCategory = new Productivity.FormMain.MyListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader29 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel22 = new System.Windows.Forms.TableLayoutPanel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel23 = new System.Windows.Forms.TableLayoutPanel();
            this.labelCategoryStatisticCaption = new System.Windows.Forms.Label();
            this.labelCategoryStatisticValue = new System.Windows.Forms.Label();
            this.metroSetSetTabPage9 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel31 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel32 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonExcelExportyearStatistic = new System.Windows.Forms.Button();
            this.comboBoxStatYearTypeViewSelect = new System.Windows.Forms.ComboBox();
            this.comboBoxStatYearSelectYear = new System.Windows.Forms.ComboBox();
            this.buttonStatYearUpdate = new System.Windows.Forms.Button();
            this.comboBoxStatYearCategorySelect = new System.Windows.Forms.ComboBox();
            this.progressBarStatYear = new System.Windows.Forms.ProgressBar();
            this.comboBoxStatYearEquipSelect = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel33 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel34 = new System.Windows.Forms.TableLayoutPanel();
            this.chartStatYear = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel35 = new System.Windows.Forms.TableLayoutPanel();
            this.labelYearStatisticCaption = new System.Windows.Forms.Label();
            this.labelYearStatisticValue = new System.Windows.Forms.Label();
            this.dataGridViewYearStatistic = new System.Windows.Forms.DataGridView();
            this.metroSetSetTabPage7 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel24 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel25 = new System.Windows.Forms.TableLayoutPanel();
            this.metroSetLabel5 = new MetroSet_UI.Controls.MetroSetLabel();
            this.metroSetSwitchUserStatistic = new MetroSet_UI.Controls.MetroSetSwitch();
            this.comboBoxUserYears = new System.Windows.Forms.ComboBox();
            this.progressBarUserStatistic = new System.Windows.Forms.ProgressBar();
            this.comboBoxUserNames = new System.Windows.Forms.ComboBox();
            this.comboBoxUserCategory = new System.Windows.Forms.ComboBox();
            this.buttonUpdateUserStatistic = new System.Windows.Forms.Button();
            this.tableLayoutPanel26 = new System.Windows.Forms.TableLayoutPanel();
            this.ListViewUserWorking = new Productivity.FormMain.MyListView();
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader30 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel27 = new System.Windows.Forms.TableLayoutPanel();
            this.chartUserMakereadyCount = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartUserAmount = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartUserWorkingOutput = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.metroSetSetTabPage8 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel28 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel29 = new System.Windows.Forms.TableLayoutPanel();
            this.metroSetSwitch3 = new MetroSet_UI.Controls.MetroSetSwitch();
            this.comboBoxPlanCategory = new System.Windows.Forms.ComboBox();
            this.comboBoxPlanEquips = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.metroSetLabel4 = new MetroSet_UI.Controls.MetroSetLabel();
            this.tableLayoutPanel30 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewPlan = new Productivity.FormMain.MyListView();
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader27 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader28 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.metroSetSetTabPage5 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.dataGridViewOrderDetails = new Productivity.FormMain.DoubleBufferedDataGridView();
            this.metroSetSetTabPage3 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewCategory = new Productivity.FormMain.MyListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewEquips = new Productivity.FormMain.MyListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCatDown = new System.Windows.Forms.Button();
            this.buttonCatAdd = new System.Windows.Forms.Button();
            this.buttonCatEdit = new System.Windows.Forms.Button();
            this.buttonCatDelete = new System.Windows.Forms.Button();
            this.buttonCatUp = new System.Windows.Forms.Button();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonEquipAdd = new System.Windows.Forms.Button();
            this.buttonEquipDel = new System.Windows.Forms.Button();
            this.buttonEquipUp = new System.Windows.Forms.Button();
            this.buttonEquipDown = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.formattedNumericUpDown10 = new Productivity.FormattedNumericUpDown();
            this.formattedNumericUpDown11 = new Productivity.FormattedNumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.formattedNumericUpDown2 = new Productivity.FormattedNumericUpDown();
            this.formattedNumericUpDown1 = new Productivity.FormattedNumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.formattedNumericUpDown3 = new Productivity.FormattedNumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.metroSetCheckBox1 = new MetroSet_UI.Controls.MetroSetCheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.metroSetCheckBox2 = new MetroSet_UI.Controls.MetroSetCheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.metroSetCheckBox6 = new MetroSet_UI.Controls.MetroSetCheckBox();
            this.metroSetSetTabPage4 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonEditViewPath = new System.Windows.Forms.Button();
            this.buttonDelViewPath = new System.Windows.Forms.Button();
            this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.metroSetCheckBox3 = new MetroSet_UI.Controls.MetroSetCheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.formattedNumericUpDown5 = new Productivity.FormattedNumericUpDown();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.formattedNumericUpDown9 = new Productivity.FormattedNumericUpDown();
            this.formattedNumericUpDown8 = new Productivity.FormattedNumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.formattedNumericUpDown7 = new Productivity.FormattedNumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.formattedNumericUpDown6 = new Productivity.FormattedNumericUpDown();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.metroSetCheckBox4 = new MetroSet_UI.Controls.MetroSetCheckBox();
            this.metroSetCheckBox5 = new MetroSet_UI.Controls.MetroSetCheckBox();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSettingsSave = new System.Windows.Forms.Button();
            this.buttonSettingsReload = new System.Windows.Forms.Button();
            this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
            this.listViewPages = new Productivity.FormMain.MyListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonViewAdd = new System.Windows.Forms.Button();
            this.buttonViewEdit = new System.Windows.Forms.Button();
            this.buttonViewDel = new System.Windows.Forms.Button();
            this.buttonViewUp = new System.Windows.Forms.Button();
            this.buttonViewDown = new System.Windows.Forms.Button();
            this.metroSetControlBox1 = new MetroSet_UI.Controls.MetroSetControlBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.metroSetTabControl1.SuspendLayout();
            this.metroSetSetTabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOneShift)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown4)).BeginInit();
            this.metroSetSetTabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.metroSetSetTabPage6.SuspendLayout();
            this.tableLayoutPanel19.SuspendLayout();
            this.tableLayoutPanel20.SuspendLayout();
            this.tableLayoutPanel21.SuspendLayout();
            this.tableLayoutPanel22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableLayoutPanel23.SuspendLayout();
            this.metroSetSetTabPage9.SuspendLayout();
            this.tableLayoutPanel31.SuspendLayout();
            this.tableLayoutPanel32.SuspendLayout();
            this.tableLayoutPanel33.SuspendLayout();
            this.tableLayoutPanel34.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatYear)).BeginInit();
            this.tableLayoutPanel35.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewYearStatistic)).BeginInit();
            this.metroSetSetTabPage7.SuspendLayout();
            this.tableLayoutPanel24.SuspendLayout();
            this.tableLayoutPanel25.SuspendLayout();
            this.tableLayoutPanel26.SuspendLayout();
            this.tableLayoutPanel27.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartUserMakereadyCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartUserAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartUserWorkingOutput)).BeginInit();
            this.metroSetSetTabPage8.SuspendLayout();
            this.tableLayoutPanel28.SuspendLayout();
            this.tableLayoutPanel29.SuspendLayout();
            this.tableLayoutPanel30.SuspendLayout();
            this.metroSetSetTabPage5.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrderDetails)).BeginInit();
            this.metroSetSetTabPage3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown11)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown3)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.metroSetSetTabPage4.SuspendLayout();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel11.SuspendLayout();
            this.tableLayoutPanel12.SuspendLayout();
            this.tableLayoutPanel13.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown5)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown6)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tableLayoutPanel14.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroSetTabControl1
            // 
            this.metroSetTabControl1.AnimateEasingType = MetroSet_UI.Enums.EasingType.SineInOut;
            this.metroSetTabControl1.AnimateTime = 200;
            this.metroSetTabControl1.BackgroundColor = System.Drawing.Color.White;
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage1);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage2);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage6);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage9);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage7);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage8);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage5);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage3);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage4);
            this.metroSetTabControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetTabControl1.IsDerivedStyle = true;
            this.metroSetTabControl1.ItemSize = new System.Drawing.Size(130, 38);
            this.metroSetTabControl1.Location = new System.Drawing.Point(12, 90);
            this.metroSetTabControl1.Name = "metroSetTabControl1";
            this.metroSetTabControl1.SelectedIndex = 1;
            this.metroSetTabControl1.SelectedTextColor = System.Drawing.Color.White;
            this.metroSetTabControl1.Size = new System.Drawing.Size(1260, 510);
            this.metroSetTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.metroSetTabControl1.Speed = 100;
            this.metroSetTabControl1.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetTabControl1.StyleManager = null;
            this.metroSetTabControl1.TabIndex = 0;
            this.metroSetTabControl1.TabStyle = MetroSet_UI.Enums.TabStyle.Style2;
            this.metroSetTabControl1.ThemeAuthor = "Narwin";
            this.metroSetTabControl1.ThemeName = "MetroLite";
            this.metroSetTabControl1.UnselectedTextColor = System.Drawing.Color.Gray;
            this.metroSetTabControl1.UseAnimation = false;
            this.metroSetTabControl1.SelectedIndexChanged += new System.EventHandler(this.metroSetTabControl1_SelectedIndexChangedAsync);
            // 
            // metroSetSetTabPage1
            // 
            this.metroSetSetTabPage1.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage1.Controls.Add(this.tableLayoutPanel1);
            this.metroSetSetTabPage1.Font = null;
            this.metroSetSetTabPage1.ImageIndex = 0;
            this.metroSetSetTabPage1.ImageKey = null;
            this.metroSetSetTabPage1.IsDerivedStyle = true;
            this.metroSetSetTabPage1.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage1.Name = "metroSetSetTabPage1";
            this.metroSetSetTabPage1.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage1.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage1.StyleManager = null;
            this.metroSetSetTabPage1.TabIndex = 0;
            this.metroSetSetTabPage1.Text = "Детали смены";
            this.metroSetSetTabPage1.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage1.ThemeName = "MetroLite";
            this.metroSetSetTabPage1.ToolTipText = null;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewOneShift, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridViewOneShift
            // 
            this.dataGridViewOneShift.AllowUserToAddRows = false;
            this.dataGridViewOneShift.AllowUserToDeleteRows = false;
            this.dataGridViewOneShift.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridViewOneShift.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOneShift.ColumnHeadersVisible = false;
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridViewOneShift, 2);
            this.dataGridViewOneShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOneShift.Location = new System.Drawing.Point(3, 39);
            this.dataGridViewOneShift.MultiSelect = false;
            this.dataGridViewOneShift.Name = "dataGridViewOneShift";
            this.dataGridViewOneShift.ReadOnly = true;
            this.dataGridViewOneShift.RowHeadersVisible = false;
            this.dataGridViewOneShift.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewOneShift.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOneShift.Size = new System.Drawing.Size(1246, 422);
            this.dataGridViewOneShift.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 12;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.dateTimePicker1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBox1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetSwitch1, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetLabel1, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetSwitch2, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetLabel2, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetLabel3, 10, 0);
            this.tableLayoutPanel2.Controls.Add(this.formattedNumericUpDown4, 9, 0);
            this.tableLayoutPanel2.Controls.Add(this.button2, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonPreview, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonNext, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1246, 30);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePicker1.Location = new System.Drawing.Point(3, 3);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(174, 23);
            this.dateTimePicker1.TabIndex = 0;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChangedAsync);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(183, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(154, 24);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChangedAsync);
            // 
            // metroSetSwitch1
            // 
            this.metroSetSwitch1.BackColor = System.Drawing.Color.Transparent;
            this.metroSetSwitch1.BackgroundColor = System.Drawing.Color.Empty;
            this.metroSetSwitch1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(159)))), ((int)(((byte)(147)))));
            this.metroSetSwitch1.CheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitch1.CheckState = MetroSet_UI.Enums.CheckState.Checked;
            this.metroSetSwitch1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetSwitch1.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitch1.DisabledCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitch1.DisabledUnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitch1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetSwitch1.IsDerivedStyle = true;
            this.metroSetSwitch1.Location = new System.Drawing.Point(463, 3);
            this.metroSetSwitch1.Name = "metroSetSwitch1";
            this.metroSetSwitch1.Size = new System.Drawing.Size(58, 22);
            this.metroSetSwitch1.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSwitch1.StyleManager = null;
            this.metroSetSwitch1.Switched = true;
            this.metroSetSwitch1.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.metroSetSwitch1.TabIndex = 3;
            this.metroSetSwitch1.Text = "Автообновление";
            this.metroSetSwitch1.ThemeAuthor = "Narwin";
            this.metroSetSwitch1.ThemeName = "MetroLite";
            this.metroSetSwitch1.UnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetSwitch1.SwitchedChanged += new MetroSet_UI.Controls.MetroSetSwitch.SwitchedChangedEventHandler(this.metroSetSwitch1_SwitchedChanged);
            // 
            // metroSetLabel1
            // 
            this.metroSetLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetLabel1.IsDerivedStyle = true;
            this.metroSetLabel1.Location = new System.Drawing.Point(527, 0);
            this.metroSetLabel1.Name = "metroSetLabel1";
            this.metroSetLabel1.Size = new System.Drawing.Size(214, 30);
            this.metroSetLabel1.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetLabel1.StyleManager = null;
            this.metroSetLabel1.TabIndex = 4;
            this.metroSetLabel1.Text = "Автовыбор текущей смены";
            this.metroSetLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.metroSetLabel1.ThemeAuthor = "Narwin";
            this.metroSetLabel1.ThemeName = "MetroLite";
            // 
            // metroSetSwitch2
            // 
            this.metroSetSwitch2.BackColor = System.Drawing.Color.Transparent;
            this.metroSetSwitch2.BackgroundColor = System.Drawing.Color.Empty;
            this.metroSetSwitch2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(159)))), ((int)(((byte)(147)))));
            this.metroSetSwitch2.CheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitch2.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetSwitch2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetSwitch2.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitch2.DisabledCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitch2.DisabledUnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitch2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetSwitch2.IsDerivedStyle = true;
            this.metroSetSwitch2.Location = new System.Drawing.Point(907, 3);
            this.metroSetSwitch2.Name = "metroSetSwitch2";
            this.metroSetSwitch2.Size = new System.Drawing.Size(58, 22);
            this.metroSetSwitch2.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSwitch2.StyleManager = null;
            this.metroSetSwitch2.Switched = false;
            this.metroSetSwitch2.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.metroSetSwitch2.TabIndex = 5;
            this.metroSetSwitch2.Text = "metroSetSwitch2";
            this.metroSetSwitch2.ThemeAuthor = "Narwin";
            this.metroSetSwitch2.ThemeName = "MetroLite";
            this.metroSetSwitch2.UnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetSwitch2.SwitchedChanged += new MetroSet_UI.Controls.MetroSetSwitch.SwitchedChangedEventHandler(this.metroSetSwitch2_SwitchedChanged);
            // 
            // metroSetLabel2
            // 
            this.metroSetLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetLabel2.IsDerivedStyle = true;
            this.metroSetLabel2.Location = new System.Drawing.Point(971, 0);
            this.metroSetLabel2.Name = "metroSetLabel2";
            this.metroSetLabel2.Size = new System.Drawing.Size(134, 30);
            this.metroSetLabel2.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetLabel2.StyleManager = null;
            this.metroSetLabel2.TabIndex = 6;
            this.metroSetLabel2.Text = "Автообновление";
            this.metroSetLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.metroSetLabel2.ThemeAuthor = "Narwin";
            this.metroSetLabel2.ThemeName = "MetroLite";
            // 
            // metroSetLabel3
            // 
            this.metroSetLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetLabel3.IsDerivedStyle = true;
            this.metroSetLabel3.Location = new System.Drawing.Point(1171, 0);
            this.metroSetLabel3.Name = "metroSetLabel3";
            this.metroSetLabel3.Size = new System.Drawing.Size(114, 30);
            this.metroSetLabel3.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetLabel3.StyleManager = null;
            this.metroSetLabel3.TabIndex = 8;
            this.metroSetLabel3.Text = "мин.";
            this.metroSetLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.metroSetLabel3.ThemeAuthor = "Narwin";
            this.metroSetLabel3.ThemeName = "MetroLite";
            // 
            // formattedNumericUpDown4
            // 
            this.formattedNumericUpDown4.Format = null;
            this.formattedNumericUpDown4.Location = new System.Drawing.Point(1111, 3);
            this.formattedNumericUpDown4.Maximum = new decimal(new int[] {
            720,
            0,
            0,
            0});
            this.formattedNumericUpDown4.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.formattedNumericUpDown4.Name = "formattedNumericUpDown4";
            this.formattedNumericUpDown4.Size = new System.Drawing.Size(54, 23);
            this.formattedNumericUpDown4.TabIndex = 10;
            this.formattedNumericUpDown4.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.formattedNumericUpDown4.ValueChanged += new System.EventHandler(this.formattedNumericUpDown4_ValueChanged);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(747, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 24);
            this.button2.TabIndex = 12;
            this.button2.Text = "Обновление";
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_ClickAsync);
            // 
            // buttonPreview
            // 
            this.buttonPreview.ForeColor = System.Drawing.Color.Black;
            this.buttonPreview.Location = new System.Drawing.Point(343, 3);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(54, 23);
            this.buttonPreview.TabIndex = 13;
            this.buttonPreview.Text = "◄";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.ForeColor = System.Drawing.Color.Black;
            this.buttonNext.Location = new System.Drawing.Point(403, 3);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(54, 23);
            this.buttonNext.TabIndex = 14;
            this.buttonNext.Text = "►";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // metroSetSetTabPage2
            // 
            this.metroSetSetTabPage2.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage2.Controls.Add(this.tableLayoutPanel3);
            this.metroSetSetTabPage2.Font = null;
            this.metroSetSetTabPage2.ImageIndex = 0;
            this.metroSetSetTabPage2.ImageKey = null;
            this.metroSetSetTabPage2.IsDerivedStyle = true;
            this.metroSetSetTabPage2.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage2.Name = "metroSetSetTabPage2";
            this.metroSetSetTabPage2.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage2.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage2.StyleManager = null;
            this.metroSetSetTabPage2.TabIndex = 1;
            this.metroSetSetTabPage2.Text = "Детали месяца";
            this.metroSetSetTabPage2.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage2.ThemeName = "MetroLite";
            this.metroSetSetTabPage2.ToolTipText = null;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.ForeColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.tableLayoutPanel3.SetColumnSpan(this.dataGridView1, 2);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 39);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1246, 422);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 7;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.button6, 5, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBox4, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBox3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBox2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBox7, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.button1, 4, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1246, 30);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // button6
            // 
            this.button6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button6.ForeColor = System.Drawing.Color.Gray;
            this.button6.Location = new System.Drawing.Point(863, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(94, 24);
            this.button6.TabIndex = 8;
            this.button6.Text = "Excel";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // comboBox4
            // 
            this.comboBox4.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "Группировка по оборудованию",
            "Группировка по сотруднику"});
            this.comboBox4.Location = new System.Drawing.Point(243, 3);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(234, 24);
            this.comboBox4.TabIndex = 5;
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChangedAsync);
            // 
            // comboBox3
            // 
            this.comboBox3.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(123, 3);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(114, 24);
            this.comboBox3.TabIndex = 4;
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChangedAsync);
            // 
            // comboBox2
            // 
            this.comboBox2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(3, 3);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(114, 24);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChangedAsync);
            // 
            // comboBox7
            // 
            this.comboBox7.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Items.AddRange(new object[] {
            "Отображать выработку в часах",
            "Отображать выработку в процентах"});
            this.comboBox7.Location = new System.Drawing.Point(483, 3);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(274, 24);
            this.comboBox7.TabIndex = 7;
            this.comboBox7.SelectedIndexChanged += new System.EventHandler(this.comboBox7_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.ForeColor = System.Drawing.Color.Gray;
            this.button1.Location = new System.Drawing.Point(763, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "Обновить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_ClickAsync);
            // 
            // metroSetSetTabPage6
            // 
            this.metroSetSetTabPage6.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage6.Controls.Add(this.tableLayoutPanel19);
            this.metroSetSetTabPage6.Font = null;
            this.metroSetSetTabPage6.ImageIndex = 0;
            this.metroSetSetTabPage6.ImageKey = null;
            this.metroSetSetTabPage6.IsDerivedStyle = true;
            this.metroSetSetTabPage6.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage6.Name = "metroSetSetTabPage6";
            this.metroSetSetTabPage6.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage6.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage6.StyleManager = null;
            this.metroSetSetTabPage6.TabIndex = 5;
            this.metroSetSetTabPage6.Text = "Итоги участка за месяц";
            this.metroSetSetTabPage6.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage6.ThemeName = "MetroLite";
            this.metroSetSetTabPage6.ToolTipText = null;
            // 
            // tableLayoutPanel19
            // 
            this.tableLayoutPanel19.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel19.ColumnCount = 1;
            this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel19.Controls.Add(this.tableLayoutPanel20, 0, 0);
            this.tableLayoutPanel19.Controls.Add(this.tableLayoutPanel21, 0, 1);
            this.tableLayoutPanel19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel19.ForeColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel19.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel19.Name = "tableLayoutPanel19";
            this.tableLayoutPanel19.RowCount = 2;
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel19.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel19.TabIndex = 1;
            // 
            // tableLayoutPanel20
            // 
            this.tableLayoutPanel20.ColumnCount = 6;
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel20.Controls.Add(this.comboBoxStatType, 3, 0);
            this.tableLayoutPanel20.Controls.Add(this.comboBoxStatCategory, 2, 0);
            this.tableLayoutPanel20.Controls.Add(this.comboBoxStatMonth, 0, 0);
            this.tableLayoutPanel20.Controls.Add(this.comboBoxStatYear, 0, 0);
            this.tableLayoutPanel20.Controls.Add(this.button4, 4, 0);
            this.tableLayoutPanel20.Controls.Add(this.progressBar1, 5, 0);
            this.tableLayoutPanel20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel20.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel20.Name = "tableLayoutPanel20";
            this.tableLayoutPanel20.RowCount = 1;
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel20.Size = new System.Drawing.Size(1246, 30);
            this.tableLayoutPanel20.TabIndex = 2;
            // 
            // comboBoxStatType
            // 
            this.comboBoxStatType.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatType.FormattingEnabled = true;
            this.comboBoxStatType.Items.AddRange(new object[] {
            "Количество продукции",
            "Процент выработки",
            "Количество приладок",
            "Сумма времени приладок"});
            this.comboBoxStatType.Location = new System.Drawing.Point(483, 3);
            this.comboBoxStatType.Name = "comboBoxStatType";
            this.comboBoxStatType.Size = new System.Drawing.Size(234, 24);
            this.comboBoxStatType.TabIndex = 7;
            this.comboBoxStatType.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatType_SelectedIndexChanged);
            // 
            // comboBoxStatCategory
            // 
            this.comboBoxStatCategory.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatCategory.FormattingEnabled = true;
            this.comboBoxStatCategory.Location = new System.Drawing.Point(243, 3);
            this.comboBoxStatCategory.Name = "comboBoxStatCategory";
            this.comboBoxStatCategory.Size = new System.Drawing.Size(234, 24);
            this.comboBoxStatCategory.TabIndex = 5;
            this.comboBoxStatCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatCategory_SelectedIndexChanged);
            // 
            // comboBoxStatMonth
            // 
            this.comboBoxStatMonth.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatMonth.FormattingEnabled = true;
            this.comboBoxStatMonth.Location = new System.Drawing.Point(123, 3);
            this.comboBoxStatMonth.Name = "comboBoxStatMonth";
            this.comboBoxStatMonth.Size = new System.Drawing.Size(114, 24);
            this.comboBoxStatMonth.TabIndex = 4;
            this.comboBoxStatMonth.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatMonth_SelectedIndexChanged);
            // 
            // comboBoxStatYear
            // 
            this.comboBoxStatYear.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatYear.FormattingEnabled = true;
            this.comboBoxStatYear.Location = new System.Drawing.Point(3, 3);
            this.comboBoxStatYear.Name = "comboBoxStatYear";
            this.comboBoxStatYear.Size = new System.Drawing.Size(114, 24);
            this.comboBoxStatYear.TabIndex = 3;
            this.comboBoxStatYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatYear_SelectedIndexChanged);
            // 
            // button4
            // 
            this.button4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button4.ForeColor = System.Drawing.Color.Gray;
            this.button4.Location = new System.Drawing.Point(723, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(94, 24);
            this.button4.TabIndex = 6;
            this.button4.Text = "Обновить";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(823, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(420, 24);
            this.progressBar1.TabIndex = 8;
            // 
            // tableLayoutPanel21
            // 
            this.tableLayoutPanel21.ColumnCount = 2;
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel21.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel21.Controls.Add(this.ListViewWorkingOutCategory, 0, 0);
            this.tableLayoutPanel21.Controls.Add(this.tableLayoutPanel22, 0, 0);
            this.tableLayoutPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel21.Location = new System.Drawing.Point(3, 39);
            this.tableLayoutPanel21.Name = "tableLayoutPanel21";
            this.tableLayoutPanel21.RowCount = 1;
            this.tableLayoutPanel21.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel21.Size = new System.Drawing.Size(1246, 422);
            this.tableLayoutPanel21.TabIndex = 4;
            // 
            // ListViewWorkingOutCategory
            // 
            this.ListViewWorkingOutCategory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader29,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.ListViewWorkingOutCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewWorkingOutCategory.FullRowSelect = true;
            this.ListViewWorkingOutCategory.GridLines = true;
            this.ListViewWorkingOutCategory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewWorkingOutCategory.HideSelection = false;
            this.ListViewWorkingOutCategory.Location = new System.Drawing.Point(626, 3);
            this.ListViewWorkingOutCategory.MultiSelect = false;
            this.ListViewWorkingOutCategory.Name = "ListViewWorkingOutCategory";
            this.ListViewWorkingOutCategory.ShowItemToolTips = true;
            this.ListViewWorkingOutCategory.Size = new System.Drawing.Size(617, 416);
            this.ListViewWorkingOutCategory.TabIndex = 5;
            this.ListViewWorkingOutCategory.UseCompatibleStateImageBehavior = false;
            this.ListViewWorkingOutCategory.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "№";
            this.columnHeader3.Width = 30;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Оператор";
            this.columnHeader4.Width = 220;
            // 
            // columnHeader29
            // 
            this.columnHeader29.Text = "Смен";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Выработка";
            this.columnHeader6.Width = 85;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Доля в выработке";
            this.columnHeader7.Width = 130;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Премия";
            this.columnHeader8.Width = 70;
            // 
            // tableLayoutPanel22
            // 
            this.tableLayoutPanel22.ColumnCount = 1;
            this.tableLayoutPanel22.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel22.Controls.Add(this.chart1, 0, 0);
            this.tableLayoutPanel22.Controls.Add(this.tableLayoutPanel23, 0, 1);
            this.tableLayoutPanel22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel22.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel22.Name = "tableLayoutPanel22";
            this.tableLayoutPanel22.RowCount = 2;
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel22.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel22.Size = new System.Drawing.Size(617, 416);
            this.tableLayoutPanel22.TabIndex = 4;
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Transparent;
            this.chart1.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chart1.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Cursor = System.Windows.Forms.Cursors.Default;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 3);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(611, 326);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // tableLayoutPanel23
            // 
            this.tableLayoutPanel23.ColumnCount = 2;
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.Controls.Add(this.labelCategoryStatisticCaption, 0, 0);
            this.tableLayoutPanel23.Controls.Add(this.labelCategoryStatisticValue, 1, 0);
            this.tableLayoutPanel23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel23.Location = new System.Drawing.Point(3, 335);
            this.tableLayoutPanel23.Name = "tableLayoutPanel23";
            this.tableLayoutPanel23.RowCount = 3;
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel23.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel23.Size = new System.Drawing.Size(611, 78);
            this.tableLayoutPanel23.TabIndex = 1;
            // 
            // labelCategoryStatisticCaption
            // 
            this.labelCategoryStatisticCaption.AutoSize = true;
            this.labelCategoryStatisticCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCategoryStatisticCaption.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCategoryStatisticCaption.ForeColor = System.Drawing.Color.Black;
            this.labelCategoryStatisticCaption.Location = new System.Drawing.Point(3, 0);
            this.labelCategoryStatisticCaption.Name = "labelCategoryStatisticCaption";
            this.labelCategoryStatisticCaption.Size = new System.Drawing.Size(299, 38);
            this.labelCategoryStatisticCaption.TabIndex = 0;
            this.labelCategoryStatisticCaption.Text = "Сумма выработки:";
            this.labelCategoryStatisticCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCategoryStatisticValue
            // 
            this.labelCategoryStatisticValue.AutoSize = true;
            this.labelCategoryStatisticValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCategoryStatisticValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCategoryStatisticValue.ForeColor = System.Drawing.Color.Black;
            this.labelCategoryStatisticValue.Location = new System.Drawing.Point(308, 0);
            this.labelCategoryStatisticValue.Name = "labelCategoryStatisticValue";
            this.labelCategoryStatisticValue.Size = new System.Drawing.Size(300, 38);
            this.labelCategoryStatisticValue.TabIndex = 1;
            this.labelCategoryStatisticValue.Text = "0";
            this.labelCategoryStatisticValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // metroSetSetTabPage9
            // 
            this.metroSetSetTabPage9.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage9.Controls.Add(this.tableLayoutPanel31);
            this.metroSetSetTabPage9.Font = null;
            this.metroSetSetTabPage9.ImageIndex = 0;
            this.metroSetSetTabPage9.ImageKey = null;
            this.metroSetSetTabPage9.IsDerivedStyle = true;
            this.metroSetSetTabPage9.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage9.Name = "metroSetSetTabPage9";
            this.metroSetSetTabPage9.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage9.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage9.StyleManager = null;
            this.metroSetSetTabPage9.TabIndex = 8;
            this.metroSetSetTabPage9.Text = "Итоги участка за год";
            this.metroSetSetTabPage9.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage9.ThemeName = "MetroLite";
            this.metroSetSetTabPage9.ToolTipText = null;
            // 
            // tableLayoutPanel31
            // 
            this.tableLayoutPanel31.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel31.ColumnCount = 1;
            this.tableLayoutPanel31.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel31.Controls.Add(this.tableLayoutPanel32, 0, 0);
            this.tableLayoutPanel31.Controls.Add(this.tableLayoutPanel33, 0, 1);
            this.tableLayoutPanel31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel31.ForeColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel31.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel31.Name = "tableLayoutPanel31";
            this.tableLayoutPanel31.RowCount = 2;
            this.tableLayoutPanel31.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel31.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel31.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel31.TabIndex = 2;
            // 
            // tableLayoutPanel32
            // 
            this.tableLayoutPanel32.ColumnCount = 7;
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel32.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel32.Controls.Add(this.buttonExcelExportyearStatistic, 5, 0);
            this.tableLayoutPanel32.Controls.Add(this.comboBoxStatYearTypeViewSelect, 3, 0);
            this.tableLayoutPanel32.Controls.Add(this.comboBoxStatYearSelectYear, 0, 0);
            this.tableLayoutPanel32.Controls.Add(this.buttonStatYearUpdate, 4, 0);
            this.tableLayoutPanel32.Controls.Add(this.comboBoxStatYearCategorySelect, 1, 0);
            this.tableLayoutPanel32.Controls.Add(this.progressBarStatYear, 6, 0);
            this.tableLayoutPanel32.Controls.Add(this.comboBoxStatYearEquipSelect, 2, 0);
            this.tableLayoutPanel32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel32.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel32.Name = "tableLayoutPanel32";
            this.tableLayoutPanel32.RowCount = 1;
            this.tableLayoutPanel32.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel32.Size = new System.Drawing.Size(1246, 30);
            this.tableLayoutPanel32.TabIndex = 2;
            // 
            // buttonExcelExportyearStatistic
            // 
            this.buttonExcelExportyearStatistic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExcelExportyearStatistic.ForeColor = System.Drawing.Color.Gray;
            this.buttonExcelExportyearStatistic.Location = new System.Drawing.Point(1023, 3);
            this.buttonExcelExportyearStatistic.Name = "buttonExcelExportyearStatistic";
            this.buttonExcelExportyearStatistic.Size = new System.Drawing.Size(94, 24);
            this.buttonExcelExportyearStatistic.TabIndex = 9;
            this.buttonExcelExportyearStatistic.Text = "Excel";
            this.buttonExcelExportyearStatistic.UseVisualStyleBackColor = true;
            this.buttonExcelExportyearStatistic.Click += new System.EventHandler(this.button7_Click);
            // 
            // comboBoxStatYearTypeViewSelect
            // 
            this.comboBoxStatYearTypeViewSelect.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatYearTypeViewSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatYearTypeViewSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatYearTypeViewSelect.FormattingEnabled = true;
            this.comboBoxStatYearTypeViewSelect.Items.AddRange(new object[] {
            "Количество продукции",
            "Процент выработки",
            "Количество приладок",
            "Сумма времени приладок",
            "Количество отработанных смен",
            "Общее время выработки",
            "Сумма премии за выработку"});
            this.comboBoxStatYearTypeViewSelect.Location = new System.Drawing.Point(683, 3);
            this.comboBoxStatYearTypeViewSelect.Name = "comboBoxStatYearTypeViewSelect";
            this.comboBoxStatYearTypeViewSelect.Size = new System.Drawing.Size(234, 24);
            this.comboBoxStatYearTypeViewSelect.TabIndex = 7;
            this.comboBoxStatYearTypeViewSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatYearTypeViewSelect_SelectedIndexChangedAsync);
            // 
            // comboBoxStatYearSelectYear
            // 
            this.comboBoxStatYearSelectYear.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatYearSelectYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatYearSelectYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatYearSelectYear.FormattingEnabled = true;
            this.comboBoxStatYearSelectYear.Location = new System.Drawing.Point(3, 3);
            this.comboBoxStatYearSelectYear.Name = "comboBoxStatYearSelectYear";
            this.comboBoxStatYearSelectYear.Size = new System.Drawing.Size(114, 24);
            this.comboBoxStatYearSelectYear.TabIndex = 3;
            this.comboBoxStatYearSelectYear.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatYearSelectYear_SelectedIndexChanged);
            // 
            // buttonStatYearUpdate
            // 
            this.buttonStatYearUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStatYearUpdate.ForeColor = System.Drawing.Color.Gray;
            this.buttonStatYearUpdate.Location = new System.Drawing.Point(923, 3);
            this.buttonStatYearUpdate.Name = "buttonStatYearUpdate";
            this.buttonStatYearUpdate.Size = new System.Drawing.Size(94, 24);
            this.buttonStatYearUpdate.TabIndex = 6;
            this.buttonStatYearUpdate.Text = "Обновить";
            this.buttonStatYearUpdate.UseVisualStyleBackColor = true;
            this.buttonStatYearUpdate.Click += new System.EventHandler(this.buttonStatYearUpdate_Click);
            // 
            // comboBoxStatYearCategorySelect
            // 
            this.comboBoxStatYearCategorySelect.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatYearCategorySelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatYearCategorySelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatYearCategorySelect.FormattingEnabled = true;
            this.comboBoxStatYearCategorySelect.Location = new System.Drawing.Point(123, 3);
            this.comboBoxStatYearCategorySelect.Name = "comboBoxStatYearCategorySelect";
            this.comboBoxStatYearCategorySelect.Size = new System.Drawing.Size(234, 24);
            this.comboBoxStatYearCategorySelect.TabIndex = 5;
            this.comboBoxStatYearCategorySelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatYearCategorySelect_SelectedIndexChanged);
            // 
            // progressBarStatYear
            // 
            this.progressBarStatYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarStatYear.Location = new System.Drawing.Point(1123, 3);
            this.progressBarStatYear.Name = "progressBarStatYear";
            this.progressBarStatYear.Size = new System.Drawing.Size(120, 24);
            this.progressBarStatYear.TabIndex = 8;
            // 
            // comboBoxStatYearEquipSelect
            // 
            this.comboBoxStatYearEquipSelect.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxStatYearEquipSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxStatYearEquipSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStatYearEquipSelect.FormattingEnabled = true;
            this.comboBoxStatYearEquipSelect.Location = new System.Drawing.Point(363, 3);
            this.comboBoxStatYearEquipSelect.Name = "comboBoxStatYearEquipSelect";
            this.comboBoxStatYearEquipSelect.Size = new System.Drawing.Size(314, 24);
            this.comboBoxStatYearEquipSelect.TabIndex = 4;
            this.comboBoxStatYearEquipSelect.SelectedIndexChanged += new System.EventHandler(this.comboBoxStatYearEquipSelect_SelectedIndexChanged);
            // 
            // tableLayoutPanel33
            // 
            this.tableLayoutPanel33.ColumnCount = 1;
            this.tableLayoutPanel33.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel33.Controls.Add(this.tableLayoutPanel34, 0, 1);
            this.tableLayoutPanel33.Controls.Add(this.dataGridViewYearStatistic, 0, 0);
            this.tableLayoutPanel33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel33.Location = new System.Drawing.Point(3, 39);
            this.tableLayoutPanel33.Name = "tableLayoutPanel33";
            this.tableLayoutPanel33.RowCount = 2;
            this.tableLayoutPanel33.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 99F));
            this.tableLayoutPanel33.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel33.Size = new System.Drawing.Size(1246, 422);
            this.tableLayoutPanel33.TabIndex = 4;
            // 
            // tableLayoutPanel34
            // 
            this.tableLayoutPanel34.ColumnCount = 1;
            this.tableLayoutPanel34.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel34.Controls.Add(this.chartStatYear, 0, 0);
            this.tableLayoutPanel34.Controls.Add(this.tableLayoutPanel35, 0, 1);
            this.tableLayoutPanel34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel34.Location = new System.Drawing.Point(3, 420);
            this.tableLayoutPanel34.Name = "tableLayoutPanel34";
            this.tableLayoutPanel34.RowCount = 2;
            this.tableLayoutPanel34.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel34.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel34.Size = new System.Drawing.Size(1240, 1);
            this.tableLayoutPanel34.TabIndex = 4;
            // 
            // chartStatYear
            // 
            this.chartStatYear.BackColor = System.Drawing.Color.Transparent;
            this.chartStatYear.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartStatYear.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea2.Name = "ChartArea1";
            this.chartStatYear.ChartAreas.Add(chartArea2);
            this.chartStatYear.Cursor = System.Windows.Forms.Cursors.Default;
            this.chartStatYear.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartStatYear.Legends.Add(legend2);
            this.chartStatYear.Location = new System.Drawing.Point(3, 3);
            this.chartStatYear.Name = "chartStatYear";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartStatYear.Series.Add(series2);
            this.chartStatYear.Size = new System.Drawing.Size(1234, 1);
            this.chartStatYear.TabIndex = 0;
            this.chartStatYear.Text = "chart2";
            // 
            // tableLayoutPanel35
            // 
            this.tableLayoutPanel35.ColumnCount = 2;
            this.tableLayoutPanel35.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel35.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel35.Controls.Add(this.labelYearStatisticCaption, 0, 0);
            this.tableLayoutPanel35.Controls.Add(this.labelYearStatisticValue, 1, 0);
            this.tableLayoutPanel35.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel35.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel35.Name = "tableLayoutPanel35";
            this.tableLayoutPanel35.RowCount = 1;
            this.tableLayoutPanel35.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel35.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel35.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel35.Size = new System.Drawing.Size(1234, 1);
            this.tableLayoutPanel35.TabIndex = 1;
            // 
            // labelYearStatisticCaption
            // 
            this.labelYearStatisticCaption.AutoSize = true;
            this.labelYearStatisticCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelYearStatisticCaption.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelYearStatisticCaption.ForeColor = System.Drawing.Color.Black;
            this.labelYearStatisticCaption.Location = new System.Drawing.Point(3, 0);
            this.labelYearStatisticCaption.Name = "labelYearStatisticCaption";
            this.labelYearStatisticCaption.Size = new System.Drawing.Size(611, 1);
            this.labelYearStatisticCaption.TabIndex = 0;
            this.labelYearStatisticCaption.Text = "Сумма выработки:";
            this.labelYearStatisticCaption.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelYearStatisticValue
            // 
            this.labelYearStatisticValue.AutoSize = true;
            this.labelYearStatisticValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelYearStatisticValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelYearStatisticValue.ForeColor = System.Drawing.Color.Black;
            this.labelYearStatisticValue.Location = new System.Drawing.Point(620, 0);
            this.labelYearStatisticValue.Name = "labelYearStatisticValue";
            this.labelYearStatisticValue.Size = new System.Drawing.Size(611, 1);
            this.labelYearStatisticValue.TabIndex = 1;
            this.labelYearStatisticValue.Text = "0";
            this.labelYearStatisticValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridViewYearStatistic
            // 
            this.dataGridViewYearStatistic.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewYearStatistic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewYearStatistic.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridViewYearStatistic.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewYearStatistic.Name = "dataGridViewYearStatistic";
            this.dataGridViewYearStatistic.RowHeadersVisible = false;
            this.dataGridViewYearStatistic.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dataGridViewYearStatistic.RowTemplate.ReadOnly = true;
            this.dataGridViewYearStatistic.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewYearStatistic.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewYearStatistic.Size = new System.Drawing.Size(1240, 411);
            this.dataGridViewYearStatistic.TabIndex = 5;
            // 
            // metroSetSetTabPage7
            // 
            this.metroSetSetTabPage7.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage7.Controls.Add(this.tableLayoutPanel24);
            this.metroSetSetTabPage7.Font = null;
            this.metroSetSetTabPage7.ImageIndex = 0;
            this.metroSetSetTabPage7.ImageKey = null;
            this.metroSetSetTabPage7.IsDerivedStyle = true;
            this.metroSetSetTabPage7.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage7.Name = "metroSetSetTabPage7";
            this.metroSetSetTabPage7.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage7.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage7.StyleManager = null;
            this.metroSetSetTabPage7.TabIndex = 6;
            this.metroSetSetTabPage7.Text = "Статистика сотрудника";
            this.metroSetSetTabPage7.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage7.ThemeName = "MetroLite";
            this.metroSetSetTabPage7.ToolTipText = null;
            // 
            // tableLayoutPanel24
            // 
            this.tableLayoutPanel24.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel24.ColumnCount = 1;
            this.tableLayoutPanel24.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel24.Controls.Add(this.tableLayoutPanel25, 0, 0);
            this.tableLayoutPanel24.Controls.Add(this.tableLayoutPanel26, 0, 1);
            this.tableLayoutPanel24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel24.ForeColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel24.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel24.Name = "tableLayoutPanel24";
            this.tableLayoutPanel24.RowCount = 2;
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel24.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel24.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel24.TabIndex = 2;
            // 
            // tableLayoutPanel25
            // 
            this.tableLayoutPanel25.ColumnCount = 7;
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel25.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel25.Controls.Add(this.metroSetLabel5, 4, 0);
            this.tableLayoutPanel25.Controls.Add(this.metroSetSwitchUserStatistic, 3, 0);
            this.tableLayoutPanel25.Controls.Add(this.comboBoxUserYears, 0, 0);
            this.tableLayoutPanel25.Controls.Add(this.progressBarUserStatistic, 6, 0);
            this.tableLayoutPanel25.Controls.Add(this.comboBoxUserNames, 2, 0);
            this.tableLayoutPanel25.Controls.Add(this.comboBoxUserCategory, 1, 0);
            this.tableLayoutPanel25.Controls.Add(this.buttonUpdateUserStatistic, 5, 0);
            this.tableLayoutPanel25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel25.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel25.Name = "tableLayoutPanel25";
            this.tableLayoutPanel25.RowCount = 1;
            this.tableLayoutPanel25.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel25.Size = new System.Drawing.Size(1246, 30);
            this.tableLayoutPanel25.TabIndex = 2;
            // 
            // metroSetLabel5
            // 
            this.metroSetLabel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetLabel5.IsDerivedStyle = true;
            this.metroSetLabel5.Location = new System.Drawing.Point(707, 0);
            this.metroSetLabel5.Name = "metroSetLabel5";
            this.metroSetLabel5.Size = new System.Drawing.Size(254, 30);
            this.metroSetLabel5.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetLabel5.StyleManager = null;
            this.metroSetLabel5.TabIndex = 13;
            this.metroSetLabel5.Text = "Оборудование выбранного участка";
            this.metroSetLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.metroSetLabel5.ThemeAuthor = "Narwin";
            this.metroSetLabel5.ThemeName = "MetroLite";
            // 
            // metroSetSwitchUserStatistic
            // 
            this.metroSetSwitchUserStatistic.BackColor = System.Drawing.Color.Transparent;
            this.metroSetSwitchUserStatistic.BackgroundColor = System.Drawing.Color.Empty;
            this.metroSetSwitchUserStatistic.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(159)))), ((int)(((byte)(147)))));
            this.metroSetSwitchUserStatistic.CheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitchUserStatistic.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetSwitchUserStatistic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetSwitchUserStatistic.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitchUserStatistic.DisabledCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitchUserStatistic.DisabledUnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitchUserStatistic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetSwitchUserStatistic.IsDerivedStyle = true;
            this.metroSetSwitchUserStatistic.Location = new System.Drawing.Point(643, 3);
            this.metroSetSwitchUserStatistic.Name = "metroSetSwitchUserStatistic";
            this.metroSetSwitchUserStatistic.Size = new System.Drawing.Size(58, 22);
            this.metroSetSwitchUserStatistic.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSwitchUserStatistic.StyleManager = null;
            this.metroSetSwitchUserStatistic.Switched = false;
            this.metroSetSwitchUserStatistic.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.metroSetSwitchUserStatistic.TabIndex = 12;
            this.metroSetSwitchUserStatistic.Text = "Автообновление";
            this.metroSetSwitchUserStatistic.ThemeAuthor = "Narwin";
            this.metroSetSwitchUserStatistic.ThemeName = "MetroLite";
            this.metroSetSwitchUserStatistic.UnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetSwitchUserStatistic.SwitchedChanged += new MetroSet_UI.Controls.MetroSetSwitch.SwitchedChangedEventHandler(this.metroSetSwitchUserStatistic_SwitchedChanged);
            // 
            // comboBoxUserYears
            // 
            this.comboBoxUserYears.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxUserYears.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxUserYears.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUserYears.FormattingEnabled = true;
            this.comboBoxUserYears.Location = new System.Drawing.Point(3, 3);
            this.comboBoxUserYears.Name = "comboBoxUserYears";
            this.comboBoxUserYears.Size = new System.Drawing.Size(74, 24);
            this.comboBoxUserYears.TabIndex = 4;
            this.comboBoxUserYears.SelectedIndexChanged += new System.EventHandler(this.comboBoxUserYears_SelectedIndexChanged);
            // 
            // progressBarUserStatistic
            // 
            this.progressBarUserStatistic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarUserStatistic.Location = new System.Drawing.Point(1067, 3);
            this.progressBarUserStatistic.Name = "progressBarUserStatistic";
            this.progressBarUserStatistic.Size = new System.Drawing.Size(176, 24);
            this.progressBarUserStatistic.TabIndex = 8;
            // 
            // comboBoxUserNames
            // 
            this.comboBoxUserNames.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxUserNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUserNames.FormattingEnabled = true;
            this.comboBoxUserNames.Location = new System.Drawing.Point(323, 3);
            this.comboBoxUserNames.Name = "comboBoxUserNames";
            this.comboBoxUserNames.Size = new System.Drawing.Size(314, 24);
            this.comboBoxUserNames.TabIndex = 5;
            this.comboBoxUserNames.SelectedIndexChanged += new System.EventHandler(this.comboBoxUserNames_SelectedIndexChanged);
            // 
            // comboBoxUserCategory
            // 
            this.comboBoxUserCategory.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxUserCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxUserCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUserCategory.FormattingEnabled = true;
            this.comboBoxUserCategory.Location = new System.Drawing.Point(83, 3);
            this.comboBoxUserCategory.Name = "comboBoxUserCategory";
            this.comboBoxUserCategory.Size = new System.Drawing.Size(234, 24);
            this.comboBoxUserCategory.TabIndex = 3;
            this.comboBoxUserCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxUserCategory_SelectedIndexChanged);
            // 
            // buttonUpdateUserStatistic
            // 
            this.buttonUpdateUserStatistic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdateUserStatistic.ForeColor = System.Drawing.Color.Gray;
            this.buttonUpdateUserStatistic.Location = new System.Drawing.Point(967, 3);
            this.buttonUpdateUserStatistic.Name = "buttonUpdateUserStatistic";
            this.buttonUpdateUserStatistic.Size = new System.Drawing.Size(94, 24);
            this.buttonUpdateUserStatistic.TabIndex = 6;
            this.buttonUpdateUserStatistic.Text = "Обновить";
            this.buttonUpdateUserStatistic.UseVisualStyleBackColor = true;
            this.buttonUpdateUserStatistic.Click += new System.EventHandler(this.buttonUpdateUserStatistic_Click);
            // 
            // tableLayoutPanel26
            // 
            this.tableLayoutPanel26.ColumnCount = 2;
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel26.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel26.Controls.Add(this.ListViewUserWorking, 0, 0);
            this.tableLayoutPanel26.Controls.Add(this.tableLayoutPanel27, 0, 0);
            this.tableLayoutPanel26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel26.Location = new System.Drawing.Point(3, 39);
            this.tableLayoutPanel26.Name = "tableLayoutPanel26";
            this.tableLayoutPanel26.RowCount = 1;
            this.tableLayoutPanel26.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel26.Size = new System.Drawing.Size(1246, 422);
            this.tableLayoutPanel26.TabIndex = 4;
            // 
            // ListViewUserWorking
            // 
            this.ListViewUserWorking.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader30,
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader18});
            this.ListViewUserWorking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListViewUserWorking.FullRowSelect = true;
            this.ListViewUserWorking.GridLines = true;
            this.ListViewUserWorking.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListViewUserWorking.HideSelection = false;
            this.ListViewUserWorking.Location = new System.Drawing.Point(626, 3);
            this.ListViewUserWorking.MultiSelect = false;
            this.ListViewUserWorking.Name = "ListViewUserWorking";
            this.ListViewUserWorking.ShowItemToolTips = true;
            this.ListViewUserWorking.Size = new System.Drawing.Size(617, 416);
            this.ListViewUserWorking.TabIndex = 5;
            this.ListViewUserWorking.UseCompatibleStateImageBehavior = false;
            this.ListViewUserWorking.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "№";
            this.columnHeader9.Width = 30;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Месяц";
            this.columnHeader10.Width = 90;
            // 
            // columnHeader30
            // 
            this.columnHeader30.Text = "Смен";
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Количество";
            this.columnHeader13.Width = 90;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Время";
            this.columnHeader14.Width = 70;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Выработка";
            this.columnHeader15.Width = 85;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Приладки";
            this.columnHeader16.Width = 85;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Премия, %";
            this.columnHeader18.Width = 80;
            // 
            // tableLayoutPanel27
            // 
            this.tableLayoutPanel27.ColumnCount = 1;
            this.tableLayoutPanel27.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel27.Controls.Add(this.chartUserMakereadyCount, 0, 2);
            this.tableLayoutPanel27.Controls.Add(this.chartUserAmount, 0, 0);
            this.tableLayoutPanel27.Controls.Add(this.chartUserWorkingOutput, 0, 1);
            this.tableLayoutPanel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel27.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel27.Name = "tableLayoutPanel27";
            this.tableLayoutPanel27.RowCount = 3;
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel27.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel27.Size = new System.Drawing.Size(617, 416);
            this.tableLayoutPanel27.TabIndex = 4;
            // 
            // chartUserMakereadyCount
            // 
            this.chartUserMakereadyCount.BackColor = System.Drawing.Color.Transparent;
            this.chartUserMakereadyCount.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartUserMakereadyCount.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea3.Name = "ChartArea1";
            this.chartUserMakereadyCount.ChartAreas.Add(chartArea3);
            this.chartUserMakereadyCount.Cursor = System.Windows.Forms.Cursors.Default;
            this.chartUserMakereadyCount.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chartUserMakereadyCount.Legends.Add(legend3);
            this.chartUserMakereadyCount.Location = new System.Drawing.Point(3, 279);
            this.chartUserMakereadyCount.Name = "chartUserMakereadyCount";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartUserMakereadyCount.Series.Add(series3);
            this.chartUserMakereadyCount.Size = new System.Drawing.Size(611, 134);
            this.chartUserMakereadyCount.TabIndex = 2;
            this.chartUserMakereadyCount.Text = "chart4";
            // 
            // chartUserAmount
            // 
            this.chartUserAmount.BackColor = System.Drawing.Color.Transparent;
            this.chartUserAmount.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartUserAmount.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea4.Name = "ChartArea1";
            this.chartUserAmount.ChartAreas.Add(chartArea4);
            this.chartUserAmount.Cursor = System.Windows.Forms.Cursors.Default;
            this.chartUserAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.Name = "Legend1";
            this.chartUserAmount.Legends.Add(legend4);
            this.chartUserAmount.Location = new System.Drawing.Point(3, 3);
            this.chartUserAmount.Name = "chartUserAmount";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartUserAmount.Series.Add(series4);
            this.chartUserAmount.Size = new System.Drawing.Size(611, 132);
            this.chartUserAmount.TabIndex = 0;
            this.chartUserAmount.Text = "chart2";
            // 
            // chartUserWorkingOutput
            // 
            this.chartUserWorkingOutput.BackColor = System.Drawing.Color.Transparent;
            this.chartUserWorkingOutput.BackSecondaryColor = System.Drawing.Color.Transparent;
            this.chartUserWorkingOutput.BorderlineColor = System.Drawing.Color.Transparent;
            chartArea5.Name = "ChartArea1";
            this.chartUserWorkingOutput.ChartAreas.Add(chartArea5);
            this.chartUserWorkingOutput.Cursor = System.Windows.Forms.Cursors.Default;
            this.chartUserWorkingOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.Name = "Legend1";
            this.chartUserWorkingOutput.Legends.Add(legend5);
            this.chartUserWorkingOutput.Location = new System.Drawing.Point(3, 141);
            this.chartUserWorkingOutput.Name = "chartUserWorkingOutput";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chartUserWorkingOutput.Series.Add(series5);
            this.chartUserWorkingOutput.Size = new System.Drawing.Size(611, 132);
            this.chartUserWorkingOutput.TabIndex = 1;
            this.chartUserWorkingOutput.Text = "chart3";
            // 
            // metroSetSetTabPage8
            // 
            this.metroSetSetTabPage8.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage8.Controls.Add(this.tableLayoutPanel28);
            this.metroSetSetTabPage8.Font = null;
            this.metroSetSetTabPage8.ImageIndex = 0;
            this.metroSetSetTabPage8.ImageKey = null;
            this.metroSetSetTabPage8.IsDerivedStyle = true;
            this.metroSetSetTabPage8.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage8.Name = "metroSetSetTabPage8";
            this.metroSetSetTabPage8.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage8.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage8.StyleManager = null;
            this.metroSetSetTabPage8.TabIndex = 7;
            this.metroSetSetTabPage8.Text = "План работы";
            this.metroSetSetTabPage8.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage8.ThemeName = "MetroLite";
            this.metroSetSetTabPage8.ToolTipText = null;
            // 
            // tableLayoutPanel28
            // 
            this.tableLayoutPanel28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel28.ColumnCount = 1;
            this.tableLayoutPanel28.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel28.Controls.Add(this.tableLayoutPanel29, 0, 0);
            this.tableLayoutPanel28.Controls.Add(this.tableLayoutPanel30, 0, 1);
            this.tableLayoutPanel28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel28.ForeColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel28.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel28.Name = "tableLayoutPanel28";
            this.tableLayoutPanel28.RowCount = 2;
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel28.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel28.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel28.TabIndex = 3;
            // 
            // tableLayoutPanel29
            // 
            this.tableLayoutPanel29.ColumnCount = 6;
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel29.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel29.Controls.Add(this.metroSetSwitch3, 2, 0);
            this.tableLayoutPanel29.Controls.Add(this.comboBoxPlanCategory, 0, 0);
            this.tableLayoutPanel29.Controls.Add(this.comboBoxPlanEquips, 0, 0);
            this.tableLayoutPanel29.Controls.Add(this.button5, 4, 0);
            this.tableLayoutPanel29.Controls.Add(this.progressBar2, 5, 0);
            this.tableLayoutPanel29.Controls.Add(this.metroSetLabel4, 3, 0);
            this.tableLayoutPanel29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel29.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel29.Name = "tableLayoutPanel29";
            this.tableLayoutPanel29.RowCount = 1;
            this.tableLayoutPanel29.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel29.Size = new System.Drawing.Size(1246, 30);
            this.tableLayoutPanel29.TabIndex = 2;
            // 
            // metroSetSwitch3
            // 
            this.metroSetSwitch3.BackColor = System.Drawing.Color.Transparent;
            this.metroSetSwitch3.BackgroundColor = System.Drawing.Color.Empty;
            this.metroSetSwitch3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(159)))), ((int)(((byte)(147)))));
            this.metroSetSwitch3.CheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitch3.CheckState = MetroSet_UI.Enums.CheckState.Checked;
            this.metroSetSwitch3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetSwitch3.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitch3.DisabledCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetSwitch3.DisabledUnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetSwitch3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetSwitch3.IsDerivedStyle = true;
            this.metroSetSwitch3.Location = new System.Drawing.Point(563, 3);
            this.metroSetSwitch3.Name = "metroSetSwitch3";
            this.metroSetSwitch3.Size = new System.Drawing.Size(58, 22);
            this.metroSetSwitch3.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSwitch3.StyleManager = null;
            this.metroSetSwitch3.Switched = true;
            this.metroSetSwitch3.SymbolColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(92)))));
            this.metroSetSwitch3.TabIndex = 10;
            this.metroSetSwitch3.Text = "Автообновление";
            this.metroSetSwitch3.ThemeAuthor = "Narwin";
            this.metroSetSwitch3.ThemeName = "MetroLite";
            this.metroSetSwitch3.UnCheckColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetSwitch3.SwitchedChanged += new MetroSet_UI.Controls.MetroSetSwitch.SwitchedChangedEventHandler(this.metroSetSwitch3_SwitchedChanged);
            // 
            // comboBoxPlanCategory
            // 
            this.comboBoxPlanCategory.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxPlanCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxPlanCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlanCategory.FormattingEnabled = true;
            this.comboBoxPlanCategory.Location = new System.Drawing.Point(3, 3);
            this.comboBoxPlanCategory.Name = "comboBoxPlanCategory";
            this.comboBoxPlanCategory.Size = new System.Drawing.Size(234, 24);
            this.comboBoxPlanCategory.TabIndex = 9;
            this.comboBoxPlanCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlanCategory_SelectedIndexChanged);
            // 
            // comboBoxPlanEquips
            // 
            this.comboBoxPlanEquips.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBoxPlanEquips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxPlanEquips.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlanEquips.FormattingEnabled = true;
            this.comboBoxPlanEquips.Location = new System.Drawing.Point(243, 3);
            this.comboBoxPlanEquips.Name = "comboBoxPlanEquips";
            this.comboBoxPlanEquips.Size = new System.Drawing.Size(314, 24);
            this.comboBoxPlanEquips.TabIndex = 5;
            this.comboBoxPlanEquips.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlanEquips_SelectedIndexChanged);
            // 
            // button5
            // 
            this.button5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button5.ForeColor = System.Drawing.Color.Gray;
            this.button5.Location = new System.Drawing.Point(837, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(104, 24);
            this.button5.TabIndex = 6;
            this.button5.Text = "Обновить";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // progressBar2
            // 
            this.progressBar2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar2.Location = new System.Drawing.Point(947, 3);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(296, 24);
            this.progressBar2.TabIndex = 8;
            // 
            // metroSetLabel4
            // 
            this.metroSetLabel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetLabel4.IsDerivedStyle = true;
            this.metroSetLabel4.Location = new System.Drawing.Point(627, 0);
            this.metroSetLabel4.Name = "metroSetLabel4";
            this.metroSetLabel4.Size = new System.Drawing.Size(204, 30);
            this.metroSetLabel4.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetLabel4.StyleManager = null;
            this.metroSetLabel4.TabIndex = 11;
            this.metroSetLabel4.Text = "Отображение всех заказов";
            this.metroSetLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.metroSetLabel4.ThemeAuthor = "Narwin";
            this.metroSetLabel4.ThemeName = "MetroLite";
            // 
            // tableLayoutPanel30
            // 
            this.tableLayoutPanel30.ColumnCount = 1;
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel30.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel30.Controls.Add(this.listViewPlan, 0, 0);
            this.tableLayoutPanel30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel30.Location = new System.Drawing.Point(3, 39);
            this.tableLayoutPanel30.Name = "tableLayoutPanel30";
            this.tableLayoutPanel30.RowCount = 1;
            this.tableLayoutPanel30.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel30.Size = new System.Drawing.Size(1246, 422);
            this.tableLayoutPanel30.TabIndex = 4;
            // 
            // listViewPlan
            // 
            this.listViewPlan.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader22,
            this.columnHeader23,
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26,
            this.columnHeader27,
            this.columnHeader28});
            this.listViewPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPlan.FullRowSelect = true;
            this.listViewPlan.GridLines = true;
            this.listViewPlan.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewPlan.HideSelection = false;
            this.listViewPlan.Location = new System.Drawing.Point(3, 3);
            this.listViewPlan.MultiSelect = false;
            this.listViewPlan.Name = "listViewPlan";
            this.listViewPlan.ShowItemToolTips = true;
            this.listViewPlan.Size = new System.Drawing.Size(1240, 416);
            this.listViewPlan.TabIndex = 5;
            this.listViewPlan.UseCompatibleStateImageBehavior = false;
            this.listViewPlan.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "№";
            this.columnHeader19.Width = 30;
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "Начало";
            this.columnHeader20.Width = 120;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "Завершение";
            this.columnHeader21.Width = 120;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Заказ";
            this.columnHeader22.Width = 100;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Заказчик";
            this.columnHeader23.Width = 220;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "Описание";
            this.columnHeader24.Width = 260;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "Приладка";
            this.columnHeader25.Width = 80;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "Работа";
            this.columnHeader26.Width = 80;
            // 
            // columnHeader27
            // 
            this.columnHeader27.Text = "Тираж";
            this.columnHeader27.Width = 90;
            // 
            // columnHeader28
            // 
            this.columnHeader28.Text = "Статус";
            this.columnHeader28.Width = 100;
            // 
            // metroSetSetTabPage5
            // 
            this.metroSetSetTabPage5.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage5.Controls.Add(this.tableLayoutPanel17);
            this.metroSetSetTabPage5.Font = null;
            this.metroSetSetTabPage5.ImageIndex = 0;
            this.metroSetSetTabPage5.ImageKey = null;
            this.metroSetSetTabPage5.IsDerivedStyle = true;
            this.metroSetSetTabPage5.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage5.Name = "metroSetSetTabPage5";
            this.metroSetSetTabPage5.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage5.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage5.StyleManager = null;
            this.metroSetSetTabPage5.TabIndex = 4;
            this.metroSetSetTabPage5.Text = "Детали заказа";
            this.metroSetSetTabPage5.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage5.ThemeName = "MetroLite";
            this.metroSetSetTabPage5.ToolTipText = null;
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel17.ColumnCount = 1;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.Controls.Add(this.tableLayoutPanel18, 0, 0);
            this.tableLayoutPanel17.Controls.Add(this.dataGridViewOrderDetails, 0, 1);
            this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel17.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 2;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel17.TabIndex = 0;
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 4;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel18.Controls.Add(this.textBox1, 0, 0);
            this.tableLayoutPanel18.Controls.Add(this.button3, 1, 0);
            this.tableLayoutPanel18.Controls.Add(this.comboBox6, 2, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 1;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(1246, 30);
            this.tableLayoutPanel18.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(134, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(143, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 24);
            this.button3.TabIndex = 1;
            this.button3.Text = "Поиск";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBox6
            // 
            this.comboBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point(243, 3);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(396, 24);
            this.comboBox6.TabIndex = 2;
            this.comboBox6.SelectedIndexChanged += new System.EventHandler(this.comboBox6_SelectedIndexChanged);
            // 
            // dataGridViewOrderDetails
            // 
            this.dataGridViewOrderDetails.AllowUserToAddRows = false;
            this.dataGridViewOrderDetails.AllowUserToDeleteRows = false;
            this.dataGridViewOrderDetails.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridViewOrderDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridViewOrderDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrderDetails.ColumnHeadersVisible = false;
            this.dataGridViewOrderDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOrderDetails.Location = new System.Drawing.Point(3, 39);
            this.dataGridViewOrderDetails.MultiSelect = false;
            this.dataGridViewOrderDetails.Name = "dataGridViewOrderDetails";
            this.dataGridViewOrderDetails.ReadOnly = true;
            this.dataGridViewOrderDetails.RowHeadersVisible = false;
            this.dataGridViewOrderDetails.RowHeadersWidth = 51;
            this.dataGridViewOrderDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOrderDetails.Size = new System.Drawing.Size(1246, 422);
            this.dataGridViewOrderDetails.TabIndex = 1;
            // 
            // metroSetSetTabPage3
            // 
            this.metroSetSetTabPage3.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage3.Controls.Add(this.tableLayoutPanel5);
            this.metroSetSetTabPage3.Font = null;
            this.metroSetSetTabPage3.ImageIndex = 0;
            this.metroSetSetTabPage3.ImageKey = null;
            this.metroSetSetTabPage3.IsDerivedStyle = true;
            this.metroSetSetTabPage3.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage3.Name = "metroSetSetTabPage3";
            this.metroSetSetTabPage3.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage3.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage3.StyleManager = null;
            this.metroSetSetTabPage3.TabIndex = 2;
            this.metroSetSetTabPage3.Text = "Параметры";
            this.metroSetSetTabPage3.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage3.ThemeName = "MetroLite";
            this.metroSetSetTabPage3.ToolTipText = null;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 472F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 4;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel7.Controls.Add(this.listViewCategory, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.listViewEquips, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel8, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel9, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel6, 2, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 2;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1246, 458);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // listViewCategory
            // 
            this.listViewCategory.CheckBoxes = true;
            this.listViewCategory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12});
            this.listViewCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewCategory.FullRowSelect = true;
            this.listViewCategory.GridLines = true;
            this.listViewCategory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewCategory.HideSelection = false;
            this.listViewCategory.Location = new System.Drawing.Point(3, 3);
            this.listViewCategory.MultiSelect = false;
            this.listViewCategory.Name = "listViewCategory";
            this.listViewCategory.ShowItemToolTips = true;
            this.listViewCategory.Size = new System.Drawing.Size(305, 410);
            this.listViewCategory.TabIndex = 2;
            this.listViewCategory.UseCompatibleStateImageBehavior = false;
            this.listViewCategory.View = System.Windows.Forms.View.Details;
            this.listViewCategory.SelectedIndexChanged += new System.EventHandler(this.listViewCategory_SelectedIndexChanged);
            this.listViewCategory.SizeChanged += new System.EventHandler(this.listViewCategory_SizeChanged);
            this.listViewCategory.Click += new System.EventHandler(this.listViewCategory_Click);
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "№";
            this.columnHeader11.Width = 40;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Категория";
            this.columnHeader12.Width = 250;
            // 
            // listViewEquips
            // 
            this.listViewEquips.CheckBoxes = true;
            this.listViewEquips.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader17});
            this.listViewEquips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewEquips.FullRowSelect = true;
            this.listViewEquips.GridLines = true;
            this.listViewEquips.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewEquips.HideSelection = false;
            this.listViewEquips.Location = new System.Drawing.Point(314, 3);
            this.listViewEquips.MultiSelect = false;
            this.listViewEquips.Name = "listViewEquips";
            this.listViewEquips.ShowItemToolTips = true;
            this.listViewEquips.Size = new System.Drawing.Size(305, 410);
            this.listViewEquips.TabIndex = 1;
            this.listViewEquips.UseCompatibleStateImageBehavior = false;
            this.listViewEquips.View = System.Windows.Forms.View.Details;
            this.listViewEquips.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewEquips_ItemChecked);
            this.listViewEquips.SelectedIndexChanged += new System.EventHandler(this.listViewEquips_SelectedIndexChanged);
            this.listViewEquips.SizeChanged += new System.EventHandler(this.listViewEquips_SizeChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "№";
            this.columnHeader5.Width = 40;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Оборудование";
            this.columnHeader17.Width = 250;
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 5;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel8.Controls.Add(this.buttonCatDown, 4, 0);
            this.tableLayoutPanel8.Controls.Add(this.buttonCatAdd, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.buttonCatEdit, 1, 0);
            this.tableLayoutPanel8.Controls.Add(this.buttonCatDelete, 2, 0);
            this.tableLayoutPanel8.Controls.Add(this.buttonCatUp, 3, 0);
            this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 419);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(305, 36);
            this.tableLayoutPanel8.TabIndex = 3;
            // 
            // buttonCatDown
            // 
            this.buttonCatDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCatDown.Enabled = false;
            this.buttonCatDown.Location = new System.Drawing.Point(247, 3);
            this.buttonCatDown.Name = "buttonCatDown";
            this.buttonCatDown.Size = new System.Drawing.Size(55, 30);
            this.buttonCatDown.TabIndex = 0;
            this.buttonCatDown.Text = "Down";
            this.buttonCatDown.UseVisualStyleBackColor = true;
            this.buttonCatDown.Click += new System.EventHandler(this.buttonCatDown_Click);
            // 
            // buttonCatAdd
            // 
            this.buttonCatAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCatAdd.Location = new System.Drawing.Point(3, 3);
            this.buttonCatAdd.Name = "buttonCatAdd";
            this.buttonCatAdd.Size = new System.Drawing.Size(55, 30);
            this.buttonCatAdd.TabIndex = 0;
            this.buttonCatAdd.Text = "Add";
            this.buttonCatAdd.UseVisualStyleBackColor = true;
            this.buttonCatAdd.Click += new System.EventHandler(this.buttonAddCat_Click);
            // 
            // buttonCatEdit
            // 
            this.buttonCatEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCatEdit.Enabled = false;
            this.buttonCatEdit.Location = new System.Drawing.Point(64, 3);
            this.buttonCatEdit.Name = "buttonCatEdit";
            this.buttonCatEdit.Size = new System.Drawing.Size(55, 30);
            this.buttonCatEdit.TabIndex = 1;
            this.buttonCatEdit.Text = "Edit";
            this.buttonCatEdit.UseVisualStyleBackColor = true;
            this.buttonCatEdit.Click += new System.EventHandler(this.buttonCatEdit_Click);
            // 
            // buttonCatDelete
            // 
            this.buttonCatDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCatDelete.Enabled = false;
            this.buttonCatDelete.Location = new System.Drawing.Point(125, 3);
            this.buttonCatDelete.Name = "buttonCatDelete";
            this.buttonCatDelete.Size = new System.Drawing.Size(55, 30);
            this.buttonCatDelete.TabIndex = 2;
            this.buttonCatDelete.Text = "Del";
            this.buttonCatDelete.UseVisualStyleBackColor = true;
            this.buttonCatDelete.Click += new System.EventHandler(this.buttonCatDelete_Click);
            // 
            // buttonCatUp
            // 
            this.buttonCatUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCatUp.Enabled = false;
            this.buttonCatUp.Location = new System.Drawing.Point(186, 3);
            this.buttonCatUp.Name = "buttonCatUp";
            this.buttonCatUp.Size = new System.Drawing.Size(55, 30);
            this.buttonCatUp.TabIndex = 3;
            this.buttonCatUp.Text = "Up";
            this.buttonCatUp.UseVisualStyleBackColor = true;
            this.buttonCatUp.Click += new System.EventHandler(this.buttonCatUp_Click);
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 4;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel9.Controls.Add(this.buttonEquipAdd, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.buttonEquipDel, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.buttonEquipUp, 2, 0);
            this.tableLayoutPanel9.Controls.Add(this.buttonEquipDown, 3, 0);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(314, 419);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(305, 36);
            this.tableLayoutPanel9.TabIndex = 4;
            // 
            // buttonEquipAdd
            // 
            this.buttonEquipAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipAdd.Enabled = false;
            this.buttonEquipAdd.Location = new System.Drawing.Point(3, 3);
            this.buttonEquipAdd.Name = "buttonEquipAdd";
            this.buttonEquipAdd.Size = new System.Drawing.Size(70, 30);
            this.buttonEquipAdd.TabIndex = 1;
            this.buttonEquipAdd.Text = "Add";
            this.buttonEquipAdd.UseVisualStyleBackColor = true;
            this.buttonEquipAdd.Click += new System.EventHandler(this.buttonEquipAdd_Click);
            // 
            // buttonEquipDel
            // 
            this.buttonEquipDel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipDel.Enabled = false;
            this.buttonEquipDel.Location = new System.Drawing.Point(79, 3);
            this.buttonEquipDel.Name = "buttonEquipDel";
            this.buttonEquipDel.Size = new System.Drawing.Size(70, 30);
            this.buttonEquipDel.TabIndex = 2;
            this.buttonEquipDel.Text = "Del";
            this.buttonEquipDel.UseVisualStyleBackColor = true;
            this.buttonEquipDel.Click += new System.EventHandler(this.buttonEquipDel_Click);
            // 
            // buttonEquipUp
            // 
            this.buttonEquipUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipUp.Enabled = false;
            this.buttonEquipUp.Location = new System.Drawing.Point(155, 3);
            this.buttonEquipUp.Name = "buttonEquipUp";
            this.buttonEquipUp.Size = new System.Drawing.Size(70, 30);
            this.buttonEquipUp.TabIndex = 3;
            this.buttonEquipUp.Text = "Up";
            this.buttonEquipUp.UseVisualStyleBackColor = true;
            this.buttonEquipUp.Click += new System.EventHandler(this.buttonEquipUp_Click);
            // 
            // buttonEquipDown
            // 
            this.buttonEquipDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipDown.Enabled = false;
            this.buttonEquipDown.Location = new System.Drawing.Point(231, 3);
            this.buttonEquipDown.Name = "buttonEquipDown";
            this.buttonEquipDown.Size = new System.Drawing.Size(71, 30);
            this.buttonEquipDown.TabIndex = 4;
            this.buttonEquipDown.Text = "Down";
            this.buttonEquipDown.UseVisualStyleBackColor = true;
            this.buttonEquipDown.Click += new System.EventHandler(this.buttonEquipDown_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.groupBox9, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.groupBox2, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.groupBox3, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.groupBox4, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.groupBox7, 0, 5);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(625, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 7;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(305, 410);
            this.tableLayoutPanel6.TabIndex = 5;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.label9);
            this.groupBox9.Controls.Add(this.label10);
            this.groupBox9.Controls.Add(this.formattedNumericUpDown10);
            this.groupBox9.Controls.Add(this.formattedNumericUpDown11);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox9.Location = new System.Drawing.Point(3, 73);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(299, 64);
            this.groupBox9.TabIndex = 5;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Рабочее время смены";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(146, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "ЧЧЧ:ММ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(70, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = ":";
            // 
            // formattedNumericUpDown10
            // 
            this.formattedNumericUpDown10.Format = "00";
            this.formattedNumericUpDown10.Location = new System.Drawing.Point(83, 29);
            this.formattedNumericUpDown10.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.formattedNumericUpDown10.Name = "formattedNumericUpDown10";
            this.formattedNumericUpDown10.Size = new System.Drawing.Size(57, 23);
            this.formattedNumericUpDown10.TabIndex = 2;
            // 
            // formattedNumericUpDown11
            // 
            this.formattedNumericUpDown11.Format = "000";
            this.formattedNumericUpDown11.Location = new System.Drawing.Point(11, 29);
            this.formattedNumericUpDown11.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.formattedNumericUpDown11.Name = "formattedNumericUpDown11";
            this.formattedNumericUpDown11.Size = new System.Drawing.Size(57, 23);
            this.formattedNumericUpDown11.TabIndex = 1;
            this.formattedNumericUpDown11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.formattedNumericUpDown11.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.formattedNumericUpDown2);
            this.groupBox1.Controls.Add(this.formattedNumericUpDown1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Время для выработки 100%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "ЧЧЧ:ММ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(70, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = ":";
            // 
            // formattedNumericUpDown2
            // 
            this.formattedNumericUpDown2.Format = "00";
            this.formattedNumericUpDown2.Location = new System.Drawing.Point(83, 29);
            this.formattedNumericUpDown2.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.formattedNumericUpDown2.Name = "formattedNumericUpDown2";
            this.formattedNumericUpDown2.Size = new System.Drawing.Size(57, 23);
            this.formattedNumericUpDown2.TabIndex = 2;
            // 
            // formattedNumericUpDown1
            // 
            this.formattedNumericUpDown1.Format = "000";
            this.formattedNumericUpDown1.Location = new System.Drawing.Point(11, 29);
            this.formattedNumericUpDown1.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.formattedNumericUpDown1.Name = "formattedNumericUpDown1";
            this.formattedNumericUpDown1.Size = new System.Drawing.Size(57, 23);
            this.formattedNumericUpDown1.TabIndex = 1;
            this.formattedNumericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.formattedNumericUpDown1.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.formattedNumericUpDown3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 64);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Количество смен на производстве";
            // 
            // formattedNumericUpDown3
            // 
            this.formattedNumericUpDown3.Format = null;
            this.formattedNumericUpDown3.Location = new System.Drawing.Point(11, 28);
            this.formattedNumericUpDown3.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.formattedNumericUpDown3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.formattedNumericUpDown3.Name = "formattedNumericUpDown3";
            this.formattedNumericUpDown3.Size = new System.Drawing.Size(57, 23);
            this.formattedNumericUpDown3.TabIndex = 0;
            this.formattedNumericUpDown3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.metroSetCheckBox1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 213);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 64);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Оборудование в статистике";
            // 
            // metroSetCheckBox1
            // 
            this.metroSetCheckBox1.BackColor = System.Drawing.Color.Transparent;
            this.metroSetCheckBox1.BackgroundColor = System.Drawing.Color.White;
            this.metroSetCheckBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetCheckBox1.Checked = false;
            this.metroSetCheckBox1.CheckSignColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetCheckBox1.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetCheckBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetCheckBox1.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetCheckBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetCheckBox1.IsDerivedStyle = true;
            this.metroSetCheckBox1.Location = new System.Drawing.Point(11, 30);
            this.metroSetCheckBox1.Name = "metroSetCheckBox1";
            this.metroSetCheckBox1.SignStyle = MetroSet_UI.Enums.SignStyle.Sign;
            this.metroSetCheckBox1.Size = new System.Drawing.Size(286, 16);
            this.metroSetCheckBox1.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetCheckBox1.StyleManager = null;
            this.metroSetCheckBox1.TabIndex = 0;
            this.metroSetCheckBox1.Text = "Отображать все оборудование";
            this.metroSetCheckBox1.ThemeAuthor = "Narwin";
            this.metroSetCheckBox1.ThemeName = "MetroLite";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.metroSetCheckBox2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 283);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(299, 64);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Выборка заказов из БД";
            // 
            // metroSetCheckBox2
            // 
            this.metroSetCheckBox2.BackColor = System.Drawing.Color.Transparent;
            this.metroSetCheckBox2.BackgroundColor = System.Drawing.Color.White;
            this.metroSetCheckBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetCheckBox2.Checked = false;
            this.metroSetCheckBox2.CheckSignColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetCheckBox2.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetCheckBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetCheckBox2.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetCheckBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetCheckBox2.IsDerivedStyle = true;
            this.metroSetCheckBox2.Location = new System.Drawing.Point(11, 32);
            this.metroSetCheckBox2.Name = "metroSetCheckBox2";
            this.metroSetCheckBox2.SignStyle = MetroSet_UI.Enums.SignStyle.Sign;
            this.metroSetCheckBox2.Size = new System.Drawing.Size(286, 16);
            this.metroSetCheckBox2.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetCheckBox2.StyleManager = null;
            this.metroSetCheckBox2.TabIndex = 0;
            this.metroSetCheckBox2.Text = "С указанием номера смены";
            this.metroSetCheckBox2.ThemeAuthor = "Narwin";
            this.metroSetCheckBox2.ThemeName = "MetroLite";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.metroSetCheckBox6);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(3, 353);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(299, 64);
            this.groupBox7.TabIndex = 4;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Смены без выработки";
            // 
            // metroSetCheckBox6
            // 
            this.metroSetCheckBox6.BackColor = System.Drawing.Color.Transparent;
            this.metroSetCheckBox6.BackgroundColor = System.Drawing.Color.White;
            this.metroSetCheckBox6.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetCheckBox6.Checked = false;
            this.metroSetCheckBox6.CheckSignColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetCheckBox6.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetCheckBox6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetCheckBox6.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetCheckBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetCheckBox6.IsDerivedStyle = true;
            this.metroSetCheckBox6.Location = new System.Drawing.Point(8, 30);
            this.metroSetCheckBox6.Name = "metroSetCheckBox6";
            this.metroSetCheckBox6.SignStyle = MetroSet_UI.Enums.SignStyle.Sign;
            this.metroSetCheckBox6.Size = new System.Drawing.Size(286, 16);
            this.metroSetCheckBox6.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetCheckBox6.StyleManager = null;
            this.metroSetCheckBox6.TabIndex = 1;
            this.metroSetCheckBox6.Text = "Не учитывать при расчётах";
            this.metroSetCheckBox6.ThemeAuthor = "Narwin";
            this.metroSetCheckBox6.ThemeName = "MetroLite";
            // 
            // metroSetSetTabPage4
            // 
            this.metroSetSetTabPage4.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage4.Controls.Add(this.tableLayoutPanel10);
            this.metroSetSetTabPage4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetSetTabPage4.Font = null;
            this.metroSetSetTabPage4.ImageIndex = 0;
            this.metroSetSetTabPage4.ImageKey = null;
            this.metroSetSetTabPage4.IsDerivedStyle = true;
            this.metroSetSetTabPage4.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage4.Name = "metroSetSetTabPage4";
            this.metroSetSetTabPage4.Size = new System.Drawing.Size(1252, 464);
            this.metroSetSetTabPage4.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage4.StyleManager = null;
            this.metroSetSetTabPage4.TabIndex = 3;
            this.metroSetSetTabPage4.Text = "Параметры монитора";
            this.metroSetSetTabPage4.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage4.ThemeName = "MetroLite";
            this.metroSetSetTabPage4.ToolTipText = null;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel11, 0, 0);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel12, 0, 1);
            this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 2;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Size = new System.Drawing.Size(1252, 464);
            this.tableLayoutPanel10.TabIndex = 0;
            // 
            // tableLayoutPanel11
            // 
            this.tableLayoutPanel11.ColumnCount = 5;
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel11.Controls.Add(this.comboBox5, 0, 0);
            this.tableLayoutPanel11.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel11.Controls.Add(this.buttonEditViewPath, 2, 0);
            this.tableLayoutPanel11.Controls.Add(this.buttonDelViewPath, 3, 0);
            this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel11.Name = "tableLayoutPanel11";
            this.tableLayoutPanel11.RowCount = 1;
            this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel11.Size = new System.Drawing.Size(1246, 29);
            this.tableLayoutPanel11.TabIndex = 0;
            // 
            // comboBox5
            // 
            this.comboBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point(3, 3);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(313, 24);
            this.comboBox5.TabIndex = 0;
            this.comboBox5.SelectedIndexChanged += new System.EventHandler(this.comboBox5_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(322, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(686, 29);
            this.label3.TabIndex = 1;
            this.label3.Text = "path";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonEditViewPath
            // 
            this.buttonEditViewPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEditViewPath.Location = new System.Drawing.Point(1014, 3);
            this.buttonEditViewPath.Name = "buttonEditViewPath";
            this.buttonEditViewPath.Size = new System.Drawing.Size(84, 23);
            this.buttonEditViewPath.TabIndex = 2;
            this.buttonEditViewPath.Text = "Изменить";
            this.buttonEditViewPath.UseVisualStyleBackColor = true;
            this.buttonEditViewPath.Click += new System.EventHandler(this.buttonEditViewPath_Click);
            // 
            // buttonDelViewPath
            // 
            this.buttonDelViewPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelViewPath.Location = new System.Drawing.Point(1104, 3);
            this.buttonDelViewPath.Name = "buttonDelViewPath";
            this.buttonDelViewPath.Size = new System.Drawing.Size(84, 23);
            this.buttonDelViewPath.TabIndex = 3;
            this.buttonDelViewPath.Text = "Удалить";
            this.buttonDelViewPath.UseVisualStyleBackColor = true;
            this.buttonDelViewPath.Click += new System.EventHandler(this.buttonDelViewPath_Click);
            // 
            // tableLayoutPanel12
            // 
            this.tableLayoutPanel12.ColumnCount = 2;
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel12.Controls.Add(this.tableLayoutPanel13, 0, 0);
            this.tableLayoutPanel12.Controls.Add(this.tableLayoutPanel14, 1, 0);
            this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanel12.Name = "tableLayoutPanel12";
            this.tableLayoutPanel12.RowCount = 1;
            this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel12.Size = new System.Drawing.Size(1246, 423);
            this.tableLayoutPanel12.TabIndex = 1;
            this.tableLayoutPanel12.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel12_Paint);
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.ColumnCount = 1;
            this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel13.Controls.Add(this.groupBox5, 0, 0);
            this.tableLayoutPanel13.Controls.Add(this.groupBox6, 0, 1);
            this.tableLayoutPanel13.Controls.Add(this.groupBox8, 0, 2);
            this.tableLayoutPanel13.Controls.Add(this.tableLayoutPanel16, 0, 3);
            this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.RowCount = 5;
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel13.Size = new System.Drawing.Size(554, 417);
            this.tableLayoutPanel13.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.metroSetCheckBox3);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.formattedNumericUpDown5);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(548, 64);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Отображение дней статистики";
            // 
            // metroSetCheckBox3
            // 
            this.metroSetCheckBox3.BackColor = System.Drawing.Color.Transparent;
            this.metroSetCheckBox3.BackgroundColor = System.Drawing.Color.White;
            this.metroSetCheckBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetCheckBox3.Checked = false;
            this.metroSetCheckBox3.CheckSignColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetCheckBox3.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetCheckBox3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetCheckBox3.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetCheckBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetCheckBox3.IsDerivedStyle = true;
            this.metroSetCheckBox3.Location = new System.Drawing.Point(340, 25);
            this.metroSetCheckBox3.Name = "metroSetCheckBox3";
            this.metroSetCheckBox3.SignStyle = MetroSet_UI.Enums.SignStyle.Sign;
            this.metroSetCheckBox3.Size = new System.Drawing.Size(213, 16);
            this.metroSetCheckBox3.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetCheckBox3.StyleManager = null;
            this.metroSetCheckBox3.TabIndex = 2;
            this.metroSetCheckBox3.Text = "Отображать текущий день";
            this.metroSetCheckBox3.ThemeAuthor = "Narwin";
            this.metroSetCheckBox3.ThemeName = "MetroLite";
            this.metroSetCheckBox3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(198, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "количество дней для показа";
            // 
            // formattedNumericUpDown5
            // 
            this.formattedNumericUpDown5.Format = null;
            this.formattedNumericUpDown5.Location = new System.Drawing.Point(6, 22);
            this.formattedNumericUpDown5.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.formattedNumericUpDown5.Name = "formattedNumericUpDown5";
            this.formattedNumericUpDown5.Size = new System.Drawing.Size(53, 23);
            this.formattedNumericUpDown5.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.formattedNumericUpDown9);
            this.groupBox6.Controls.Add(this.formattedNumericUpDown8);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.formattedNumericUpDown7);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.formattedNumericUpDown6);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(3, 73);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(548, 64);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Ширина основных столбцов";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(432, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "- итоги";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(281, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 17);
            this.label8.TabIndex = 4;
            this.label8.Text = "- выработка";
            // 
            // formattedNumericUpDown9
            // 
            this.formattedNumericUpDown9.Format = null;
            this.formattedNumericUpDown9.Location = new System.Drawing.Point(379, 23);
            this.formattedNumericUpDown9.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.formattedNumericUpDown9.Name = "formattedNumericUpDown9";
            this.formattedNumericUpDown9.Size = new System.Drawing.Size(52, 23);
            this.formattedNumericUpDown9.TabIndex = 4;
            // 
            // formattedNumericUpDown8
            // 
            this.formattedNumericUpDown8.Format = null;
            this.formattedNumericUpDown8.Location = new System.Drawing.Point(228, 23);
            this.formattedNumericUpDown8.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.formattedNumericUpDown8.Name = "formattedNumericUpDown8";
            this.formattedNumericUpDown8.Size = new System.Drawing.Size(53, 23);
            this.formattedNumericUpDown8.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(176, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "- имя";
            // 
            // formattedNumericUpDown7
            // 
            this.formattedNumericUpDown7.Format = null;
            this.formattedNumericUpDown7.Location = new System.Drawing.Point(124, 23);
            this.formattedNumericUpDown7.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.formattedNumericUpDown7.Name = "formattedNumericUpDown7";
            this.formattedNumericUpDown7.Size = new System.Drawing.Size(52, 23);
            this.formattedNumericUpDown7.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "- номер";
            // 
            // formattedNumericUpDown6
            // 
            this.formattedNumericUpDown6.Format = null;
            this.formattedNumericUpDown6.Location = new System.Drawing.Point(7, 23);
            this.formattedNumericUpDown6.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.formattedNumericUpDown6.Name = "formattedNumericUpDown6";
            this.formattedNumericUpDown6.Size = new System.Drawing.Size(52, 23);
            this.formattedNumericUpDown6.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.metroSetCheckBox4);
            this.groupBox8.Controls.Add(this.metroSetCheckBox5);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(3, 143);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(548, 84);
            this.groupBox8.TabIndex = 3;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Автоформатирование";
            // 
            // metroSetCheckBox4
            // 
            this.metroSetCheckBox4.BackColor = System.Drawing.Color.Transparent;
            this.metroSetCheckBox4.BackgroundColor = System.Drawing.Color.White;
            this.metroSetCheckBox4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetCheckBox4.Checked = false;
            this.metroSetCheckBox4.CheckSignColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetCheckBox4.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetCheckBox4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetCheckBox4.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetCheckBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetCheckBox4.IsDerivedStyle = true;
            this.metroSetCheckBox4.Location = new System.Drawing.Point(7, 44);
            this.metroSetCheckBox4.Name = "metroSetCheckBox4";
            this.metroSetCheckBox4.SignStyle = MetroSet_UI.Enums.SignStyle.Sign;
            this.metroSetCheckBox4.Size = new System.Drawing.Size(479, 16);
            this.metroSetCheckBox4.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetCheckBox4.StyleManager = null;
            this.metroSetCheckBox4.TabIndex = 5;
            this.metroSetCheckBox4.Text = "Увеличивать ширину столбцов выработки под ширину окна";
            this.metroSetCheckBox4.ThemeAuthor = "Narwin";
            this.metroSetCheckBox4.ThemeName = "MetroLite";
            // 
            // metroSetCheckBox5
            // 
            this.metroSetCheckBox5.BackColor = System.Drawing.Color.Transparent;
            this.metroSetCheckBox5.BackgroundColor = System.Drawing.Color.White;
            this.metroSetCheckBox5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(155)))), ((int)(((byte)(155)))));
            this.metroSetCheckBox5.Checked = false;
            this.metroSetCheckBox5.CheckSignColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(177)))), ((int)(((byte)(225)))));
            this.metroSetCheckBox5.CheckState = MetroSet_UI.Enums.CheckState.Unchecked;
            this.metroSetCheckBox5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetCheckBox5.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.metroSetCheckBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.metroSetCheckBox5.IsDerivedStyle = true;
            this.metroSetCheckBox5.Location = new System.Drawing.Point(7, 22);
            this.metroSetCheckBox5.Name = "metroSetCheckBox5";
            this.metroSetCheckBox5.SignStyle = MetroSet_UI.Enums.SignStyle.Sign;
            this.metroSetCheckBox5.Size = new System.Drawing.Size(478, 16);
            this.metroSetCheckBox5.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetCheckBox5.StyleManager = null;
            this.metroSetCheckBox5.TabIndex = 0;
            this.metroSetCheckBox5.Text = "При возможности добавлять дни отображения статистики";
            this.metroSetCheckBox5.ThemeAuthor = "Narwin";
            this.metroSetCheckBox5.ThemeName = "MetroLite";
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.ColumnCount = 3;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel16.Controls.Add(this.buttonSettingsSave, 1, 0);
            this.tableLayoutPanel16.Controls.Add(this.buttonSettingsReload, 2, 0);
            this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel16.Location = new System.Drawing.Point(3, 233);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 1;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(548, 29);
            this.tableLayoutPanel16.TabIndex = 4;
            // 
            // buttonSettingsSave
            // 
            this.buttonSettingsSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSettingsSave.Location = new System.Drawing.Point(351, 3);
            this.buttonSettingsSave.Name = "buttonSettingsSave";
            this.buttonSettingsSave.Size = new System.Drawing.Size(94, 23);
            this.buttonSettingsSave.TabIndex = 0;
            this.buttonSettingsSave.Text = "Сохранить";
            this.buttonSettingsSave.UseVisualStyleBackColor = true;
            this.buttonSettingsSave.Click += new System.EventHandler(this.buttonSettingsSave_Click);
            // 
            // buttonSettingsReload
            // 
            this.buttonSettingsReload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSettingsReload.Location = new System.Drawing.Point(451, 3);
            this.buttonSettingsReload.Name = "buttonSettingsReload";
            this.buttonSettingsReload.Size = new System.Drawing.Size(94, 23);
            this.buttonSettingsReload.TabIndex = 1;
            this.buttonSettingsReload.Text = "Сбросить";
            this.buttonSettingsReload.UseVisualStyleBackColor = true;
            this.buttonSettingsReload.Click += new System.EventHandler(this.buttonSettingsReload_Click);
            // 
            // tableLayoutPanel14
            // 
            this.tableLayoutPanel14.ColumnCount = 1;
            this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.Controls.Add(this.listViewPages, 0, 0);
            this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel15, 0, 1);
            this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel14.Location = new System.Drawing.Point(563, 3);
            this.tableLayoutPanel14.Name = "tableLayoutPanel14";
            this.tableLayoutPanel14.RowCount = 2;
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel14.Size = new System.Drawing.Size(680, 417);
            this.tableLayoutPanel14.TabIndex = 1;
            // 
            // listViewPages
            // 
            this.listViewPages.CheckBoxes = true;
            this.listViewPages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewPages.FullRowSelect = true;
            this.listViewPages.GridLines = true;
            this.listViewPages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewPages.HideSelection = false;
            this.listViewPages.Location = new System.Drawing.Point(3, 3);
            this.listViewPages.MultiSelect = false;
            this.listViewPages.Name = "listViewPages";
            this.listViewPages.ShowItemToolTips = true;
            this.listViewPages.Size = new System.Drawing.Size(674, 376);
            this.listViewPages.TabIndex = 3;
            this.listViewPages.UseCompatibleStateImageBehavior = false;
            this.listViewPages.View = System.Windows.Forms.View.Details;
            this.listViewPages.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewPages_ItemChecked);
            this.listViewPages.SelectedIndexChanged += new System.EventHandler(this.listViewPages_SelectedIndexChanged);
            this.listViewPages.DoubleClick += new System.EventHandler(this.listViewPages_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "№";
            this.columnHeader1.Width = 40;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Категория";
            this.columnHeader2.Width = 250;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 6;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Controls.Add(this.buttonViewAdd, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.buttonViewEdit, 1, 0);
            this.tableLayoutPanel15.Controls.Add(this.buttonViewDel, 2, 0);
            this.tableLayoutPanel15.Controls.Add(this.buttonViewUp, 3, 0);
            this.tableLayoutPanel15.Controls.Add(this.buttonViewDown, 4, 0);
            this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel15.Location = new System.Drawing.Point(3, 385);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(674, 29);
            this.tableLayoutPanel15.TabIndex = 0;
            // 
            // buttonViewAdd
            // 
            this.buttonViewAdd.Location = new System.Drawing.Point(3, 3);
            this.buttonViewAdd.Name = "buttonViewAdd";
            this.buttonViewAdd.Size = new System.Drawing.Size(74, 23);
            this.buttonViewAdd.TabIndex = 0;
            this.buttonViewAdd.Text = "Add";
            this.buttonViewAdd.UseVisualStyleBackColor = true;
            this.buttonViewAdd.Click += new System.EventHandler(this.buttonViewAdd_Click);
            // 
            // buttonViewEdit
            // 
            this.buttonViewEdit.Enabled = false;
            this.buttonViewEdit.Location = new System.Drawing.Point(83, 3);
            this.buttonViewEdit.Name = "buttonViewEdit";
            this.buttonViewEdit.Size = new System.Drawing.Size(74, 23);
            this.buttonViewEdit.TabIndex = 1;
            this.buttonViewEdit.Text = "Edit";
            this.buttonViewEdit.UseVisualStyleBackColor = true;
            this.buttonViewEdit.Click += new System.EventHandler(this.buttonViewEdit_Click);
            // 
            // buttonViewDel
            // 
            this.buttonViewDel.Enabled = false;
            this.buttonViewDel.Location = new System.Drawing.Point(163, 3);
            this.buttonViewDel.Name = "buttonViewDel";
            this.buttonViewDel.Size = new System.Drawing.Size(74, 23);
            this.buttonViewDel.TabIndex = 2;
            this.buttonViewDel.Text = "Delete";
            this.buttonViewDel.UseVisualStyleBackColor = true;
            this.buttonViewDel.Click += new System.EventHandler(this.buttonViewDel_Click);
            // 
            // buttonViewUp
            // 
            this.buttonViewUp.Enabled = false;
            this.buttonViewUp.Location = new System.Drawing.Point(243, 3);
            this.buttonViewUp.Name = "buttonViewUp";
            this.buttonViewUp.Size = new System.Drawing.Size(74, 23);
            this.buttonViewUp.TabIndex = 3;
            this.buttonViewUp.Text = "Up";
            this.buttonViewUp.UseVisualStyleBackColor = true;
            this.buttonViewUp.Click += new System.EventHandler(this.buttonViewUp_Click);
            // 
            // buttonViewDown
            // 
            this.buttonViewDown.Enabled = false;
            this.buttonViewDown.Location = new System.Drawing.Point(323, 3);
            this.buttonViewDown.Name = "buttonViewDown";
            this.buttonViewDown.Size = new System.Drawing.Size(74, 23);
            this.buttonViewDown.TabIndex = 4;
            this.buttonViewDown.Text = "Down";
            this.buttonViewDown.UseVisualStyleBackColor = true;
            this.buttonViewDown.Click += new System.EventHandler(this.buttonViewDown_Click);
            // 
            // metroSetControlBox1
            // 
            this.metroSetControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroSetControlBox1.CloseHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.metroSetControlBox1.CloseHoverForeColor = System.Drawing.Color.White;
            this.metroSetControlBox1.CloseNormalForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.DisabledForeColor = System.Drawing.Color.DimGray;
            this.metroSetControlBox1.IsDerivedStyle = true;
            this.metroSetControlBox1.Location = new System.Drawing.Point(1433, 31);
            this.metroSetControlBox1.MaximizeBox = true;
            this.metroSetControlBox1.MaximizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.metroSetControlBox1.MaximizeHoverForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.MaximizeNormalForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.MinimizeBox = true;
            this.metroSetControlBox1.MinimizeHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.metroSetControlBox1.MinimizeHoverForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.MinimizeNormalForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.Name = "metroSetControlBox1";
            this.metroSetControlBox1.Size = new System.Drawing.Size(100, 25);
            this.metroSetControlBox1.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetControlBox1.StyleManager = null;
            this.metroSetControlBox1.TabIndex = 1;
            this.metroSetControlBox1.Text = "metroSetControlBox1";
            this.metroSetControlBox1.ThemeAuthor = "Narwin";
            this.metroSetControlBox1.ThemeName = "MetroLite";
            this.metroSetControlBox1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_TickAsync);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderThickness = 0F;
            this.ClientSize = new System.Drawing.Size(1284, 612);
            this.Controls.Add(this.metroSetControlBox1);
            this.Controls.Add(this.metroSetTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(12, 90, 12, 12);
            this.ShowBorder = true;
            this.ShowIcon = false;
            this.ShowTitle = false;
            this.SmallRectThickness = 15;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Статистика";
            this.TextColor = System.Drawing.Color.DimGray;
            this.UseSlideAnimation = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_LoadAsync);
            this.metroSetTabControl1.ResumeLayout(false);
            this.metroSetSetTabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOneShift)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown4)).EndInit();
            this.metroSetSetTabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.metroSetSetTabPage6.ResumeLayout(false);
            this.tableLayoutPanel19.ResumeLayout(false);
            this.tableLayoutPanel20.ResumeLayout(false);
            this.tableLayoutPanel21.ResumeLayout(false);
            this.tableLayoutPanel22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableLayoutPanel23.ResumeLayout(false);
            this.tableLayoutPanel23.PerformLayout();
            this.metroSetSetTabPage9.ResumeLayout(false);
            this.tableLayoutPanel31.ResumeLayout(false);
            this.tableLayoutPanel32.ResumeLayout(false);
            this.tableLayoutPanel33.ResumeLayout(false);
            this.tableLayoutPanel34.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartStatYear)).EndInit();
            this.tableLayoutPanel35.ResumeLayout(false);
            this.tableLayoutPanel35.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewYearStatistic)).EndInit();
            this.metroSetSetTabPage7.ResumeLayout(false);
            this.tableLayoutPanel24.ResumeLayout(false);
            this.tableLayoutPanel25.ResumeLayout(false);
            this.tableLayoutPanel26.ResumeLayout(false);
            this.tableLayoutPanel27.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartUserMakereadyCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartUserAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartUserWorkingOutput)).EndInit();
            this.metroSetSetTabPage8.ResumeLayout(false);
            this.tableLayoutPanel28.ResumeLayout(false);
            this.tableLayoutPanel29.ResumeLayout(false);
            this.tableLayoutPanel30.ResumeLayout(false);
            this.metroSetSetTabPage5.ResumeLayout(false);
            this.tableLayoutPanel17.ResumeLayout(false);
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel18.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrderDetails)).EndInit();
            this.metroSetSetTabPage3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown11)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown3)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.metroSetSetTabPage4.ResumeLayout(false);
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel11.ResumeLayout(false);
            this.tableLayoutPanel11.PerformLayout();
            this.tableLayoutPanel12.ResumeLayout(false);
            this.tableLayoutPanel13.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown5)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown6)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tableLayoutPanel14.ResumeLayout(false);
            this.tableLayoutPanel15.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroSet_UI.Controls.MetroSetTabControl metroSetTabControl1;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage1;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetroSet_UI.Controls.MetroSetControlBox metroSetControlBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage3;
        private MetroSet_UI.Controls.MetroSetSwitch metroSetSwitch1;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button button1;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage4;
        private DoubleBufferedDataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel7;
        private MyListView listViewEquips;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader17;
        private MyListView listViewCategory;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private TableLayoutPanel tableLayoutPanel8;
        private TableLayoutPanel tableLayoutPanel9;
        private Button buttonCatAdd;
        private Button buttonCatEdit;
        private Button buttonCatDelete;
        private Button buttonCatUp;
        private Button buttonCatDown;
        private Button buttonEquipAdd;
        private Button buttonEquipDel;
        private Button buttonEquipUp;
        private Button buttonEquipDown;
        private TableLayoutPanel tableLayoutPanel6;
        private GroupBox groupBox1;
        private FormattedNumericUpDown formattedNumericUpDown1;
        private FormattedNumericUpDown formattedNumericUpDown2;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private FormattedNumericUpDown formattedNumericUpDown3;
        private GroupBox groupBox3;
        private MetroSet_UI.Controls.MetroSetCheckBox metroSetCheckBox1;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel1;
        private MetroSet_UI.Controls.MetroSetSwitch metroSetSwitch2;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel2;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel3;
        private FormattedNumericUpDown formattedNumericUpDown4;
        private Timer timer1;
        private Button button2;
        private DoubleBufferedDataGridView dataGridViewOneShift;
        private GroupBox groupBox4;
        private MetroSet_UI.Controls.MetroSetCheckBox metroSetCheckBox2;
        private TableLayoutPanel tableLayoutPanel10;
        private TableLayoutPanel tableLayoutPanel11;
        private ComboBox comboBox5;
        private TableLayoutPanel tableLayoutPanel12;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel13;
        private GroupBox groupBox5;
        private Label label4;
        private FormattedNumericUpDown formattedNumericUpDown5;
        private MetroSet_UI.Controls.MetroSetCheckBox metroSetCheckBox3;
        private GroupBox groupBox6;
        private Label label7;
        private FormattedNumericUpDown formattedNumericUpDown9;
        private Label label6;
        private FormattedNumericUpDown formattedNumericUpDown7;
        private Label label5;
        private FormattedNumericUpDown formattedNumericUpDown6;
        private MetroSet_UI.Controls.MetroSetCheckBox metroSetCheckBox4;
        private Label label8;
        private FormattedNumericUpDown formattedNumericUpDown8;
        private GroupBox groupBox8;
        private MetroSet_UI.Controls.MetroSetCheckBox metroSetCheckBox5;
        private TableLayoutPanel tableLayoutPanel14;
        private MyListView listViewPages;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TableLayoutPanel tableLayoutPanel15;
        private Button buttonViewAdd;
        private Button buttonViewEdit;
        private Button buttonViewDel;
        private Button buttonViewUp;
        private Button buttonViewDown;
        private Button buttonEditViewPath;
        private Button buttonDelViewPath;
        private TableLayoutPanel tableLayoutPanel16;
        private Button buttonSettingsSave;
        private Button buttonSettingsReload;
        private GroupBox groupBox7;
        private MetroSet_UI.Controls.MetroSetCheckBox metroSetCheckBox6;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage5;
        private TableLayoutPanel tableLayoutPanel17;
        private TableLayoutPanel tableLayoutPanel18;
        private TextBox textBox1;
        private Button button3;
        private ComboBox comboBox6;
        private DoubleBufferedDataGridView dataGridViewOrderDetails;
        private Button buttonPreview;
        private Button buttonNext;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage6;
        private TableLayoutPanel tableLayoutPanel19;
        private TableLayoutPanel tableLayoutPanel20;
        private ComboBox comboBoxStatCategory;
        private ComboBox comboBoxStatMonth;
        private ComboBox comboBoxStatYear;
        private Button button4;
        private TableLayoutPanel tableLayoutPanel21;
        private TableLayoutPanel tableLayoutPanel22;
        private ComboBox comboBoxStatType;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private ProgressBar progressBar1;
        private MyListView ListViewWorkingOutCategory;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private TableLayoutPanel tableLayoutPanel23;
        private Label labelCategoryStatisticCaption;
        private Label labelCategoryStatisticValue;
        private ColumnHeader columnHeader8;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage7;
        private TableLayoutPanel tableLayoutPanel24;
        private TableLayoutPanel tableLayoutPanel25;
        private ComboBox comboBoxUserNames;
        private ComboBox comboBoxUserYears;
        private ComboBox comboBoxUserCategory;
        private Button buttonUpdateUserStatistic;
        private ProgressBar progressBarUserStatistic;
        private TableLayoutPanel tableLayoutPanel26;
        private TableLayoutPanel tableLayoutPanel27;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartUserAmount;
        private MyListView ListViewUserWorking;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader13;
        private ColumnHeader columnHeader14;
        private ColumnHeader columnHeader15;
        private ColumnHeader columnHeader16;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartUserMakereadyCount;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartUserWorkingOutput;
        private ColumnHeader columnHeader18;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage8;
        private TableLayoutPanel tableLayoutPanel28;
        private TableLayoutPanel tableLayoutPanel29;
        private ComboBox comboBoxPlanEquips;
        private Button button5;
        private ProgressBar progressBar2;
        private TableLayoutPanel tableLayoutPanel30;
        private MyListView listViewPlan;
        private ColumnHeader columnHeader19;
        private ColumnHeader columnHeader20;
        private ColumnHeader columnHeader21;
        private ColumnHeader columnHeader22;
        private ColumnHeader columnHeader23;
        private ColumnHeader columnHeader24;
        private ColumnHeader columnHeader25;
        private ComboBox comboBoxPlanCategory;
        private MetroSet_UI.Controls.MetroSetSwitch metroSetSwitch3;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel4;
        private ColumnHeader columnHeader26;
        private ColumnHeader columnHeader27;
        private ColumnHeader columnHeader28;
        private MetroSet_UI.Controls.MetroSetLabel metroSetLabel5;
        private MetroSet_UI.Controls.MetroSetSwitch metroSetSwitchUserStatistic;
        private ColumnHeader columnHeader29;
        private ColumnHeader columnHeader30;
        private ComboBox comboBox7;
        private GroupBox groupBox9;
        private Label label9;
        private Label label10;
        private FormattedNumericUpDown formattedNumericUpDown10;
        private FormattedNumericUpDown formattedNumericUpDown11;
        private Button button6;
        private MetroSet_UI.Child.MetroSetSetTabPage metroSetSetTabPage9;
        private TableLayoutPanel tableLayoutPanel31;
        private TableLayoutPanel tableLayoutPanel32;
        private ComboBox comboBoxStatYearTypeViewSelect;
        private ComboBox comboBoxStatYearCategorySelect;
        private ComboBox comboBoxStatYearEquipSelect;
        private ComboBox comboBoxStatYearSelectYear;
        private Button buttonStatYearUpdate;
        private ProgressBar progressBarStatYear;
        private TableLayoutPanel tableLayoutPanel33;
        private TableLayoutPanel tableLayoutPanel34;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStatYear;
        private TableLayoutPanel tableLayoutPanel35;
        private Label labelYearStatisticCaption;
        private Label labelYearStatisticValue;
        private DataGridView dataGridViewYearStatistic;
        private Button buttonExcelExportyearStatistic;
    }
}

