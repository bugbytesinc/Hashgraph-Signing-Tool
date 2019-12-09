using Hashgraph.SigningTool.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public partial class Step1 : UserControl
    {
        private EnterPrivateEd25519KeyModel _model;
        public Step1()
        {
            DataContext = _model = new EnterPrivateEd25519KeyModel();
            InitializeComponent();
        }
        private void OnConfirmPrivateKey(object sender, RoutedEventArgs e)
        {
            if (_model.CanContinue && SigningData.TryAddKey(_model.PrivateKey))
            {
                this.MoveToControl<Step2>();
            }
        }
        private void OnPasteFromClipboard(object sender, RoutedEventArgs e)
        {
            _model.TryPasteKeyFromClipboard();
        }
    }
}
