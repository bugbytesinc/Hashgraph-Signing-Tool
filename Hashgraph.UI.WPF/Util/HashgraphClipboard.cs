using Google.Protobuf;
using System;
using System.IO;
using System.Windows;

namespace Hashgraph.UI.WPF.Util
{
    public static class HashgraphClipboard
    {
        public static bool TryGet<T>(string friendlyName, out T data, out string error) where T : IMessage<T>, new()
        {
            if (TryGetAsBytes<T>(friendlyName, out ReadOnlyMemory<byte> bytes, out error))
            {
                data = (new MessageParser<T>(() => new T())).ParseFrom(bytes.ToArray());
                return true;
            }
            else
            {
                data = default;
                return false;
            }
        }
        public static bool TryGetAsBytes<T>(string friendlyName, out ReadOnlyMemory<byte> data, out string error) where T : IMessage<T>, new()
        {
            var clipboardId = GetClipboardId<T>();
            var clipboardData = Clipboard.GetDataObject();
            var parser = new MessageParser<T>(() => new T());
            if (clipboardData == null)
            {
                data = default;
                error = "There appears to be no data on the clipboard.";
                return false;
            }
            if (clipboardData.GetDataPresent(clipboardId))
            {
                var stream = clipboardData.GetData(clipboardId) as MemoryStream;
                if (stream != null)
                {
                    var bytes = stream.ToArray();
                    if (bytes.Length > 0)
                    {
                        try
                        {
                            if (parser.ParseFrom(bytes) != null)
                            {
                                data = bytes;
                                error = null;
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            data = default;
                            error = $"Cliboard does not appear to be holding a {friendlyName}: {ex.Message}";
                            return false;
                        }
                    }
                }
            }
            // Fallback to Hex Encoded Text if Necessary
            if (clipboardData != null && clipboardData.GetDataPresent(DataFormats.UnicodeText, true))
            {
                ReadOnlyMemory<byte> bytes;
                var hex = clipboardData.GetData(DataFormats.UnicodeText, true) as string;
                try
                {
                    bytes = Hex.ToBytes(hex);
                }
                catch
                {
                    data = default;
                    error = $"The data in the clipboard did not appear to be holding a binary form of {friendlyName} nor one encoded in text as Hex.";
                    return false;
                }
                if (bytes.IsEmpty)
                {
                    data = default;
                    error = $"There appears to be no {friendlyName} data on the clipboard.";
                    return false;
                }
                try
                {
                    if (parser.ParseFrom(bytes.ToArray()) != null)
                    {
                        data = bytes;
                        error = null;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    data = default;
                    error = $"The data in the clipboard encoded as hex did not appear to be a {friendlyName}: {ex.Message}";
                    return false;
                }
            }
            data = default;
            error = "Unable to recognize the data on the clipboard.";
            return false;
        }
        public static void Set<T>(T message) where T : IMessage<T>, new()
        {
            SetAsBytes<T>(message.ToByteArray());
        }
        public static void SetAsBytes<T>(ReadOnlyMemory<byte> data) where T : IMessage<T>, new()
        {
            using (var stream = new MemoryStream())
            {
                stream.Write(data.Span);
                var clipboardData = new DataObject();
                clipboardData.SetData(GetClipboardId<T>(), stream, false);
                clipboardData.SetData(DataFormats.UnicodeText, Hex.FromBytes(data));
                Clipboard.SetDataObject(clipboardData, true);
            }
        }
        private static string GetClipboardId<T>()
        {
            return $"HederaHashgraph:Protobuf:{nameof(T)}";
        }
    }
}
