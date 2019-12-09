using Hashgraph.UI.WPF.Util;
using NSec.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Hashgraph.UI.WPF.Controls
{
    [TemplatePart(Name = "PART_Fingerprint", Type = typeof(Run))]
    public class PublicKeyFingerprintControl : Control
    {
        private Run _fingerprintTextBlock;
        static PublicKeyFingerprintControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PublicKeyFingerprintControl), new FrameworkPropertyMetadata(typeof(PublicKeyFingerprintControl)));
            IsTabStopProperty.OverrideMetadata(typeof(PublicKeyFingerprintControl), new FrameworkPropertyMetadata(false));
        }

        public static readonly DependencyProperty PublicKeyProperty = DependencyProperty.Register(nameof(PublicKey), typeof(PublicKey), typeof(PublicKeyFingerprintControl), new PropertyMetadata(onRenderablePropertyChanged));
        public static readonly DependencyProperty EllipseForegroundProperty = DependencyProperty.Register(nameof(EllipseForeground), typeof(Brush), typeof(PublicKeyFingerprintControl));
        public static readonly DependencyProperty ByteCountProperty = DependencyProperty.Register(nameof(ByteCount), typeof(int), typeof(PublicKeyFingerprintControl), new PropertyMetadata(onRenderablePropertyChanged));

        public PublicKey PublicKey
        {
            get { return (PublicKey)GetValue(PublicKeyProperty); }
            set { SetValue(PublicKeyProperty, value); }
        }
        public Brush EllipseForeground
        {
            get { return (Brush)GetValue(EllipseForegroundProperty); }
            set { SetValue(EllipseForegroundProperty, value); }
        }
        public int ByteCount
        {
            get { return (int)GetValue(ByteCountProperty); }
            set { SetValue(ByteCountProperty, value); }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _fingerprintTextBlock = GetTemplateChild("PART_Fingerprint") as Run;
            updateDisplay();
        }
        private void updateDisplay()
        {
            if (_fingerprintTextBlock != null)
            {
                var publicKey = PublicKey;
                if (publicKey != null)
                {
                    var count = ByteCount;
                    var bytes = publicKey.Export(KeyBlobFormat.PkixPublicKey);
                    if (bytes.Length > 32 + ByteCount)
                    {
                        _fingerprintTextBlock.Text = Hex.FromBytes(bytes[^32..][..count]);
                    }
                    else if (bytes.Length > count)
                    {
                        _fingerprintTextBlock.Text = Hex.FromBytes(bytes[..count]);
                    }
                    else
                    {
                        _fingerprintTextBlock.Text = Hex.FromBytes(bytes);
                    }
                }
                else
                {
                    _fingerprintTextBlock.Text = string.Empty;
                }
            }
        }
        private static void onRenderablePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs events)
        {
            (source as PublicKeyFingerprintControl)?.updateDisplay();
        }
    }
}
