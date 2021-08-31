
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SueMBService.API
{
    public  class CharacterCreateService
    {
		public  static void GeranateTroops(int num, int level, string culture)
		{
			List<CharacterObject> list = new List<CharacterObject>();
			bool flag = CharacterObject.FindAll((CharacterObject obj) => obj != null && obj.Culture != null && obj.Culture.Name.ToString().Equals(culture)).Count<CharacterObject>() > 0;
			if (flag)
			{
				list = CharacterObject.FindAll((CharacterObject t) => t.IsSoldier && t.Tier == level && t.Culture.Name.ToString().Equals(culture)).ToList<CharacterObject>();
			}
			else
			{
				list = CharacterObject.FindAll((CharacterObject t) => t.IsSoldier && t.Tier == level).ToList<CharacterObject>();
			}
			bool flag2 = list.Count > 0;
			if (flag2)
			{
				for (int i = 0; i < num; i++)
				{
					CharacterObject randomElement = list.GetRandomElement<CharacterObject>();
					MobileParty.MainParty.MemberRoster.AddToCounts(randomElement, 1, false, 0, 0, true, -1);
				}
			}
		}


		public  static List<Hero> GeranateHeros(int num, int level, string culture, int skillLevl, bool TakeAllPerks)
		{
			List<Hero> list = new List<Hero>();
			List<CharacterObject> e = new List<CharacterObject>();
			bool flag = CharacterObject.FindAll((CharacterObject obj) => obj != null && obj.Culture != null && obj.Culture.Name.ToString().Equals(culture)).Count<CharacterObject>() > 0;
			if (flag)
			{
				e = CharacterObject.FindAll((CharacterObject t) => t.IsSoldier && t.Tier == level && t.Culture.Name.ToString().Equals(culture)).ToList<CharacterObject>();
			}
			else
			{
				e = CharacterObject.FindAll((CharacterObject t) => t.IsSoldier && t.Tier == level).ToList<CharacterObject>();
			}
			for (int i = 0; i < num; i++)
			{
				CharacterObject randomElement = e.GetRandomElement<CharacterObject>();
				randomElement.IsFemale = true;
				Hero hero = HeroCreator.CreateSpecialHero(randomElement, Hero.MainHero.BornSettlement, Clan.PlayerClan, Clan.PlayerClan, 20);
				MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
				hero.ChangeState(Hero.CharacterStates.Active);
				hero.CompanionOf = Clan.PlayerClan;
				hero.CacheLastSeenInformation(hero.HomeSettlement, true);
				hero.SyncLastSeenInformation();
				bool flag2 = skillLevl > 0;
				if (flag2)
				{
					foreach (SkillObject current in TaleWorlds.CampaignSystem.Skills.All)
					{
						hero.HeroDeveloper.ChangeSkillLevel(current, skillLevl, false);
						if (TakeAllPerks)
						{
							hero.HeroDeveloper.TakeAllPerks(current);
						}
					}
				}

				OccuptionService.ChangeToWanderer(hero.CharacterObject);
				list.Add(hero);
			}
			return list;
		}

		public static void SetHeroCoupleAndChildren(List<Hero> list, int maxNumber , float childrenProbability)
        {
			List<Hero> list2 = new List<Hero>();
			List<Hero> spouseList = new List<Hero>();
			while (list.Count > 1 && spouseList.Count < maxNumber)
			{
				Hero randomElement = list.GetRandomElement<Hero>();
				list.Remove(randomElement);
				list2.Add(randomElement);
				Hero randomElement2 = list.GetRandomElement<Hero>();
				randomElement.Spouse = randomElement2;
     
				list.Remove(randomElement2);
				list2.Add(randomElement2);
				spouseList.Add(randomElement2);
			}
			float num = childrenProbability;
			bool flag6 = num > 0f;
			if (flag6)
			{
				bool flag7 = num > 1f;
				if (flag7)
				{
					num /= 10f;
				}
				spouseList.ForEach(delegate (Hero obj)
				{
					bool flag9 = MBRandom.RandomFloat <= childrenProbability;
					bool flag10 = flag9;
					if (flag10)
					{
						Hero hero = HeroCreator.DeliverOffSpring(obj, obj.Spouse, false);
					}
				});
			}
		}
	}
}
