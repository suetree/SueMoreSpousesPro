using HarmonyLib;
using SandBox.ViewModelCollection;
using SueMoreSpouses.Behaviors;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(SPScoreboardVM), "OnExitBattle")]
	internal class BattleSimulationOnExitBattlePath
	{
		public static void Postfix(SPScoreboardVM __instance)
		{
			ExplainedNumber explainedNumber = default(ExplainedNumber);
			ExplainedNumber explainedNumber2 = default(ExplainedNumber);
			ExplainedNumber explainedNumber3 = default(ExplainedNumber);
			float renownChange = 0f;
			float influenceChange = 0f;
			float moraleChange = 0f;
			float goldChange = 0f;
			float playerEarnedLootPercentage = 0f;
			bool isActive = PlayerEncounter.IsActive;
			if (isActive)
			{
				PlayerEncounter.GetBattleRewards(out renownChange, out influenceChange, out moraleChange, out goldChange, out playerEarnedLootPercentage, ref explainedNumber, ref explainedNumber2, ref explainedNumber3);
			}
			Campaign.Current.GetCampaignBehavior<SpousesStatsBehavior>().GetSpouseStatsBusiness().EndCountHeroBattleData(__instance.BattleResultIndex, renownChange, influenceChange, moraleChange, goldChange, playerEarnedLootPercentage);
		}
	}
}
