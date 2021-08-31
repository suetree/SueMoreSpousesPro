using HarmonyLib;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using SueTheGeneration.Behavior;

namespace SueTheGeneration
{
    public class SubModule : MBSubModuleBase
    {

        protected override void OnSubModuleLoad()
        {
            Harmony harmony = new Harmony("sue.mod.mb2.SueTheGeneration");
            harmony.PatchAll();
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {

            if (gameStarterObject.GetType() == typeof(CampaignGameStarter))
            {
                (gameStarterObject as CampaignGameStarter).AddBehavior(new TheGenerateBehavior());
            }
        }
    }
}
