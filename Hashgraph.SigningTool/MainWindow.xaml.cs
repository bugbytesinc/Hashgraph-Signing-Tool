using Hashgraph.SigningTool.Models;
using Hashgraph.UI.WPF;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public partial class MainWindow : Window
    {
        private UserPreferenceSettings settings;
        public MainWindow()
        {
            settings = new UserPreferenceSettings();
            ThemeResourceDictionary.SetTheme(settings.CurrentTheme);
            InitializeComponent();
            FindAndDisplayStep();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var button = new Button()
            {
                Style = Resources["ChangeModeWindowButtonStyle"] as Style
            };
            button.Click += OnChangeModeClicked;
            (Template.FindName("ExtraButtonArea", this) as StackPanel).Children.Insert(0, button);
        }

        private void OnChangeModeClicked(object sender, RoutedEventArgs e)
        {
            switch (settings.DisplayMode)
            {
                case UserPreferenceSettings.Mode.AllInOne:
                    settings.DisplayMode = UserPreferenceSettings.Mode.Wizard;
                    break;
                case UserPreferenceSettings.Mode.Wizard:
                    settings.DisplayMode = UserPreferenceSettings.Mode.AllInOne;
                    break;
            }
            FindAndDisplayStep();
        }

        private void FindAndDisplayStep()
        {
            switch (settings.DisplayMode)
            {
                case UserPreferenceSettings.Mode.AllInOne:
                    Content = new AllInOneStep();
                    break;
                case UserPreferenceSettings.Mode.Wizard:
                    if (SigningData.TransactionBodyBytes.IsEmpty)
                    {
                        if (SigningData.PublicKeys.Length == 0)
                        {
                            Content = new Step1();
                        }
                        else
                        {
                            Content = new Step2();
                        }
                    }
                    else if (SigningData.PublicKeys.Length == 0)
                    {
                        Content = new Step1();
                    }
                    else
                    {
                        Content = new Step4();
                    }
                    break;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            settings.CurrentTheme = ThemeResourceDictionary.CurrentTheme;
            settings.Save();
            base.OnClosing(e);
        }
    }
}
