using Hashgraph.SigningTool.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public partial class Step4 : UserControl
    {
        private SignAndCopyToClipboardModel _model;
        public Step4()
        {
            DataContext = _model = new SignAndCopyToClipboardModel();
            InitializeComponent();
        }

        private void OnSignAndCopyToClipboard(object sender, RoutedEventArgs e)
        {
            _model.SignAndCopyToClipboard();
            Window.GetWindow(this).Content = new Step5();
        }

        private void OnSignNewTransaction(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Step3();
        }
    }
}