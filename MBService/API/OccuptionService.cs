using SueMBService.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace SueMBService.API
{
    public class OccuptionService
    {

        public static void ChangeOccupation0fHero(Hero hero, Occupation occupation)
        {
            if (hero.CharacterObject.Occupation == occupation) return;

            ReflectUtils.ReflectPropertyAndSetValue("Occupation", occupation, hero);
            ChangeOccupation0fCharacter(hero.CharacterObject, occupation);

            if (Occupation.Lord == occupation)
            {
                //hero.IsNoble = true;
            }
            else
            {
                //hero.IsNoble = false;
            }
        } 
     

        /**
         * 修改 Character  职业，要修改两个字段 _originCharacter 和 _originCharacterStringId；
         * Character  职业是从xml 模板里面读取，所以这里修改逻辑就是修改角色的绑定原始模板
         * 
         */
        public static void ChangeOccupation0fCharacter(CharacterObject target, Occupation occupation)
        {
            if (target.Occupation == occupation) return;
            List<CharacterObject> list = CharacterObject.All.Where(obj => obj.Occupation == occupation && obj.IsOriginalCharacter).ToList();
            if (list.Count > 0)
            {
                CharacterObject source = list.OrderBy(_ => Guid.NewGuid()).First();
                CopyOccupation(target, source);
            }
        }

        private static void CopyOccupation(CharacterObject target, CharacterObject source)
        {
           if (target.Occupation == source.Occupation) return;
            ReflectUtils.ReflectFieldAndSetValue("_originCharacter", source, target);
            ReflectUtils.ReflectFieldAndSetValue("_originCharacterStringId", source.StringId, target);
        }


    }
}
