using System.Windows;
using TERA_Tweaker.init;

namespace TERA_Tweaker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool dragStarted = false;

        public MainWindow()
        {
            InitializeComponent();

            //First get the game directory for futher actions 
            var gameDir = Initializer.GetGameDirectory();
            //Save gamedir in app settings
            Properties.Settings.Default.GameDir = gameDir;
            Properties.Settings.Default.Save();
        }

        private void sliderPerformance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Coming Soon
        }

        private void buttonCustomSettings_Click(object sender, RoutedEventArgs e)
        {
            //Comming Soon
        }

        private void buttonResetIniTweaks_Click(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void buttonApplyIniTweaks_Click(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void buttonResetFileTweaks_Click(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void buttonApplyFileTweaks_Click(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void labelKoreanUIFiles_Checked(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void labelRmGunnerAnims_Checked(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void labelRmBrawlerAnims_Checked(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void labelRmReaperAnims_Checked(object sender, RoutedEventArgs e)
        {
            //Coming Soon
        }

        private void buttonCustomSettings_Click(object sender, RoutedEventArgs e)
        {
            CustomTweaksWindow ctw = new CustomTweaksWindow();
            ctw.ShowDialog();
        }

        private void sliderPerformance_Drop(object sender, DragEventArgs e)
        {
            
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

        private void sliderPerformance_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void sliderPerformance_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            
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
    }
}
