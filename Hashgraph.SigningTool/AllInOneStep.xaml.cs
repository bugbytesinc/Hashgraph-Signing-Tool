using Hashgraph.UI.WPF.Util;
using Hashgraph.SigningTool.Models;
using NSec.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hashgraph.SigningTool
{
    /// <summary>
    /// Interaction logic for AllInOneStep.xaml
    /// </summary>
    public partial class AllInOneStep : UserControl
    {
        private AllInOneModel _model;
        public AllInOneStep()
        {
            DataContext = _model = new AllInOneModel();
            InitializeComponent();
        }

        private void OnPasteKeyFromClipboard(object sender, RoutedEventArgs e)
        {
            _model.TryPasteKeyFromClipboard();
        }

        private void OnRemovePrivateKey(object sender, RoutedEventArgs e)
        {
            var keyInHex = ((Button)sender).Tag as string;
            if (keyInHex != null)
            {
                _model.TryRemoveKeyFromList(keyInHex);
            }
        }

        private void OnPasteTransactionFromClipboard(object sender, RoutedEventArgs e)
        {
            _model.TryPasteTransactionFromClipboard();
        }

        private void OnSignTransaction(object sender, RoutedEventArgs e)
        {
            _model.TrySignTransactionAndCopyToClipboard();
        }

        private void OnClearTransaction(object sender, RoutedEventArgs e)
        {
            _model.ClearTransaction();
        }
    }
}
