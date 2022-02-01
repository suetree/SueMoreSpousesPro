using HarmonyLib;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SueMoreSpouses.Patch.LocationCrashBugFix
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

    [HarmonyPatch(typeof(Location), "DeserializeDelegate")]
    internal class LocatonDeserializeDelegatePath
    {
        public static bool Prefix(ref Location __instance, string text)
        {
            bool flag = text == null;
            return !flag;
        }
    }

    [HarmonyPatch(typeof(Location), "CanAIEnter")]
    internal class LocatonCanAIEnterPath
    {
        public static bool Prefix(ref Location __instance, LocationCharacter character)
        {
            bool flag = __instance == null || __instance.Name == null;
            bool result;
            if (flag)
            {
                bool flag2 = __instance != null;
                if (flag2)
                {
                    Traverse traverse = Traverse.Create(__instance);
                    CanUseDoor value = traverse.Field<CanUseDoor>("_aiCanEnterDelegate").Value;
                    string value2 = traverse.Field<string>("_aiCanEnter").Value;
                    bool flag3 = value == null && value2 == null;
                    if (flag3)
                    {
                        result = false;
                        return result;
                    }
                }
            }
            result = true;
            return result;
        }
    }

    [HarmonyPatch(typeof(Location), "CanAIExit")]
    internal class LocatonCanAIExitPath
    {
        public static bool Prefix(ref Location __instance, LocationCharacter character)
        {
            bool flag = __instance == null || __instance.Name == null;
            bool result;
            if (flag)
            {
                bool flag2 = __instance != null;
                if (flag2)
                {
                    Traverse traverse = Traverse.Create(__instance);
                    CanUseDoor value = traverse.Field<CanUseDoor>("_aiCanExitDelegate").Value;
                    string value2 = traverse.Field<string>("_aiCanExit").Value;
                    bool flag3 = value == null && value2 == null;
                    if (flag3)
                    {
                        result = false;
                        return result;
                    }
                }
            }
            result = true;
            return result;
        }
    }
}
