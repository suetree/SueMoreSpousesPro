using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;

namespace SueEasyMenu.GauntletUI.Widgets
{
	public class EMOptionsDropdownWidget : Widget
	{
		private Action<Widget> _clickHandler;

		private Action<Widget> _listSelectionHandler;

		private Action<Widget> _listItemRemovedHandler;

		private Action<Widget, Widget> _listItemAddedHandler;

		private Vector2 _dropdownOpenPosition;

		private float _animationSpeedModifier = 15f;

		private ButtonWidget _button;

		private ListPanel _listPanel;

		private List<int> _selectedIndexs = new List<int>();

		private Widget _dropdownContainerWidget;

		private Widget _dropdownClipWidget;

		private bool _isOpen;

		private bool _buttonClicked;

		private bool _updateSelectedItem = true;

		private bool _isSingleSelectMode;

		[Editor(false)]
		public RichTextWidget RichTextWidget
		{
			get;
			set;
		}

		[Editor(false)]
		public ButtonWidget Button
		{
			get
			{
				return this._button;
			}
			set
			{
				if (this._button != null)
				{
					this._button.ClickEventHandlers.Remove(this._clickHandler);
				}
				this._button = value;
				if (this._button != null)
				{
					this._button.ClickEventHandlers.Add(this._clickHandler);
				}
				this.RefreshSelectedItem();
			}
		}

		[Editor(false)]
		public Widget DropdownContainerWidget
		{
			get
			{
				return this._dropdownContainerWidget;
			}
			set
			{
				this._dropdownContainerWidget = value;
			}
		}

		[Editor(false)]
		public bool IsSingleSelectMode
		{
			get
			{
				return this._isSingleSelectMode;
			}
			set
			{
				this._isSingleSelectMode = value;
			}
		}

		[Editor(false)]
		public Widget DropdownClipWidget
		{
			get
			{
				return this._dropdownClipWidget;
			}
			set
			{
				this._dropdownClipWidget = value;
				this._dropdownClipWidget.ParentWidget = base.EventManager.Root;
				this._dropdownClipWidget.HorizontalAlignment = HorizontalAlignment.Left;
				this._dropdownClipWidget.VerticalAlignment = VerticalAlignment.Top;
			}
		}

		[Editor(false)]
		public ListPanel ListPanel
		{
			get
			{
				return this._listPanel;
			}
			set
			{
				if (this._listPanel != null)
				{
					this._listPanel.SelectEventHandlers.Remove(this._listSelectionHandler);
					this._listPanel.ItemAddEventHandlers.Remove(this._listItemAddedHandler);
					this._listPanel.ItemRemoveEventHandlers.Remove(this._listItemRemovedHandler);
				}
				this._listPanel = value;
				if (this._listPanel != null)
				{
					this._listPanel.SelectEventHandlers.Add(this._listSelectionHandler);
					this._listPanel.ItemAddEventHandlers.Add(this._listItemAddedHandler);
					this._listPanel.ItemRemoveEventHandlers.Add(this._listItemRemovedHandler);
				}
				this.RefreshSelectedItem();
			}
		}

		[Editor(false)]
		public int ListPanelValue
		{
			get
			{
				if (this.ListPanel != null)
				{
					return this.ListPanel.IntValue;
				}
				return -1;
			}
			set
			{
				if (this.ListPanel != null && this.ListPanel.IntValue != value)
				{
					this.ListPanel.IntValue = value;
				}
			}
		}

