using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace SueMoreSpouses.GauntletUI.States
{
    internal class FaceDetailsCreatorState : GameState
    {
        public Hero EditHero
        {
            get;
            set;
        }

        public override bool IsMenuState
        {
            get
            {
                return true;
            }
        }

        public FaceDetailsCreatorState()
        {
        }

        public FaceDetailsCreatorState(Hero hero)
        {
            EditHero = hero;
        }
    }
}
