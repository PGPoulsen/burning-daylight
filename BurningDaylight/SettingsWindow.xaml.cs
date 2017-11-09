using System.ComponentModel;
using System.Windows;
using BurningDaylight.Properties;

namespace BurningDaylight
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Closing += DiscardChanges;
        }

        private void DiscardChanges(object sender, CancelEventArgs e)
        {
            Settings.Default.Reload();
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
            DialogResult = false;
            Close();
        }
    }
}
