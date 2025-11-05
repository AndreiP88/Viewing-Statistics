using System.Collections.Generic;

namespace libData
{
    public class UserShift
    {
        //public int IdUser;
        public string ShiftDate;
        public int ShiftNumber;
        public string ShiftDateBegin;
        public string ShiftDateEnd;
        public List<UserShiftOrder> Orders;
        public List<int> Equips = new List<int>();
        public int WorkingOut;

        public UserShift(string shiftDate, int shiftNumber, string shiftDateBegin, string shiftDateEnd) 
        {
            //this.IdUser = idUser;
            this.ShiftDate = shiftDate;
            this.ShiftNumber = shiftNumber;
            this.ShiftDateBegin = shiftDateBegin;
            this.ShiftDateEnd = shiftDateEnd;
        }
    }
}
