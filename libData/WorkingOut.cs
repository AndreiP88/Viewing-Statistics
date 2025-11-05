using System.Collections.Generic;

namespace libData
{
    public class WorkingOut
    {
        public int Id;
        public int Equip;
        //public string ShiftDate;
        //public int ShiftNumber;
        public List<WorkingOutValue> WorkingOutList;
        public float WorkingOutSumm;
        public float WorkingOutBacklog;
        public float BonusWorkingOutSumm;
        public List<float> PercentsWorkingOut = new List<float>();
        public int NumberOfShiftsWorked;
        public int NumberOfIdleShifts;

        //public Equips(int equip,  string shiftDate, int shiftNumber)
        public WorkingOut(int id)
        {
            Id = id;
            //ShiftDate = shiftDate;
            //ShiftNumber = shiftNumber;
        }

        public WorkingOut(int id, int equip)
        {
            Id = id;
            Equip = equip;
        }
    }
}
