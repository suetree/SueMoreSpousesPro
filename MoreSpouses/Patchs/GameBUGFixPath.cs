using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;

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
