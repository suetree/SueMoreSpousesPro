using Helpers;
using SueMoreSpouses.Operation;
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

namespace SueMoreSpouses
{
	internal class HeroRlationOperation
	{
		private class DistinctTest<TModel> : IEqualityComparer<TModel>
		{
			public bool Equals(TModel x, TModel y)
			{
				Hero hero = x as Hero;
				Hero hero2 = y as Hero;
				bool flag = hero != null && hero2 != null;
				return flag && hero.StringId == hero2.StringId;
			}

			public int GetHashCode(TModel obj)
			{
				return obj.ToString().GetHashCode();
			}
		}

	

		public static void ChangeCompanionToSpouse(Hero hero)
		{
			bool flag = hero == null || !hero.IsPlayerCompanion;
			if (!flag)
			{
				bool flag2 = Hero.MainHero.Spouse == hero || Hero.MainHero.ExSpouses.Contains(hero);
				if (!flag2)
				{
					hero.CompanionOf = null;
					OccuptionChange.ChangeOccupationToLord(hero.CharacterObject);
					HeroRlationOperation.MarryHero(hero);
					hero.IsNoble = true;
					HeroRlationOperation.RefreshClanPanelList(hero);
				}
			}
		}

		public static void NPCToSouse(CharacterObject character, CampaignGameStarter campaignGameStarter)
		{
			Hero hero = HeroRlationOperation.DealNPC(character, campaignGameStarter);
			bool flag = hero != null;
			if (flag)
			{
				hero.CompanionOf = null;
				OccuptionChange.ChangeOccupationToLord(hero.CharacterObject);
				HeroRlationOperation.MarryHero(hero);
				hero.IsNoble = true;
				HeroRlationOperation.RefreshClanPanelList(hero);
			}
		}

		public static void NPCToCompanion(CharacterObject character, CampaignGameStarter campaignGameStarter)
		{
			Hero hero = HeroRlationOperation.DealNPC(character, campaignGameStarter);
			OccuptionChange.ChangeToWanderer(hero.CharacterObject);
			bool flag = !MobileParty.MainParty.MemberRoster.Contains(hero.CharacterObject);
			if (flag)
			{
				MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
			}
			AddCompanionAction.Apply(Clan.PlayerClan, hero);
		}

		private static Hero DealNPC(CharacterObject target, CampaignGameStarter campaignGameStarter)
		{
			Hero hero = null;
			bool flag = target != null;
			if (flag)
			{
				CharacterObject oneToOneConversationCharacter = CharacterObject.OneToOneConversationCharacter;
				hero = HeroCreator.CreateSpecialHero(oneToOneConversationCharacter, null, Clan.PlayerClan, Clan.PlayerClan, -1);
				hero.ChangeState(Hero.CharacterStates.Active);
				hero.CacheLastSeenInformation(hero.HomeSettlement, true);
				hero.SyncLastSeenInformation();
				HeroInitPropertyUtils.InitHeroForNPC(hero);
				AddHeroToPartyAction.Apply(hero, MobileParty.MainParty, true);
				CampaignEventDispatcher.Instance.OnHeroCreated(hero, false);
				ConversationUtils.ChangeCurrentCharaObject(campaignGameStarter, hero);
				bool flag2 = hero.Age > 30f;
				if (flag2)
				{
					CampaignTime randomBirthDayForAge = HeroHelper.GetRandomBirthDayForAge(22f);
					hero.SetBirthDay(randomBirthDayForAge);
				}
			}
			return hero;
		}

		public static void ChangePrisonerLordToSpouse(Hero hero)
		{
			bool flag = hero == null && hero.CharacterObject.Occupation != Occupation.Lord;
			if (!flag)
			{
				HeroRlationOperation.DealLordForClan(hero);
				HeroRlationOperation.ChangePrisonerToParty(hero);
				HeroRlationOperation.MarryHero(hero);
			}
		}

