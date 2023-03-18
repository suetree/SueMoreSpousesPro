using Helpers;
using SandBox;
using SandBox.Missions.MissionLogics;
using SandBox.Source.Missions;
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
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.TroopSuppliers;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;

namespace SueMoreSpouses
{
    /**
     * ²Î¿¼ SandBoxMissions
     */
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
			}, PartyBase.MainParty.Side, Mission.BattleSizeType.Battle);
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
			List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority();
			return MissionState.OpenNew("Battle", rec, delegate(Mission mission)
			{
				List<MissionBehavior> list = new List<MissionBehavior>();
				list.Add(new MissionOptionsComponent());
				list.Add(new CampaignMissionComponent());
				list.Add(new BattleEndLogic());
				list.Add(new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant));
				list.Add(new BattleMissionStarterLogic());
				list.Add(new BattleSpawnLogic("battle_set"));
				list.Add(new MissionAgentPanicHandler());
				list.Add(new AgentHumanAILogic());
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
                list.Add(new EquipmentControllerLeaveLogic());
                list.Add(new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority, FormationClass.NumberOfRegularFormations));
                list.Add(new DeploymentMissionController(false));
                list.Add(new BattleDeploymentHandler(false));
               
                return list.ToArray();
			}, true, true);
		}

		public static Mission OpenBattleNormal(string scene, string sceneLevels)
		{
			MissionAgentSpawnLogic lc = new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
			{
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null),
				new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null)
			}, PartyBase.MainParty.Side, Mission.BattleSizeType.Battle);
			bool isPlayerSergeant = MobileParty.MainParty.MapEvent.IsPlayerSergeant();
			bool isPlayerInArmy = MobileParty.MainParty.Army != null;
            IEnumerable<MapEventParty> arg_6D_0 = MobileParty.MainParty.MapEvent.AttackerSide.Parties;
  
            bool isPlayerAttacker = !arg_6D_0.Where(p => p.Party == MobileParty.MainParty.Party).IsEmpty<MapEventParty>();
            List<string> heroesOnPlayerSideByPriority = HeroHelper.OrderHeroesOnPlayerSideByPriority();
            return MissionState.OpenNew("Battle", SandBoxMissions.CreateSandBoxMissionInitializerRecord(scene, sceneLevels, false), delegate(Mission mission)
			{
                MissionBehavior[] expr_07 = new MissionBehavior[25];
                expr_07[0] = CreateCampaignMissionAgentSpawnLogic(false);
                expr_07[1] = new BattleSpawnLogic("battle_set");
                expr_07[2] = new SandBoxBattleMissionSpawnHandler();
                expr_07[3] = new CampaignMissionComponent();
                expr_07[4] = new BattleAgentLogic();
                expr_07[5] = new MountAgentLogic();
                expr_07[6] = new MissionOptionsComponent();
                expr_07[7] = new BattleEndLogic();
                expr_07[8] = new MissionCombatantsLogic(MobileParty.MainParty.MapEvent.InvolvedParties, PartyBase.MainParty, MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Defender), MobileParty.MainParty.MapEvent.GetLeaderParty(BattleSideEnum.Attacker), Mission.MissionTeamAITypeEnum.FieldBattle, isPlayerSergeant);
                expr_07[9] = new BattleObserverMissionLogic();
                expr_07[10] = new AgentHumanAILogic();
                expr_07[11] = new AgentVictoryLogic();
                expr_07[12] = new MissionAgentPanicHandler();
                expr_07[13] = new BattleMissionAgentInteractionLogic();
                expr_07[14] = new AgentMoraleInteractionLogic();
                expr_07[15] = new AssignPlayerRoleInTeamMissionController(!isPlayerSergeant, isPlayerSergeant, isPlayerInArmy, heroesOnPlayerSideByPriority, FormationClass.NumberOfRegularFormations);
                expr_07[16] = new SandboxGeneralsAndCaptainsAssignmentLogic(MapEvent.PlayerMapEvent.AttackerSide.LeaderParty.LeaderHero?.Name, MapEvent.PlayerMapEvent.DefenderSide.LeaderParty.LeaderHero?.Name);
                expr_07[17] = new EquipmentControllerLeaveLogic();
                expr_07[18] = new MissionHardBorderPlacer();
                expr_07[19] = new MissionBoundaryPlacer();
                expr_07[20] = new MissionBoundaryCrossingHandler();
                expr_07[21] = new HighlightsController();
                expr_07[22] = new BattleHighlightsController();
                expr_07[23] = new DeploymentMissionController(isPlayerAttacker);
                expr_07[24] = new BattleDeploymentHandler(isPlayerAttacker);
                return expr_07;
            }, true, true);
		}

        private static MissionAgentSpawnLogic CreateCampaignMissionAgentSpawnLogic(bool isSiege = false)
        {
            return new MissionAgentSpawnLogic(new IMissionTroopSupplier[]
            {
                new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Defender, null),
                new PartyGroupTroopSupplier(MapEvent.PlayerMapEvent, BattleSideEnum.Attacker, null)
            }, PartyBase.MainParty.Side,  Mission.BattleSizeType.Battle);
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