		[Editor(false)]
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
					base.OnPropertyChanged(this.SelectedIndexs, "SelectedIndexs");
				}
			}
		}

		[Editor(false)]
		public bool UpdateSelectedItem
		{
			get
			{
				return this._updateSelectedItem;
			}
			set
			{
				if (this._updateSelectedItem != value)
				{
					this._updateSelectedItem = value;
				}
			}
		}

		public EMOptionsDropdownWidget(UIContext context) : base(context)
		{
			this._clickHandler = new Action<Widget>(this.OnButtonClick);
			this._listSelectionHandler = new Action<Widget>(this.OnSelectionChanged);
			this._listItemRemovedHandler = new Action<Widget>(this.OnListChanged);
			this._listItemAddedHandler = new Action<Widget, Widget>(this.OnListChanged);
			
		}

		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._buttonClicked)
			{
				this._buttonClicked = false;
			}
			else if (base.EventManager.LatestMouseUpWidget != this._button && this._isOpen && this.DropdownClipWidget.IsVisible)
			{
                if (this._isSingleSelectMode)
                {
					this.ClosePanel();
				}else if (null != base.EventManager.LatestMouseUpWidget && !this._isSingleSelectMode && !this.ListPanel.Children.Contains(base.EventManager.LatestMouseUpWidget))
                {
					this.ClosePanel();
				}
            }
           
			this.RefreshSelectedItem();
		}

		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isOpen && Vector2.Distance(this.DropdownClipWidget.GlobalPosition, this._dropdownOpenPosition) > 5f)
			{
				this.ClosePanelInOneFrame();
			}
			this.UpdateListPanelPosition(dt);
		}

		private void UpdateListPanelPosition(float dt)
		{
			this.DropdownClipWidget.HorizontalAlignment = HorizontalAlignment.Left;
			this.DropdownClipWidget.VerticalAlignment = VerticalAlignment.Top;
			Vector2 vector = Vector2.One;
			float num;
			if (this._isOpen)
			{
				Widget child = this.DropdownContainerWidget.GetChild(0);
				num = child.Size.Y + child.MarginBottom * base._scaleToUse;
			}
			else
			{
				num = 0f;
			}
			vector = this.Button.GlobalPosition + new Vector2((this.Button.Size.X - this.DropdownClipWidget.Size.X) / 2f, this.Button.Size.Y);
			this.DropdownClipWidget.ScaledPositionXOffset = vector.X;
			this.DropdownClipWidget.ScaledSuggestedHeight = MathF.Lerp(this.DropdownClipWidget.ScaledSuggestedHeight, num, dt * this._animationSpeedModifier, 1E-05f);
			this.DropdownClipWidget.ScaledPositionYOffset = MathF.Lerp(this.DropdownClipWidget.ScaledPositionYOffset, vector.Y, dt * this._animationSpeedModifier, 1E-05f);
			if (!this._isOpen && Math.Abs(this.DropdownClipWidget.ScaledSuggestedHeight - num) < 0.5f)
			{
				this.DropdownClipWidget.IsVisible = false;
				return;
			}
			if (this._isOpen)
			{
				this.DropdownClipWidget.IsVisible = true;
			}
		}

		protected virtual void OpenPanel()
		{
			this._isOpen = true;
			this.DropdownClipWidget.IsVisible = true;
			this._dropdownOpenPosition = this.Button.GlobalPosition + new Vector2((this.Button.Size.X - this.DropdownClipWidget.Size.X) / 2f, this.Button.Size.Y);
		}

		protected virtual void ClosePanel()
		{
			this._isOpen = false;
		}

		private void ClosePanelInOneFrame()
		{
			this._isOpen = false;
			this.DropdownClipWidget.IsVisible = false;
			this.DropdownClipWidget.ScaledSuggestedHeight = 0f;
		}

		public void OnButtonClick(Widget widget)
		{
			this._buttonClicked = true;
			if (this._isOpen)
			{
				this.ClosePanel();
				return;
			}
			this.OpenPanel();
		}

		public void UpdateButtonText(string text)
		{
			if (this.RichTextWidget == null)
			{
				return;
			}
			if (text != null)
			{
				this.RichTextWidget.Text = text;
				return;
			}
			this.RichTextWidget.Text = " ";
		}

		public void OnListChanged(Widget widget)
		{
			this.RefreshSelectedItem();
			this.DropdownContainerWidget.IsVisible = (widget.ChildCount > 1);
		}

		public void OnListChanged(Widget parentWidget, Widget addedWidget)
		{
			this.RefreshSelectedItem();
			this.DropdownContainerWidget.IsVisible = (parentWidget.ChildCount > 0);
		}

		public void OnSelectionChanged(Widget widget)
		{
			if (this.UpdateSelectedItem)
			{
				this.RefreshSelectedItem();
			}
		}

		private void RefreshSelectedItem()
		{
			if (this.UpdateSelectedItem)
			{

				string text = "";
				if (this.SelectedIndexs.Count >= 0 && this.ListPanel != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (int index in SelectedIndexs)
                    {
						Widget child = this.ListPanel.GetChild(index);
						if (child != null)
						{
							using (IEnumerator<Widget> enumerator = child.AllChildren.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									RichTextWidget richTextWidget = enumerator.Current as RichTextWidget;
									if (richTextWidget != null)
									{
										stringBuilder.Append(richTextWidget.Text).Append(" ");
									}
								}
							}
						}
					}
					text = stringBuilder.ToString();


				}
				this.UpdateButtonText(text);
				if (this.ListPanel != null)
				{
					for (int i = 0; i < this.ListPanel.ChildCount; i++)
					{
						Widget child2 = this.ListPanel.GetChild(i);
						(child2 as ButtonWidget).IsSelected = SelectedIndexs.Contains(i);
					}
				}
			}
		}
	}
}
