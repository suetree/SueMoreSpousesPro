
using SueTheGeneration.GauntletUI.State;
using SueTheGenernation.GauntletUI.ViewModel;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SueTheGeneration.GauntletUI
{
    [GameStateScreen(typeof(TheGenerationState))]
    class TheGenerationScreen : ScreenBase, IGameStateListener
    {
        private TheGenerationState _theSecondGenerationState;
		//private bool _oldGameStateManagerDisabledStatus;
		private GauntletLayer _theSecondGenerationLayer;
		private TheSecondGernationVM _theSecondGernationVM;
		private SpriteCategory _spriteCategory;


		public TheGenerationScreen(TheGenerationState state)
        {
			LoadingWindow.EnableGlobalLoadingWindow();
			this._theSecondGenerationState = state;
			
		}

		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			LoadingWindow.DisableGlobalLoadingWindow();//关闭加载界面
			if (this._theSecondGenerationLayer.Input.IsHotKeyReleased("Exit") || this._theSecondGenerationLayer.Input.IsGameKeyReleased(36))
			{
				this.CloseScreen();
			}
		}

		public void CloseScreen()
		{
			Game.Current.GameStateManager.PopState(0);
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();
            LoadingWindow.EnableGlobalLoadingWindow();
		}

		private void InitLayer()
        {
			this._theSecondGenerationLayer = new GauntletLayer(100, "GauntletLayer");
			this._theSecondGenerationLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._theSecondGenerationLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
			this._theSecondGenerationLayer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(this._theSecondGenerationLayer);
			base.AddLayer(this._theSecondGenerationLayer);
			this._theSecondGernationVM = new TheSecondGernationVM(this);
			this._theSecondGenerationLayer.LoadMovie("TheGenerationDashboard", this._theSecondGernationVM);
		}

		protected override void OnFinalize()
		{
			base.OnFinalize();
			if (LoadingWindow.GetGlobalLoadingWindowState())
			{
				LoadingWindow.DisableGlobalLoadingWindow();
			}
			this._theSecondGernationVM = null;
			this._theSecondGenerationLayer = null;
		}

		protected override void OnActivate()
		{
			base.OnActivate();
			SpriteData spriteData = UIResourceManager.SpriteData;
			TwoDimensionEngineResourceContext resourceContext = UIResourceManager.ResourceContext;
			ResourceDepot uIResourceDepot = UIResourceManager.UIResourceDepot;
			this._spriteCategory = spriteData.SpriteCategories["ui_clan"];
			this._spriteCategory.Load(resourceContext, uIResourceDepot);
			InitLayer();
		}

		protected override void OnDeactivate()
		{
			base.OnDeactivate();
			LoadingWindow.EnableGlobalLoadingWindow();
			InformationManager.ClearAllMessages();
		}

		void IGameStateListener.OnActivate()
        {
           
        }

        void IGameStateListener.OnDeactivate()
        {
          
        }

        void IGameStateListener.OnFinalize()
        {
            
        }

        void IGameStateListener.OnInitialize()
        {
            
        }
    }
}
