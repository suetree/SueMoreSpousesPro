using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace SueMoreSpouses.GauntletUI.Widgets
{
    internal class SPBattleResultTitleBackgroundWidget : Widget
    {
        private int _battleResult;

        private Widget _victoryWidget;

        private Widget _defeatWidget;

        [Editor(false)]
        public int BattleResult
        {
            get
            {
                return _battleResult;
            }
            set
            {
                bool flag = _battleResult != value;
                if (flag)
                {
                    _battleResult = value;
                    OnPropertyChanged(value, "BattleResult");
                    BattleResultUpdated();
                }
            }
        }

        [Editor(false)]
        public Widget VictoryWidget
        {
            get
            {
                return _victoryWidget;
            }
            set
            {
                bool flag = _victoryWidget != value;
                if (flag)
                {
                    _victoryWidget = value;
                    OnPropertyChanged(value, "VictoryWidget");
                }
            }
        }

        [Editor(false)]
        public Widget DefeatWidget
        {
            get
            {
                return _defeatWidget;
            }
            set
            {
                bool flag = _defeatWidget != value;
                if (flag)
                {
                    _defeatWidget = value;
                    OnPropertyChanged(value, "DefeatWidget");
                }
            }
        }

        public SPBattleResultTitleBackgroundWidget(UIContext context) : base(context)
        {
        }

        private void BattleResultUpdated()
        {
            bool flag = BattleResult == 1;
            if (flag)
            {
                DefeatWidget.IsVisible = false;
                VictoryWidget.IsVisible = true;
            }
            else
            {
                DefeatWidget.IsVisible = true;
                VictoryWidget.IsVisible = false;
            }
        }
    }
}
