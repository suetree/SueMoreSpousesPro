using SueMBService.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace SueLordFromFamily.Actions
{
    public class ClanCreateAction
    {

		public int SelectClanTier = 2;

		public bool IsTogetherWithThireChildren;

		public Hero TargetSpouse
		{
			get;
			set;
		}

		public Settlement TargetSettlement
		{
			get;
			set;
		}

		public void reset()
		{
			this.TargetSpouse = null;
			this.TargetSettlement = null;
			this.IsTogetherWithThireChildren = false;
		}

		public void CreateVassal()
		{
			bool flag = this.TargetSettlement == null;
			if (!flag)
			{
				Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
				bool flag2 = oneToOneConversationHero == null;
				if (!flag2)
				{
					Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
					bool flag3 = kingdom == null;
					if (!flag3)
					{
						CultureObject culture = this.TargetSettlement.Culture;
                        if (culture.StringId.ToLower() == "paleician")
                        {

                        }
						TextObject textObject = NameGenerator.Current.GenerateClanName(culture, this.TargetSettlement);
						string str = Guid.NewGuid().ToString().Replace("-", "");
					/*	bool flag4 = oneToOneConversationHero.LastSeenPlace == null;
						if (flag4)
						{
							//oneToOneConversationHero.CacheLastSeenInformation(oneToOneConversationHero.HomeSettlement, true);
							//oneToOneConversationHero.SyncLastSeenInformation();
						}*/
                        OccuptionService.ChangeOccupation0fHero(oneToOneConversationHero, Occupation.Lord);
                        ClanLordService.DealApplyByFire(oneToOneConversationHero);
                       
                        oneToOneConversationHero.ChangeState(Hero.CharacterStates.Active);
						//Clan clan = MBObjectManager.Instance.CreateObject<Clan>("sue_clan_" + str);
						Clan clan = Clan.CreateClan("sue_clan_" + str);
						Banner banner = Banner.CreateRandomClanBanner(-1);
						clan.InitializeClan(textObject, textObject, culture, banner, default(Vec2), false);
						clan.SetLeader(oneToOneConversationHero);
						FieldInfo field = clan.GetType().GetField("_tier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						bool flag5 = null != field;
						if (flag5)
						{
							field.SetValue(clan, this.SelectClanTier);
						}
						clan.AddRenown((float)(50 * this.SelectClanTier), true);
						oneToOneConversationHero.Clan = clan;
						oneToOneConversationHero.CompanionOf = null;
						//oneToOneConversationHero.IsNoble = true;
						//oneToOneConversationHero.SetTraitLevel(DefaultTraits.Commander, 1);
						MobileParty mobileParty = clan.CreateNewMobileParty(oneToOneConversationHero);
						mobileParty.ItemRoster.AddToCounts(DefaultItems.Grain, 10);
						mobileParty.ItemRoster.AddToCounts(DefaultItems.Meat, 5);
						ChangeOwnerOfSettlementAction.ApplyByKingDecision(oneToOneConversationHero, this.TargetSettlement);
						clan.UpdateHomeSettlement(this.TargetSettlement);
						int num = this.TakeMoneyByTier(this.SelectClanTier);
						bool flag6 = this.TargetSpouse != null;
						if (flag6)
						{
							GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, oneToOneConversationHero, num / 2, false);
						}
						else
						{
							GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, oneToOneConversationHero, num, false);
						}
						int relation = this.ShipIncreateByTier(this.SelectClanTier);
						ChangeRelationAction.ApplyPlayerRelation(oneToOneConversationHero, relation, true, true);
						bool flag7 = this.TargetSpouse != null;
						if (flag7)
						{
							ChangeRelationAction.ApplyPlayerRelation(this.TargetSpouse, relation, true, true);
						}
						int shipReduce = this.ShipReduceByTier(this.SelectClanTier);
						Kingdom kingdom2 = Hero.MainHero.MapFaction as Kingdom;
						bool flag8 = kingdom2 != null && shipReduce > 0;
						if (flag8)
						{
							kingdom2.Clans.ToList<Clan>().ForEach(delegate (Clan obj)
							{
								bool flag11 = obj != Clan.PlayerClan;
								if (flag11)
								{
									ChangeRelationAction.ApplyPlayerRelation(obj.Leader, shipReduce * -1, true, true);
								}
							});
						}
						bool flag9 = this.TargetSpouse != null;
						if (flag9)
						{
							this.TargetSpouse.Spouse = oneToOneConversationHero;
							//InformationManager.AddQuickInformation(new TextObject(string.Format("{0} marry with {1}", oneToOneConversationHero.Name, this.TargetSpouse.Name), null), 0, null, "event:/ui/notification/quest_finished");
							ClanLordService.DealApplyByFire( this.TargetSpouse);
							this.TargetSpouse.ChangeState(Hero.CharacterStates.Active);
							//this.TargetSpouse.IsNoble = true;
                            OccuptionService.ChangeOccupation0fHero(this.TargetSpouse, Occupation.Lord);
                            this.TargetSpouse.CompanionOf = null;
							this.TargetSpouse.Clan = clan;
							//this.targetSpouse.SetTraitLevel(DefaultTraits.Commander, 1);
							MobileParty mobileParty2 = clan.CreateNewMobileParty(this.TargetSpouse);
							mobileParty2.ItemRoster.AddToCounts(DefaultItems.Grain, 10);
							mobileParty2.ItemRoster.AddToCounts(DefaultItems.Meat, 5);
							GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, this.TargetSpouse, num / 2, false);
						}
						bool flag10 = this.IsTogetherWithThireChildren;
						if (flag10)
						{
							this.DealTheirChildren(oneToOneConversationHero, clan);
						}
						ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom, true);
					}
				}
			}
		}

		public int TakeMoneyByTier(int tier)
		{
			return (int)Math.Pow(5.0, (double)tier) * 1000;
		}

		public int ShipIncreateByTier(int tier)
		{
			return tier * 10;
		}

		public int ShipReduceByTier(int tier)
		{
			int num = tier * 10;
			Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
			bool flag = kingdom != null && kingdom.Clans.Count >= 3;
			if (flag)
			{
				num /= kingdom.Clans.Count - 1;
			}
			return num;
		}

		public void DealTheirChildren(Hero hero, Clan clan)
		{
			bool flag = hero.Children.Count > 0;
			if (flag)
			{
				hero.Children.ForEach(delegate (Hero chilredn)
				{
					ClanLordService.NewClanAllocateForHero(chilredn, clan);
					this.DealTheirChildren(chilredn, clan);
				});
			}
		}

		public static List<Settlement> GetCandidateSettlements()
		{
			Vec2 playerPosition = Hero.MainHero.PartyBelongedTo.Position2D;
			IEnumerable<Settlement> settlements = Hero.MainHero.Clan.Settlements;
			Func<Settlement, bool> predicate = (settlement) => settlement.IsTown || settlement.IsCastle;
			return (from n in settlements.Where(predicate)
					orderby n.Position2D.Distance(playerPosition)
					select n).ToList<Settlement>();
		}
	}
}
