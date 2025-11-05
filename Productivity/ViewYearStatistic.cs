using System.Collections.Generic;

namespace Productivity
{
    internal class ViewYearStatistic
    {
        private int id;
        private string name;
        private object valueMonth1;
        private object valueMonth2;
        private object valueMonth3;
        private object valueMonth4;
        private object valueMonth5;
        private object valueMonth6;
        private object valueMonth7;
        private object valueMonth8;
        private object valueMonth9;
        private object valueMonth10;
        private object valueMonth11;
        private object valueMonth12;
        private object totallOutput;

        public ViewYearStatistic()
        {
            
        }

        public ViewYearStatistic(int Id, string Name)
        {
            id = Id;
            name = Name;
        }

        public void EnterMonthValue(int monthIndex, object value)
        {
            switch (monthIndex)
            {
                case 0:
                    valueMonth1 = value;
                    break;
                case 1:
                    valueMonth2 = value;
                    break;
                case 2:
                    valueMonth3 = value;
                    break;
                case 3:
                    valueMonth4 = value;
                    break;
                case 4:
                    valueMonth5 = value;
                    break;
                case 5:
                    valueMonth6 = value;
                    break;
                case 6:
                    valueMonth7 = value;
                    break;
                case 7:
                    valueMonth8 = value;
                    break;
                case 8:
                    valueMonth9 = value;
                    break;
                case 9:
                    valueMonth10 = value;
                    break;
                case 10:
                    valueMonth11 = value;
                    break;
                case 11:
                    valueMonth12 = value;
                    break;
                default:
                    break;
            }
        }

        public List<object> GetMonthValue()
        {
            List<object> result = new List<object>
            {
                valueMonth1,
                valueMonth2,
                valueMonth3,
                valueMonth4,
                valueMonth5,
                valueMonth6,
                valueMonth7,
                valueMonth8,
                valueMonth9,
                valueMonth10,
                valueMonth11,
                valueMonth12
            };

            return result;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }

        public object M01
        {
            get => valueMonth1;
            set => valueMonth1 = value;
        }

        public object M02
        {
            get => valueMonth2;
            set => valueMonth2 = value;
        }

        public object M03
        {
            get => valueMonth3;
            set => valueMonth3 = value;
        }

        public object M04
        {
            get => valueMonth4;
            set => valueMonth4 = value;
        }

        public object M05
        {
            get => valueMonth5;
            set => valueMonth5 = value;
        }

        public object M06
        {
            get => valueMonth6;
            set => valueMonth6 = value;
        }

        public object M07
        {
            get => valueMonth7;
            set => valueMonth7 = value;
        }

        public object M08
        {
            get => valueMonth8;
            set => valueMonth8 = value;
        }

        public object M09
        {
            get => valueMonth9;
            set => valueMonth9 = value;
        }

        public object M10
        {
            get => valueMonth10;
            set => valueMonth10 = value;
        }

        public object M11
        {
            get => valueMonth11;
            set => valueMonth11 = value;
        }

        public object M12
        {
            get => valueMonth12;
            set => valueMonth12 = value;
        }

        public object TotallOutput
        {
            get => totallOutput;
            set => totallOutput = value;
        }
    }
}
