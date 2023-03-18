using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using static TaleWorlds.CampaignSystem.Hero;

namespace SueMBService.API
{
    public class ClanLordService
    {

        public static void DealLordForClan(Hero hero)
        {
            Clan clan = hero.Clan;
            if (clan == Clan.PlayerClan) { return; }
            if (clan.Leader == hero)
            {
                List<Hero> others = clan.Heroes.Where((obj) => (obj != hero && obj.IsAlive)).ToList();
                if (others.Count() > 0)
                {
                    Hero target = others.GetRandomElement();
                    ChangeClanLeaderAction.ApplyWithSelectedNewLeader(clan, target);
                 }
                else
                {
                    CampaignEventDispatcher eventDispatcher = CampaignEventDispatcher();
                    List<Settlement> settlements = clan.Settlements.ToList();
                    settlements.ForEach((settlement) => ChangeOwnerOfSettlementAction.ApplyByDestroyClan(settlement, Hero.MainHero));
                    List<Hero> deadHeros = clan.Heroes.Where((obj) => (obj != hero && obj.IsDead)).ToList();
                    Hero target = null;
                    if (deadHeros.Count > 0)
                    {
                        target = deadHeros.GetRandomElement();
                        clan.SetLeader(target);
                    }
                    else
                    {
                        CharacterObject character = CharacterObject.FindFirst(obj => obj.Culture == hero.Culture && obj.Occupation == Occupation.Lord);
                        target = HeroCreator.CreateSpecialHero(character, hero.HomeSettlement, null, null, -1);
                        target.ChangeState(CharacterStates.Dead);
                        target.Clan = clan;
                      
                        eventDispatcher.OnHeroCreated(target, false);
                        clan.SetLeader(target);
                    }

                    if (null != eventDispatcher)
                    {
                        eventDispatcher.OnClanLeaderChanged(hero, target);
                    }
                    DestroyClanAction.Apply(clan);
                    DealKindomLeader(clan, hero);
                }

            }
            hero.Clan = Clan.PlayerClan;
        }

        public static CampaignEventDispatcher CampaignEventDispatcher()
        {
            CampaignEventDispatcher dispatcher = null; ;
            PropertyInfo propertyInfo = Campaign.Current.GetType().GetProperty("CampaignEventDispatcher", BindingFlags.Instance | BindingFlags.NonPublic);
            if (null != propertyInfo)
            {
                dispatcher = (CampaignEventDispatcher)propertyInfo.GetValue(Campaign.Current);

            }

            return dispatcher;
        }

        public static void DealKindomLeader(Clan clan, Hero hero)
        {
            if (null != clan.Kingdom && clan.Kingdom.Leader == hero)
            {
                //InformationManager.DisplayMessage(new InformationMessage("clan.Kingdom.Leader  Change"));
                List<Clan> oteherClans = clan.Kingdom.Clans.Where((obj) => obj != clan && !obj.IsEliminated).ToList();
                if (oteherClans.Count > 0)
                {
                    IEnumerable<Clan> sortedStudents = from item in oteherClans orderby item.Renown descending select item;
                    Clan targetClan = sortedStudents.First();
                    clan.Kingdom.RulingClan = targetClan;
                    TextObject textObject = GameTexts.FindText("sue_more_spouses_kindom_leader_change", null);
                    StringHelpers.SetCharacterProperties("SUE_HERO", targetClan.Leader.CharacterObject,  textObject);
                    //InformationManager.ShowTextInquiry()
                    //InformationManager.AddQuickInformation(textObject, 0, null, "event:/ui/notification/quest_finished");
                    InformationManager.DisplayMessage(new InformationMessage(textObject.ToString() + "instead of QuickInformation"));
                }
                else
                {
                    //InformationManager.DisplayMessage(new InformationMessage("clan.Kingdom  destory"));
                    DestroyKingdomAction.Apply(clan.Kingdom);
                }

            }
        }

        public static void NewClanAllocateForHero(Hero hero, Clan clan)
        {
            bool flag = hero.Clan == Clan.PlayerClan;
            if (flag)
            {
                DealApplyByFire( hero);
                OccuptionService.ChangeOccupation0fHero(hero, Occupation.Lord);
                hero.Clan = clan;
                hero.CompanionOf = null;
                hero.ChangeState(Hero.CharacterStates.Active);
                // hero.IsNotable = true;
                bool flag2 = hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && hero.PartyBelongedTo == null;
                if (flag2)
                {
                    MobileParty mobileParty = clan.CreateNewMobileParty(hero);
                    mobileParty.ItemRoster.AddToCounts(DefaultItems.Grain, 10);
                    mobileParty.ItemRoster.AddToCounts(DefaultItems.Meat, 5);
                }
                GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, hero, 2000, false);
            }
        }

        public static void DealApplyByFire( Hero hero)
        {
            /*bool flag = hero.LastSeenPlace == null;
            if (flag)
            {
                hero.CacheLastSeenInformation(hero.HomeSettlement, true);
                hero.SyncLastSeenInformation();
            }*/
            RemoveCompanionAction.ApplyByFire(hero.Clan, hero);
        }

    }
}
