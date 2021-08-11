using HarmonyLib;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace SueMoreSpouses
{
	[HarmonyPatch(typeof(PregnancyCampaignBehavior), "DailyTickHero")]
	public class DailyTickHeroPath
	{
		private static void Postfix(PregnancyCampaignBehavior __instance, Hero hero)
		{
			Type type = __instance.GetType();
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
			bool flag = null == type;
			if (!flag)
			{
				bool flag2 = hero.IsFemale && hero.Age > 18f && Hero.MainHero.ExSpouses.Contains(hero) && !hero.IsPregnant && hero != Hero.MainHero.Spouse;
				if (flag2)
				{
					try
					{
						MethodInfo method = type.GetMethod("RefreshSpouseVisit", bindingAttr);
						object[] parameters = new object[]
						{
							hero
						};
						bool flag3 = null != method;
						if (flag3)
						{
							object obj = method.Invoke(__instance, parameters);
						}
					}
					catch (TargetInvocationException ex)
					{
						InformationManager.DisplayMessage(new InformationMessage("MoreSpouses.DailyTickHero error:" + ex.Message));
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
							InformationManager.DisplayMessage(new InformationMessage("MoreSpouses.DailyTickHero error:" + ex2.Message));
						}
					}
				}
			}
		}
	}
}
