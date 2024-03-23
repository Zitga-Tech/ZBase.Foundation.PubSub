#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    public struct CachedPublisher<TMessage> : IDisposable
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
        where TMessage : new()
#else
        where TMessage : IMessage, new()
#endif
    {
        private MessageBroker<TMessage> _broker;

        internal CachedPublisher(MessageBroker<TMessage> broker)
        {
            _broker = broker ?? throw new System.ArgumentNullException(nameof(broker));
        }

        public readonly bool IsValid => _broker != null;

        public void Dispose()
        {
            _broker?.OnUncache();
            _broker = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Publish(CancellationToken cancelToken = default, ILogger logger = null)
        {
            Publish(new TMessage(), cancelToken, logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Publish(
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
        public readonly UniTask PublishAsync(CancellationToken cancelToken = default, ILogger logger = null)
        {
            return PublishAsync(new TMessage(), cancelToken, logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly UniTask PublishAsync(
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
        private readonly bool Validate()
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