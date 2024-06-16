#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#else
#define __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
#endif

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    public struct CachedPublisher<TMessage> : IDisposable
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
        where TMessage : new()
#else
        where TMessage : IMessage, new()
#endif
    {
        internal MessageBroker<TMessage> _broker;

        internal CachedPublisher(MessageBroker<TMessage> broker)
        {
            _broker = broker ?? throw new System.ArgumentNullException(nameof(broker));
        }

        public readonly bool IsValid => _broker != null;

        public void Dispose()
        {
            _broker?.OnUncache();
            _broker = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly void Publish(CancellationToken token = default, ILogger logger = null)
        {
            Publish(new TMessage(), token, logger);
        }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public readonly void Publish(
              TMessage message
            , CancellationToken token = default
            , ILogger logger = null
        )
        {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            if (Validate(message, logger) == false)
            {
                return;
            }
#endif

            _broker.PublishAsync(message, default, token, logger ?? DefaultLogger.Default).Forget();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly UniTask PublishAsync(CancellationToken token = default, ILogger logger = null)
        {
            return PublishAsync(new TMessage(), token, logger);
        }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public readonly UniTask PublishAsync(
              TMessage message
            , CancellationToken token = default
            , ILogger logger = null
        )
        {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            if (Validate(message, logger) == false)
            {
                return UniTask.CompletedTask;
            }
#endif

            return _broker.PublishAsync(message, default, token, logger ?? DefaultLogger.Default);
        }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public readonly void PublishWithContext(
              CancellationToken token = default
            , ILogger logger = null
            , [CallerLineNumber] int callerLineNumber = 0
            , [CallerMemberName] string callerMemberName = ""
            , [CallerFilePath] string callerFilePath = ""
        )
        {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            if (Validate(logger) == false)
            {
                return;
            }
#endif

            var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
            var context = new PublishingContext(caller);
            _broker.PublishAsync(new TMessage(), context, token, logger ?? DefaultLogger.Default).Forget();
        }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public readonly void PublishWithContext(
              TMessage message
            , CancellationToken token = default
            , ILogger logger = null
            , [CallerLineNumber] int callerLineNumber = 0
            , [CallerMemberName] string callerMemberName = ""
            , [CallerFilePath] string callerFilePath = ""
        )
        {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            if (Validate(message, logger) == false)
            {
                return;
            }
#endif

            var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
            var context = new PublishingContext(caller);
            _broker.PublishAsync(message, context, token, logger ?? DefaultLogger.Default).Forget();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly UniTask PublishWithContextAsync(
              CancellationToken token = default
            , ILogger logger = null
            , [CallerLineNumber] int callerLineNumber = 0
            , [CallerMemberName] string callerMemberName = ""
            , [CallerFilePath] string callerFilePath = ""
        )
        {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            if (Validate(logger) == false)
            {
                return UniTask.CompletedTask;
            }
#endif

            var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
            var context = new PublishingContext(caller);
            return _broker.PublishAsync(new TMessage(), context, token, logger ?? DefaultLogger.Default);
        }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public readonly UniTask PublishWithContextAsync(
              TMessage message
            , CancellationToken token = default
            , ILogger logger = null
            , [CallerLineNumber] int callerLineNumber = 0
            , [CallerMemberName] string callerMemberName = ""
            , [CallerFilePath] string callerFilePath = ""
        )
        {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            if (Validate(message, logger) == false)
            {
                return UniTask.CompletedTask;
            }
#endif

            var caller = new CallerInfo(callerLineNumber, callerMemberName, callerFilePath);
            var context = new PublishingContext(caller);
            return _broker.PublishAsync(message, context, token, logger ?? DefaultLogger.Default);
        }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
        private readonly bool Validate(ILogger logger)
        {
            if (_broker == null)
            {
                (logger ?? DefaultLogger.Default).LogError(
                    $"{GetType()} must be retrieved via `{nameof(MessagePublisher)}.{nameof(MessagePublisher.Cache)}` API"
                );

                return false;
            }

            return true;
        }

        private readonly bool Validate(TMessage message, ILogger logger)
        {
            if (_broker == null)
            {
                (logger ?? DefaultLogger.Default).LogError(
                    $"{GetType()} must be retrieved via `{nameof(MessagePublisher)}.{nameof(MessagePublisher.Cache)}` API"
                );

                return false;
            }

            if (message == null)
            {
                (logger ?? DefaultLogger.Default).LogException(new System.ArgumentNullException(nameof(message)));
                return false;
            }

            return true;
        }
#endif
    }
}