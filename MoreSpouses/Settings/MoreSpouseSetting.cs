using SueEasyMenu.Attributes;
using SueEasyMenu.Settings;

namespace SueMoreSpouses.Settings
{
    [EMSaveSettingAttribute("SueMoreSpouses", "SettingDataV3")]
    internal class MoreSpouseSetting : EMBaseSaveSettings
    {
        public static MoreSpouseSetting GetInstance()
        {

            return EMSettingFileManager.Instance.GetSetting<MoreSpouseSetting>();

        }
        public EXSpouseGetPregnancySetting EXSpouseGetPregnancySetting { set; get; }

        public ChildrenFastGrowthSetting ChildrenFastGrowthSetting { set; get; }

        public NamelessNPCSetting NamelessNPCSetting { set; get; }

        public MoreSpouseSetting()
        {
            EXSpouseGetPregnancySetting = new EXSpouseGetPregnancySetting();
            ChildrenFastGrowthSetting = new ChildrenFastGrowthSetting();
            NamelessNPCSetting = new NamelessNPCSetting();

        }


    }


}
