
using SueEasyMenu.Models;
using SueMBService.API;
using SueTheGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using static SueTheGeneration.TheGenerationDataSetting;

namespace SueTheGeneration.Actions
{
    class TheGenerationAction
    {
		public static void PreOnGameStart()
		{
			TheGenerationDataSetting setting = TheGenerationDataSetting.Instance;

			if (setting.EnableGroupInfluence) StartSetForFaction(setting);
			if (setting.EnableGroupTroop) StartSetTroop(setting);
			if (setting.EnableGroupCompanion) StartSetCompanion(setting);
			if (setting.EnableGroupPlayerFamily) StartSetPlayerFamily(setting);

		}
		

		private static void StartSetForFaction(TheGenerationDataSetting setting)
        {
			if (setting.ResourceGold > 0)
			{
				GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, setting.ResourceGold * 10 * 1000, true);
			}
			if (setting.ResourceMeat > 0)
			{
				MobileParty.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, setting.ResourceMeat);
				MobileParty.MainParty.ItemRoster.AddToCounts(DefaultItems.Meat, setting.ResourceMeat);
			}

			if (setting.ClanTier > Clan.PlayerClan.Tier)
			{
				FieldInfo fieldInfoId = Clan.PlayerClan.GetType().GetField("_tier", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (null != fieldInfoId)
				{
					fieldInfoId.SetValue(Clan.PlayerClan, setting.ClanTier);
				}
			}
			Clan.PlayerClan.AddRenown(setting.ClanRenown, true);
			Clan.PlayerClan.Influence += setting.ClanInfluence;

			bool enableKimdom = setting.EnableKimdom;
			if (enableKimdom)
			{
				List<Settlement> list = DistributeRandomSettlement(setting);
				CreateKingdom(list, setting.KindomCulture.Value as CultureObject);
			}
		}

		private static void StartSetTroop(TheGenerationDataSetting setting)
        {
			FillTroops(setting);
		}

		private static void StartSetCompanion(TheGenerationDataSetting setting)
        {
			bool flag4 = setting.HeroNumbers > 0;
			if (flag4)
			{
				List<Hero> list = CharacterCreateService.GeranateHeros(setting.HeroNumbers, setting.HeroFromTier, "", setting.HeroSkillLevel, setting.HeroTakeAllPerks);
				bool flag5 = setting.HeroCoupleMaxNumbers > 0 && setting.HeroNumbers > 1;
				if (flag5)
				{
					CharacterCreateService.SetHeroCoupleAndChildren(list, setting.HeroCoupleMaxNumbers, setting.HeroCoupleChildrenProbability);
				}
			}
		}

		private static void StartSetPlayerFamily(TheGenerationDataSetting setting)
        {
			if (setting.PlayerSkill > 0)
			{
				foreach (SkillObject current in TaleWorlds.CampaignSystem.Skills.All)
				{
					Hero.MainHero.HeroDeveloper.ChangeSkillLevel(current, setting.PlayerSkill, false);
				}
			}

			if (null != setting.PlayerSpouse && setting.PlayerSpouse.Value is CharacterObject)
			{
				CharacterObject character = setting.PlayerSpouse.Value as CharacterObject;
				if (character.IsHero)
				{
					SpouseService.MainHeroMarryTo(character.HeroObject);
				}
			}

			if (null != setting.PlayerChildren && setting.PlayerChildren.Count > 0)
			{
				foreach (object obj in setting.PlayerChildren)
				{
					if (null != obj && obj is ChildrenTemplateData)
					{
						ChildrenTemplateData templateData = obj as ChildrenTemplateData;

						if (templateData.Mother.Value is CharacterObject)
						{
							CharacterObject character = templateData.Mother.Value as CharacterObject;
							if (character.IsHero)
							{
								for (int i = 0; i < templateData.Number; i++)
								{
									Hero hero = HeroCreator.DeliverOffSpring( character.HeroObject, Hero.MainHero, false, Hero.MainHero.Culture, templateData.Age);
									if (templateData.Age < Campaign.Current.Models.AgeModel.HeroComesOfAge)
									{
										//Settlement
									}
									else
									{
										MobileParty.MainParty.MemberRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
									}
									hero.UpdatePlayerGender(templateData.IsFemale);
									int skillLevel = 0;
									if (templateData.MinSkill <= templateData.MaxSkill)
									{
										skillLevel = new Random().Next(templateData.MinSkill, templateData.MaxSkill);
									}

									foreach (SkillObject current in TaleWorlds.CampaignSystem.Skills.All)
									{
										hero.HeroDeveloper.ChangeSkillLevel(current, skillLevel, false);
										//hero.HeroDeveloper.TakeAllPerks(current);
									}
								}

							}
						}

					}

				}
			}
		}


		private static void FillTroops(TheGenerationDataSetting setting)
        {
			ArrayList soldierArrayList = setting.Soldiers;
			if (null != soldierArrayList && soldierArrayList.Count > 0)
			{
				foreach (object obj in soldierArrayList)
				{
					if (obj is SoldierTemplateData)
					{
						EMOptionPair pair = (obj as SoldierTemplateData).Soldier;
						int num = (obj as SoldierTemplateData).Number;
						CharacterObject character = (CharacterObject)pair.Value;
						MobileParty.MainParty.MemberRoster.AddToCounts(character, num, false, 0, 0, true, -1);
					}
				}
			}
		}

		private static List<Settlement> DistributeRandomSettlement(TheGenerationDataSetting setting)
		{
			List<Settlement> list = new List<Settlement>();
			setting.FiefSettlements.ForEach(obj => list.Add(obj.Value as Settlement));
			list.ForEach(obj => ChangeOwnerOfSettlementAction.ApplyByKingDecision(Hero.MainHero, obj));
			bool flag2 = list.Count > 0;
			if (flag2)
			{
				MobileParty.MainParty.Position2D = list.First<Settlement>().GatePosition;
			}

			return list;
		}

		private static void CreateKingdom( List<Settlement> list, CultureObject culture)
		{
			Kingdom kingdom = MBObjectManager.Instance.CreateObject<Kingdom>("playerland_kingdom");
			TextObject textObject = new TextObject("{=72pbZgQL}{CLAN_NAME}", null);
			textObject.SetTextVariable("CLAN_NAME", Clan.PlayerClan.Name);
			TextObject textObject3 = new TextObject("{=EXp18CLD}Kingdom of the {CLAN_NAME}", null);
			textObject3.SetTextVariable("CLAN_NAME", Clan.PlayerClan.Name);
			kingdom.InitializeKingdom(textObject3, textObject, culture, Clan.PlayerClan.Banner, Clan.PlayerClan.Color, Clan.PlayerClan.Color2, list.First<Settlement>());
			ChangeKingdomAction.ApplyByJoinToKingdom(Clan.PlayerClan, kingdom, true);
			kingdom.RulingClan = Clan.PlayerClan;

			//StoryMode.Current.MainStoryLine.CompleteFirstPhase();
			//StoryMode.Current.MainStoryLine.SetStoryLineSide(isImperial ? MainStoryLineSide.CreateImperialKingdom : MainStoryLineSide.CreateAntiImperialKingdom);
		}


	

		
	}
}
