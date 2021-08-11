using SueMoreSpouses.Behaviors;
using System;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpousesBattleStatisticVM : ViewModel
    {

        private SpouseClanVM _parentView;

        private BattleHistoryMainVM _historyMainVM;

        private SpousesStatisticsVM _spousesStatisticsVM;

        private int _tableSelectedIndex = 0;

        [DataSourceProperty]
        public bool IsStatsTableSelected
        {
            get
            {
                return _tableSelectedIndex == 0;
            }
        }

        [DataSourceProperty]
        public string BattleStatisticText
        {
            get
            {
                return new TextObject("{=sms_battle_record_statistic}Battle Statistic", null).ToString();
            }
        }

        [DataSourceProperty]
        public string BattleHistoryText
        {
            get
            {
                return new TextObject("{=sms_battle_record_history}Battle Histroy", null).ToString();
            }
        }

        [DataSourceProperty]
        public string ClanAllDataText
        {
            get
            {
                return new TextObject("{=sms_battle_record_clear_all_data}Clear", null).ToString();
            }
        }

        [DataSourceProperty]
        public bool IsHistoryTableSelected
        {
            get
            {
                return _tableSelectedIndex == 1;
            }
        }

        [DataSourceProperty]
        public BattleHistoryMainVM HistoryMain
        {
            get
            {
                return _historyMainVM;
            }
        }

        [DataSourceProperty]
        public SpousesStatisticsVM SpousesStatistics
        {
            get
            {
                return _spousesStatisticsVM;
            }
        }

        public SpousesBattleStatisticVM(SpouseClanVM parentView)
        {
            _parentView = parentView;
            _historyMainVM = new BattleHistoryMainVM();
            _spousesStatisticsVM = new SpousesStatisticsVM();
        }

        public void SetBattleSelectedCategory(int index)
        {
            bool flag = _tableSelectedIndex != index;
            if (flag)
            {
                _tableSelectedIndex = index;
                OnPropertyChanged("IsStatsTableSelected");
                OnPropertyChanged("IsHistoryTableSelected");
            }
        }

        public void ClanAllRecordData()
        {
            TextObject textObject = GameTexts.FindText("sms_battle_record_clear_all_data_sure", null);
            string arg_6A_0 = textObject.ToString();
            string arg_6A_1 = string.Empty;
            bool arg_6A_2 = true;
            bool arg_6A_3 = true;
            string arg_6A_4 = GameTexts.FindText("sms_sure", null).ToString();
            string arg_6A_5 = GameTexts.FindText("sms_cancel", null).ToString();
            Action arg_6A_6 = delegate
            {
                Campaign.Current.GetCampaignBehavior<SpousesStatsBehavior>().ClanAllData();
                _parentView.CloseSettingView();
            };

            InquiryData data = new InquiryData(arg_6A_0, arg_6A_1, arg_6A_2, arg_6A_3, arg_6A_4, arg_6A_5, arg_6A_6, () => { }, "");
            InformationManager.ShowInquiry(data, false);
        }
    }
}
