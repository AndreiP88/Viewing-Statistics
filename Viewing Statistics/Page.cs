using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewing_Statistics
{
    internal class Page
    {
        public int Id;
        public int TypePage;
        public string Name;
        //public bool ActivePage;
        public int TimeForView;
        public List<string> CategoryesNames;
        public List<string> CategoryAndEquips;
        public string NameMediaFile;
        public Page(int id, int typePage, string name, int timeForView, List<string> categoryesNames, List<string> categoryAndEquips, string nameMediaFile)
        {
            this.Id = id;
            this.TypePage = typePage;
            this.Name = name;
            this.TimeForView = timeForView;
            this.CategoryesNames = categoryesNames;
            this.CategoryAndEquips = categoryAndEquips;
            this.NameMediaFile = nameMediaFile;
        }
    }
}
