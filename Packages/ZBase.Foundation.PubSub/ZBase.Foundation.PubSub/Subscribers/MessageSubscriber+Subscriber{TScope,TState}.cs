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
    public static partial class MessageSubscriberExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageSubscriber.Subscriber<TScope, TState> WithState<TScope, TState>(
              this MessageSubscriber.Subscriber<TScope> subscriber
            , [NotNull] TState state
        )
            where TState : class
        {
            return new MessageSubscriber.Subscriber<TScope, TState>(subscriber, state);
        }
    }

    partial class MessageSubscriber
    {
        public readonly partial struct Subscriber<TScope, TState>
            where TState : class
        {
            internal readonly Subscriber<TScope> _subscriber;

            public bool IsValid => _subscriber.IsValid;

            public TScope Scope => _subscriber.Scope;

            public TState State { get; }

            internal Subscriber(Subscriber<TScope> subscriber, [NotNull] TState state)
            {
                _subscriber = subscriber;
                State = state;
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
                if (Validate(logger) == false) return;
#endif

                _subscriber.Compress<TMessage>(logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Action<TState> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<TMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerAction<TState, TMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Action<TState, TMessage> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<TMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerActionMessage<TState, TMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Func<TState, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<TMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerFunc<TState, TMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Func<TState, TMessage, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<TMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerFuncMessage<TState, TMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] Func<TState, CancellationToken, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<TMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerFuncCancelToken<TState, TMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe<TMessage>(
                  [NotNull] MessageHandler<TState, TMessage> handler
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<TMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerMessage<TState, TMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Action<TState> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerAction<TState, TMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Action<TState, TMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerActionMessage<TState, TMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Func<TState, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerFunc<TState, TMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Func<TState, TMessage, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerFuncMessage<TState, TMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] Func<TState, CancellationToken, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerFuncCancelToken<TState, TMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe<TMessage>(
                  [NotNull] MessageHandler<TState, TMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerMessage<TState, TMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
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
                if (IsValid == true)
                {
                    return true;
                }

                (logger ?? DefaultLogger.Default).LogError(
                    $"{GetType().Name} must be retrieved via `{nameof(MessageSubscriber)}.{nameof(MessageSubscriber.Scope)}` API"
                );

                return false;
            }
#endif
        }
    }
}
