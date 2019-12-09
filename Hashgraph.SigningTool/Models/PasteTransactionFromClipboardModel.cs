using Hashgraph.UI.WPF.Util;
using Proto;
using System;
using System.Windows;

namespace Hashgraph.SigningTool.Models
{
    public class PasteTransactionFromClipboardModel : BindableDependencyObject<PasteTransactionFromClipboardModel>
    {
        public static readonly DependencyProperty ValidationMessageProperty = RegisterProperty(nameof(ValidationMessage), string.Empty);
        public static readonly DependencyProperty SingularLanguageProperty = RegisterProperty(nameof(SingularLanguage), true);

        public string ValidationMessage { get { return (string)GetValue(ValidationMessageProperty); } set { SetValue(ValidationMessageProperty, value); } }
        public bool SingularLanguage { get { return (bool)GetValue(SingularLanguageProperty); } set { SetValue(SingularLanguageProperty, value); } }
        public ReadOnlyMemory<byte> TransactionBodyBytes { get; private set; }

        public PasteTransactionFromClipboardModel()
        {
            SingularLanguage = SigningData.PublicKeys.Length < 2;
        }

        public bool TryPasteFromClipboard()
        {
            if (HashgraphClipboard.TryGetAsBytes<TransactionBody>("Hedera Transaction", out ReadOnlyMemory<byte> data, out string error))
            {
                SigningData.TransactionBodyBytes = data;
                TransactionBodyBytes = data;
                return true;
            }
            else
            {
                ValidationMessage = error;
                return false;
            }
        }
    }
}