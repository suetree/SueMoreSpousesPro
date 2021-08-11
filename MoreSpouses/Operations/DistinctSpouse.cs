using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;

namespace SueMoreSpouses.Operation
{
	internal class DistinctSpouse<TModel> : IEqualityComparer<TModel>
	{
		public bool Equals(TModel x, TModel y)
		{
			Hero hero = x as Hero;
			Hero hero2 = y as Hero;
			bool flag = hero != null && hero2 != null;
			return flag && hero.StringId == hero2.StringId;
		}

		public int GetHashCode(TModel obj)
		{
			return obj.ToString().GetHashCode();
		}
	}
}
