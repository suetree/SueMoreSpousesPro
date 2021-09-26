using HarmonyLib;
using SueEasyMenu.Models;
using SueMBService.Utils;
using SueMoreSpouses.Operation;
using SueMoreSpouses.Settings;
using SueMoreSpouses.Utils;
using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SueMoreSpouses.Patch
{
    internal class AgingCampaignBehaviorPatch
	{
		[HarmonyPatch(typeof(AgingCampaignBehavior), "DailyTickHero")]
		public class AgingCampaignBehaviorDailyTickPatch
		{
			private static void Prefix(Hero hero)
			{
				
					ChildrenFastGrowthSetting setting = MoreSpouseSetting.GetInstance().ChildrenFastGrowthSetting;
                    if (setting.Enable)
                    {
                        bool flag = AgingCampaignBehaviorPatch.AgingCampaignBehaviorDailyTickPatch.IsInScope(hero);
                        if (flag)
                        {
                            bool flag2 = hero.Age < (float)setting.ChildrenFastGrowtStopGrowUpAge;
                            if ( flag2)
                            {
                                ChildrenGrowthOperation.FastGrowth(hero);
                            }
                        }
					}
			}

			private static bool IsInScope(Hero hero)
			{
				bool result = false;
				ChildrenFastGrowthSetting setting = MoreSpouseSetting.GetInstance().ChildrenFastGrowthSetting;
				bool flag = setting.Enable && setting.ChildrenFastGrowUpScope != null;
				if (flag)
				{
					EMOptionPair childrenFastGrowUpScope = setting.ChildrenFastGrowUpScope;
                    if (null != childrenFastGrowUpScope.Value)
                    {
                        int val = Convert.ToInt32(childrenFastGrowUpScope.Value) ;
                        switch (val)
                        {
                            case 0:

                                bool flag2 = hero == Hero.MainHero || Hero.MainHero.Children.Contains(hero) || (Hero.MainHero.Father != null && Hero.MainHero.Father.Children.Contains(hero));
                                if (flag2)
                                {
                                    result = true;
                                }
                                break;

                            case 1:

                                bool flag3 = hero.Clan != null && hero.Clan == Hero.MainHero.Clan;
                                if (flag3)
                                {
                                    result = true;
                                }
                                break;

                            case 2:

                                Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
                                Kingdom kingdom2 = hero.MapFaction as Kingdom;
                                bool flag4 = kingdom != null && kingdom2 != null && kingdom == kingdom2;
                                if (flag4)
                                {
                                    result = true;
                                }
                                break;

                            case 3:
                                result = true;
                                break;
                        }
                    }
                  

                    
				}
				return result;
			}
		}

		public class AgingCampaignBehaviorDailyTick2Patch
		{
			private static void Postfix(ref AgingCampaignBehavior __instance, Hero current)
			{
				bool flag = !current.IsTemplate && current.IsAlive;
				if (flag)
				{
					bool flag2 = (int)current.BirthDay.ElapsedDaysUntilNow == (int)CampaignTime.Years((float)Campaign.Current.Models.AgeModel.HeroComesOfAge).ToDays;
					if (flag2)
					{
						bool flag3 = current.HeroState != Hero.CharacterStates.Active;
						if (flag3)
						{
							CampaignEventDispatcher campaignEventDispatcher = GameComponent.CampaignEventDispatcher();
							bool flag4 = campaignEventDispatcher != null;
							if (flag4)
							{
								ReflectUtils.ReflectMethodAndInvoke("OnHeroComesOfAge", campaignEventDispatcher, new object[]
								{
									current
								});
							}
						}
					}
					else
					{
						bool flag5 = (int)current.BirthDay.ElapsedDaysUntilNow == (int)CampaignTime.Years((float)Campaign.Current.Models.AgeModel.BecomeTeenagerAge).ToDays;
						if (!flag5)
						{
							bool flag6 = (int)current.BirthDay.ElapsedDaysUntilNow == (int)CampaignTime.Years((float)Campaign.Current.Models.AgeModel.BecomeChildAge).ToDays;
							if (flag6)
							{
							}
						}
					}
				}
			}
		}

		[HarmonyPatch(typeof(HeroCreationCampaignBehavior), "DeriveSkillsFromTraits")]
		public class OnHeroComesOfAgePatch
		{
			private static void Postfix(Hero hero, CharacterObject templateCharacter = null)
			{
				ChildrenFastGrowthSetting setting = MoreSpouseSetting.GetInstance().ChildrenFastGrowthSetting;
		    /*	bool childrenSkillFixEnable = MoreSpouseSetting.Instance.SettingData.ChildrenSkillFixEnable;
				if (childrenSkillFixEnable)
				{
					int num = 0;
					foreach (SkillObject current in Skills.All)
					{
						bool flag = hero.GetSkillValue(current) <= 10;
						if (flag)
						{
							num++;
						}
					}
					bool flag2 = num >= Skills.All.Count<SkillObject>() - 3;
					if (flag2)
					{
						AgingCampaignBehaviorPatch.OnHeroComesOfAgePatch.GrowUpForFixSkill(hero);
					}
				}*/
			}

			private static void GrowUpForFixSkill(Hero hero)
			{
				bool flag = hero == Hero.MainHero;
				if (!flag)
				{
					hero.ClearSkills();
					bool isFemale = hero.IsFemale;
					float num;
					float num2;
					if (isFemale)
					{
						num = 0.4f;
						num2 = 0.6f;
					}
					else
					{
						num = 0.6f;
						num2 = 0.4f;
					}
					Hero hero2 = (hero.Father != null) ? hero.Father : hero;
					Hero hero3 = (hero.Mother != null) ? hero.Mother : hero;
					float num3 = (new Random().Next(2) == 1) ? 1.5f : 1f;
					bool flag2 = Hero.MainHero.Children.Contains(hero) || Hero.MainHero.Father.Children.Contains(hero);
					if (flag2)
					{
						int num4 = new Random().Next(5);
						bool flag3 = num4 == 1;
						if (flag3)
						{
							num3 *= 1.5f;
							InformationManager.AddQuickInformation(new TextObject(string.Format("Your children {0} get more power", hero.Name), null), 0, null, "event:/ui/notification/quest_finished");
						}
					}
					foreach (SkillObject current in Skills.All)
					{
						int num5 = (int)((float)hero2.GetSkillValue(current) * num + (float)hero3.GetSkillValue(current) * num2);
						hero.HeroDeveloper.ChangeSkillLevel(current, (int)((float)num5 * num3), false);
						hero.HeroDeveloper.TakeAllPerks(current);
					}
					hero.Level = 0;
					hero.HeroDeveloper.UnspentFocusPoints = 20;
					hero.HeroDeveloper.UnspentAttributePoints = 20;
				}
			}
		}
	}
}
