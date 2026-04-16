using System;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;

namespace ZBase.Foundation.PubSub
{
    public sealed class Messenger : IDisposable
    {
        private readonly CappedArrayPool<UniTask> _taskArrayPool;

        public Messenger()
        {
            _taskArrayPool = new(8);
            MessageSubscriber = new(_taskArrayPool);
            MessagePublisher = new(_taskArrayPool);
            AnonSubscriber = new(_taskArrayPool);
            AnonPublisher = new(_taskArrayPool);
        }

        public MessageSubscriber MessageSubscriber { get; }

        public MessagePublisher MessagePublisher { get; }

        public AnonSubscriber AnonSubscriber { get; }

        public AnonPublisher AnonPublisher { get; }

        public void Dispose()
        {
            BrokerRegistry.DisposeAll();
        }
    }
}
