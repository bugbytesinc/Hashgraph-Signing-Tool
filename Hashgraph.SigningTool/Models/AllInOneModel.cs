using Hashgraph.UI.WPF.Util;
using NSec.Cryptography;
using Proto;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Hashgraph.SigningTool.Models
{
    public class AllInOneModel : BindableDependencyObject<AllInOneModel>
    {
        public static readonly DependencyProperty SingularLanguageProperty = RegisterProperty(nameof(SingularLanguage), true);
        public static readonly DependencyProperty AdditionalLanguageProperty = RegisterProperty(nameof(AdditionalLanguage), true);
        public static readonly DependencyProperty CanSignTransactionProperty = RegisterProperty(nameof(CanSignTransaction), false);
        public static readonly DependencyProperty PublicKeysProperty = RegisterProperty<string[]>(nameof(PublicKeys), null);
        public static readonly DependencyProperty ValidationMessageProperty = RegisterProperty(nameof(ValidationMessage), string.Empty);
        public static readonly DependencyProperty TransactionProperty = RegisterProperty<TransactionModel>(nameof(Transaction), null);
        public bool SingularLanguage { get { return (bool)GetValue(SingularLanguageProperty); } set { SetValue(SingularLanguageProperty, value); } }
        public bool AdditionalLanguage { get { return (bool)GetValue(AdditionalLanguageProperty); } set { SetValue(AdditionalLanguageProperty, value); } }
        public bool CanSignTransaction { get { return (bool)GetValue(CanSignTransactionProperty); } set { SetValue(CanSignTransactionProperty, value); } }
        public string[] PublicKeys { get { return (string[])GetValue(PublicKeysProperty); } set { SetValue(PublicKeysProperty, value); } }
        public string ValidationMessage { get { return (string)GetValue(ValidationMessageProperty); } set { SetValue(ValidationMessageProperty, value); } }
        public TransactionModel Transaction { get { return (TransactionModel)GetValue(TransactionProperty); } set { SetValue(TransactionProperty, value); } }

        public AllInOneModel()
        {
            // Note workaround PublicKey.Equals Bug
            PublicKeys = SigningData.PublicKeys.Select(k => Hex.FromBytes(k.Export(KeyBlobFormat.PkixPublicKey))).ToArray();
            Transaction = SigningData.TransactionBodyBytes.IsEmpty ? null : new TransactionModel(SigningData.TransactionBodyBytes);
        }
        public void TryPasteKeyFromClipboard()
        {
            var data = Clipboard.GetDataObject();
            if (data != null && data.GetDataPresent(DataFormats.UnicodeText, true))
            {
                var keyInHex = data.GetData(DataFormats.UnicodeText, true) as string;
                try
                {
                    var key = Keys.ImportPrivateEd25519KeyFromBytes(Hex.ToBytes(keyInHex));
                    if (SigningData.TryAddKey(key))
                    {
                        // Note workaround PublicKey.Equals Bug
                        PublicKeys = SigningData.PublicKeys.Select(k => Hex.FromBytes(k.Export(KeyBlobFormat.PkixPublicKey))).ToArray();
                        ValidationMessage = string.Empty;
                    }
                    else
                    {
                        ValidationMessage = "The Key pasted from the Clipbaord is already in the list.";
                    }
                }
                catch (Exception ex)
                {
                    ValidationMessage = "Unable to parse as Ed25519 Private Key: " + ex.Message;
                }
            }
            else
            {
                ValidationMessage = "It does not appear that a Hex Encoded Ed25519 Private Key is in the Clipboard.";
            }
        }
        public void TryRemoveKeyFromList(string keyInHex)
        {
            var key = Keys.ImportPublicEd25519KeyFromBytes(Hex.ToBytes(keyInHex));
            if (SigningData.TryRemoveKey(key))
            {
                // Note workaround PublicKey.Equals Bug
                PublicKeys = SigningData.PublicKeys.Select(k => Hex.FromBytes(k.Export(KeyBlobFormat.PkixPublicKey))).ToArray();
                ValidationMessage = string.Empty;
            }
        }
        public void TryPasteTransactionFromClipboard()
        {
            if (HashgraphClipboard.TryGetAsBytes<TransactionBody>("Hedera Transaction", out ReadOnlyMemory<byte> data, out string error))
            {
                Transaction = new TransactionModel(data);
                SigningData.TransactionBodyBytes = data;
                ValidationMessage = string.Empty;
            }
            else
            {
                ValidationMessage = error;
            }
        }
        internal void TrySignTransactionAndCopyToClipboard()
        {
            HashgraphClipboard.Set(SigningData.SignTransaction());
            ValidationMessage = SingularLanguage ? "Signature copied to clipboard." : "Signatures copied to clipboard.";
        }

        internal void ClearTransaction()
        {
            SigningData.TransactionBodyBytes = null;
            Transaction = null;
            ValidationMessage = string.Empty;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs evt)
        {
            base.OnPropertyChanged(evt);
            if (evt.Property == TransactionProperty)
            {
                var oldTx = evt.OldValue as TransactionModel;
                if (oldTx != null)
                {
                    oldTx.PropertyChanged -= OnTransactionModelPropertyChanged;
                }
                var newTx = evt.NewValue as TransactionModel;
                if (newTx != null)
                {
                    newTx.PropertyChanged += OnTransactionModelPropertyChanged;
                    CanSignTransaction = PublicKeys.Length > 0 && newTx.RemainingSeconds > 0;
                }
                else
                {
                    CanSignTransaction = false;
                }
            }
            else if (evt.Property == PublicKeysProperty)
            {
                CanSignTransaction = Transaction?.RemainingSeconds > 0 && PublicKeys.Length > 0;
                var singular = PublicKeys.Length < 2;
                if (singular != SingularLanguage)
                {
                    SingularLanguage = singular;
                }
                var additional = PublicKeys.Length > 0;
                if (additional != AdditionalLanguage)
                {
                    AdditionalLanguage = additional;
                }
            }
        }
        private void OnTransactionModelPropertyChanged(object sender, PropertyChangedEventArgs evt)
        {
            if (evt.PropertyName == nameof(TransactionModel.RemainingSeconds))
            {
                if (CanSignTransaction && (sender as TransactionModel).RemainingSeconds == 0)
                {
                    CanSignTransaction = false;
                }
            }
        }
    }
}
