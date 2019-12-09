using Hashgraph.UI.WPF.Util;
using System;
using System.ComponentModel;
using System.Windows;

namespace Hashgraph.SigningTool.Models
{
    public class EnterPrivateEd25519KeyModel : BindableDependencyObject<EnterPrivateEd25519KeyModel>
    {
        public static readonly DependencyProperty CanContinueProperty = RegisterProperty(nameof(CanContinue), false);
        public static readonly DependencyProperty PrivateKeyInHexProperty = RegisterProperty(nameof(PrivateKeyInHex), string.Empty);
        public static readonly DependencyProperty ValidationMessageProperty = RegisterProperty(nameof(ValidationMessage), string.Empty);

        public bool CanContinue { get { return (bool)GetValue(CanContinueProperty); } set { SetValue(CanContinueProperty, value); } }
        public string ValidationMessage { get { return (string)GetValue(ValidationMessageProperty); } set { SetValue(ValidationMessageProperty, value); } }
        public string PrivateKeyInHex { get { return (string)GetValue(PrivateKeyInHexProperty); } set { SetValue(PrivateKeyInHexProperty, value); } }
        public NSec.Cryptography.Key PrivateKey { get; private set; }
        public EnterPrivateEd25519KeyModel()
        {
            PropertyChanged += OnPropertyChanged;
        }
        public void TryPasteKeyFromClipboard()
        {
            var data = Clipboard.GetDataObject();
            if (data != null && data.GetDataPresent(DataFormats.UnicodeText, true))
            {
                PrivateKeyInHex = data.GetData(DataFormats.UnicodeText, true) as string;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PrivateKeyInHex))
            {
                try
                {
                    var key = Keys.ImportPrivateEd25519KeyFromBytes(Hex.ToBytes(PrivateKeyInHex));
                    if (SigningData.Contains(key))
                    {
                        PrivateKey = null;
                        CanContinue = false;
                        ValidationMessage = "This key is already in the list.";
                    }
                    else
                    {
                        PrivateKey = key;
                        CanContinue = true;
                        ValidationMessage = String.Empty;
                    }
                }
                catch (Exception ex)
                {
                    PrivateKey = null;
                    CanContinue = false;
                    ValidationMessage = "Unable to parse as Ed25519 Private Key: " + ex.Message;
                }
            }
        }
    }
}
