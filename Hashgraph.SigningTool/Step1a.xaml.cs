using Hashgraph.SigningTool.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public partial class Step1a : UserControl
    {
        private EnterPrivateEd25519KeyModel _model;
        public Step1a()
        {
            DataContext = _model = new EnterPrivateEd25519KeyModel();
            InitializeComponent();
        }
        private void OnConfirmPrivateKey(object sender, RoutedEventArgs e)
        {
            if (SigningData.TryAddKey(_model.PrivateKey))
            {
                this.MoveToControl<Step2>();
            }
        }
        private void OnGoBackToConfirmation(object sender, RoutedEventArgs e)
        {
            this.MoveToControl<Step2>();
        }
        private void OnPasteFromClipboard(object sender, RoutedEventArgs e)
        {
            _model.TryPasteKeyFromClipboard();
        }
    }
}
