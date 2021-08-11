using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(Location), "CanPlayerEnter")]
	internal class LocatonCanPlayerEnterPath
	{
		public static bool Prefix(ref Location __instance)
		{
			bool flag = __instance != null;
			bool result;
			if (flag)
			{
				Traverse traverse = Traverse.Create(__instance);
				CanUseDoor value = traverse.Field<CanUseDoor>("_playerCanEnterDelegate").Value;
				string value2 = traverse.Field<string>("_playerCanEnter").Value;
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
