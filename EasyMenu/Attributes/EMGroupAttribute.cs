using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueEasyMenu.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EMGroupAttribute : Attribute
    {
        public EMGroupAttribute(string name, string describe = "", bool showEnableController = false)
        {
            Name = name;
            Describe = describe;
            ShowEnableController = showEnableController;
        }

        public bool ShowEnableController { get; set; }


        public string Name { get; set; }
        public string Describe { get; set; }
    }
}
