using System;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHistorySPScoreVM : ViewModel
    {
        private string _nameText = "";

        private int _kill;

        private int _dead;

        private int _wounded;

        private int _routed;

        private int _remaining;

        private int _readyToUpgrade;

        private bool _isMainParty;

        private bool _isMainHero;

        [DataSourceProperty]
        public string NameText
        {
            get
            {
                return _nameText;
            }
            set
            {
                bool flag = value != _nameText;
                if (flag)
                {
                    _nameText = value;
                    OnPropertyChanged("NameText");
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
        public bool IsMainParty
        {
            get
            {
                return _isMainParty;
            }
            set
            {
                bool flag = value != _isMainParty;
                if (flag)
                {
                    _isMainParty = value;
                    OnPropertyChanged("IsMainParty");
                }
            }
        }

        [DataSourceProperty]
        public int Kill
        {
            get
            {
                return _kill;
            }
            set
            {
                bool flag = value != _kill;
                if (flag)
                {
                    _kill = value;
                    OnPropertyChanged("Kill");
                }
            }
        }

        [DataSourceProperty]
        public int Dead
        {
            get
            {
                return _dead;
            }
            set
            {
                bool flag = value != _dead;
                if (flag)
                {
                    _dead = value;
                    OnPropertyChanged("Dead");
                }
            }
        }

        [DataSourceProperty]
        public int Wounded
        {
            get
            {
                return _wounded;
            }
            set
            {
                bool flag = value != _wounded;
                if (flag)
                {
                    _wounded = value;
                    OnPropertyChanged("Wounded");
                }
            }
        }

        [DataSourceProperty]
        public int Routed
        {
            get
            {
                return _routed;
            }
            set
            {
                bool flag = value != _routed;
                if (flag)
                {
                    _routed = value;
                    OnPropertyChanged("Routed");
                }
            }
        }

        [DataSourceProperty]
        public int Remaining
        {
            get
            {
                return _remaining;
            }
            set
            {
                bool flag = value != _remaining;
                if (flag)
                {
                    _remaining = value;
                    OnPropertyChanged("Remaining");
                }
            }
        }

        [DataSourceProperty]
        public int ReadyToUpgrade
        {
            get
            {
                return _readyToUpgrade;
            }
            set
            {
                bool flag = value != _readyToUpgrade;
                if (flag)
                {
                    _readyToUpgrade = value;
                    OnPropertyChanged("ReadyToUpgrade");
                }
            }
        }

        public void UpdateScores(string name, int numberRemaining, int killCount, int numberWounded, int numberRouted, int numberKilled, int numberReadyToUpgrade)
        {
            _nameText = name;
            Kill = killCount;
            Dead = numberKilled;
            Wounded = numberWounded;
            Routed = numberRouted;
            Remaining = numberRemaining;
            ReadyToUpgrade = numberReadyToUpgrade;
        }
    }
}
