using SueMoreSpouses.GauntletUI.States;
using SueMoreSpouses.Utils;
using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.BodyGenerator;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace SueMoreSpouses.GauntletUI
{
    [GameStateScreen(typeof(FaceDetailsCreatorState))]
    internal class FaceDetailsCreatorScreen : ScreenBase, IGameStateListener
    {
        private BodyGeneratorView _facegenLayer;

        private bool _oldGameStateManagerDisabledStatus;

        private FaceDetailsCreatorState _faceDetailsCreatorState;

        public FaceDetailsCreatorScreen(FaceDetailsCreatorState faceDetailsCreatorState)
        {
            LoadingWindow.EnableGlobalLoadingWindow();
            _faceDetailsCreatorState = faceDetailsCreatorState;
            _facegenLayer = new BodyGeneratorView(new ControlCharacterCreationStage(OnExit), GameTexts.FindText("str_done", null), new ControlCharacterCreationStage(OnExit), GameTexts.FindText("str_cancel", null), faceDetailsCreatorState.EditHero.CharacterObject, false, null, null, null, null, null);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            _facegenLayer.OnTick(dt);
            bool flag = _facegenLayer.SceneLayer.Input.IsHotKeyReleased("Exit");
            if (flag)
            {
                OnExit();
            }
        }

        public void OnExit()
        {
            HeroFaceUtils.UpdatePlayerCharacterBodyProperties(_faceDetailsCreatorState.EditHero, _facegenLayer.BodyGen.CurrentBodyProperties, _facegenLayer.BodyGen.IsFemale);
            Game.Current.GameStateManager.PopState(0);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _oldGameStateManagerDisabledStatus = Game.Current.GameStateManager.ActiveStateDisabledByUser;
            //Game.Current.GameStateManager.ActiveStateDisabledByUser = true;
            AddLayer(_facegenLayer.GauntletLayer);
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();
            bool globalLoadingWindowState = LoadingWindow.GetGlobalLoadingWindowState();
            if (globalLoadingWindowState)
            {
                LoadingWindow.DisableGlobalLoadingWindow();
            }
           // Game.Current.GameStateManager.ActiveStateDisabledByUser = _oldGameStateManagerDisabledStatus;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            AddLayer(_facegenLayer.SceneLayer);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            _facegenLayer.OnFinalize();
            LoadingWindow.EnableGlobalLoadingWindow();
            InformationManager.ClearAllMessages();
            Mission current = Mission.Current;
            bool flag = current != null;
            if (flag)
            {
                foreach (Agent current2 in current.Agents)
                {
                    current2.EquipItemsFromSpawnEquipment(true);
                    current2.UpdateAgentProperties();
                }
            }
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
