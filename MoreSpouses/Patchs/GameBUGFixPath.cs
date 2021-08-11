using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors.Towns;

namespace SueMoreSpouses.Patch
{
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
