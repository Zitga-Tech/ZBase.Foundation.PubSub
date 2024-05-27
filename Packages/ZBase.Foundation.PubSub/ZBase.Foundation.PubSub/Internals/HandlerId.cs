using System;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.PubSub.Internals
{
    internal readonly struct HandlerId : IEquatable<HandlerId>
    {
        public static readonly HandlerId Null = default;

        private readonly IntPtr _method;
        private readonly int _delegate;
        private readonly int _stateHash;

        public HandlerId(Delegate @delegate, int stateHash = 0) : this()
        {
            if (@delegate == null)
            {
                _method = IntPtr.Zero;
                _delegate = 0;
            }
            else
            {
                _method = @delegate.Method.MethodHandle.Value;
                _delegate = @delegate.GetHashCode();
            }

            _stateHash = stateHash;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => $"{_delegate}+{_method}+{_stateHash}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HandlerId other)
            => _delegate == other._delegate && _method == other._method && _stateHash == other._stateHash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is HandlerId other && _delegate == other._delegate && _method == other._method && _stateHash == other._stateHash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => HashCode.Combine(_delegate, _method, _stateHash);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(HandlerId left, HandlerId right)
            => left._delegate == right._delegate && left._method == right._method && left._stateHash == right._stateHash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HandlerId left, HandlerId right)
            => left._delegate != right._delegate || left._method != right._method || left._stateHash != right._stateHash;
    }
}
