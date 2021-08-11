using HarmonyLib;
using SandBox.GauntletUI;
using SueLordFromFamily.GauntletUI.ViewModels;
using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace SueLordFromFamily.Paths
{
	[HarmonyPatch(typeof(ScreenBase))]
	internal class KindomScreenLayerPatch
	{
		internal static GauntletLayer screenLayer;

		internal static KindomScreenVM kindomScreenVM;

		[HarmonyPatch("AddLayer")]
		public static void Postfix(ref ScreenBase __instance)
		{
			GauntletKingdomScreen gauntletKingdomScreen = __instance as GauntletKingdomScreen;
			bool flag = gauntletKingdomScreen != null && KindomScreenLayerPatch.screenLayer == null;
			if (flag)
			{
				KindomScreenLayerPatch.screenLayer = new GauntletLayer(100, "GauntletLayer");
				KindomScreenLayerPatch.kindomScreenVM = new KindomScreenVM(gauntletKingdomScreen);
				KindomScreenLayerPatch.screenLayer.LoadMovie("KindomScreen", KindomScreenLayerPatch.kindomScreenVM);
				KindomScreenLayerPatch.screenLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
				gauntletKingdomScreen.AddLayer(KindomScreenLayerPatch.screenLayer);
			}
		}

		[HarmonyPatch("RemoveLayer")]
		public static void Prefix(ref ScreenBase __instance, ref ScreenLayer layer)
		{
			bool flag = __instance is GauntletKingdomScreen && KindomScreenLayerPatch.screenLayer != null && layer.Input.IsCategoryRegistered(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			bool flag2 = flag;
			if (flag2)
			{
				__instance.RemoveLayer(KindomScreenLayerPatch.screenLayer);
				KindomScreenLayerPatch.kindomScreenVM.OnFinalize();
				KindomScreenLayerPatch.kindomScreenVM = null;
				KindomScreenLayerPatch.screenLayer = null;
			}
		}
	}
}
