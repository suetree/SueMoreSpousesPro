using System;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SueMoreSpouses.Utils
{
	internal class ConversationUtils
	{
		public static ConversationManager GetConversationManager(CampaignGameStarter campaignGameStarter)
		{
			ConversationManager result = null;
			FieldInfo field = campaignGameStarter.GetType().GetField("_conversationManager", BindingFlags.Instance | BindingFlags.NonPublic);
			object value = field.GetValue(campaignGameStarter);
			bool flag = value != null;
			if (flag)
			{
				result = (ConversationManager)field.GetValue(campaignGameStarter);
			}
			return result;
		}

		public static void ChangeCurrentCharaObject(CampaignGameStarter campaignGameStarter, Hero hero)
		{
			ConversationManager conversationManager = ConversationUtils.GetConversationManager(campaignGameStarter);
			bool flag = conversationManager != null;
			if (flag)
			{
				IAgent listenerAgent = conversationManager.ListenerAgent;
				bool flag2 = listenerAgent.GetType() == typeof(Agent);
				if (flag2)
				{
					Agent agent = (Agent)listenerAgent;
					FieldInfo field = agent.GetType().GetField("_character", BindingFlags.Instance | BindingFlags.NonPublic);
					bool flag3 = null != field;
					if (flag3)
					{
						field.SetValue(agent, hero.CharacterObject);
					}
					HeroFaceUtils.UpdatePlayerCharacterBodyProperties(hero, agent.BodyPropertiesValue, hero.CharacterObject.IsFemale);
					bool flag4 = agent.Name != null;
					if (flag4)
					{
						TextObject name = new TextObject(string.Format("\"{0}\"", agent.Name) + hero.Name.ToString(), null);
						hero.SetName(name);
						FieldInfo field2 = agent.GetType().GetField("_name", BindingFlags.Instance | BindingFlags.NonPublic);
						bool flag5 = null != field2;
						if (flag5)
						{
							field2.SetValue(agent, hero.Name);
						}
					}
				}
			}
		}
	}
}
