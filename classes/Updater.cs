using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TERA_Tweaker.updater
{
    class Updater
    {
        public static void Update()
        {
            if (newPresetsAvailable())
            {
                UpdatePresets();
            }
            if (newKoreanUIFilesAvailable())
            {
                UpdateKoreanUIFiles();
            }
            if (newEmptyVideoFilesAvailable())
            {
                UpdateEmptyVideoFiles();
            }
        }

        #region Update Checks Region
        private static bool newPresetsAvailable()
        {
            // TODO Add check preset version code here
            return true;
        }

        private static bool newKoreanUIFilesAvailable()
        {
            // TODO Add check Korean UI Files version code here
            return true;
        }

        private static bool newEmptyVideoFilesAvailable()
        {
            // TODO Add check Empty Video Files version code here
            return true;
        }
        #endregion

        #region Update Files Region
        private static void UpdatePresets()
        {
            // TODO Add code for downloading presets
        }

        private static void UpdateKoreanUIFiles()
        {
            // TODO Add code for downloading Korean UI Files
        }

        private static void UpdateEmptyVideoFiles()
        {
            // TODO Add code for downloading Empty Video Files
        }
        #endregion
    }
}
