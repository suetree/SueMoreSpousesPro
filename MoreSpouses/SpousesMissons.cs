using SandBox;
using SueMoreSpouses.Logics;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace SueMoreSpouses
{
	internal class SpousesMissons
	{
		public static Mission OpenDateMission(string scene, List<Hero> heros)
		{
			return MissionState.OpenNew("TownCenter", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, "", false), (Mission mission) => new MissionBehavior[]
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new BattleSpawnLogic("battle_set"),
				new SpousesDateSpawnLogic(heros),
				new BattleAgentLogic(),
				new AgentBattleAILogic(),
				new MissionFacialAnimationHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(),
				new AgentMoraleInteractionLogic(),
				new HighlightsController(),
				new BattleHighlightsController()
			}, true, true);
		}
	}
}
