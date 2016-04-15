using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
