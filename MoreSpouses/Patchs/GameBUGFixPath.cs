using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.Towns;

namespace SueMoreSpouses.Patch
{


	[HarmonyPatch(typeof(Hero), "ResetEquipments")]
	internal class HeroResetEquipmentsPath
	{
		public static bool Prefix(ref Hero __instance)
		{
			bool result = true;
			if (null == __instance.Template)
			{
				result = false;
			}
			return result;
		}
	}


}
