using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Productivity
{
    public partial class FormAddEditCategory : Form
    {
        bool _edit = false;
        int idCategory = -1;

        public FormAddEditCategory()
        {
            InitializeComponent();
        }

        public FormAddEditCategory(int idCategory)
        {
            InitializeComponent();

            _edit = true;
            this.idCategory = idCategory;
        }

        bool accept = false;
        private string categoryName = "";

        public string NameCategory
        {
            get
            {
                return categoryName;
            }
            set
            {
                categoryName = value;
            }
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            NewValue = true;
            NameCategory = textBox1.Text;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewValue = false;
            Close();
        }
    }
}
