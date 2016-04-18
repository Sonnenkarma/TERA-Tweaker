using System;
using System.Windows;
using System.Windows.Controls;
using TERA_Tweaker.init;
using TERA_Tweaker.updater;

namespace TERA_Tweaker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            //First get the game directory for futher actions 
            var gameDir = Initializer.GetGameDirectory();

            //Save gamedir in app settings
            Properties.Settings.Default.GameDir = gameDir;
            Properties.Settings.Default.Save();

            //Create Directories for Game files
            Initializer.CreateDirectoriesForTweakFiles();
        }

        private void buttonCustomSettings_Click(object sender, RoutedEventArgs e)
        {
            CustomTweaksWindow ctw = new CustomTweaksWindow();
            ctw.ShowDialog();
        }

        private void sliderPerformance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (labelCurrentPreset == null)
            {
                return;
            }

            var slider = sender as Slider;
            int value = Convert.ToInt32(slider.Value);

            switch (value)
            {
                case 1:
                    labelCurrentPreset.Content = "Best Performance";
                    break;
                case 2:
                    labelCurrentPreset.Content = "Good Performance";
                    break;
                case 3:
                    labelCurrentPreset.Content = "Balanced";
                    break;
                case 4:
                    labelCurrentPreset.Content = "Good Quality";
                    break;
                case 5:
                    labelCurrentPreset.Content = "Best Quality";
                    break;
            }
        }
        

        private void buttonResetIniTweaks_Click(object sender, RoutedEventArgs e)
        {
            labelCurrentPreset.Content = "Untouched";
            MessageBox.Show("Settings resetted.");
        }

        private void buttonApplyIniTweaks_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Settings applied.");
        }

        private void buttonApplyFileTweaks_Click(object sender, RoutedEventArgs e)
        {
            
            if (checkboxKoreanUIFiles.IsChecked.Value)
            {
                var msg = "Would you like to remove the party buff bars? ";
                msg += "This could interfere with your partyplay as priest or mystic.";
                MessageBoxResult mbResult = MessageBox.Show(msg, "Remove Partybuffs?", MessageBoxButton.YesNo);

                if (mbResult == MessageBoxResult.Yes)
                {

                } else
                {

                }
            }
            MessageBox.Show("Tweaks applied.");
        }

        private void buttonResetFileTweaks_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tweaks resetted.");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var updaterWindow = new UpdaterWindow();
            updaterWindow.ShowDialog();
        }
    }
}
