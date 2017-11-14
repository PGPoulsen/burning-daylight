using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using BurningDaylight.Properties;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using YouTrackSharp;

namespace BurningDaylight
{
    public partial class MainWindow
    {
        private UsernamePasswordConnection _youTrackConnection;
        private string _appName = "Robert's Burning Daylight Monitor";

        public MainWindow()
        {
            InitializeComponent();

            ResetYouTrackConnection();

            MonitorTimeTracking();
        }

        private async Task MonitorTimeTracking()
        {
            for (;;)
            {
                var issueService = _youTrackConnection?.CreateIssuesService();

                if (issueService != null)
                {
                    var count = await issueService.GetIssueCount(Settings.Default.IssuesBeingTimeTrackedQuery);


                    if (count < 1)
                    {
                        TaskbarIcon.ShowBalloonTip("You are burning daylight!", "Start some time tracker!",
                            BalloonIcon.Warning);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        private void ResetYouTrackConnection()
        {
            var url = Settings.Default.YouTrackUrl;
            var username = Settings.Default.YouTrackUsername;
            var password = Settings.Default.YouTrackPassword;

            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(username) &&
                !string.IsNullOrWhiteSpace(password))
            {
                _youTrackConnection = new UsernamePasswordConnection(url, username, password);
            }
            else
            {
                _youTrackConnection = null;
            }
        }

        private void Open_SettingsWindow(object sender, RoutedEventArgs e)
        {
            var w = new SettingsWindow();
            if (w.ShowDialog() == true)
            {
                ResetYouTrackConnection();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Toggle_Start_Automatically(object sender, RoutedEventArgs e)
        {
            if (StartAutomaticallyToggle.IsChecked)
            {
                AutoStartRegistry.SetValue(_appName, Assembly.GetExecutingAssembly().Location);
            }
            else
            {
                AutoStartRegistry.DeleteValue(_appName, false);
            }
        }

        private static RegistryKey AutoStartRegistry => Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        private void Initialize_StartAutomaticallyToggle(object sender, RoutedEventArgs e)
        {
            StartAutomaticallyToggle.IsChecked = AutoStartRegistry.GetValue(_appName) != null;
            StartAutomaticallyToggle.Checked += Toggle_Start_Automatically;
        }
    }
}