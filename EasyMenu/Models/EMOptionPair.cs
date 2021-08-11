using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueEasyMenu.Models
{
    public class EMOptionPair
    {
        public object Value { set; get; }
        public String Name { set; get; }

        public EMOptionPair(object value, String name)
        {
            this.Value = value;
            this.Name = name;
        }
    }
}
