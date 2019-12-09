using System;
using System.Windows;

namespace Hashgraph.UI.WPF
{
    public sealed class ThemeResourceDictionary : ResourceDictionary
    {
        public enum ThemeType
        {
            Light = 0,
            Dark = 1
        }
        [ThreadStatic] 
        private static ResourceDictionary dynamicDictionary = new ResourceDictionary() { Source = new Uri("/Hashgraph.UI.WPF;component/Themes/Dynamic/LightTheme.xaml", UriKind.Relative) };
        private static ResourceDictionary lightTheme = new ResourceDictionary() { Source = new Uri("/Hashgraph.UI.WPF;component/Themes/Dynamic/LightTheme.xaml", UriKind.Relative) };
        private static ResourceDictionary darkTheme = new ResourceDictionary() { Source = new Uri("/Hashgraph.UI.WPF;component/Themes/Dynamic/DarkTheme.xaml", UriKind.Relative) };
        public static ThemeType CurrentTheme { get; private set; } = ThemeType.Light;
        public ThemeResourceDictionary()
        {
            MergedDictionaries.Add(dynamicDictionary);
        }
        public static void SwitchTheme()
        {
            switch (CurrentTheme)
            {
                case ThemeType.Light:
                    SetTheme(ThemeType.Dark);
                    break;
                case ThemeType.Dark:
                    SetTheme(ThemeType.Light);
                    break;
            }
        }
        public static void SetTheme(ThemeType type)
        {
            switch (type)
            {
                case ThemeType.Light:
                    updateTheme(lightTheme);
                    CurrentTheme = ThemeType.Light;
                    break;
                case ThemeType.Dark:
                    updateTheme(darkTheme);
                    CurrentTheme = ThemeType.Dark;
                    break;
            }
        }

        private static void updateTheme(ResourceDictionary themeDictionary)
        {
            foreach (var key in themeDictionary.Keys)
            {
                dynamicDictionary[key] = themeDictionary[key];
            }
        }
    }
}
