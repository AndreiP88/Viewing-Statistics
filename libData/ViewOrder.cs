namespace libData
{
    public class ViewOrder
    {

        public string IdletimeName;
        public float DoneMakeReady;
        public float Done;
        public int Duration;
        public float MakeReady;
        public float Amount;
        public string TimeBegin;
        public string TimeEnd;
        public string TimePlanedEndOrder;
        public int NormTimeMakeReady;
        public int NormTimeWork;
        public int DifferentTime;
        public float WorkingOut;

        public ViewOrder()
        {
            WorkingOut = 0;
            DoneMakeReady = 0;
            Done = 0;
            Duration = 0;
            MakeReady = 0;
            Amount = 0;
            TimeEnd = "";
            TimePlanedEndOrder = "";
            NormTimeMakeReady = 0;
            NormTimeWork = 0;
            DifferentTime = 0;
            IdletimeName = "";
        }
    }
}