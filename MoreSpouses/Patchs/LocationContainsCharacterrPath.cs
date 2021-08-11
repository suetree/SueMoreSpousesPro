using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(Location), "ContainsCharacter", new Type[]
	{
		typeof(LocationCharacter)
	})]
	internal class LocationContainsCharacterrPath
	{
		[HarmonyPrefix]
		public static bool Prefix(ref bool __result, ref Location __instance)
		{
			Traverse traverse = Traverse.Create(__instance);
			List<LocationCharacter> value = traverse.Field<List<LocationCharacter>>("_characterList").Value;
			bool flag = value == null;
			bool result;
			if (flag)
			{
				__result = false;
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
