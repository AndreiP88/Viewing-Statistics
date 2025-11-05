using libData;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void SwapCategory(int firstCategory, int secondCategory)
        {
            IniFile ini = new IniFile("settings.ini");

            string firstSection = "category_" + firstCategory;
            string secondSection = "category_" + secondCategory;

            string firstName = "";
            bool firstSelected = false;
            string firstEquipsStr = "";

            if (ini.KeyExists("name", firstSection))
                firstName = ini.ReadString("name", firstSection);

            if (ini.KeyExists("selected", firstSection))
                firstSelected = ini.ReadBool("selected", firstSection);

            if (ini.KeyExists("equips", firstSection))
                firstEquipsStr = ini.ReadString("equips", firstSection);

            string secondName = "";
            bool secondSelected = false;
            string secondEquipsStr = "";

            if (ini.KeyExists("name", secondSection))
                secondName = ini.ReadString("name", secondSection);

            if (ini.KeyExists("selected", secondSection))
                secondSelected = ini.ReadBool("selected", secondSection);

            if (ini.KeyExists("equips", secondSection))
                secondEquipsStr = ini.ReadString("equips", secondSection);

            //ini.DeleteSection(oldSection);

            ini.Write("name", secondName, firstSection);
            ini.Write("selected", secondSelected.ToString(), firstSection);
            ini.Write("equips", secondEquipsStr, firstSection);

            ini.Write("name", firstName, secondSection);
            ini.Write("selected", firstSelected.ToString(), secondSection);
            ini.Write("equips", firstEquipsStr, secondSection);
        }

        public void ChangeCategoryID(int oldCategoryId, int newCategoryId)
        {
            IniFile ini = new IniFile("settings.ini");

            string oldSection = "category_" + oldCategoryId;
            string newSection = "category_" + newCategoryId;

            string name = "";
            bool selected = false;
            string equipsStr = "";

            if (ini.KeyExists("name", oldSection))
                name = ini.ReadString("name", oldSection);

            if (ini.KeyExists("selected", oldSection))
                selected = ini.ReadBool("selected", oldSection);

            if (ini.KeyExists("equips", oldSection))
                equipsStr = ini.ReadString("equips", oldSection);

            ini.DeleteSection(oldSection);

            ini.Write("name", name, newSection);
            ini.Write("selected", selected.ToString(), newSection);
            ini.Write("equips", equipsStr, newSection);
        }
    }
}
