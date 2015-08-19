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

namespace SinaDesktop.Converters
{
    public class TimeConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String str = value.ToString().Replace("T", " ");
            DateTime dt = DateTime.Parse(str);
            //DateTime dt = (DateTime)value;
            if (targetType == typeof(string))
            {
                string format = "{0}{1}之前  ";
                TimeSpan ts = DateTime.Now - dt;
                if (ts.Days > 0)
                {
                    return string.Format(format, ts.Days, "天");
                }
                else if (ts.Hours > 0)
                {
                    return string.Format(format, ts.Hours, "小时");
                }
                else
                {
                    return string.Format(format, ts.Minutes, "分钟");
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
