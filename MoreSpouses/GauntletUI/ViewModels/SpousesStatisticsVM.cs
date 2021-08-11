using SueMoreSpouses.Behaviors;
using SueMoreSpouses.Data;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpousesStatisticsVM : ViewModel
    {
        private MBBindingList<SpouseStatisticsItemVM> _statisticsViews;

        private SelectorVM<SelectorItemVM> _roleTypeVM;

        private int currentRoleType = 0;

        private SelectorVM<SelectorItemVM> _sortTypeVM;

        private int currentSortType = 0;

        private SelectorVM<SelectorItemVM> _orderTypeVM;

        private int currentOrderType = 1;

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> DropdownRoleType
        {
            get
            {
                return _roleTypeVM;
            }
        }

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> DropdownSortType
        {
            get
            {
                return _sortTypeVM;
            }
        }

        [DataSourceProperty]
        public SelectorVM<SelectorItemVM> DropdownOrderType
        {
            get
            {
                return _orderTypeVM;
            }
        }

        [DataSourceProperty]
        public MBBindingList<SpouseStatisticsItemVM> SpouseStatistics
        {
            get
            {
                return _statisticsViews;
            }
        }

        [DataSourceProperty]
        public string NameText
        {
            get
            {
                return new TextObject("{=sms_battle_record_stats_label_name}Name", null).ToString();
            }
        }

        [DataSourceProperty]
        public string TotalKillCountText
        {
            get
            {
                return new TextObject("{=sms_battle_record_stats_label_kill}Kill Count", null).ToString();
            }
        }

        [DataSourceProperty]
        public string MVPCountText
        {
            get
            {
                return new TextObject("{=sms_battle_record_stats_label_mvp}MVP Count", null).ToString();
            }
        }

        [DataSourceProperty]
        public string ZeroCountText
        {
            get
            {
                return new TextObject("{=sms_battle_record_stats_label_zero}Zero Count", null).ToString();
            }
        }

        [DataSourceProperty]
        public string FightCountText
        {
            get
            {
                return new TextObject("{=sms_battle_record_stats_label_fight}Fight Count", null).ToString();
            }
        }

        public SpousesStatisticsVM()
        {
            _statisticsViews = new MBBindingList<SpouseStatisticsItemVM>();
            GenerateStatsData();
            InitRoleTypeData();
            InitSortTypeData();
            InitOrderTypeData();
        }

        private void GenerateStatsData()
        {
            bool flag = _statisticsViews.Count > 0;
            if (flag)
            {
                _statisticsViews.Clear();
            }
            List<SpousesHeroStatistic> list = Campaign.Current.GetCampaignBehavior<SpousesStatsBehavior>().SpousesStats();
            list.Sort(delegate (SpousesHeroStatistic x, SpousesHeroStatistic y)
            {
                int num;
                switch (currentSortType)
                {
                    case 1:
                        num = x.MVPCount.CompareTo(y.MVPCount);
                        break;
                    case 2:
                        num = x.ZeroCount.CompareTo(y.ZeroCount);
                        break;
                    case 3:
                        num = x.FightCount.CompareTo(y.FightCount);
                        break;
                    default:
                        num = x.TotalKillCount.CompareTo(y.TotalKillCount);
                        break;
                }
                bool flag2 = currentOrderType == 1;
                if (flag2)
                {
                    num = -1 * num;
                }
                return num;
            });
            list.ForEach(delegate (SpousesHeroStatistic obj)
            {
                bool flag2 = obj.StatsHero != null;
                if (flag2)
                {
                    bool flag3 = CanAddStatisticsItems(obj);
                    if (flag3)
                    {
                        _statisticsViews.Add(new SpouseStatisticsItemVM(obj));
                    }
                }
            });
            OnPropertyChanged("SpouseStatistics");
        }

        private bool IsSpouse(Hero hero)
        {
            return Hero.MainHero.ExSpouses.Contains(hero) || Hero.MainHero.Spouse != null && hero == Hero.MainHero.Spouse;
        }

        private bool CanAddStatisticsItems(SpousesHeroStatistic spousesStats)
        {
            bool result = false;
            bool flag = currentRoleType != 0;
            if (flag)
            {
                bool flag2 = currentRoleType == 1;
                if (flag2)
                {
                    bool flag3 = IsSpouse(spousesStats.StatsHero);
                    if (flag3)
                    {
                        result = true;
                    }
                }
                else
                {
                    bool flag4 = currentRoleType == 2;
                    if (flag4)
                    {
                        bool isPlayerCompanion = spousesStats.StatsHero.IsPlayerCompanion;
                        if (isPlayerCompanion)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        bool flag5 = currentRoleType == 3;
                        if (flag5)
                        {
                            bool flag6 = Hero.MainHero.Children.Contains(spousesStats.StatsHero);
                            if (flag6)
                            {
                                result = true;
                            }
                        }
                        else
                        {
                            bool flag7 = !Hero.MainHero.Children.Contains(spousesStats.StatsHero) && !IsSpouse(spousesStats.StatsHero) && !spousesStats.StatsHero.IsPlayerCompanion;
                            if (flag7)
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        private void InitRoleTypeData()
        {
            _roleTypeVM = new SelectorVM<SelectorItemVM>(new List<TextObject>
            {
                new TextObject("{=sms_battle_record_stats_role_all}All", null),
                new TextObject("{=sms_battle_record_stats_role_spouse}Spouse", null),
                new TextObject("{=sms_battle_record_stats_role_companion}Companion", null),
                new TextObject("{=sms_battle_record_stats_role_children}Children", null),
                new TextObject("{=sms_battle_record_stats_role_other}Other", null)
            }, currentRoleType, delegate (SelectorVM<SelectorItemVM> item)
            {
                currentRoleType = item.SelectedIndex;
                GenerateStatsData();
            });
        }

        private void InitSortTypeData()
        {
            _sortTypeVM = new SelectorVM<SelectorItemVM>(new List<TextObject>
            {
                new TextObject(TotalKillCountText, null),
                new TextObject(MVPCountText, null),
                new TextObject(ZeroCountText, null),
                new TextObject(FightCountText, null)
            }, currentSortType, delegate (SelectorVM<SelectorItemVM> item)
            {
                currentSortType = item.SelectedIndex;
                GenerateStatsData();
            });
        }

        private void InitOrderTypeData()
        {
            _orderTypeVM = new SelectorVM<SelectorItemVM>(new List<TextObject>
            {
                new TextObject("{=sms_battle_record_stats_order_asc}ASC", null),
                new TextObject("{=sms_battle_record_stats_order_desc}DESC", null)
            }, currentOrderType, delegate (SelectorVM<SelectorItemVM> item)
            {
                currentOrderType = item.SelectedIndex;
                GenerateStatsData();
            });
        }
    }
}
