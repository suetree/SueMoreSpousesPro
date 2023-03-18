using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpousesSelectTroopsItemVM : ViewModel
    {
        private readonly Action<SpousesSelectTroopsItemVM> _onAdd;

        private readonly Action<SpousesSelectTroopsItemVM> _onRemove;

        private int _currentAmount;

        private int _maxAmount;

        private ImageIdentifierVM _visual;

        private bool _isSelected;

        private bool _isRosterFull;

        private bool _isLocked;

        private string _name;

        private string _amountText;

        private StringItemWithHintVM _tierIconData;

        private StringItemWithHintVM _typeIconData;

        public TroopRosterElement Troop
        {
            get;
            private set;
        }

        [DataSourceProperty]
        public int MaxAmount
        {
            get
            {
                return _maxAmount;
            }
            set
            {
                bool flag = value != _maxAmount;
                if (flag)
                {
                    _maxAmount = value;
                    OnPropertyChangedWithValue(value, "MaxAmount");
                    UpdateAmountText();
                }
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
                    OnPropertyChangedWithValue(value, "IsSelected");
                }
            }
        }

        [DataSourceProperty]
        public bool IsRosterFull
        {
            get
            {
                return _isRosterFull;
            }
            set
            {
                bool flag = value != _isRosterFull;
                if (flag)
                {
                    _isRosterFull = value;
                    OnPropertyChangedWithValue(value, "IsRosterFull");
                }
            }
        }

        [DataSourceProperty]
        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                bool flag = value != _isLocked;
                if (flag)
                {
                    _isLocked = value;
                    OnPropertyChangedWithValue(value, "IsLocked");
                }
            }
        }

        [DataSourceProperty]
        public int CurrentAmount
        {
            get
            {
                return _currentAmount;
            }
            set
            {
                bool flag = value != _currentAmount;
                if (flag)
                {
                    _currentAmount = value;
                    OnPropertyChangedWithValue(value, "CurrentAmount");
                    IsSelected = value > 0;
                    UpdateAmountText();
                }
            }
        }

        [DataSourceProperty]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                bool flag = value != _name;
                if (flag)
                {
                    _name = value;
                    OnPropertyChangedWithValue(value, "Name");
                }
            }
        }

        [DataSourceProperty]
        public string AmountText
        {
            get
            {
                return _amountText;
            }
            set
            {
                bool flag = value != _amountText;
                if (flag)
                {
                    _amountText = value;
                    OnPropertyChangedWithValue(value, "AmountText");
                }
            }
        }

        [DataSourceProperty]
        public ImageIdentifierVM Visual
        {
            get
            {
                return _visual;
            }
            set
            {
                bool flag = value != _visual;
                if (flag)
                {
                    _visual = value;
                    OnPropertyChangedWithValue(value, "Visual");
                }
            }
        }

        [DataSourceProperty]
        public StringItemWithHintVM TierIconData
        {
            get
            {
                return _tierIconData;
            }
            set
            {
                bool flag = value != _tierIconData;
                if (flag)
                {
                    _tierIconData = value;
                    OnPropertyChangedWithValue(value, "TierIconData");
                }
            }
        }

        [DataSourceProperty]
        public StringItemWithHintVM TypeIconData
        {
            get
            {
                return _typeIconData;
            }
            set
            {
                bool flag = value != _typeIconData;
                if (flag)
                {
                    _typeIconData = value;
                    OnPropertyChangedWithValue(value, "TypeIconData");
                }
            }
        }

        public SpousesSelectTroopsItemVM(TroopRosterElement troop, Action<SpousesSelectTroopsItemVM> onAdd, Action<SpousesSelectTroopsItemVM> onRemove)
        {
            _onAdd = onAdd;
            _onRemove = onRemove;
            Troop = troop;
            MaxAmount = Troop.Number - Troop.WoundedNumber;
            Visual = new ImageIdentifierVM(CampaignUIHelper.GetCharacterCode(troop.Character, false));
            Name = troop.Character.Name.ToString();
            TierIconData = CampaignUIHelper.GetCharacterTierData(Troop.Character, false);
            TypeIconData = CampaignUIHelper.GetCharacterTypeData(Troop.Character, false);
        }

        private void ExecuteAdd()
        {
            Action<SpousesSelectTroopsItemVM> onAdd = _onAdd;
            bool flag = onAdd == null;
            if (!flag)
            {
                onAdd.DynamicInvokeWithLog(new object[]
                {
                    this
                });
            }
        }

        private void ExecuteRemove()
        {
            Action<SpousesSelectTroopsItemVM> onRemove = _onRemove;
            bool flag = onRemove == null;
            if (!flag)
            {
                onRemove.DynamicInvokeWithLog(new object[]
                {
                    this
                });
            }
        }

        private void UpdateAmountText()
        {
            GameTexts.SetVariable("LEFT", CurrentAmount);
            GameTexts.SetVariable("RIGHT", MaxAmount);
            AmountText = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
        }

        private void ExecuteLink()
        {
            bool flag = Troop.Character != null;
            if (flag)
            {
                EncyclopediaManager encyclopediaManager = Campaign.Current.EncyclopediaManager;
                Hero heroObject = Troop.Character.HeroObject;
                encyclopediaManager.GoToLink((heroObject != null ? heroObject.EncyclopediaLink : null) ?? Troop.Character.EncyclopediaLink);
            }
        }
    }
}
