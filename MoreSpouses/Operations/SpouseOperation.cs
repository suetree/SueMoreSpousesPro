using SueMoreSpouses.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueMoreSpouses.Operation
{
    internal class SpouseOperation
	{
		public static void GetPregnancyForHero(Hero father, Hero mother)
		{
			MakePregnantAction.Apply(mother);
			PregnancyCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<PregnancyCampaignBehavior>();
			CampaignTime campaignTime = CampaignTime.DaysFromNow(Campaign.Current.Models.PregnancyModel.PregnancyDurationInDays);

			EXSpouseGetPregnancySetting setting = MoreSpouseSetting.GetInstance().EXSpouseGetPregnancySetting;
			if (setting.Enable)
			{
				campaignTime = CampaignTime.DaysFromNow(setting.ExspouseGetPregnancyDurationInDays);
			}
			Type type = campaignBehavior.GetType();
			FieldInfo field = type.GetField("_heroPregnancies", BindingFlags.Instance | BindingFlags.NonPublic);
			IList list = (IList)field.GetValue(campaignBehavior);
			object[] args = new object[]
			{
				mother,
				father,
				campaignTime
			};
			try
			{
				Type nestedType = type.GetNestedType("Pregnancy", BindingFlags.Instance | BindingFlags.NonPublic);
				object value = Activator.CreateInstance(nestedType, BindingFlags.Instance | BindingFlags.Public, null, args, null);
				list.Add(value);
			}
			catch (IOException ex)
			{
				InformationManager.DisplayMessage(new InformationMessage("MoreSpouses.ChildConceived error:" + ex.Message));
			}
		}

		public static void SetPrimarySpouse(Hero hero)
		{
			bool flag = Hero.MainHero.Spouse != hero;
			if (flag)
			{
				Hero.MainHero.Spouse = hero;
				hero.Spouse = Hero.MainHero;
				SpouseOperation.RemoveRepeatExspouses(Hero.MainHero, Hero.MainHero.Spouse);
				SpouseOperation.RemoveRepeatExspouses(hero, hero.Spouse);
			}
		}

		public static void Divorce(Hero hero)
		{
			bool flag = hero != null;
			if (flag)
			{
				bool flag2 = Hero.MainHero.Spouse == hero;
				if (flag2)
				{
					Hero.MainHero.Spouse = null;
					hero.Spouse = null;
				}
				bool flag3 = Hero.MainHero.ExSpouses.Contains(hero);
				if (flag3)
				{
					SpouseOperation.RemoveRepeatExspouses(Hero.MainHero, hero);
					SpouseOperation.RemoveRepeatExspouses(hero, Hero.MainHero);
				}
			}
		}

		public static void RemoveRepeatExspouses(Hero hero, Hero target)
		{
			bool flag = Hero.MainHero.ExSpouses.Count > 0;
			if (flag)
			{
				FieldInfo field = hero.GetType().GetField("_exSpouses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				FieldInfo field2 = hero.GetType().GetField("ExSpouses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				bool flag2 = null == field || null == field2;
				if (!flag2)
				{
					List<Hero> list = (List<Hero>)field.GetValue(hero);
					MBReadOnlyList<Hero> value = (MBReadOnlyList<Hero>)field2.GetValue(hero);
					list = list.Distinct(new DistinctSpouse<Hero>()).ToList<Hero>();
					bool flag3 = list.Contains(target);
					if (flag3)
					{
						list.Remove(target);
					}
					field.SetValue(hero, list);
					value = new MBReadOnlyList<Hero>(list);
					field2.SetValue(hero, value);
				}
			}
		}
	}
}
