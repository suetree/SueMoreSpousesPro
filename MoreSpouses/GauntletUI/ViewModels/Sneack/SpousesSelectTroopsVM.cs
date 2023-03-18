using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpousesSelectTroopsVM : ViewModel
    {
        private readonly Action<TroopRoster> _onDone;

        private readonly TroopRoster _initialRoster;

        private readonly Func<CharacterObject, bool> _canChangeChangeStatusOfTroop;

        private int _maxSelectableTroopCount;

        private int _currentTotalSelectedTroopCount;

        public bool IsFiveStackModifierActive;

        public bool IsEntireStackModifierActive;

        private bool _isEnabled;

        private string _doneText;

        private string _cancelText;

        private string _titleText;

        private string _currentSelectedAmountText;

        private string _currentSelectedAmountTitle;

        private int _maxNum;

        private MBBindingList<SpousesSelectTroopsItemVM> _troops;

        [DataSourceProperty]
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                bool flag = value != _isEnabled;
                if (flag)
                {
                    _isEnabled = value;
                    OnPropertyChangedWithValue(value, "IsEnabled");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<SpousesSelectTroopsItemVM> Troops
        {
            get
            {
                return _troops;
            }
            set
            {
                bool flag = value != _troops;
                if (flag)
                {
                    _troops = value;
                    OnPropertyChangedWithValue(value, "Troops");
                }
            }
        }

        [DataSourceProperty]
        public string DoneText
        {
            get
            {
                return _doneText;
            }
            set
            {
                bool flag = value != _doneText;
                if (flag)
                {
                    _doneText = value;
                    OnPropertyChangedWithValue(value, "DoneText");
                }
            }
        }

        [DataSourceProperty]
        public string CancelText
        {
            get
            {
                return _cancelText;
            }
            set
            {
                bool flag = value != _cancelText;
                if (flag)
                {
                    _cancelText = value;
                    OnPropertyChangedWithValue(value, "CancelText");
                }
            }
        }

        [DataSourceProperty]
        public string TitleText
        {
            get
            {
                return _titleText;
            }
            set
            {
                bool flag = value != _titleText;
                if (flag)
                {
                    _titleText = value;
                    OnPropertyChangedWithValue(value, "TitleText");
                }
            }
        }

        [DataSourceProperty]
        public string CurrentSelectedAmountText
        {
            get
            {
                return _currentSelectedAmountText;
            }
            set
            {
                bool flag = value != _currentSelectedAmountText;
                if (flag)
                {
                    _currentSelectedAmountText = value;
                    OnPropertyChangedWithValue(value, "CurrentSelectedAmountText");
                }
            }
        }

        [DataSourceProperty]
        public string CurrentSelectedAmountTitle
        {
            get
            {
                return _currentSelectedAmountTitle;
            }
            set
            {
                bool flag = value != _currentSelectedAmountTitle;
                if (flag)
                {
                    _currentSelectedAmountTitle = value;
                    OnPropertyChangedWithValue(value, "CurrentSelectedAmountTitle");
                }
            }
        }

        public SpousesSelectTroopsVM(TroopRoster initialRoster, int maxNum, Func<CharacterObject, bool> canChangeChangeStatusOfTroop, Action<TroopRoster> onDone)
        {
            _canChangeChangeStatusOfTroop = canChangeChangeStatusOfTroop;
            _onDone = onDone;
            _maxNum = maxNum;
            _initialRoster = initialRoster;
            InitList();
            RefreshValues();
            _maxSelectableTroopCount = maxNum;
            OnCurrentSelectedAmountChange();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            TitleText = new TextObject("{=sms_sneak_select_troop_title}Choose your battle companion", null).ToString();
            DoneText = GameTexts.FindText("str_done", null).ToString();
            CancelText = GameTexts.FindText("str_cancel", null).ToString();
            CurrentSelectedAmountTitle = new TextObject("{=sms_sneak_select_troop_name}Action Group", null).ToString();
        }

        private void InitList()
        {
            Troops = new MBBindingList<SpousesSelectTroopsItemVM>();
            _currentTotalSelectedTroopCount = 0;
            List<TroopRosterElement> troopRoster = MobileParty.MainParty.MemberRoster.GetTroopRoster();
            foreach (TroopRosterElement current in troopRoster)
            {
                bool flag = current.Number - current.WoundedNumber > 0;
                if (flag)
                {
                    SpousesSelectTroopsItemVM spousesSelectTroopsItemVM = new SpousesSelectTroopsItemVM(current, new Action<SpousesSelectTroopsItemVM>(OnAddCount), new Action<SpousesSelectTroopsItemVM>(OnRemoveCount));
                    spousesSelectTroopsItemVM.IsLocked = !_canChangeChangeStatusOfTroop(current.Character);
                    Troops.Add(spousesSelectTroopsItemVM);
                    int troopCount = _initialRoster.GetTroopCount(current.Character);
                    bool flag2 = troopCount > 0;
                    if (flag2)
                    {
                        spousesSelectTroopsItemVM.CurrentAmount = troopCount;
                        _currentTotalSelectedTroopCount += troopCount;
                    }
                }
            }
        }

        private void OnRemoveCount(SpousesSelectTroopsItemVM troopItem)
        {
            bool flag = troopItem.CurrentAmount > 0;
            if (flag)
            {
                int num = 1;
                bool isEntireStackModifierActive = IsEntireStackModifierActive;
                if (isEntireStackModifierActive)
                {
                    num = Math.Min(troopItem.MaxAmount - troopItem.CurrentAmount, _maxSelectableTroopCount - _currentTotalSelectedTroopCount);
                }
                else
                {
                    bool isFiveStackModifierActive = IsFiveStackModifierActive;
                    if (isFiveStackModifierActive)
                    {
                        num = Math.Min(Math.Min(troopItem.MaxAmount - troopItem.CurrentAmount, _maxSelectableTroopCount - _currentTotalSelectedTroopCount), 5);
                    }
                }
                troopItem.CurrentAmount -= num;
                _currentTotalSelectedTroopCount -= num;
            }
            OnCurrentSelectedAmountChange();
        }

        private void OnAddCount(SpousesSelectTroopsItemVM troopItem)
        {
            bool flag = troopItem.CurrentAmount < troopItem.MaxAmount && _currentTotalSelectedTroopCount < _maxSelectableTroopCount;
            if (flag)
            {
                int num = 1;
                bool isEntireStackModifierActive = IsEntireStackModifierActive;
                if (isEntireStackModifierActive)
                {
                    num = Math.Min(troopItem.MaxAmount - troopItem.CurrentAmount, _maxSelectableTroopCount - _currentTotalSelectedTroopCount);
                }
                else
                {
                    bool isFiveStackModifierActive = IsFiveStackModifierActive;
                    if (isFiveStackModifierActive)
                    {
                        num = Math.Min(Math.Min(troopItem.MaxAmount - troopItem.CurrentAmount, _maxSelectableTroopCount - _currentTotalSelectedTroopCount), 5);
                    }
                }
                troopItem.CurrentAmount += num;
                _currentTotalSelectedTroopCount += num;
            }
            OnCurrentSelectedAmountChange();
        }

        private void OnCurrentSelectedAmountChange()
        {
            using (IEnumerator<SpousesSelectTroopsItemVM> enumerator = Troops.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.IsRosterFull = _currentTotalSelectedTroopCount >= _maxSelectableTroopCount;
                }
            }
            GameTexts.SetVariable("LEFT", _currentTotalSelectedTroopCount);
            GameTexts.SetVariable("RIGHT", _maxSelectableTroopCount);
            CurrentSelectedAmountText = GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null).ToString();
        }

        private void ExecuteDone()
        {
            TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
            foreach (SpousesSelectTroopsItemVM current in Troops)
            {
                bool flag = current.CurrentAmount > 0;
                if (flag)
                {
                    troopRoster.AddToCounts(current.Troop.Character, current.CurrentAmount, false, 0, 0, true, -1);
                }
            }
            IsEnabled = false;
            _onDone.DynamicInvokeWithLog(new object[]
            {
                troopRoster
            });
        }

        private void ExecuteCancel()
        {
            IsEnabled = false;
        }

        private void ExecuteReset()
        {
            InitList();
            _maxSelectableTroopCount = _maxNum;
            OnCurrentSelectedAmountChange();
        }
    }
}
