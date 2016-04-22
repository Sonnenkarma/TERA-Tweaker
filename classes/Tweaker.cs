﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private PartyBuffOptions partyBuffOption;

        public bool removeGunnerAnimations { get; set; }
        public bool removeBrawlerAnimations { get; set; }
        public bool removeReaperAnimations { get; set; }
        //public bool removeIntroVideos { get; set; }

        public Tweaker(string gameDir)
        {
            _gameDir = gameDir;

            //Init ConfigFiles 
            LoadConfigFiles();
        }

        public bool ResetConfigChanges()
        {
            bool result = false;

            var s1engineBackup = FileManager.GetS1EngineBackup(_gameDir);

            if (s1engineBackup.Exists)
            {
                //Remove Read-Only of backup if set
                FileManager.RemoveReadOnlyFlagIfSet(s1engineBackup);

                var currentConfig = FileManager.GetCurrentS1Engine(_gameDir);

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
                    FileManager.RemoveReadOnlyFlagIfSet(currentConfig);
                    File.Copy(s1engineBackup.FullName, currentConfig.FullName, true);

                    //Set Read-Only again
                    FileManager.SetReadOnlyFlag(currentConfig);

                    //Delete Backup
                    File.Delete(s1engineBackup.FullName);

                    MessageBox.Show("Reset was successfull", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    result = true;
                }
            }
            else
            {
                MessageBox.Show("There is no backup file, which could be used for resetting the changes. Please delete the S1Engine.ini and run the game, if you wan't a clean configuration.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.Info("No backup file found at {0}", s1engineBackup.FullName);
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

        public bool ResetAdditionalTweaks()
        {
            return RemoveKoreanUIFiles() && ResetClassAnimations() && ResetIntroFiles();
        }

        private bool RemoveKoreanUIFiles()
        {
            string pathS1UI = string.Format("{0}\\{1}", _gameDir, BaseConsts.S1UI_DIR);
            if (Directory.Exists(pathS1UI))
            {
                //Mod UI Folder exists -> Delete it
                Directory.Delete(pathS1UI, true);
            }
            return true;
        }

        private bool ResetClassAnimations()
        {
            //TODO
            return true;
        }

        private bool ResetIntroFiles()
        {
            //TODO
            return true;
        }

        public bool ApplyAdditionalTweaks()
        {
            return InstallKoreanUIFiles() && RemoveClassAnimations();
        }

        private bool InstallKoreanUIFiles()
        {
            bool copyAllowed;

            //Check if "_S1UI" exists
            string pathS1UI = string.Format("{0}\\{1}", _gameDir, BaseConsts.S1UI_DIR);

            if (Directory.Exists(pathS1UI))
            {
                //There is already an _S1UI Folder. Check if it contains files
                if (Directory.GetFiles(pathS1UI).Count() > 0)
                {
                    //There are files - Overwrite them? 
                    var mbResult = MessageBox.Show("There are already UI Mods installed! \nDo you want to overwrite them?", "Overwrite", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    switch (mbResult)
                    {
                        case MessageBoxResult.Yes:
                            copyAllowed = true;
                            break;
                        case MessageBoxResult.No:
                            copyAllowed = false;
                            break;
                        default:
                            throw new Exception("Cancelled by user");
                    }
                }
                else
                {
                    copyAllowed = true;
                }
            }
            else
            {
                //Create the Mod directory
                Directory.CreateDirectory(pathS1UI);
                copyAllowed = true;
            }

            //Copy files 
            if (copyAllowed)
            {
                var koreanFiles = Directory.GetFiles(BaseConsts.KOREANUIFILES_DIR);

                foreach (var koreanFile in koreanFiles)
                {
                    FileInfo file = new FileInfo(koreanFile);
                    string fileName = file.Name;
                    string destinationFile = string.Format("{0}\\{1}", pathS1UI, fileName);
                    File.Copy(koreanFile, destinationFile, true);
                }


                //Special PartyBuff Option selected?
                string source;
                string destination = string.Format("{0}\\{1}", pathS1UI, BaseConsts.PARTYWINDOW);
                switch (partyBuffOption)
                {
                    case PartyBuffOptions.Default:
                        //Nothing to do, was set already
                        break;
                    case PartyBuffOptions.WithoutPurpleBar:
                        source = string.Format("{0}\\{1}", BaseConsts.PARTYWINDOWS_DIR, BaseConsts.PARTYWINDOW_WITHOUT_PURPLE);
                        File.Copy(source, destination, true);
                        break;
                    case PartyBuffOptions.WithPurpleBar:
                        source = string.Format("{0}\\{1}", BaseConsts.PARTYWINDOWS_DIR, BaseConsts.PARTYWINDOW_WITH_PURLE);
                        File.Copy(source, destination, true);
                        break;
                }
            }


            return true; //If code reaches this part without error, it was successful
        }

        private bool RemoveClassAnimations()
        {
            //TODO

            return true;
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
            var currentConfig = FileManager.GetCurrentS1Engine(_gameDir);

            //Remove Read-Only Flag if set
            FileManager.RemoveReadOnlyFlagIfSet(currentConfig);

            //Copy the preset
            File.Copy(presetPath, currentConfig.FullName, true);

            //Set Read-Only again
            FileManager.SetReadOnlyFlag(currentConfig);
        }

        private void BackUpUserConfiguration()
        {
            var file = FileManager.GetS1EngineBackup(_gameDir);

            var currentConfig = FileManager.GetCurrentS1Engine(_gameDir);

            if (!file.Exists)
            {
                //There is no backup. Just create it
                File.Copy(currentConfig.FullName, file.FullName);
            }
            else
            {
                //There is already a backup. Check if the current configuration is equal with a preset. If not, ask to overwrite it. 
                if (!FileIsEqualWithPreset(currentConfig))
                {
                    //Ask if the current backup should be overwritten with the current config file
                    if (OverwriteBackup())
                    {
                        File.Copy(currentConfig.FullName, file.FullName, true);
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
                if (FileManager.FilesAreEqual(currentConfigFile, presetFile.Value))
                    return true;
            }

            return false;
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

        public void SetPartyBuffOption(PartyBuffOptions option)
        {
            partyBuffOption = option;
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
