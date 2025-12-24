namespace libData
{
    public class UserShiftOrder
    {
        public int IdEquip;
        public string OrderNumber;
        public string OrderName;
        public int Status;
        public int Flags;
        public string DateBegin;
        public string DateEnd;
        public int Duration;
        public float FactOutQty;
        public float PlanOutQty;
        public int Normtime;
        public int IdManOrderJobItem;
        public int IDFBCBrigade;
        public string IdletimeName;
        public int OperationType;
        public string Note;
        public string IdletimeNote;
        public string ProblemName;
        public string ProblemCause;
        public string ProblemAction;
        public int ProblemDelay;
        public int IsOrderActive;

        public UserShiftOrder(int idEquip, string orderNumber, string orderName, int status, int flags, string dateBegin, string dateEnd, int duration, float factOutQty, float planOutQty, int normtime, int idManOrderJobItem, int idFBCBrigade, string idletimeName, int operationType, string note, string idletimeNote, string problemName, string problemCause, string problemAction, int problemDelay, int isOrderActive)
        {
            this.IdEquip = idEquip;
            this.OrderNumber = orderNumber;
            this.OrderName = orderName;
            this.Status = status;
            this.Flags = flags;
            this.DateBegin = dateBegin;
            this.DateEnd = dateEnd;
            this.Duration = duration;
            this.FactOutQty = factOutQty;
            this.PlanOutQty = planOutQty;
            this.Normtime = normtime;
            this.IdManOrderJobItem = idManOrderJobItem;
            this.IDFBCBrigade = idFBCBrigade;
            this.IdletimeName = idletimeName;
            this.OperationType = operationType;
            this.Note = note;
            this.IdletimeNote = idletimeNote;
            this.ProblemName = problemName;
            this.ProblemCause = problemCause;
            this.ProblemAction = problemAction;
            this.ProblemDelay = problemDelay;
            IsOrderActive = isOrderActive;
        }
    }
}
