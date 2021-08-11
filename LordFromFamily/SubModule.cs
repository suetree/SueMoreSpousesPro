using HarmonyLib;
using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using SueLordFromFamily.Behaviors;

namespace SueLordFromFamily
{
    public class SubModule : MBSubModuleBase
    {

        protected override void OnSubModuleLoad()
        {
            Harmony harmony = new Harmony("sue.mod.mb2.SueLordFromFamily");
            harmony.PatchAll();
        }

        protected override void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
        {

            if (gameStarterObject.GetType() == typeof(CampaignGameStarter))
            {
                (gameStarterObject as CampaignGameStarter).AddBehavior(new LordFromFamilyBehavior());
            }
        }
    }
}
