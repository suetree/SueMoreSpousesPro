using HarmonyLib;
using SandBox.GauntletUI;
using SueMoreSpouses.GauntletUI.ViewModels;
using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace SueMoreSpouses.Patch
{
    [HarmonyPatch(typeof(ScreenBase))]
	internal class ClanScreenLayerPatch
	{
		internal static GauntletLayer screenLayer;

		internal static SpouseClanVM spouseClanView;

		[HarmonyPatch("AddLayer")]
		public static void Postfix(ref ScreenBase __instance)
		{
			GauntletClanScreen gauntletClanScreen = __instance as GauntletClanScreen;
			bool flag = gauntletClanScreen != null && ClanScreenLayerPatch.screenLayer == null;
			bool flag2 = flag;
			if (flag2)
			{
				ClanScreenLayerPatch.screenLayer = new GauntletLayer(100, "GauntletLayer", false);
				ClanScreenLayerPatch.spouseClanView = new SpouseClanVM(gauntletClanScreen);
				ClanScreenLayerPatch.screenLayer.LoadMovie("SpouseScreen", ClanScreenLayerPatch.spouseClanView);
				ClanScreenLayerPatch.screenLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				gauntletClanScreen.AddLayer(ClanScreenLayerPatch.screenLayer);
			}
		}

		[HarmonyPatch("RemoveLayer")]
		public static void Prefix(ref ScreenBase __instance, ref ScreenLayer layer)
		{
			bool flag = __instance is GauntletClanScreen && ClanScreenLayerPatch.screenLayer != null && layer.Input.IsCategoryRegistered(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			bool flag2 = flag;
			if (flag2)
			{
				__instance.RemoveLayer(ClanScreenLayerPatch.screenLayer);
				ClanScreenLayerPatch.spouseClanView.OnFinalize();
				ClanScreenLayerPatch.spouseClanView = null;
				ClanScreenLayerPatch.screenLayer = null;
			}
		}
	}
}
