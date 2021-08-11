using HarmonyLib;
using SueMoreSpouses.Settings;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.Core;

namespace SueMoreSpouses
{
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
