using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;

namespace SueMoreSpouses
{
	[HarmonyPatch(typeof(PregnancyCampaignBehavior), "CheckAreNearby")]
	public class CheckAreNearbyPath
	{
		private static void Prefix(Hero hero, out Hero spouse)
		{
			bool flag = hero.IsFemale && hero.Age > 18f && Hero.MainHero.ExSpouses.Contains(hero) && !hero.IsPregnant && hero != Hero.MainHero.Spouse;
			if (flag)
			{
				spouse = Hero.MainHero;
			}
			else
			{
				spouse = hero.Spouse;
			}
		}
	}
}
