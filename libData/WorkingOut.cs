using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libData
{
    public class WorkingOut
    {
        public int Id;
        //public string ShiftDate;
        //public int ShiftNumber;
        public List<WorkingOutValue> WorkingOutList;
        public float WorkingOutSumm;
        public float WorkingOutBacklog;

        //public Equips(int equip,  string shiftDate, int shiftNumber)
        public WorkingOut(int id)
        {
            Id = id;
            //ShiftDate = shiftDate;
            //ShiftNumber = shiftNumber;
        }
    }
}
