using SandBox;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SueMoreSpouses.Logics
{
	internal class SpousesDateSpawnLogic : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		private List<Hero> _needSpawnHeros;

		private bool _isMissionInitialized;

		private bool _troopsInitialized;

		private BattleAgentLogic _battleAgentLogic;

		public bool IsSideDepleted(BattleSideEnum side)
		{
			return false;
		}

		public SpousesDateSpawnLogic(List<Hero> heros)
		{
			bool flag = heros != null;
			if (flag)
			{
				this._needSpawnHeros = heros;
			}
			else
			{
				this._needSpawnHeros = new List<Hero>();
			}
		}

		public void StopSpawner()
		{
		}

		public override void OnBehaviourInitialize()
		{
			base.OnBehaviourInitialize();
			this._battleAgentLogic = Mission.Current.GetMissionBehaviour<BattleAgentLogic>();
		}

		public override void OnMissionTick(float dt)
		{
			bool flag = !this._isMissionInitialized;
			if (flag)
			{
				this.SpawnAgents();
				this._isMissionInitialized = true;
			}
			else
			{
				bool flag2 = !this._troopsInitialized;
				if (flag2)
				{
					this._troopsInitialized = true;
					foreach (Agent current in base.Mission.Agents)
					{
						this._battleAgentLogic.OnAgentBuild(current, Clan.PlayerClan.Banner);
					}
				}
				this.FlowMainPlayer();
			}
		}

		private void FlowMainPlayer()
		{
			foreach (Agent current in base.Mission.Agents)
			{
				bool flag = current != Agent.Main;
				if (flag)
				{
					bool isHuman = current.IsHuman;
					if (isHuman)
					{
						CampaignAgentComponent component = current.GetComponent<CampaignAgentComponent>();
						bool flag2 = component != null && component.AgentNavigator == null;
						if (flag2)
						{
							component.CreateAgentNavigator();
						}
						current.SetFollowedUnit(Agent.Main);
					}
				}
			}
		}

		private void SpawnAgents()
		{
			GameEntity gameEntity = Mission.Current.Scene.FindEntityWithTag("attacker_infantry");
			foreach (Hero current in this._needSpawnHeros)
			{
				WorldFrame invalid = WorldFrame.Invalid;
				invalid = new WorldFrame(gameEntity.GetGlobalFrame().rotation, new WorldPosition(gameEntity.Scene, gameEntity.GetGlobalFrame().origin));
				SimpleAgentOrigin troopOrigin = new SimpleAgentOrigin(current.CharacterObject, -1, null, default(UniqueTroopDescriptor));
				bool spawnWithHorse = true;
				Agent agent = Mission.Current.SpawnTroop(troopOrigin, true, false, spawnWithHorse, false, false, 0, 0, true, false, false, null, new Vec2?(invalid.ToGroundMatrixFrame().rotation.f.AsVec2));
				agent.UpdateSpawnEquipmentAndRefreshVisuals(current.CivilianEquipment);
				bool flag = !agent.IsMainAgent;
				if (flag)
				{
					this.SimulateAgent(agent);
				}
			}
		}

		public void SimulateAgent(Agent agent)
		{
			bool isHuman = agent.IsHuman;
			if (isHuman)
			{
				AgentNavigator agentNavigator = agent.GetComponent<CampaignAgentComponent>().AgentNavigator;
				int num = MBRandom.RandomInt(35, 50);
				agent.PreloadForRendering();
				for (int i = 0; i < num; i++)
				{
					bool flag = agentNavigator != null;
					if (flag)
					{
						agentNavigator.Tick(0.1f, true);
					}
					bool isUsingGameObject = agent.IsUsingGameObject;
					if (isUsingGameObject)
					{
						agent.CurrentlyUsedGameObject.SimulateTick(0.1f);
					}
				}
			}
		}
	}
}
