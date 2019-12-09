using System;
using System.Windows.Data;

namespace Hashgraph.UI.WPF.Converters
{
    public class ToggleTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var options = parameter?.ToString()?.Split('|');
            if (options != null && options.Length > 1)
            {
                if (bool.TryParse(value?.ToString(), out bool toggle) && toggle)
                {
                    return options[0];
                }
                return options[1];
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
