using HarmonyLib;
using SueMBService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace SueLordFromFamily.Paths
{

    /**
     * Just for fix CalradiaExpandedKingdoms Bug
     */
    [HarmonyPatch(typeof(NameGenerator), "GenerateClanName")]
    class CalradiaExpandedKingdomsFixPath
    {
        public static void Prefix(ref NameGenerator __instance, CultureObject culture, Settlement clanOriginSettlement, ref TextObject __result)
        {
            bool IsContainCalradiaExpandedKingdomsMod = IsContainCalradiaExpandedKingdoms();
            if (!IsContainCalradiaExpandedKingdomsMod)
            {
                return;
            }
            if (null == __result)
            {
                __result = GenerateClanName(__instance, culture, clanOriginSettlement);
            }
            if (culture.StringId.ToLower() == "rhodok")
            {
                __result.SetTextVariable("ORIGIN_SETTLEMENT", clanOriginSettlement.Name);
            }
            if (culture.StringId.ToLower() == "apolssalian")
            {
                __result.SetTextVariable("ORIGIN_SETTLEMENT", clanOriginSettlement.Name);
            }
            if (culture.StringId.ToLower() == "lyrion")
            {
                __result.SetTextVariable("ORIGIN_SETTLEMENT", clanOriginSettlement.Name);
            }
            if (culture.StringId.ToLower() == "paleician")
            {
                __result.SetTextVariable("ORIGIN_SETTLEMENT", clanOriginSettlement.Name);
            }
        }

        public static bool IsContainCalradiaExpandedKingdoms()
        {
            bool result = false;
            MBReadOnlyList<CultureObject> cultureObjects = MBObjectManager.Instance.GetObjectTypeList<CultureObject>();
            foreach (CultureObject cul in cultureObjects)
            {
                if (cul.StringId.ToLower() == "rhodok" || cul.StringId.ToLower() == "apolssalian" 
                    || cul.StringId.ToLower() == "lyrion" || cul.StringId.ToLower() == "paleician")
                {
                    result = true;
                    break;
                }
            }
            return result;
        }




        public static TextObject GenerateClanName(NameGenerator instance, CultureObject culture, Settlement clanOriginSettlement)
        {
            TextObject[] arg_18_0 = GetClanNameListForCulture(culture);
            Dictionary<TextObject, int> clanNameWeights = new Dictionary<TextObject, int>();
            TextObject[] array = arg_18_0;
            for (int i = 0; i < array.Length; i++)
            {
                TextObject clanNameElement = array[i];
                if (!clanNameWeights.ContainsKey(clanNameElement))
                {
                    int num = Clan.All.Count((Clan t) => t.Name.Equals(clanNameElement)) * 3;
                    num += Clan.All.Count((Clan t) => t.Name.HasSameValue(clanNameElement));
                    clanNameWeights.Add(clanNameElement, num);
                }
            }
            int maxOccurence = clanNameWeights.Values.Max() + 1;
            int index;
            int num2 = clanNameWeights.Values.Max() + 1;
            List<ValueTuple<TextObject, float>> list = new List<ValueTuple<TextObject, float>>();
            foreach (TextObject textObject1 in clanNameWeights.Keys)
            {
                list.Add(new ValueTuple<TextObject, float>(textObject1, (float)(num2 - clanNameWeights[textObject1])));
            }
            MBRandom.ChooseWeighted<TextObject>(clanNameWeights.Keys, (TextObject t) => (float)(maxOccurence - clanNameWeights[t]), out index);
            TextObject textObject = clanNameWeights.ElementAt(index).Key.CopyTextObject();
            if (culture.StringId.ToLower() == "vlandia")
            {
                textObject.SetTextVariable("ORIGIN_SETTLEMENT", clanOriginSettlement.Name);
            }
            return textObject;
        }

        private static TextObject[] GetClanNameListForCulture(CultureObject clanCulture)
        {
            TextObject[] result = null;
            if (!clanCulture.ClanNameList.IsEmpty<TextObject>())
            {
                result = clanCulture.ClanNameList.ToArray<TextObject>();
            }
            return result;
        }

    }
}
