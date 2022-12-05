#if !(UNITY_EDITOR || DEBUG) || DISABLE_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    public partial class MessageSubscriber
    {
        private readonly SingletonContainer<MessageBroker> _brokers;
        private readonly CappedArrayPool<UniTask> _taskArrayPool;

        internal MessageSubscriber(SingletonContainer<MessageBroker> brokers)
        {
            _brokers = brokers;
            _taskArrayPool = new(8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<GlobalScope> Global()
        {
            return new(this, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<TScope> Scope<TScope>(TScope scope)
        {
            return new(this, scope);
        }

        public readonly struct Subscriber<TScope> : IMessageSubscriber<TScope>
        {
            private readonly MessageSubscriber _subscriber;

            public TScope Scope { get; }

            public bool IsValid => _subscriber != null;

            internal Subscriber(MessageSubscriber subscriber, TScope scope)
            {
                _subscriber = subscriber;
                Scope = scope;
            }

            /// <inheritdoc/>
#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Compress<TMessage>()
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

                if (_subscriber._brokers.TryGet<MessageBroker<TScope, TMessage>>(out var broker))
                {
                    broker.Compress(Scope);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TMessage>(
                  Action handler
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                return Subscribe<TMessage>(
                      (_, _) => {
                          handler?.Invoke();
                          return UniTask.CompletedTask;
                      }
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TMessage>(
                  Action<TMessage> handler
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                return Subscribe<TMessage>(
                      (message, _) => {
                          handler?.Invoke(message);
                          return UniTask.CompletedTask;
                      }
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TMessage>(
                  Func<UniTask> handler
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                return Subscribe<TMessage>(
                      (_, _) => handler != null ? handler() : UniTask.CompletedTask
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TMessage>(
                  Func<TMessage, UniTask> handler
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                return Subscribe<TMessage>(
                      (message, _) => handler != null ? handler(message) : UniTask.CompletedTask
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TMessage>(
                  Func<CancellationToken, UniTask> handler
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                return Subscribe<TMessage>(
                      (_, cancelToken) => handler != null ? handler(cancelToken) : UniTask.CompletedTask
                    , order
                );
            }

            public ISubscription Subscribe<TMessage>(
                  MessageHandler<TMessage> handler
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(handler, order, out var subscription);
                return subscription;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TMessage>(
                  Action handler
                , in CancellationToken unsubscribeToken
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                Subscribe<TMessage>(
                      (_, _) => { handler?.Invoke(); return UniTask.CompletedTask; }
                    , unsubscribeToken
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TMessage>(
                  Action<TMessage> handler
                , in CancellationToken unsubscribeToken
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                Subscribe<TMessage>(
                      (message, _) => { handler?.Invoke(message); return UniTask.CompletedTask; }
                    , unsubscribeToken
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TMessage>(
                  Func<UniTask> handler
                , in CancellationToken unsubscribeToken
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                Subscribe<TMessage>(
                      (_, _) => handler != null ? handler() : UniTask.CompletedTask
                    , unsubscribeToken
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TMessage>(
                  Func<TMessage, UniTask> handler
                , in CancellationToken unsubscribeToken
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                Subscribe<TMessage>(
                      (message, _) => handler != null ? handler(message) : UniTask.CompletedTask
                    , unsubscribeToken
                    , order
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TMessage>(
                  Func<CancellationToken, UniTask> handler
                , in CancellationToken unsubscribeToken
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                Subscribe<TMessage>(
                      (_, cancelToken) => handler != null ? handler(cancelToken) : UniTask.CompletedTask
                    , unsubscribeToken
                    , order
                );
            }

            public void Subscribe<TMessage>(
                  MessageHandler<TMessage> handler
                , in CancellationToken unsubscribeToken
                , int order = 0
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(handler, order, out var subscription))
                {
                    unsubscribeToken.Register(x => ((Subscription<TMessage>)x)?.Dispose(), subscription);
                }
            }

            private bool TrySubscribe<TMessage>(
                  MessageHandler<TMessage> handler
                , int order
                , out Subscription<TMessage> subscription
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    subscription = default;
                    return false;
                }
#endif

                var taskArrayPool = _subscriber._taskArrayPool;
                var brokers = _subscriber._brokers;

                lock (brokers)
                {
                    if (brokers.TryGet<MessageBroker<TScope, TMessage>>(out var broker) == false)
                    {
                        broker = new MessageBroker<TScope, TMessage>();

                        if (brokers.TryAdd(broker) == false)
                        {
                            broker?.Dispose();
                            subscription = default;
                            return false;
                        }
                    }

                    subscription = broker.Subscribe(Scope, handler, order, taskArrayPool);
                    return subscription.IsValid;
                }
            }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            private bool Validate()
            {
                if (_subscriber != null)
                {
                    return true;
                }

                UnityEngine.Debug.LogError(
                    $"{GetType().Name} must be retrieved via `{nameof(MessageSubscriber)}.{nameof(MessageSubscriber.Scope)}` API"
                );

                return false;
            }
#endif
        }
    }
}
