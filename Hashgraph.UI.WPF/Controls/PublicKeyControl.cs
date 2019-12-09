using Hashgraph.UI.WPF.Util;
using NSec.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hashgraph.UI.WPF.Controls
{
    [TemplatePart(Name = "PART_Prefix", Type = typeof(Run))]
    [TemplatePart(Name = "PART_Value", Type = typeof(Run))]
    public class PublicKeyControl : Control
    {
        private Run _prefixTextBlock;
        private Run _valueTextBlock;
        static PublicKeyControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PublicKeyControl), new FrameworkPropertyMetadata(typeof(PublicKeyControl)));
            IsTabStopProperty.OverrideMetadata(typeof(PublicKeyControl), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty PublicKeyProperty = DependencyProperty.Register(nameof(PublicKey), typeof(PublicKey), typeof(PublicKeyControl), new PropertyMetadata(onPublicKeyChanged));
        public static readonly DependencyProperty PrefixForegroundProperty = DependencyProperty.Register(nameof(PrefixForeground), typeof(Brush), typeof(PublicKeyControl));
        public static readonly DependencyProperty AbbreviatePrefixProperty = DependencyProperty.Register(nameof(AbbreviatePrefix), typeof(bool), typeof(PublicKeyControl));
        public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(PublicKeyControl), new FrameworkPropertyMetadata(TextWrapping.NoWrap));

        public PublicKey PublicKey
        {
            get { return (PublicKey)GetValue(PublicKeyProperty); }
            set { SetValue(PublicKeyProperty, value); }
        }
        public Brush PrefixForeground
        {
            get { return (Brush)GetValue(PrefixForegroundProperty); }
            set { SetValue(PrefixForegroundProperty, value); }
        }
        public bool AbbreviatePrefix
        {
            get { return (bool)GetValue(AbbreviatePrefixProperty); }
            set { SetValue(AbbreviatePrefixProperty, value); }
        }
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _prefixTextBlock = GetTemplateChild("PART_Prefix") as Run;
            _valueTextBlock = GetTemplateChild("PART_Value") as Run;
            updateDisplay();
        }

        private void updateDisplay()
        {
            var (prefix, value) = getKeyAsHexParts();
            if (_prefixTextBlock != null)
            {
                _prefixTextBlock.Text = prefix;
            }
            if (_valueTextBlock != null)
            {
                _valueTextBlock.Text = value;
            }
        }

        private (string prefix, string value) getKeyAsHexParts()
        {
            var publicKey = PublicKey;
            var prefix = string.Empty;
            var value = string.Empty;
            if (publicKey != null)
            {
                var bytes = publicKey.Export(KeyBlobFormat.PkixPublicKey);
                if (bytes.Length > 34)
                {
                    prefix = AbbreviatePrefix ?
                        Hex.FromBytes(bytes[..^32][..2]) + "..." :
                        Hex.FromBytes(bytes[..^32]);
                    value = Hex.FromBytes(bytes[^32..]);
                }
                else
                {
                    value = Hex.FromBytes(bytes);
                }
            }
            return (prefix, value);
        }

        private static void onPublicKeyChanged(DependencyObject source, DependencyPropertyChangedEventArgs events)
        {
            (source as PublicKeyControl)?.updateDisplay();
        }

    }
}
