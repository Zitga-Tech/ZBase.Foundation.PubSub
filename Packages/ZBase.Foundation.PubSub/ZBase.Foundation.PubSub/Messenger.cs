using System;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    public sealed class Messenger : IDisposable
    {
        private readonly SingletonContainer<MessageBroker> _brokers = new();

        public Messenger()
        {
            MessageSubscriber = new(_brokers);
            MessagePublisher = new(_brokers);
            AnonSubscriber = new(_brokers);
            AnonPublisher = new(_brokers);
        }

        public MessageSubscriber MessageSubscriber { get; }

        public MessagePublisher MessagePublisher { get; }

        public AnonSubscriber AnonSubscriber { get; }

        public AnonPublisher AnonPublisher { get; }

        public void Dispose()
        {
            _brokers.Dispose();
        }
    }
}
