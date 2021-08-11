using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(Location), "GetLocationCharacter", new Type[]
	{
		typeof(IAgentOriginBase)
	})]
	internal class LocationGetLocationCharacterPath
	{
		[HarmonyPrefix]
		public static bool Prefix(ref LocationCharacter __result, ref Location __instance)
		{
			Traverse traverse = Traverse.Create(__instance);
			List<LocationCharacter> value = traverse.Field<List<LocationCharacter>>("_characterList").Value;
			bool flag = value == null;
			bool result;
			if (flag)
			{
				__result = null;
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
