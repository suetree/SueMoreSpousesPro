using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SueMoreSpouses.Logics
{
    /**
     * �ο� BaseMissionTroopSpawnHandler
     */
    internal class BattleHeroJustTroopSpawnHandlerLogic : MissionLogic
	{
		private MissionAgentSpawnLogic _missionAgentSpawnLogic;

		private MapEvent _mapEvent;



		public override void AfterStart()
		{
            this._missionAgentSpawnLogic = base.Mission.GetMissionBehavior<MissionAgentSpawnLogic>();
            this._mapEvent = MapEvent.PlayerMapEvent;

            Scene scene = base.Mission.Scene;
			List<GameEntity> list = base.Mission.Scene.FindEntitiesWithTag("sp_special_item").ToList<GameEntity>();

			int defenderInitialSpawn = this._mapEvent.GetNumberOfInvolvedMen(BattleSideEnum.Defender);
            int attackerInitialSpawn = this._mapEvent.GetNumberOfInvolvedMen(BattleSideEnum.Attacker);

            this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Defender, false);
			this._missionAgentSpawnLogic.SetSpawnHorses(BattleSideEnum.Attacker, false);
			this._missionAgentSpawnLogic.InitWithSinglePhase(defenderInitialSpawn, attackerInitialSpawn, defenderInitialSpawn, attackerInitialSpawn, true, true, 1f);
		}

	}
}
