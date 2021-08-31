using SueEasyMenu.Attributes;
using SueEasyMenu.Models;
using SueEasyMenu.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SueMoreSpouses.Settings
{
    [EMGroup("{=suems_setting_group_exspouse_pregnancy} EX-Spouse Get Pregnancy", "{=suems_setting_exspouse_pregnancy_enable}", false)]
    public class EXSpouseGetPregnancySetting : EMBaseGroupSetting
    {

        [EMGroupItem("{=suems_setting_exspouse_pregnancy_daily_chance}Daily chance of MainPlayer's ex-spouse or spouse get pregnancy", EMOptionType.FloatProperty, 0, 1)]
        public float ExspouseGetPregnancyDailyChance
        {
            get;
            set;
        }

        [EMGroupItem("{=suems_setting_exspouse_pregnancy_duration_in_days}Ex-spouse or spouse pregnancy duration in day", EMOptionType.IntegerProperty, 2, 72)]
        public int ExspouseGetPregnancyDurationInDays
        {
            get;
            set;
        }

        public EXSpouseGetPregnancySetting()
        {
            ExspouseGetPregnancyDailyChance = 0.5f;
            ExspouseGetPregnancyDurationInDays = 30;

        }
    }
}
