using System.Collections.Generic;

namespace libData
{
    public class PageView
    {
        public int Id;
        public int TypePage;
        public string categoryesNames;
        public string Name;
        public bool ActivePage;
        public int TimeForView;
        public List<string> CategoryesNames;
        public List<string> CategoryAndEquips;
        public List<string> OutValues;
        public int TypeLoad;
        public string NameMediaFile;

        public PageView(int id, int typePage, string name, bool activePage, int timeForView, List<string> categoryesNames, List<string> categoryAndEquips, List<string> outValues, int typeLoad, string nameMediaFile)
        {
            this.Id = id;
            this.TypePage = typePage;
            this.Name = name;
            this.ActivePage = activePage;
            this.TimeForView = timeForView;
            this.CategoryesNames = categoryesNames;
            this.CategoryAndEquips = categoryAndEquips;
            this.OutValues = outValues;
            this.TypeLoad = typeLoad;
            this.NameMediaFile = nameMediaFile;
        }
    }
}
