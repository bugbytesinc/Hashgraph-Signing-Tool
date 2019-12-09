using Hashgraph.SigningTool.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public partial class Step3 : UserControl
    {
        private PasteTransactionFromClipboardModel _model;
        public Step3()
        {
            DataContext = _model = new PasteTransactionFromClipboardModel();
            InitializeComponent();
        }
        private void OnPasteTransactionFromClipboard(object sender, RoutedEventArgs e)
        {
            if (_model.TryPasteFromClipboard())
            {
                this.MoveToControl<Step4>();
            }
        }
        private void OnChangeSigningKeys(object sender, RoutedEventArgs e)
        {
            this.MoveToControl<Step2>();
        }
    }
}