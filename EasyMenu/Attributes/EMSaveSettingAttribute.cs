using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueEasyMenu.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EMSaveSettingAttribute : Attribute
    {
        public string SavePath { get; set; }

        public string SaveName { get; set; }

        public EMSaveSettingAttribute(string savePath, string saveName)
        {
            SavePath = savePath;
            SaveName = saveName;
        }
    }
}
