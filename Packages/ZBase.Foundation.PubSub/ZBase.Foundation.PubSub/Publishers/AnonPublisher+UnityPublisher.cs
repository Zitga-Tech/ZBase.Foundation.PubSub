﻿#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#else
#define __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
#endif

using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    partial class AnonPublisher
    {
        /// <summary>
        /// Anonymous Publisher that allows invoking handlers that take no message argument
        /// </summary>
        public readonly partial struct UnityPublisher<TScope>
            where TScope : UnityEngine.Object
        {
            internal readonly MessagePublisher.Publisher<UnityObjectRef<TScope>> _publisher;

            public bool IsValid => _publisher.IsValid;

            public UnityObjectRef<TScope> Scope => _publisher.Scope;

            internal UnityPublisher(MessagePublisher.UnityPublisher<TScope> publisher)
            {
                _publisher = publisher._publisher;
            }

#if __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            public CachedPublisher<AnonMessage> Cache(ILogger logger = null)
            {
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
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
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
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
#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
                if (Validate(logger) == false)
                {
                    return UniTask.CompletedTask;
                }
#endif

                return _publisher.PublishAsync<AnonMessage>(default, cancelToken, logger);
            }

#if __ZBASE_FOUNDATION_PUBSUB_VALIDATION__
            private bool Validate(ILogger logger)
            {
                if (IsValid == true)
                {
                    return true;
                }

                (logger ?? DefaultLogger.Default).LogError(
                    $"{GetType().Name} must be retrieved via `{nameof(AnonPublisher)}.{nameof(AnonPublisher.UnityScope)}` API"
                );

                return false;
            }
#endif

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            partial void RetainUsings();
        }
    }
}
