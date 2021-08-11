using HarmonyLib;
using SueMoreSpouses.Settings;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace SueMoreSpouses.Patch
{
    [HarmonyPatch(typeof(HeroCreator), "DeliverOffSpring")]
	internal class HeroCreatorDeliverOffSpringPath
	{
		private static void Postfix(ref Hero __result, Hero mother, Hero father, bool isOffspringFemale, int age = 1)
		{
			bool flag = Hero.MainHero.Children.Contains(__result);
			ChildrenFastGrowthSetting setting = MoreSpouseSetting.GetInstance().ChildrenFastGrowthSetting;
			if (flag && setting.Enable)
			{
				bool flag2 = setting.ChildrenNamePrefix != null && setting.ChildrenNamePrefix.Length > 0;
				if (flag2)
				{
					TextObject name = new TextObject(setting.ChildrenNamePrefix + __result.Name.ToString(), null);
					__result.SetName(name);
				}
				bool flag3 = setting.ChildrenNameSuffix != null && setting.ChildrenNameSuffix.Length > 0;
				if (flag3)
				{
					TextObject name = new TextObject(__result.Name.ToString() + setting.ChildrenNameSuffix, null);
					__result.SetName(name);
				}
			}
		}
	}
}
