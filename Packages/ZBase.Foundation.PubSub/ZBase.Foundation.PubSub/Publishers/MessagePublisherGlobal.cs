using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Message Publisher in the <see cref="GlobalScope"/>
    /// </summary>
    public static class MessagePublisherGlobal
    {
        /// <summary>
        /// Publishes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Publish<TMessage>(
              this MessagePublisher self
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : new()
#else
            where TMessage : IMessage, new()
#endif
        {
            self.Global().Publish<TMessage>(cancelToken, logger);
        }

        /// <summary>
        /// Publishes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Publish<TMessage>(
              this MessagePublisher self
            , TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            self.Global().Publish(message, cancelToken, logger);
        }

        /// <summary>
        /// Publishes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask PublishAsync<TMessage>(
              this MessagePublisher self
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : new()
#else
            where TMessage : IMessage, new()
#endif
        {
            return self.Global().PublishAsync<TMessage>(cancelToken, logger);
        }

        /// <summary>
        /// Publishes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask PublishAsync<TMessage>(
              this MessagePublisher self
            , TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : IMessage
#endif
        {
            return self.Global().PublishAsync(message, cancelToken, logger);
        }
    }
}
