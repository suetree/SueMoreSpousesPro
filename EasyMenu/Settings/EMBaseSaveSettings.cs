using Newtonsoft.Json;
using SueEasyMenu.Attributes;
using SueEasyMenu.Models;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace SueEasyMenu.Settings
{
    public abstract class EMBaseSaveSettings
    {
        public List<EMOptionGroup> GenerateSettingsProperties()
        {
            EMOptionBuilder optionBuilder = new EMOptionBuilder();

            PropertyInfo[] properties = GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                object settingObj = propertyInfo.GetValue(this);
                Type propertyType = settingObj.GetType();
                if (settingObj is EMBaseGroupSetting)
                {
                    EMGroupAttribute classIfno = propertyType.GetCustomAttribute<EMGroupAttribute>();
                    EMOptionGroup grou = optionBuilder.BuildGroup((enbale) =>
                    {
                        (settingObj as EMBaseGroupSetting).Enable = (bool)enbale;
                    }, classIfno.Name);
                    grou.ShowEnableController = classIfno.ShowEnableController;
                    grou.Enable = (settingObj as EMBaseGroupSetting).Enable;
                    PropertyInfo[] setttingsProperties = propertyType.GetProperties();
                    foreach (PropertyInfo info in setttingsProperties)
                    {
                        EMGroupItemAttribute proInfo = info.GetCustomAttribute<EMGroupItemAttribute>();
                        if (null != proInfo)
                        {
                            object value = info.GetValue(settingObj);
                            EMOptionItem optionItem = new EMOptionItem((val) =>
                            {
                                info.SetValue(settingObj, val);
                            }, proInfo.DisplayName, value, proInfo.OptionType, proInfo.MinValue, proInfo.MaxValue);
                            if (proInfo.OptionType == EMOptionType.SingleSelectProperty || proInfo.OptionType == EMOptionType.MultipleSelectProperty)
                            {
                                object pair = propertyType.GetProperty(proInfo.SelectItemsKey).GetValue(settingObj);
                                if (pair is List<EMOptionPair>)
                                {
                                    optionItem.SelectItems = pair as List<EMOptionPair>;
                                }
                            }
                            grou.AddOption(optionItem);
                        }
                    }
                }
            }
            return optionBuilder.Groups;

        }

        public void SaveSettingData()
        {
            EMSettingFileManager.Instance.SaveSettingData(this);

        }
       // public abstract EMSettingSaveParams GetSaveParams();

    }
}
