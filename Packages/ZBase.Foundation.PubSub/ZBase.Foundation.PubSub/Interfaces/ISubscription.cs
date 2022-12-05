using System;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.PubSub
{
    public interface ISubscription : IDisposable
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unsubscribe() 
            => Dispose();
    }
}
