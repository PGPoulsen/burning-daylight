using System;
using System.Threading.Tasks;
using System.Windows;
using BurningDaylight.Properties;
using Hardcodet.Wpf.TaskbarNotification;
using YouTrackSharp;

namespace BurningDaylight
{
    public partial class MainWindow
    {
        private UsernamePasswordConnection _youTrackConnection;

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

            if (!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
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
    }
}
