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
    public partial class ImageViewerChildWindow : ChildWindow
    {
        #region Constructor
        public ImageViewerChildWindow()
        {
            InitializeComponent();
        }

        public ImageViewerChildWindow(string sTitle, string midImageURI, string originalImageURI)
        {
            InitializeComponent();
            this.Title = sTitle;

            imgViewer.Source = new BitmapImage(new Uri(midImageURI, UriKind.Absolute));

        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

