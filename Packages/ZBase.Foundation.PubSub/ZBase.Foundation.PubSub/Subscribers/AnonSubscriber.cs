#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Runtime.CompilerServices;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Anonymous Subscriber allows registering handlers that take no message argument
    /// </summary>
    public partial class AnonSubscriber
    {
        private readonly MessageSubscriber _subscriber;

        internal AnonSubscriber(
              SingletonContainer<MessageBroker> brokers
            , CappedArrayPool<UniTask> taskArrayPool
        )
        {
            _subscriber = new(brokers, taskArrayPool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<GlobalScope> Global()
        {
            return new(_subscriber.Global());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<TScope> Scope<TScope>(TScope scope)
        {
            return new(_subscriber.Scope(scope));
        }

        /// <summary>
        /// Anonymous Subscriber allows registering handlers that take no message argument
        /// </summary>
        public readonly struct Subscriber<TScope> : IAnonSubscriber<TScope>
        {
            private readonly MessageSubscriber.Subscriber<TScope> _subscriber;

            public TScope Scope => _subscriber.Scope;

            public bool IsValid => _subscriber.IsValid;

            internal Subscriber(MessageSubscriber.Subscriber<TScope> subscriber)
            {
                _subscriber = subscriber;
            }

            /// <inheritdoc/>
#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Compress()
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return;
                }
#endif

                _subscriber.Compress<AnonMessage>();
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  Action handler
                , int order = 0
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return Subscription<AnonMessage>.None;
                }
#endif

                return _subscriber.Subscribe<AnonMessage>(handler, order);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  Func<CancellationToken, UniTask> handler
                , int order = 0
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return Subscription<AnonMessage>.None;
                }
#endif

                return _subscriber.Subscribe<AnonMessage>(handler, order);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public ISubscription Subscribe(
                  Func<UniTask> handler
                , int order = 0
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return Subscription<AnonMessage>.None;
                }
#endif

                return _subscriber.Subscribe<AnonMessage>(handler, order);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  Action handler
                , CancellationToken unsubscribeToken
                , int order = 0
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<AnonMessage>(handler, unsubscribeToken, order);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  Func<CancellationToken, UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<AnonMessage>(handler, unsubscribeToken, order);
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public void Subscribe(
                  Func<UniTask> handler
                , CancellationToken unsubscribeToken
                , int order = 0
            )
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                if (Validate() == false)
                {
                    return;
                }
#endif

                _subscriber.Subscribe<AnonMessage>(handler, unsubscribeToken, order);
            }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            private bool Validate()
            {
                if (_subscriber.IsValid == true)
                {
                    return true;
                }

                UnityEngine.Debug.LogError(
                    $"{GetType().Name} must be retrieved via `{nameof(AnonSubscriber)}.{nameof(AnonSubscriber.Scope)}` API"
                );

                return false;
            }
#endif
        }
    }
}
