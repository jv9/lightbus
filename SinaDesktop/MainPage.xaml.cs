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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SinaDesktop.Controls;
using SinaDesktop.Utilities;


namespace SinaDesktop
{
    public partial class MainPage : UserControl
    {
        Window OOBWindow = Application.Current.MainWindow;
        private GlobalSettingsChildWindow globalSettingsChildWindow = null;
  
        public MainPage()
        {
            InitializeComponent();

            //spHeader.Width = OOBWindow.Width - 60;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OOBWindow.DragMove();
        }

        private void border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement border = sender as FrameworkElement;
            App.Current.MainWindow.DragResize((WindowResizeEdge)Enum.Parse(typeof(WindowResizeEdge), border.Tag.ToString(), true));
        }

        private void hbGlobleSetting_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = .6;
            showrectProgress.Begin();
        }

        private void hbGlobleSetting_MouseLeave(object sender, MouseEventArgs e)
        {
            HyperlinkButton button = sender as HyperlinkButton;
            button.Opacity = 1;
        }

        private void hbGlobleSetting_Click(object sender, RoutedEventArgs e)
        {
            globalSettingsChildWindow = new GlobalSettingsChildWindow(Application.Current.MainWindow.Height);
            globalSettingsChildWindow.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            globalSettingsChildWindow.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            globalSettingsChildWindow.Margin = new Thickness(2, 1, 0, 2);
            globalSettingsChildWindow.Show();
        }

        






        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        //private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        //{
        //    foreach (UIElement child in LinksStackPanel.Children)
        //    {
        //        HyperlinkButton hb = child as HyperlinkButton;
        //        if (hb != null && hb.NavigateUri != null)
        //        {
        //            if (ContentFrame.UriMapper.MapUri(e.Uri).ToString().Equals(ContentFrame.UriMapper.MapUri(hb.NavigateUri).ToString()))
        //            {
        //                VisualStateManager.GoToState(hb, "ActiveLink", true);
        //            }
        //            else
        //            {
        //                VisualStateManager.GoToState(hb, "InactiveLink", true);
        //            }
        //        }
        //    }
        //}

        //// If an error occurs during navigation, show an error window
        //private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        //{
        //    e.Handled = true;
        //    ChildWindow errorWin = new ErrorWindow(e.Uri);
        //    errorWin.Show();
        //}

    }
}