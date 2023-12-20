using System;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    public sealed class Messenger : IDisposable
    {
        private readonly SingletonContainer<MessageBroker> _brokers = new();
        private readonly CappedArrayPool<UniTask> _taskArrayPool;

        public Messenger()
        {
            _taskArrayPool = new(8);
            MessageSubscriber = new(_brokers, _taskArrayPool);
            MessagePublisher = new(_brokers, _taskArrayPool);
            AnonSubscriber = new(_brokers, _taskArrayPool);
            AnonPublisher = new(_brokers, _taskArrayPool);
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
