namespace libData
{
    public class OrderSearchValue
    {
        public int IDEquip;
        public int IDEmployee;
        public int Flags;
        public int PlanOutQTY;
        public string DateBegin;
        public string DateEnd;
        public int Duration;
        public int FactOutQTY;
        public int IDManOrderJobItem;
        public string Idletime;
        public int OperationType;

        public OrderSearchValue(int idEquip, int idEmpoyee, int flags, int planOutQTY, string dateBegin, string dateEnd, int duration, int factOutQTY, int idManOrderJobItem, string idletime, int operationType)
        {
            this.IDEquip = idEquip;
            this.IDEmployee = idEmpoyee;
            this.Flags = flags;
            this.PlanOutQTY = planOutQTY;
            this.DateBegin = dateBegin;
            this.DateEnd = dateEnd;
            this.Duration = duration;
            this.FactOutQTY = factOutQTY;
            this.IDManOrderJobItem = idManOrderJobItem;
            this.Idletime = idletime;
            this.OperationType = operationType;
        }
    }
}