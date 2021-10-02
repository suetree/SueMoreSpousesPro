using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueEasyMenu.GauntletUI.ViewModels
{
    public class EMOptionClassicsGroupVM : ViewModel
    {
        private MBBindingList<EMOptionGroupVM> _optionGroupVMs;

        private EMOptionGroupVM _currentGroupVM;

        [DataSourceProperty]
        public string Name
        {

            get
            {
                return new TextObject("TEST", null).ToString();
            }
        }

        [DataSourceProperty]
        public bool EnableGroup
        {

            get
            {
                return _currentGroupVM.BoolValue;
            }

            set
            {
                if (!_currentGroupVM.BoolValue.Equals(value))
                {
                    _currentGroupVM.BoolValue = value;
                    OnPropertyChangedWithValue(value, "EnableGroup");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EMOptionGroupItemVM> OptionItems
        {
            get
            {
                return _currentGroupVM.OptionItems;
            }
        }


        [DataSourceProperty]
        public MBBindingList<EMOptionGroupVM> SettingGroups
        {
            get
            {
                return _optionGroupVMs;
            }
        }

        public EMOptionClassicsGroupVM(MBBindingList<EMOptionGroupVM> groups)
        {
            _optionGroupVMs = groups;
            _currentGroupVM = _optionGroupVMs[0];
            _currentGroupVM.IsSelected = true;
            foreach (EMOptionGroupVM group in _optionGroupVMs)
            {
                group.SetOnGroupSelectedAction(OnGroupSelected);
                group.SetOnGroupEnableChangeAction(OnGroupEnableChange);
                foreach (EMOptionGroupItemVM item in group.OptionItems)
                {
                    item.BackgroundBrushName = "";
                }
            }
        }

        private void OnGroupSelected(EMOptionGroupVM group)
        {
            if (null != _currentGroupVM)
            {
                _currentGroupVM.IsSelected = false;
            }
            _currentGroupVM = group;
            OnPropertyChanged("OptionItems");
            OnPropertyChanged("EnableGroup");
        }

        private void OnGroupEnableChange(EMOptionGroupVM group)
        {
            OnPropertyChanged("EnableGroup");
        }
    }
}
