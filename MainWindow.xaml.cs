using System.Windows;
using TERA_Tweaker.init;

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
    }
}