		public static void ChangePrisonerWandererToSpouse(Hero hero)
		{
			bool flag = hero == null && hero.CharacterObject.Occupation != Occupation.Wanderer;
			if (!flag)
			{
				HeroRlationOperation.ChangePrisonerToParty(hero);
				OccuptionChange.ChangeOccupationToLord(hero.CharacterObject);
				HeroRlationOperation.MarryHero(hero);
			}
		}

		public static void ChangePrisonerLordToFamily(Hero hero)
		{
			bool flag = hero == null && hero.CharacterObject.Occupation != Occupation.Lord;
			if (!flag)
			{
				HeroRlationOperation.DealLordForClan(hero);
				HeroRlationOperation.ChangePrisonerToParty(hero);
			}
		}

		public static void DealLordForClan(Hero hero)
		{
			Clan clan = hero.Clan;
			bool flag = clan.Leader == hero;
			if (flag)
			{
				List<Hero> list = (from obj in clan.Heroes
				where obj != hero && obj.IsAlive
				select obj).ToList<Hero>();
				bool flag2 = list.Count<Hero>() > 0;
				if (flag2)
				{
					Hero randomElement = list.GetRandomElement<Hero>();
					ChangeClanLeaderAction.ApplyWithSelectedNewLeader(clan, randomElement);
                    TextObject textObject = GameTexts.FindText("sue_more_spouses_clan_leave", null);
                    StringHelpers.SetCharacterProperties("SUE_HERO", hero.CharacterObject, textObject);
					InformationManager.AddQuickInformation(textObject, 0, null, "event:/ui/notification/quest_finished");
                    textObject = GameTexts.FindText("sue_more_spouses_clan_change", null);
                    StringHelpers.SetCharacterProperties("SUE_HERO", randomElement.CharacterObject, textObject);
					InformationManager.AddQuickInformation(textObject, 0, null, "event:/ui/notification/quest_finished");
				}
				else
				{
					List<Settlement> list2 = clan.Settlements.ToList<Settlement>();
			
				   list2.ForEach((settlement) => {
					   ChangeOwnerOfSettlementAction.ApplyByDestroyClan(settlement, Hero.MainHero);
				   });
					List<Hero> list3 = (from obj in clan.Heroes
					where obj != hero && obj.IsDead
					select obj).ToList<Hero>();
					bool flag3 = list3.Count > 0;
					Hero hero2;
					if (flag3)
					{
						hero2 = list3.GetRandomElement<Hero>();
						clan.SetLeader(hero2);
					}
					else
					{
						CharacterObject template = CharacterObject.FindFirst((CharacterObject obj) => obj.Culture == hero.Culture && obj.Occupation == Occupation.Lord);
						hero2 = HeroCreator.CreateSpecialHero(template, hero.HomeSettlement, null, null, -1);
						hero2.ChangeState(Hero.CharacterStates.Dead);
						hero2.Clan = clan;
						CampaignEventDispatcher.Instance.OnHeroCreated(hero2, false);
						clan.SetLeader(hero2);
					}
					bool flag4 = GameComponent.CampaignEventDispatcher() != null;
					if (flag4)
					{
						GameComponent.CampaignEventDispatcher().OnClanLeaderChanged(hero, hero2);
					}
					DestroyClanAction.Apply(clan);
					HeroRlationOperation.dealKindomLeader(clan, hero);
				}
			}
			hero.Clan = Clan.PlayerClan;
		}

		public static void dealKindomLeader(Clan clan, Hero hero)
		{
			bool flag = clan.Kingdom != null && clan.Kingdom.Leader == hero;
			if (flag)
			{
				List<Clan> list = (from obj in clan.Kingdom.Clans
				where obj != clan && !obj.IsEliminated
				select obj).ToList<Clan>();
				bool flag2 = list.Count > 0;
				if (flag2)
				{
					IEnumerable<Clan> source = list.OrderByDescending((item) => item.Renown);
					Clan clan2 = source.First<Clan>();
					clan.Kingdom.RulingClan = clan2;
                    TextObject textObject = GameTexts.FindText("sue_more_spouses_kindom_leader_change", null);
                    StringHelpers.SetCharacterProperties("SUE_HERO", clan2.Leader.CharacterObject, textObject);
					InformationManager.AddQuickInformation(textObject, 0, null, "event:/ui/notification/quest_finished");
				}
				else
				{
					DestroyKingdomAction.Apply(clan.Kingdom);
				}
			}
		}

