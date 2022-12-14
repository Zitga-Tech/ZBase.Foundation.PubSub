#if !(UNITY_EDITOR || DEBUG) || DISABLE_DEBUG
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

        internal AnonPublisher(SingletonContainer<MessageBroker> brokers)
        {
            _publisher = new(brokers);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Publisher<GlobalScope> Global()
        {
            return new(_publisher.Global());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Publisher<TScope> Scope<TScope>(TScope scope)
        {
            return new(_publisher.Scope(scope));
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
            public void Publish(
                  in CancellationToken cancelToken = default
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
                  in CancellationToken cancelToken = default
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
