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
    public class EMOptionGroupVM: ViewModel
    {
        EMOptionGroup _optionGroup;
        MBBindingList<EMOptionGroupItemVM> _optionItemVMs;

        private  Action<EMOptionGroupVM> _onGroupSelected;
        private Action<EMOptionGroupVM> _onGroupEnableChange;

        private bool _isSelected;

        public EMOptionGroupVM(EMOptionGroup optionGroup)
        {
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
                   
                    this._optionGroup.SetEnableAndRefreshOriginalValue(value);
                    base.OnPropertyChanged("BoolValue");
                    if (null != this._onGroupEnableChange)
                    {
                        this._onGroupEnableChange(this);
                    }
                }
            }

            get {
                return this._optionGroup.Enable;
            }
        }

        [DataSourceProperty]
        public bool ShowEnableController
        {
           
            get
            {
                return this._optionGroup.ShowEnableController;
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

        [DataSourceProperty]
        public bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                bool flag = value != this._isSelected;
                if (flag)
                {
                    this._isSelected = value;
                    base.OnPropertyChanged("IsSelected");
                }
            }
        }



        public void OnGroupSelected()
        {
            bool flag = !this.IsSelected;
            if (flag && null != this._onGroupSelected)
            {
                this.IsSelected = true;
                this._onGroupSelected(this);
            }
        }

        public void SetOnGroupSelectedAction(Action<EMOptionGroupVM> groupSelected)
        {
            this._onGroupSelected = groupSelected;
        }

        public void SetOnGroupEnableChangeAction(Action<EMOptionGroupVM> onGroupEnableChange)
        {
            this._onGroupEnableChange = onGroupEnableChange;
        }
    }
}
