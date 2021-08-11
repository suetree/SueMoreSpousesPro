using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;

namespace SueLordFromFamily.GauntletUI.States
{
	internal class NewClanBannerEditorState : GameState
	{
		private IBannerEditorStateHandler _handler;

		public CharacterObject EditCharacter
		{
			get;
			set;
		}

		public Clan EditClan
		{
			get;
			set;
		}

		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		public IBannerEditorStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		public NewClanBannerEditorState(CharacterObject character, Clan clan)
		{
			this.EditCharacter = character;
			this.EditClan = clan;
		}

		public Clan GetClan()
		{
			return this.EditClan;
		}

		public CharacterObject GetCharacter()
		{
			return this.EditCharacter;
		}
	}
}
