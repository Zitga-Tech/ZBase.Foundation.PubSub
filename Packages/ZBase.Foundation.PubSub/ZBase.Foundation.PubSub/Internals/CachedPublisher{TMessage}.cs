#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class CachedPublisher<TMessage> : ZBase.Foundation.PubSub.CachedPublisher<TMessage>
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
        where TMessage : new()
#else
        where TMessage : IMessage, new()
#endif
    {
        private readonly MessageBroker<TMessage> _broker;

        internal CachedPublisher(MessageBroker<TMessage> broker)
        {
            _broker = broker ?? throw new System.ArgumentNullException(nameof(broker));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void Publish(in CancellationToken cancelToken = default, ILogger logger = null)
        {
            Publish(new TMessage(), cancelToken, logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void Publish(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            if (message == null)
            {
                (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                return;
            }
#endif

            _broker.PublishAsync(message, cancelToken, logger ?? DefaultLogger.Default).Forget();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override UniTask PublishAsync(in CancellationToken cancelToken = default, ILogger logger = null)
        {
            return PublishAsync(new TMessage(), cancelToken, logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override UniTask PublishAsync(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            if (message == null)
            {
                (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                return UniTask.CompletedTask;
            }
#endif

            return _broker.PublishAsync(message, cancelToken, logger ?? DefaultLogger.Default);
        }
    }
}
