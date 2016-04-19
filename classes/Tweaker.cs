using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using TERA_Tweaker.consts;

namespace TERA_Tweaker.classes
{
    class Tweaker
    {
        private string _gameDir;
        private Dictionary<string, IniFile> ConfigFiles;
        private Dictionary<string, FileInfo> PresetConfigs;
        private OptimizationType optimizationLevel;

        public Tweaker(string gameDir)
        {
            _gameDir = gameDir;

            //Init ConfigFiles 
            LoadConfigFiles();
        }

        public bool ResetConfigChanges()
        {
            bool result = false;

            string backupPath = string.Format("{0}\\{1}\\{2}", Directory.GetCurrentDirectory(), BaseConsts.PRESETS_DIR, BaseConsts.UNTOUCHED_S1ENGINE);
            FileInfo file = new FileInfo(backupPath);

            if (file.Exists)
            {
                string currentConfigPath = string.Format("{0}\\{1}\\{2}", _gameDir, BaseConsts.CONFIG_DIR, BaseConsts.S1ENGINE);
                FileInfo currentConfig = new FileInfo(currentConfigPath);

                bool copyIsAllowed;
                LoadPresets();
                if (FileIsEqualWithPreset(currentConfig))
                {
                    //It is one of our presets, just overwrite it and finish the code
                    copyIsAllowed = true;
                }
                else
                {
                    //It's an unknown config which would be overwritten with the backup. Ask the user first
                    copyIsAllowed = AskUserToOverwrite();
                }

                if (copyIsAllowed)
                {
                    //Remove Read-Only
                    RemoveReadOnlyFlagIfSet(currentConfigPath);
                    File.Copy(backupPath, currentConfigPath, true);

                    //Set Read-Only again
                    SetReadOnlyFlag(currentConfigPath);

                    //Delete Backup
                    File.Delete(backupPath);

                    MessageBox.Show("Reset was successfull", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                }
            }
            else
            {
                MessageBox.Show("There is no backup file, which could be used for resetting the changes. Please delete the S1Engine.ini and run the game, if you wan't a clean configuration.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.Info("No backup file found at {0}", backupPath);
            }

            return result;
        }

        private bool AskUserToOverwrite()
        {
            var result = MessageBox.Show("The current configuration isn't a preset of this tool. Are you sure, you want to overwrite it with the backup, which was made by this tool?", "Overwrite?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                return true;
            else
                return false;
        }

        public void ApplyConfigChanges()
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
            //Load the presets
            LoadPresets();

            switch (optimizationLevel)
            {
                case OptimizationType.BestPerformance:
                    ApplyPresetToGame(BaseConsts.PRESET_BEST_PERFORMANCE);
                    break;
                case OptimizationType.GoodPerformance:
                    ApplyPresetToGame(BaseConsts.PRESET_GOOD_PERFORMANCE);
                    break;
                case OptimizationType.Balanced:
                    ApplyPresetToGame(BaseConsts.PRESET_BALANCED);
                    break;
                case OptimizationType.GoodQuality:
                    ApplyPresetToGame(BaseConsts.PRESET_GOOD_QUALITY);
                    break;
                case OptimizationType.BestQuality:
                    ApplyPresetToGame(BaseConsts.PRESET_BEST_QUALITY);
                    break;
                default:
                    Logger.Warn("Unknown OptimizationType for presets: {0}", optimizationLevel);
                    break;
            }
        }

        private void ApplyPresetToGame(string presetFile)
        {
            //First backup the current configuration if necessary
            BackUpUserConfiguration();

            //Now copy our preset to the game 
            string presetPath = PresetConfigs[presetFile].FullName.ToString();
            string gameConfigPath = string.Format("{0}\\{1}\\{2}", _gameDir, BaseConsts.CONFIG_DIR, BaseConsts.S1ENGINE);

            //Remove Read-Only Flag if set
            RemoveReadOnlyFlagIfSet(gameConfigPath);

            //Copy the preset
            File.Copy(presetPath, gameConfigPath, true);

            //Set Read-Only again
            SetReadOnlyFlag(gameConfigPath);
        }

        private void RemoveReadOnlyFlagIfSet(string gameConfigPath)
        {
            var file = new FileInfo(gameConfigPath);
            if (file.IsReadOnly)
                file.IsReadOnly = false;
        }

        private void SetReadOnlyFlag(string gameConfigPath)
        {
            var file = new FileInfo(gameConfigPath);
            file.IsReadOnly = true;
        }

        private void BackUpUserConfiguration()
        {
            string backupPath = string.Format("{0}\\{1}\\{2}", Directory.GetCurrentDirectory(), BaseConsts.PRESETS_DIR, BaseConsts.UNTOUCHED_S1ENGINE);
            FileInfo file = new FileInfo(backupPath);

            string currentConfigPath = string.Format("{0}\\{1}\\{2}", _gameDir, BaseConsts.CONFIG_DIR, BaseConsts.S1ENGINE);
            if (!file.Exists)
            {
                //There is no backup. Just create it
                File.Copy(currentConfigPath, backupPath);
            }
            else
            {
                FileInfo currentConfigFile = new FileInfo(currentConfigPath);

                //There is already a backup. Check if the current configuration is equal with a preset. If not, ask to overwrite it. 
                if (!FileIsEqualWithPreset(currentConfigFile))
                {
                    //Ask if the current backup should be overwritten with the current config file
                    if (OverwriteBackup())
                    {
                        File.Copy(currentConfigPath, backupPath, true);
                    }
                }
            }
        }

        private bool OverwriteBackup()
        {
            bool result;

            string message = "There is already a backup of an previous configuration. Do you want to overwrite it?";
            string caption = "Overwrite?";

            var msgBoxResult = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            switch (msgBoxResult)
            {
                case MessageBoxResult.Yes:
                    result = true;
                    break;
                case MessageBoxResult.No:
                    result = false;
                    break;
                case MessageBoxResult.Cancel:
                    throw new Exception("Cancelled by user");
                default:
                    throw new ArgumentException(string.Format("Invalid Messagebox Result {0}", msgBoxResult.ToString()));
            }

            return result;
        }

        private bool FileIsEqualWithPreset(FileInfo currentConfigFile)
        {
            foreach (var presetFile in PresetConfigs)
            {
                if (FilesAreEqual(currentConfigFile, presetFile.Value))
                    return true;
            }

            return false;
        }

        private bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            FileStream fs1 = first.OpenRead();
            FileStream fs2 = second.OpenRead();

            byte[] firstHash = MD5.Create().ComputeHash(fs1);
            byte[] secondHash = MD5.Create().ComputeHash(fs2);

            for (int i = 0; i < firstHash.Length; i++)
            {
                if (firstHash[i] != secondHash[i])
                {
                    //Close both FileStreams and return the result
                    fs1.Close();
                    fs2.Close();
                    return false;
                }
            }

            //Close both FileStreams and return the result
            fs1.Close();
            fs2.Close();
            return true;
        }

        private void LoadPresets()
        {
            if (PresetConfigs == null)
            {
                PresetConfigs = new Dictionary<string, FileInfo>();

                string[] iniFiles = new string[] { 
                BaseConsts.PRESET_BEST_PERFORMANCE,
                BaseConsts.PRESET_GOOD_PERFORMANCE, 
                BaseConsts.PRESET_BALANCED, 
                BaseConsts.PRESET_GOOD_QUALITY, 
                BaseConsts.PRESET_BEST_QUALITY };

                foreach (var iniFile in iniFiles)
                {
                    var path = string.Format("{0}\\{1}\\{2}", Directory.GetCurrentDirectory(), BaseConsts.PRESETS_DIR, iniFile);
                    if (File.Exists(path))
                        PresetConfigs.Add(iniFile, new FileInfo(path));
                    else
                    {
                        Logger.Warn("File '{0}' doesn't exist in path '{1}'", iniFile, path);
                    }
                }
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