		private static void ChangePrisonerToParty(Hero hero)
		{
			bool flag = hero == null || !hero.IsPrisoner || !MobileParty.MainParty.PrisonRoster.Contains(hero.CharacterObject);
			if (!flag)
			{
				MobileParty.MainParty.PrisonRoster.RemoveIf((TroopRosterElement cobj) => cobj.Character.IsHero && cobj.Character.HeroObject == hero);
				hero.ChangeState(Hero.CharacterStates.Active);
				MobileParty.MainParty.AddElementToMemberRoster(hero.CharacterObject, 1, false);
			}
		}

		private static void MarryHero(Hero hero)
		{
			bool flag = Hero.MainHero.Spouse == hero || Hero.MainHero.ExSpouses.Contains(hero);
			if (!flag)
			{
				Hero spouse = Hero.MainHero.Spouse;
				bool flag2 = true;
				bool flag3 = Hero.MainHero.PartyBelongedTo.MemberRoster.Contains(hero.CharacterObject);
				if (flag3)
				{
					Hero.MainHero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
				}
				bool flag4 = hero.PartyBelongedTo != null;
				if (flag4)
				{
					MobileParty partyBelongedTo = hero.PartyBelongedTo;
					partyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1, default(UniqueTroopDescriptor), 0);
					partyBelongedTo.RemoveParty();
				}
				bool flag5 = hero.Clan == null;
				if (flag5)
				{
					hero.Clan = Hero.MainHero.Clan;
				}
				MarriageAction.Apply(Hero.MainHero, hero, true);
				hero.ChangeState(Hero.CharacterStates.Active);
				bool flag6 = flag2;
				if (flag6)
				{
					Hero.MainHero.PartyBelongedTo.MemberRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
				}
				SpouseOperation.RemoveRepeatExspouses(Hero.MainHero, Hero.MainHero.Spouse);
                TextObject textObject = GameTexts.FindText("sue_more_spouses_marry_target", null);
                StringHelpers.SetCharacterProperties("SUE_HERO", hero.CharacterObject, textObject);
				InformationManager.AddQuickInformation(textObject, 0, null, "event:/ui/notification/quest_finished");
				bool flag7 = spouse != null;
				if (flag7)
				{
					SpouseOperation.SetPrimarySpouse(spouse);
				}
			}
		}

		public static void DealApplyByFire(Clan clan, Hero hero)
		{
			bool flag = hero.LastSeenPlace == null;
			if (flag)
			{
				hero.CacheLastSeenInformation(hero.HomeSettlement, true);
				hero.SyncLastSeenInformation();
			}
			RemoveCompanionAction.ApplyByFire(Hero.MainHero.Clan, hero);
		}

		public static void RefreshClanPanelList(Hero hero)
		{
			FieldInfo field = Clan.PlayerClan.GetType().GetField("_nobles", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag = null != field;
			if (flag)
			{
				object value = field.GetValue(Clan.PlayerClan);
				bool flag2 = value != null;
				if (flag2)
				{
					List<Hero> list = (List<Hero>)value;
					bool flag3 = !list.Contains(hero);
					if (flag3)
					{
						list.Add(hero);
					}
				}
			}
			FieldInfo field2 = Clan.PlayerClan.GetType().GetField("_lords", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag4 = null != field2;
			if (flag4)
			{
				object value2 = field2.GetValue(Clan.PlayerClan);
				bool flag5 = value2 != null;
				if (flag5)
				{
					List<Hero> list2 = (List<Hero>)value2;
					bool flag6 = !list2.Contains(hero);
					if (flag6)
					{
						list2.Add(hero);
					}
				}
			}
		}
	}
}
