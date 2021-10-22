using SandBox.View.Menu;
using SueMBService.Events;
using SueMBService.Utils;
using SueMoreSpouses.GauntletUI.ViewModels;
using SueMoreSpouses.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Overlay;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace SueMoreSpouses.Behaviors
{
    internal class SpousesSneakBehavior : CampaignBehaviorBase
	{
		private enum SneakType
		{
			LordShall,
			Center,
			Tavern,
			Prison
		}


		private int _alertCoefficient = 0;

		private int AlertRate = 2;

		private int _alertCoefficientReduceWeek = 0;

		private CampaignGameStarter _gameStarter;

		private IEnumerable<MobileParty> _parties;

		private IEnumerable<Hero> _lordHerosWithOutParty;

		private MobileParty _tempMainMobile;

		private List<CharacterObject> _prisoners;

		private int AlertReducWeekPeriod = 1;

		private int SneakMaxNum = 20;

		private int EscapeAlertNum = 10;

		private SpousesSneakBehavior.SneakType _sneakType = SpousesSneakBehavior.SneakType.LordShall;

		private MobileParty _tempTargetParty;

		private Settlement _lastSettlement;

		public override void RegisterEvents()
		{

			//CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.AddGameMenu));
			SueEventManager.Instance.OnGameLoadedAfterEvent.AddNonSerializedListener(this, this.AddGameMenu);
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, this.AddGameMenu);
			CampaignEvents.MapEventEnded.AddNonSerializedListener(this, new Action<MapEvent>(this.OnPlayerBattleEnd));
		}

		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<int>("AlertCoefficient", ref this._alertCoefficient);
			dataStore.SyncData<int>("AlertCoefficientWeek", ref this._alertCoefficientReduceWeek);
		}

		private void OnPlayerBattleEnd(MapEvent mapEvent)
		{
			bool isPlayerMapEvent = mapEvent.IsPlayerMapEvent;
			if (isPlayerMapEvent)
			{
				bool flag = this._tempMainMobile != null;
				if (flag)
				{
					ICollection<TroopRosterElement> collection = this._tempMainMobile.MemberRoster.RemoveIf((obj) => obj.Character != null);
					foreach (TroopRosterElement current in collection)
					{
						bool isHero = current.Character.IsHero;
						if (isHero)
						{
							MobileParty.MainParty.MemberRoster.AddToCounts(current.Character, 1, false, 0, 0, true, -1);
						}
						else
						{
							MobileParty.MainParty.MemberRoster.AddToCounts(current.Character, current.Number, false, 0, 0, true, -1);
						}
					}
                    //MobileParty.MainParty.Party.SetCustomOwner(Hero.MainHero);
                    MobileParty.MainParty.Party.SetCustomOwner (Hero.MainHero);
                    bool flag2 = mapEvent.WinningSide == mapEvent.PlayerSide;
					if (flag2)
					{
						this.AlertCoefficientIncrease();
						bool flag3 = this._sneakType == SpousesSneakBehavior.SneakType.Prison;
						if (flag3)
						{
							bool flag4 = this._prisoners != null && this._prisoners.Count > 0;
							if (flag4)
							{
								foreach (CharacterObject current2 in this._prisoners)
								{
									bool isHero2 = current2.IsHero;
									if (isHero2)
									{
										PartyBase partyBelongedToAsPrisoner = current2.HeroObject.PartyBelongedToAsPrisoner;
										bool flag5 = partyBelongedToAsPrisoner != null;
										if (flag5)
										{
											partyBelongedToAsPrisoner.MemberRoster.AddToCounts(current2, -1, false, 0, 0, true, -1);
											bool flag6 = current2.HeroObject.Clan == Clan.PlayerClan;
											if (flag6)
											{
												current2.HeroObject.ChangeState(Hero.CharacterStates.Active);
												MobileParty.MainParty.MemberRoster.AddToCounts(current2, 1, false, 0, 0, true, -1);
												string arg_1FE_0 = this.ShowTextObject("{=sms_sneak_rescue_successful}Successful rescue");
												TextObject expr_1F2 = current2.HeroObject.Name;
												InformationManager.DisplayMessage(new InformationMessage(arg_1FE_0 + ((expr_1F2 != null) ? expr_1F2.ToString() : null), Colors.Green));
											}
											else
											{
												MobileParty.MainParty.AddPrisoner(current2, 1);
											}
										}
									}
								}
								this.RemovePrisonInSettlement();
							}
						}
					}
					else
					{
						this.AlertCoefficientReduce();
						bool flag7 = this._tempTargetParty != null;
						if (flag7)
						{
							bool flag8 = this._tempTargetParty.PrisonRoster.Count > 0;
							if (flag8)
							{
								bool flag9 = this._lastSettlement != null && this._lastSettlement.Parties.Count > 0;
								if (flag9)
								{
									ICollection<TroopRosterElement> collection2 = this._tempTargetParty.PrisonRoster.RemoveIf((obj) => obj.Character != null);
									foreach (TroopRosterElement current3 in collection2)
									{
										bool flag10 = current3.Character != Hero.MainHero.CharacterObject;
										if (flag10)
										{
											this._lastSettlement.Parties[0].PrisonRoster.AddToCounts(current3.Character, 1, false, 0, 0, true, -1);
										}
									}
								}
								else
								{
									this._tempTargetParty.PrisonRoster.Reset();
								}
							}
						}
					}
					this._tempTargetParty.RemoveParty();
					this._tempTargetParty = null;
					this._tempMainMobile.RemoveParty();
					this._tempMainMobile = null;
					this._lastSettlement = null;
				}
			}
		}

		private void RemovePrisonInSettlement()
		{
			List<PartyBase> list = new List<PartyBase>
			{
				this._lastSettlement.GetComponent<Town>().Owner
			};
			foreach (MobileParty current in this._lastSettlement.GetComponent<Town>().Owner.Settlement.Parties)
			{
				bool flag = current.IsCommonAreaParty || current.IsGarrison;
				if (flag)
				{
					list.Add(current.Party);
				}
			}
			foreach (PartyBase current2 in list)
			{
				foreach (CharacterObject current3 in this._prisoners)
				{
					bool isHero = current3.IsHero;
					if (isHero)
					{
						bool flag2 = current2.PrisonRoster.FindIndexOfTroop(current3) >= 0;
						if (flag2)
						{
							current3.HeroObject.ChangeState(Hero.CharacterStates.Active);
							current2.PrisonRoster.RemoveTroop(current3, 1, default(UniqueTroopDescriptor), 0);
						}
					}
				}
			}
		}

		private void AlertCoefficientReduce()
		{
			this._alertCoefficient--;
			bool flag = this._alertCoefficient < 0;
			if (flag)
			{
				this._alertCoefficient = 0;
			}
			InformationManager.DisplayMessage(new InformationMessage(this.ShowTextObject("{=sms_sneak_alarm_value}Alarm Value") + ": " + this._alertCoefficient.ToString(), Colors.Green));
		}

		private void AlertCoefficientIncrease()
		{
			this._alertCoefficient++;
			bool flag = this._alertCoefficient > 10;
			if (flag)
			{
				this._alertCoefficient = 10;
			}
			InformationManager.DisplayMessage(new InformationMessage(this.ShowTextObject("{=sms_sneak_alarm_value}Alarm Value") + ": " + this._alertCoefficient.ToString(), Colors.Red));
			bool flag2 = this._alertCoefficient >= this.EscapeAlertNum;
			if (flag2)
			{
			}
		}

		public void AddGameMenu(CampaignGameStarter gameStarter)
		{
			this._gameStarter = gameStarter;
			gameStarter.AddGameMenuOption("town", "sms_sneak", "{=sms_sneak_on_secret_mission}Go on a secret mission", new GameMenuOption.OnConditionDelegate(this.ShowCondition), new GameMenuOption.OnConsequenceDelegate(this.SwitchStealMenuStart), false, 3, false);
			gameStarter.AddGameMenuOption("castle", "sms_sneak", "{=sms_sneak_on_secret_mission}Go on a secret mission", new GameMenuOption.OnConditionDelegate(this.ShowCondition), new GameMenuOption.OnConsequenceDelegate(this.SwitchStealMenuStart), false, 3, false);
			gameStarter.AddGameMenu("sms_sneak", "{=sms_sneak_menu_describe}Attack a Lord and take him prisoner. Attack the dungeon and save your companions", new OnInitDelegate(this.MenuInit), GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.none, null);
			gameStarter.AddGameMenuOption("sms_sneak", "sms_sneak_lords_shall", "{=sms_sneak_attack_lord}Attack the Lord", new GameMenuOption.OnConditionDelegate(this.HasLordWithOutParty), new GameMenuOption.OnConsequenceDelegate(this.SwitchStealMenuLordWithoutParty), false, -1, false);
			gameStarter.AddGameMenuOption("sms_sneak", "sms_sneak_party_prison", "{=sms_sneak_attack_dungeon}Attack the dungeon", new GameMenuOption.OnConditionDelegate(this.HasPrison), new GameMenuOption.OnConsequenceDelegate(this.BattleInPrison), false, -1, false);
			gameStarter.AddGameMenuOption("sms_sneak", "sms_sneak_leave", "{=sms_sneak_leave}Leave", null, new GameMenuOption.OnConsequenceDelegate(this.Leave), true, -1, false);
		}

		private void MenuInit(MenuCallbackArgs args)
		{
			this._lastSettlement = Settlement.CurrentSettlement;

			this._parties = Settlement.CurrentSettlement.Parties.Where((obj) => obj.ActualClan != null && obj.ActualClan != Clan.PlayerClan);
	
			this._lordHerosWithOutParty = Settlement.CurrentSettlement.HeroesWithoutParty.Where((obj) => obj.CharacterObject.Occupation == Occupation.Lord && obj.Clan != Clan.PlayerClan);
			bool flag = this._prisoners != null;
			if (flag)
			{
				this._prisoners.Clear();
			}
			bool flag2 = Settlement.CurrentSettlement.GetComponent<Town>() != null;
			if (flag2)
			{
				int count = this._lastSettlement.Parties.Count;
				this._prisoners = Settlement.CurrentSettlement.GetComponent<Town>().GetPrisonerHeroes();
			}
			GameMenuManager gameMenuManager = ReflectUtils.ReflectField<GameMenuManager>("_gameMenuManager", this._gameStarter);
			bool flag3 = gameMenuManager != null;
			if (flag3)
			{
				Dictionary<string, GameMenu> dictionary = ReflectUtils.ReflectField<Dictionary<string, GameMenu>>("_gameMenus", gameMenuManager);
				List<string> list = dictionary.Keys.ToList<string>();
				List<GameMenu> list2 = dictionary.Values.ToList<GameMenu>();
				for (int i = 1; i < list2.Count; i++)
				{
					bool flag4 = list2[i].StringId == "sms_sneak_party" || list2[i].StringId == "sms_sneak_lord_whitout_party";
					if (flag4)
					{
						dictionary.Remove(list[i]);
					}
				}
			}
			this.MenuLordWithoutPartyInit(args);
		}

		private void MenuPartyInit(MenuCallbackArgs args)
		{
			this._gameStarter.AddGameMenu("sms_sneak_party", "此时，你看到一些部队正在操练， 选择一支部队进行袭击", null, GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.none, null);
			bool flag = this._parties != null && this._parties.Count<MobileParty>() > 0;
			if (flag)
			{
				foreach (MobileParty current in this._parties)
				{
					this._gameStarter.AddGameMenuOption("sms_sneak_party", "sms_sneak_party_name", current.Name.ToString(), null, new GameMenuOption.OnConsequenceDelegate(this.BattleInCenter), false, -1, false);
				}
			}
			this._gameStarter.AddGameMenuOption("sms_sneak_party", "sms_sneak_leave", "{=sms_sneak_leave}Leave", null, new GameMenuOption.OnConsequenceDelegate(this.Leave), true, -1, false);
		}

		private void MenuLordWithoutPartyInit(MenuCallbackArgs args)
		{
			this._gameStarter.AddGameMenu("sms_sneak_lord_whitout_party", "{=sms_sneak_attack_lord_describe}At this point, you see some lords in the hall, with a small number of troops", null, GameOverlays.MenuOverlayType.SettlementWithBoth, GameMenu.MenuFlags.none, null);
			bool flag = this._lordHerosWithOutParty != null && this._lordHerosWithOutParty.Count<Hero>() > 0;
			if (flag)
			{
				foreach (Hero current in this._lordHerosWithOutParty)
				{
					this._gameStarter.AddGameMenuOption("sms_sneak_lord_whitout_party", "sms_sneak_hero_name", current.Name.ToString(), null, new GameMenuOption.OnConsequenceDelegate(this.BattleInLordShall), false, -1, false);
				}
			}
			this._gameStarter.AddGameMenuOption("sms_sneak_lord_whitout_party", "sms_sneak_leave", "{=sms_sneak_leave}Leave", null, new GameMenuOption.OnConsequenceDelegate(this.Leave), true, -1, false);
		}

		public bool ShowCondition(MenuCallbackArgs args)
		{
			return Campaign.Current.IsNight;
		}

		public void SwitchStealMenuStart(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("sms_sneak");
		}

		public void SwitchStealMenuParty(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("sms_sneak_party");
		}

		public void SwitchStealMenuLordWithoutParty(MenuCallbackArgs args)
		{
			GameMenu.SwitchToMenu("sms_sneak_lord_whitout_party");
		}

		public bool HasLordParty(MenuCallbackArgs args)
		{

			return Settlement.CurrentSettlement.Parties.Where((obj) => obj.ActualClan != null && obj.ActualClan != Clan.PlayerClan).Count<MobileParty>() > 0;
		}

		public bool HasLordWithOutParty(MenuCallbackArgs args)
		{
			
		   return Settlement.CurrentSettlement.HeroesWithoutParty.Where((obj) => obj.CharacterObject.Occupation == Occupation.Lord && obj.Clan != Clan.PlayerClan).Count<Hero>() > 0;
		}

		public bool HasTavern(MenuCallbackArgs args)
		{
			return Settlement.CurrentSettlement.IsTown;
		}

		public bool HasPrison(MenuCallbackArgs args)
		{
			List<CharacterObject> prisonerHeroes = Settlement.CurrentSettlement.GetComponent<SettlementComponent>().GetPrisonerHeroes();
			return Settlement.CurrentSettlement.GetComponent<Town>() != null && prisonerHeroes.Count > 0;
		}

		private void BattleInTavern(MenuCallbackArgs args)
		{
			this._sneakType = SpousesSneakBehavior.SneakType.Tavern;
			int upgradeLevel = Settlement.CurrentSettlement.GetComponent<Town>().GetWallLevel();
			string scene = LocationComplex.Current.GetLocationWithId("tavern").GetSceneName(upgradeLevel);
			int settlementUpgradeLevel = Campaign.Current.Models.LocationModel.GetSettlementUpgradeLevel(PlayerEncounter.LocationEncounter);
			this._tempTargetParty = MBObjectManager.Instance.CreateObject<MobileParty>("sms_prison");
			this.AddRandomTroopToParty(this._tempTargetParty);
			this.SelectMainPartyMember(args, delegate
			{
				this.PreBattle(this._tempTargetParty);
				this.OpenBattleJustHero(scene, upgradeLevel);
			}, 20);
		}

		private void BattleInPrison(MenuCallbackArgs args)
		{
			this._sneakType = SpousesSneakBehavior.SneakType.Prison;
			int upgradeLevel = Settlement.CurrentSettlement.GetComponent<Town>().GetWallLevel();
			string scene = LocationComplex.Current.GetLocationWithId("prison").GetSceneName(upgradeLevel);
			scene = "sms_prison";
			this._tempTargetParty = MBObjectManager.Instance.CreateObject<MobileParty>("sms_prison");
			this._tempTargetParty.Party.SetCustomOwner  (Settlement.CurrentSettlement.OwnerClan.Leader) ;
			this.AddRandomTroopToParty(this._tempTargetParty);
			MobileParty arg_C3_0 = this._tempTargetParty;
			TextObject expr_A7 = Settlement.CurrentSettlement.Name;
			arg_C3_0.SetCustomName(new TextObject(((expr_A7 != null) ? expr_A7.ToString() : null) + "监狱警卫队", null));
			this.SelectMainPartyMember(args, delegate
			{
				this.PreBattle(this._tempTargetParty);
				this.OpenBattleJustHero(scene, upgradeLevel);
			}, this.SneakMaxNum);
		}

		public void BattleInLordShall(MenuCallbackArgs args)
		{
			this._sneakType = SpousesSneakBehavior.SneakType.LordShall;
			Hero randomElement = (from obj in this._lordHerosWithOutParty
			where obj.Name.ToString() == args.Text.ToString()
			select obj).ToList<Hero>().GetRandomElement<Hero>();
			bool flag = randomElement == null;
			if (flag)
			{
				PlayerEncounter.LeaveSettlement();
				PlayerEncounter.Finish(true);
			}
			else
			{
				int upgradeLevel = Settlement.CurrentSettlement.GetComponent<Town>().GetWallLevel();
				string scene = LocationComplex.Current.GetLocationWithId("lordshall").GetSceneName(upgradeLevel);
				this._tempTargetParty = randomElement.Clan.CreateNewMobileParty(randomElement);
				this.AddRandomTroopToParty(this._tempTargetParty);
				this.SelectMainPartyMember(args, delegate
				{
					this.PreBattle(this._tempTargetParty);
					this.OpenBattleJustHero(scene, upgradeLevel);
				}, this.SneakMaxNum);
			}
		}

		public void BattleInCenter(MenuCallbackArgs args)
		{
			this._sneakType = SpousesSneakBehavior.SneakType.Center;
			MobileParty randomElement = (from obj in this._parties
			where obj.Name.ToString() == args.Text.ToString()
			select obj).ToList<MobileParty>().GetRandomElement<MobileParty>();
			bool flag = randomElement == null;
			if (flag)
			{
				PlayerEncounter.LeaveSettlement();
				PlayerEncounter.Finish(true);
			}
			else
			{
				int wallLevel = Settlement.CurrentSettlement.GetComponent<Town>().GetWallLevel();
				string sceneName = LocationComplex.Current.GetLocationWithId("center").GetSceneName(wallLevel);
				this.PreBattle(randomElement);
				this.StartBattleNormal(sceneName, wallLevel);
			}
		}

		private bool CanChangeStatusOfTroop(CharacterObject character)
		{
			return !character.IsPlayerCharacter && !character.IsNotTransferableInPartyScreen && !character.IsNotTransferableInHideouts && character.IsHero;
		}

		private void AddRandomTroopToParty(MobileParty targetParty)
		{
			int count = 5 + this.AlertRate * this._alertCoefficient;
	
			CharacterObject randomElement = CharacterObject.All.Where((obj) => obj.IsSoldier && obj.Tier >= 4 && obj.Culture == Settlement.CurrentSettlement.Culture).ToList<CharacterObject>().GetRandomElement<CharacterObject>();
			targetParty.MemberRoster.AddToCounts(randomElement, count, false, 0, 0, true, -1);
		
			randomElement = CharacterObject.All.Where((obj) => obj.IsInfantry && obj.IsSoldier && obj.Tier < 4 && obj.Culture == Settlement.CurrentSettlement.Culture).ToList<CharacterObject>().GetRandomElement<CharacterObject>();
			targetParty.MemberRoster.AddToCounts(randomElement, count, false, 0, 0, true, -1);
		}

		private void SelectMainPartyMember(MenuCallbackArgs args, Action nextStep, int maxNum)
		{
			bool flag = this._tempMainMobile == null;
			if (flag)
			{
				this._tempMainMobile = MBObjectManager.Instance.CreateObject<MobileParty>("sms_sneak_temp_party");
			}
			else
			{
				this._tempMainMobile.MemberRoster.Reset();
			}
			int count = MobileParty.MainParty.MemberRoster.Count;
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			List<Hero> list = new List<Hero>();
			list.Add(Hero.MainHero);
			FlattenedTroopRoster strongestAndPriorTroops = GameComponent.GetStrongestAndPriorTroops(MobileParty.MainParty, maxNum, list);
			troopRoster.Add(strongestAndPriorTroops);
			bool flag2 = this.OpenSlelectTroops(args, troopRoster, maxNum, new Func<CharacterObject, bool>(this.CanChangeStatusOfTroop), delegate(TroopRoster troop)
			{
				this.DealPatyTroop(troop);
				nextStep();
			});
			bool flag3 = !flag2;
			if (flag3)
			{
				TroopRoster memberRoster = MobileParty.MainParty.MemberRoster;
				List<TroopRosterElement> troopRoster2 = memberRoster.GetTroopRoster();
				TroopRoster troopRoster3 = TroopRoster.CreateDummyTroopRoster();
				foreach (TroopRosterElement current in troopRoster2)
				{
					bool flag4 = current.Character.IsHero && !current.Character.IsPlayerCharacter;
					if (flag4)
					{
						bool flag5 = troopRoster3.Count < maxNum;
						if (flag5)
						{
							troopRoster3.AddToCounts(current.Character, current.Number, false, 0, 0, true, -1);
						}
					}
				}
				this.DealPatyTroop(troopRoster3);
				nextStep();
			}
		}

		private void DealPatyTroop(TroopRoster battleTroopRoster)
		{
			TroopRoster memberRoster = MobileParty.MainParty.MemberRoster;
			ICollection<TroopRosterElement> collection = memberRoster.RemoveIf((TroopRosterElement obj) => !battleTroopRoster.Contains(obj.Character));
			foreach (TroopRosterElement current in collection)
			{
				bool isHero = current.Character.IsHero;
				if (isHero)
				{
					this._tempMainMobile.MemberRoster.AddToCounts(current.Character, 1, false, 0, 0, true, -1);
				}
				else
				{
					this._tempMainMobile.MemberRoster.AddToCounts(current.Character, current.Number, false, 0, 0, true, -1);
				}
			}
		}

		private bool OpenSlelectTroops(MenuCallbackArgs args, TroopRoster initialRoster, int maxNum, Func<CharacterObject, bool> canChangeStatusOfTroop, Action<TroopRoster> onDone)
		{
			bool result = false;
			bool flag = args.MenuContext.Handler is MenuViewContext;
			if (flag)
			{
				result = true;
				MenuViewContext menuViewContext = (MenuViewContext)args.MenuContext.Handler;
				MenuView menuView = null;
				menuView = menuViewContext.AddMenuView<SpousesDefaultSelectTroops>(new object[]
				{
					initialRoster,
					maxNum,
					canChangeStatusOfTroop,
					onDone,
					new Action(delegate
					{
						bool flag2 = menuView != null;
						if (flag2)
						{
							menuViewContext.RemoveMenuView(menuView);
						}
					})
				});
			}
			return result;
		}

		private void PreBattle(MobileParty targetParty)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
			StartBattleAction.Apply(targetParty.Party, MobileParty.MainParty.Party);
			PlayerEncounter.RestartPlayerEncounter(MobileParty.MainParty.Party, targetParty.Party, true);
			PlayerEncounter.Update();
		}

		private void StartBattleNormal(string scene, int upgradeLevel)
		{
			string civilianUpgradeLevelTag = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(upgradeLevel);
			GameComponent.OpenBattleNormal(scene, civilianUpgradeLevelTag);
		}

		private void OpenBattleJustHero(string scene, int upgradeLevel)
		{
			string civilianUpgradeLevelTag = Campaign.Current.Models.LocationModel.GetCivilianUpgradeLevelTag(upgradeLevel);
			GameComponent.OpenBattleJustHero(scene, civilianUpgradeLevelTag);
		}

		public void Leave(MenuCallbackArgs args)
		{
			PlayerEncounter.LeaveSettlement();
			PlayerEncounter.Finish(true);
		}

		public void CharacterChangeLocation(Hero hero, Settlement startLocation, Settlement endLocation)
		{
			bool flag = startLocation != null;
			if (flag)
			{
				LeaveSettlementAction.ApplyForCharacterOnly(hero);
			}
			EnterSettlementAction.ApplyForCharacterOnly(hero, endLocation);
			bool flag2 = hero != null;
			if (flag2)
			{
				hero.SpcDaysInLocation = 0;
			}
		}

		private string ShowTextObject(string text)
		{
			return new TextObject(text, null).ToString();
		}
	}
}
