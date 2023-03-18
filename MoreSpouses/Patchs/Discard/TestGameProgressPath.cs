using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace SueMoreSpouses.MoreSpouses.Patchs
{
	//[HarmonyPatch(typeof(PlayerTownVisitCampaignBehavior), "RegisterEvents")]
	internal class TestPath
	{
		public static bool Prefix()
		{
			bool result = true;

			return result;
		}
	}
}
