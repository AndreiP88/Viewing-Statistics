using libData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libINIFile
{
    public class ValueView
    {
        public List<ViewPath> LoadView(bool loadAllPages = true)
        {
            List<ViewPath> viewList = new List<ViewPath>();

            string startStrSection = "viewMonitor_";

            IniFile ini = new IniFile("settings.ini");

            string[] sections = ini.GetAllSections();

            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].StartsWith(startStrSection))
                {
                    string name = "";
                    string path = "";

                    if (ini.KeyExists("name", sections[i]))
                    {
                        name = ini.ReadString("name", sections[i]);
                    }

                    if (ini.KeyExists("path", sections[i]))
                    {
                        path = ini.ReadString("path", sections[i]);
                    }

                    viewList.Add(new ViewPath(
                        name,
                        path
                    ));

                }
            }
            return viewList;
        }


    }
}
