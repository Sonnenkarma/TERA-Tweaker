using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using TERA_Tweaker.consts;

namespace TERA_Tweaker.init
{
    public static class Initializer
    {
        public static string GetGameDirectory()
        {
            //If App-Settings contains path return it
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.GameDir))
                return Properties.Settings.Default.GameDir;

            //Looking for TERA-Launcher.exe in current directory
            var dirCheck = Directory.GetCurrentDirectory();
            if (FoundInDir(dirCheck, SearchOption.TopDirectoryOnly))
                return dirCheck;

            //If not found check the default Frogster Installation directory
            dirCheck = string.Format(@"{0}\{1}", ProgramFilesx86(), BaseConsts.DEFAULT_FROGSTER_DIR);
            if (FoundInDir(dirCheck, SearchOption.AllDirectories))
                return dirCheck;

            //If not found check the default GameForge Installation directory
            dirCheck = string.Format(@"{0}\{1}", ProgramFilesx86(), BaseConsts.DEFAULT_GF_DIR);
            if (FoundInDir(dirCheck, SearchOption.AllDirectories))
                return dirCheck;

            //If not found check the default EME Installation directory
            dirCheck = BaseConsts.DEFAULT_EME_DIR;
            if (FoundInDir(dirCheck, SearchOption.AllDirectories))
                return dirCheck;

            //If still not found, user should select it manually
            return GetGameDirectoryByUserSelection();
        }

        static private bool FoundInDir(string dirToCheck, SearchOption searchOption)
        {
            bool result = false;
            if (Directory.Exists(dirToCheck))
            {
                var foundFiles = Directory.GetFiles(dirToCheck, BaseConsts.LAUNCHER_FILENAME, searchOption);
                if (foundFiles.Count() == 1)
                    result = true;
            }
            return result;
        }

        private static string ProgramFilesx86()
        {
            if (Environment.Is64BitOperatingSystem)
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        public static string GetGameDirectoryByUserSelection()
        {
            // Create Dialog
            var dlg = new OpenFileDialog();

            // Set filter
            dlg.DefaultExt = ".exe";
            dlg.Filter = "TERA Launcher (TERA-Launcher.exe)|TERA-Launcher.exe";

            // Show dialig
            if (dlg.ShowDialog() == true)
            {
                return dlg.FileName; //return selection
            }

            return string.Empty; //empty string if selection cancelled
        }

        public static void CreateDirectoriesForTweakFiles()
        {
            if (!Directory.Exists(BaseConsts.EMPTYVIDEOFILES_DIR))
            {
                Directory.CreateDirectory(BaseConsts.EMPTYVIDEOFILES_DIR);
            }
            if (!Directory.Exists(BaseConsts.KOREANUIFILES_DIR))
            {
                Directory.CreateDirectory(BaseConsts.KOREANUIFILES_DIR);
            }
            if (!Directory.Exists(BaseConsts.PRESETS_DIR))
            {
                Directory.CreateDirectory(BaseConsts.PRESETS_DIR);
            }
        }
    }
}
