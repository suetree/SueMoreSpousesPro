using System;
using TaleWorlds.GauntletUI;

namespace SueMoreSpouses.GauntletUI.Widgets
{
    internal class SpouseCharacterTableauWidget : TextureWidget
    {
        private string _bannerCode;

        private string _bodyProperties;

        private string _charStringId;

        private int _stanceIndex;

        private uint _armorColor1;

        private uint _armorColor2;

        private bool _isFemale;

        private bool _isEquipmentAnimActive;

        private string _equipmentCode;

        private string _mountCreationKey;

        [Editor(false)]
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
                    OnPropertyChanged(value, "BannerCodeText");
                    SetTextureProviderProperty("BannerCodeText", value);
                }
            }
        }

        [Editor(false)]
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
                    OnPropertyChanged(value, "BodyProperties");
                    SetTextureProviderProperty("BodyProperties", value);
                }
            }
        }

        [Editor(false)]
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
                    OnPropertyChanged(value, "CharStringId");
                    SetTextureProviderProperty("CharStringId", value);
                }
            }
        }

        [Editor(false)]
        public int StanceIndex
        {
            get
            {
                return _stanceIndex;
            }
            set
            {
                bool flag = value != _stanceIndex;
                if (flag)
                {
                    _stanceIndex = value;
                    OnPropertyChanged(value, "StanceIndex");
                    SetTextureProviderProperty("StanceIndex", value);
                }
            }
        }

        [Editor(false)]
        public bool IsEquipmentAnimActive
        {
            get
            {
                return _isEquipmentAnimActive;
            }
            set
            {
                bool flag = value != _isEquipmentAnimActive;
                if (flag)
                {
                    _isEquipmentAnimActive = value;
                    OnPropertyChanged(value, "IsEquipmentAnimActive");
                    SetTextureProviderProperty("IsEquipmentAnimActive", value);
                }
            }
        }

        [Editor(false)]
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
                    OnPropertyChanged(value, "IsFemale");
                    SetTextureProviderProperty("IsFemale", value);
                }
            }
        }

        [Editor(false)]
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
                    OnPropertyChanged(value, "EquipmentCode");
                    SetTextureProviderProperty("EquipmentCode", value);
                }
            }
        }

        [Editor(false)]
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
                    OnPropertyChanged(value, "MountCreationKey");
                    SetTextureProviderProperty("MountCreationKey", value);
                }
            }
        }

        [Editor(false)]
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
                    OnPropertyChanged(value, "ArmorColor1");
                    SetTextureProviderProperty("ArmorColor1", value);
                }
            }
        }

        [Editor(false)]
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
                    OnPropertyChanged(value, "ArmorColor2");
                    SetTextureProviderProperty("ArmorColor2", value);
                }
            }
        }

        public SpouseCharacterTableauWidget(UIContext context) : base(context)
        {
            TextureProviderName = "SpouseCharacterTableauTextureProvider";
        }

        protected override void OnMousePressed()
        {
            SetTextureProviderProperty("CurrentlyRotating", true);
        }

        protected override void OnMouseReleased()
        {
            SetTextureProviderProperty("CurrentlyRotating", false);
        }
    }
}
