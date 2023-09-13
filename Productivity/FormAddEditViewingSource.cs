using libData;
using libINIFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Productivity
{
    public partial class FormAddEditViewingSource : Form
    {
        int ViewID = -1;

        public FormAddEditViewingSource()
        {
            InitializeComponent();
        }

        public FormAddEditViewingSource(int viewID)
        {
            InitializeComponent();

            this.ViewID = viewID;
        }

        bool accept = false;

        public bool NewValue
        {
            get
            {
                return accept;
            }
            set
            {
                accept = value;
            }
        }

        private bool SaveView()
        {
            bool result = false;

            if (textBox1.Text != "" && textBox2.Text != "")
            {
                ValueView valueView = new ValueView();

                ViewPath view = new ViewPath(
                textBox1.Text,
                textBox2.Text
                );

                if (ViewID == -1)
                {
                    valueView.AddNewView(view);
                }
                else
                {
                    valueView.SaveView(view, ViewID);
                }

                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SaveView())
            {
                NewValue = true;

                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewValue = false;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void FormAddEditViewingSource_Load(object sender, EventArgs e)
        {
            if (ViewID != -1)
            {
                ValueView valueView = new ValueView();

                ViewPath view = valueView.LoadView(ViewID);

                textBox1.Text = view.Name;
                textBox2.Text = view.Path;

                //openFileDialog1.FileName = view.Path;
                openFileDialog1.InitialDirectory = Path.GetDirectoryName(view.Path);

                button1.Text = "Сохранить";
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";

                button1.Text = "Добавить";
            }
        }
    }
}
