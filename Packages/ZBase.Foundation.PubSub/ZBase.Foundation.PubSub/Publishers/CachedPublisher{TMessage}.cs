#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    public readonly struct CachedPublisher<TMessage>
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
        public void Publish(CancellationToken cancelToken = default, ILogger logger = null)
        {
            Publish(new TMessage(), cancelToken, logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Publish(
              TMessage message
            , CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            if (Validate() == false)
            {
                return;
            }

            if (message == null)
            {
                (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                return;
            }
#endif

            _broker.PublishAsync(message, cancelToken, logger ?? DefaultLogger.Default).Forget();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniTask PublishAsync(CancellationToken cancelToken = default, ILogger logger = null)
        {
            return PublishAsync(new TMessage(), cancelToken, logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UniTask PublishAsync(
              TMessage message
            , CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            if (Validate() == false)
            {
                return UniTask.CompletedTask;
            }

            if (message == null)
            {
                (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                return UniTask.CompletedTask;
            }
#endif

            return _broker.PublishAsync(message, cancelToken, logger ?? DefaultLogger.Default);
        }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
        private bool Validate()
        {
            if (_broker != null)
            {
                return true;
            }

            UnityEngine.Debug.LogError(
                $"{GetType()} must be retrieved via `{nameof(MessagePublisher)}.{nameof(MessagePublisher.Cache)}` API"
            );

            return false;
        }
#endif
    }
}