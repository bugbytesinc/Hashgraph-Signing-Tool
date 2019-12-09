using Google.Protobuf;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Hashgraph.SigningTool.Models
{
    public class TransactionModel : BindableDependencyObject<TransactionModel>
    {
        public static readonly DependencyProperty NetworkNodeProperty = RegisterProperty(nameof(NetworkNode), string.Empty);
        public static readonly DependencyProperty TxIdProperty = RegisterProperty(nameof(TxId), string.Empty);
        public static readonly DependencyProperty MemoProperty = RegisterProperty(nameof(Memo), string.Empty);
        public static readonly DependencyProperty BodyProperty = RegisterProperty(nameof(Body), string.Empty);
        public static readonly DependencyProperty RemainingSecondsProperty = RegisterProperty<int>(nameof(RemainingSeconds), 0);
        public string NetworkNode { get { return (string)GetValue(NetworkNodeProperty); } set { SetValue(NetworkNodeProperty, value); } }
        public string TxId { get { return (string)GetValue(TxIdProperty); } set { SetValue(TxIdProperty, value); } }
        public string Memo { get { return (string)GetValue(MemoProperty); } set { SetValue(MemoProperty, value); } }
        public string Body { get { return (string)GetValue(BodyProperty); } set { SetValue(BodyProperty, value); } }
        public int RemainingSeconds { get { return (int)GetValue(RemainingSecondsProperty); } set { SetValue(RemainingSecondsProperty, value); } }
        private DateTime _expiration;
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long NanosPerTick = 1_000_000_000L / TimeSpan.TicksPerSecond;
        public TransactionModel(ReadOnlyMemory<byte> transactionBodyBytes)
        {
            var transactionBody = Proto.TransactionBody.Parser.ParseFrom(transactionBodyBytes.ToArray());
            var payer = $"{transactionBody.TransactionID.AccountID.ShardNum}.{transactionBody.TransactionID.AccountID.RealmNum}.{transactionBody.TransactionID.AccountID.AccountNum}";
            var timestamp = EPOCH.AddTicks(transactionBody.TransactionID.TransactionValidStart.Seconds * TimeSpan.TicksPerSecond + transactionBody.TransactionID.TransactionValidStart.Nanos / NanosPerTick);
            _expiration = timestamp.AddSeconds(transactionBody.TransactionValidDuration.Seconds);
            NetworkNode = $"{transactionBody.NodeAccountID.ShardNum}.{transactionBody.NodeAccountID.RealmNum}.{transactionBody.NodeAccountID.AccountNum}";
            TxId = $"{payer}@{timestamp:s}";
            Memo = transactionBody.Memo;
            Body = JsonSerializer.Serialize(JsonDocument.Parse(JsonFormatter.Default.Format(transactionBody)).RootElement, new JsonSerializerOptions { WriteIndented = true });
            StartCountDown();
        }
        private async void StartCountDown()
        {
            do
            {
                RemainingSeconds = Math.Max((int)(_expiration - DateTime.UtcNow).TotalSeconds, 0);
                await Task.Delay(1000);
            } while (RemainingSeconds > 0);
        }
    }
}
