using System.Runtime.CompilerServices;

namespace ZBase.Foundation.PubSub.Internals
{
    internal static class BrokerStore<TScope, TMessage>
    {
        private static MessageBroker<TScope, TMessage> s_broker;
        private static readonly object s_lock = new();
        private static bool s_registered;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessageBroker<TScope, TMessage> Get()
            => s_broker;

        public static MessageBroker<TScope, TMessage> GetOrCreate()
        {
            if (s_broker != null) return s_broker;

            lock (s_lock)
            {
                if (s_broker != null) return s_broker;

                s_broker = new MessageBroker<TScope, TMessage>();

                if (s_registered == false)
                {
                    BrokerRegistry.Register(static () => Clear());
                    s_registered = true;
                }

                return s_broker;
            }
        }

        public static void Clear()
        {
            lock (s_lock)
            {
                s_broker?.Dispose();
                s_broker = null;
                s_registered = false;
            }
        }
    }
}
