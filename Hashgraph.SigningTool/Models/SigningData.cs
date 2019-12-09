using Google.Protobuf;
using NSec.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hashgraph.SigningTool.Models
{
    public static class SigningData
    {
        private static List<Key> _keys = new List<Key>();
        public static ReadOnlyMemory<byte> TransactionBodyBytes { get; set; }
        public static PublicKey[] PublicKeys
        {
            get
            {
                return _keys.Select(key => key.PublicKey).ToArray();
            }
        }
        public static bool Contains(Key privateKey)
        {
            var pub = privateKey.PublicKey;
            foreach (var existing in _keys)
            {
                if (pub.Equals(existing.PublicKey))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool TryAddKey(Key privateKey)
        {
            if (!Contains(privateKey))
            {
                _keys.Add(privateKey);
                return true;
            }
            return false;
        }
        public static bool TryRemoveKey(PublicKey key)
        {
            foreach (var existing in _keys)
            {
                if (key.Equals(existing.PublicKey))
                {
                    return _keys.Remove(existing);
                }
            }
            return false;
        }
        public static bool PopCurrentKey()
        {
            if (_keys.Count > 0)
            {
                _keys.RemoveAt(_keys.Count - 1);
            }
            return _keys.Count > 0;
        }
        public static Proto.SignatureMap SignTransaction()
        {
            var signatures = new Proto.SignatureMap();
            var transaction = TransactionBodyBytes.Span;
            foreach (var key in _keys)
            {
                signatures.SigPair.Add(new Proto.SignaturePair
                {
                    PubKeyPrefix = ByteString.CopyFrom(key.PublicKey.Export(KeyBlobFormat.PkixPublicKey).TakeLast(32).Take(6).ToArray()),
                    Ed25519 = ByteString.CopyFrom(SignatureAlgorithm.Ed25519.Sign(key, transaction))
                });
            }
            return signatures;
        }
        public static void Reset()
        {
            _keys.Clear();
            TransactionBodyBytes = null;
        }
    }
}