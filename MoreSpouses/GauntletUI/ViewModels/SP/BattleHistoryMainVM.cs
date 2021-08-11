using SueMoreSpouses.Behaviors;
using SueMoreSpouses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHistoryMainVM : ViewModel
    {
        private MBBindingList<BattleHitoryRecordVM> _battleRecordViews;

        private List<SpousesBattleRecord> _battleRecords;

        private BattleHitoryRecordVM _lastBattleRecordSelected;

        private BattleHistorySPVM _historySP;

        [DataSourceProperty]
        public MBBindingList<BattleHitoryRecordVM> HistoryBattleRecords
        {
            get
            {
                return _battleRecordViews;
            }
        }

        [DataSourceProperty]
        public BattleHistorySPVM HistorySP
        {
            get
            {
                return _historySP;
            }
        }

        public BattleHistoryMainVM()
        {
            InitBattleRecordData();
        }

        public void InitBattleRecordData()
        {
            _battleRecordViews = new MBBindingList<BattleHitoryRecordVM>();
            _battleRecords = Campaign.Current.GetCampaignBehavior<SpousesStatsBehavior>().SpousesBattleRecords();
            _battleRecords.ForEach(delegate (SpousesBattleRecord obj)
            {
                bool flag2 = obj != null;
                if (flag2)
                {
                    _battleRecordViews.Add(new BattleHitoryRecordVM(obj, new Action<BattleHitoryRecordVM>(OnBattleRecordSelected)));
                }
            });
            bool flag = _battleRecordViews.Count > 0;
            if (flag)
            {
                _lastBattleRecordSelected = _battleRecordViews.First();
                _lastBattleRecordSelected.IsSelected = true;
                _historySP = new BattleHistorySPVM(_lastBattleRecordSelected.GetSpousesBattleRecord);
                HistorySP.SetBattleRecord(_lastBattleRecordSelected.GetSpousesBattleRecord);
            }
        }

        private void OnBattleRecordSelected(BattleHitoryRecordVM selectedRecord)
        {
            bool flag = _lastBattleRecordSelected != null;
            if (flag)
            {
                _lastBattleRecordSelected.IsSelected = false;
            }
            _lastBattleRecordSelected = selectedRecord;
            HistorySP.SetBattleRecord(selectedRecord.GetSpousesBattleRecord);
        }
    }
}
