
using SueMBService.API;
using SueMBService.Dialogue;
using SueMBService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueLordFromFamily.Dialogues
{
	public class ChaneHeroClanDialogue : AbsCreateDialogue
	{

	public static string FLAG_CLAN_CREATE_CHOICE_CLAN_ITEM = "sue_clan_create_from_family_choice_clan_item";

	private Clan targetChangeClan;

	public ChaneHeroClanDialogue(CampaignGameStarter campaignGameStarter) : base(campaignGameStarter)
	{
	}

	public override void GenerateDialogue()
	{
		new DialogueCreator().IsPlayer(true).Id("sue_clan_create_from_family_change_clan").InputOrder("hero_main_options").OutOrder("sue_clan_create_from_family_change_clan_request").Text("{=sue_clan_create_from_family_change_clan_request}I want you to work for another clan").Condition(new DialogueCreator.ConditionDelegate(this.ChangeClanCondition)).CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_change_clan_answer").InputOrder("sue_clan_create_from_family_change_clan_request").OutOrder("sue_clan_create_from_family_change_clan_answer_select").Text("{=sue_clan_create_from_family_change_clan_answer}Yes, i do. Please tell me which clan I'm going to").CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_change_clan_complete").InputOrder("sue_clan_create_from_family_change_clan_answer_select_result").OutOrder("sue_clan_create_from_family_complete_2").Text("{=sue_clan_create_from_family_complete}I would go through fire and water for you.").Result(new DialogueCreator.ResultDelegate(this.ChangeHeroToOtherClan)).CreateAndAdd(base.CampaignGameStarter);
	}

	private void GenerateDialogueForSelectClan()
	{
		CleanRepeatableLine(base.CampaignGameStarter, ChaneHeroClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_ITEM);
		Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
		IEnumerable<Clan> clans = kingdom.Clans;

		List<Clan> list = clans.Where((clan) => clan != Clan.PlayerClan).ToList<Clan>();
		int num = 10;
		bool flag = list.Count<Clan>() <= num;
		if (flag)
		{
			list.ForEach(delegate (Clan clan)
			{
				this.addPlayerLineToSelectClan(clan);
			});
			base.CampaignGameStarter.AddPlayerLine(ChaneHeroClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_ITEM, "sue_clan_create_from_family_change_clan_answer_select", "close_window", "{=sue_clan_create_from_family_of_forget}Or forget", null, null, 100, null);
		}
		else
		{
			List<int> canAddIndexs = RandomUtils.RandomNumbers(num, 0, list.Count<Clan>(), new List<int>());
			int index = 0;
			list.ForEach(delegate (Clan clan)
			{
				bool flag2 = canAddIndexs.Contains(index);
				if (flag2)
				{
					this.addPlayerLineToSelectClan(clan);
				}
				index++;
			});
			base.CampaignGameStarter.AddPlayerLine(ChaneHeroClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_ITEM, "sue_clan_create_from_family_change_clan_answer_select", "sue_clan_create_from_family_take_clan_change", "{=sue_clan_create_from_family_choice_spouse_item_change}The next group.", null, delegate
			{
				this.GenerateDialogueForSelectClan();
			}, 100, null);
			base.CampaignGameStarter.AddDialogLine(ChaneHeroClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_ITEM, "sue_clan_create_from_family_take_clan_change", "sue_clan_create_from_family_change_clan_answer_select", "{=sue_clan_create_from_family_choice_spouse_item_change_tip}Help me choose a nice one.", null, null, 100, null);
			base.CampaignGameStarter.AddPlayerLine(ChaneHeroClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_ITEM, "sue_clan_create_from_family_change_clan_answer_select", "close_window", "{=sue_clan_create_from_family_of_forget}Or forget", null, null, 100, null);
		}
	}

	private void addPlayerLineToSelectClan(Clan clan)
	{
		IEnumerable<Hero> heros = clan.Heroes;
		int num = heros.Select((obj) => (int)obj.Age >= Campaign.Current.Models.AgeModel.HeroComesOfAge).Count<bool>();
		string text = clan.Name.ToString() + string.Format("   ( Hero Count = {0};  Clan Tier = {1} )", num, clan.Tier);
		base.CampaignGameStarter.AddPlayerLine(ChaneHeroClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_ITEM, "sue_clan_create_from_family_change_clan_answer_select", "sue_clan_create_from_family_change_clan_answer_select_result", text, null, delegate
		{
			this.targetChangeClan = clan;
		}, 100, null);
	}

	private bool ChangeClanCondition()
	{
		Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
		bool flag = oneToOneConversationHero == null;
		bool result;
		if (flag)
		{
			result = false;
		}
		else
		{
			bool flag2 = false;
			bool flag3 = oneToOneConversationHero.Clan == Clan.PlayerClan;
			if (flag3)
			{
				Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
				bool flag4 = ((kingdom != null) ? kingdom.Leader : null) == Hero.MainHero && kingdom.Clans.Count > 1;
				if (flag4)
				{
					this.ResetDataForChangeClan();
					flag2 = true;
				}
			}
			result = flag2;
		}
		return result;
	}

	private void ResetDataForChangeClan()
	{
		this.targetChangeClan = null;
		this.GenerateDialogueForSelectClan();
	}

	private void ChangeHeroToOtherClan()
	{
		Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
		bool flag = this.targetChangeClan != null;
		if (flag)
		{
				ClanLordService.NewClanAllocateForHero(oneToOneConversationHero, this.targetChangeClan);
		}
		else
		{
			InformationManager.DisplayMessage(new InformationMessage("LordFromFamily error occurred, cant change the hero to other clan!"));
		}
	}
}
}
