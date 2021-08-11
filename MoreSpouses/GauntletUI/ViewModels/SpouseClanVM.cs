using SandBox.GauntletUI;
using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpouseClanVM : ViewModel
    {
        private IGauntletMovie _currentMovie;

        private GauntletLayer _spouseServiceLayer;

        private GauntletClanScreen _parentScreen;

        private SpouseDashboardVM _spouseServiceView;

        [DataSourceProperty]
        public string BtnName
        {
            get
            {
                return new TextObject("{=sue_more_spouses_btn_mangager}Spouse Service", null).ToString();
            }
        }

        public SpouseClanVM(GauntletClanScreen gauntletClanScreen)
        {
            _parentScreen = gauntletClanScreen;
        }

        public void ShowSpouseServiceView()
        {
            bool flag = _spouseServiceLayer == null;
            bool flag2 = flag;
            if (flag2)
            {
                _spouseServiceLayer = new GauntletLayer(200, "GauntletLayer", false);
                _spouseServiceView = new SpouseDashboardVM(this, _parentScreen);
                _currentMovie = _spouseServiceLayer.LoadMovie("SpouseDashboard", _spouseServiceView);
                _spouseServiceLayer.IsFocusLayer = true;
                ScreenManager.TrySetFocus(_spouseServiceLayer);
                _spouseServiceLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
                _parentScreen.AddLayer(_spouseServiceLayer);
                _spouseServiceLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            }
        }

        public void CloseSettingView()
        {
            bool flag = _spouseServiceLayer != null;
            bool flag2 = flag;
            if (flag2)
            {
                _spouseServiceLayer.ReleaseMovie(_currentMovie);
                _parentScreen.RemoveLayer(_spouseServiceLayer);
                _spouseServiceLayer.InputRestrictions.ResetInputRestrictions();
                _spouseServiceLayer = null;
                _spouseServiceView = null;
                RefreshValues();
            }
        }

        public bool IsHotKeyPressed(string hotkey)
        {
            bool flag = _spouseServiceLayer != null;
            return flag && _spouseServiceLayer.Input.IsHotKeyReleased(hotkey);
        }
    }
}
