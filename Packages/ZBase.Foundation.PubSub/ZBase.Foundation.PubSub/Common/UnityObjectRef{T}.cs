#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#else
#define __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
#endif

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.PubSub
{
    public readonly struct UnityObjectRef<T> : IEquatable<UnityObjectRef<T>>
        where T : UnityEngine.Object
    {
        private readonly int _instanceId;
        private readonly byte _created;

        public UnityObjectRef([NotNull] UnityEngine.Object obj)
        {
            if (obj == false) throw new ArgumentNullException(nameof(obj));
            _instanceId = obj.GetInstanceID();
            _created = byte.MaxValue;
        }

        public bool IsCreated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _created != 0;
        }

        public int InstanceId
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _instanceId;
        }

        public T ToObject()
        {
            ThrowIfNotCreated(IsCreated);
            return UnityEngine.Resources.InstanceIDToObject(_instanceId) as T;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(UnityObjectRef<T> other)
            => _created == other._created && _instanceId == other._instanceId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is UnityObjectRef<T> other && _created == other._created && _instanceId == other._instanceId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => HashCode.Combine(this._created, this._instanceId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => IsCreated ? ToObject().ToString() : _instanceId.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UnityObjectRef<T>([NotNull] UnityEngine.Object obj)
            => new(obj);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(UnityObjectRef<T> left, UnityObjectRef<T> right)
            => left._created == right._created && left._instanceId == right._instanceId;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(UnityObjectRef<T> left, UnityObjectRef<T> right)
            => left._created != right._created || left._instanceId != right._instanceId;

        [Conditional("__ZBASE_FOUNDATION_PUBSUB_VALIDATION__"), DoesNotReturn]
        private static void ThrowIfNotCreated(bool value)
        {
            if (value == false)
            {
                throw new InvalidOperationException("UnityObjectRef must be created properly");
            }
        }
    }
}
