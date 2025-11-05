namespace libData
{
    public class UserWorkingOutput
    {
        int userId;
        string userName;
        int amount;
        float worTime;
        float percent;
        int makeready;
        float makereadyTime;
        float bonus;
        int countShifts;

        public int UserID { get { return userId; } set { userId = value; } }
        public string UserName { get { return userName; } set { userName = value; } }
        public int Amount { get { return amount; } set { amount = value; } }
        public float Worktime { get { return worTime; } set { worTime = value; } }
        public float Percent { get { return percent; } set { percent = value; } }
        public int Makeready { get { return makeready; } set { makeready = value; } }
        public float MakereadyTime { get { return makereadyTime; } set { makereadyTime = value; } }
        public float Bonus { get { return bonus; } set { bonus = value; } }
        public int CountShifts { get { return countShifts; } set { countShifts = value; } }

        public UserWorkingOutput()
        {
            userId = 0;
            userName = string.Empty;
            amount = 0;
            worTime = 0;
            percent = 0;
            makeready = 0;
            makereadyTime = 0;
            bonus = 0;
            countShifts = 0;
        }
        public UserWorkingOutput(int id, string name, int lAmount, float lWorkTime, float lPercent, int lMakeready, float lMakereadyTime, float lBonus, int lCountShifts)
        {
            this.userId = id;
            this.userName = name;
            this.amount = lAmount;
            this.worTime = lWorkTime;
            this.percent = lPercent;
            this.makeready = lMakeready;
            this.makereadyTime = lMakereadyTime;
            this.bonus = lBonus;
            this.countShifts = lCountShifts;
        }
    }
}
