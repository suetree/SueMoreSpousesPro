using SandBox.GauntletUI;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using SueEasyMenu.GauntletUI.ViewModels;
using SueEasyMenu.Models;
using SueMoreSpouses.Settings;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpouseDashboardVM : ViewModel
    {
        private SpouseClanVM _parentView;

        private GauntletClanScreen _parentScreen;

        private EMOptionClassicsGroupVM _eMOptionClassicsGroupVM;



        private bool _isFemaleSelected = true;

        private bool _isSettingSelected = false;

        private bool _isStatisticsSelected = false;

        private SpouseServiceVM _spouseServiceView;

        private SpousesBattleStatisticVM _spousesBattleStats;

        [DataSourceProperty]
        public string DisplayName
        {
            get
            {
                return new TextObject("{=sue_more_spouses_btn_mangager}Spouse Service", null).ToString();
            }
        }

        [DataSourceProperty]
        public string FemaleDoctorText
        {
            get
            {
                return new TextObject("{=suems_table_spouse}Spouse", null).ToString();
            }
        }

        [DataSourceProperty]
        public string RecordText
        {
            get
            {
                return new TextObject("{=sms_battle_record}Battle Record", null).ToString();
            }
        }

        [DataSourceProperty]
        public string SettingText
        {
            get
            {
                return new TextObject("{=suems_table_settings}Setting", null).ToString();
            }
        }

        [DataSourceProperty]
        public SpouseServiceVM SpousesService
        {
            get
            {
                return _spouseServiceView;
            }
        }

        [DataSourceProperty]
        public SpousesBattleStatisticVM SpousesBattleStats
        {
            get
            {
                return _spousesBattleStats;
            }
        }

        [DataSourceProperty]
        public EMOptionClassicsGroupVM ClassicsGroup
        {
            get
            {
                return _eMOptionClassicsGroupVM;
            }
        }

        [DataSourceProperty]
        public bool IsFemaleDoctorSelected
        {
            get
            {
                return _isFemaleSelected;
            }
            set
            {
                bool flag = value != _isFemaleSelected;
                if (flag)
                {
                    _isFemaleSelected = value;
                    OnPropertyChanged("IsFemaleDoctorSelected");
                }
            }
        }

        [DataSourceProperty]
        public bool IsStatisticsSelected
        {
            get
            {
                return _isStatisticsSelected;
            }
            set
            {
                bool flag = value != _isStatisticsSelected;
                if (flag)
                {
                    _isStatisticsSelected = value;
                    OnPropertyChanged("IsStatisticsSelected");
                }
            }
        }

        [DataSourceProperty]
        public bool IsSettingSelected
        {
            get
            {
                return _isSettingSelected;
            }
            set
            {
                bool flag = value != _isSettingSelected;
                if (flag)
                {
                    _isSettingSelected = value;
                    OnPropertyChanged("IsSettingSelected");
                }
            }
        }

        public SpouseDashboardVM(SpouseClanVM parent, GauntletClanScreen parentScreen)
        {
            _parentView = parent;
            _parentScreen = parentScreen;
            MBBindingList<EMOptionGroupVM> settingGroups = new MBBindingList<EMOptionGroupVM>();
            List<EMOptionGroup> spouseSettingGroups = MoreSpouseSetting.GetInstance().GenerateSettingsProperties();
            spouseSettingGroups.ForEach(delegate (EMOptionGroup obj)
            {
                settingGroups.Add(new EMOptionGroupVM(obj));
            });

            _eMOptionClassicsGroupVM = new EMOptionClassicsGroupVM(settingGroups);
            _spousesBattleStats = new SpousesBattleStatisticVM(_parentView);
            _spouseServiceView = new SpouseServiceVM();
            RefreshValues();
        }

        public void SetSelectedCategory(int index)
        {
            IsFemaleDoctorSelected = false;
            IsSettingSelected = false;
            IsStatisticsSelected = false;
            bool flag = index == 0;
            if (flag)
            {
                IsFemaleDoctorSelected = true;
            }
            else
            {
                bool flag2 = index == 1;
                if (flag2)
                {
                    IsSettingSelected = true;
                }
                else
                {
                    bool flag3 = index == 2;
                    if (flag3)
                    {
                        IsStatisticsSelected = true;
                    }
                }
            }
        }

        public void ExecuteCloseSettings()
        {
            _parentView.CloseSettingView();
            MoreSpouseSetting.GetInstance().SaveSettingData();
            OnFinalize();
        }

        public new void OnFinalize()
        {
            base.OnFinalize();
            bool flag = Game.Current != null;
            bool flag2 = flag;
            if (flag2)
            {
                Game.Current.AfterTick = (Action<float>)Delegate.Remove(Game.Current.AfterTick, new Action<float>(AfterTick));
            }
            _parentView = null;
        }

        public void AfterTick(float dt)
        {
            bool flag = _parentView.IsHotKeyPressed("Exit");
            bool flag2 = flag;
            if (flag2)
            {
                ExecuteCloseSettings();
            }
        }
    }
}
