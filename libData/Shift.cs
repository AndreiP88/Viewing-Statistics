namespace libData
{
    public class Shift
    {
        public string DateShift;
        public int IdUser;
        public int ShiftNumber;
        public string ShiftStart;
        public string ShiftEnd;

        public Shift(string dateShift, int user, int shiftNum, string shiftStart, string shiftEnd)
        {
            this.DateShift = dateShift;
            this.IdUser = user;
            this.ShiftNumber = shiftNum;
            this.ShiftStart = shiftStart;
            this.ShiftEnd = shiftEnd;
        }
    }
}
