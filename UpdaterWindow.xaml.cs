using System;
using System.Windows;
using TERA_Tweaker.updater;

namespace TERA_Tweaker
{
    /// <summary>
    /// Interaktionslogik für UpdaterWindow.xaml
    /// </summary>
    public partial class UpdaterWindow : Window
    {
        public UpdaterWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Updater.Update();
            this.Close();
        }
    }
}
