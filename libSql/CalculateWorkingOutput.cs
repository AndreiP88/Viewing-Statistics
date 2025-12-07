using libData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace libSql
{
    public class CalculateWorkingOutput
    {
        public UserWorkingOutput FullWorkingOutput(int userID, DateTime selectMonth, CancellationToken token, List<int> equipList = null)
        {
            UserWorkingOutput result = new UserWorkingOutput();

            List<int> userIndex = new List<int> { userID };

            ShiftsDetails shiftsDetails = WorkingOutDetailsASLight(userIndex, selectMonth, token, equipList);

            if (shiftsDetails != null)
            {
                result.Worktime = shiftsDetails.allTimeWorkingOutShift;
                result.Amount = shiftsDetails.amountAllOrdersShift;
                result.Percent = shiftsDetails.percentWorkingOutShift;
                result.Makeready = shiftsDetails.countMakereadyShift;
                result.MakereadyTime = shiftsDetails.MakereadyWorkTime;
                result.Bonus = shiftsDetails.percentBonusShift;
                result.CountShifts = shiftsDetails?.countShifts ?? 0;
            }

            return result;
        }
        public UserWorkingOutput FullWorkingOutput(List<int> userIDs, DateTime selectMonth, CancellationToken token, List<int> equipList = null)
        {
            UserWorkingOutput result = new UserWorkingOutput();

            ShiftsDetails shiftsDetails = WorkingOutDetailsASLight(userIDs, selectMonth, token, equipList);

            if (shiftsDetails != null)
            {
                result.Worktime = shiftsDetails.allTimeWorkingOutShift;
                result.Amount = shiftsDetails.amountAllOrdersShift;
                result.Percent = shiftsDetails.percentWorkingOutShift;
                result.Makeready = shiftsDetails.countMakereadyShift;
                result.MakereadyTime = shiftsDetails.MakereadyWorkTime;
                result.Bonus = shiftsDetails.percentBonusShift;
                result.CountShifts = shiftsDetails?.countShifts ?? 0;
            }

            return result;
        }

        private ShiftsDetails WorkingOutDetailsASLight(List<int> userIndexFromAS, DateTime selectMonth, CancellationToken token, List<int> equipListAS = null)
        {
            List<ShiftsDetails> shiftsList = null;
            ShiftsDetails shiftsDetails;
            ValueShifts valueShifts = new ValueShifts();
            //ValueUserBase userBase = new ValueUserBase();

            //List<int> userIndexFromAS = userBase.GetIndexUserFromASBase(userID);

            List<User> usersList = new List<User>();

            for (int i = 0; i < userIndexFromAS.Count; i++)
            {
                usersList.Add(new User(userIndexFromAS[i]));
                usersList[usersList.Count - 1].Shifts = new List<UserShift>();
            }

            try
            {
                //shiftsList = valueShifts.LoadShiftsForSelectedMonthLight(userIndexFromAS, selectMonth, 2, 650, true, equipListAS);
                shiftsList = valueShifts.LoadShiftsForSelectedMonthFromFBCBrigadeListLight(userIndexFromAS, selectMonth, 2, 650, true, equipListAS);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            int totalAmount = 0;
            int totalCountMakeReady = 0;
            float totalMakeReadyWorkTime = 0;
            float totalTimeWorkigOut = 0;
            //float totalPercentWorkingOut = 0;
            float totalBonusWorkingOut = 0;
            List<float> totalPercentWorkingOutList = new List<float>();
            int countShifts = 0;

            for (int i = 0; i < shiftsList.Count; i++)
            {
                //countShifts += shiftsList[i].countShifts;
                totalAmount += shiftsList[i].amountAllOrdersShift;
                totalTimeWorkigOut += shiftsList[i].allTimeWorkingOutShift;
                totalCountMakeReady += shiftsList[i].countMakereadyShift;
                totalMakeReadyWorkTime += shiftsList[i].MakereadyWorkTime;
                totalBonusWorkingOut += shiftsList[i].percentBonusShift;


                if (shiftsList[i].allTimeWorkingOutShift > 0)
                {
                    totalPercentWorkingOutList.Add(shiftsList[i].percentWorkingOutShift);
                }
            }

            countShifts += shiftsList.Count;

            float percentWorkingOutAverage = 0;

            //Сделать подсчет активных смен, для расчёта выработки
            if (totalPercentWorkingOutList.Count > 0)
            {
                percentWorkingOutAverage = totalPercentWorkingOutList.Sum() / totalPercentWorkingOutList.Count;
            }

            shiftsDetails = new ShiftsDetails(
                countShifts,
                -1,
                -1,
                (int)totalTimeWorkigOut,
                -1,
                totalCountMakeReady,
                totalMakeReadyWorkTime,
                totalAmount,
                percentWorkingOutAverage,
                totalBonusWorkingOut
                );

            return shiftsDetails;
        }

        private float CalculateWorkTimeForOneOrder(UserShiftOrder order)
        {
            float workingOut = 0;

            if (order.Normtime > 0)
            {
                if (order.PlanOutQty > 0)
                {
                    workingOut += ((float)order.FactOutQty * (float)order.Normtime) / (float)order.PlanOutQty;
                }
                else
                {
                    if (order.FactOutQty > 0)
                    {
                        workingOut += (float)order.Normtime;
                    }
                }
            }

            return workingOut;
        }

        public float GetBonusWorkingOutF(int wOut)
        {
            float result = 0;

            if (wOut < 600)
            {
                result = 0f;
            }
            if (wOut >= 600 && wOut < 630)
            {
                result = 0.1f;
            }
            else if (wOut >= 630 && wOut < 660)
            {
                result = 0.12f;
            }
            else if (wOut >= 660 && wOut < 720)
            {
                result = 0.15f;
            }
            else if (wOut >= 720)
            {
                result = 0.2f;
            }

            return result;
        }
    }
}
