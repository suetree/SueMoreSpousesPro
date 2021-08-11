using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	public class ADOptionsScreenWidget : OptionsScreenWidget
	{
		private Widget _currentOptionWidget;

		private bool _initialized;

		public ADOptionsScreenWidget(UIContext context) : base(context)
		{
		}

		protected override void OnUpdate(float dt)
		{
			bool flag = !this._initialized;
			if (flag)
			{
				using (IEnumerator<Widget> enumerator = base.AllChildren.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						OptionsItemWidget optionsItemWidget;
						bool flag2 = (optionsItemWidget = (enumerator.Current as OptionsItemWidget)) != null;
						if (flag2)
						{
							optionsItemWidget.SetCurrentScreenWidget(this);
						}
					}
				}
				this._initialized = true;
			}
		}
	}
}
