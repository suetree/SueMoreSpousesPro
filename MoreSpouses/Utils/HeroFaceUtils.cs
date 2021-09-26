using SueMBService.Utils;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SueMoreSpouses.Utils
{
	internal class HeroFaceUtils
	{
		public static void UpdatePlayerCharacterBodyProperties(Hero hero, BodyProperties properties, bool isFemale)
		{
			ReflectUtils.ReflectPropertyAndSetValue("StaticBodyProperties", properties.StaticProperties, hero);
			hero.Weight = properties.Weight;
			hero.Build = properties.Build;
			hero.UpdatePlayerGender(isFemale);
		}
	}
}
