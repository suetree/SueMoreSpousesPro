using HarmonyLib;
using SueMBService.Events;

using TaleWorlds.CampaignSystem;

namespace SueMBService.Paths
{
	[HarmonyPatch(typeof(CampaignEvents), "OnGameLoaded")]
	class CampaignEventsPath
    {
		public static void Postfix(CampaignGameStarter campaignGameStarter)
		{
			SueEventManager.Instance.OnGameLoadedAfter(campaignGameStarter);
		}
	}


}
