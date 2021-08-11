using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;

namespace SueMoreSpouses
{
	internal class OccuptionChange
	{
		

		public void ChangeToLord(CharacterObject characterObject)
		{
			OccuptionChange.ChangeOccupation(characterObject, Hero.MainHero.CharacterObject);
		}

		public static void ChangeToWanderer(CharacterObject target)
		{
			bool flag = target.Occupation == Occupation.Wanderer;
			if (!flag)
			{
				FieldInfo field = target.GetType().GetField("_originCharacter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				PropertyInfo property = typeof(CharacterObject).GetProperty("Occupation");
				bool flag2 = null != property && null != property.DeclaringType;
				if (flag2)
				{
					property = property.DeclaringType.GetProperty("Occupation");
					bool flag3 = null != property;
					if (flag3)
					{
						property.SetValue(target, Occupation.Wanderer, null);
					}
				}

				List<CharacterObject> list = CharacterObject.Templates.Where((obj) => obj.Occupation == Occupation.Wanderer).ToList<CharacterObject>();
				CharacterObject characterObject = list.OrderBy((c) => Guid.NewGuid()).First<CharacterObject>();
				bool flag4 = null != field;
				if (flag4)
				{
					field.SetValue(target, characterObject);
				}
				else
				{
					FieldInfo field2 = target.GetType().GetField("_originCharacterStringId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					bool flag5 = null != field2;
					if (flag5)
					{
						field2.SetValue(target, characterObject.StringId);
					}
				}
				target.HeroObject.IsNoble = false;
			}
		}

		public static void ChangeOccupationToLord(CharacterObject target)
		{
			bool flag = target.Occupation == Occupation.Lord;
			if (!flag)
			{
				FieldInfo field = target.GetType().GetField("_originCharacter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				PropertyInfo property = typeof(CharacterObject).GetProperty("Occupation");
				bool flag2 = null != property && null != property.DeclaringType;
				if (flag2)
				{
					property = property.DeclaringType.GetProperty("Occupation");
					bool flag3 = null != property;
					if (flag3)
					{
						property.SetValue(target, Occupation.Lord, null);
					}
				}
				bool flag4 = null != field;
				if (flag4)
				{
					field.SetValue(target, CharacterObject.PlayerCharacter);
				}
				else
				{
					FieldInfo field2 = target.GetType().GetField("_originCharacterStringId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					bool flag5 = null != field2;
					if (flag5)
					{
						field2.SetValue(target, CharacterObject.PlayerCharacter.StringId);
					}
				}
			}
		}

		private static void ChangeOccupation(CharacterObject target, CharacterObject origin)
		{
			bool flag = target.Occupation == origin.Occupation;
			if (!flag)
			{
				FieldInfo field = target.GetType().GetField("_originCharacter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				CharacterObject characterObject = (CharacterObject)field.GetValue(target);
				PropertyInfo property = typeof(CharacterObject).GetProperty("Occupation");
				bool flag2 = null != property && null != property.DeclaringType;
				if (flag2)
				{
					property = property.DeclaringType.GetProperty("Occupation");
					bool flag3 = null != property;
					if (flag3)
					{
						property.SetValue(target, origin.Occupation, null);
					}
				}
				field.SetValue(target, origin);
			}
		}

		private string loadSaveData()
		{
			return "";
		}

		private void saveData(string json)
		{
		}
	}
}
