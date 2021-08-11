using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueEasyMenu.GauntletUI.ViewModels
{
	public class EMSelectorVM<T> : TaleWorlds.Library.ViewModel where T : SelectorItemVM
    {

		private Action<EMSelectorVM<T>> _onChange;

		private MBBindingList<T> _itemList;


		private bool _hasSingleItem;

		[DataSourceProperty]
		public MBBindingList<T> ItemList
		{
			get
			{
				return this._itemList;
			}
			set
			{
				if (value != this._itemList)
				{
					this._itemList = value;
					base.OnPropertyChangedWithValue(value, "ItemList");
				}
			}
		}
		private List<int> _selectedIndexs = new List<int>();

		[DataSourceProperty]
		public List<int> SelectedIndexs
		{
			get
			{
				return this._selectedIndexs;
			}
			set
			{
				if (this._selectedIndexs != value)
				{
					this._selectedIndexs = value;
					base.OnPropertyChangedWithValue(this.SelectedIndexs, "SelectedIndexs");
				}
			}
		}

		

		[DataSourceProperty]
		public bool HasSingleItem
		{
			get
			{
				return this._hasSingleItem;
			}
			set
			{
				if (value != this._hasSingleItem)
				{
					this._hasSingleItem = value;
					base.OnPropertyChangedWithValue(value, "HasSingleItem");
				}
			}
		}

		public EMSelectorVM( Action<EMSelectorVM<T>> onChange)
		{
			this.ItemList = new MBBindingList<T>();
			this.HasSingleItem = true;
			this._onChange = onChange;
		}

		public EMSelectorVM(IEnumerable<string> list, List<int> selectIndexs, Action<EMSelectorVM<T>> onChange, bool isMutipleSelectMode)
		{
			this.IsMutipleSelectMode = isMutipleSelectMode;
			this.ItemList = new MBBindingList<T>();
			this.Refresh(list, selectIndexs, onChange);
		}

		public EMSelectorVM(IEnumerable<TextObject> list, List<int> selectIndexs , Action<EMSelectorVM<T>> onChange, bool isMutipleSelectMode)
		{
			this.IsMutipleSelectMode = isMutipleSelectMode;
			this.ItemList = new MBBindingList<T>();
			this.Refresh(list, selectIndexs, onChange);
		}

		[DataSourceProperty]
		public bool IsSingleSelectMode
		{
			get
			{
				return !this.IsMutipleSelectMode;
			}

		}

		[DataSourceProperty]
		public bool IsMutipleSelectMode { set; get; }

		public void Refresh(IEnumerable<string> list, List<int> selectedIndexs, Action<EMSelectorVM<T>> onChange)
		{
			this.ItemList.Clear();
			Action<T> test = OnSelectorItemClick;
			foreach (string current in list)
			{
				T item = (T)((object)Activator.CreateInstance(typeof(T), new object[]
				{
					current, test, IsMutipleSelectMode
				}));
				this.ItemList.Add(item);
			}
			Refresh(selectedIndexs, onChange);
		}

		public void Refresh(IEnumerable<TextObject> list, List<int> selectedIndexs, Action<EMSelectorVM<T>> onChange)
		{
			this.ItemList.Clear();
			Action<T> test = OnSelectorItemClick;
			foreach (TextObject current in list)
			{
				T item = (T)((object)Activator.CreateInstance(typeof(T), new object[]
				{
					current, test, IsMutipleSelectMode
				}));
				this.ItemList.Add(item);
			}
			Refresh(selectedIndexs, onChange);
		}

		public void Refresh(List<int> selectedIndexs, Action<EMSelectorVM<T>> onChange)
        {
			this.SelectedIndexs = selectedIndexs;
			this.HasSingleItem = (this.ItemList.Count <= 1);
			this._onChange = onChange;
		}

		public void OnSelectorItemClick(T t)
		{
			int intValue = -1;
			for (int i = 0; i < _itemList.Count; i++)
			{
				if (t == _itemList[i])
				{
					intValue = i;
				}
			}

            if (this.IsMutipleSelectMode)
            {
				if (this.SelectedIndexs.Contains(intValue))
				{
					this.SelectedIndexs.Remove(intValue);
				}
				else
				{
					this.SelectedIndexs.Add(intValue);
				}
            }
            else
            {
				this.SelectedIndexs.Clear();
				this.SelectedIndexs.Add(intValue);

			}

			this._onChange(this);


		}

	

		public void SetOnChangeAction(Action<EMSelectorVM<T>> onChange)
		{
			this._onChange = onChange;
		}

		public void AddItem(T item)
		{
			this.ItemList.Add(item);
			this.HasSingleItem = (this.ItemList.Count <= 1);
		}

		public void ExecuteRandomize()
		{
			MBBindingList<T> expr_06 = this.ItemList;
			bool arg_34_0;
			if (expr_06 == null)
			{
				arg_34_0 = false;
			}
			else
			{
				arg_34_0 = (expr_06.Count(obj => obj.CanBeSelected) > 0);
			}
			if (arg_34_0)
			{

				T randomElement = this.ItemList.GetRandomElementWithPredicate((T i) => i.CanBeSelected);
				//this.SelectedIndex = this.ItemList.IndexOf(randomElement);
			}
		}

		public void ExecuteSelectNextItem()
		{
			if (IsMutipleSelectMode || this.SelectedIndexs.Count == 0) return;
			MBBindingList<T> expr_06 = this.ItemList;
			int currentIndex = this.SelectedIndexs[0];
			if (expr_06 != null && expr_06.Count > 0)
			{
				for (int num = (currentIndex + 1) % this.ItemList.Count; num != currentIndex; num = (num + 1) % this.ItemList.Count)
				{
					if (this.ItemList[num].CanBeSelected)
					{
						currentIndex = num;
						this.SelectedIndexs.Clear();
						this.SelectedIndexs.Add(currentIndex);
						return;
					}
				}
			}
		}

		public void ExecuteSelectPreviousItem()
		{
			if (IsMutipleSelectMode || this.SelectedIndexs.Count == 0) return;
			MBBindingList<T> expr_06 = this.ItemList;
			int currentIndex = this.SelectedIndexs[0];
			if (expr_06 != null && expr_06.Count > 0)
			{
				for (int num = (currentIndex - 1 >= 0) ? (currentIndex - 1) : (this.ItemList.Count - 1); num != currentIndex; num = ((num - 1 >= 0) ? (num - 1) : (this.ItemList.Count - 1)))
				{
					if (this.ItemList[num].CanBeSelected)
					{
						currentIndex = num;
						this.SelectedIndexs.Clear();
						this.SelectedIndexs.Add(currentIndex);
						return;
					}
				}
			}
		}

		public List<T> GetSelectItems()
		{
			List<T> selectes = new List<T>();
            foreach (int index in this.SelectedIndexs)
            {
				selectes.Add(this._itemList[index]);

			}
			return selectes;
		}

		

		public override void RefreshValues()
		{
			base.RefreshValues();
			MBBindingList<T> arg_2B_0 = this._itemList;
			
			arg_2B_0.ApplyActionOnAllItems(obj => obj.RefreshValues());
		}
	}
}
