using libData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libINIFile
{
    public class ValueCategoryes
    {
        public List<Category> GetSelectedCategoriesAndEquipsList()
        {
            List<Category> categories = new List<Category>();

            string startStrSection = "category_";

            IniFile ini = new IniFile("settings.ini");

            string[] sections = ini.GetAllSections();

            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].StartsWith(startStrSection))
                {
                    string name = "";
                    bool selected = false;
                    string equipsStr = "";
                    List<Equip> equipsForCategory = new List<Equip>();

                    if (ini.KeyExists("name", sections[i]))
                        name = ini.ReadString("name", sections[i]);

                    if (ini.KeyExists("selected", sections[i]))
                        selected = ini.ReadBool("selected", sections[i]);

                    if (ini.KeyExists("equips", sections[i]))
                    {
                        equipsStr = ini.ReadString("equips", sections[i]);

                        //equipsForCategory = equipsStr?.Split(';')?.Select(Int32.Parse)?.ToList();

                        List<string> strings = equipsStr?.Split(';')?.ToList();

                        for (int j = 0; j < strings.Count; j++)
                        {
                            bool selectedEquip;

                            string[] stringsEquip = strings[j]?.Split(':');

                            int idEquip = Convert.ToInt32(stringsEquip[0]);

                            if (stringsEquip[1] == "1")
                            {
                                selectedEquip = true;
                            }
                            else
                            {
                                selectedEquip = false;
                            }

                            equipsForCategory.Add(new Equip(
                                idEquip,
                                selectedEquip
                                ));
                        }
                    }

                    int idCategory = Convert.ToInt32(sections[i].Substring(startStrSection.Length));

                    categories.Add(new Category(
                        idCategory,
                        name,
                        selected,
                        equipsForCategory
                        ));
                }
                //categoryes.Add(Convert.ToInt32(sections[i].Substring(startStrSection.Length)));
            }

            categories.Sort((v, s) => v.Id.CompareTo(s.Id));

            return categories;
        }
    }
}
