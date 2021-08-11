using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;

namespace SueMoreSpouses
{
	internal class SpousesStatsMissionLogic : MissionLogic
	{
		private SpouseStatsBusiness _spouseStatsBusiness;

		public SpousesStatsMissionLogic(SpouseStatsBusiness spouseStatsBusiness)
		{
			this._spouseStatsBusiness = spouseStatsBusiness;
		}

		public override void ShowBattleResults()
		{
			foreach (Agent current in Mission.Current.PlayerTeam.ActiveAgents)
			{
				CharacterObject characterObject = current.Character as CharacterObject;
				bool flag = characterObject != null;
				if (flag)
				{
				}
			}
			foreach (Agent current2 in Mission.Current.PlayerEnemyTeam.ActiveAgents)
			{
			}
		}
	}
}
