using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Productivity
{
    public class WorkingOutValue
    {
        public string ShiftDate;
        public int ShiftNumber;
        public int WorkingOut;

        public WorkingOutValue(string shiftDate, int shiftNumber, int workingOut)
        {
            ShiftDate = shiftDate;
            ShiftNumber = shiftNumber;
            WorkingOut = workingOut;
        }
    }
}
