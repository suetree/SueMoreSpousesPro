using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueMBService.Events
{
    public interface ISueEvent<T>
    {

        void AddNonSerializedListener(object owner, Action<T> action);

        void Excute(T t);
    }
}
