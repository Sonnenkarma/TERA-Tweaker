using System.Collections.Generic;
using System.IO;
using TERA_Tweaker.consts;

namespace TERA_Tweaker.classes
{
    class Tweaker
    {
        private string _gameDir;
        private Dictionary<string, IniFile> ConfigFiles;
        private OptimizationType optimizationLevel;

        public Tweaker(string gameDir)
        {
            _gameDir = gameDir;

            //Init ConfigFiles 
            LoadConfigFiles();
        }

        public void ApplyChanges()
        {
            if (optimizationLevel == OptimizationType.Untouched)
                return;

            if (optimizationLevel == OptimizationType.Custom)
            {
                //Using Custom Changes
                ApplyCustomChanges();
            }
            else
            {
                //Using presets
                UsePreset();
            }
        }

        private void ApplyCustomChanges()
        {
            //TODO 
        }

        private void UsePreset()
        {
            switch (optimizationLevel)
            {
                case OptimizationType.BestPerformance:
                    break;
                case OptimizationType.GoodPerformance:
                    break;
                case OptimizationType.Balanced:
                    break;
                case OptimizationType.GoodQuality:
                    break;
                case OptimizationType.BestQuality:
                    break;
                default:
                    Logger.Warn("Unknown OptimizationType for presets: {0}", optimizationLevel);
                    break;
            }
        }

        public void SetOptimizationLevel(OptimizationType optimization)
        {
            optimizationLevel = optimization;
        }

        public string GetValueOfIniFile(string iniFile, string key, string section = null)
        {
            string result = string.Empty;
            if (ConfigFilesContainsFile(iniFile))
            {
                var configFile = ConfigFiles[iniFile];
                result = configFile.Read(key, section);
            }
            return result;
        }

        public void SetValueOfIniFile(string iniFile, string key, string value, string section = null)
        {
            if (ConfigFilesContainsFile(iniFile))
            {
                var configFile = ConfigFiles[iniFile];
                configFile.Write(key, value, section);
            }
        }

        public void DeleteKeyOfIniFile(string iniFile, string key, string section = null)
        {
            if (ConfigFilesContainsFile(iniFile))
            {
                var configFile = ConfigFiles[iniFile];
                configFile.DeleteKey(key, section);
            }
        }

        public void DeleteSectionOfIniFile(string iniFile, string section = null)
        {
            if (ConfigFilesContainsFile(iniFile))
            {
                var configFile = ConfigFiles[iniFile];
                configFile.DeleteSection(section);
            }
        }

        private bool ConfigFilesContainsFile(string iniFile)
        {
            if (ConfigFiles.ContainsKey(iniFile))
            {
                return true;
            }
            else
            {
                Logger.Warn("Config File '{0}' doesn't exist or wasn't loaded properly", iniFile);
                return false;
            }
        }

        private void LoadConfigFiles()
        {
            if (ConfigFiles == null)
                ConfigFiles = new Dictionary<string, IniFile>();

            string[] iniFiles = new string[] { BaseConsts.S1ENGINE, BaseConsts.S1INPUT };

            foreach (var iniFile in iniFiles)
            {
                var path = string.Format("{0}\\{1}\\{2}", _gameDir, BaseConsts.CONFIG_DIR, iniFile);
                if (File.Exists(path))
                    ConfigFiles.Add(iniFile, new IniFile(path));
                else
                {
                    Logger.Warn("File '{0}' doesn't exist in path '{1}'", iniFile, path);
                }
            }
        }

    }
}
