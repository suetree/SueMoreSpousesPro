using Newtonsoft.Json;
using SueEasyMenu.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SueEasyMenu.Settings
{
    public class EMSettingFileManager
    {
        private static EMSettingFileManager _instance;

        private List<EMBaseSaveSettings> _settings = new List<EMBaseSaveSettings>();

        public static EMSettingFileManager Instance
        {
            get
            {
                bool flag = _instance == null;
                if (flag)
                {
                    _instance = new EMSettingFileManager();
                }
                return _instance;
            }
        }

    

        public EMSettingFileManager()
        {
           
        }

        public void SaveSettingsToLocal<T>() where T : EMBaseSaveSettings
        {
            T t = GetSetting<T>();
            SaveSettingData(t);

        }

        public T GetSetting<T>() where T : EMBaseSaveSettings
        {
            T t = default;
            bool has = false;
            foreach (EMBaseSaveSettings setting in this._settings)
            {
                if (setting is T)
                {
                    t = setting as T;
                    has = true;
                }
            }

            if (!has)
            {
                t = LoadSettingData<T>();
                if (null == t)
                {
                    t = (T)Activator.CreateInstance(typeof(T));
                }
                this._settings.Add(t);
            }

            return t;
        }


        public T LoadSettingData<T>() where T : EMBaseSaveSettings
        {
            T t = default;
            PlatformFilePath path = GetSaveParamsByType(typeof(T));
            bool flag = Common.PlatformFileHelper.FileExists(path);
            if (flag)
            {
                string fileContentString = Common.PlatformFileHelper.GetFileContentString(path);
                bool flag2 = fileContentString != null;
                if (flag2)
                {
                    t = (T)JsonConvert.DeserializeObject(fileContentString, typeof(T));
                    //this.SettingData.InitData();
                }
            }
            return t;
        }

        public void SaveSettingData<E>(E t)
        {
            PlatformFilePath path = GetSaveParamsByType(t.GetType());
            string data = JsonConvert.SerializeObject(t, Formatting.Indented);

            SaveResult saveResult = Common.PlatformFileHelper.SaveFileString(path, data);
        }

        private PlatformFilePath GetSaveParamsByType(Type type)
        {
            PlatformFilePath path;
            EMSaveSettingAttribute saveSettingAttribute = type.GetCustomAttribute<EMSaveSettingAttribute>();
            string defaultPath = "SueMoreSpouses";
            string defaultName = type.Name + ".json";
            if (null == saveSettingAttribute)
            {
                path = new PlatformFilePath(EngineFilePaths.ConfigsPath  + "\\"+ defaultPath, defaultName);
            }
            else
            {
                path = new PlatformFilePath(EngineFilePaths.ConfigsPath + "\\" + saveSettingAttribute.SavePath, saveSettingAttribute.SaveName + ".json");
            }
            return path;
        }

    }
}
