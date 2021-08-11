using SueEasyMenu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SueEasyMenu.GauntletUI.ViewModels
{
    public class EMOptionGroupItemGroupVM : ViewModel
    {
        EMOptionGroup _optionGroup;
        MBBindingList<EMOptionGroupItemVM> _optionItemVMs;
        Action<EMOptionGroupItemGroupVM> _onDeleteClickAction;

        public EMOptionGroupItemGroupVM(EMOptionGroup optionGroup, Action<EMOptionGroupItemGroupVM> onDeleteClickAction )
        {
            this._onDeleteClickAction = onDeleteClickAction;
            this._optionGroup = optionGroup;
            this._optionItemVMs = new MBBindingList<EMOptionGroupItemVM>();
            foreach (EMOptionItem item in this._optionGroup.OptionItems)
            {
                this._optionItemVMs.Add(new EMOptionGroupItemVM(item));
            }
        }

        [DataSourceProperty]
        public string Name 
        {

            get {
                return  new TextObject(this._optionGroup.Name, null).ToString(); 
            }
        }

        [DataSourceProperty]
        public string Describe
        {

            get
            {
                return new TextObject(this._optionGroup.Describe, null).ToString();
            }
        }

        [DataSourceProperty]
        public bool BoolValue
        {
            set {
                if (!this._optionGroup.Enable.Equals(value))
                {
                    this._optionGroup.Enable = value;
                    base.OnPropertyChanged("BoolValue");
                }
            }

            get {
                return this._optionGroup.Enable;
            }
        }

        [DataSourceProperty]
        public MBBindingList<EMOptionGroupItemVM> OptionItems
        {
            get
            {
                return this._optionItemVMs;
            }
        }

        public void ExecuteDeleteItemFromGroup()
        {
            if(null != this._onDeleteClickAction)
            {
                this._onDeleteClickAction(this);
            }
        }
    }
}
