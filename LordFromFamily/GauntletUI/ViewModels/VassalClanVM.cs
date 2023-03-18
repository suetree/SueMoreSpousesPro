using SueLordFromFamily.GauntletUI.States;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueLordFromFamily.GauntletUI.ViewModels
{
	internal class VassalClanVM : ViewModel
	{
		private readonly Action<VassalClanVM> _onSelect;

		public readonly Clan Clan;

		private string _name;

		private ImageIdentifierVM _visual;

		private ImageIdentifierVM _banner;

		private ImageIdentifierVM _banner_9;

		private MBBindingList<HeroVM> _members;

		private MBBindingList<KingdomClanFiefItemVM> _fiefs;

		private int _influence;

		private int _numOfMembers;

		private int _numOfFiefs;

		private string _tierText;

		private int _clanType = -1;

		[DataSourceProperty]
		public string EditVassalBannerText
		{
			get
			{
				return new TextObject("{=sue_clan_create_from_family_edit_banner}Edit Banner", null).ToString();
			}
		}

		[DataSourceProperty]
		public string EditVassalNameText
		{
			get
			{
				return new TextObject("{=sue_clan_create_from_family_edit_name}Edit Name", null).ToString();
			}
		}

		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				bool flag = value != this._name;
				if (flag)
				{
					this._name = value;
					base.OnPropertyChanged("Name");
				}
			}
		}

		[DataSourceProperty]
		public int ClanType
		{
			get
			{
				return this._clanType;
			}
			set
			{
				bool flag = value != this._clanType;
				if (flag)
				{
					this._clanType = value;
					base.OnPropertyChanged("ClanType");
				}
			}
		}

		[DataSourceProperty]
		public int NumOfMembers
		{
			get
			{
				return this._numOfMembers;
			}
			set
			{
				bool flag = value != this._numOfMembers;
				if (flag)
				{
					this._numOfMembers = value;
					base.OnPropertyChanged("NumOfMembers");
				}
			}
		}

		[DataSourceProperty]
		public int NumOfFiefs
		{
			get
			{
				return this._numOfFiefs;
			}
			set
			{
				bool flag = value != this._numOfFiefs;
				if (flag)
				{
					this._numOfFiefs = value;
					base.OnPropertyChanged("NumOfFiefs");
				}
			}
		}

		[DataSourceProperty]
		public string TierText
		{
			get
			{
				return this._tierText;
			}
			set
			{
				bool flag = value != this._tierText;
				if (flag)
				{
					this._tierText = value;
					base.OnPropertyChanged("TierText");
				}
			}
		}

		[DataSourceProperty]
		public ImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				bool flag = value != this._banner;
				if (flag)
				{
					this._banner = value;
					base.OnPropertyChanged("Banner");
				}
			}
		}

		[DataSourceProperty]
		public ImageIdentifierVM Banner_9
		{
			get
			{
				return this._banner_9;
			}
			set
			{
				bool flag = value != this._banner_9;
				if (flag)
				{
					this._banner_9 = value;
					base.OnPropertyChanged("Banner_9");
				}
			}
		}

		[DataSourceProperty]
		public MBBindingList<HeroVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				bool flag = value != this._members;
				if (flag)
				{
					this._members = value;
					base.OnPropertyChanged("Members");
				}
			}
		}

		[DataSourceProperty]
		public MBBindingList<KingdomClanFiefItemVM> Fiefs
		{
			get
			{
				return this._fiefs;
			}
			set
			{
				bool flag = value != this._fiefs;
				if (flag)
				{
					this._fiefs = value;
					base.OnPropertyChanged("Fiefs");
				}
			}
		}

		[DataSourceProperty]
		public int Influence
		{
			get
			{
				return this._influence;
			}
			set
			{
				bool flag = value != this._influence;
				if (flag)
				{
					this._influence = value;
					base.OnPropertyChanged("Influence");
				}
			}
		}

		[DataSourceProperty]
		public ImageIdentifierVM Visual
		{
			get
			{
				return this._visual;
			}
			set
			{
				bool flag = value != this._visual;
				if (flag)
				{
					this._visual = value;
					base.OnPropertyChanged("Visual");
				}
			}
		}

		public VassalClanVM(Clan clan, Action<VassalClanVM> onSelect)
		{
			this.Clan = clan;
			this._onSelect = onSelect;
			this.RefreshValues();
			this.Refresh();
		}

		public void EditClanBanner()
		{
			this.OpenBannerSelectionScreen(this.Clan, this.Clan.Leader);
		}

		public void EditClanName()
		{
			InformationManager.ShowTextInquiry(new TextInquiryData(new TextObject("{=JJiKk4ow}Select your family name: ", null).ToString(), string.Empty, true, false, GameTexts.FindText("str_done", null).ToString(), null, new Action<string>(this.OnChangeClanNameDone), null, false, new Func<string, Tuple<bool, string>>(this.IsNewClanNameApplicable), "", ""), false);
		}

		private Tuple<bool, string> IsNewClanNameApplicable(string input)
		{
			return new Tuple<bool, string>(input.Length <= 50 && input.Length >= 1, "EMPTY");
		}

		private void OnChangeClanNameDone(string newClanName)
		{
			TextObject textObject = new TextObject(newClanName ?? "", null);
			//this.Clan.ChangeClanName(textObject);
			this.Name = textObject.ToString();
		}

		private void OpenBannerSelectionScreen(Clan clan, Hero hero)
		{
			NewClanBannerEditorState gameState = new NewClanBannerEditorState(hero.CharacterObject, clan);
			/*bool flag = Game.Current.GameStateManager.GameStateManagerListener != null;
			if (flag)
			{
				Game.Current.GameStateManager.GameStateManagerListener.OnCreateState(gameState);
			}*/
			Game.Current.GameStateManager.PushState(gameState, 0);
           /// Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<BannerEditorState>());
        }

		public override void RefreshValues()
		{
			base.RefreshValues();
			CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(this.Clan.Leader.CharacterObject, false);
			this.Visual = new ImageIdentifierVM(characterCode);
			this.Banner = new ImageIdentifierVM(this.Clan.Banner);
			this.Banner_9 = new ImageIdentifierVM(BannerCode.CreateFrom(this.Clan.Banner), true);
			this.Name = this.Clan.Name.ToString();
			GameTexts.SetVariable("TIER", this.Clan.Tier);
			this.TierText = GameTexts.FindText("str_clan_tier", null).ToString();
		}

		public void Refresh()
		{
			this.Members = new MBBindingList<HeroVM>();
			this.ClanType = 0;
			bool isUnderMercenaryService = this.Clan.IsUnderMercenaryService;
			if (isUnderMercenaryService)
			{
				this.ClanType = 2;
			}
			else
			{
				bool flag = this.Clan.Kingdom.RulingClan == this.Clan;
				if (flag)
				{
					this.ClanType = 1;
				}
			}
			this.NumOfMembers = this.Members.Count;
			this.Fiefs = new MBBindingList<KingdomClanFiefItemVM>();
			IEnumerable<Settlement> settlements = this.Clan.Settlements;
			this.NumOfFiefs = this.Fiefs.Count;
			this.Influence = (int)this.Clan.Influence;
		}
	}
}
