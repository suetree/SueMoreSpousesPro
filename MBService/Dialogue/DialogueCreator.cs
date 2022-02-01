using System;
using TaleWorlds.CampaignSystem;

namespace SueMBService.Dialogue
{
	public class DialogueCreator
	{
		public delegate bool ConditionDelegate();

		public delegate void ResultDelegate();

		private string id;

		private string inputOrder;

		private string outOrder;

		private string text;

		private DialogueCreator.ConditionDelegate condition;

		private DialogueCreator.ResultDelegate result;

		private bool isPlayer;

		public DialogueCreator Id(string id)
		{
			this.id = id;
			return this;
		}

		public DialogueCreator IsPlayer(bool isPlayer)
		{
			this.isPlayer = isPlayer;
			return this;
		}

		public DialogueCreator InputOrder(string inputOrder)
		{
			this.inputOrder = inputOrder;
			return this;
		}

		public DialogueCreator OutOrder(string outOrder)
		{
			this.outOrder = outOrder;
			return this;
		}

		public DialogueCreator Text(string text)
		{
			this.text = text;
			return this;
		}

		public DialogueCreator Condition(DialogueCreator.ConditionDelegate condition)
		{
			this.condition = condition;
			return this;
		}

		public DialogueCreator Result(DialogueCreator.ResultDelegate result)
		{
			this.result = result;
			return this;
		}

		public void CreateAndAdd(CampaignGameStarter campaignGameStarter)
		{
			bool flag = this.isPlayer;
			if (flag)
			{
				campaignGameStarter.AddPlayerLine(this.id, this.inputOrder, this.outOrder, this.text, this.NewCondition(this.condition), this.NewResult(this.result), 100, null, null);
			}
			else
			{
				campaignGameStarter.AddDialogLine(this.id, this.inputOrder, this.outOrder, this.text, this.NewCondition(this.condition), this.NewResult(this.result), 100, null);
			}
		}

		public ConversationSentence.OnConsequenceDelegate NewResult(DialogueCreator.ResultDelegate action)
		{
			bool flag = action != null;
			ConversationSentence.OnConsequenceDelegate onConsequenceDelegate;
			if (flag)
			{
				onConsequenceDelegate = new ConversationSentence.OnConsequenceDelegate(action.Invoke);
			}
			else
			{
				onConsequenceDelegate = null;
			}
			return onConsequenceDelegate;
		}

		public ConversationSentence.OnConditionDelegate NewCondition(DialogueCreator.ConditionDelegate action)
		{
			bool flag = action != null;
			ConversationSentence.OnConditionDelegate onConditionDelegate;
			if (flag)
			{
				onConditionDelegate = new ConversationSentence.OnConditionDelegate(action.Invoke);
			}
			else
			{
				onConditionDelegate = null;
			}
			return onConditionDelegate;
		}
	}
}
