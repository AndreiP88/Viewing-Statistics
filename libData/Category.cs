using System.Collections.Generic;

namespace libData
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
