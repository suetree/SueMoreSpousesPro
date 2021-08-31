using Newtonsoft.Json;
using SueEasyMenu.Attributes;
using SueEasyMenu.Models;
using SueEasyMenu.Settings;
using System;
using System.Collections.Generic;
using System.Linq;


namespace SueMoreSpouses.Settings
{

    [EMGroupAttribute("{=suems_setting_group_children_fast_growth}Children Fast Growth", "{=suems_setting_children_fast_growth_enable}", true)]
    public class ChildrenFastGrowthSetting : EMBaseGroupSetting
    {
        [EMGroupItem("{=suems_setting_children_fast_growth_cycle}Children fast growth cycle in days", EMOptionType.IntegerProperty, 2, 72)]
        public int ChildrenFastGrowthCycleInDays { set; get; }

        [EMGroupItem("{=suems_setting_children_fast_growth_stop_age}Children fast growth stop in age", EMOptionType.IntegerProperty, 6, 36)]

        public float ChildrenFastGrowtStopGrowUpAge { set; get; }

        [EMGroupItem("{=suems_setting_children_fast_growth_scope}Children fast growth scope", EMOptionType.SingleSelectProperty, "HeroSelectScope")]
        public EMOptionPair ChildrenFastGrowUpScope
        {
            get;
            set;
        }

        [EMGroupItem("{=suems_setting_children_name_prefix}", EMOptionType.InputTextProperty)]
        public string ChildrenNamePrefix
        {
            get;
            set;
        }
        [EMGroupItem("{=ChildrenNameSuffix}", EMOptionType.InputTextProperty)]
        public string ChildrenNameSuffix
        {
            get;
            set;
        }

        [JsonIgnoreAttribute]
        public List<EMOptionPair> HeroSelectScope { get; set; }


        public ChildrenFastGrowthSetting()
        {

            
            ChildrenFastGrowtStopGrowUpAge = 18;
            ChildrenFastGrowthCycleInDays = 36;

            List<EMOptionPair> list = new List<EMOptionPair>();
            list.Add(new EMOptionPair(0, "{=hero_scope_player_related}Player rerelated"));
            list.Add(new EMOptionPair(1, "{=hero_scope_clan_related}Clan related"));
            list.Add(new EMOptionPair(2, "{=hero_scope_kindow_related}Kindom related"));
            list.Add(new EMOptionPair(3, "{=hero_scope_world_related}World related"));
            HeroSelectScope = list;
            bool flag = ChildrenFastGrowUpScope == null;
            if (flag)
            {
                ChildrenFastGrowUpScope = list.First();
            }
        }

     
    }
}
