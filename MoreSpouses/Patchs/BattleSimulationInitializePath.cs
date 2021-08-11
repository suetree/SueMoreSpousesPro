using HarmonyLib;
using SandBox.ViewModelCollection;
using SueMoreSpouses.Behaviors;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(SPScoreboardVM), "Initialize")]
	internal class BattleSimulationInitializePath
	{
		public static void Prefix(SPScoreboardVM __instance, IMissionScreen missionScreen, Mission mission, Action releaseSimulationSources, Action<bool> onToggle)
		{
			Campaign.Current.GetCampaignBehavior<SpousesStatsBehavior>().GetSpouseStatsBusiness().Initialize();
		}
	}
}
