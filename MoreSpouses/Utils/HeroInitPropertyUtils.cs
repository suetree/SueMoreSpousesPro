using SueMoreSpouses.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SueMoreSpouses.Utils
{
    internal class HeroInitPropertyUtils
	{
		public static void ChangeBodyProperties(Hero hero, BodyProperties bodyProperties)
		{
			FieldInfo field = hero.GetType().GetField("StaticBodyProperties", BindingFlags.Instance | BindingFlags.NonPublic);
			bool flag = null != field;
			if (flag)
			{
				field.SetValue(hero, bodyProperties);
			}
		}

		public static void InitTrait(Hero hero)
		{
			Random random = new Random();
			hero.SetTraitLevelInternal(GetTraitObjectByName("Valor"), random.Next(0, 5));
			hero.SetTraitLevelInternal(GetTraitObjectByName("Manager"), random.Next(0, 5));
			hero.SetTraitLevelInternal(GetTraitObjectByName("Calculating"), random.Next(0, 5));
			hero.SetTraitLevelInternal(GetTraitObjectByName("Politician"), random.Next(0, 5));
			hero.SetTraitLevelInternal(GetTraitObjectByName("Commander"), random.Next(0, 5));
			hero.SetTraitLevelInternal(GetTraitObjectByName("HopliteFightingSkills"), random.Next(0, 5));
		}

		private static TraitObject GetTraitObjectByName(string Name)
        {
			return TraitObject.All.First((x) => x.StringId == Name);

		}

		public static void InitAttributeAndFouse(Hero hero)
		{
			int num = 4;
			int num2 = 4;
			hero.HeroDeveloper.UnspentAttributePoints = 10;
			hero.HeroDeveloper.UnspentFocusPoints = 10;
			hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Vigor, num, false);
			hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, num, false);
			hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Cunning, num, false);
			hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, num, false);
			hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Intelligence, num, false);
			hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Social, num, false);
			bool flag = hero.CharacterObject.Occupation == Occupation.TavernWench;
			if (flag)
			{
				hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Vigor, num2, false);
				hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, num2, false);
				hero.HeroDeveloper.AddFocus(DefaultSkills.OneHanded, 5, false);
				hero.HeroDeveloper.AddFocus(DefaultSkills.TwoHanded, 5, false);
				hero.HeroDeveloper.AddFocus(DefaultSkills.Crossbow, 5, false);
				hero.HeroDeveloper.AddFocus(DefaultSkills.Bow, 5, false);
				hero.HeroDeveloper.AddFocus(DefaultSkills.Steward, 5, false);
			}
			else
			{
				bool flag2 = hero.CharacterObject.Occupation == Occupation.Townsfolk;
				if (flag2)
				{
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, num2, false);
					hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Vigor, num2, false);
					hero.HeroDeveloper.AddFocus(DefaultSkills.Riding, 5, false);
					hero.HeroDeveloper.AddFocus(DefaultSkills.Polearm, 5, false);
					hero.HeroDeveloper.AddFocus(DefaultSkills.Throwing, 5, false);
					hero.HeroDeveloper.AddFocus(DefaultSkills.Medicine, 5, false);
					HeroInitPropertyUtils.FillBattleEquipment(hero);
				}
				else
				{
					bool flag3 = hero.CharacterObject.Occupation == Occupation.Villager;
					if (flag3)
					{
						hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, num2, false);
						hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, num2, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Bow, 5, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Riding, 5, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Medicine, 5, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Engineering, 5, false);
					}
					else
					{
						hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Control, num2, false);
						hero.HeroDeveloper.AddAttribute(DefaultCharacterAttributes.Endurance, num2, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Medicine, 5, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Charm, 5, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Roguery, 5, false);
						hero.HeroDeveloper.AddFocus(DefaultSkills.Athletics, 5, false);
					}
				}
			}
		}

		public static void InitHeroSkill(Hero hero)
		{
			int num = 5;
			int num2 = 100;
			Random random = new Random();
			bool flag = MBRandom.RandomFloat < 0.2f;
			if (flag)
			{
				num += 30;
				num2 += 30;
				TextObject expr_2D = hero.Name;
				InformationManager.DisplayMessage(new InformationMessage(((expr_2D != null) ? expr_2D.ToString() : null) + " gain more power"));
			}
			foreach (SkillObject current in Skills.All)
			{
				for (int i = 0; i < random.Next(5); i++)
				{
					random.Next(num, num2);
				}
				int changeAmount = random.Next(num, num2);
				hero.HeroDeveloper.ChangeSkillLevel(current, changeAmount, false);
			}

			NamelessNPCSetting setting = MoreSpouseSetting.GetInstance().NamelessNPCSetting;
			bool nPCCharaObjectSkillAuto = setting.NPCCharaObjectSkillAuto;
			if (nPCCharaObjectSkillAuto)
			{
				foreach (SkillObject current2 in Skills.All)
				{
					hero.HeroDeveloper.TakeAllPerks(current2);
				}
			}
		}

		public static void InitHeroForNPC(Hero hero)
		{
			HeroInitPropertyUtils.InitTrait(hero);
			HeroInitPropertyUtils.InitAttributeAndFouse(hero);
			HeroInitPropertyUtils.FillBattleEquipment(hero);
			HeroInitPropertyUtils.InitHeroSkill(hero);
		}

		public static void FillBattleEquipment(Hero hero)
		{
			Occupation occupation = hero.CharacterObject.Occupation;
			Occupation occupation2 = occupation;
			int num;
			if (occupation2 != Occupation.Villager)
			{
				if (occupation2 != Occupation.Townsfolk)
				{
					if (occupation2 != Occupation.TavernWench)
					{
						num = 3;
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 2;
			}
			NamelessNPCSetting setting = MoreSpouseSetting.GetInstance().NamelessNPCSetting;
			int tier = setting.NPCCharaObjectFromTier;
            // 如果  tier = 0； 下面逻辑也能因为是0级别找不到随机获取
            bool flag = tier > 6;
			if (flag)
			{
				tier = 6;
			}
			Equipment battleEquipment = hero.BattleEquipment;
			List<ItemObject> list = Items.All.ToList<ItemObject>().FindAll((ItemObject obj) => HeroInitPropertyUtils.IsBattleEquipmentItem(obj, hero.Culture, tier));
			ItemObject randomElement = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.HeadArmor, hero.Culture, tier).GetRandomElement<ItemObject>();
			bool flag2 = randomElement != null;
			if (flag2)
			{
				battleEquipment[EquipmentIndex.NumAllWeaponSlots] = new EquipmentElement(randomElement, null);
			}
			ItemObject randomElement2 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.Cape, hero.Culture, tier).GetRandomElement<ItemObject>();
			bool flag3 = randomElement2 != null;
			if (flag3)
			{
				battleEquipment[EquipmentIndex.Cape] = new EquipmentElement(randomElement2, null);
			}
			ItemObject randomElement3 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.BodyArmor, hero.Culture, tier).GetRandomElement<ItemObject>();
			bool flag4 = randomElement3 != null;
			if (flag4)
			{
				battleEquipment[EquipmentIndex.Body] = new EquipmentElement(randomElement3, null);
			}
			ItemObject randomElement4 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.HandArmor, hero.Culture, tier).GetRandomElement<ItemObject>();
			bool flag5 = randomElement4 != null;
			if (flag5)
			{
				battleEquipment[EquipmentIndex.Gloves] = new EquipmentElement(randomElement4, null);
			}
			ItemObject randomElement5 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.LegArmor, hero.Culture, tier).GetRandomElement<ItemObject>();
			bool flag6 = randomElement5 != null;
			if (flag6)
			{
				battleEquipment[EquipmentIndex.Leg] = new EquipmentElement(randomElement5, null);
			}
			ItemObject randomElement6 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.Horse, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement7 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.HorseHarness, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement8 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.OneHandedWeapon, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement9 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.TwoHandedWeapon, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement10 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.Shield, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement11 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.Thrown, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement12 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.Polearm, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement13 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.Bow, hero.Culture, tier).GetRandomElement<ItemObject>();
			ItemObject randomElement14 = HeroInitPropertyUtils.GetItemObject(ItemObject.ItemTypeEnum.Arrows, hero.Culture, tier).GetRandomElement<ItemObject>();
			bool flag7 = num == 0;
			if (flag7)
			{
				bool flag8 = randomElement8 != null;
				if (flag8)
				{
					battleEquipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(randomElement8, null);
				}
				bool flag9 = randomElement10 != null;
				if (flag9)
				{
					battleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(randomElement10, null);
				}
				bool flag10 = randomElement9 != null;
				if (flag10)
				{
					battleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(randomElement9, null);
				}
				bool flag11 = randomElement11 != null;
				if (flag11)
				{
					battleEquipment[EquipmentIndex.Weapon3] = new EquipmentElement(randomElement11, null);
				}
			}
			else
			{
				bool flag12 = num == 1;
				if (flag12)
				{
					bool flag13 = randomElement8 != null;
					if (flag13)
					{
						battleEquipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(randomElement8, null);
					}
					bool flag14 = randomElement10 != null;
					if (flag14)
					{
						battleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(randomElement10, null);
					}
					bool flag15 = randomElement12 != null;
					if (flag15)
					{
						battleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(randomElement12, null);
					}
					bool flag16 = randomElement11 != null;
					if (flag16)
					{
						battleEquipment[EquipmentIndex.Weapon3] = new EquipmentElement(randomElement11, null);
					}
					bool flag17 = randomElement6 != null;
					if (flag17)
					{
						battleEquipment[EquipmentIndex.ArmorItemEndSlot] = new EquipmentElement(randomElement6, null);
					}
					bool flag18 = randomElement7 != null;
					if (flag18)
					{
						battleEquipment[EquipmentIndex.HorseHarness] = new EquipmentElement(randomElement7, null);
					}
				}
				else
				{
					bool flag19 = num == 2;
					if (flag19)
					{
						bool flag20 = randomElement9 != null;
						if (flag20)
						{
							battleEquipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(randomElement9, null);
						}
						bool flag21 = randomElement13 != null;
						if (flag21)
						{
							battleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(randomElement13, null);
						}
						bool flag22 = randomElement14 != null;
						if (flag22)
						{
							battleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(randomElement14, null);
						}
						bool flag23 = randomElement14 != null;
						if (flag23)
						{
							battleEquipment[EquipmentIndex.Weapon3] = new EquipmentElement(randomElement14, null);
						}
					}
					else
					{
						bool flag24 = randomElement8 != null;
						if (flag24)
						{
							battleEquipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(randomElement8, null);
						}
						bool flag25 = randomElement10 != null;
						if (flag25)
						{
							battleEquipment[EquipmentIndex.Weapon1] = new EquipmentElement(randomElement10, null);
						}
						bool flag26 = randomElement13 != null;
						if (flag26)
						{
							battleEquipment[EquipmentIndex.Weapon2] = new EquipmentElement(randomElement13, null);
						}
						bool flag27 = randomElement14 != null;
						if (flag27)
						{
							battleEquipment[EquipmentIndex.Weapon3] = new EquipmentElement(randomElement14, null);
						}
					}
				}
			}
		}

		private static List<ItemObject> GetItemObject(ItemObject.ItemTypeEnum typeEnum, BasicCultureObject culture, int tier)
		{
			List<ItemObject> list =  Items.All.ToList<ItemObject>().FindAll((ItemObject obj) => obj.Culture == culture && typeEnum == obj.ItemType && HeroInitPropertyUtils.GetTierByObjectItemItemTiers(obj.Tier) == tier);
			bool flag = list.Count == 0;
			if (flag)
			{
				list =  Items.All.ToList<ItemObject>().FindAll((ItemObject obj) => obj.Culture == culture && typeEnum == obj.ItemType);
			}
			bool flag2 = list.Count == 0;
			if (flag2)
			{
				list =  Items.All.ToList<ItemObject>().FindAll((ItemObject obj) => typeEnum == obj.ItemType && HeroInitPropertyUtils.GetTierByObjectItemItemTiers(obj.Tier) == tier);
			}
			bool flag3 = list.Count == 0;
			if (flag3)
			{
				list =  Items.All.ToList<ItemObject>().FindAll((ItemObject obj) => typeEnum == obj.ItemType);
			}
			return list;
		}

		private static bool IsBattleEquipmentItem(ItemObject item, BasicCultureObject culture, int tier)
		{
			return item.Culture == culture && HeroInitPropertyUtils.GetTierByObjectItemItemTiers(item.Tier) == tier;
		}

		private static int GetTierByObjectItemItemTiers(ItemObject.ItemTiers tier)
		{
			int result;
			switch (tier)
			{
			case ItemObject.ItemTiers.Tier1:
				result = 1;
				break;
			case ItemObject.ItemTiers.Tier2:
				result = 2;
				break;
			case ItemObject.ItemTiers.Tier3:
				result = 3;
				break;
			case ItemObject.ItemTiers.Tier4:
				result = 4;
				break;
			case ItemObject.ItemTiers.Tier5:
				result = 5;
				break;
			case ItemObject.ItemTiers.Tier6:
				result = 6;
				break;
			default:
				result = 6;
				break;
			}
			return result;
		}
	}
}
