using Helpers;
using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpouseServiceItemVM : ViewModel
    {
        private readonly Action<SpouseServiceItemVM> _onCharacterSelect;

        private readonly Hero _hero;

        private ImageIdentifierVM _visual;

        private ImageIdentifierVM _banner_9;

        private bool _isSelected;

        private bool _isChild;

        private bool _isMainHero;

        private bool _isFamilyMember;

        private string _name;

        private string _locationText;

        private string _relationToMainHeroText;

        private string _governorOfText;

        private string _currentActionText;

        private string _pregnancyStatus;

        private string _spousePrimaryStatus;

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

        [DataSourceProperty]
        public bool IsChild
        {
            get
            {
                return _isChild;
            }
            set
            {
                bool flag = value != _isChild;
                if (flag)
                {
                    _isChild = value;
                    OnPropertyChanged("IsChild");
                }
            }
        }

        [DataSourceProperty]
        public bool IsMainHero
        {
            get
            {
                return _isMainHero;
            }
            set
            {
                bool flag = value != _isMainHero;
                if (flag)
                {
                    _isMainHero = value;
                    OnPropertyChanged("IsMainHero");
                }
            }
        }

        [DataSourceProperty]
        public bool IsFamilyMember
        {
            get
            {
                return _isFamilyMember;
            }
            set
            {
                bool flag = value != _isFamilyMember;
                if (flag)
                {
                    _isFamilyMember = value;
                    OnPropertyChanged("IsFamilyMember");
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
                    OnPropertyChanged("Visual");
                }
            }
        }

        [DataSourceProperty]
        public ImageIdentifierVM Banner_9
        {
            get
            {
                return _banner_9;
            }
            set
            {
                bool flag = value != _banner_9;
                if (flag)
                {
                    _banner_9 = value;
                    OnPropertyChanged("Banner_9");
                }
            }
        }

        [DataSourceProperty]
        public string LocationText
        {
            get
            {
                return _locationText;
            }
            set
            {
                bool flag = value != _locationText;
                if (flag)
                {
                    _locationText = value;
                    OnPropertyChanged("LocationText");
                }
            }
        }

        [DataSourceProperty]
        public string RelationToMainHeroText
        {
            get
            {
                return _relationToMainHeroText;
            }
            set
            {
                bool flag = value != _relationToMainHeroText;
                if (flag)
                {
                    _relationToMainHeroText = value;
                    OnPropertyChanged("RelationToMainHeroText");
                }
            }
        }

        [DataSourceProperty]
        public string GovernorOfText
        {
            get
            {
                return _governorOfText;
            }
            set
            {
                bool flag = value != _governorOfText;
                if (flag)
                {
                    _governorOfText = value;
                    OnPropertyChanged("GovernorOfText");
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
                    OnPropertyChanged("Name");
                }
            }
        }

        [DataSourceProperty]
        public string PregnancyStatus
        {
            get
            {
                return _pregnancyStatus;
            }
            set
            {
                bool flag = value != _pregnancyStatus;
                if (flag)
                {
                    _pregnancyStatus = value;
                    OnPropertyChanged("PregnancyStatus");
                }
            }
        }

        [DataSourceProperty]
        public string SpousePrimaryStatus
        {
            get
            {
                return _spousePrimaryStatus;
            }
            set
            {
                bool flag = value != _spousePrimaryStatus;
                if (flag)
                {
                    _spousePrimaryStatus = value;
                    OnPropertyChanged("SpousePrimaryStatus");
                }
            }
        }

        [DataSourceProperty]
        public string CurrentActionText
        {
            get
            {
                return _currentActionText;
            }
            set
            {
                bool flag = value != _currentActionText;
                if (flag)
                {
                    _currentActionText = value;
                    OnPropertyChanged("CurrentActionText");
                }
            }
        }

        public Hero Hero
        {
            get
            {
                return _hero;
            }
        }

        public SpouseServiceItemVM(Hero hero, Action<SpouseServiceItemVM> onCharacterSelect)
        {
            _hero = hero;
            _onCharacterSelect = onCharacterSelect;
            CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false);
            Visual = new ImageIdentifierVM(characterCode);
            Banner_9 = new ImageIdentifierVM(BannerCode.CreateFrom(hero.ClanBanner), true);
            RefreshValues();
        }

        public void OnCharacterSelect()
        {
            IsSelected = true;
            _onCharacterSelect(this);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            bool isPregnant = _hero.IsPregnant;
            if (isPregnant)
            {
                PregnancyStatus = new TextObject("{=suems_doctor_satus_pregnancy_get}Pregnant", null).ToString();
            }
            else
            {
                PregnancyStatus = "";
            }
            bool flag = Hero.MainHero.Spouse == _hero;
            if (flag)
            {
                SpousePrimaryStatus = new TextObject("{=suems_doctor_satus_primary_spouse}Primary", null).ToString();
            }
            else
            {
                SpousePrimaryStatus = "";
            }
            Name = _hero.Name.ToString();
            CurrentActionText = _hero != Hero.MainHero ? CampaignUIHelper.GetHeroBehaviorText(_hero) : "";
            bool flag2 = _hero.PartyBelongedToAsPrisoner != null;
            if (flag2)
            {
                TextObject textObject = new TextObject("{=a8nRxITn}Prisoner of {PARTY_NAME}", null);
                textObject.SetTextVariable("PARTY_NAME", _hero.PartyBelongedToAsPrisoner.Name);
                LocationText = textObject.ToString();
            }
            else
            {
                LocationText = _hero != Hero.MainHero ? StringHelpers.GetLastKnownLocation(_hero).ToString() : " ";
            }
        }
    }
}
