using HarmonyLib;
using SueMBService.Utils;
using SueMoreSpouses.Settings;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.Core;

namespace SueMoreSpouses.Patch.ExSpousesPregnancy
{
	[HarmonyPatch(typeof(PregnancyCampaignBehavior), "DailyTickHero")]
	public class DailyTickHeroPath
	{
		private static void Postfix(PregnancyCampaignBehavior __instance, Hero hero)
		{

            bool condition = ReflectUtils.ReflectMethodAndInvoke<bool>("HeroPregnancyCheckCondition", __instance, new object[] { hero });
            if (!condition) return;

            Type type = __instance.GetType();
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			bool flag = null == type;
			if (!flag)
			{
				bool flag2 = hero.IsFemale && hero.Age > 18f  &&  Hero.MainHero.ExSpouses.Contains(hero) && !hero.IsPregnant && hero != Hero.MainHero.Spouse;
				if (flag2)
				{
					try
					{
						MethodInfo method = type.GetMethod("RefreshSpouseVisit", bindingAttr);
						object[] parameters = new object[]
						{
							hero
						};
						if (null != method)
						{
							object obj = method.Invoke(__instance, parameters);
						}
					}
					catch (TargetInvocationException ex)
					{
						InformationManager.DisplayMessage(new InformationMessage("MoreSpouses.DailyTickHero 1 error:" + ex.Message));
					}
				}
				else
				{
					bool flag4 = !hero.IsFemale && hero.IsPregnant && Hero.MainHero.ExSpouses.Contains(hero);
					if (flag4)
					{
						try
						{
							MethodInfo[] methods = type.GetMethods(bindingAttr);
							object[] parameters2 = new object[]
							{
								hero
							};
							MethodInfo[] array = methods;
							for (int i = 0; i < array.Length; i++)
							{
								MethodInfo methodInfo = array[i];
								bool flag5 = methodInfo.Name.Equals("CheckOffspringsToDeliver", StringComparison.Ordinal);
								if (flag5)
								{
									ParameterInfo[] parameters3 = methodInfo.GetParameters();
									bool flag6 = parameters3.Length == 1;
									if (flag6)
									{
										Type parameterType = parameters3[0].ParameterType;
										Type type2 = hero.GetType();
										bool flag7 = parameterType.Equals(type2);
										if (flag7)
										{
											methodInfo.Invoke(__instance, parameters2);
										}
									}
								}
							}
						}
						catch (TargetInvocationException ex2)
						{
							InformationManager.DisplayMessage(new InformationMessage("MoreSpouses.DailyTickHero 2 error:" + ex2.Message));
						}
					}
				}
			}
		}
	}

    [HarmonyPatch(typeof(PregnancyCampaignBehavior), "CheckAreNearby")]
    public class CheckAreNearbyPath
    {
        private static void Prefix(Hero hero, out Hero spouse)
        {
            bool flag = hero.IsFemale && hero.Age > 18f && Hero.MainHero.ExSpouses.Contains(hero) && !hero.IsPregnant && hero != Hero.MainHero.Spouse;
            if (flag)
            {
                spouse = Hero.MainHero;
            }
            else
            {
                spouse = hero.Spouse;
            }
        }
    }

    [HarmonyPatch(typeof(DefaultPregnancyModel), "GetDailyChanceOfPregnancyForHero")]
    public class GetDailyChanceOfPregnancyForHero
    {
        private static bool Prefix(ref float __result, Hero hero)
        {
            EXSpouseGetPregnancySetting spouseGetPregnancySetting = MoreSpouseSetting.GetInstance().EXSpouseGetPregnancySetting;
            if (spouseGetPregnancySetting.Enable)
            {
                bool flag = hero.Clan == Clan.PlayerClan && hero != Hero.MainHero && (Hero.MainHero.ExSpouses.Contains(hero) || hero == Hero.MainHero.Spouse);
                if (flag)
                {
                    float exspouseGetPregnancyDailyChance = spouseGetPregnancySetting.ExspouseGetPregnancyDailyChance;
                    __result = exspouseGetPregnancyDailyChance;

                    return false;
                }
            }
            return true;
        }
    }


    [HarmonyPatch(typeof(PregnancyCampaignBehavior), "ChildConceived")]
    public class ChildConceivedPath
    {
        private static bool Prefix(PregnancyCampaignBehavior __instance, Hero mother)
        {
            bool flag = mother.IsFemale && mother.Age > 18f && (Hero.MainHero.ExSpouses.Contains(mother) || mother == Hero.MainHero.Spouse);
            bool result;
            if (flag)
            {
                EXSpouseGetPregnancySetting getPregnancySetting = MoreSpouseSetting.GetInstance().EXSpouseGetPregnancySetting;
                CampaignTime campaignTime = CampaignTime.DaysFromNow(Campaign.Current.Models.PregnancyModel.PregnancyDurationInDays);
                if (getPregnancySetting.Enable)
                {
                    campaignTime = CampaignTime.DaysFromNow(getPregnancySetting.ExspouseGetPregnancyDurationInDays);
                }
                Type type = __instance.GetType();
                FieldInfo field = type.GetField("_heroPregnancies", BindingFlags.Instance | BindingFlags.NonPublic);
                IList list = (IList)field.GetValue(__instance);
                object[] args = new object[]
                {
                    mother,
                    Hero.MainHero,
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
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
}
