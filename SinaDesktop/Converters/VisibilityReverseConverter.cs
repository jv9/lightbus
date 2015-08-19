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
using System.Globalization;

namespace SinaDesktop.Converters
{
    public class VisibilityReverseConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
        {
            bool visibility = (bool)value;
            return visibility ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility == Visibility.Visible);
        }

        #endregion
    }
}
