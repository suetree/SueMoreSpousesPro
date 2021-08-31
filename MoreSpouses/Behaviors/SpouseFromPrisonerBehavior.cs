using System;
using System.Runtime.CompilerServices;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SueMoreSpouses.Behaviors
{
    internal class SpouseFromPrisonerBehavior : CampaignBehaviorBase
    {
        public delegate bool ConditionDelegate();

        public delegate void ResultDelegate();


        private CampaignGameStarter currStarter;

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            this.currStarter = starter;
            InformationManager.DisplayMessage(new InformationMessage("MoreSpouses OnSessionLaunched"));
            starter.AddPlayerLine("conversation_prisoner_chat_start", "start", "close_window", "{=sue_more_spouses_female_spouses_status_prisoner_become_active}Change you to active",
                new ConversationSentence.OnConditionDelegate(this.isSpouseAndPrisoner), new ConversationSentence.OnConsequenceDelegate(this.changeSpousePrisonerStatusToActive), 100, null, null);
            starter.AddPlayerLine("conversation_prisoner_chat_player", "hero_main_options", "sue_more_spouses_companion_become_spouse", "{=sue_more_spouses_companion_become_spouse}I'm interested in you, become my spouse", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsPlayerCompanionAndCanBecomeSpouse)), null, 100, null, null);
            string arg_D1_1 = "sue_more_spouses_companion_choice_result";
            string arg_D1_2 = "sue_more_spouses_companion_become_spouse";
            string arg_D1_3 = "sue_more_spouses_companion_become_spouse_accept";
            string arg_D1_4 = "{=sue_more_spouses_companion_become_spouse_accept}Me too";
            starter.AddDialogLine(arg_D1_1, arg_D1_2, arg_D1_3, arg_D1_4, null, this.Result(() => {
                Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
                HeroRlationOperation.ChangeCompanionToSpouse(oneToOneConversationHero);
            }), 100, null);
            starter.AddPlayerLine("sms_tavernmaid_chat_player", "cprostitute_talk_00_response", "sue_more_spouses_tavernmaid_start", "{=sue_more_spouses_npc_becomes_spouse}I have a business for you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), null, 100, null, null);
            starter.AddPlayerLine("sms_tavernmaid_chat_player", "cprostitute_talk_02_response", "sue_more_spouses_tavernmaid_start", "{=sue_more_spouses_npc_becomes_spouse}I have a business for you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), null, 100, null, null);
            starter.AddPlayerLine("sms_tavernmaid_chat_player", "cprostitute_talk_01_response", "sue_more_spouses_tavernmaid_start", "{=sue_more_spouses_npc_becomes_spouse}I have a business for you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), null, 100, null, null);
            starter.AddPlayerLine("sms_tavernmaid_chat_player", "tcustomer_00", "sue_more_spouses_tavernmaid_start", "{=sue_more_spouses_npc_becomes_spouse}I have a business for you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), null, 100, null, null);
            starter.AddPlayerLine("sms_tavernmaid_chat_player", "ccustomer_00", "sue_more_spouses_tavernmaid_start", "{=sue_more_spouses_npc_becomes_spouse}I have a business for you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), null, 100, null, null);
            starter.AddPlayerLine("sms_tavernmaid_chat_player", "town_or_village_player", "sue_more_spouses_tavernmaid_start", "{=sue_more_spouses_npc_becomes_spouse}I have a business for you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), null, 100, null, null);
            starter.AddPlayerLine("sms_tavernmaid_chat_player", "tavernmaid_talk", "sue_more_spouses_tavernmaid_start", "{=sue_more_spouses_npc_becomes_spouse}I have a business for you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), null, 100, null, null);
            starter.AddDialogLine("sms_tavernmaid_ask_what", "sue_more_spouses_tavernmaid_start", "sms_tavernmaid_ask_what", "{=sms_tavernmaid_ask_what}Isn't it? Speaking to see", null, null, 100, null);
            starter.AddPlayerLine("sms_tavernmaid_ask_what_result", "sms_tavernmaid_ask_what", "sms_tavernmaid_accept_result", "{=sue_more_spouses_prisoner_punish_lord_become_spouse}Give up everything you have to be my spouse", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), this.Result(delegate
            {
                CharacterObject oneToOneConversationCharacter = CharacterObject.OneToOneConversationCharacter;
                bool flag = oneToOneConversationCharacter != null;
                if (flag)
                {
                    HeroRlationOperation.NPCToSouse(oneToOneConversationCharacter, this.currStarter);
                }
            }), 100, null, null);
            starter.AddPlayerLine("sms_tavernmaid_ask_what_result", "sms_tavernmaid_ask_what", "sms_tavernmaid_accept_result", "{=sue_more_spouses_prisoner_punish_lord_become_wanderer_companion}Give up everything you have and become one of my clan", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsNormalNPC)), this.Result(delegate
            {
                CharacterObject oneToOneConversationCharacter = CharacterObject.OneToOneConversationCharacter;
                bool flag = oneToOneConversationCharacter != null;
                if (flag)
                {
                    HeroRlationOperation.NPCToCompanion(oneToOneConversationCharacter, this.currStarter);
                }
            }), 100, null, null);
            starter.AddRepeatablePlayerLine("sms_tavernmaid_ask_what_result", "sms_tavernmaid_ask_what", "close_window", "{=sue_more_spouses_prisoner_punish_cancel}It is a joke", null, null, 100, null);
            starter.AddDialogLine("sms_tavernmaid_accept_result", "sms_tavernmaid_accept_result", "sue_more_spouses_companion_become_spouse_accept", "{=sms_tavernmaid_accept_result}Thank you very much for the beginning of my wonderful life", null, null, 100, null);
            starter.AddPlayerLine("conversation_prisoner_chat_player", "prisoner_recruit_start_player", "sue_more_spouses_prisoner_punish_start", "{=sue_more_spouses_prisoner_punish_start}I will punish you", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsPrisioner)), null, 100, null, null);
            starter.AddDialogLine("sue_more_spouses_prisoner_beg_for_mercy", "sue_more_spouses_prisoner_punish_start", "sue_more_spouses_prisoner_beg_for_mercy", "{=sue_more_spouses_prisoner_beg_for_mercy}Forgive me, my Lord, for my sins", null, null, 100, null);
            starter.AddPlayerLine("sue_more_spouses_prisoner_punish_lord_become_spouse", "sue_more_spouses_prisoner_beg_for_mercy", "sue_more_spouses_prisoner_punish_result", "{=sue_more_spouses_prisoner_punish_lord_become_spouse}Give up everything you have to be my spouse", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsLord)), this.Result(delegate
            {
                this.ChangePrisonerLordToSpouse();
            }), 100, null, null);
            starter.AddPlayerLine("sue_more_spouses_prisoner_punish_lord_become_wanderer_companion", "sue_more_spouses_prisoner_beg_for_mercy", "sue_more_spouses_prisoner_punish_result", "{=sue_more_spouses_prisoner_punish_lord_become_wanderer_companion}Give up everything you have and become one of my clan", this.Condition(new SpouseFromPrisonerBehavior.ConditionDelegate(this.IsLord)), this.Result(delegate
            {
                this.ChangePrisonerLordToFamily();
            }), 100, null, null);
            starter.AddRepeatablePlayerLine("sue_more_spouses_prisoner_punish_cancel", "sue_more_spouses_prisoner_beg_for_mercy", "close_window", "{=sue_more_spouses_prisoner_punish_cancel}It is a joke", null, null, 100, null);
            starter.AddDialogLine("sue_more_spouses_prisoner_punish_result", "sue_more_spouses_prisoner_punish_result", "sue_more_spouses_companion_become_spouse_accept", "{=sue_more_spouses_prisoner_punish_accept}Thank you for your forgiveness", null, null, 100, null);

            ///官方已经关闭了。俘虏招募，这里自己加上俘虏对话
            starter.AddDialogLine("conversation_prisoner_chat_start", "start", "prisoner_recruit_start_player", "{=k7ebznzr}Yes?", new ConversationSentence.OnConditionDelegate(this.conversation_prisoner_chat_start_on_condition), null, 100, null);
            starter.AddRepeatablePlayerLine("sue_more_spouses_prisoner_punish_cancel", "prisoner_recruit_start_player", "close_window", "{=sue_more_spouses_prisoner_punish_cancel}It is a joke", null, null, 100, null);

        }

        private bool conversation_prisoner_chat_start_on_condition()
        {
            bool flag = (CharacterObject.OneToOneConversationCharacter.IsHero && (Hero.OneToOneConversationHero.PartyBelongedTo == null || !Hero.OneToOneConversationHero.PartyBelongedTo.IsActive)) || (CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.PrisonGuard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.Guard && CharacterObject.OneToOneConversationCharacter.Occupation != Occupation.CaravanGuard && MobileParty.ConversationParty != null && MobileParty.ConversationParty.IsMainParty);
            bool b = MobileParty.MainParty.PrisonRoster.Contains(CharacterObject.OneToOneConversationCharacter) & flag;
            return b;
        }

        public void changeSpousePrisonerStatusToActive()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			bool flag = oneToOneConversationHero != null;
			if (flag)
			{
				oneToOneConversationHero.ChangeState(Hero.CharacterStates.Active);
			}
		}

		private void ChangePrisonerLordToSpouse()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			bool flag = oneToOneConversationHero != null;
			if (flag)
			{
				HeroRlationOperation.ChangePrisonerLordToSpouse(oneToOneConversationHero);
			}
		}

		private void ChangePrisonerLordToFamily()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			bool flag = oneToOneConversationHero != null;
			if (flag)
			{
				HeroRlationOperation.ChangePrisonerLordToFamily(oneToOneConversationHero);
			}
		}

	/*	private string LoactionText(string idStr)
		{
			//return GameTexts.FindText(idStr, null).ToString();
			StringBuilder builder = new StringBuilder().Append("{=").Append(idStr).Append("}");
			return new TextObject(builder.ToString()).ToString();
		}*/

		public bool isSpouseAndPrisoner()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && oneToOneConversationHero.IsPrisoner && (oneToOneConversationHero == Hero.MainHero.Spouse || Hero.MainHero.ExSpouses.Contains(oneToOneConversationHero));
		}

		public bool CanBecomeSpouse()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && oneToOneConversationHero.Spouse == null && !Hero.MainHero.ExSpouses.Contains(oneToOneConversationHero);
		}

		public bool IsPrisioner()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && MobileParty.MainParty.PrisonRoster.Contains(oneToOneConversationHero.CharacterObject);
		}

		public bool IsNormalNPCLord()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && oneToOneConversationHero.CharacterObject.Occupation != Occupation.Lord;
		}

		public bool IsLord()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && oneToOneConversationHero.CharacterObject.Occupation == Occupation.Lord;
		}

		public bool IsNotLord()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null;
		}

		public bool IsPlayerCompanion()
		{
			Hero oneToOneConversationHero = Hero.OneToOneConversationHero;
			return oneToOneConversationHero != null && oneToOneConversationHero.IsPlayerCompanion;
		}

		public bool IsNormalNPC()
		{
			bool flag = false;
			CharacterObject oneToOneConversationCharacter = CharacterObject.OneToOneConversationCharacter;
			bool flag2 = oneToOneConversationCharacter != null;
			if (flag2)
			{
				flag = true;
			}
			return (!this.IsPlayerCompanionAndCanBecomeSpouse() && !this.isSpouseAndPrisoner()) & flag;
		}

		public bool IsPlayerCompanionAndCanBecomeSpouse()
		{
			return this.CanBecomeSpouse() && this.IsPlayerCompanion();
		}

		public ConversationSentence.OnConsequenceDelegate Result(SpouseFromPrisonerBehavior.ResultDelegate action)
		{
			return new ConversationSentence.OnConsequenceDelegate(action.Invoke);
		}

		public ConversationSentence.OnConditionDelegate Condition(SpouseFromPrisonerBehavior.ConditionDelegate action)
		{
			return new ConversationSentence.OnConditionDelegate(action.Invoke);
		}
	}
}
