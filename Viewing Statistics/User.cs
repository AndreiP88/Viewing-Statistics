using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewing_Statistics
{
    internal class User
    {
        public int Id;
        public List<UserShift> Shifts;

        public User(int id)
        {
            this.Id = id;
        }
    }
}
