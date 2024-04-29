using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    static partial class AnonSubscriberGlobal
    {
        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState>(
              this AnonSubscriber self
            , [NotNull] TState state
            , [NotNull] Action<TState> handler
            , int order = 0
        )
            where TState : class
        {
            return self.Global().Subscribe<TState>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState>(
              this AnonSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, CancellationToken, UniTask> handler
            , int order = 0
        )
            where TState : class
        {
            return self.Global().Subscribe<TState>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState>(
              this AnonSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, UniTask> handler
            , int order = 0
        )
            where TState : class
        {
            return self.Global().Subscribe<TState>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState>(
              this AnonSubscriber self
            , [NotNull] TState state
            , [NotNull] Action<TState> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
        {
            self.Global().Subscribe<TState>(state, handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState>(
              this AnonSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, CancellationToken, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
        {
            self.Global().Subscribe<TState>(state, handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState>(
              this AnonSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
        {
            self.Global().Subscribe<TState>(state, handler, unsubscribeToken, order);
        }
    }
}
