using libData;
using libINIFile;
using libSql;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Productivity
{
    public partial class FormAddEquips : Form
    {
        public FormAddEquips()
        {
            InitializeComponent();
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
            NewValue = true;
            EquipsList = GetCheckedEquips();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewValue = false;
            Close();
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
                ValueEquips equipsValue = new ValueEquips();
                
                Dictionary<int, string> machines = equipsValue.LoadMachine();

                foreach (KeyValuePair<int, string> equip in machines)
                {
                    if (!CheckEquipIsHere(equip.Key))
                    {
                        ListViewItem item = new ListViewItem();

                        item.Name = equip.Key.ToString();
                        item.Text = (listViewEquips.Items.Count + 1).ToString("D2");
                        item.SubItems.Add(equip.Value);

                        listViewEquips.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения");
            }
        }
    }
}
