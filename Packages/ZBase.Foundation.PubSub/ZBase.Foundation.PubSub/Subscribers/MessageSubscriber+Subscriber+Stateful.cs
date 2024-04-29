#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    partial class MessageSubscriber
    {
        partial struct Subscriber<TScope>
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Action<TState> handler
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new StatefulHandlerAction<TState, TMessage>(state, handler), order, out var subscription, logger);
                return subscription;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Action<TState, TMessage> handler
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new StatefulHandlerActionMessage<TState, TMessage>(state, handler), order, out var subscription, logger);
                return subscription;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Func<TState, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new StatefulHandlerFunc<TState, TMessage>(state, handler), order, out var subscription, logger);
                return subscription;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Func<TState, TMessage, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new StatefulHandlerFuncMessage<TState, TMessage>(state, handler), order, out var subscription, logger);
                return subscription;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public ISubscription Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Func<TState, CancellationToken, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new StatefulHandlerFuncCancelToken<TState, TMessage>(state, handler), order, out var subscription, logger);
                return subscription;
            }

            public ISubscription Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] MessageHandler<TState, TMessage> handler
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                TrySubscribe(new StatefulHandlerMessage<TState, TMessage>(state, handler), order, out var subscription, logger);
                return subscription;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Action<TState> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new StatefulHandlerAction<TState, TMessage>(state, handler), order, out var subscription, logger))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Action<TState, TMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new StatefulHandlerActionMessage<TState, TMessage>(state, handler), order, out var subscription, logger))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Func<TState, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new StatefulHandlerFunc<TState, TMessage>(state, handler), order, out var subscription, logger))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Func<TState, TMessage, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new StatefulHandlerFuncMessage<TState, TMessage>(state, handler), order, out var subscription, logger))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] Func<TState, CancellationToken, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new StatefulHandlerFuncCancelToken<TState, TMessage>(state, handler), order, out var subscription, logger))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Subscribe<TState, TMessage>(
                  [NotNull] TState state
                , [NotNull] MessageHandler<TState, TMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
                where TState : class
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (TrySubscribe(new StatefulHandlerMessage<TState, TMessage>(state, handler), order, out var subscription, logger))
                {
                    RegisterUnsubscription(subscription, unsubscribeToken);
                }
            }
        }
    }
}
