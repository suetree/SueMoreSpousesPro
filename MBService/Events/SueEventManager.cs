using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace SueMBService.Events
{
    public class SueEventManager
    {

        private static SueEventManager _instance = new SueEventManager();

        private readonly SueBaseEvent<CampaignGameStarter> _onGameLoadedAfterEvent = new SueBaseEvent<CampaignGameStarter>();

        public  SueBaseEvent<CampaignGameStarter> OnGameLoadedAfterEvent
        {
            get
            {
                return SueEventManager.Instance._onGameLoadedAfterEvent;
            }
        }

        public static SueEventManager Instance
        {
            get { return _instance; }
        }


        public  void OnGameLoadedAfter(CampaignGameStarter campaignGameStarter)
        {
            this._onGameLoadedAfterEvent.Excute(campaignGameStarter);
        }

    }
}
