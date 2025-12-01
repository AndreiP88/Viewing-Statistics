namespace libData
{
    public class WorkingOutValue
    {
        public string ShiftDate;
        public int ShiftNumber;
        public float WorkingOut;
        public int NumberOfIdleShifts;

        public WorkingOutValue(string shiftDate, int shiftNumber, float workingOut)
        {
            ShiftDate = shiftDate;
            ShiftNumber = shiftNumber;
            WorkingOut = workingOut;
            NumberOfIdleShifts = -1;
        }
    }
}
