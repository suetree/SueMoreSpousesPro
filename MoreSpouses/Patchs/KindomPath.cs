using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SueMoreSpouses.Patch
{
    internal class KindomPath
    {
        private static bool Prefix(Clan clan, Kingdom kingdom, object detail, int awardMultiplier = 0, bool byRebellion = false, bool showNotification = true)
        {
            bool flag = Clan.PlayerClan == clan;
            bool result;
            if (flag)
            {
                bool flag2 = Hero.MainHero.MapFaction != null && Hero.MainHero.MapFaction is Kingdom && Hero.MainHero.IsFactionLeader;
                if (flag2)
                {
                    InformationManager.DisplayMessage(new InformationMessage(" Not allowed MainPlay  to leave the MainPlay's Kindom "));
                    result = false;
                    return result;
                }
            }
            result = true;
            return result;
        }
    }
}
