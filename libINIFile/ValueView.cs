using libData;
using System.Collections.Generic;

namespace libINIFile
{
    public class ValueView
    {
        public List<ViewPath> LoadViewList()
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

        public ViewPath LoadView(int viewID)
        {
            string startStrSection = "viewMonitor_";

            IniFile ini = new IniFile("settings.ini");

            string section = startStrSection + viewID;

            string name = "";
            string path = "";

            if (ini.KeyExists("name", section))
            {
                name = ini.ReadString("name", section);
            }

            if (ini.KeyExists("path", section))
            {
                path = ini.ReadString("path", section);
            }

            ViewPath view = new ViewPath(
                name,
                path
                );

            return view;
        }

        public void SaveView(ViewPath viewPath, int indexViewMonitor)
        {
            string startStrSection = "viewMonitor_";

            IniFile ini = new IniFile("settings.ini");

            string section = startStrSection + indexViewMonitor;

            ini.Write("name", viewPath.Name, section);
            ini.Write("path", viewPath.Path, section);
        }

        public void AddNewView(ViewPath viewPath)
        {
            int countViews = GetCountViewingSources();

            SaveView(viewPath, countViews + 1);
        }

        public void SwapView(int firstIndex, int secondIndex)
        {
            ViewPath firstView = LoadView(firstIndex);
            ViewPath secondView = LoadView(secondIndex);

            SaveView(secondView, firstIndex);
            SaveView(firstView, secondIndex);
        }

        public void DeleteViewSource(int viewID)
        {
            string startStrSection = "viewMonitor_";

            IniFile ini = new IniFile("settings.ini");

            int countViews = GetCountViewingSources();

            for (int i = viewID; i < countViews; i++)
            {
                SwapView(i, i + 1);
            }

            string section = startStrSection + countViews;
            ini.DeleteSection(section);
        }

        public int GetCountViewingSources()
        {
            int result = 0;

            string startStrSection = "viewMonitor_";

            IniFile ini = new IniFile("settings.ini");

            string[] sections = ini.GetAllSections();

            for (int i = 0; i < sections.Length; i++)
            {
                if (sections[i].StartsWith(startStrSection))
                {
                    result++;
                }
            }

            return result;
        }


    }
}
