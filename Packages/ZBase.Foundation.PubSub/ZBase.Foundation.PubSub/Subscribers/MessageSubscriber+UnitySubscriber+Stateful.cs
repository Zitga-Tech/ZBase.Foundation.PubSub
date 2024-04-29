#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    partial class MessageSubscriber
    {
        partial struct UnitySubscriber<TScope>
        {
#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return Subscription<TMessage>.None;
                }
#endif

                return _subscriber.Subscribe<TState, TMessage>(state, handler, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return Subscription<TMessage>.None;
                }
#endif

                return _subscriber.Subscribe<TState, TMessage>(state, handler, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return Subscription<TMessage>.None;
                }
#endif

                return _subscriber.Subscribe<TState, TMessage>(state, handler, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return Subscription<TMessage>.None;
                }
#endif

                return _subscriber.Subscribe<TState, TMessage>(state, handler, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return Subscription<TMessage>.None;
                }
#endif

                return _subscriber.Subscribe<TState, TMessage>(state, handler, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return Subscription<TMessage>.None;
                }
#endif

                return _subscriber.Subscribe<TState, TMessage>(state, handler, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<TState, TMessage>(state, handler, unsubscribeToken, order, logger);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            partial void StatefulRetainUsings();
        }
    }
}
