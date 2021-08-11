using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueMBService.Events
{
    public class SueBaseEvent<T>: ISueEvent<T>
    {

        public SueEventStep<T> FirstStep;

        public void AddNonSerializedListener(object owner, Action<T> action)
        {
            SueEventStep<T> step = new SueEventStep<T>(owner, action);
            if (null == FirstStep)
            {
                FirstStep = step;
            }
            else
            {
                FirstStep.Next = step;
            }
        }

        public void Excute(T t)
        {
            FirstStep.Action.Invoke(t);
        }

      
    }
}
