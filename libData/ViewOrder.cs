using System.Collections.Generic;

namespace libData
{
    public class ViewOrder
    {
        //public int Id;
        //public string Name;
        //public string OrderNumber;
        //public string OrderName;
        public string IdletimeName;
        public int Done;
        public int Duration;
        public int Amount;
        //public int LastAmount;
        //public int NormTime;
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
            Done = 0;
            Duration = 0;
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


/*
                            dataGridViewOneShift.Rows[indexRow].Cells[0].Value = (k + 1).ToString();
                            dataGridViewOneShift.Rows[indexRow].Cells[1].Value = "    " + machines[usersShiftList[j].Equip];
                            dataGridViewOneShift.Rows[indexRow].Cells[2].Value = order.OrderNumber;
                            dataGridViewOneShift.Rows[indexRow].Cells[3].Value = order.OrderName;
                            dataGridViewOneShift.Rows[indexRow].Cells[4].Value = idletimeName;

                            //тут еще ничего не работает
                            //dataGridViewOneShift.Rows[indexRow].Cells[4].Value = order.PlanOutQty.ToString("N0") + " | " + order.FactOutQty.ToString("N0");
                            dataGridViewOneShift.Rows[indexRow].Cells[5].Value = lastAmount.ToString("N0") + " | " + amount.ToString("N0");
                            dataGridViewOneShift.Rows[indexRow].Cells[6].Value = time.MinuteToTimeString(normTimeFull);
                            dataGridViewOneShift.Rows[indexRow].Cells[7].Value = order.DateBegin;
                            //dataGridViewOneShift.Rows[indexRow].Cells[8].Value = user.Shifts[0].Orders[indexesUserShiftsOrders[indexesUserShiftsOrders.Count - 1]].DateEnd;
                            dataGridViewOneShift.Rows[indexRow].Cells[8].Value = timeEnd;
                            dataGridViewOneShift.Rows[indexRow].Cells[9].Value = time.MinuteToTimeString(duration);
                            dataGridViewOneShift.Rows[indexRow].Cells[10].Value = timePlanedEndOrder;
                            dataGridViewOneShift.Rows[indexRow].Cells[11].Value = time.MinuteToTimeString(differentTime);
                            dataGridViewOneShift.Rows[indexRow].Cells[12].Value = done.ToString("N0");
                            dataGridViewOneShift.Rows[indexRow].Cells[13].Value = time.MinuteToTimeString((int)Math.Round(workingOut));
 */