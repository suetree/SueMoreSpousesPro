using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpouseCharacterVM : ViewModel
    {
        public enum StanceTypes
        {
            None,
            EmphasizeFace,
            SideView,
            CelebrateVictory,
            OnMount
        }

        private bool _isDead;

        private CharacterObject _character;

        private int _selectedIndex = 0;

        private string _mountCreationKey = "";

        private string _bodyProperties = "";

        private bool _isFemale;

        private int _stanceIndex;

        private uint _armorColor1;

        private uint _armorColor2;

        private string _equipmentCode;

        protected Equipment _equipment;

        private string _charStringId;

        protected string _bannerCode;

        [DataSourceProperty]
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
            set
            {
                bool flag = value != _isDead;
                if (flag)
                {
                    _isDead = value;
                    OnPropertyChanged("IsDead");
                }
            }
        }

        [DataSourceProperty]
        public bool IsBattledSelected
        {
            get
            {
                return _selectedIndex == 1;
            }
        }

        [DataSourceProperty]
        public bool IsCivilizedSelected
        {
            get
            {
                return _selectedIndex == 0;
            }
        }

        [DataSourceProperty]
        public bool IsUnderwearSelected
        {
            get
            {
                return _selectedIndex == 2;
            }
        }

        [DataSourceProperty]
        public string BannerCodeText
        {
            get
            {
                return _bannerCode;
            }
            set
            {
                bool flag = value != _bannerCode;
                if (flag)
                {
                    _bannerCode = value;
                    OnPropertyChangedWithValue(value, "BannerCodeText");
                }
            }
        }

        [DataSourceProperty]
        public string BodyProperties
        {
            get
            {
                return _bodyProperties;
            }
            set
            {
                bool flag = value != _bodyProperties;
                if (flag)
                {
                    _bodyProperties = value;
                    OnPropertyChangedWithValue(value, "BodyProperties");
                }
            }
        }

        [DataSourceProperty]
        public string MountCreationKey
        {
            get
            {
                return _mountCreationKey;
            }
            set
            {
                bool flag = value != _mountCreationKey;
                if (flag)
                {
                    _mountCreationKey = value;
                    OnPropertyChangedWithValue(value, "MountCreationKey");
                }
            }
        }

        [DataSourceProperty]
        public string CharStringId
        {
            get
            {
                return _charStringId;
            }
            set
            {
                bool flag = value != _charStringId;
                if (flag)
                {
                    _charStringId = value;
                    OnPropertyChangedWithValue(value, "CharStringId");
                }
            }
        }

        [DataSourceProperty]
        public int StanceIndex
        {
            get
            {
                return _stanceIndex;
            }
            private set
            {
                bool flag = value != _stanceIndex;
                if (flag)
                {
                    _stanceIndex = value;
                    OnPropertyChangedWithValue(value, "StanceIndex");
                }
            }
        }

        [DataSourceProperty]
        public bool IsFemale
        {
            get
            {
                return _isFemale;
            }
            set
            {
                bool flag = value != _isFemale;
                if (flag)
                {
                    _isFemale = value;
                    OnPropertyChangedWithValue(value, "IsFemale");
                }
            }
        }

        [DataSourceProperty]
        public string EquipmentCode
        {
            get
            {
                return _equipmentCode;
            }
            set
            {
                bool flag = value != _equipmentCode;
                if (flag)
                {
                    _equipmentCode = value;
                    OnPropertyChangedWithValue(value, "EquipmentCode");
                }
            }
        }

        [DataSourceProperty]
        public uint ArmorColor1
        {
            get
            {
                return _armorColor1;
            }
            set
            {
                bool flag = value != _armorColor1;
                if (flag)
                {
                    _armorColor1 = value;
                    OnPropertyChangedWithValue(value, "ArmorColor1");
                }
            }
        }

        [DataSourceProperty]
        public uint ArmorColor2
        {
            get
            {
                return _armorColor2;
            }
            set
            {
                bool flag = value != _armorColor2;
                if (flag)
                {
                    _armorColor2 = value;
                    OnPropertyChangedWithValue(value, "ArmorColor2");
                }
            }
        }

        public SpouseCharacterVM()
        {
        }

        public SpouseCharacterVM(StanceTypes stance = StanceTypes.None)
        {
            _equipment = new Equipment(false);
            EquipmentCode = _equipment.CalculateEquipmentCode();
            StanceIndex = (int)stance;
        }

        public void SetEquipment(EquipmentIndex index, EquipmentElement item)
        {
            _equipment[(int)index] = item;
            EquipmentCode = _equipment.CalculateEquipmentCode();
        }

        public void SetSelectedTab(int index)
        {
            _selectedIndex = index;
            OnPropertyChanged("IsBattledSelected");
            OnPropertyChanged("IsCivilizedSelected");
            OnPropertyChanged("IsUnderwearSelected");
            ChangeClothesBySelectedIndex();
        }

        public void ChangeClothesBySelectedIndex()
        {
            bool flag = _character == null;
            if (!flag)
            {
                int selectedIndex = _selectedIndex;
                int num = selectedIndex;
                if (num != 1)
                {
                    if (num != 2)
                    {
                        FillEquipment(_character.FirstCivilianEquipment);
                    }
                    else
                    {
                        Equipment equipment = new Equipment();
                        equipment[10] = _character.Equipment[10];
                        FillEquipment(equipment);
                    }
                }
                else
                {
                    FillEquipment(_character.FirstBattleEquipment);
                }
            }
        }

        public void FillFrom(CharacterObject character, int seed = -1)
        {
            bool flag = FaceGen.GetMaturityTypeWithAge(character.Age) > BodyMeshMaturityType.Child;
            if (flag)
            {
                _character = character;
                bool flag2 = character.Culture != null;
                if (flag2)
                {
                    ArmorColor1 = character.Culture.Color;
                    ArmorColor2 = character.Culture.Color2;
                }
                CharStringId = character.StringId;
                IsFemale = character.IsFemale;
                ChangeClothesBySelectedIndex();
            }
        }

        private void FillEquipment(Equipment equipment)
        {
            BodyProperties = _character.GetBodyProperties(equipment, -1).ToString();
            MountCreationKey = TaleWorlds.Core.MountCreationKey.GetRandomMountKey(equipment != null ? equipment[10].Item : null, Common.GetDJB2(_character.StringId)).ToString();
            _equipment = equipment != null ? equipment.Clone(false) : null;
            EquipmentCode = equipment != null ? equipment.CalculateEquipmentCode() : null;
        }
    }
}
