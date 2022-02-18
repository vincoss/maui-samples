using System;
using System.Collections.Generic;

namespace ShortMvvm.Dto
{
    public class KeyDataGuidString : KeyData<Guid, string>
    {

    }

    public class KeyDataIntString : KeyData<int, string>
    {

    }

    public class KeyData<K, V> : IEquatable<KeyData<K, V>>
    {
        public K Key { get; set; }
        public V Value { get; set; }

        public override string ToString()
        {
            if (EqualityComparer<V>.Default.Equals(Value, default(V)))
            {
                return base.ToString();
            }
            return Value.ToString();
        }

        #region Override methods

        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 1521134295) + Key.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            var local = obj as KeyData<K, V>;
            if (local == null)
            {
                return false;
            }
            return Equals(local);
        }

        public bool Equals(KeyData<K, V> other)
        {
            if (other == null)
            {
                return false;
            }
            bool result = Key.Equals(other.Key);
            return result;
        }

        public static bool operator ==(KeyData<K, V> e1, KeyData<K, V> e2)
        {
            if (ReferenceEquals(e1, null))
            {
                return ReferenceEquals(e2, null);
            }
            return e1.Equals(e2);
        }

        public static bool operator !=(KeyData<K, V> e1, KeyData<K, V> e2)
        {
            return !(e1 == e2);
        }

        #endregion
    }
}
