using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Message Subscriber in the <see cref="GlobalScope"/>
    /// </summary>
    public static class MessageSubscriberGlobal
    {
        /// <summary>
        /// Remove empty handler groups in the <see cref="GlobalScope"/> to optimize performance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Compress<TMessage>(this MessageSubscriber self)
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Compress<TMessage>();
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TMessage>(
              this MessageSubscriber self
            , Action handler
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TMessage>(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TMessage>(
              this MessageSubscriber self
            , Action<TMessage> handler
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TMessage>(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TMessage>(
              this MessageSubscriber self
            , Func<CancellationToken, UniTask> handler
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TMessage>(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TMessage>(
              this MessageSubscriber self
            , MessageHandler<TMessage> handler
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TMessage>(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TMessage>(
              this MessageSubscriber self
            , Func<TMessage, UniTask> handler
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TMessage>(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ISubscription Subscribe<TMessage>(
              this MessageSubscriber self
            , Func<UniTask> handler
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().Subscribe<TMessage>(handler, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TMessage>(
              this MessageSubscriber self
            , Action handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TMessage>(handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TMessage>(
              this MessageSubscriber self
            , Action<TMessage> handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TMessage>(handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TMessage>(
              this MessageSubscriber self
            , Func<CancellationToken, UniTask> handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TMessage>(handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TMessage>(
              this MessageSubscriber self
            , MessageHandler<TMessage> handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TMessage>(handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TMessage>(
              this MessageSubscriber self
            , Func<TMessage, UniTask> handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TMessage>(handler, unsubscribeToken, order);
        }

        /// <summary>
        /// Subscribes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<TMessage>(
              this MessageSubscriber self
            , Func<UniTask> handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Subscribe<TMessage>(handler, unsubscribeToken, order);
        }
    }
}
