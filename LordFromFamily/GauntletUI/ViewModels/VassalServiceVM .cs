using SandBox.GauntletUI;
using SueLordFromFamily.GauntletUI.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueLordFromFamily.GauntletUI.ViewModels
{
	internal class VassalServiceVM : ViewModel
	{

	private KindomScreenVM parentView;

	private GauntletKingdomScreen parentScreen;

	private Action editClanBanner;

	private MBBindingList<VassalClanVM> _clans;

	private MBBindingList<MemberItemVM> _members;

	[DataSourceProperty]
	public MBBindingList<VassalClanVM> Clans
	{
		get
		{
			return this._clans;
		}
	}

	[DataSourceProperty]
	public MBBindingList<MemberItemVM> Members
	{
		get
		{
			return this._members;
		}
	}

	[DataSourceProperty]
	public string DisplayName
	{
		get
		{
			return new TextObject("{=sue_clan_create_from_family_clan_service}Clan Service", null).ToString();
		}
	}

	public VassalServiceVM(KindomScreenVM parent, GauntletKingdomScreen parentScreen, Action editClanBanner)
	{
		this.parentView = parent;
		this.parentScreen = parentScreen;
		this.editClanBanner = editClanBanner;
		this._clans = new MBBindingList<VassalClanVM>();
		this._members = new MBBindingList<MemberItemVM>();
		List<Clan> list = Clan.All.Where((clan) => clan.StringId.Contains("sue_clan_")).ToList();
		Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
		bool flag = kingdom.Clans.Count > 1;
		if (flag)
		{
			IEnumerable<Clan> clans = kingdom.Clans;
			IEnumerable<Clan> source = clans.Where((obj) => obj != Clan.PlayerClan);
			source.ToList<Clan>().ForEach(delegate (Clan obj)
			{
				this._clans.Add(new VassalClanVM(obj, new Action<VassalClanVM>(this.OnSelectVassal)));
			});
			Clan clan = source.First<Clan>();
			IEnumerable<Hero> heroes = clan.Heroes;
			heroes.ToList<Hero>().ForEach(delegate (Hero obj)
			{
				this._members.Add(new MemberItemVM(obj, new Action<MemberItemVM>(this.OnSelectMember)));
			});
		}
		this.RefreshValues();
	}

	public override void RefreshValues()
	{
		bool flag = this.Clans != null;
		if (flag)
		{
			List<VassalClanVM> arg_39_0 = this.Clans.ToList<VassalClanVM>();
			arg_39_0.ForEach((obj) => obj.RefreshValues());
		}
	}

	public void OnSelectVassal(VassalClanVM vassalItem)
	{
	}

	public void OnSelectMember(MemberItemVM vassalItem)
	{
	}

	public void EditClanBannar()
	{
		InformationManager.DisplayMessage(new InformationMessage("测试修改封臣"));
		Kingdom kingdom = Hero.MainHero.MapFaction as Kingdom;
		bool flag = kingdom.Clans.Count > 1;
		if (flag)
		{
			IEnumerable<Clan> arg_59_0 = kingdom.Clans;
			Clan clan = arg_59_0.Where((obj) => obj != Clan.PlayerClan).First<Clan>();
			bool flag2 = clan != null;
			if (flag2)
			{
				this.OpenBannerSelectionScreen(clan, clan.Leader);
			}
			else
			{
				InformationManager.DisplayMessage(new InformationMessage("没有封臣"));
			}
		}
		else
		{
			InformationManager.DisplayMessage(new InformationMessage("没有封臣"));
		}
	}

	private void OpenBannerSelectionScreen(Clan clan, Hero hero)
	{
		NewClanBannerEditorState newClanBannerEditorState = new NewClanBannerEditorState(hero.CharacterObject, clan);
		FieldInfo field = hero.CharacterObject.GetType().GetField("GameStateManager", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		bool flag = null != field;
		if (flag)
		{
			field.SetValue(newClanBannerEditorState, Game.Current.GameStateManager);
		}
		bool flag2 = Game.Current.GameStateManager.GameStateManagerListener != null;
		if (flag2)
		{
			Game.Current.GameStateManager.GameStateManagerListener.OnCreateState(newClanBannerEditorState);
		}
		Game.Current.GameStateManager.PushState(newClanBannerEditorState, 0);
	}

	public void ExecuteCloseWindow()
	{
		this.parentView.CloseSettingView();
		this.OnFinalize();
	}

	public new void OnFinalize()
	{
		base.OnFinalize();
		bool flag = Game.Current != null;
		bool flag2 = flag;
		if (flag2)
		{
			Game.Current.AfterTick = (Action<float>)Delegate.Remove(Game.Current.AfterTick, new Action<float>(this.AfterTick));
		}
		this.parentView = null;
	}

	public void AfterTick(float dt)
	{
		bool flag = this.parentView.IsHotKeyPressed("Exit");
		bool flag2 = flag;
		if (flag2)
		{
			this.ExecuteCloseWindow();
		}
	}
}
}
