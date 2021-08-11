using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueEasyMenu.GauntletUI.ViewModels
{
    public class EMTestItemVM : ViewModel
    {
        [DataSourceProperty]
        public string Name
        {

            get
            {
                return new TextObject("TEST", null).ToString();
            }
        }
    }
}
