using SueEasyMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueEasyMenu.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EMGroupItemAttribute : Attribute
    {

        public string DisplayName { get; set; }

        public EMOptionType OptionType { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        public string SelectItemsKey { get; set; }

        public EMGroupItemAttribute(string displayName, EMOptionType optionType, float minValue = 0, float maxValue = 0)
        {
            DisplayName = displayName;
            OptionType = optionType;
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public EMGroupItemAttribute(string displayName, EMOptionType optionType, string selectItemsKey)
        {
            DisplayName = displayName;
            OptionType = optionType;
            SelectItemsKey = selectItemsKey;
        }
    }
}
