using SueMBService.Utils;
using SueMoreSpouses.Utils;
using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;

namespace SueMoreSpouses.Patch
{
	internal class OccpationFullPath
	{
		public static void Postfix(ref EncyclopediaHeroPageVM __instance)
		{
			bool flag = ReflectUtils.ReflectField("_hero", __instance) != null;
			if (flag)
			{
				Hero hero = (Hero)ReflectUtils.ReflectField("_hero", __instance);
				string value = CampaignUIHelper.GetHeroOccupationName(hero);
				bool flag2 = string.IsNullOrEmpty(value);
				if (flag2)
				{
					value = Enum.GetName(typeof(Occupation), hero.CharacterObject.Occupation);
					string definition = GameTexts.FindText("str_enc_sf_occupation", null).ToString();
					__instance.Stats.Add(new StringPairItemVM(definition, value, null));
				}
				FieldInfo field = hero.CharacterObject.GetType().GetField("_originCharacter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				bool flag3 = null != field;
				if (flag3)
				{
					object value2 = field.GetValue(hero.CharacterObject);
					bool flag4 = value2 is CharacterObject;
					if (flag4)
					{
						CharacterObject characterObject = (CharacterObject)value2;
						__instance.Stats.Add(new StringPairItemVM("模板: ", characterObject.StringId, null));
					}
				}
			}
		}
	}
}
