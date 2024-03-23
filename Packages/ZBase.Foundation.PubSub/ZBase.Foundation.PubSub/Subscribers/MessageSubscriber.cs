#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new HandlerAction<TMessage>(handler), order, out var subscription);
                return subscription;
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new HandlerActionMessage<TMessage>(handler), order, out var subscription);
                return subscription;
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new HandlerFunc<TMessage>(handler), order, out var subscription);
                return subscription;
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new HandlerFuncMessage<TMessage>(handler), order, out var subscription);
                return subscription;
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new HandlerFuncCancelToken<TMessage>(handler), order, out var subscription);
                return subscription;
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

                TrySubscribe(new HandlerMessage<TMessage>(handler), order, out var subscription);
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new HandlerAction<TMessage>(handler), order, out var subscription))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new HandlerActionMessage<TMessage>(handler), order, out var subscription))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new HandlerFunc<TMessage>(handler), order, out var subscription))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new HandlerFuncMessage<TMessage>(handler), order, out var subscription))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
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
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new HandlerFuncCancelToken<TMessage>(handler), order, out var subscription))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

                if (TrySubscribe(new HandlerMessage<TMessage>(handler), order, out var subscription))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void RegisterUnsubscription<TMessage>(
                  Subscription<TMessage> subscription
                , CancellationToken unsubscribeToken
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                unsubscribeToken.Register(static x => ((Subscription<TMessage>)x)?.Dispose(), subscription);
            }

            private bool TrySubscribe<TMessage>(
                  IHandler<TMessage> handler
                , int order
                , out Subscription<TMessage> subscription
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    subscription = Subscription<TMessage>.None;
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
                            subscription = Subscription<TMessage>.None;
                            return false;
                        }
                    }

                    subscription = broker.Subscribe(Scope, handler, order, taskArrayPool);
                    return true;
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
