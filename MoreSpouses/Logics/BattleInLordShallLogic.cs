using SandBox;
using SandBox.Missions.MissionLogics;
using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SueMoreSpouses.Logics
{
	internal class BattleInLordShallLogic : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		private BattleAgentLogic _battleAgentLogic;

		private bool _isMissionInitialized;

		private bool _troopsInitialized;

		private int _numberOfMaxTroopForPlayer;

		private int _numberOfMaxTroopForEnemy;

		private int _playerSideSpawnedTroopCount;

		private int _otherSideSpawnedTroopCount;

		private readonly IMissionTroopSupplier[] _troopSuppliers;

		public BattleInLordShallLogic(IMissionTroopSupplier[] suppliers, int numberOfMaxTroopForPlayer, int numberOfMaxTroopForEnemy)
		{
			this._troopSuppliers = suppliers;
			this._numberOfMaxTroopForPlayer = numberOfMaxTroopForPlayer;
			this._numberOfMaxTroopForEnemy = numberOfMaxTroopForEnemy;
		}

		public override void AfterStart()
		{
			base.AfterStart();
			this._battleAgentLogic = Mission.Current.GetMissionBehavior<BattleAgentLogic>();
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
						this._battleAgentLogic.OnAgentBuild(current, null);
					}
				}
			}
		}

		private void SpawnAgents()
		{
			IMissionTroopSupplier[] troopSuppliers = this._troopSuppliers;
			for (int i = 0; i < troopSuppliers.Length; i++)
			{
				foreach (IAgentOriginBase current in troopSuppliers[i].SupplyTroops(this._numberOfMaxTroopForPlayer + this._numberOfMaxTroopForEnemy).ToList<IAgentOriginBase>())
				{
					bool flag = current.IsUnderPlayersCommand || current.Troop.IsPlayerCharacter;
					bool flag2 = (!flag || this._numberOfMaxTroopForPlayer >= this._playerSideSpawnedTroopCount) && (flag || this._numberOfMaxTroopForEnemy >= this._otherSideSpawnedTroopCount);
					if (flag2)
					{
                        Vec3 initializePosiition = Vec3.Zero;

                        Mission.Current.SpawnTroop(current, flag, false, false, false, 0, 0, false, true, false, initializePosiition, null, null);
						bool flag3 = flag;
						if (flag3)
						{
							this._playerSideSpawnedTroopCount++;
						}
						else
						{
							this._otherSideSpawnedTroopCount++;
						}
					}
				}
			}
		}

		public void StopSpawner()
		{
		}

		public bool IsSideDepleted(BattleSideEnum side)
		{
			bool flag = side == base.Mission.PlayerTeam.Side;
			bool result;
			if (flag)
			{
				result = (this._troopSuppliers[(int)side].NumRemovedTroops == this._playerSideSpawnedTroopCount);
			}
			else
			{
				result = (this._troopSuppliers[(int)side].NumRemovedTroops == this._otherSideSpawnedTroopCount);
			}
			return result;
		}

        public void StartSpawner(BattleSideEnum side)
        {
            //throw new NotImplementedException();
        }

        public void StopSpawner(BattleSideEnum side)
        {
           // throw new NotImplementedException();
        }

        public bool IsSideSpawnEnabled(BattleSideEnum side)
        {
            return true;
        }

        public float GetReinforcementInterval()
        {
            return 0;
           // throw new NotImplementedException();
        }
    }
}
