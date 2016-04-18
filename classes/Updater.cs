using System;
using System.Windows;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TERA_Tweaker.updater
{
    class Updater : DependencyObject
    {
        public static readonly DependencyProperty UpdateStatusProperty;
        public string UpdateStatus
        {
            get { return (string)GetValue(UpdateStatusProperty); }
            set { SetValue(UpdateStatusProperty, value); }
        }


        static Updater()
        {
            UpdateStatusProperty = DependencyProperty.Register("UpdateStatus", typeof(string), typeof(Updater));
        }

        public void Update()
        {
            UpdateStatus = "Checking for updates...";
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
        private bool newPresetsAvailable()
        {
            // TODO Add check preset version code here
            return true;
        }

        private bool newKoreanUIFilesAvailable()
        {
            // TODO Add check Korean UI Files version code here
            return true;
        }

        private bool newEmptyVideoFilesAvailable()
        {
            // TODO Add check Empty Video Files version code here
            return true;
        }
        #endregion

        #region Update Files Region
        private void UpdatePresets()
        {
            UpdateStatus = "Updating presets...";
            // TODO Add code for downloading presets
        }

        private void UpdateKoreanUIFiles()
        {
            UpdateStatus = "Updating Korean UI Files...";
            // TODO Add code for downloading Korean UI Files
        }

        private void UpdateEmptyVideoFiles()
        {
            UpdateStatus = "Updating Empty Video Files...";
            // TODO Add code for downloading Empty Video Files
        }
        #endregion
    }
}