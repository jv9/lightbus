using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using SinaDesktop.Utilities;

namespace SinaDesktop.Controls
{
    public partial class GlobalSettingsChildWindow : ChildWindow
    {
        public GlobalSettingsChildWindow(double currentHeight)
        {
            InitializeComponent();
            globalSettingsChildWindow.Height = currentHeight;

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string currentVersion = currentAssembly.FullName;

            AssemblyName asmName = new AssemblyName(currentVersion);

            tbVersion.Text = "v" + asmName.Version.ToString();

            numUpdateTimer.Value = ConfigurationSettings.UpdateTimer;
            numNotificationTimer.Value = ConfigurationSettings.NotificationTimer;
            chkAutoUpdate.IsChecked = ConfigurationSettings.AutoUpdate;
            chkAutoLogin.IsChecked = ConfigurationSettings.AutoLogin;
            chkDisableNotification.IsChecked = ConfigurationSettings.NotificationDisable;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationSettings.UpdateTimer = numUpdateTimer.Value;
            ConfigurationSettings.NotificationTimer = numNotificationTimer.Value;
            ConfigurationSettings.AutoUpdate = (bool)chkAutoUpdate.IsChecked;
            ConfigurationSettings.AutoLogin = (bool)chkAutoLogin.IsChecked;
            ConfigurationSettings.NotificationDisable = (bool)chkDisableNotification.IsChecked;

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void hbBack_Click(object sender, RoutedEventArgs e)
        {
            ConfigurationSettings.UpdateTimer = numUpdateTimer.Value;
            ConfigurationSettings.NotificationTimer = numNotificationTimer.Value;
            ConfigurationSettings.AutoUpdate = (bool)chkAutoUpdate.IsChecked;
            ConfigurationSettings.AutoLogin = (bool)chkAutoLogin.IsChecked;
            ConfigurationSettings.NotificationDisable = (bool)chkDisableNotification.IsChecked;

            this.DialogResult = true;
        }

        private void HyperLinkButton_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = 0.8;
        }

        private void HyperLinkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = 0.5;
        }

        private void btAutoUpdate_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.CheckAndDownloadUpdateAsync();
            Application.Current.CheckAndDownloadUpdateCompleted += new CheckAndDownloadUpdateCompletedEventHandler(Current_CheckAndDownloadUpdateCompleted);
        }

        private void Current_CheckAndDownloadUpdateCompleted(object sender, CheckAndDownloadUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                MessageBox.Show("LightBus微博应用新版本已经下载成功，将在下次启动时生效。");
            }
            else if (e.Error != null && e.Error is PlatformNotSupportedException)
            {
                MessageBox.Show("在检测应用更新时出现以下错误信息：" + Environment.NewLine + e.Error.Message);
            }
            else
            {
                MessageBox.Show("LightBus微博应用是最新版本，无须更新。");
            }
        }
    }
}

