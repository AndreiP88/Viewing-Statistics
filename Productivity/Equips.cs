using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Productivity
{
    public class Equips
    {
        public int Equip;
        public string ShiftDate;
        public int ShiftNumber;
        public int WorkingOut;

        public Equips(int equip,  string shiftDate, int shiftNumber)
        {
            Equip = equip;
            ShiftDate = shiftDate;
            ShiftNumber = shiftNumber;
        }
    }
}
