using System;
using System.Windows;
using System.ComponentModel;
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
            var updater = new Updater();
            updater.Update();
            this.Close();
        }

    }
}
