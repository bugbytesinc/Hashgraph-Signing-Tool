using Hashgraph.UI.WPF;
using System.Configuration;

namespace Hashgraph.SigningTool
{
    public class UserPreferenceSettings : ApplicationSettingsBase
    {
        [UserScopedSetting()]
        [DefaultSettingValue(nameof(ThemeResourceDictionary.ThemeType.Light))]
        public ThemeResourceDictionary.ThemeType CurrentTheme
        {
            get { return ((ThemeResourceDictionary.ThemeType)this[nameof(CurrentTheme)]); }
            set { this[nameof(CurrentTheme)] = value; }
        }
        [UserScopedSetting()]
        [DefaultSettingValue(nameof(Mode.Wizard))]
        public Mode DisplayMode
        {
            get { return ((Mode)this[nameof(DisplayMode)]); }
            set { this[nameof(DisplayMode)] = value; }
        }
        public enum Mode
        {
            Wizard = 0,
            AllInOne = 1
        }
    }
}
