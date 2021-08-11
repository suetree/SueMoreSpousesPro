using Helpers;
using SueMoreSpouses.Settings;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SueMoreSpouses.Operation
{
    internal class ChildrenGrowthOperation
	{
		public static void FastGrowth(Hero child)
		{
			ChildrenFastGrowthSetting childrenFastGrowthSetting  = MoreSpouseSetting.GetInstance().ChildrenFastGrowthSetting;
			if (childrenFastGrowthSetting.Enable)
			{
				bool flag2 = child == null;
				if (!flag2)
				{
					bool flag3 = child.Age < (float)childrenFastGrowthSetting.ChildrenFastGrowtStopGrowUpAge;
					if (flag3)
					{
						int num = (int)child.Age;
						int childrenFastGrowthCycleInDays = childrenFastGrowthSetting.ChildrenFastGrowthCycleInDays;
						int num2 = (int)CampaignTime.Now.ToDays;
						bool flag4 = num2 % childrenFastGrowthCycleInDays == 0;
						if (flag4)
						{
							CampaignTime birthDay = CampaignTime.Years((float)CampaignTime.Now.ToYears - (float)(num + 1));
							child.SetBirthDay(birthDay);
							bool flag5 = Hero.MainHero.Children.Contains(child);
							if (flag5)
							{
								InformationManager.DisplayMessage(new InformationMessage(child.Name.ToString() + "  AGE =" + child.Age.ToString()));
							}
						}
						int num3 = (int)CampaignTime.Years((float)Campaign.Current.Models.AgeModel.HeroComesOfAge).ToDays;
						num2 = (int)child.BirthDay.ElapsedDaysUntilNow;
						bool flag6 = num2 == num3;
						if (flag6)
						{
							bool flag7 = child == Hero.MainHero || Hero.MainHero.Children.Contains(child) || (Hero.MainHero.Father != null && Hero.MainHero.Father.Children.Contains(child));
							if (flag7)
							{
								TextObject textObject = GameTexts.FindText("suems_children_grow_up_to_hero_age", null);
								StringHelpers.SetCharacterProperties("SUE_HERO", child.CharacterObject, textObject);
								InformationManager.AddQuickInformation(textObject, 0, null, "event:/ui/notification/quest_finished");
							}
						}
					}
				}
			}
		}
	}
}
