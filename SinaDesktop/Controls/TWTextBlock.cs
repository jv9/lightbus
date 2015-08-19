using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SinaDesktop.Controls
{
    public class TWTextBlock : Control
    {
        public static DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(TWTextBlock),
           new PropertyMetadata(null, OnTextChanged));
        WrapPanel _rootPanel;
        private const string TWITTER_USER_URL_FORMAT = "http://t.sina.com.cn/n/{0}";

        public TWTextBlock()
        {
            this.DefaultStyleKey = typeof(TWTextBlock);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootPanel = GetTemplateChild("RootPanel") as WrapPanel;
            UpdatePanelControls();
        }

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((TWTextBlock)sender).OnTextChanged((string)e.NewValue);
        }
        protected virtual void OnTextChanged(string newString)
        {
            UpdatePanelControls();
        }
        private void UpdatePanelControls()
        {
            if (_rootPanel != null)
            {
                _rootPanel.Children.Clear();
                string[] strs = Text.Split(' ');
                for (int i = 0; i < strs.Length; i++)
                {
                    string str = strs[i];
                    if (str.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                    {
                        _rootPanel.Children.Add(GetHyperlinkControl(str, str));
                    }
                    else if (str.StartsWith("//@", StringComparison.InvariantCultureIgnoreCase))
                    {
                        //str = str.Substring(3);
                        //  || str.StartsWith("@", StringComparison.InvariantCultureIgnoreCase)
                        string[] tmpUserName = str.Split(':');
                        string[] tmpSubUserName = tmpUserName[0].Split('@');

                        string url = string.Format(TWITTER_USER_URL_FORMAT, tmpSubUserName[1]);

                        _rootPanel.Children.Add(GetTextControl(tmpSubUserName[0]));
                        _rootPanel.Children.Add(GetHyperlinkControl(tmpSubUserName[1], url));
                        _rootPanel.Children.Add(GetTextControl(tmpUserName[1]));
                    }
                    else
                    {
                        _rootPanel.Children.Add(GetTextControl(str));
                    }

                    if (i < strs.Length - 1)
                    {
                        _rootPanel.Children.Add(GetSpaceControl());
                    }
                }
            }
        }

        private UIElement GetSpaceControl()
        {
            return new TextBlock() { Text = " " };
        }
        private UIElement GetTextControl(string str)
        {
            return new TextBlock() { Text = str, TextWrapping = 0 };
        }
        private UIElement GetHyperlinkControl(string text, string url)
        {
            HyperlinkButton btn = new HyperlinkButton()
            {
                Content = text,
                NavigateUri = new Uri(url, UriKind.Absolute),
                TargetName = "_blank"
            };
            ToolTipService.SetToolTip(btn, url);
            if (LinkStyle != null)
                btn.Style = LinkStyle;
            return btn;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public Style LinkStyle
        {
            get;
            set;
        }
    }
}
