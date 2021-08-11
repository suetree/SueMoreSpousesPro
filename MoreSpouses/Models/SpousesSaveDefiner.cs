using SueMoreSpouses.Data.sp;
using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace SueMoreSpouses.Data
{
	internal class SpousesSaveDefiner : SaveableTypeDefiner
	{
		public SpousesSaveDefiner() : base(942285691)
		{
		}

		protected override void DefineClassTypes()
		{
			base.AddClassDefinition(typeof(SpousesHeroStatistic), 2009021);
			base.AddClassDefinition(typeof(SpousesBattleRecord), 2009022);
			base.AddClassDefinition(typeof(SpousesBattleRecordSide), 2009023);
			base.AddClassDefinition(typeof(SpousesBattleRecordParty), 2009024);
			base.AddClassDefinition(typeof(SpousesBattleRecordCharacter), 2009025);
			base.AddClassDefinition(typeof(SpousesBattleRecordReward), 2009026);
		}

		protected override void DefineContainerDefinitions()
		{
			base.ConstructContainerDefinition(typeof(List<SpousesHeroStatistic>));
			base.ConstructContainerDefinition(typeof(List<SpousesBattleRecord>));
			base.ConstructContainerDefinition(typeof(List<SpousesBattleRecordSide>));
			base.ConstructContainerDefinition(typeof(List<SpousesBattleRecordParty>));
			base.ConstructContainerDefinition(typeof(List<SpousesBattleRecordCharacter>));
			base.ConstructContainerDefinition(typeof(List<SpousesBattleRecordReward>));
		}
	}
}
