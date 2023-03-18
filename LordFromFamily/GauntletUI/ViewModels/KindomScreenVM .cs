using SandBox.GauntletUI;
using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace SueLordFromFamily.GauntletUI.ViewModels
{
	internal class KindomScreenVM : ViewModel
	{
		private GauntletKingdomScreen _parentScreen;

		private IGauntletMovie _currentMovie;

		private GauntletLayer _serviceLayer;

		private VassalServiceVM clanServiceVM;

		[DataSourceProperty]
		public string BtnName
		{
			get
			{
				return new TextObject("{=sue_clan_create_from_family_clan_service}Clan Service", null).ToString();
			}
		}

		public KindomScreenVM(GauntletKingdomScreen gauntletClanScreen)
		{
			this._parentScreen = gauntletClanScreen;
		}

		public void ShowClanServiceView()
		{
			bool flag = this._serviceLayer == null;
			bool flag2 = flag;
			if (flag2)
			{
				this._serviceLayer = new GauntletLayer(200, "GauntletLayer");
				this.clanServiceVM = new VassalServiceVM(this, this._parentScreen, new Action(this.OpenBannerEditorWithPlayerClan));
				this._currentMovie = this._serviceLayer.LoadMovie("VassalService", this.clanServiceVM);
				this._serviceLayer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(this._serviceLayer);
				this._serviceLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
				this._parentScreen.AddLayer(this._serviceLayer);
				this._serviceLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			}
		}

		private void OpenBannerEditorWithPlayerClan()
		{
			Game.Current.GameStateManager.PushState(Game.Current.GameStateManager.CreateState<BannerEditorState>(), 0);
		}

		public override void RefreshValues()
		{
			bool flag = this.clanServiceVM != null;
			if (flag)
			{
				this.clanServiceVM.RefreshValues();
			}
		}

		public void CloseSettingView()
		{
			bool flag = this._serviceLayer != null;
			bool flag2 = flag;
			if (flag2)
			{
				this._serviceLayer.ReleaseMovie(this._currentMovie);
				this._parentScreen.RemoveLayer(this._serviceLayer);
				this._serviceLayer.InputRestrictions.ResetInputRestrictions();
				this._serviceLayer = null;
				this.clanServiceVM = null;
				this.RefreshValues();
			}
		}

		public bool IsHotKeyPressed(string hotkey)
		{
			bool flag = this._serviceLayer != null;
			return flag && this._serviceLayer.Input.IsHotKeyReleased(hotkey);
		}
	}
}
