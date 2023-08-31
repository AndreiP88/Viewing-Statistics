using libData;
using libINIFile;
using libSql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Productivity
{
    public partial class FormAddEditViewing : Form
    {
        private string Path;
        private int IndexPage;

        public FormAddEditViewing(string path, int indexPage)
        {
            InitializeComponent();

            this.Path = path;
            this.IndexPage = indexPage;
        }

        public FormAddEditViewing(string path)
        {
            InitializeComponent();

            this.Path = path;
            this.IndexPage = -1;
        }

        bool accept = false;
        private List<string> listEquips;

        public List<string> EquipsList
        {
            get
            {
                return listEquips;
            }
            set
            {
                listEquips = value;
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
            if (IndexPage == -1)
            {
                //AddNewPage();
            }
            else
            {
                //SaveCurreentPage();
            }

            NewValue = true;
            //EquipsList = GetCheckedEquips();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewValue = false;
            Close();
        }

        private void LoadPageValue(Page currentPage)
        {
            if (currentPage.TypePage == 0)
            {
                metroSetCheckBox1.Checked = currentPage.ActivePage;
                metroSetTextBox1.Text = currentPage.Name;
                formattedNumericUpDown1.Value = currentPage.TimeForView;
                comboBox1.SelectedIndex = currentPage.TypeLoad;

                List<string> viewing = currentPage.OutValues;

                if (viewing[0] == "0")
                {
                    metroSetCheckBox2.Checked = false;
                }
                else
                {
                    metroSetCheckBox2.Checked = true;
                }

                if (viewing[1] == "0")
                {
                    metroSetCheckBox3.Checked = false;
                }
                else
                {
                    metroSetCheckBox3.Checked = true;
                }

                if (viewing[2] == "0")
                {
                    metroSetCheckBox4.Checked = false;
                }
                else
                {
                    metroSetCheckBox4.Checked = true;
                }
            }
            else
            {

            }
        }

        private void LoadPageValueDefault()
        {
            metroSetCheckBox1.Checked = true;
            metroSetTextBox1.Text = "";
            formattedNumericUpDown1.Value = 40;
            comboBox1.SelectedIndex = 0;

            metroSetCheckBox2.Checked = true;
            metroSetCheckBox3.Checked = true;
            metroSetCheckBox4.Checked = true;
        }

        private void LoadAllEquipsAndCategory(Page page = null)
        {
            ValueCategoryes valueCategoryes = new ValueCategoryes();
            ValueEquips equipsValue = new ValueEquips();

            List<Category> categories = valueCategoryes.GetSelectedCategoriesAndEquipsList();
            Dictionary<int, string> machines = equipsValue.LoadMachine();

            for (int i = 0; i < categories.Count; i++)
            {
                Category category = categories[i];

                ListViewGroup group = new ListViewGroup();

                group.Name = category.Id.ToString();
                group.Header = category.Name;

                listViewEquips.Groups.Add(group);

                for (int j = 0; j < category.Equips.Count; j++)
                {
                    ListViewItem item = new ListViewItem(group);

                    item.Name = category.Equips[j].Id.ToString();
                    item.Text = (listViewEquips.Items.Count + 1).ToString("D2");
                    item.SubItems.Add(machines[category.Equips[j].Id]);

                    if (page != null && page.TypePage == 0)
                        item.Checked = CheckEquipFromCurrentPage(category.Equips[j].Id, page);

                    listViewEquips.Items.Add(item);
                }
            }
        }

        private bool CheckEquipFromCurrentPage(int equip, Page currentPage)
        {
            bool result = false;

            if (currentPage != null)
            {
                for (int i = 0; i < currentPage.CategoryAndEquips.Count; i++)
                {
                    string[] catEquip = currentPage.CategoryAndEquips[i].Split(':');

                    if (catEquip.Length > 0)
                    {
                        if (catEquip[1] == equip.ToString())
                        {
                            result = true;
                            break;
                        }
                        else
                            result = false;
                    }
                }
            }

            return result;
        }

        private List<string> GetCheckedEquips()
        {
            List<string> selectedEquips = new List<string>();

            for (int i = 0; i < listViewEquips.Items.Count; i++)
            {
                if (listViewEquips.Items[i].Checked)
                {
                    selectedEquips.Add(listViewEquips.Items[i].Name);
                }
            }

            return selectedEquips;
        }

        private bool CheckEquipIsHere(int idEquip)
        {
            bool result = false;

            ValueCategoryes valueCategoryes = new ValueCategoryes();
            List<Category> categories = valueCategoryes.GetSelectedCategoriesAndEquipsList();

            for (int i = 0; i < categories.Count; i++)
            {
                Category category = categories[i];

                if (category.Equips.FindIndex((v) => v.Id == idEquip) != -1)
                {
                    result = true;
                    break;
                }
                else
                    result = false;
            }

            return result;
        }

        private void FormAddEquips_Load(object sender, EventArgs e)
        {
            try
            {
                if (IndexPage != -1)
                {
                    ValuePagesList pagesList = new ValuePagesList(Path);

                    Page page = pagesList.LoadPage(IndexPage);

                    LoadPageValue(page);
                    LoadAllEquipsAndCategory(page);
                }
                else
                {
                    LoadPageValueDefault();
                    LoadAllEquipsAndCategory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }
    }
}
