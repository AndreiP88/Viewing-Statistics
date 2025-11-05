namespace libData
{
    public class Equip
    {
        public int Id;
        public bool Selected;
        public string Name;

        public Equip(int id, bool selected)
        {
            this.Id = id;
            this.Selected = selected;
        }

        public Equip(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
