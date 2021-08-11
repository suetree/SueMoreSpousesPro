using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SueEasyMenu.Models.EMBaseOption;

namespace SueEasyMenu.Models
{
    public class EMOptionItem: EMBaseOption
    {
        public object TargetInstance { get; set; }
        public Type TargetInstanceType { get; set; }

        public String IdentifyKey { get; set; }

        public String DisplayName { get; set; }

        private object _propertyValue;
        public EMOptionType OptionType { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public List<EMOptionPair> SelectItems { get; set; }

        //
        public List<EMOptionItem> ItemTemplate = new List<EMOptionItem>();

        public ChangeDelegate CurrentValueDelegate;



        public EMOptionItem(ChangeDelegate changeDelegate, string name, object value, EMOptionType type, float minValue = 0,float maxValue = 0)
        {
            CurrentValueDelegate = changeDelegate;
            if (null != changeDelegate) base.OnValueChangeEvent += changeDelegate;
            DisplayName = name;
            this. _propertyValue = value;
            OptionType = type;
            MinValue = minValue;
            MaxValue = maxValue;
           
        }

        public EMOptionItem(ChangeDelegate changeDelegate, string name, object value, EMOptionType type,  List<EMOptionItem> items ) : this(changeDelegate, name, value, type, 0, 100)
        {
            if (null != items)
            {
                ItemTemplate = items;
            }
        }


        public EMOptionItem(ChangeDelegate changeDelegate, string name, object value, EMOptionType type) : this(changeDelegate, name, value, type, 0, 100)
        {
           
        }

        public EMOptionItem(string identifyKey, ChangeDelegate changeDelegate,  string name, object value, EMOptionType type, float minValue = 0, float maxValue = 0) : this(null, name, value, type, 0, 100)
        {
            IdentifyKey = identifyKey;
        }


   


        public EMOptionItem CopyForItemTemplate(object obj)
        {
            EMOptionItem copy = new EMOptionItem(IdentifyKey, null, DisplayName, this._propertyValue, OptionType, MinValue, MaxValue);
            copy.FillSelectItems(SelectItems);
            copy.TargetInstanceType = TargetInstanceType;
            copy.TargetInstance = obj;
            copy.ItemTemplate = ItemTemplate;
            return copy;
        }

        public EMOptionItem FillSelectItems(List<EMOptionPair> list)
        {
            SelectItems = list;
            return this;
        }

        public EMOptionItem SetTargetInstanceType(Type type)
        {
            TargetInstanceType = type;
            return this;
        }

        public object PropertyValue
        {
            get
            {
                return this._propertyValue;
            }
         
        }

        public void SetAndRefreshOriginalValue(object value)
        {
            if (value != this._propertyValue)
            {
                this._propertyValue = value;
              
              if(null != CurrentValueDelegate) OnValueChange(value);
               ChangeOrignalValue(value);
            }
        }

        public void ChangeOrignalValue(object value)
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
        }

    }
}
