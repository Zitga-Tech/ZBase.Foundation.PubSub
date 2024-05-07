using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using ZBase.Foundation.PubSub.Internals;
using ZCPG = ZBase.Collections.Pooled.Generic;

namespace ZBase.Foundation.PubSub
{
    internal sealed class Subscription<TMessage> : ISubscription
    {
        private static Subscription<TMessage> s_none;

        public static Subscription<TMessage> None => s_none;

        static Subscription()
        {
            Init();
        }

        /// <seealso href="https://docs.unity3d.com/Manual/DomainReloading.html"/>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_none = new(default, default);
        }

        private IHandler<TMessage> _handler;
        private readonly WeakReference<ZCPG.ArrayDictionary<HandlerId, IHandler<TMessage>>> _handlers;

        public Subscription(
              IHandler<TMessage> handler
            , ZCPG.ArrayDictionary<HandlerId, IHandler<TMessage>> handlers
        )
        {
            _handler = handler;
            _handlers = new WeakReference<ZCPG.ArrayDictionary<HandlerId, IHandler<TMessage>>>(handlers);
        }

        public void Dispose()
        {
            if (_handler == null)
            {
                return;
            }

            var id = _handler.Id;
            _handler.Dispose();
            _handler = null;

            if (_handlers.TryGetTarget(out var handlers))
            {
                handlers.Remove(id);
            }
        }
    }

    internal static class SubscriptionExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RegisterTo<TMessage>(
              [NotNull] this Subscription<TMessage> subscription
            , CancellationToken unsubscribeToken
        )
#if !ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
                where TMessage : IMessage
#endif
        {
            unsubscribeToken.Register(static x => ((Subscription<TMessage>)x)?.Dispose(), subscription);
        }
    }
}