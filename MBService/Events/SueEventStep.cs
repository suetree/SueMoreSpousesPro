using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueMBService.Events
{

    public class SueEventStep<T>
    {
        public SueEventStep<T> Next;

        public object Owner;

        public Action<T> Action;



        public SueEventStep(object owner, Action<T> action)
        {
            Owner = owner;
            Action = action;
        }
    }
}
