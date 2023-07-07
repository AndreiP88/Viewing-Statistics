using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Productivity
{
    public class Category
    {
        public int Id;
        public bool Selected;
        public string Name;
        public List<Equip> Equips;

        public Category(int id, string name, bool selected, List<Equip> equips)
        {
            this.Id = id;
            this.Name = name;
            this.Selected = selected;
            this.Equips = equips;
        }
    }
}
