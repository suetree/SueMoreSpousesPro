using SueLordFromFamily.Dialogues;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueLordFromFamily.Behaviors
{
    public class LordFromFamilyBehavior : CampaignBehaviorBase
	{
		public override void RegisterEvents()
		{
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		public override void SyncData(IDataStore dataStore)
		{
		}

		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			new ChaneHeroClanDialogue(campaignGameStarter).GenerateDialogue();
			new CreateClanDialogue(campaignGameStarter).GenerateDialogue();
			InformationManager.DisplayMessage(new InformationMessage("LordFromFamily OnSessionLaunched"));
		}
	}
}
