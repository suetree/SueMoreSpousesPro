using HarmonyLib;
using SandBox.ViewModelCollection;
using SueMoreSpouses.Behaviors;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(SPScoreboardVM), "TroopNumberChanged")]
	internal class BattleSimulationTroopNumberChangedPath
	{
		public static void Postfix(SPScoreboardVM __instance, BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberDead = 0, int numberWounded = 0, int numberRouted = 0, int numberKilled = 0, int numberReadyToUpgrade = 0)
		{
			Campaign.Current.GetCampaignBehavior<SpousesStatsBehavior>().GetSpouseStatsBusiness().RecordBattleData(side, battleCombatant, character, number, numberDead, numberKilled, numberRouted, numberWounded);
		}
	}
}
