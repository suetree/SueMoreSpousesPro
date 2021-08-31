using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SueMoreSpouses.Behaviors
{
	internal class SpouseClanLeaderFixBehavior : CampaignBehaviorBase
	{
	


		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTickFixClanLeader));
		}

		public override void SyncData(IDataStore dataStore)
		{
		}

		public void DailyTickFixClanLeader()
		{
			bool flag = Clan.PlayerClan.Leader != Hero.MainHero;
			if (flag)
			{
				Clan.PlayerClan.SetLeader(Hero.MainHero);
				InformationManager.DisplayMessage(new InformationMessage("MoreSpouses: player clan leader fix"));
			}
			IEnumerable<Clan> clans = Clan.All.ToList<Clan>();

			IEnumerable<Clan> source = clans.Where((obj) => obj.IsEliminated && obj.Leader != null && obj.Leader.Clan != obj);
			bool flag2 = source.Count<Clan>() > 0;
			if (flag2)
			{
				for (int i = 0; i < source.Count<Clan>(); i++)
				{
					Clan current = source.ElementAt(i);
					bool flag3 = current.Leader != null && current.Leader.Clan != null && current.Leader.Clan != current;
					if (flag3)
					{
						Hero sourc = current.Leader;
						List<Hero> list = (from obj in current.Heroes
						where obj != sourc
						select obj).ToList<Hero>();
						bool flag4 = list.Count > 0;
						Hero hero;
						if (flag4)
						{
							hero = list.GetRandomElement<Hero>();
							current.SetLeader(hero);
						}
						else
						{
							CharacterObject template = CharacterObject.FindFirst((CharacterObject obj) => obj.Culture == current.Culture && obj.Occupation == Occupation.Lord);
							hero = HeroCreator.CreateSpecialHero(template, sourc.HomeSettlement, null, null, -1);
							hero.ChangeState(Hero.CharacterStates.Dead);
							hero.Clan = current;
							CampaignEventDispatcher.Instance.OnHeroCreated(hero, false);
							current.SetLeader(hero);
						}
						bool flag5 = GameComponent.CampaignEventDispatcher() != null;
						if (flag5)
						{
							GameComponent.CampaignEventDispatcher().OnClanLeaderChanged(sourc, hero);
						}
						string arg_1FC_0 = "MoreSpouses: ";
						TextObject expr_1EB = current.Name;
						InformationManager.DisplayMessage(new InformationMessage(arg_1FC_0 + ((expr_1EB != null) ? expr_1EB.ToString() : null) + " clan leader fix"));
					}
				}
			}
			IEnumerable<Kingdom> kingdoms = Kingdom.All.ToList<Kingdom>();

			IEnumerable<Kingdom> source2 = kingdoms.Where((obj) => obj.RulingClan == Clan.PlayerClan && !obj.Clans.Contains(Clan.PlayerClan));
			bool flag6 = source2.Count<Kingdom>() > 0;
			if (flag6)
			{
				for (int j = 0; j < source2.Count<Kingdom>(); j++)
				{
					Kingdom kingdom = source2.ElementAt(j);
					IEnumerable<Clan> clans2 = kingdom.Clans;
	
					IEnumerable<Clan> source3 = clans2.Where((obj) => !obj.IsEliminated);
					bool flag7 = source3.Count<Clan>() > 0;
					if (flag7)
					{
						kingdom.RulingClan = source3.ToList<Clan>().GetRandomElement<Clan>();
					}
					else
					{
						kingdom.RulingClan = kingdom.Clans.GetRandomElement<Clan>();
					}
					string arg_305_0 = "MoreSpouses: ";
					TextObject expr_2F4 = kingdom.Name;
					InformationManager.DisplayMessage(new InformationMessage(arg_305_0 + ((expr_2F4 != null) ? expr_2F4.ToString() : null) + " kingdom leader fix"));
				}
			}
			bool flag8 = MobileParty.MainParty != null && MobileParty.MainParty.Party != null && MobileParty.MainParty.Party.Owner != null && MobileParty.MainParty.Party.Owner != Hero.MainHero;
			if (flag8)
			{
                //MobileParty.MainParty.Party.SetCustomOwner(Hero.MainHero);
                MobileParty.MainParty.Party.SetCustomOwner (Hero.MainHero);
            }
		}
	}
}
