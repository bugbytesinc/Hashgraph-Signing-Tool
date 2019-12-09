using System;
using System.Collections;
using System.Windows;
using System.Windows.Data;

namespace Hashgraph.UI.WPF.Converters
{
    public class EmptyToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var collection = value as ICollection;
            if (collection != null)
            {
                return collection.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
