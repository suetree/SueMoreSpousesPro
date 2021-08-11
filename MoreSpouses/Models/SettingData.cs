
using SueEasyMenu.Models;
using System.Collections.Generic;
using System.Linq;

namespace SueMoreSpouses.Data
{
	internal class SettingData
	{
		public bool ExspouseGetPregnancyEnable
		{
			get;
			set;
		}

		public float ExspouseGetPregnancyDailyChance
		{
			get;
			set;
		}

		public float ExspouseGetPregnancyDurationInDays
		{
			get;
			set;
		}

		public bool ChildrenFastGrowthEnable
		{
			get;
			set;
		}

		public int ChildrenFastGrowthCycleInDays
		{
			get;
			set;
		}

		public int ChildrenFastGrowtStopGrowUpAge
		{
			get;
			set;
		}

		public bool ChildrenSkillFixEnable
		{
			get;
			set;
		}

		public List<EMOptionPair> HeroSelectScope
		{
			get;
			set;
		}

		public EMOptionPair ChildrenFastGrowUpScope
		{
			get;
			set;
		}

		public string ChildrenNamePrefix
		{
			get;
			set;
		}

		public string ChildrenNameSuffix
		{
			get;
			set;
		}

		public bool NPCCharaObjectSkillAuto
		{
			get;
			set;
		}

		public int NPCCharaObjectFromTier
		{
			get;
			set;
		}

		public SettingData()
		{
			this.ChildrenFastGrowtStopGrowUpAge = 18;
			this.ChildrenFastGrowthCycleInDays = 36;
			this.ExspouseGetPregnancyDailyChance = 0.5f;
			this.ExspouseGetPregnancyDurationInDays = 30f;
			this.NPCCharaObjectFromTier = 2;
			this.InitData();
		}

		public void InitData()
		{
			List<EMOptionPair> list = new List<EMOptionPair>();
			list.Add(new EMOptionPair(0, "{=hero_scope_player_related}Player rerelated"));
			list.Add(new EMOptionPair(1, "{=hero_scope_clan_related}Clan related"));
			list.Add(new EMOptionPair(2, "{=hero_scope_kindow_related}Kindom related"));
			list.Add(new EMOptionPair(3, "{=hero_scope_world_related}World related"));
			this.HeroSelectScope = list;
			bool flag = this.ChildrenFastGrowUpScope == null;
			if (flag)
			{
				this.ChildrenFastGrowUpScope = list.First<EMOptionPair>();
			}
		}
	}
}
