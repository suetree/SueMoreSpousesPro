using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace SueMoreSpouses.Data
{
	internal class SpousesHeroStatistic
	{
		[SaveableProperty(1)]
		internal Hero StatsHero
		{
			get;
			set;
		}

		[SaveableProperty(2)]
		internal int TotalKillCount
		{
			get;
			set;
		}

		[SaveableProperty(3)]
		internal int MVPCount
		{
			get;
			set;
		}

		[SaveableProperty(4)]
		internal int ZeroCount
		{
			get;
			set;
		}

		[SaveableProperty(5)]
		internal int FightCount
		{
			get;
			set;
		}

		public SpousesHeroStatistic(Hero hero)
		{
			this.StatsHero = hero;
		}
	}
}
