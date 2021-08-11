using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SueEasyMenu.Models
{
    public class EMOptionGroup : EMBaseOption
    {
        public String IdentifyKey;
        public object TargetInstance { get; set; }
        public String Name { get; set; }
        public String Describe { get; set; }
        public bool Enable { get; set; }

        public bool ShowEnableController { get; set; }
        public List<EMOptionItem> OptionItems { get; set; }

        public ChangeDelegate ChangeValueDelegate;

        public EMOptionGroup(object instance, string identifyKey, string name, string describe = "")
        {
            TargetInstance = instance;
            IdentifyKey = identifyKey;
            Name = name;
            Describe = describe;
        }

        public EMOptionGroup(ChangeDelegate changeDelegate, string name, string describe = "")
        {
            Name = name;
            Describe = describe;
            ChangeValueDelegate = changeDelegate;
            this.OnValueChangeEvent += changeDelegate;

        }

        public EMOptionGroup AddOption(EMOptionItem property)
        {
            if (null != property)
            {
                if (null == OptionItems)
                {
                    OptionItems = new List<EMOptionItem>();
                }

                if (!OptionItems.Contains(property))
                {
                    OptionItems.Add(property);
                }
            }
            return this;
        }

        public void SetEnableAndRefreshOriginalValue(bool value)
        {
            if (value != this.Enable)
            {
                this.Enable = value;
                OnValueChange(value);
            }
        }

     /*   public void ChangeOrignalValue(object value)
        {
            if (null != TargetInstance)
            {
                Type type = TargetInstance.GetType();
                PropertyInfo propertyInfo = type.GetProperty(IdentifyKey);
                if (null != propertyInfo)
                {
                    object v = Convert.ChangeType(value, propertyInfo.PropertyType);
                    propertyInfo.SetValue(TargetInstance, v, null);
                }
            }
        }*/

    }
}
