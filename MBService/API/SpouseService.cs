
using Helpers;
using SueMBService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMBService.API
{
    public class SpouseService
    {
        public static void  MainHeroMarryTo(Hero hero)
        {
            hero.CompanionOf = null;
          
            OccuptionService.ChangeOccupation0fHero(hero, Occupation.Lord);
            ClanLordService.DealLordForClan(hero);
            MarryHero(hero);
            ResetPositionInClan(hero);
        }

        private static void MarryHero(Hero hero)
        {
            if (Hero.MainHero.Spouse == hero || Hero.MainHero.ExSpouses.Contains(hero)) return;
            //1.4.3 结婚后，第二个英雄状态会变成逃亡状态，并且会被所在部队移除, 所在部队还会解散。

            Hero mainSpouse = Hero.MainHero.Spouse;
            if (null == hero.Clan)
            {
                hero.Clan = Hero.MainHero.Clan;
            }
            bool needAddPlayerTroop = true;
            if (Hero.MainHero.PartyBelongedTo.MemberRoster.Contains(hero.CharacterObject))
            {
                Hero.MainHero.PartyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1);
            }
            if (hero.PartyBelongedTo != null)
            {
                MobileParty partyBelongedTo = hero.PartyBelongedTo;
                partyBelongedTo.MemberRoster.RemoveTroop(hero.CharacterObject, 1);
                partyBelongedTo.RemoveParty();
            }

            
            MarriageAction.Apply(Hero.MainHero, hero);
            hero.ChangeState(Hero.CharacterStates.Active);
            if (needAddPlayerTroop)
            {
                Hero.MainHero.PartyBelongedTo.MemberRoster.AddToCounts(hero.CharacterObject, 1);
            }
            RemoveRepeatExspouses(Hero.MainHero, Hero.MainHero.Spouse);
            if (null != mainSpouse)
            {
                SetPrimarySpouse(mainSpouse);
            }

        }

        public static void NoticeMarray(Hero hero)
        {
            TextObject textObject = GameTexts.FindText("sue_more_spouses_marry_target", null);
            StringHelpers.SetCharacterProperties("SUE_HERO", hero.CharacterObject, textObject);
            InformationManager.AddQuickInformation(textObject, 0, null, "event:/ui/notification/quest_finished");
        }


        public static void ResetPositionInClan(Hero hero)
        {
            if (null == hero.Clan) return;
           
            List<Hero> lordsCache = ReflectUtils.ReflectField<List<Hero>>("_lordsCache", hero.Clan);
            if (hero.Occupation == Occupation.Lord)
            {
                if (!lordsCache.Contains(hero))
                {
                        lordsCache.Add(hero);
                 }
            }
            else
            {
                if (lordsCache.Contains(hero))
                {
                    lordsCache.Remove(hero);
                }
            }
            ReflectUtils.ReflectPropertyAndSetValue("Lords", new MBReadOnlyList<Hero>(lordsCache), hero.Clan);

            List<Hero> companionsCache = ReflectUtils.ReflectField<List<Hero>>("_companionsCache", hero.Clan);
            if (hero.Occupation != Occupation.Lord)
            {
                if (!companionsCache.Contains(hero))
                {
                    companionsCache.Add(hero);
                }
            }
            else
            {
                if (companionsCache.Contains(hero))
                {
                    companionsCache.Remove(hero);
                }
            }
             ReflectUtils.ReflectPropertyAndSetValue("Companions", new MBReadOnlyList<Hero>(companionsCache), hero.Clan);
            
           
            List<Hero> heroCache = ReflectUtils.ReflectField<List<Hero>>("_heroesCache", hero.Clan);
            if (!heroCache.Contains(hero))
            {
                heroCache.Add(hero);
            }
            ReflectUtils.ReflectPropertyAndSetValue("Heros", new MBReadOnlyList<Hero>(heroCache), hero.Clan);
          
        }

        public static void RemoveRepeatExspouses(Hero hero, Hero target)
        {
            if (Hero.MainHero.ExSpouses.Count > 0)
            {
                FieldInfo fieldInfo = hero.GetType().GetField("_exSpouses", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                FieldInfo fieldInfo2 = hero.GetType().GetField("ExSpouses", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (null == fieldInfo || null == fieldInfo2) return;
                List<Hero> heroes = (List<Hero>)fieldInfo.GetValue(hero);
                MBReadOnlyList<Hero> heroes2 = (MBReadOnlyList<Hero>)fieldInfo2.GetValue(hero);
                //heroes.D
                heroes = heroes.Distinct(new DistinctSpouse<Hero>()).ToList();
                if (heroes.Contains(target))
                {
                    heroes.Remove(target);
                }
                fieldInfo.SetValue(hero, heroes);
                heroes2 = new MBReadOnlyList<Hero>(heroes);
                fieldInfo2.SetValue(hero, heroes2);
            }
        }


        public static void SetPrimarySpouse(Hero hero)
        {
            if (Hero.MainHero.Spouse != hero)
            {
                Hero.MainHero.Spouse = hero;
                hero.Spouse = Hero.MainHero;
                RemoveRepeatExspouses(Hero.MainHero, Hero.MainHero.Spouse);
                RemoveRepeatExspouses(hero, hero.Spouse);
            }
        }

        class DistinctSpouse<TModel> : IEqualityComparer<TModel>
        {
            public bool Equals(TModel x, TModel y)
            {
                Hero t = x as Hero;
                Hero tt = y as Hero;
                if (t != null && tt != null) return t.StringId == tt.StringId;
                return false;
            }

            public int GetHashCode(TModel obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }
}
