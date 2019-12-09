using Hashgraph.SigningTool.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public partial class Step5 : UserControl
    {
        private SignAndCopyToClipboardModel _model;
        public Step5()
        {
            DataContext = _model = new SignAndCopyToClipboardModel();
            InitializeComponent();
        }

        private void OnCopyToClipboard(object sender, RoutedEventArgs e)
        {
            _model.SignAndCopyToClipboard();
        }

        private void OnStartOver(object sender, RoutedEventArgs e)
        {
            SigningData.Reset();
            this.MoveToControl<Step1>();
        }

        private void OnSignNewTransaction(object sender, RoutedEventArgs e)
        {
            SigningData.TransactionBodyBytes = null;
            this.MoveToControl<Step3>();
        }
    }
}
