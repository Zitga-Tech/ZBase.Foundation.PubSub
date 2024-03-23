#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    public partial class MessagePublisher
    {
        private readonly SingletonContainer<MessageBroker> _brokers;
        private readonly CappedArrayPool<UniTask> _taskArrayPool;

        internal MessagePublisher(
              SingletonContainer<MessageBroker> brokers
            , CappedArrayPool<UniTask> taskArrayPool
        )
        {
            _brokers = brokers;
            _taskArrayPool = taskArrayPool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Publisher<GlobalScope> Global()
        {
            return new(this, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Publisher<TScope> Scope<TScope>(TScope scope)
        {
            return new(this, scope);
        }

        public CachedPublisher<TMessage> Cache<TScope, TMessage>(TScope scope, ILogger logger = null)
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
            where TMessage : new()
#else
            where TMessage : IMessage, new()
#endif
        {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            if (scope == null)
            {
                (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(scope)));
                return default;
            }
#endif

            var brokers = _brokers;

            lock (brokers)
            {
                if (brokers.TryGet<MessageBroker<TScope, TMessage>>(out var scopedBroker) == false)
                {
                    scopedBroker = new MessageBroker<TScope, TMessage>();

                    if (brokers.TryAdd(scopedBroker) == false)
                    {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                        (logger ?? DefaultLogger.Default).LogError(
                            $"Something went wrong when registering a new instance of {typeof(MessageBroker<TScope, TMessage>)}!"
                        );
#endif

                        scopedBroker?.Dispose();
                        return default;
                    }
                }

                var broker = scopedBroker.Cache(scope, _taskArrayPool);
                return new CachedPublisher<TMessage>(broker);
            }
        }

        public readonly struct Publisher<TScope> : IMessagePublisher<TScope>
        {
            private readonly MessagePublisher _publisher;

            public TScope Scope { get; }

            public bool IsValid => _publisher != null;

            internal Publisher(MessagePublisher publisher, TScope scope)
            {
                _publisher = publisher;
                Scope = scope;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Publish<TMessage>(
                  CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
                Publish<TMessage>(new(), cancelToken, logger);
            }

            public void Publish<TMessage>(
                  TMessage message
                , CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return;
                }

                if (Scope == null)
                {
                    (logger ?? DefaultLogger.Default).LogException(new System.NullReferenceException(nameof(Scope)));
                    return;
                }

                if (message == null)
                {
                    (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                    return;
                }
#endif

                if (_publisher._brokers.TryGet<MessageBroker<TScope, TMessage>>(out var broker))
                {
                    broker.PublishAsync(Scope, message, cancelToken, logger ?? DefaultLogger.Default).Forget();
                }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public UniTask PublishAsync<TMessage>(
                  CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
                return PublishAsync<TMessage>(new(), cancelToken, logger);
            }

            public UniTask PublishAsync<TMessage>(
                  TMessage message
                , CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return UniTask.CompletedTask;
                }

                if (Scope == null)
                {
                    (logger ?? DefaultLogger.Default).LogException(new System.NullReferenceException(nameof(Scope)));
                    return UniTask.CompletedTask;
                }

                if (message == null)
                {
                    (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                    return UniTask.CompletedTask;
                }
#endif

                if (_publisher._brokers.TryGet<MessageBroker<TScope, TMessage>>(out var broker))
                {
                    return broker.PublishAsync(Scope, message, cancelToken, logger ?? DefaultLogger.Default);
                }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif

                return UniTask.CompletedTask;
            }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            private bool Validate()
            {
                if (_publisher != null)
                {
                    return true;
                }

                UnityEngine.Debug.LogError(
                    $"{GetType()} must be retrieved via `{nameof(MessagePublisher)}.{nameof(MessagePublisher.Scope)}` API"
                );

                return false;
            }
#endif

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            private static void LogWarning<TMessage>(TScope scope, ILogger logger)
            {
                (logger ?? DefaultLogger.Default).LogWarning(
                    $"Found no subscription for `{typeof(TMessage)}` in scope `{scope}`"
                );
            }
#endif
        }
    }
}
