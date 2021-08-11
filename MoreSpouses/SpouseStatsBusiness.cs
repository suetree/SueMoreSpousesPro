using HarmonyLib;
using SueMoreSpouses.Data;
using SueMoreSpouses.Data.sp;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SueMoreSpouses
{
	internal class SpouseStatsBusiness
	{
		private List<SpousesHeroStatistic> _spousesStats;

		private List<SpousesBattleRecord> _spousesBattleRecordDatas;

		private Dictionary<CharacterObject, int> TempHeroStatisticRecordDic = new Dictionary<CharacterObject, int>();

		private Hero lastHero = null;

		private int lastKillCount = 0;

		private SpousesBattleRecordSide attackerSide;

		private SpousesBattleRecordSide defenderSide;

		private SpousesBattleRecord spousesBattleRecord;

		private Banner attackerBanner;

		private Banner defenderBanner;

		private BattleSideEnum PlayerSide;

		public SpouseStatsBusiness(List<SpousesHeroStatistic> spousesStats, List<SpousesBattleRecord> spousesBattleRecordDatas)
		{
			this._spousesStats = spousesStats;
			this._spousesBattleRecordDatas = spousesBattleRecordDatas;
		}

		public void EndCountHeroBattleData(int battleResultIndex, float renownChange, float influenceChange, float moraleChange, float goldChange, float playerEarnedLootPercentage)
		{
			bool flag = this.defenderSide != null && this.attackerSide != null;
			if (flag)
			{
				bool flag2 = this.spousesBattleRecord == null;
				if (flag2)
				{
					this.spousesBattleRecord = new SpousesBattleRecord();
					this.spousesBattleRecord.BattleResultIndex = battleResultIndex;
					this.spousesBattleRecord.RecordReward = new SpousesBattleRecordReward(renownChange, influenceChange, moraleChange, goldChange, playerEarnedLootPercentage);
				}
				this.spousesBattleRecord.AttackerSide = this.attackerSide;
				this.spousesBattleRecord.DefenderSide = this.defenderSide;
				bool flag3 = this._spousesBattleRecordDatas.Count >= 20;
				if (flag3)
				{
					this._spousesBattleRecordDatas.RemoveAt(this._spousesBattleRecordDatas.Count - 1);
				}
				bool flag4 = this.spousesBattleRecord != null;
				if (flag4)
				{
					this._spousesBattleRecordDatas.Insert(0, this.spousesBattleRecord);
				}
			}
			bool flag5 = this.TempHeroStatisticRecordDic.Count > 0;
			if (flag5)
			{
				foreach (KeyValuePair<CharacterObject, int> current in this.TempHeroStatisticRecordDic)
				{
					this.CountHeroBattleDataForStatistic(current.Key, current.Value);
				}
			}
			bool flag6 = this.lastHero != null;
			if (flag6)
			{
				this._spousesStats.ForEach(delegate(SpousesHeroStatistic obj)
				{
					bool flag7 = obj.StatsHero == this.lastHero;
					if (flag7)
					{
						obj.MVPCount++;
					}
				});
			}
			this.ResetData();
		}

		public void Initialize()
		{
			this.ResetData();
			this.PlayerSide = (PlayerEncounter.PlayerIsAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
			bool flag = MobileParty.MainParty.MapEvent != null;
			if (flag)
			{
				this.attackerBanner = MobileParty.MainParty.MapEvent.DefenderSide.LeaderParty.Banner;
				this.defenderBanner = MobileParty.MainParty.MapEvent.AttackerSide.LeaderParty.Banner;
			}
		}

		public void ResetData()
		{
			this.lastHero = null;
			this.lastKillCount = 0;
			this.attackerSide = null;
			this.defenderSide = null;
			this.attackerBanner = null;
			this.defenderBanner = null;
			this.spousesBattleRecord = null;
			this.TempHeroStatisticRecordDic.Clear();
		}

		private void RecordHeroBattleStatistic(CharacterObject character, int killCount)
		{
			bool flag = character != null && MobileParty.MainParty != null;
			if (flag)
			{
				bool flag2 = character.IsHero && character.HeroObject.Clan == Clan.PlayerClan;
				if (flag2)
				{
					bool flag3 = this.TempHeroStatisticRecordDic.ContainsKey(character);
					if (flag3)
					{
						int num = this.TempHeroStatisticRecordDic.GetValueSafe(character);
						num += killCount;
						this.TempHeroStatisticRecordDic[character] = num;
					}
					else
					{
						this.TempHeroStatisticRecordDic.Add(character, killCount);
					}
				}
			}
		}

		public void RecordBattleData(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int remain, int killed, int killCount, int wounder, int wounded)
		{
			bool flag = character is CharacterObject;
			if (flag)
			{
				this.RecordHeroBattleStatistic((CharacterObject)character, killCount);
			}
			bool flag2 = side == BattleSideEnum.Attacker;
			if (flag2)
			{
				bool flag3 = this.attackerSide == null;
				if (flag3)
				{
					Banner banner = null;
					bool flag4 = this.attackerBanner != null;
					if (flag4)
					{
						banner = this.attackerBanner;
					}
					else
					{
						bool flag5 = MobileParty.MainParty.MapEvent != null;
						if (flag5)
						{
							banner = MobileParty.MainParty.MapEvent.AttackerSide.LeaderParty.Banner;
						}
					}
					this.attackerSide = new SpousesBattleRecordSide(GameTexts.FindText("str_battle_result_army", "attacker").ToString(), banner);
				}
				this.SetSideData(this.attackerSide, battleCombatant, character, remain, killCount, killed, wounder, wounded);
			}
			else
			{
				bool flag6 = this.defenderSide == null;
				if (flag6)
				{
					Banner banner2 = null;
					bool flag7 = this.defenderBanner != null;
					if (flag7)
					{
						banner2 = this.defenderBanner;
					}
					else
					{
						bool flag8 = MobileParty.MainParty.MapEvent != null;
						if (flag8)
						{
							banner2 = MobileParty.MainParty.MapEvent.DefenderSide.LeaderParty.Banner;
						}
					}
					this.defenderSide = new SpousesBattleRecordSide(GameTexts.FindText("str_battle_result_army", "defender").ToString(), banner2);
				}
				this.SetSideData(this.defenderSide, battleCombatant, character, remain, killCount, killed, wounder, wounded);
			}
		}

		private void SetSideData(SpousesBattleRecordSide side, IBattleCombatant battleCombatant, BasicCharacterObject character, int remain, int killCount, int killed, int wounder, int wounded)
		{
			bool flag = side != null;
			if (flag)
			{
				side.Remain += remain;
				side.KillCount += killCount;
				side.Killed += killed;
				side.Wounded += wounded;
				side.RunAway += wounder;
				SpousesBattleRecordParty spousesBattleRecordParty = side.GetPartyByUniqueId(battleCombatant.GetHashCode().ToString() ?? "");
				bool flag2 = spousesBattleRecordParty == null;
				if (flag2)
				{
					spousesBattleRecordParty = new SpousesBattleRecordParty(battleCombatant.GetHashCode().ToString() ?? "");
					spousesBattleRecordParty.Name = battleCombatant.Name.ToString();
					side.Parties.Add(spousesBattleRecordParty);
				}
				spousesBattleRecordParty.Remain += remain;
				spousesBattleRecordParty.KillCount += killCount;
				spousesBattleRecordParty.Killed += killed;
				spousesBattleRecordParty.Wounded += wounded;
				spousesBattleRecordParty.RunAway += wounder;
				SpousesBattleRecordCharacter spousesBattleRecordCharacter = spousesBattleRecordParty.GetBattleRecordCharacter(character);
				bool flag3 = spousesBattleRecordCharacter == null;
				if (flag3)
				{
					spousesBattleRecordCharacter = new SpousesBattleRecordCharacter(character);
					spousesBattleRecordParty.Characters.Add(spousesBattleRecordCharacter);
				}
				spousesBattleRecordCharacter.Remain += remain;
				spousesBattleRecordCharacter.KillCount += killCount;
				spousesBattleRecordCharacter.Killed += killed;
				spousesBattleRecordCharacter.Wounded += wounded;
				spousesBattleRecordCharacter.RunAway += wounder;
			}
		}

		public void CountHeroBattleDataForStatistic(CharacterObject character, int killCount)
		{
			bool flag = character != null && character.IsHero && this._spousesStats != null;
			if (flag)
			{
				bool flag2 = character != null && character.HeroObject.Clan != null && character.HeroObject.Clan == Clan.PlayerClan;
				if (flag2)
				{
					bool containHero = false;
					this._spousesStats.ForEach(delegate(SpousesHeroStatistic obj)
					{
						bool flag7 = obj.StatsHero == character.HeroObject;
						if (flag7)
						{
							obj.TotalKillCount += killCount;
							obj.FightCount++;
							bool flag8 = killCount == 0;
							if (flag8)
							{
								obj.ZeroCount++;
							}
							containHero = true;
						}
					});
					bool flag3 = !containHero;
					if (flag3)
					{
						SpousesHeroStatistic spousesHeroStatistic = new SpousesHeroStatistic(character.HeroObject);
						spousesHeroStatistic.TotalKillCount = killCount;
						spousesHeroStatistic.FightCount++;
						bool flag4 = killCount == 0;
						if (flag4)
						{
							spousesHeroStatistic.ZeroCount++;
						}
						this._spousesStats.Add(spousesHeroStatistic);
					}
					bool flag5 = this.lastHero == null;
					if (flag5)
					{
						this.lastHero = character.HeroObject;
						this.lastKillCount = killCount;
					}
					else
					{
						bool flag6 = killCount > this.lastKillCount;
						if (flag6)
						{
							this.lastHero = character.HeroObject;
							this.lastKillCount = killCount;
						}
					}
				}
			}
		}
	}
}
