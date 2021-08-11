using SueMoreSpouses.Data.sp;
using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHistorySPRewardVM : ViewModel
    {
        private float _renownChange;

        private float _influenceChange;

        private float _moraleChange;

        private float _goldChange;

        private float _playerEarnedLootPercentage;

        [DataSourceProperty]
        public string RenownChange
        {
            get
            {
                return _renownChange.ToString("F2");
            }
        }

        [DataSourceProperty]
        public string RenownChangeText
        {
            get
            {
                return new TextObject("{=sms_battle_record_history_reward_renownChange}RenownChange", null).ToString();
            }
        }

        [DataSourceProperty]
        public string InfluenceChange
        {
            get
            {
                return _influenceChange.ToString("F2");
            }
        }

        [DataSourceProperty]
        public string InfluenceChangeText
        {
            get
            {
                return new TextObject("{=sms_battle_record_history_reward_influenceChange}InfluenceChange", null).ToString();
            }
        }

        [DataSourceProperty]
        public string MoraleChange
        {
            get
            {
                return _moraleChange.ToString("F2");
            }
        }

        [DataSourceProperty]
        public string MoraleChangeText
        {
            get
            {
                return new TextObject("{=sms_battle_record_history_reward_MoraleChange}MoraleChange", null).ToString();
            }
        }

        [DataSourceProperty]
        public string GoldChange
        {
            get
            {
                return _goldChange.ToString("F2");
            }
        }

        [DataSourceProperty]
        public string GoldChangeText
        {
            get
            {
                return new TextObject("{=sms_battle_record_history_reward_GoldChange}GoldChange", null).ToString();
            }
        }

        [DataSourceProperty]
        public string PlayerEarnedLootPercentage
        {
            get
            {
                return Math.Round(_playerEarnedLootPercentage, 2).ToString() ?? "";
            }
        }

        [DataSourceProperty]
        public string PlayerEarnedLootPercentageText
        {
            get
            {
                return new TextObject("{=sms_battle_record_history_reward_PlayerEarnedLootPercentage}PlayerEarnedLootPercentage", null).ToString();
            }
        }

        public BattleHistorySPRewardVM(SpousesBattleRecordReward battleRecordReward)
        {
            bool flag = battleRecordReward != null;
            if (flag)
            {
                FillData(battleRecordReward);
            }
        }

        public void FillData(SpousesBattleRecordReward battleRecordReward)
        {
            bool flag = battleRecordReward == null;
            if (flag)
            {
                battleRecordReward = new SpousesBattleRecordReward(0f, 0f, 0f, 0f, 0f);
            }
            _renownChange = battleRecordReward.RenownChange;
            _influenceChange = battleRecordReward.InfluenceChange;
            _moraleChange = battleRecordReward.MoraleChange;
            _goldChange = battleRecordReward.GoldChange;
            _playerEarnedLootPercentage = battleRecordReward.PlayerEarnedLootPercentage;
            OnPropertyChanged("RenownChange");
            OnPropertyChanged("InfluenceChange");
            OnPropertyChanged("MoraleChange");
            OnPropertyChanged("GoldChange");
            OnPropertyChanged("PlayerEarnedLootPercentage");
        }
    }
}
