using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueEasyMenu.Settings
{
    public class EMSettingSaveParams
    {

        public string Path { set; get; }

        public string FileName { set; get; }

        public EMSettingSaveParams(string path, string fileName)
        {
            Path = path;
            FileName = fileName;
        }
    }
}
