using SandBox.View.Map;
using SandBox.View.Menu;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Missions;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    [OverrideView(typeof(SpousesDefaultSelectTroops))]
    internal class GauntletMenuSpousesSelectTroops : MenuView
    {
        private readonly TroopRoster _initialRoster;

        private readonly Action<TroopRoster> _onDone;

        private readonly Func<CharacterObject, bool> _changeChangeStatusOfTroop;

        private readonly Action _onCanel;

        private GauntletLayer _gauntletLayer;

        private SpousesSelectTroopsVM _dataSource;

        private IGauntletMovie _movie;

        private int _maxNum;

        public GauntletMenuSpousesSelectTroops(TroopRoster initialRoster, int maxNum, Func<CharacterObject, bool> changeChangeStatusOfTroop, Action<TroopRoster> onDone, Action onCanel)
        {
            _onCanel = onCanel;
            _onDone = onDone;
            _initialRoster = initialRoster;
            _maxNum = maxNum;
            _changeChangeStatusOfTroop = changeChangeStatusOfTroop;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _dataSource = new SpousesSelectTroopsVM(_initialRoster, _maxNum, _changeChangeStatusOfTroop, _onDone)
            {
                IsEnabled = true
            };
            _gauntletLayer = new GauntletLayer(205, "GauntletLayer", false)
            {
                Name = "MenuSpouseSelectTroops"
            };
            _gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            _gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
            _movie = _gauntletLayer.LoadMovie("SpousesSelectTroops", _dataSource);
            _gauntletLayer.IsFocusLayer = true;
            ScreenManager.TrySetFocus(_gauntletLayer);
            MenuViewContext.AddLayer(_gauntletLayer);
            MapScreen mapScreen;
            bool flag = (mapScreen = ScreenManager.TopScreen as MapScreen) != null;
            if (flag)
            {
                mapScreen.IsInHideoutTroopManage = true;
            }
        }

        protected override void OnFinalize()
        {
            _gauntletLayer.IsFocusLayer = false;
            ScreenManager.TryLoseFocus(_gauntletLayer);
            _dataSource.OnFinalize();
            _dataSource = null;
            _gauntletLayer.ReleaseMovie(_movie);
            MenuViewContext.RemoveLayer(_gauntletLayer);
            _movie = null;
            _gauntletLayer = null;
            MapScreen mapScreen;
            bool flag = (mapScreen = ScreenManager.TopScreen as MapScreen) != null;
            if (flag)
            {
                mapScreen.IsInHideoutTroopManage = false;
            }
            base.OnFinalize();
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            _dataSource.IsFiveStackModifierActive = _gauntletLayer.Input.IsHotKeyDown("FiveStackModifier") || _gauntletLayer.Input.IsHotKeyDown("FiveStackModifierAlt");
            _dataSource.IsEntireStackModifierActive = _gauntletLayer.Input.IsHotKeyDown("EntireStackModifier") || _gauntletLayer.Input.IsHotKeyDown("EntireStackModifierAlt");
            _gauntletLayer.Input.IsHotKeyReleased("Exit");
            bool flag = _gauntletLayer.Input.IsHotKeyReleased("Exit");
            if (flag)
            {
                _dataSource.IsEnabled = false;
            }
            bool flag2 = !_dataSource.IsEnabled;
            if (flag2)
            {
                bool flag3 = _onCanel != null;
                if (flag3)
                {
                    _onCanel();
                }
            }
        }
    }
}
