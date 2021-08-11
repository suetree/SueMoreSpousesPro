using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueEasyMenu.Models
{
    public class EMBaseOption
    {

        public delegate void ChangeDelegate(object value);

        public event ChangeDelegate OnValueChangeEvent;

        protected void OnValueChange(object obj)
        {
            if(null!= OnValueChangeEvent) OnValueChangeEvent(obj);
        }
    }
}
