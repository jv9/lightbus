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
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SinaDesktop.Converters
{
    public class StringConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = (string)value;
            if (targetType == typeof(Visibility))
            {
                return string.IsNullOrEmpty(str) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (targetType == typeof(Uri))
            {
                return new Uri(str);
            }
            else if (targetType == typeof(ImageSource))
            {
                if (str == null)
                { return str; }
                else
                {
                    string tmpString = str.Substring(str.Length - 6, 6);

                    if (tmpString == "50/0/1")
                    {
                        str = "../Images/Male.jpg";
                        return new BitmapImage(new Uri(str,UriKind.Relative));
                    }
                    else if (tmpString == "50/0/0")
                    {
                        str = "../Images/Female.jpg";
                        return new BitmapImage(new Uri(str, UriKind.Relative));
                    }
                    else
                    {
                        return new BitmapImage(new Uri(str));
                    }
                }
                    
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
