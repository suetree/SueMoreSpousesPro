using SueMoreSpouses.Data.sp;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace SueMoreSpouses.Data
{
	internal class SpousesBattleRecordSide
	{
		[SaveableProperty(1)]
		internal List<SpousesBattleRecordParty> Parties
		{
			get;
			set;
		}

		[SaveableProperty(2)]
		internal string Name
		{
			get;
			set;
		}

		[SaveableProperty(3)]
		internal Banner Banner
		{
			get;
			set;
		}

		[SaveableProperty(4)]
		internal int KillCount
		{
			get;
			set;
		}

		[SaveableProperty(5)]
		internal int Remain
		{
			get;
			set;
		}

		[SaveableProperty(6)]
		internal int Killed
		{
			get;
			set;
		}

		[SaveableProperty(7)]
		internal int Wounded
		{
			get;
			set;
		}

		[SaveableProperty(8)]
		internal int RunAway
		{
			get;
			set;
		}

		public SpousesBattleRecordSide(string name, Banner banner)
		{
			this.Name = name;
			this.Banner = banner;
			this.Parties = new List<SpousesBattleRecordParty>();
		}

		public SpousesBattleRecordParty GetPartyByUniqueId(string uniqueId)
		{
			SpousesBattleRecordParty result = null;
			bool flag = uniqueId != null;
			if (flag)
			{
				foreach (SpousesBattleRecordParty current in this.Parties)
				{
					bool flag2 = uniqueId == current.UniqueId;
					if (flag2)
					{
						result = current;
						break;
					}
				}
			}
			return result;
		}
	}
}
