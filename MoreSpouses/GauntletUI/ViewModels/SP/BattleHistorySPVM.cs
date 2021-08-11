using SueMoreSpouses.Data;
using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHistorySPVM : ViewModel
    {
        private SpousesBattleRecord _battleRecord;

        private BattleHistorySPSideVM _attackers;

        private BattleHistorySPSideVM _defenders;

        private BattleHistorySPRewardVM _battleHistoryReward;

        private int _battleResultIndex = -1;

        private string _battleResult;

        private bool _isPalyerWin;

        public SpousesBattleRecord BattleRecord
        {
            get
            {
                return _battleRecord;
            }
            set
            {
                bool flag = value != _battleRecord;
                if (flag)
                {
                    _battleRecord = value;
                    OnPropertyChangedWithValue(value, "BattleRecord");
                }
            }
        }

        [DataSourceProperty]
        public BattleHistorySPRewardVM BattleHistoryReward
        {
            get
            {
                return _battleHistoryReward;
            }
            set
            {
                bool flag = value != _battleHistoryReward;
                if (flag)
                {
                    _battleHistoryReward = value;
                    OnPropertyChangedWithValue(value, "BattleHistoryReward");
                }
            }
        }

        [DataSourceProperty]
        public BattleHistorySPSideVM Attackers
        {
            get
            {
                return _attackers;
            }
            set
            {
                bool flag = value != _attackers;
                if (flag)
                {
                    _attackers = value;
                    OnPropertyChangedWithValue(value, "Attackers");
                }
            }
        }

        [DataSourceProperty]
        public BattleHistorySPSideVM Defenders
        {
            get
            {
                return _defenders;
            }
            set
            {
                bool flag = value != _defenders;
                if (flag)
                {
                    _defenders = value;
                    OnPropertyChangedWithValue(value, "Defenders");
                }
            }
        }

        [DataSourceProperty]
        public bool IsPalyerWin
        {
            get
            {
                return _isPalyerWin;
            }
            set
            {
                bool flag = value != _isPalyerWin;
                if (flag)
                {
                    _isPalyerWin = value;
                    OnPropertyChangedWithValue(value, "IsPalyerWin");
                }
            }
        }

        [DataSourceProperty]
        public int BattleResultIndex
        {
            get
            {
                return _battleResultIndex;
            }
            set
            {
                bool flag = value != _battleResultIndex;
                if (flag)
                {
                    _battleResultIndex = value;
                    OnPropertyChangedWithValue(value, "BattleResultIndex");
                }
            }
        }

        [DataSourceProperty]
        public string BattleResult
        {
            get
            {
                return _battleResult;
            }
            set
            {
                bool flag = value != _battleResult;
                if (flag)
                {
                    _battleResult = value;
                    OnPropertyChangedWithValue(value, "BattleResult");
                }
            }
        }

        public BattleHistorySPVM(SpousesBattleRecord battleRecord)
        {
            _attackers = new BattleHistorySPSideVM();
            _defenders = new BattleHistorySPSideVM();
            BattleHistoryReward = new BattleHistorySPRewardVM(battleRecord.RecordReward);
            SetBattleRecord(battleRecord);
        }

        public void SetBattleRecord(SpousesBattleRecord battleRecord)
        {
            BattleResultIndex = battleRecord.BattleResultIndex;
            BattleResult = battleRecord.BattleResultIndex == 1 ? GameTexts.FindText("str_victory", null).ToString() : GameTexts.FindText("str_defeat", null).ToString();
            IsPalyerWin = battleRecord.BattleResultIndex == 1;
            _battleRecord = battleRecord;
            Attackers.FillHistorySide(battleRecord.AttackerSide);
            Defenders.FillHistorySide(battleRecord.DefenderSide);
            BattleHistoryReward.FillData(battleRecord.RecordReward);
            RefreshValues();
        }
    }
}
