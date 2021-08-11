using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(Location), "CanPlayerSee")]
	internal class LocatonCanPlayerSeePath
	{
		public static bool Prefix(ref Location __instance)
		{
			bool flag = __instance != null;
			bool result;
			if (flag)
			{
				Traverse traverse = Traverse.Create(__instance);
				CanUseDoor value = traverse.Field<CanUseDoor>("_playerCanSeeDelegate").Value;
				string value2 = traverse.Field<string>("_playerCanSee").Value;
				bool flag2 = value == null && value2 == null;
				if (flag2)
				{
					result = false;
					return result;
				}
			}
			result = true;
			return result;
		}
	}
}
