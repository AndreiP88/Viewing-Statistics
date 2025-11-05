using System.Collections.Generic;

namespace libData
{
    public class User
    {
        public int Id;
        public string Name;
        //public int Equip;
        public List<UserShift> Shifts;
        public float WorkingOutUser;
        public float WorkingOutBacklog;

        public User(int id)
        {
            this.Id = id;
            //this.Equip = equip;
        }
        public User(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
