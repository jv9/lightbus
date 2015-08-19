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
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SinaDesktop.Utilities;

namespace SinaDesktop.Controls
{
    public partial class NotificationControl : UserControl
    {
        private NotificationWindow window = null;
        Storyboard _timer = new Storyboard();

        #region Constructor
        public NotificationControl()
        {
            InitializeComponent();
        }

        public NotificationControl(NotificationWindow tmpwindow, String tmpFriendName, String tmpContent, String tmpFriendPic, bool tmpIsVerify)
        {
            InitializeComponent();
            this.window = tmpwindow;

            tbFriendName.Text = tmpFriendName;

            if (tmpIsVerify)
            {
                imgVerify.Visibility = Visibility.Visible;
            }
            else
            {
                imgVerify.Visibility = Visibility.Collapsed;
            }
            
            if (tmpContent.Length < 105)
            {
                tbMessageText.Text = tmpContent;
            }
            else
            {
                tbMessageText.Text = tmpContent.Substring(0, 105) + " ...";
            }
            imgFriendPic.Source = new BitmapImage(new Uri(tmpFriendPic, UriKind.Absolute));

            _timer.Duration = TimeSpan.FromMilliseconds(ConfigurationSettings.NotificationTimer/1000);
            _timer.Completed += new EventHandler(Timer_Tick);
            _timer.Begin();
        }
        #endregion

        #region Private Events
        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Activate();
        }

        private void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            _timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (pbProgress.Value < pbProgress.Maximum)
            {
                pbProgress.Value++;
                _timer.Begin();
            }
        }

        private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            _timer.Begin();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.window.Close();
        }

        private void HyperLinkButton_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = 1;
        }

        private void HyperLinkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = 0.5;
        }
        #endregion
        
    }
}
