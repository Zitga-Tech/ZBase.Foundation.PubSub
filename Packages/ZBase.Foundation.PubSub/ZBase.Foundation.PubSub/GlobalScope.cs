using System;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.PubSub
{
    public readonly struct GlobalScope : IEquatable<GlobalScope>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(GlobalScope other)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is GlobalScope;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => default(int).GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => nameof(GlobalScope);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(GlobalScope _, GlobalScope __)
            => true;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(GlobalScope _, GlobalScope __)
            => false;
    }
}
