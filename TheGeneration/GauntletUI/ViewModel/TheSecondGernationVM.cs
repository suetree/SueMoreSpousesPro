using SueEasyMenu.GauntletUI.ViewModels;
using SueEasyMenu.Models;
using SueTheGeneration;
using SueTheGeneration.GauntletUI;
using SueTheGeneration.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueTheGenernation.GauntletUI.ViewModel
{

    class TheSecondGernationVM: TaleWorlds.Library.ViewModel
    {
        private TheGenerationScreen _screen;
        List<EMOptionGroup> _optionGroups;
        MBBindingList<EMOptionGroupVM> optionGroupVMs;
        TheGenerationDataSetting _theSecondGenerationDataSetting;

        public TheSecondGernationVM(TheGenerationScreen screen)
        {
            this._screen = screen;
            this._theSecondGenerationDataSetting = TheGenerationDataSetting.Instance;
            this._optionGroups = this._theSecondGenerationDataSetting.GenerateSettingData();
            optionGroupVMs = new MBBindingList<EMOptionGroupVM>();
            foreach (EMOptionGroup group in this._optionGroups)
            {
                optionGroupVMs.Add(new EMOptionGroupVM(group));
            }
        }


        [DataSourceProperty]
        public MBBindingList<EMOptionGroupVM> Groups
        {
            get
            {
                return this.optionGroupVMs;
            }
        }


        [DataSourceProperty]
        public string DoneLbl
        {
            get
            {
                return new TextObject("{=tsg_complete}Complete").ToString();
            }
        }

        public void ExecuteClose()
        {
            this._screen.CloseScreen();
            TheGenerationAction.PreOnGameStart();
        }

     /*   public void ExecuteClose()
        {
            this._screen.CloseScreen();
            TheGenerationAction.PreOnGameStart();
        }*/
    }
}
