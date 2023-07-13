using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Productivity
{
    internal class ValueDateTime
    {
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

        public string MinuteToTimeString(int totalMinutes)
        {
            string result = "00:00";
            string sign = "";

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
    }
}
