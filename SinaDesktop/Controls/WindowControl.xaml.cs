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
using SinaDesktop.Utilities;

namespace SinaDesktop.Controls
{
    public partial class WindowControl : UserControl
    {
        bool maximized = false;

        public WindowControl()
        {
            InitializeComponent();
        }

        private void btMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (!maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                maximized = true;
            }
            else
            {
                maximized = false;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Maximized;

            // Toggle between restore and maximize buttons
            restoreButton.Visibility = System.Windows.Visibility.Visible;
            maximizeButton.Visibility = System.Windows.Visibility.Collapsed;

            // Don't show the resize icon if we're maximized
            // resizeButton.Visibility = System.Windows.Visibility.Collapsed;

            // Restore to it's original opacity since this won't be reset with the MouseLeave Event
            maximizeButton.Opacity = 0.5;
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Normal;

            maximizeButton.Visibility = System.Windows.Visibility.Visible;
            restoreButton.Visibility = System.Windows.Visibility.Collapsed;

            // Make sure the resize icon is showing 
            //resizeButton.Visibility = System.Windows.Visibility.Visible;

            // Restore to it's original opacity since this won't be reset with the MouseLeave Event
            restoreButton.Opacity = 0.5;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
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

        private void miniWindowsButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.MiniMode = true;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            maximizeButton.Visibility = System.Windows.Visibility.Collapsed;
            restoreButton.Visibility = System.Windows.Visibility.Collapsed;
            miniWindowsButton.Visibility = System.Windows.Visibility.Collapsed;
            regularButton.Visibility = System.Windows.Visibility.Visible;

            miniWindowsButton.Opacity = 0.5;

            Application.Current.MainWindow.Width = 442;
        }

        private void regularButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.MiniMode = false;
            miniWindowsButton.Visibility = System.Windows.Visibility.Visible;
            regularButton.Visibility = System.Windows.Visibility.Collapsed;
            maximizeButton.Visibility = System.Windows.Visibility.Visible;
            restoreButton.Visibility = System.Windows.Visibility.Collapsed;
           
            regularButton.Opacity = 0.5;

            Application.Current.MainWindow.Width = 720;

        }
    }
}
