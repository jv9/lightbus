using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace SinaDesktop.Controls
{
    public class HtmlTextBlock : RichTextBox
    {
        const string UriPattern = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#][a-z0-9A-Z]*)?)|@([\u4e00-\u9fa5A-Za-z0-9_]*)?|#.+?#|(@[a-zA-Z0-9_-][\u4e00-\u9fa5A-Za-z0-9_]*)";
        //const string UriPattern = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)|@([\u4e00-\u9fa5A-Za-z0-9_]*)?|#.+?#|(@[a-zA-Z0-9_-][\u4e00-\u9fa5A-Za-z0-9_]*)";
        public HtmlTextBlock()
        {
            base.IsReadOnly = true;
            base.BorderThickness = new Thickness(0);
            Padding = new Thickness(0);
            //base.Background = new SolidColorBrush(Colors.blu);

        }

        #region Text

        /// <summary>
        /// Text Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HtmlTextBlock),
                new PropertyMetadata("", new PropertyChangedCallback(OnTextChanged)));

        /// <summary>
        /// Gets or sets the Text property. This dependency property 
        /// indicates ....
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Text property.
        /// </summary>
        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HtmlTextBlock target = (HtmlTextBlock)d;
            string oldValue = (string)e.OldValue;
            string newValue = target.Text;
            //target.Background = new SolidColorBrush(Colors.Blue);
            target.OnTextChanged(oldValue, newValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Text property.
        /// </summary>
        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
            Blocks.Clear();
            GetUriMatches(newValue);
        }

        #endregion

        private void GetUriMatches(string text)
        {
            int index = 0;
            Regex uriLocator = new Regex(UriPattern);
            if (text == null)
                text = "此人很懒,什么也没有写.";
            MatchCollection uriMatches = uriLocator.Matches(text);

            Paragraph p = new Paragraph();
            //p.Foreground = new System.Windows.Media.SolidColorBrush(App.Theme.ForegroundColor);
            
            Uri currentUri;
            foreach (Match uri in uriMatches.Cast<Match>())
            {
                Uri realUri;
                string value = uri.Value;
                if (value.StartsWith("@"))
                {
                    realUri = new Uri("http://weibo.com/n/" + uri.Value.Substring(1), UriKind.Absolute);
                }
                else if (value.StartsWith("#"))
                {
                    realUri = new Uri("http://weibo.com/k/" + uri.Value.Substring(1), UriKind.Absolute);
                }
                else if (Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out currentUri))
                {
                    realUri = new Uri(uri.Value, UriKind.RelativeOrAbsolute);
                }
                else
                {
                    continue;
                }

                p.Inlines.Add(text.Substring(index, uri.Index - index));
                index = uri.Index + uri.Length;

                Hyperlink h = new Hyperlink();
                //h.Foreground = new System.Windows.Media.SolidColorBrush(App.Theme.LinkColor);
                //h.MouseOverForeground = new System.Windows.Media.SolidColorBrush(App.Theme.ForegroundColor);
                h.Inlines.Add(uri.Value);
                h.NavigateUri = realUri;
                h.TargetName = "_blank";
                p.Inlines.Add(h);
            }
            p.Inlines.Add(text.Substring(index));
            Blocks.Add(p);

            return;

            //Regex.Matches("bla @andrejt sds", @"(@)(.*)(\s)")[0].Groups[2].Value
        }

    }
}
