using SueMoreSpouses.Data;
using SueMoreSpouses.Data.sp;
using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHistorySPSideVM : ViewModel
    {
        private SpousesBattleRecordSide _side;

        private BattleHistorySPScoreVM _sideScore;

        private MBBindingList<BattleHistorySPPartyVM> _parties;

        private ImageIdentifierVM _bannerVisual;

        private ImageIdentifierVM _bannerVisualSmall;

        [DataSourceProperty]
        public BattleHistorySPScoreVM Score
        {
            get
            {
                return _sideScore;
            }
        }

        [DataSourceProperty]
        public ImageIdentifierVM BannerVisual
        {
            get
            {
                return _bannerVisual;
            }
            set
            {
                bool flag = value != _bannerVisual;
                if (flag)
                {
                    _bannerVisual = value;
                    OnPropertyChangedWithValue(value, "BannerVisual");
                }
            }
        }

        [DataSourceProperty]
        public ImageIdentifierVM BannerVisualSmall
        {
            get
            {
                return _bannerVisualSmall;
            }
            set
            {
                bool flag = value != _bannerVisualSmall;
                if (flag)
                {
                    _bannerVisualSmall = value;
                    OnPropertyChangedWithValue(value, "BannerVisualSmall");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<BattleHistorySPPartyVM> Parties
        {
            get
            {
                return _parties;
            }
            set
            {
                bool flag = value != _parties;
                if (flag)
                {
                    _parties = value;
                    OnPropertyChanged("Parties");
                }
            }
        }

        public BattleHistorySPSideVM()
        {
            Parties = new MBBindingList<BattleHistorySPPartyVM>();
            _sideScore = new BattleHistorySPScoreVM();
        }

        public void FillHistorySide(SpousesBattleRecordSide side)
        {
            _side = side;
            Score.UpdateScores(side.Name, side.Remain, side.KillCount, side.Wounded, side.RunAway, side.Killed, 0);
            Parties.Clear();
            bool flag = side.Parties.Count > 0;
            if (flag)
            {
                side.Parties.ForEach(delegate (SpousesBattleRecordParty obj)
                {
                    bool flag3 = obj != null;
                    if (flag3)
                    {
                        Parties.Add(new BattleHistorySPPartyVM(obj));
                    }
                });
            }
            bool flag2 = side.Banner != null;
            if (flag2)
            {
                BannerCode bannerCode = BannerCode.CreateFrom(side.Banner);
                BannerVisual = new ImageIdentifierVM(bannerCode, true);
                BannerVisualSmall = new ImageIdentifierVM(bannerCode, false);
            }
        }
    }
}
