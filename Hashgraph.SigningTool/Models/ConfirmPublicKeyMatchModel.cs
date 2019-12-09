using NSec.Cryptography;
using System.Linq;
using System.Windows;

namespace Hashgraph.SigningTool.Models
{
    public class ConfirmPublicKeyMatchModel : BindableDependencyObject<ConfirmPublicKeyMatchModel>
    {
        public static readonly DependencyProperty LatestKeyProperty = RegisterProperty<PublicKey>(nameof(LatestKey), null);
        public static readonly DependencyProperty OtherKeysProperty = RegisterProperty<PublicKey[]>(nameof(OtherKeys), null);
        public static readonly DependencyProperty SingularLanguageProperty = RegisterProperty(nameof(SingularLanguage), true);
        public PublicKey LatestKey { get { return (PublicKey)GetValue(LatestKeyProperty); } set { SetValue(LatestKeyProperty, value); } }
        public PublicKey[] OtherKeys { get { return (PublicKey[])GetValue(OtherKeysProperty); } set { SetValue(OtherKeysProperty, value); } }
        public bool SingularLanguage { get { return (bool)GetValue(SingularLanguageProperty); } set { SetValue(SingularLanguageProperty, value); } }

        public ConfirmPublicKeyMatchModel()
        {
            var keys = SigningData.PublicKeys;
            LatestKey = keys.Last();
            OtherKeys = keys[..^1];
            SingularLanguage = OtherKeys.Length < 2;
        }
    }
}