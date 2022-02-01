using SueMoreSpouses.Data;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem;

namespace SueMoreSpouses.Behaviors
{
	internal class SpousesStatsBehavior : CampaignBehaviorBase
	{
		[SaveableField(1)]
		private List<SpousesHeroStatistic> _spousesStats;

		[SaveableField(2)]
		private List<SpousesBattleRecord> _spousesBattleRecords;

		private SpouseStatsBusiness spouseStatsBusiness;

		public override void RegisterEvents()
		{
		}

		public override void SyncData(IDataStore dataStore)
		{
			try
			{
				dataStore.SyncData<List<SpousesHeroStatistic>>("SpousesStats", ref this._spousesStats);
				dataStore.SyncData<List<SpousesBattleRecord>>("SpousesBattleRecords", ref this._spousesBattleRecords);
			}
			catch (Exception)
			{
			}
			finally
			{
			}
		}

		public void MapBattle(MapEvent mapEvent)
		{
			bool flag = Hero.MainHero != null && mapEvent.IsPlayerMapEvent && mapEvent.IsPlayerSimulation;
			if (flag)
			{
			}
		}

		public void CombatBattle(IMission imission)
		{
			bool flag = this._spousesStats == null;
			if (flag)
			{
				this._spousesStats = new List<SpousesHeroStatistic>();
			}
			Mission mission = imission as Mission;
			bool flag2 = mission != null && (mission.CombatType == Mission.MissionCombatType.Combat || mission.CombatType == Mission.MissionCombatType.ArenaCombat);
			if (flag2)
			{
				Mission.Current.AddMissionBehavior(new SpousesStatsMissionLogic(this.GetSpouseStatsBusiness()));
			}
		}

		public List<SpousesHeroStatistic> SpousesStats()
		{
			bool flag = this._spousesStats == null;
			if (flag)
			{
				this._spousesStats = new List<SpousesHeroStatistic>();
			}
			return this._spousesStats;
		}

		public void ClanAllData()
		{
			this.SpousesBattleRecords().Clear();
			this.SpousesStats().Clear();
		}

		public List<SpousesBattleRecord> SpousesBattleRecords()
		{
			bool flag = this._spousesBattleRecords == null;
			if (flag)
			{
				this._spousesBattleRecords = new List<SpousesBattleRecord>();
			}
			return this._spousesBattleRecords;
		}

		public SpouseStatsBusiness GetSpouseStatsBusiness()
		{
			bool flag = this.spouseStatsBusiness == null;
			if (flag)
			{
				this.spouseStatsBusiness = new SpouseStatsBusiness(this.SpousesStats(), this.SpousesBattleRecords());
			}
			return this.spouseStatsBusiness;
		}
	}
}
