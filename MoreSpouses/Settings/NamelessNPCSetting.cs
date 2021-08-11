using SueEasyMenu.Attributes;
using SueEasyMenu.Models;
using SueEasyMenu.Settings;

namespace SueMoreSpouses.Settings
{
    [EMGroup("{=suems_setting_group_nameless_npc}Nameless NPC")]
    public class NamelessNPCSetting : EMBaseGroupSetting
    {
        [EMGroupItem("{=sms_npc_join_auto_skill}NPC who have no name automatic assignment skills", EMOptionType.BoolProperty)]
        public bool NPCCharaObjectSkillAuto
        {
            get;
            set;
        }

        [EMGroupItem("{=sms_npc_from_tier}NPC who have no name equipment tier", EMOptionType.IntegerProperty, 0f, 6f)]
        public int NPCCharaObjectFromTier
        {
            get;
            set;
        }
    }
}
