using SueMBService.API;
using SueMBService.Dialogue;
using SueMBService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using SueLordFromFamily.Actions;
using System.Text;
using TaleWorlds.Localization;

namespace SueLordFromFamily.Dialogues
{
	internal class CreateClanDialogue : AbsCreateDialogue
	{


	public static string FLAG_CLAN_CREATE_CHOICE_SETTLEMENT_ITEM = "sue_clan_create_from_family_choice_settlememt_item";

	public static string FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM = "sue_clan_create_from_family_choice_settlememt_item";

	public static string FLAG_CLAN_CREATE_CHOICE_CLAN_MONEY_TIER_ITEM = "sue_clan_create_from_family_choice_clan_money_tier";

	private ClanCreateAction clanCreateBussniess;

	public CreateClanDialogue(CampaignGameStarter campaignGameStarter) : base(campaignGameStarter)
	{
		this.clanCreateBussniess = new ClanCreateAction();
	}

	public override void GenerateDialogue()
	{
		new DialogueCreator().IsPlayer(true).Id("sue_clan_create_from_family").InputOrder("hero_main_options").OutOrder("sue_clan_create_from_family_request").Text("{=sue_clan_create_from_family_request}I want you to be a vassal of my kindom").Condition(new DialogueCreator.ConditionDelegate(this.CreateClanCondition)).CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family").InputOrder("sue_clan_create_from_family_request").OutOrder("sue_clan_create_from_family_start").Text("{=sue_clan_create_from_family_choice_settlement_tip}Really? Which territory am I going to get?").CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_need_spouse").InputOrder("sue_clan_create_from_family_choice_other").OutOrder("sue_clan_create_from_family_take_spouse").Text("{=sue_clan_create_from_family_need_spouse}Not spouse to you").Condition(new DialogueCreator.ConditionDelegate(this.NeedSpouse)).CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_need_money").InputOrder("sue_clan_create_from_family_choice_other").OutOrder("sue_clan_create_from_family_take_money").Text("{=sue_clan_create_from_family_need_money}I need some money to hire soldiers.").Condition(new DialogueCreator.ConditionDelegate(this.NeedNotSpouse)).Result(new DialogueCreator.ResultDelegate(this.NeedNotSpouseResult)).CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_need_money").InputOrder("sue_clan_create_from_family_request_money").OutOrder("sue_clan_create_from_family_take_money").Text("{=sue_clan_create_from_family_need_money}I need some money to hire soldiers.").CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_request_children").InputOrder("sue_clan_create_from_family_complete_take_money").OutOrder("sue_clan_create_from_family_request_children_out").Condition(new DialogueCreator.ConditionDelegate(this.HasChildren)).Text("{=sue_clan_create_from_family_vassal_request_children}I want our kids to come with us").CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_request_complete_no_children").InputOrder("sue_clan_create_from_family_complete_take_money").OutOrder("sue_clan_create_from_family_complete_2").Text("{=sue_clan_create_from_family_complete}I would go through fire and water for you").Condition(new DialogueCreator.ConditionDelegate(this.HasNoChildren)).Result(new DialogueCreator.ResultDelegate(this.CreateVassal)).CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(true).Id("sue_clan_create_from_family_request_children_result_1").InputOrder("sue_clan_create_from_family_request_children_out").OutOrder("sue_clan_create_from_family_complete").Text("{=sue_clan_create_from_family_vassal_request_children_result_1}Well, as much as I like them, having them with you will make them happier").Result(new DialogueCreator.ResultDelegate(this.TogetherWithThireChildren)).CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(true).Id("sue_clan_create_from_family_request_children_result_2").InputOrder("sue_clan_create_from_family_request_children_out").OutOrder("sue_clan_create_from_family_complete").Text("{=sue_clan_create_from_family_vassal_request_children_result_2}I'm sorry. I can't do this. It's safer for your children to stay with me").CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(true).Id("sue_clan_create_from_family_money_close").InputOrder("sue_clan_create_from_family_request_children_out").OutOrder("close_window").Text("{=sue_clan_create_from_family_of_forget}Or forget").CreateAndAdd(base.CampaignGameStarter);
		new DialogueCreator().IsPlayer(false).Id("sue_clan_create_from_family_request_complete").InputOrder("sue_clan_create_from_family_complete").OutOrder("sue_clan_create_from_family_complete_2").Text("{=sue_clan_create_from_family_complete}I would go through fire and water for you.").Result(new DialogueCreator.ResultDelegate(this.CreateVassal)).CreateAndAdd(base.CampaignGameStarter);
	}

	private bool CreateClanCondition()
	{
		bool flag = Hero.OneToOneConversationHero == null;
		bool result;
		if (flag)
		{
			result = false;
		}
		else
		{
			IEnumerable<Settlement> settlements = Hero.MainHero.Clan.Settlements;
	
			List<Settlement> list = settlements.Where((settlement) => settlement.IsTown || settlement.IsCastle).ToList<Settlement>();
			bool flag2 = list.Count < 1;
			if (flag2)
			{
				result = false;
			}
			else
			{
				Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
				bool flag3 = oneToOneConversationHero != null && oneToOneConversationHero.Clan == Clan.PlayerClan && oneToOneConversationHero.PartyBelongedTo != null && oneToOneConversationHero.PartyBelongedTo.LeaderHero == Hero.MainHero && Hero.MainHero.MapFaction is Kingdom && !Hero.MainHero.ExSpouses.Contains(oneToOneConversationHero) && oneToOneConversationHero != Hero.MainHero.Spouse;
				if (flag3)
				{
					Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
					bool flag4 = ((kingdom != null) ? kingdom.Ruler : null) == Hero.MainHero;
					if (flag4)
					{
						bool flag5 = Hero.MainHero.Clan.Gold >= 50000;
						bool flag6 = flag5;
						if (flag6)
						{
							this.ResetDataForCreateClan();
						}
						result = flag5;
						return result;
					}
				}
				result = false;
			}
		}
		return result;
	}

	private void CreateVassal()
	{
		this.clanCreateBussniess.CreateVassal();
	}

	private void ShowSelectSettlement()
	{
		IEnumerable<Settlement> settlements = Hero.MainHero.Clan.Settlements;
	
		List<Settlement> list = settlements.Where((settlement) => settlement.IsTown || settlement.IsCastle).ToList<Settlement>();
		int num = 10;
		bool flag = list.Count<Settlement>() <= num;
		if (flag)
		{
			list.ForEach(delegate (Settlement settlement)
			{
				this.addPlayerLineToSelectSettlement(settlement);
			});
			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SETTLEMENT_ITEM, "sue_clan_create_from_family_start", "close_window", "{=sue_clan_create_from_family_of_forget}Or forget", null, null, 100, null);
		}
		else
		{
			List<int> canAddIndexs = RandomUtils.RandomNumbers(num, 0, list.Count<Settlement>(), new List<int>());
			int index = 0;
			list.ForEach(delegate (Settlement settlement)
			{
				bool flag2 = canAddIndexs.Contains(index);
				if (flag2)
				{
					this.addPlayerLineToSelectSettlement(settlement);
				}
				index++;
			});
			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SETTLEMENT_ITEM, "sue_clan_create_from_family_start", "sue_clan_create_from_family_take_settlement_change", "{=sue_clan_create_from_family_choice_spouse_item_change}The next group. ", null, delegate
			{
				this.GenerateDataForCreateClan();
			}, 100, null);
			base.CampaignGameStarter.AddDialogLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SETTLEMENT_ITEM, "sue_clan_create_from_family_take_settlement_change", "sue_clan_create_from_family_start", "{=sue_clan_create_from_family_choice_spouse_item_change_tip}Help me choose a nice one.", null, null, 100, null);
			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SETTLEMENT_ITEM, "sue_clan_create_from_family_start", "close_window", "{=sue_clan_create_from_family_of_forget} Or forget", null, null, 100, null);
		}
	}

	private void ShowSelectSpouseList()
	{
		IEnumerable<TroopRosterElement> troopRosters = MobileParty.MainParty.MemberRoster.GetTroopRoster();
		IEnumerable<TroopRosterElement> enumerable = troopRosters.Where((obj) => obj.Character.IsHero && obj.Character.HeroObject.IsPlayerCompanion);
		int num = 10;
		bool flag = enumerable.Count<TroopRosterElement>() <= num;
		if (flag)
		{
			enumerable.ToList<TroopRosterElement>().ForEach(delegate (TroopRosterElement obj)
			{
				Hero heroObject = obj.Character.HeroObject;
				this.addPlayerLineToSelectSpouse(heroObject);
			});
			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM, "sue_clan_create_from_family_take_spouse", "sue_clan_create_from_family_request_money", "{=sue_clan_create_from_family_need_spouse_not}Not spouse to you", null, null, 100, null);
			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM, "sue_clan_create_from_family_take_spouse", "close_window", "{=sue_clan_create_from_family_of_forget}Or forget ", null, null, 100, null);
		}
		else
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			List<CharacterObject> list = new List<CharacterObject>();
			foreach (TroopRosterElement current in enumerable)
			{
				list.Add(current.Character);
			}
			int item = list.IndexOf(oneToOneConversationHero.CharacterObject);
			List<int> canAddIndexs = RandomUtils.RandomNumbers(num, 0, list.Count<CharacterObject>(), new List<int>
				{
					item
				});
			int index = 0;
			list.ForEach(delegate (CharacterObject obj)
			{
				Hero heroObject = obj.HeroObject;
				bool flag2 = canAddIndexs.Contains(index);
				if (flag2)
				{
					this.addPlayerLineToSelectSpouse(heroObject);
				}
				index++;
			});
			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM, "sue_clan_create_from_family_take_spouse", "sue_clan_create_from_family_take_spouse_change", "{=sue_clan_create_from_family_choice_spouse_item_change}The next group. ", null, delegate
			{
				this.GenerateDataForCreateClan();
			}, 100, null);
			base.CampaignGameStarter.AddDialogLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM, "sue_clan_create_from_family_take_spouse_change", "sue_clan_create_from_family_take_spouse", "{=sue_clan_create_from_family_choice_spouse_item_change_tip}Help me choose a nice one.", null, null, 100, null);
			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM, "sue_clan_create_from_family_take_spouse", "sue_clan_create_from_family_request_money", "{=sue_clan_create_from_family_need_spouse_not}Not spouse to you", null, null, 100, null);

			base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM, "sue_clan_create_from_family_take_spouse", "close_window", "{=sue_clan_create_from_family_of_forget}Or forget ", null, null, 100, null);
		}
	}

	private void ShowClanMoneyTierList()
	{
		int num = 6;
		for (int i = 2; i < 7; i++)
		{
			bool flag = this.PlayGetMoneyByTierCondition(i);
			bool flag2 = !flag;
			if (flag2)
			{
				num = i - 1;
				break;
			}
		}
		for (int j = 2; j <= num; j++)
		{
			this.addPlayerLineToSelectClanMoneyTier(j);
		}
		base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_MONEY_TIER_ITEM, "sue_clan_create_from_family_take_money", "close_window", "{=sue_clan_create_from_family_of_forget}Or forget", null, null, 100, null);
	}

	public bool HasNoChildren()
	{
		return !this.HasChildren();
	}

	public bool HasChildren()
	{
		bool result = false;
		Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
		bool flag = oneToOneConversationHero.Children.Count > 0;
		if (flag)
		{
			oneToOneConversationHero.Children.ForEach(delegate (Hero child)
			{
				bool flag2 = child.Clan == Clan.PlayerClan;
				if (flag2)
				{
					result = true;
				}
			});
		}
		return result;
	}

	public void TogetherWithThireChildren()
	{
		this.clanCreateBussniess.IsTogetherWithThireChildren = true;
	}

	private void ResetDataForCreateClan()
	{
		this.clanCreateBussniess.reset();
		this.GenerateDataForCreateClan();
	}

	private bool NeedSpouse()
	{
		return !this.NeedNotSpouse();
	}

	private bool NeedNotSpouse()
	{
		Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
		return oneToOneConversationHero != null && oneToOneConversationHero.Spouse != null && oneToOneConversationHero.Spouse.Clan == Clan.PlayerClan;
	}

	public void NeedNotSpouseResult()
	{
		Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
		bool flag = oneToOneConversationHero != null && oneToOneConversationHero.Spouse != null && oneToOneConversationHero.Spouse != Hero.MainHero && !Hero.MainHero.ExSpouses.Contains(oneToOneConversationHero.Spouse);
		if (flag)
		{
			bool flag2 = oneToOneConversationHero.Spouse.Clan == Clan.PlayerClan;
			if (flag2)
			{
				this.clanCreateBussniess.TargetSpouse = oneToOneConversationHero.Spouse;
			}
		}
	}

	public bool PlayGetMoneyByTierCondition(int tier)
	{
		int num = this.clanCreateBussniess.TakeMoneyByTier(tier);
		return Hero.MainHero.Clan.Gold >= num;
	}

	public void PlayerGetMoneyByTierResult(int tier)
	{
		this.clanCreateBussniess.SelectClanTier = tier;
	}

	private void GenerateDataForCreateClan()
	{
		CleanRepeatableLine(base.CampaignGameStarter, CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SETTLEMENT_ITEM);
		CleanRepeatableLine(base.CampaignGameStarter, CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM);
		CleanRepeatableLine(base.CampaignGameStarter, CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_MONEY_TIER_ITEM);
		this.ShowSelectSettlement();
		this.ShowSelectSpouseList();
		this.ShowClanMoneyTierList();
	}

	private void addPlayerLineToSelectSpouse(Hero spouse)
	{
		string displayName = spouse.Name.ToString();
			if (null != spouse.Spouse) {
				displayName += new StringBuilder("(").Append(new TextObject("{=slff_label_spouse}").ToString()).Append(": ").Append(spouse.Spouse.Name).Append(")");

		 }
		base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SPOUSE_ITEM, "sue_clan_create_from_family_take_spouse", "sue_clan_create_from_family_request_money", displayName, delegate
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != spouse;
		}, delegate
		{
			this.clanCreateBussniess.TargetSpouse = spouse;
		}, 100, null);
	}

	private void addPlayerLineToSelectSettlement(Settlement settlement)
	{
		base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_SETTLEMENT_ITEM, "sue_clan_create_from_family_start", "sue_clan_create_from_family_choice_other", settlement.Name.ToString(), null, delegate
		{
			this.clanCreateBussniess.TargetSettlement = settlement;
		}, 100, null);
	}

	private void addPlayerLineToSelectClanMoneyTier(int tier)
	{
		base.CampaignGameStarter.AddRepeatablePlayerLine(CreateClanDialogue.FLAG_CLAN_CREATE_CHOICE_CLAN_MONEY_TIER_ITEM, "sue_clan_create_from_family_take_money", "sue_clan_create_from_family_complete_take_money", string.Format("{0} GLOD ( Tier = {1} )", this.clanCreateBussniess.TakeMoneyByTier(tier), tier), null, delegate
		{
			this.clanCreateBussniess.SelectClanTier = tier;
		}, 100, null);
	}
}
}
