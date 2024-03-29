﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int FactOutQty;
        public int PlanOutQty;
        public int Normtime;
        public int IdManOrderJobItem;
        public int IDFBCBrigade;
        public string IdletimeName;

        public UserShiftOrder(int idEquip, string orderNumber, string orderName, int status, int flags, string dateBegin, string dateEnd, int duration, int factOutQty, int planOutQty, int normtime, int idManOrderJobItem, int idFBCBrigade, string idletimeName)
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
        }
    }
}
