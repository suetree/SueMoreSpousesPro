using SueMoreSpouses.Data;
using System;
using TaleWorlds.Library;

namespace SueMoreSpouses.GauntletUI.ViewModels
{
    internal class BattleHistorySPCharacterVM : ViewModel
    {
        private SpousesBattleRecordCharacter _battleRecordCharacter;

        private BattleHistorySPScoreVM _score;

        [DataSourceProperty]
        public BattleHistorySPScoreVM Score
        {
            get
            {
                return _score;
            }
        }

        public BattleHistorySPCharacterVM(SpousesBattleRecordCharacter character)
        {
            _battleRecordCharacter = character;
            _score = new BattleHistorySPScoreVM();
            Score.UpdateScores(character.Character.Name.ToString(), character.Remain, character.KillCount, character.Wounded, character.RunAway, character.Killed, 0);
        }
    }
}
