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
    public static partial class AnonSubscriberExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AnonSubscriber.Subscriber<TScope, TState> WithState<TScope, TState>(
              this AnonSubscriber.Subscriber<TScope> subscriber
            , [NotNull] TState state
        )
            where TState : class
        {
            return new AnonSubscriber.Subscriber<TScope, TState>(subscriber, state);
        }
    }

    public partial class AnonSubscriber
    {
        /// <summary>
        /// Anonymous Subscriber allows registering handlers that take no message argument
        /// </summary>
        public readonly partial struct Subscriber<TScope, TState>
            where TState : class
        {
            internal readonly MessageSubscriber.Subscriber<TScope> _subscriber;

            public bool IsValid => _subscriber.IsValid;

            public TScope Scope => _subscriber.Scope;

            public TState State { get; }

            internal Subscriber(Subscriber<TScope> subscriber, [NotNull] TState state)
            {
                _subscriber = subscriber._subscriber;
                State = state;
            }

            /// <summary>
            /// Remove empty handler groups to optimize performance.
            /// </summary>
#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Compress(ILogger logger = null)
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                _subscriber.Compress<AnonMessage>(logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  [NotNull] Action<TState> handler
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<AnonMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerAction<TState, AnonMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  [NotNull] Action<TState, AnonMessage> handler
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<AnonMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerActionMessage<TState, AnonMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  [NotNull] Func<TState, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<AnonMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerFunc<TState, AnonMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  [NotNull] Func<TState, AnonMessage, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<AnonMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerFuncMessage<TState, AnonMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  [NotNull] Func<TState, CancellationToken, UniTask> handler
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<AnonMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerFuncCancelToken<TState, AnonMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  [NotNull] MessageHandler<TState, AnonMessage> handler
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return Subscription<AnonMessage>.None;
#endif

                ThrowIfHandlerIsNull(handler);

                _subscriber.TrySubscribe(new StatefulHandlerMessage<TState, AnonMessage>(State, handler), order, out var subscription, logger);
                return subscription;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  [NotNull] Action<TState> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerAction<TState, AnonMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  [NotNull] Action<TState, AnonMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerActionMessage<TState, AnonMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  [NotNull] Func<TState, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerFunc<TState, AnonMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  [NotNull] Func<TState, AnonMessage, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerFuncMessage<TState, AnonMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  [NotNull] Func<TState, CancellationToken, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerFuncCancelToken<TState, AnonMessage>(State, handler), order, out var subscription, logger))
                {
                    subscription.RegisterTo(unsubscribeToken);
                }
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  [NotNull] MessageHandler<TState, AnonMessage> handler
                , CancellationToken unsubscribeToken
                , int order = 0
                , ILogger logger = null
            )
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false) return;
#endif

                ThrowIfHandlerIsNull(handler);

                if (_subscriber.TrySubscribe(new StatefulHandlerMessage<TState, AnonMessage>(State, handler), order, out var subscription, logger))
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
                    $"{GetType().Name} must be retrieved via `{nameof(AnonSubscriber)}.{nameof(AnonSubscriber.Scope)}` API"
                );

                return false;
            }
#endif
        }
    }
}
