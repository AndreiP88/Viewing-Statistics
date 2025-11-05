using System;

namespace libTime
{
    public class ValueDateTime
    {
        public string SelectStartDateTimeFromShiftNumberAndDateForFBC(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T00:00:00.000";
            }
            else
            {
                result = date.ToString("yyyy-MM-dd") + "T08:00:00.000";
            }

            return result;
        }

        public string SelectEndDateTimeFromShiftNumberAndDateForFBC(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T20:00:00.000";
            }
            else
            {
                result = date.AddDays(1).ToString("yyyy-MM-dd") + "T08:00:00.000";
            }

            return result;
        }

        public string SelectStartDateTimeFromShiftNumberAndDateOnlyTimeForFBC(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T03:00:00.000";
            }
            else
            {
                result = date.ToString("yyyy-MM-dd") + "T15:00:00.000";
            }
            
            return result;
        }

        public string SelectEndDateTimeFromShiftNumberAndDateOnlyTimeForFBC(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T13:00:00.000";
            }
            else
            {
                result = date.AddDays(1).ToString("yyyy-MM-dd") + "T01:00:00.000";
            }

            return result;
        }

        public string SelectStartDateTimeFromShiftNumberAndDate(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T07:00:00.000";
            }
            else
            {
                result = date.ToString("yyyy-MM-dd") + "T19:00:00.000";
            }

            return result;
        }

        public string SelectEndDateTimeFromShiftNumberAndDate(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T21:00:00.000";
            }
            else
            {
                result = date.AddDays(1).ToString("yyyy-MM-dd") + "T09:00:00.000";
            }

            return result;
        }

        public string SelectStartDateTimeFromShiftNumberAndDateOnlyTime(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T08:00:00.000";
            }
            else
            {
                result = date.ToString("yyyy-MM-dd") + "T20:00:00.000";
            }

            return result;
        }

        public string SelectEndDateTimeFromShiftNumberAndDateOnlyTime(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T20:00:00.000";
            }
            else
            {
                result = date.AddDays(1).ToString("yyyy-MM-dd") + "T08:00:00.000";
            }

            return result;
        }

        public string StartShiftPlanedDateTimeForDataBase(string startShift)
        {
            string result = "";

            DateTime date = Convert.ToDateTime(startShift);

            result = date.ToString("yyyy-MM-dd") + "T" + date.ToString("HH:mm:ss:fff");

            return result;
        }

        public string StartShiftPlanedDateTimeForDataBase(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("yyyy-MM-dd") + "T08:00:00.000";
            }
            else
            {
                result = date.ToString("yyyy-MM-dd") + "T20:00:00.000";
            }

            return result;
        }

        public string StartShiftPlanedDateTime(DateTime date, int shiftNumber)
        {
            string result = "";

            if (shiftNumber == 1)
            {
                result = date.ToString("dd.MM.yyyy") + " 08:00:00";
            }
            else
            {
                result = date.ToString("dd.MM.yyyy") + " 20:00:00";
            }

            return result;
        }

        public string EndShiftPlanedDateTime(string date)
        {
            string result = "";

            DateTime dateTime = Convert.ToDateTime(date);

            result = dateTime.AddHours(12).ToString("dd.MM.yyyy HH:mm") + ":00";

            return result;
        }

        public string MinuteToTimeString(int totalMinutes, bool isNullToEmptyString = false)
        {
            string result = "00:00";
            string sign = "";

            if (isNullToEmptyString && totalMinutes == 0)
            {
                return "";
            }

            int absMinutes = Math.Abs(totalMinutes);

            if (totalMinutes  >= 0)
            {
                sign = "";
            }
            else
            {
                sign = "-";
            }

            int hours = 0;
            int minutes = absMinutes % 60;

            if (absMinutes >= 60)
            {
                hours = absMinutes / 60;
            }

            result = sign + hours.ToString("D2") + ":" + minutes.ToString("D2");

            return result;
        }

        public int HoursAndMinutesToTotalMinutes(int hours, int minutes)
        {
            int hoursToMinutes = minutes;

            hoursToMinutes += hours * 60;

            return hoursToMinutes;
        }

        public int[] TotalMinutesToHoursAndMinutes(int totalMinutes)
        {
            int minutes = Math.Abs(totalMinutes % 60);

            int hours = totalMinutes / 60;

            int[] result = new int[] { hours, minutes };

            return result;
        }

        public int DateDifferenceToMinutes(string firstDate, string secondDate)
        {
            int result;

            TimeSpan totalTime;

            DateTime firstD = StringToDateTime(firstDate);
            DateTime secondD = StringToDateTime(secondDate);

            if (firstD > secondD)
            {
                totalTime = firstD.Subtract(secondD);
                result = (int)totalTime.TotalMinutes;
            }
            else
            {
                totalTime = secondD.Subtract(firstD);
                result = (int)totalTime.TotalMinutes * (-1);
            }

            return result;
        }

        public string DateTimeAmountMunutes(string firstDate, int secondTime)
        {
            DateTime totalTime;
            DateTime firstD;

            firstD = StringToDateTime(firstDate);

            totalTime = firstD.AddMinutes(secondTime);

            return DateToSting(totalTime);
        }

        public DateTime StringToDateTime(string date)
        {
            DateTime result = DateTime.Now;

            try
            {
                if (date != "")
                {
                    result = Convert.ToDateTime(date);
                }
            }
            catch
            {
                result = DateTime.Now;
            }

            return result;
        }

        private string DateToSting(DateTime tDate)
        {
            string result = tDate.ToString("dd.MM.yyyy HH:mm");

            return result;
        }
    }
}
