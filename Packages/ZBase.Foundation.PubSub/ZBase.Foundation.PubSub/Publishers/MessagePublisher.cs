#if !(UNITY_EDITOR || DEBUG) || DISABLE_DEBUG
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
        private static readonly DefaultLogger s_defaultLogger = new();

        private readonly SingletonContainer<MessageBroker> _brokers;

        internal MessagePublisher(SingletonContainer<MessageBroker> brokers)
        {
            _brokers = brokers;
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
                  in CancellationToken cancelToken = default
                , ILogger logger = null
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
                Publish<TMessage>(new(), cancelToken);
            }

            public void Publish<TMessage>(
                  TMessage message
                , in CancellationToken cancelToken = default
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
#endif

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Scope == null)
                {
                    (logger ?? s_defaultLogger).LogException(new System.NullReferenceException(nameof(Scope)));
                    return;
                }

                if (message == null)
                {
                    (logger ?? s_defaultLogger).LogException(new System.ArgumentNullException(nameof(message)));
                    return;
                }
#endif

                if (_publisher._brokers.TryGet<MessageBroker<TScope, TMessage>>(out var broker))
                {
                    broker.PublishAsync(Scope, message, cancelToken, logger ?? s_defaultLogger).Forget();
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
                  in CancellationToken cancelToken = default
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
                , in CancellationToken cancelToken = default
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
#endif

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Scope == null)
                {
                    (logger ?? s_defaultLogger).LogException(new System.NullReferenceException(nameof(Scope)));
                    return UniTask.CompletedTask;
                }

                if (message == null)
                {
                    (logger ?? s_defaultLogger).LogException(new System.ArgumentNullException(nameof(message)));
                    return UniTask.CompletedTask;
                }
#endif

                if (_publisher._brokers.TryGet<MessageBroker<TScope, TMessage>>(out var broker))
                {
                    return broker.PublishAsync(Scope, message, cancelToken, logger ?? s_defaultLogger);
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
                    $"{GetType().Name} must be retrieved via `{nameof(MessagePublisher)}.{nameof(MessagePublisher.Scope)}` API"
                );

                return false;
            }
#endif

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            private static void LogWarning<TMessage>(TScope scope, ILogger logger)
            {
                (logger ?? s_defaultLogger).LogWarning(
                    $"Found no subscription for `{typeof(TMessage).Name}` in scope `{scope}`"
                );
            }
#endif
        }
    }
}
