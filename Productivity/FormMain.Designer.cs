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
            this.metroSetSetTabPage2 = new MetroSet_UI.Child.MetroSetSetTabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new Productivity.FormMain.DoubleBufferedDataGridView();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.formattedNumericUpDown2 = new Productivity.FormattedNumericUpDown();
            this.formattedNumericUpDown1 = new Productivity.FormattedNumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.formattedNumericUpDown3 = new Productivity.FormattedNumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.metroSetCheckBox1 = new MetroSet_UI.Controls.MetroSetCheckBox();
            this.metroSetSetTabPage4 = new MetroSet_UI.Child.MetroSetSetTabPage();
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
            this.metroSetSetTabPage3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown3)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroSetTabControl1
            // 
            this.metroSetTabControl1.AnimateEasingType = MetroSet_UI.Enums.EasingType.SineInOut;
            this.metroSetTabControl1.AnimateTime = 200;
            this.metroSetTabControl1.BackgroundColor = System.Drawing.Color.White;
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage1);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage2);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage3);
            this.metroSetTabControl1.Controls.Add(this.metroSetSetTabPage4);
            this.metroSetTabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.metroSetTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroSetTabControl1.IsDerivedStyle = true;
            this.metroSetTabControl1.ItemSize = new System.Drawing.Size(130, 38);
            this.metroSetTabControl1.Location = new System.Drawing.Point(12, 90);
            this.metroSetTabControl1.Name = "metroSetTabControl1";
            this.metroSetTabControl1.SelectedIndex = 0;
            this.metroSetTabControl1.SelectedTextColor = System.Drawing.Color.White;
            this.metroSetTabControl1.Size = new System.Drawing.Size(1276, 518);
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
            this.metroSetTabControl1.SelectedIndexChanged += new System.EventHandler(this.metroSetTabControl1_SelectedIndexChanged);
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
            this.metroSetSetTabPage1.Size = new System.Drawing.Size(1268, 472);
            this.metroSetSetTabPage1.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage1.StyleManager = null;
            this.metroSetSetTabPage1.TabIndex = 0;
            this.metroSetSetTabPage1.Text = "Статистика смены";
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1268, 472);
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
            this.dataGridViewOneShift.Size = new System.Drawing.Size(1262, 430);
            this.dataGridViewOneShift.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 10;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.dateTimePicker1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBox1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetSwitch1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetLabel1, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetSwitch2, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetLabel2, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.metroSetLabel3, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.formattedNumericUpDown4, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.button2, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1262, 30);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePicker1.Location = new System.Drawing.Point(3, 3);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(174, 23);
            this.dateTimePicker1.TabIndex = 0;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1 смена",
            "2 смена"});
            this.comboBox1.Location = new System.Drawing.Point(183, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(154, 24);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
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
            this.metroSetSwitch1.Location = new System.Drawing.Point(343, 3);
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
            this.metroSetLabel1.Location = new System.Drawing.Point(407, 0);
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
            this.metroSetSwitch2.Location = new System.Drawing.Point(787, 3);
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
            this.metroSetLabel2.Location = new System.Drawing.Point(851, 0);
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
            this.metroSetLabel3.Location = new System.Drawing.Point(1051, 0);
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
            this.formattedNumericUpDown4.Location = new System.Drawing.Point(991, 3);
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
            this.button2.Location = new System.Drawing.Point(627, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(154, 24);
            this.button2.TabIndex = 12;
            this.button2.Text = "Обновление";
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.metroSetSetTabPage2.Size = new System.Drawing.Size(1268, 472);
            this.metroSetSetTabPage2.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage2.StyleManager = null;
            this.metroSetSetTabPage2.TabIndex = 1;
            this.metroSetSetTabPage2.Text = "Статистик за месяц";
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
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1268, 472);
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
            this.dataGridView1.Size = new System.Drawing.Size(1262, 430);
            this.dataGridView1.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 5;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.comboBox4, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBox3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.comboBox2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button1, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1262, 30);
            this.tableLayoutPanel4.TabIndex = 2;
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
            this.comboBox4.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
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
            this.comboBox3.SelectedIndexChanged += new System.EventHandler(this.comboBox3_SelectedIndexChanged);
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
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.ForeColor = System.Drawing.Color.Gray;
            this.button1.Location = new System.Drawing.Point(483, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 24);
            this.button1.TabIndex = 6;
            this.button1.Text = "Обновить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.metroSetSetTabPage3.Size = new System.Drawing.Size(1268, 472);
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
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1268, 472);
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
            this.tableLayoutPanel7.Size = new System.Drawing.Size(1262, 466);
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
            this.listViewCategory.Size = new System.Drawing.Size(309, 418);
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
            this.listViewEquips.Location = new System.Drawing.Point(318, 3);
            this.listViewEquips.MultiSelect = false;
            this.listViewEquips.Name = "listViewEquips";
            this.listViewEquips.ShowItemToolTips = true;
            this.listViewEquips.Size = new System.Drawing.Size(309, 418);
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
            this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 427);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(309, 36);
            this.tableLayoutPanel8.TabIndex = 3;
            // 
            // buttonCatDown
            // 
            this.buttonCatDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCatDown.Enabled = false;
            this.buttonCatDown.Location = new System.Drawing.Point(247, 3);
            this.buttonCatDown.Name = "buttonCatDown";
            this.buttonCatDown.Size = new System.Drawing.Size(59, 30);
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
            this.tableLayoutPanel9.Location = new System.Drawing.Point(318, 427);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(309, 36);
            this.tableLayoutPanel9.TabIndex = 4;
            // 
            // buttonEquipAdd
            // 
            this.buttonEquipAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipAdd.Enabled = false;
            this.buttonEquipAdd.Location = new System.Drawing.Point(3, 3);
            this.buttonEquipAdd.Name = "buttonEquipAdd";
            this.buttonEquipAdd.Size = new System.Drawing.Size(71, 30);
            this.buttonEquipAdd.TabIndex = 1;
            this.buttonEquipAdd.Text = "Add";
            this.buttonEquipAdd.UseVisualStyleBackColor = true;
            this.buttonEquipAdd.Click += new System.EventHandler(this.buttonEquipAdd_Click);
            // 
            // buttonEquipDel
            // 
            this.buttonEquipDel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipDel.Enabled = false;
            this.buttonEquipDel.Location = new System.Drawing.Point(80, 3);
            this.buttonEquipDel.Name = "buttonEquipDel";
            this.buttonEquipDel.Size = new System.Drawing.Size(71, 30);
            this.buttonEquipDel.TabIndex = 2;
            this.buttonEquipDel.Text = "Del";
            this.buttonEquipDel.UseVisualStyleBackColor = true;
            this.buttonEquipDel.Click += new System.EventHandler(this.buttonEquipDel_Click);
            // 
            // buttonEquipUp
            // 
            this.buttonEquipUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipUp.Enabled = false;
            this.buttonEquipUp.Location = new System.Drawing.Point(157, 3);
            this.buttonEquipUp.Name = "buttonEquipUp";
            this.buttonEquipUp.Size = new System.Drawing.Size(71, 30);
            this.buttonEquipUp.TabIndex = 3;
            this.buttonEquipUp.Text = "Up";
            this.buttonEquipUp.UseVisualStyleBackColor = true;
            this.buttonEquipUp.Click += new System.EventHandler(this.buttonEquipUp_Click);
            // 
            // buttonEquipDown
            // 
            this.buttonEquipDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquipDown.Enabled = false;
            this.buttonEquipDown.Location = new System.Drawing.Point(234, 3);
            this.buttonEquipDown.Name = "buttonEquipDown";
            this.buttonEquipDown.Size = new System.Drawing.Size(72, 30);
            this.buttonEquipDown.TabIndex = 4;
            this.buttonEquipDown.Text = "Down";
            this.buttonEquipDown.UseVisualStyleBackColor = true;
            this.buttonEquipDown.Click += new System.EventHandler(this.buttonEquipDown_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(633, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 5;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(309, 418);
            this.tableLayoutPanel6.TabIndex = 5;
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
            this.groupBox1.Size = new System.Drawing.Size(303, 64);
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
            this.groupBox2.Location = new System.Drawing.Point(3, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(303, 64);
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
            this.groupBox3.Location = new System.Drawing.Point(3, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(303, 64);
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
            // metroSetSetTabPage4
            // 
            this.metroSetSetTabPage4.BaseColor = System.Drawing.Color.White;
            this.metroSetSetTabPage4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.metroSetSetTabPage4.Font = null;
            this.metroSetSetTabPage4.ImageIndex = 0;
            this.metroSetSetTabPage4.ImageKey = null;
            this.metroSetSetTabPage4.IsDerivedStyle = true;
            this.metroSetSetTabPage4.Location = new System.Drawing.Point(4, 42);
            this.metroSetSetTabPage4.Name = "metroSetSetTabPage4";
            this.metroSetSetTabPage4.Size = new System.Drawing.Size(1268, 472);
            this.metroSetSetTabPage4.Style = MetroSet_UI.Enums.Style.Light;
            this.metroSetSetTabPage4.StyleManager = null;
            this.metroSetSetTabPage4.TabIndex = 3;
            this.metroSetSetTabPage4.Text = "Параметры монитора";
            this.metroSetSetTabPage4.ThemeAuthor = "Narwin";
            this.metroSetSetTabPage4.ThemeName = "MetroLite";
            this.metroSetSetTabPage4.ToolTipText = null;
            // 
            // metroSetControlBox1
            // 
            this.metroSetControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.metroSetControlBox1.CloseHoverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.metroSetControlBox1.CloseHoverForeColor = System.Drawing.Color.White;
            this.metroSetControlBox1.CloseNormalForeColor = System.Drawing.Color.Gray;
            this.metroSetControlBox1.DisabledForeColor = System.Drawing.Color.DimGray;
            this.metroSetControlBox1.IsDerivedStyle = true;
            this.metroSetControlBox1.Location = new System.Drawing.Point(1206, -4);
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
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1300, 620);
            this.Controls.Add(this.metroSetControlBox1);
            this.Controls.Add(this.metroSetTabControl1);
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(12, 90, 12, 12);
            this.ShowBorder = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Статистика";
            this.TextColor = System.Drawing.SystemColors.GrayText;
            this.UseSlideAnimation = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
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
            this.metroSetSetTabPage3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formattedNumericUpDown3)).EndInit();
            this.groupBox3.ResumeLayout(false);
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
    }
}

