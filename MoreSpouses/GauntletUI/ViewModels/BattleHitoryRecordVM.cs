using SueMoreSpouses.Data;
using System;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHitoryRecordVM : ViewModel
    {
        private readonly SpousesBattleRecord _recordData;

        private readonly Action<BattleHitoryRecordVM> _onRecordSelected;

        private bool _isSelected;

        public SpousesBattleRecord GetSpousesBattleRecord
        {
            get
            {
                return _recordData;
            }
        }

        [DataSourceProperty]
        public string Name
        {
            get
            {
                return _recordData.Name;
            }
        }

        [DataSourceProperty]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                bool flag = value != _isSelected;
                if (flag)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public SpousesBattleRecord RecordData
        {
            get
            {
                return _recordData;
            }
        }

        public BattleHitoryRecordVM(SpousesBattleRecord data, Action<BattleHitoryRecordVM> recordSelected)
        {
            _recordData = data;
            _onRecordSelected = recordSelected;
        }

        public void OnHistoryRecordSelected()
        {
            bool flag = !IsSelected;
            if (flag)
            {
                IsSelected = true;
                _onRecordSelected(this);
            }
        }
    }
}
