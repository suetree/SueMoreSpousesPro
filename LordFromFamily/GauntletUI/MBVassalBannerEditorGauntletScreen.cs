using SandBox.GauntletUI;
using SandBox.GauntletUI.BannerEditor;
using SueLordFromFamily.GauntletUI.States;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SueLordFromFamily.GauntletUI
{
	[GameStateScreen(typeof(NewClanBannerEditorState))]
	internal class MBVassalBannerEditorGauntletScreen : ScreenBase, IGameStateListener
	{
		private const int ViewOrderPriority = 16;

		private readonly BannerEditorView _bannerEditorLayer;

		private readonly Clan _clan;

		private bool _oldGameStateManagerDisabledStatus;

		public MBVassalBannerEditorGauntletScreen(NewClanBannerEditorState bannerEditorState)
		{
			LoadingWindow.EnableGlobalLoadingWindow();
			this._clan = bannerEditorState.GetClan();
			this._bannerEditorLayer = new BannerEditorView(bannerEditorState.GetCharacter(), bannerEditorState.GetClan().Banner, new ControlCharacterCreationStage(this.OnDone), new TextObject("{=WiNRdfsm}Done", null), new ControlCharacterCreationStage(this.OnCancel), new TextObject("{=3CpNUnVl}Cancel", null), null, null, null, null, null);
			this._bannerEditorLayer.DataSource.SetClanRelatedRules(bannerEditorState.GetClan().Kingdom == null);
		}

		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this._bannerEditorLayer.OnTick(dt);
			bool flag = Input.IsKeyReleased(InputKey.Escape);
			if (flag)
			{
				this.OnCancel();
			}
		}

		public void OnDone()
		{
			uint primaryColor = this._bannerEditorLayer.DataSource.BannerVM.GetPrimaryColor();
			uint sigilColor = this._bannerEditorLayer.DataSource.BannerVM.GetSigilColor();
			this._clan.Color = primaryColor;
			this._clan.Color2 = sigilColor;
			Game.Current.GameStateManager.PopState(0);
		}

		public void OnCancel()
		{
			Game.Current.GameStateManager.PopState(0);
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
			this._oldGameStateManagerDisabledStatus = Game.Current.GameStateManager.ActiveStateDisabledByUser;
			//Game.Current.GameStateManager.ActiveStateDisabledByUser = true;
		}

		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._bannerEditorLayer.OnFinalize();
			bool globalLoadingWindowState = LoadingWindow.GetGlobalLoadingWindowState();
			if (globalLoadingWindowState)
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			//Game.Current.GameStateManager.ActiveStateDisabledByUser = this._oldGameStateManagerDisabledStatus;
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			base.AddLayer(this._bannerEditorLayer.GauntletLayer);
			base.AddLayer(this._bannerEditorLayer.SceneLayer);
		}

		protected override void OnDeactivate()
		{
			this._bannerEditorLayer.OnDeactivate();
		}

		void IGameStateListener.OnActivate()
		{
		}

		void IGameStateListener.OnDeactivate()
		{
		}

		void IGameStateListener.OnInitialize()
		{
		}

		void IGameStateListener.OnFinalize()
		{
		}
	}
}
