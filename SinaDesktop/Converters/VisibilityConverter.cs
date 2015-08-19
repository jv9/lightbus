using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Data;

namespace SinaDesktop.Converters
{
    public class VisibilityConverter : IValueConverter 
    {

        #region IValueConverter Members

        public object Convert(
        object value,
        Type targetType,
        object parameter,
        CultureInfo culture)
        {
            bool visibility = (bool)value;
            return visibility ? Visibility.Visible : Visibility.Collapsed;
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
