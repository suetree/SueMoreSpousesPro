using Helpers;
using SueMBService.API;
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
     

        public static void ChangeCompanionToSpouse(Hero hero)
        {
            bool flag = hero == null || !hero.IsPlayerCompanion;
            if (!flag)
            {
                bool flag2 = Hero.MainHero.Spouse == hero || Hero.MainHero.ExSpouses.Contains(hero);
                if (!flag2)
                {
                    SpouseService.MainHeroMarryTo(hero);
                    SpouseService.NoticeMarray(hero);
                }
            }
        }

        public static void NameLessNPCToSpouse(CharacterObject character, CampaignGameStarter campaignGameStarter)
        {
            Hero hero = HeroRlationOperation.GenerateHeroFromConversationCharacter(character, campaignGameStarter);
            bool flag = hero != null;
            if (flag)
            {
                SpouseService.MainHeroMarryTo(hero);
                SpouseService.NoticeMarray(hero);
            }
        }

        public static void NameLessNPCToCompanion(CharacterObject character, CampaignGameStarter campaignGameStarter)
        {
            Hero hero = HeroRlationOperation.GenerateHeroFromConversationCharacter(character, campaignGameStarter);
            OccuptionService.ChangeOccupation0fHero(hero, Occupation.Wanderer);
            bool flag = !MobileParty.MainParty.MemberRoster.Contains(hero.CharacterObject);
            if (flag)
            {
                MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
            }
            AddCompanionAction.Apply(Clan.PlayerClan, hero);
        }

        private static Hero GenerateHeroFromConversationCharacter(CharacterObject target, CampaignGameStarter campaignGameStarter)
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
                ClanLordService.DealLordForClan(hero);
                ChangePrisonerToParty(hero);
                SpouseService.MainHeroMarryTo(hero);
                SpouseService.NoticeMarray(hero);
            }
        }

  
        public static void ChangePrisonerLordToFamily(Hero hero)
        {
            bool flag = hero == null && hero.CharacterObject.Occupation != Occupation.Lord;
            if (!flag)
            {
                ClanLordService.DealLordForClan(hero);
                ChangePrisonerToParty(hero);
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

    }

	


}
