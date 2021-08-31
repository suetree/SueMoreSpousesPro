using TaleWorlds.MountAndBlade;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using HarmonyLib;
using TaleWorlds.Localization;
using System;
using SueTheGeneration.Behavior;
using SueLordFromFamily.Behaviors;
using SueMoreSpouses.Behaviors;
using System.Reflection;
using TaleWorlds.Library;

namespace SueMoreSpouses
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            Harmony harmony = new Harmony("sue.mod.mb2.SueMoreSpouses");
            harmony.PatchAll();

          
        }

    

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            //InformationManager.DisplayMessage(new InformationMessage("SueBloodTies OnGameStart"));
            bool flag = gameStarterObject.GetType() == typeof(CampaignGameStarter);
            if (flag)
            {
                CampaignGameStarter gameInitializer = (CampaignGameStarter)gameStarterObject;
       

                //gameInitializer.AddBehavior(new LordFromFamilyBehavior());

                gameInitializer.LoadGameTexts(string.Format("{0}/Modules/{1}/ModuleData/sue_more_spoues.xml", BasePath.Name, "SueMoreSpouses"));
                gameInitializer.AddBehavior(new SpouseFromPrisonerBehavior());
                gameInitializer.AddBehavior(new SpousesStatsBehavior());
                gameInitializer.AddBehavior(new SpouseClanLeaderFixBehavior());
                gameInitializer.AddBehavior(new SpousesSneakBehavior());
            }
        }


    }
}