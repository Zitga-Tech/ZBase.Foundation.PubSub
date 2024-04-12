﻿using System.Diagnostics.CodeAnalysis;
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
        public Publisher<TScope> Scope<TScope>([NotNull] TScope scope)
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
        public CachedPublisher<AnonMessage> Cache<TScope>([NotNull] TScope scope, ILogger logger = null)
        {
            return Scope(scope).Cache(logger);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnityPublisher<TScope> UnityScope<TScope>([NotNull] TScope scope)
            where TScope : UnityEngine.Object
        {
            return new(_publisher.UnityScope(scope));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CachedPublisher<AnonMessage> UnityCache<TScope>([NotNull] TScope scope, ILogger logger = null)
            where TScope : UnityEngine.Object
        {
            return UnityScope(scope).Cache(logger);
        }
    }
}
