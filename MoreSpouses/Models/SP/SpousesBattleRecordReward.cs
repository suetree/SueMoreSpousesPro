using System;
using TaleWorlds.SaveSystem;

namespace SueMoreSpouses.Data.sp
{
	internal class SpousesBattleRecordReward
	{
		[SaveableProperty(1)]
		internal float RenownChange
		{
			get;
			set;
		}

		[SaveableProperty(2)]
		internal float InfluenceChange
		{
			get;
			set;
		}

		[SaveableProperty(3)]
		internal float MoraleChange
		{
			get;
			set;
		}

		[SaveableProperty(4)]
		internal float GoldChange
		{
			get;
			set;
		}

		[SaveableProperty(5)]
		internal float PlayerEarnedLootPercentage
		{
			get;
			set;
		}

		public SpousesBattleRecordReward(float renownChange, float influenceChange, float moraleChange, float goldChange, float playerEarnedLootPercentage)
		{
			this.RenownChange = renownChange;
			this.InfluenceChange = influenceChange;
			this.MoraleChange = moraleChange;
			this.GoldChange = goldChange;
			this.PlayerEarnedLootPercentage = playerEarnedLootPercentage;
		}
	}
}
