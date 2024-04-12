#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System.Threading;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Anonymous Publisher that allows invoking handlers that take no message argument
    /// </summary>
    public partial class AnonPublisher
    {
        private readonly MessagePublisher _publisher;

        internal AnonPublisher(
              SingletonContainer<MessageBroker> brokers
            , CappedArrayPool<UniTask> taskArrayPool
        )
        {
            _publisher = new(brokers, taskArrayPool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Publisher<GlobalScope> Global()
        {
            return new(_publisher.Global());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Publisher<TScope> Scope<TScope>()
            where TScope : struct
        {
            return new(_publisher.Scope(default(TScope)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Publisher<TScope> Scope<TScope>(TScope scope)
        {
            return new(_publisher.Scope(scope));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CachedPublisher<AnonMessage> GlobalCache(ILogger logger = null)
        {
            return Global().Cache(logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CachedPublisher<AnonMessage> Cache<TScope>(ILogger logger = null)
            where TScope : struct
        {
            return Scope(default(TScope)).Cache(logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CachedPublisher<AnonMessage> Cache<TScope>(TScope scope, ILogger logger = null)
        {
            return Scope(scope).Cache(logger);
        }

        /// <summary>
        /// Anonymous Publisher that allows invoking handlers that take no message argument
        /// </summary>
        public readonly struct Publisher<TScope> : IAnonPublisher<TScope>
        {
            private readonly MessagePublisher.Publisher<TScope> _publisher;

            public TScope Scope => _publisher.Scope;

            public bool IsValid => _publisher.IsValid;

            internal Publisher(MessagePublisher.Publisher<TScope> publisher)
            {
                _publisher = publisher;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public CachedPublisher<AnonMessage> Cache(ILogger logger = null)
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return default;
                }
#endif

                return _publisher.Cache<AnonMessage>(logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Publish(
                  CancellationToken cancelToken = default
                , ILogger logger = null
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return;
                }
#endif

                _publisher.Publish<AnonMessage>(cancelToken, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public UniTask PublishAsync(
                  CancellationToken cancelToken = default
                , ILogger logger = null
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                return _publisher.PublishAsync<AnonMessage>(default, cancelToken, logger);
            }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            private bool Validate()
            {
                if (IsValid == true)
                {
                    return true;
                }

                UnityEngine.Debug.LogError(
                    $"{GetType().Name} must be retrieved via `{nameof(AnonPublisher)}.{nameof(AnonPublisher.Scope)}` API"
                );

                return false;
            }
#endif
        }
    }
}
