using Helpers;
using SandBox;
using SandBox.Source.Missions;
using SandBox.Source.Missions.Handlers;
using SueMBService.Utils;
using SueMoreSpouses.Logics;
using SueMoreSpouses.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace SueMoreSpouses
{
	internal class GameComponent
	{
	
		public static CampaignEventDispatcher CampaignEventDispatcher()
		{
			CampaignEventDispatcher result = null;
			PropertyInfo property = Campaign.Current.GetType().GetProperty("CampaignEventDispatcher", BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag = null != property;
			if (flag)
			{
				result = (CampaignEventDispatcher)property.GetValue(Campaign.Current);
			}
			return result;
		}

		public static void SendEvent(string eventMethod, object[] objectParams)
		{
			CampaignEventDispatcher campaignEventDispatcher = GameComponent.CampaignEventDispatcher();
			bool flag = campaignEventDispatcher != null;
			if (flag)
			{
				ReflectUtils.ReflectMethodAndInvoke("OnHeroComesOfAge", campaignEventDispatcher, objectParams);
			}
		}

		public static void StartBattle(PartyBase defenderParty)
		{
			StartBattleAction.Apply(MobileParty.MainParty.Party, defenderParty);
			PlayerEncounter.RestartPlayerEncounter(MobileParty.MainParty.Party, defenderParty, true);
			PlayerEncounter.Update();
			CampaignMission.OpenBattleMission(PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D));
		}

		public static Mission OpenCastleCourtyardMission(string scene, string sceneLevels, Location location, CharacterObject talkToChar)
		{
			return MissionState.OpenNew("TownCenter", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false), (Mission mission) => new List<MissionBehaviour>
			{
				new MissionOptionsComponent(),
				new CampaignMissionComponent(),
				new MissionBasicTeamLogic(),
				new MissionSettlementPrepareLogic(),
				new TownCenterMissionController(),
				new MissionAgentLookHandler(),
				new SandBoxMissionHandler(),
				new BasicLeaveMissionLogic(),
				new LeaveMissionLogic(),
				new BattleAgentLogic(),
				new MissionAgentPanicHandler(),
				new AgentTownAILogic(),
				new MissionConversationHandler(talkToChar),
				new MissionAgentHandler(location, null),
				new HeroSkillHandler(),
				new MissionFightHandler(),
				new MissionFacialAnimationHandler(),
				new MissionDebugHandler(),
				new MissionHardBorderPlacer(),
				new MissionBoundaryPlacer(),
				new MissionBoundaryCrossingHandler(),
				new VisualTrackerMissionBehavior()
			}.ToArray(), true, true);
		}

		public static Mission OpenBattleJustHero(string scene, string upgradeLevel)
		{
			MissionInitializerRecord rec = new MissionInitializerRecord(scene)
			{
				PlayingInCampaignMode = Campaign.Current.GameMode == CampaignGameMode.Campaign,
				AtmosphereOnCampaign = (Campaign.Current.GameMode == CampaignGameMode.Campaign) ? Campaign.Current.Models.MapWeatherModel.GetAtmosphereModel(CampaignTime.Now, MobileParty.MainParty.GetLogicalPosition()) : null,
				SceneLevels = upgradeLevel
			};
			MissionAgentSpawnLogic lc2 = new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
			{
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null),
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null)
			}, PartyBase.MainParty.Side);
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority();
			return MissionState.OpenNew("Battle", rec, delegate(Mission mission)
			{
				List<MissionBehaviour> list = new List<MissionBehaviour>();
				list.Add(new MissionOptionsComponent());
				list.Add(new CampaignMissionComponent());
				list.Add(new BattleEndLogic());
				list.Add(new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant));
				list.Add(new BattleMissionStarterLogic());
				list.Add(new BattleSpawnLogic("battle_set"));
				list.Add(new MissionAgentPanicHandler());
				list.Add(new AgentBattleAILogic());
				list.Add(new BattleObserverMissionLogic());
				list.Add(lc2);
				list.Add(new MissionFightHandler());
				list.Add(new MountAgentLogic());
				list.Add(new BattleAgentLogic());
				list.Add(new AgentVictoryLogic());
				list.Add(new MissionDebugHandler());
				list.Add(new MissionHardBorderPlacer());
				list.Add(new MissionBoundaryPlacer());
				list.Add(new MissionBoundaryCrossingHandler());
				list.Add(new BattleMissionAgentInteractionLogic());
				list.Add(new HighlightsController());
				list.Add(new BattleHighlightsController());
				list.Add(new BattleHeroJustTroopSpawnHandlerLogic());
				list.Add(new FieldBattleController());
				list.Add(new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority, FormationClass.NumberOfRegularFormations));
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				string arg_1AA_0 = (leaderHero != null) ? leaderHero.Name.ToString() : null;
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				return list.ToArray();
			}, true, true);
		}

		public static Mission OpenBattleNormal(string scene, string sceneLevels)
		{
			MissionAgentSpawnLogic lc = new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
			{
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null),
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null)
			}, PartyBase.MainParty.Side);
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority();
			return MissionState.OpenNew("Battle", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false), delegate(Mission mission)
			{
				MissionBehaviour[] array = new MissionBehaviour[26];
				array[0] = new MissionOptionsComponent();
				array[1] = new CampaignMissionComponent();
				array[2] = new BattleEndLogic();
				array[3] = new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
				array[4] = new MissionDefaultCaptainAssignmentLogic();
				array[5] = new BattleMissionStarterLogic();
				array[6] = new BattleSpawnLogic("battle_set");
				array[7] = new AgentBattleAILogic();
				array[8] = lc;
				array[9] = new BaseMissionTroopSpawnHandler();
				array[10] = new MountAgentLogic();
				array[11] = new BattleObserverMissionLogic();
				array[12] = new BattleAgentLogic();
				array[13] = new AgentVictoryLogic();
				array[14] = new MissionDebugHandler();
				array[15] = new MissionAgentPanicHandler();
				array[16] = new MissionHardBorderPlacer();
				array[17] = new MissionBoundaryPlacer();
				array[18] = new MissionBoundaryCrossingHandler();
				array[19] = new BattleMissionAgentInteractionLogic();
				array[20] = new FieldBattleController();
				array[21] = new AgentMoraleInteractionLogic();
				array[22] = new HighlightsController();
				array[23] = new BattleHighlightsController();
				array[24] = new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority, FormationClass.NumberOfRegularFormations);
				int num = 25;
				Hero leaderHero = MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero;
				TextObject attackerGeneralName = (leaderHero != null) ? leaderHero.Name : null;
				Hero leaderHero2 = MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero;
				array[num] = new CreateBodyguardMissionBehavior(attackerGeneralName, (leaderHero2 != null) ? leaderHero2.Name : null, null, null, true);
				return array;
			}, true, true);
		}

		public static FlattenedTroopRoster GetStrongestAndPriorTroops(MobileParty mobileParty, int maxTroopCount, List<Hero> includeList)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			FlattenedTroopRoster flattenedTroopRoster = mobileParty.MemberRoster.ToFlattenedRoster();
		
		     flattenedTroopRoster.RemoveIf((x) => x.IsWounded);

			IEnumerable<CharacterObject> characterObjects = flattenedTroopRoster.Select((x) => x.Troop);
			List<CharacterObject> list = characterObjects.OrderByDescending((x) => x.Level).ToList<CharacterObject>();
			bool flag = includeList != null && includeList.Count > 0;
			if (flag)
			{
				using (List<Hero>.Enumerator enumerator = includeList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Hero hero = enumerator.Current;
						bool flag2 = list.Any((CharacterObject x) => x == hero.CharacterObject);
						if (flag2)
						{
							list.Remove(hero.CharacterObject);
							troopRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
							maxTroopCount--;
						}
					}
				}
			}
	
			List<CharacterObject> list2 = list.Where((x) => x.IsHero).ToList<CharacterObject>();
			int num = Math.Min(list2.Count<CharacterObject>(), maxTroopCount);
			for (int i = 0; i < num; i++)
			{
				troopRoster.AddToCounts(list2[i], 1, false, 0, 0, true, -1);
				list.Remove(list2[i]);
			}
			return troopRoster.ToFlattenedRoster();
		}
	}
}
