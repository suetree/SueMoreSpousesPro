using Helpers;
using SueMoreSpouses.GauntletUI.States;
using SueMoreSpouses.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpouseServiceVM : ViewModel
    {
        private List<Hero> _spouses = new List<Hero>();

        private MBBindingList<SpouseServiceItemVM> _spouseViews;

        private Hero _selectedHero;

        private SpouseServiceItemVM _currentSpouseView;

        private SpouseCharacterVM _selectedCharacter;

        private SpouseServiceItemVM _lastPrimaryView;

        private bool _canGetPregnancy;

        private bool _notPrimarySpouse;

        [DataSourceProperty]
        public MBBindingList<SpouseServiceItemVM> Spouses
        {
            get
            {
                return _spouseViews;
            }
        }

        [DataSourceProperty]
        public string PregnancyText
        {
            get
            {
                return new TextObject("{=suems_table_doctor_pregnancy}Get Pregnancy", null).ToString();
            }
        }

        [DataSourceProperty]
        public string SetPrimarySpouseText
        {
            get
            {
                return new TextObject("{=suems_table_doctor_primary_spouse}Primary Spouse", null).ToString();
            }
        }

        [DataSourceProperty]
        public string DivorceText
        {
            get
            {
                return new TextObject("{=suems_table_doctor_divorce}divorce", null).ToString();
            }
        }

        [DataSourceProperty]
        public bool CanGetPregnancy
        {
            get
            {
                return _canGetPregnancy;
            }
            set
            {
                bool flag = value != _canGetPregnancy;
                if (flag)
                {
                    _canGetPregnancy = value;
                    OnPropertyChanged("CanGetPregnancy");
                }
            }
        }

        [DataSourceProperty]
        public bool CanDivorce
        {
            get
            {
                return _selectedHero != null;
            }
        }

        [DataSourceProperty]
        public bool ShowCharacterView
        {
            get
            {
                return _spouseViews != null && _spouseViews.Count() > 0;
            }
        }

        [DataSourceProperty]
        public bool IsNotPrimarySpouse
        {
            get
            {
                return _notPrimarySpouse;
            }
            set
            {
                bool flag = value != _notPrimarySpouse;
                if (flag)
                {
                    _notPrimarySpouse = value;
                    OnPropertyChanged("IsNotPrimarySpouse");
                }
            }
        }

        [DataSourceProperty]
        public SpouseCharacterVM SelectedCharacter
        {
            get
            {
                return _selectedCharacter;
            }
            set
            {
                bool flag = value != _selectedCharacter;
                if (flag)
                {
                    _selectedCharacter = value;
                    OnPropertyChanged("SelectedCharacter");
                }
            }
        }

        public SpouseServiceVM()
        {
            RefreshSpouseViews();
        }

        public void RefreshSpouseViews()
        {
            _spouses = new List<Hero>();
            _selectedHero = null;
            bool flag = _spouseViews != null;
            if (flag)
            {
                _spouseViews.Clear();
            }
            _spouseViews = new MBBindingList<SpouseServiceItemVM>();
            bool flag2 = Hero.MainHero.Spouse != null;
            if (flag2)
            {
                _spouses.Add(Hero.MainHero.Spouse);
            }
            Hero.MainHero.ExSpouses.ToList().ForEach(delegate (Hero obj)
            {
                bool flag5 = !_spouses.Contains(obj);
                if (flag5)
                {
                    bool isAlive = obj.IsAlive;
                    if (isAlive)
                    {
                        _spouses.Add(obj);
                    }
                }
            });
            _spouses.ForEach(delegate (Hero obj)
            {
                bool flag5 = obj != null;
                if (flag5)
                {
                    SpouseServiceItemVM spouseServiceItemVM2 = new SpouseServiceItemVM(obj, new Action<SpouseServiceItemVM>(OnSelectedSpouse));
                    _spouseViews.Add(spouseServiceItemVM2);
                    bool flag6 = obj == Hero.MainHero.Spouse;
                    if (flag6)
                    {
                        _lastPrimaryView = spouseServiceItemVM2;
                    }
                }
            });
            bool flag3 = _spouses.Count > 0;
            if (flag3)
            {
                _selectedHero = _spouses.First();
                SpouseServiceItemVM spouseServiceItemVM = _spouseViews.First();
                spouseServiceItemVM.IsSelected = true;
                OnSelectedSpouse(spouseServiceItemVM);
            }
            bool flag4 = _spouses.Count == 0;
            if (flag4)
            {
                IsNotPrimarySpouse = false;
            }
            OnPropertyChanged("Spouses");
            OnPropertyChanged("ShowCharacterView");
            OnPropertyChanged("CanDivorce");
        }

        public void OnSelectedSpouse(SpouseServiceItemVM spouseItemVM)
        {
            bool flag = spouseItemVM != _currentSpouseView;
            if (flag)
            {
                bool flag2 = _currentSpouseView != null;
                if (flag2)
                {
                    _currentSpouseView.IsSelected = false;
                }
                _currentSpouseView = spouseItemVM;
                _selectedHero = spouseItemVM.Hero;
                bool flag3 = SelectedCharacter == null;
                if (flag3)
                {
                    SelectedCharacter = new SpouseCharacterVM(SpouseCharacterVM.StanceTypes.None);
                }
                SelectedCharacter.FillFrom(_selectedHero.CharacterObject, -1);
                CanGetPregnancy = !_selectedHero.IsPregnant;
                IsNotPrimarySpouse = _selectedHero != Hero.MainHero.Spouse;
            }
        }

        public void ExecuteDivorce()
        {
            bool flag = _selectedHero == null;
            if (!flag)
            {
                TextObject textObject = GameTexts.FindText("sms_divorce_sure", null);
                StringHelpers.SetCharacterProperties("SUE_HERO", _selectedHero.CharacterObject, textObject);
                InquiryData data = new InquiryData(textObject.ToString(), string.Empty, true, true, GameTexts.FindText("sms_sure", null).ToString(), GameTexts.FindText("sms_cancel", null).ToString(), delegate
                {
                    SpouseOperation.Divorce(_selectedHero);
                    RefreshSpouseViews();
                }, null, "");
                InformationManager.ShowInquiry(data, false);
            }
        }

        public void ExecuteDate()
        {
            string battleSceneForMapPosition = PlayerEncounter.GetBattleSceneForMapPosition(MobileParty.MainParty.Position2D);
            List<Hero> list = new List<Hero>();
            list.AddRange(_spouses);
            list.Add(Hero.MainHero);
            SpousesMissons.OpenDateMission(battleSceneForMapPosition, list);
        }

        public void ExecutePregnancy()
        {
            bool isPregnant = _selectedHero.IsPregnant;
            if (!isPregnant)
            {
                SpouseOperation.GetPregnancyForHero(Hero.MainHero, _selectedHero);
                bool flag = Hero.MainHero.IsFemale && !Hero.MainHero.IsPregnant;
                if (flag)
                {
                    SpouseOperation.GetPregnancyForHero(_selectedHero, Hero.MainHero);
                }
                CanGetPregnancy = false;
                bool flag2 = _currentSpouseView != null;
                if (flag2)
                {
                    _currentSpouseView.RefreshValues();
                }
            }
        }

        public void SetPrimarySpouse()
        {
            bool flag = _selectedHero == Hero.MainHero.Spouse;
            if (!flag)
            {
                SpouseOperation.SetPrimarySpouse(_selectedHero);
                IsNotPrimarySpouse = false;
                bool flag2 = _currentSpouseView != null;
                if (flag2)
                {
                    _currentSpouseView.RefreshValues();
                }
                bool flag3 = _lastPrimaryView != null;
                if (flag3)
                {
                    _lastPrimaryView.RefreshValues();
                }
                _lastPrimaryView = _currentSpouseView;
            }
        }

        public void ActionStand()
        {
        }

        public void ExecuteFaceDetailsCreator()
        {
            FaceGen.ShowDebugValues = true;
            FaceDetailsCreatorState gameState = Game.Current.GameStateManager.CreateState<FaceDetailsCreatorState>(new object[]
            {
                _selectedHero
            });
            Game.Current.GameStateManager.PushState(gameState, 0);
        }

        public void ActionCelebrateVictory()
        {
        }

        public new void OnFinalize()
        {
            base.OnFinalize();
            bool flag = _selectedCharacter != null;
            if (flag)
            {
                _selectedCharacter.OnFinalize();
            }
            _selectedCharacter = null;
        }
    }
}
