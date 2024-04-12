using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Anonymous Subscriber allows registering handlers that take no message argument
    /// to the <see cref="GlobalScope"/>
    /// </summary>
    public static class AnonSubscriberGlobal
    {
        /// <summary>
        /// Remove empty handler groups in the <see cref="GlobalScope"/> to optimize performance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Compress(this AnonSubscriber self)
        {
            self.Global().Compress();
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe(
              this AnonSubscriber self
            , [NotNull] Action handler
            , int order = 0
        )
        {
            return self.Global().Subscribe(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe(
              this AnonSubscriber self
            , [NotNull] Func<CancellationToken, UniTask> handler
            , int order = 0
        )
        {
            return self.Global().Subscribe(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe(
              this AnonSubscriber self
            , [NotNull] Func<UniTask> handler
            , int order = 0
        )
        {
            return self.Global().Subscribe(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe(
              this AnonSubscriber self
            , [NotNull] Action handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
        {
            self.Global().Subscribe(handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe(
              this AnonSubscriber self
            , [NotNull] Func<CancellationToken, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
        {
            self.Global().Subscribe(handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe(
              this AnonSubscriber self
            , [NotNull] Func<UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
        {
            self.Global().Subscribe(handler, unsubscribeToken, order);
        }
    }
}
