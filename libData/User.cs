using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libData
{
    public class User
    {
        public int Id;
        //public int Equip;
        public List<UserShift> Shifts;
        public float WorkingOutUser;
        public float WorkingOutBacklog;

        public User(int id)
        {
            this.Id = id;
            //this.Equip = equip;
        }
    }
}
