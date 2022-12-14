using UnityEngine;
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

        private MessageHandler<TMessage> _handler;
        private ZCPG.ArrayHashSet<MessageHandler<TMessage>> _handlers;

        private bool _canDispose;

        public Subscription(
              MessageHandler<TMessage> handler
            , ZCPG.ArrayHashSet<MessageHandler<TMessage>> handlers
        )
        {
            _handler = handler;
            _handlers = handlers;

            _canDispose = handler != null && handlers != null;
        }

        public bool IsValid => _canDispose;

        public void Dispose()
        {
            if (_canDispose == false)
            {
                return;
            }

            _canDispose = false;

            if (_handler != null)
            {
                _handlers?.Remove(_handler);
            }

            _handler = null;
            _handlers = null;
        }
    }
}