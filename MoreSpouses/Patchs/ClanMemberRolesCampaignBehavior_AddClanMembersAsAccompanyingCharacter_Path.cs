using HarmonyLib;
using SandBox.CampaignBehaviors;
using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;

namespace SueMoreSpouses.Patch
{
	[HarmonyPatch(typeof(ClanMemberRolesCampaignBehavior), "AddClanMembersAsAccompanyingCharacter")]
	internal class ClanMemberRolesCampaignBehavior_AddClanMembersAsAccompanyingCharacter_Path
	{
	

		public static bool Prefix(ref ClanMemberRolesCampaignBehavior __instance, Hero member)
		{
			CharacterObject characterObject = member.CharacterObject;
			bool flag = characterObject.IsHero && !characterObject.HeroObject.IsWounded && __instance.IsFollowingPlayer(member);
			if (flag)
			{
				LocationCharacter locationCharacter = LocationCharacter.CreateBodyguardHero(characterObject.HeroObject, MobileParty.MainParty, new LocationCharacter.AddBehaviorsDelegate(SandBoxManager.Instance.AgentBehaviorManager.AddFirstCompanionBehavior));
				PlayerEncounter.LocationEncounter.AddAccompanyingCharacter(locationCharacter, true);
				AccompanyingCharacter accompanyingCharacter = PlayerEncounter.LocationEncounter.GetAccompanyingCharacter(locationCharacter);
				bool flag2 = accompanyingCharacter != null;
				if (flag2)
				{
					accompanyingCharacter.DisallowEntranceToAllLocations();
				    accompanyingCharacter.AllowEntranceToLocations((x) => x == LocationComplex.Current.GetLocationWithId("center") || x == LocationComplex.Current.GetLocationWithId("village_center"));
				}
			}
			return false;
		}
	}
}
