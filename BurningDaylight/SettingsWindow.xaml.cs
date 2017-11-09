using System.Windows;
using BurningDaylight.Properties;

namespace BurningDaylight
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Save_Settings(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(YouTrackPasswordBox.Password))
            {
                Settings.Default.YouTrackPassword = YouTrackPasswordBox.Password;
            }
            Settings.Default.Save();
            DialogResult = true;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reload();
            DialogResult = false;
            Close();
        }
    }
}
