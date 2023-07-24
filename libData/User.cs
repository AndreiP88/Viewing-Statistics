﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libData
{
    public class User
    {
        public int Id;
        public int Equip;
        public List<UserShift> Shifts;
        public int WorkingOutUser;
        public int WorkingOutBacklog;

        public User(int id, int equip)
        {
            this.Id = id;
            this.Equip = equip;
        }
    }
}