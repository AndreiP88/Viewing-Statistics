using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viewing_Statistics
{
    internal class UserShift
    {
        //public int IdUser;
        public string ShiftDate;
        public int ShiftNumber;
        public List<Order> Orders;

        public UserShift(string shiftDate, int shiftNumber) 
        {
            //this.IdUser = idUser;
            this.ShiftDate = shiftDate;
            this.ShiftNumber = shiftNumber;        
        }
    }
}
