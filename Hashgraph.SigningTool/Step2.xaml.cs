using Hashgraph.SigningTool.Models;
using System.Windows;
using System.Windows.Controls;

namespace Hashgraph.SigningTool
{
    public partial class Step2 : UserControl
    {
        public Step2()
        {
            DataContext = new ConfirmPublicKeyMatchModel();
            InitializeComponent();
        }
        private void OnConfirmKeys(object sender, RoutedEventArgs e)
        {
            this.MoveToControl<Step3>();
        }
        private void OnAddNewKey(object sender, RoutedEventArgs e)
        {
            this.MoveToControl<Step1a>();
        }
        private void OnClearKey(object sender, RoutedEventArgs e)
        {
            if (SigningData.PopCurrentKey())
            {
                this.MoveToControl<Step1a>();
            }
            else
            {
                this.MoveToControl<Step1>();
            }
        }
    }
}
