using SueMoreSpouses.Data;
using SueMoreSpouses.Data.sp;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHistorySPPartyVM : ViewModel
    {


        private SpousesBattleRecordParty _party;

        private MBBindingList<BattleHistorySPCharacterVM> _members;

        private BattleHistorySPScoreVM _score;

        [DataSourceProperty]
        public BattleHistorySPScoreVM Score
        {
            get
            {
                return _score;
            }
        }

        [DataSourceProperty]
        public MBBindingList<BattleHistorySPCharacterVM> Members
        {
            get
            {
                return _members;
            }
        }

        public BattleHistorySPPartyVM(SpousesBattleRecordParty party)
        {
            _party = party;
            _score = new BattleHistorySPScoreVM();
            Score.UpdateScores(party.Name, party.Remain, party.KillCount, party.Wounded, party.RunAway, party.Killed, 0);
            _members = new MBBindingList<BattleHistorySPCharacterVM>();
            bool flag = _party.Characters != null && _party.Characters.Count > 0;
            if (flag)
            {

                _party.Characters.Sort((x, y) => -1 * x.KillCount.CompareTo(y.KillCount));
                _party.Characters.ForEach(delegate (SpousesBattleRecordCharacter obj)
                {
                    bool flag2 = obj != null;
                    if (flag2)
                    {
                        _members.Add(new BattleHistorySPCharacterVM(obj));
                    }
                });
            }
        }
    }
}
