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
using System.Windows.Navigation;

namespace SinaDesktop
{
    public partial class InstallPage : Page
    {
        public InstallPage()
        {
            InitializeComponent();
            if (Application.Current.InstallState == InstallState.Installed)
            {
                lblMessage.Text = "LightBus微博应用已经安装. " +
                    "该应用无法运行在浏览器下。 " +
                    "请运行应用桌面快捷方式。";
                cmdInstall.IsEnabled = false;
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void cmdInstall_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.InstallState != InstallState.Installed)
            {
                // Attempt to install it.
                bool installAccepted = Application.Current.Install();

                if (!installAccepted)
                {
                    lblMessage.Text =
                      "安装失败，请点击重新安装LightBus微博应用。";
                }
                else
                {
                    cmdInstall.IsEnabled = false;
                    lblMessage.Text = "LightBus微博应用正在安装中... ";
                }
            }
        }

        public void DisplayInstalled()
        {
            lblMessage.Text =
              "LightBus微博应用已经安装成功并运行，请关闭当前网页。";
        }

        public void DisplayFailed()
        {
            lblMessage.Text = "LightBus微博应用安装失败。";
            cmdInstall.IsEnabled = true;
        }

    }
}
