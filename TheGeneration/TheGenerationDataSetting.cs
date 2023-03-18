
using SueEasyMenu.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace SueTheGeneration
{
	class TheGenerationDataSetting
	{

		private static TheGenerationDataSetting _instance = new TheGenerationDataSetting();

		public static TheGenerationDataSetting Instance
		{
			get
			{
				return _instance;
			}

		}
		public bool EnableGroupInfluence { get; set; }
		public int ResourceGold { get; set; }
		public int ResourceMeat { get; set; }
		public int ClanTier { get; set; }
		public int ClanRenown { get; set; }
		public int ClanInfluence { get; set; }
		public bool EnableKimdom { get; set; }
		//public bool IsSupportImperial { get; set; }
		//public int FiefNumbers { get; set; }
		public EMOptionPair KindomCulture { get; set; }
		public List<EMOptionPair> FiefSettlements { get; set; }


		public bool EnableGroupCompanion { get; set; }
		//public bool EnableHero { get; set; }
		public int HeroNumbers { get; set; }
		public int HeroCoupleMaxNumbers { get; set; }
		public float HeroCoupleChildrenProbability { get; set; }
		public int HeroFromTier { get; set; }
		public int HeroSkillLevel { get; set; }
		public bool HeroTakeAllPerks { get; set; }


		public bool EnableGroupTroop { get; set; }
		public ArrayList Soldiers { get; set; }

		public bool EnableGroupPlayerFamily { get; set; }
		public EMOptionPair PlayerSpouse { get; set; }
		public ArrayList PlayerChildren { get; set; }
		public int PlayerSkill { get; set; }



		public List<EMOptionGroup> GenerateSettingData()
		{

			EMOptionBuilder optionBuilder = new EMOptionBuilder();
			optionBuilder.BuildGroup((val) => {
				EnableGroupInfluence = (bool)val;
				//InformationManager.DisplayMessage(new InformationMessage("EnableGroupInfluence = " + EnableGroupInfluence));
			}, "{=tsg_setting_clan}Clan", "{=tsg_setting_clan_describe}Enable clan settings")
				//.AddOption(new EMOptionItem("FiefNumbers", "{=trs_setting_fief_number}Fief Number", FiefNumbers, EMOptionType.IntegerProperty, 0, 10))
				.AddOption(new EMOptionItem((val) => EnableKimdom = (bool)val, "{=tsg_setting_kindom_creation}Enable kindom creation", EnableKimdom, EMOptionType.BoolProperty))
				.AddOption(new EMOptionItem((val) => KindomCulture = (EMOptionPair)val, "{=tsg_setting_kindom_cultrue}Kindom cultrue", KindomCulture, EMOptionType.SingleSelectProperty).FillSelectItems(CulturePairs()))
				.AddOption(new EMOptionItem((val) => FiefSettlements = (List<EMOptionPair>)val, "{=tsg_setting_fielf}Fielf", FiefSettlements, EMOptionType.MultipleSelectProperty).FillSelectItems(FiefPairs()))
				.AddOption(new EMOptionItem((val) => ClanTier = (int)val, "{=tsg_setting_clan_tier}Clan tier", ClanTier, EMOptionType.IntegerProperty, 1, 6))
				.AddOption(new EMOptionItem((val) => ClanRenown = (int)val, "{=tsg_setting_clan_renown}Clan renown", ClanRenown, EMOptionType.IntegerProperty, 0, 10000))
				.AddOption(new EMOptionItem((val) => ClanInfluence = (int)val, "{=tsg_setting_clan_influence}Clan influence", ClanInfluence, EMOptionType.IntegerProperty, 0, 50000))
				.AddOption(new EMOptionItem((val) => ResourceGold = (int)val, "{=tsg_setting_gold}Gold", ResourceGold, EMOptionType.IntegerProperty, 0, 300))
				.AddOption(new EMOptionItem((val) => ResourceMeat = (int)val, "{=tsg_setting_meat} Meat", ResourceMeat, EMOptionType.IntegerProperty, 0, 100));

			optionBuilder.BuildGroup((val) => EnableGroupCompanion = (bool)val,  "{=tsg_setting_companion}Companion", "{=tsg_setting_companion_describe}Enable conpanion settings")
				.AddOption(new EMOptionItem((val) => HeroNumbers = (int)val, "{=tsg_setting_number}Number", HeroNumbers, EMOptionType.IntegerProperty, 0, 100))
				.AddOption(new EMOptionItem((val) => HeroCoupleMaxNumbers = (int)val, "{=tsg_setting_companion_couple_max_num}Maximum number of couples", HeroCoupleMaxNumbers, EMOptionType.IntegerProperty, 0, 50))
				.AddOption(new EMOptionItem((val) => HeroCoupleChildrenProbability = (int)val, "{=tsg_setting_companion_couple_has_children_probility}Probability of couples having children", HeroCoupleChildrenProbability, EMOptionType.IntegerProperty, 0, 10))
				.AddOption(new EMOptionItem((val) => HeroFromTier = (int)val, "{=tsg_setting_companion_tier}Companion tier", HeroFromTier, EMOptionType.IntegerProperty, 1, 6))
				.AddOption(new EMOptionItem((val) => HeroSkillLevel = (int)val, "{=tsg_setting_companion_skill_level}Companion sill level", HeroSkillLevel, EMOptionType.IntegerProperty, 0, 300))
				.AddOption(new EMOptionItem((val) => HeroTakeAllPerks = (bool)val, "{=tsg_setting_companion_perk_auto}Companion perk auto", HeroTakeAllPerks, EMOptionType.BoolProperty));


			optionBuilder.BuildGroup((val) => EnableGroupTroop = (bool)val,  "{=tsg_setting_troop}Troop", "{=tsg_setting_troop_describe}Enable troop settings")
				.AddOption(new EMOptionItem((val) => Soldiers = (ArrayList)val, "{=tsg_setting_troop_list}Troop List", Soldiers, EMOptionType.ListProerty, SoldierFieldTemplate()).SetTargetInstanceType(typeof(SoldierTemplateData)));

			optionBuilder.BuildGroup((val) => EnableGroupPlayerFamily = (bool)val, "{=tsg_setting_player_family}Player family", "{=tsg_setting_player_family_describe}Enable player family settings")
			.AddOption(new EMOptionItem((val) => PlayerSkill = (int)val, "{=tsg_setting_player_skill_level}Player skill level", PlayerSkill, EMOptionType.IntegerProperty, 0, 300))
			.AddOption(new EMOptionItem((val) => PlayerSpouse = (EMOptionPair)val, "{=tsg_setting_player_spouse}Player spouse", PlayerSpouse, EMOptionType.SingleSelectProperty).FillSelectItems(HeroPairs()))
			.AddOption(new EMOptionItem((val) => PlayerChildren = (ArrayList)val, "{=tsg_setting_player_children_list}Children List", PlayerChildren, EMOptionType.ListProerty, ChildrenTemplate()).SetTargetInstanceType(typeof(ChildrenTemplateData)));

			//optionBuilder.BuildGroup("{=trs_setting_resource}Resource", "启用该选项，将会给玩家一些初始资源和自身属性");
			return optionBuilder.Groups;
		}

		private List<EMOptionItem> SoldierFieldTemplate() {
			List<EMOptionItem> list = new List<EMOptionItem>();
			list.Add(new EMOptionItem( "Soldier", null, "{=tsg_setting_character}Character", SoldierPairs()[0], EMOptionType.SingleSelectProperty).FillSelectItems(SoldierPairs()));
			list.Add(new EMOptionItem( "Number", null, "{=tsg_setting_number}Number", 1, EMOptionType.IntegerProperty, 0, 100));
			return list;
		}

		private List<EMOptionItem> ChildrenTemplate()
		{
			List<EMOptionItem> list = new List<EMOptionItem>();
			list.Add(new EMOptionItem( "Mother", null, "{=tsg_setting_player_children_mother}Children Mother", HeroPairs()[0], EMOptionType.SingleSelectProperty).FillSelectItems(HeroPairs()));
			list.Add(new EMOptionItem( "Number", null, "{=tsg_setting_number}Number", 1, EMOptionType.IntegerProperty, 0, 20));
			list.Add(new EMOptionItem( "Age", null, "{=tsg_setting_age}Age", 1, EMOptionType.IntegerProperty, 1, 30));
			list.Add(new EMOptionItem( "IsFemale", null, "{=tsg_setting_is_female}Is female", true, EMOptionType.BoolProperty));
			list.Add(new EMOptionItem("MinSkill", null, "{=tsg_setting_skill_level_min}Min skill level", 1, EMOptionType.IntegerProperty, 1, 300));
			list.Add(new EMOptionItem( "MaxSkill", null, "{=tsg_setting_skill_level_max}Max skill level", 100, EMOptionType.IntegerProperty, 1, 300));

			return list;
		}

		public TheGenerationDataSetting()
		{

			this.EnableGroupCompanion = true;
			this.EnableGroupInfluence = true;
			this.EnableGroupTroop = true;
			this.EnableGroupPlayerFamily = true;

			this.EnableKimdom = true;
			//this.FiefNumbers = 1;
			//this.IsSupportImperial = true;
			this.ClanTier = 1;
			this.ClanRenown = 0;
			this.ResourceGold = 100;
			this.ResourceMeat = 50;
			this.KindomCulture = CulturePairs()[0];
			this.FiefSettlements = new List<EMOptionPair>();
			this.FiefSettlements.Add(FiefPairs()[0]);

			this.PlayerSkill = 100;

			
			this.HeroNumbers = 10;
			this.HeroSkillLevel = 100;
			this.HeroTakeAllPerks = true;
			this.HeroFromTier = 6;
			this.HeroCoupleMaxNumbers = 0;
			this.HeroCoupleChildrenProbability = 0f;

			
			this.Soldiers = new ArrayList();

		
			this.PlayerSpouse = HeroPairs()[0];
			this.PlayerChildren = new ArrayList();

		}

		List<EMOptionPair> _cultureList;
		public List<EMOptionPair> CulturePairs()
		{
			if (null == _cultureList)
			{
				List<EMOptionPair> list = new List<EMOptionPair>();
				foreach (CultureObject current in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
				{
					/*if (current.IsMainCulture)
					{

					}*/
					list.Add(new EMOptionPair(current, current.Name.ToString()));
				}
				_cultureList = list;
			}

			return _cultureList;
		}

		List<EMOptionPair> _heroList;
		public List<EMOptionPair> HeroPairs()
		{
			if (null == _heroList)
			{
				List<EMOptionPair> list = new List<EMOptionPair>();
				IEnumerable<CharacterObject> heros = CharacterObject.All.Where(
					obj => obj.IsHero
                    && obj.Age < 45 && obj.IsFemale && obj.HeroObject.IsActive
                    &&  obj.IsHero && CharacterObject.PlayerCharacter != obj && (obj.Occupation == Occupation.Lord || obj.Occupation == Occupation.Wanderer));
				foreach (CharacterObject current in heros)
				{
					list.Add(new EMOptionPair(current, current.Name.ToString()));
				}
				_heroList = list;
			}

			return _heroList;
		}

		List<EMOptionPair> _fiefList;
		public List<EMOptionPair> FiefPairs()
		{
			if (null == _fiefList)
			{
				List<EMOptionPair> list = new List<EMOptionPair>();
				IEnumerable<Settlement> settlements = Settlement.All.Where(obj => obj.IsTown || obj.IsCastle);
				foreach (Settlement current in settlements)
				{
					list.Add(new EMOptionPair(current, current.Name.ToString()));
				}
				_fiefList = list;
			}

			return _fiefList;
		}

		List<EMOptionPair> _soldierList;
		public List<EMOptionPair> SoldierPairs()
		{
			if (null == _soldierList)
			{
				List<EMOptionPair> list = new List<EMOptionPair>();
				IEnumerable<CharacterObject> soldiers = CharacterObject.All.Where(obj => obj.IsSoldier);
				foreach (CharacterObject current in soldiers)
				{
					list.Add(new EMOptionPair(current, current.Name.ToString()));
				}
				_soldierList = list;
			}

			return _soldierList;
		}

		public class SoldierTemplateData
		{
			public int Number { get; set; }
			public EMOptionPair Soldier { get; set; }

			public SoldierTemplateData()
			{
				Number = 10;
				Soldier = TheGenerationDataSetting.Instance.SoldierPairs()[0];
			}
		}

		public class ChildrenTemplateData{

			public int Number { get; set; }
			public EMOptionPair Mother { get; set; }
			public int Age { get; set; }
			public int MinSkill { get; set; }
			public int MaxSkill { get; set; }
			public bool IsFemale { get; set; }

			public ChildrenTemplateData()
			{
				Number = 1;
				Mother = TheGenerationDataSetting.Instance.HeroPairs()[0];
				Age = 1;
				IsFemale = true;
				MinSkill = 1;
				MaxSkill = 100;
			}
		}

	}
}
