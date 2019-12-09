using Hashgraph.UI.WPF.Util;
using System;
using System.Windows.Data;

namespace Hashgraph.UI.WPF.Converters
{
    public class StringToPublicKeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var keyInHex = value?.ToString();
            try
            {
                if (!String.IsNullOrWhiteSpace(keyInHex))
                {
                    return Keys.ImportPublicEd25519KeyFromBytes(Hex.ToBytes(keyInHex));
                }
            }
            catch
            {
                //noop
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
