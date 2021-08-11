using SueMoreSpouses.Data;
using System;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class SpouseStatisticsItemVM : ViewModel
    {
        private readonly SpousesHeroStatistic _spousesStats;

        private ImageIdentifierVM _visual;

        private string _name;

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
        public string TotalKillCount
        {
            get
            {
                return _spousesStats.TotalKillCount.ToString() ?? "";
            }
        }

        [DataSourceProperty]
        public string MVPCount
        {
            get
            {
                return _spousesStats.MVPCount.ToString() ?? "";
            }
        }

        [DataSourceProperty]
        public string ZeroCount
        {
            get
            {
                return _spousesStats.ZeroCount.ToString() ?? "";
            }
        }

        [DataSourceProperty]
        public string FightCount
        {
            get
            {
                return _spousesStats.FightCount.ToString() ?? "";
            }
        }

        public SpouseStatisticsItemVM(SpousesHeroStatistic spousesStats)
        {
            _spousesStats = spousesStats;
            CharacterCode characterCode = CampaignUIHelper.GetCharacterCode(spousesStats.StatsHero.CharacterObject, false);
            Visual = new ImageIdentifierVM(characterCode);
            Name = spousesStats.StatsHero.Name.ToString();
        }
    }
}
