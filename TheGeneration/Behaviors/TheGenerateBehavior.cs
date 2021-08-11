using SueTheGeneration.GauntletUI.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SueTheGeneration.Behavior
{
    public class TheGenerateBehavior : CampaignBehaviorBase
    {
        private bool _isGenerated = false;

        public override void RegisterEvents()
        {
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, this.OnHourlyTick);
        }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData<bool>("IsAlreadyGenerated", ref this._isGenerated);
        }

        private void OnHourlyTick()
        {
            if (!this._isGenerated)
            {
                this._isGenerated = true;
                String title = LocationText("{=tsg_event_the_second}The second generation event") ;
                //String describe = "一个一脸疲惫衣衫褴褛的老人瘸着腿慢慢地向你走来。 他走到你跟前，仔细端详了下你的脸。然后说他是你父亲老部下，要求你给他1000块钱，他要回家耕田。并且会告诉你一个秘密。 ";
                String describe = LocationText("{=tsg_event_the_second_describe}An old man with a tired, ragged face comes slowly limping towards you.He comes up to you and looks at your face.Then he said he was an old subordinate of your father and asked you to give him 1000 gold. He wanted to go home and plow the fields.And will tell you a secret.");
                InquiryData inquiryData = new InquiryData(title, describe, true,true, LocationText("{=tsg_event_the_second_aff}To give him gold"), LocationText("{=tsg_event_the_second_neg}To take him off"), ShowSecondSecret, OnNegative);
                InformationManager.ShowInquiry(inquiryData, true);
            }

        }

        private void ShowSecondSecret()
        {
            String title = LocationText("{=tsg_father_heritage}List of Fathers' Heritage");
            //  String describe = "老人递给你了一本目录，上面记录着你父亲生前不为人知的遗产目录";
            String describe = LocationText("{=tsg_father_heritage_describe}The old man hands you a catalog of your father's unknown legacy");
            InquiryData inquiryData = new InquiryData(title, describe, true, true, LocationText("{=tsg_father_heritage_accept}Accept"), LocationText("{=tsg_father_heritage_refuse}Refuse"), OnAffirmative, OnNegative);
            InformationManager.ShowInquiry(inquiryData, true);
        }

        private void OnAffirmative()
        {
            //InformationManager.DisplayMessage(new InformationMessage("恭喜你打开神秘事件"));
            TheGenerationState state = Game.Current.GameStateManager.CreateState<TheGenerationState>();
            Game.Current.GameStateManager.PushState(state, 0);
        }

        private void OnNegative()
        {
            InformationManager.DisplayMessage(new InformationMessage(LocationText("{=tsg_event_the_second_old_faild_leave}The old man walked away, scolding and swearing")));
            ///Hero.MainHero.CharacterObject.hi
        }


        private string LocationText(string str)
        {
            return new TextObject(str).ToString();
        }
    }
}
