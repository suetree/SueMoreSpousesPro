using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SueEasyMenu.Models.EMBaseOption;

namespace SueEasyMenu.Models
{
    public class EMOptionBuilder
    {
        public List<EMOptionGroup> Groups { get; set; }


        public EMOptionGroup BuildGroup(ChangeDelegate changeDelegate, string name, string describe = "", bool enable = true)
        {
            if (null == Groups)
            {
                Groups = new List<EMOptionGroup>();
            }
            EMOptionGroup group = new EMOptionGroup(changeDelegate, name);
            group.Describe = describe;
            group.Enable = enable;
            this.Groups.Add(group);
            return group;
        }

       
    }
}
