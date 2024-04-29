using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    static partial class MessageSubscriberGlobal
    {
        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Action<TState> handler
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TState, TMessage>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Action<TState, TMessage> handler
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TState, TMessage>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, CancellationToken, UniTask> handler
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TState, TMessage>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] MessageHandler<TState, TMessage> handler
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TState, TMessage>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, TMessage, UniTask> handler
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TState, TMessage>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, UniTask> handler
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TState, TMessage>(state, handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Action<TState> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Action<TState, TMessage> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, CancellationToken, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] MessageHandler<TState, TMessage> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, TMessage, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TState, TMessage>(
              this MessageSubscriber self
            , [NotNull] TState state
            , [NotNull] Func<TState, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
        )
            where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order);
        }
    }
}
