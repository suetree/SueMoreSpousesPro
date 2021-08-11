using System;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace SueMoreSpouses.Data
{
	internal class SpousesBattleRecordCharacter
	{
		[SaveableProperty(1)]
		internal BasicCharacterObject Character
		{
			get;
			set;
		}

		[SaveableProperty(2)]
		internal int KillCount
		{
			get;
			set;
		}

		[SaveableProperty(3)]
		internal int Remain
		{
			get;
			set;
		}

		[SaveableProperty(4)]
		internal int Killed
		{
			get;
			set;
		}

		[SaveableProperty(5)]
		internal int Wounded
		{
			get;
			set;
		}

		[SaveableProperty(6)]
		internal int RunAway
		{
			get;
			set;
		}

		public SpousesBattleRecordCharacter(BasicCharacterObject character)
		{
			this.Character = character;
		}
	}
}
