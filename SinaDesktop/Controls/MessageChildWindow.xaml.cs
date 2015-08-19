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

namespace SinaDesktop.Controls
{
    public partial class MessageChildWindow : ChildWindow
    {
        #region Public Property
        public string ChildWindowTitle { get; set; }
        #endregion

        #region Constructor
        public MessageChildWindow()
        {
            InitializeComponent();
        }

        public MessageChildWindow(int iAction)
        {
            InitializeComponent();

            switch (iAction)
            {
                case 0:
                    //Warning
                    //tbTitle .Text="登录错误";
                    imageAlert.Source = new BitmapImage(new Uri("/SinaDesktop;component/Images/error.png", UriKind.Relative));
                    tbMessage.Text = "用户名和密码不能为空。";
                    break;
                case 1:
                    imageAlert.Source = new BitmapImage(new Uri("/SinaDesktop;component/Images/error.png", UriKind.Relative));
                    tbMessage.Text = "帐号认证错误,请重新输入用户名和密码。";
                    break;
            }
        }
        #endregion

        #region Private Events
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion

    }
}

