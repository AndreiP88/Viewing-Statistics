using libData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace libINIFile
{
    public class ValuePagesList
    {
        private string iniFile;

        public ValuePagesList(string iniFile)
        {
            this.iniFile = iniFile;
        }

        public ValuePagesList()
        {
            this.iniFile = "view.ini";
        }

        public List<Page> LoadPagesList(bool loadAllPages = true)
        {
            List<Page> pageList = new List<Page>();

            string startStrSection = "page_";

            IniFile ini = new IniFile(iniFile);

            string[] sections = ini.GetAllSections();

            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].StartsWith(startStrSection))
                {
                    if (ini.KeyExists("activePage", sections[i]))
                    {
                        int idCategory = Convert.ToInt32(sections[i].Substring(startStrSection.Length));

                        bool activePage = ini.ReadBool("activePage", sections[i]);
                        string name = ini.ReadString("name", sections[i]);
                        int typePage = ini.ReadInt("typePage", sections[i]);
                        int timeForView = ini.ReadInt("timeForView", sections[i]);
                        List<string> categoryesNames = ini.ReadString("categoryesNames", sections[i])?.Split(';')?.ToList();
                        List<string> categoryAndEquips = ini.ReadString("categoryAndEquips", sections[i])?.Split(';')?.ToList();
                        List<string> outValues = ini.ReadString("outValues", sections[i])?.Split(';')?.ToList();
                        int typeLoad = ini.ReadInt("typeLoad", sections[i]);
                        string nameMediaFile = ini.ReadString("nameMediaFile", sections[i]);

                        if (loadAllPages)
                        {
                            pageList.Add(new Page(
                                idCategory,
                                typePage,
                                name,
                                activePage,
                                timeForView,
                                categoryesNames,
                                categoryAndEquips,
                                outValues,
                                typeLoad,
                                nameMediaFile
                            ));
                        }
                        else
                        {
                            if (activePage)
                            {
                                pageList.Add(new Page(
                                    idCategory,
                                    typePage,
                                    name,
                                    activePage,
                                    timeForView,
                                    categoryesNames,
                                    categoryAndEquips,
                                    outValues,
                                    typeLoad,
                                    nameMediaFile
                                ));
                            }
                        }
                        

                        if (ini.ReadBool("activePage", sections[i]))
                        {
                            
                        }
                    }

                    //equipsForCategory = equipsStr?.Split(';')?.Select(Int32.Parse)?.ToList();

                    //List<string> strings = equipsStr?.Split(';')?.ToList();
                }
            }
            //categoryes.Add(Convert.ToInt32(sections[i].Substring(startStrSection.Length)));
            return pageList;
        }

        public Page LoadPage(int pageID)
        {
            string startStrSection = "page_";

            IniFile ini = new IniFile(iniFile);

            string section = startStrSection + pageID;

            int idCategory = Convert.ToInt32(section.Substring(startStrSection.Length));

            bool activePage = ini.ReadBool("activePage", section);
            string name = ini.ReadString("name", section);
            int typePage = ini.ReadInt("typePage", section);
            int timeForView = ini.ReadInt("timeForView", section);
            List<string> categoryesNames = ini.ReadString("categoryesNames", section)?.Split(';')?.ToList();
            List<string> categoryAndEquips = ini.ReadString("categoryAndEquips", section)?.Split(';')?.ToList();
            List<string> outValues = ini.ReadString("outValues", section)?.Split(';')?.ToList();
            int typeLoad = ini.ReadInt("typeLoad", section);
            string nameMediaFile = ini.ReadString("nameMediaFile", section);

            Page page = (new Page(
                    idCategory,
                    typePage,
                    name,
                    activePage,
                    timeForView,
                    categoryesNames,
                    categoryAndEquips,
                    outValues,
                    typeLoad,
                    nameMediaFile
                ));

            return page;
        }

        public int GetCountOutValue(List<string> list)
        {
            int count = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == "1")
                {
                    count++;
                }
            }

            return count;
        }
    }
}
