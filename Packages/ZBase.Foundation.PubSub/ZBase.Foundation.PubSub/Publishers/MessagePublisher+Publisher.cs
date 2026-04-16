#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#else
#define __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
#endif

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    partial class MessagePublisher
    {
        public readonly struct Publisher<TScope>
        {
            internal readonly MessagePublisher _publisher;

            public bool IsValid => _publisher != null;

            public TScope Scope { get; }

            internal Publisher([NotNull] MessagePublisher publisher, [NotNull] TScope scope)
            {
                _publisher = publisher;
                Scope = scope;
            }

            public CachedPublisher<TMessage> Cache<TMessage>(ILogger logger = null)
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
                {
                    return default;
                }
#endif

                var scopedBroker = BrokerStore<TScope, TMessage>.GetOrCreate();
                var broker = scopedBroker.Cache(Scope, _publisher._taskArrayPool);
                return new CachedPublisher<TMessage>(broker);
            }
            
#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Publish<TMessage>(
                  CancellationToken token = default
                , ILogger logger = null
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    broker.PublishAsync(Scope, new TMessage(), default, token, logger ?? DefaultLogger.Default).Forget();
                }
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Publish<TMessage>(
                  TMessage message
                , CancellationToken token = default
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(message, logger) == false)
                {
                    return;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    broker.PublishAsync(Scope, message, default, token, logger ?? DefaultLogger.Default).Forget();
                }
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public UniTask PublishAsync<TMessage>(
                  CancellationToken token = default
                , ILogger logger = null
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    return broker.PublishAsync(Scope, new TMessage(), default, token, logger ?? DefaultLogger.Default);
                }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif

                return UniTask.CompletedTask;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public UniTask PublishAsync<TMessage>(
                  TMessage message
                , CancellationToken token = default
                , ILogger logger = null
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(message, logger) == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    return broker.PublishAsync(Scope, message, default, token, logger ?? DefaultLogger.Default);
                }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif

                return UniTask.CompletedTask;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void PublishWithContext<TMessage>(
                  CancellationToken token = default
                , ILogger logger = null
                , [CallerLineNumber] int callerLineNumber = 0
                , [CallerMemberName] string callerMemberName = ""
                , [CallerFilePath] string callerFilePath = ""
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
                {
                    return;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
                    var context = new PublishingContext(caller);
                    broker.PublishAsync(Scope, new TMessage(), context, token, logger ?? DefaultLogger.Default).Forget();
                }
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void PublishWithContext<TMessage>(
                  TMessage message
                , CancellationToken token = default
                , ILogger logger = null
                , [CallerLineNumber] int callerLineNumber = 0
                , [CallerMemberName] string callerMemberName = ""
                , [CallerFilePath] string callerFilePath = ""
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(message, logger) == false)
                {
                    return;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
                    var context = new PublishingContext(caller);
                    broker.PublishAsync(Scope, message, context, token, logger ?? DefaultLogger.Default).Forget();
                }
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public UniTask PublishWithContextAsync<TMessage>(
                  CancellationToken token = default
                , ILogger logger = null
                , [CallerLineNumber] int callerLineNumber = 0
                , [CallerMemberName] string callerMemberName = ""
                , [CallerFilePath] string callerFilePath = ""
            )
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : new()
#else
                where TMessage : IMessage, new()
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
                    var context = new PublishingContext(caller);
                    return broker.PublishAsync(Scope, new TMessage(), context, token, logger ?? DefaultLogger.Default);
                }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif

                return UniTask.CompletedTask;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public UniTask PublishWithContextAsync<TMessage>(
                  TMessage message
                , CancellationToken token = default
                , ILogger logger = null
                , [CallerLineNumber] int callerLineNumber = 0
                , [CallerMemberName] string callerMemberName = ""
                , [CallerFilePath] string callerFilePath = ""
            )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(message, logger) == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                var broker = BrokerStore<TScope, TMessage>.Get();

                if (broker != null)
                {
                    var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
                    var context = new PublishingContext(caller);
                    return broker.PublishAsync(Scope, message, context, token, logger ?? DefaultLogger.Default);
                }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                else
                {
                    LogWarning<TMessage>(Scope, logger);
                }
#endif

                return UniTask.CompletedTask;
            }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            private bool Validate(ILogger logger)
            {
                if (_publisher == null)
                {
                    (logger ?? DefaultLogger.Default).LogError(
                        $"{GetType()} must be retrieved via `{nameof(MessagePublisher)}.{nameof(MessagePublisher.Scope)}` API"
                    );

                    return false;
                }

                if (Scope == null)
                {
                    (logger ?? DefaultLogger.Default).LogException(new System.NullReferenceException(nameof(Scope)));
                    return false;
                }

                return true;
            }

            private bool Validate<TMessage>(TMessage message, ILogger logger)
            {
                if (_publisher == null)
                {
                    (logger ?? DefaultLogger.Default).LogError(
                        $"{GetType()} must be retrieved via `{nameof(MessagePublisher)}.{nameof(MessagePublisher.Scope)}` API"
                    );

                    return false;
                }

                if (Scope == null)
                {
                    (logger ?? DefaultLogger.Default).LogException(new System.NullReferenceException(nameof(Scope)));
                    return false;
                }

                if (message == null)
                {
                    (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                    return false;
                }

                return true;
            }

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
