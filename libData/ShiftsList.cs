using System;

namespace libData
{
    public class ShiftsList
    {
        public int IdUser;
        public DateTime ShiftDate;
        public int ShiftNumber;
        public string ShiftDateBegin;
        public string ShiftDateEnd;

        public ShiftsList(int idUser, DateTime shiftDate, int shiftNumber, string shiftDateBegin, string shiftDateEnd) 
        {
            this.IdUser = idUser;
            this.ShiftDate = shiftDate;
            this.ShiftNumber = shiftNumber;
            this.ShiftDateBegin = shiftDateBegin;
            this.ShiftDateEnd = shiftDateEnd;
        }
    }
}
