using HarmonyLib;
using SueMoreSpouses.Settings;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;

namespace SueMoreSpouses
{
    [HarmonyPatch(typeof(DefaultPregnancyModel), "GetDailyChanceOfPregnancyForHero")]
	public class GetDailyChanceOfPregnancyForHero
	{
		private static void Postfix(ref float __result, Hero hero)
		{
			EXSpouseGetPregnancySetting spouseGetPregnancySetting = MoreSpouseSetting.GetInstance().EXSpouseGetPregnancySetting;
			if (spouseGetPregnancySetting.Enable)
			{
				bool flag = hero.Clan == Clan.PlayerClan && hero != Hero.MainHero && (Hero.MainHero.ExSpouses.Contains(hero) || hero == Hero.MainHero.Spouse);
				if (flag)
				{
					float exspouseGetPregnancyDailyChance = spouseGetPregnancySetting.ExspouseGetPregnancyDailyChance;
					__result = exspouseGetPregnancyDailyChance;
				}
			}
		}
	}
}
