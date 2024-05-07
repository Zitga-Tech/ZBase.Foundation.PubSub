#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#else
#define __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
#endif

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    partial class MessageSubscriber
    {
        public readonly partial struct Subscriber<TScope>
        {
            internal readonly MessageSubscriber _subscriber;

            public TScope Scope { get; }

            public bool IsValid => _subscriber != null;

            internal Subscriber([NotNull] MessageSubscriber subscriber, [NotNull] TScope scope)
            {
                _subscriber = subscriber;
                Scope = scope;
            }

            /// <summary>
            /// Remove empty handler groups to optimize performance.
            /// </summary>
#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Compress<TMessage>(ILogger logger = null)
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                if (_subscriber._brokers.TryGet<MessageBroker<TScope, TMessage>>(out var broker))
                {
                    broker.Compress(Scope);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Action handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);
                TrySubscribe(new HandlerAction<TMessage>(handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Action<TMessage> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);
                TrySubscribe(new HandlerActionMessage<TMessage>(handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Func<UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);
                TrySubscribe(new HandlerFunc<TMessage>(handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Func<TMessage, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);
                TrySubscribe(new HandlerFuncMessage<TMessage>(handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Func<CancellationToken, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);
                TrySubscribe(new HandlerFuncCancelToken<TMessage>(handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] MessageHandler<TMessage> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);
                TrySubscribe(new HandlerMessage<TMessage>(handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Action handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);

                if (TrySubscribe(new HandlerAction<TMessage>(handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Action<TMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);

                if (TrySubscribe(new HandlerActionMessage<TMessage>(handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Func<UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);

                if (TrySubscribe(new HandlerFunc<TMessage>(handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Func<TMessage, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);

                if (TrySubscribe(new HandlerFuncMessage<TMessage>(handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Func<CancellationToken, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);

                if (TrySubscribe(new HandlerFuncCancelToken<TMessage>(handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] MessageHandler<TMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                ThrowIfHandlerIsNull(handler);

                if (TrySubscribe(new HandlerMessage<TMessage>(handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

            internal bool TrySubscribe<TMessage>(
                  [NotNull] IHandler<TMessage> handler
                , int order
                , out Subscription<TMessage> subscription
                , ILogger logger
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
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

            [Conditional("__ZBASE_FOUNDATION_PUBSUB_VALIDATION__"), DoesNotReturn]
            private static void ThrowIfHandlerIsNull(Delegate handler)
            {
                if (handler == null) throw new ArgumentNullException(nameof(handler));
            }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            private bool Validate(ILogger logger)
            {
                if (_subscriber != null)
                {
                    return true;
                }

                (logger ?? DefaultLogger.Default).LogError(
                    $"{GetType()} must be retrieved via `{nameof(MessageSubscriber)}.{nameof(MessageSubscriber.Scope)}` API"
                );

                return false;
            }
#endif

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            partial void RetainUsings();
        }
    }
}
