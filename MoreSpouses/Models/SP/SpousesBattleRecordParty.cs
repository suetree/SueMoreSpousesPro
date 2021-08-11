using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace SueMoreSpouses.Data.sp
{
	internal class SpousesBattleRecordParty
	{
		[SaveableProperty(1)]
		internal List<SpousesBattleRecordCharacter> Characters
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
		internal string UniqueId
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

		public SpousesBattleRecordParty(string uniqueId)
		{
			this.UniqueId = uniqueId;
			this.Characters = new List<SpousesBattleRecordCharacter>();
		}

		public SpousesBattleRecordCharacter GetBattleRecordCharacter(BasicCharacterObject character)
		{
			SpousesBattleRecordCharacter result;
			foreach (SpousesBattleRecordCharacter current in this.Characters)
			{
				bool flag = current.Character == character;
				if (flag)
				{
					result = current;
					return result;
				}
			}
			result = null;
			return result;
		}

		public void AddSpousesBattleRecordCharacter(SpousesBattleRecordCharacter data)
		{
			this.Characters.Add(data);
		}
	}
}
