using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libData
{
    public class Equip
    {
        public int Id;
        public bool Selected;

        public Equip(int id, bool selected)
        {
            this.Id = id;
            this.Selected = selected;
        }
    }
}
