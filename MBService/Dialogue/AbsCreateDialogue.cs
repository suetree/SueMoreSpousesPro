using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SueMBService.Dialogue
{
	public abstract class AbsCreateDialogue
    {
		public CampaignGameStarter CampaignGameStarter
		{
			get;
			set;
		}

		public AbsCreateDialogue(CampaignGameStarter campaignGameStarter)
		{
			this.CampaignGameStarter = campaignGameStarter;
		}

		public abstract void GenerateDialogue();

	/*	public string LoactionText(string idStr)
		{
			//return GameTexts.FindText(idStr, null).ToString();
			StringBuilder stringBuilder = new StringBuilder("{=").Append(idStr).Append("}");

			return new TextObject(stringBuilder.ToString()).ToString();
		}*/

		public  void CleanRepeatableLine(CampaignGameStarter campaignGameStarter, string flag)
		{
			FieldInfo field = campaignGameStarter.GetType().GetField("_conversationManager", BindingFlags.Instance | BindingFlags.NonPublic);
			object value = field.GetValue(campaignGameStarter);
			bool flag2 = value != null;
			if (flag2)
			{
				ConversationManager conversationManager = (ConversationManager)field.GetValue(campaignGameStarter);
				FieldInfo field2 = conversationManager.GetType().GetField("_sentences", BindingFlags.Instance | BindingFlags.NonPublic);
				bool flag3 = null != field2;
				if (flag3)
				{
					List<ConversationSentence> list = (List<ConversationSentence>)field2.GetValue(conversationManager);
					list.RemoveAll((ConversationSentence s) => flag == s.Id);
				}
			}
		}
	}
}
