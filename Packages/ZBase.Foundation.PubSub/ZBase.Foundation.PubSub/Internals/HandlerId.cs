using System;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.PubSub.Internals
{
    internal readonly struct HandlerId : IEquatable<HandlerId>
    {
        public static readonly HandlerId Null = default;

        private readonly IntPtr _method;
        private readonly int _delegate;

        public HandlerId(Delegate @delegate) : this()
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
        }

        public override string ToString()
            => $"{_delegate}+{_method}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HandlerId other)
            => _delegate == other._delegate && _method == other._method;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is HandlerId other && _delegate == other._delegate && _method == other._method;

        public override int GetHashCode()
            => HashCode.Combine(_delegate, _method);

        public static bool operator ==(HandlerId left, HandlerId right)
            => left._delegate == right._delegate && left._method == right._method;

        public static bool operator !=(HandlerId left, HandlerId right)
            => left._delegate != right._delegate || left._method != right._method;
    }
}
