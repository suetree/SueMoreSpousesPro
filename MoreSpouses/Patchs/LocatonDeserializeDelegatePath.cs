using HarmonyLib;
using System;
using TaleWorlds.CampaignSystem;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(Location), "DeserializeDelegate")]
	internal class LocatonDeserializeDelegatePath
	{
		public static bool Prefix(ref Location __instance, string text)
		{
			bool flag = text == null;
			return !flag;
		}
	}
}
