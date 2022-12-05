using System;
using System.Threading;
using Collections.Pooled.Generic;
using Collections.Pooled.Generic.Internals.Unsafe;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class MessageBroker<TScope, TMessage>
        : MessageBroker
    {
        private readonly ArrayDictionary<TScope, MessageBroker<TMessage>> _scopedBrokers = new();

        public bool IsEmpty => _scopedBrokers.Count <= 0;

        public UniTask PublishAsync(
              TScope scope, TMessage message
            , in CancellationToken cancelToken
            , ILogger logger
        )
        {
            if (_scopedBrokers.TryGetValue(scope, out var broker))
            {
                return broker.PublishAsync(message, cancelToken, logger);
            }

            return UniTask.CompletedTask;
        }

        public Subscription<TMessage> Subscribe(
              TScope scope
            , MessageHandler<TMessage> handler
            , int order
            , CappedArrayPool<UniTask> taskArrayPool
        )
        {
            var scopedBrokers = _scopedBrokers;

            lock (scopedBrokers)
            {
                if (scopedBrokers.TryGetValue(scope, out var broker) == false)
                {
                    scopedBrokers[scope] = broker = new MessageBroker<TMessage>();
                    broker.TaskArrayPool = taskArrayPool;
                }

                return broker.Subscribe(handler, order);
            }
        }

        public override void Dispose()
        {
            _scopedBrokers.GetUnsafeValues(out var brokerArray, out var count);
            var brokers = brokerArray.AsSpan(0, count);

            foreach (var broker in brokers)
            {
                broker?.Dispose();
            }

            _scopedBrokers.Dispose();
        }

        /// <summary>
        /// Remove empty handler groups to optimize performance.
        /// </summary>
        public void Compress(TScope scope)
        {
            var scopedBrokers = _scopedBrokers;

            if (scopedBrokers.TryGetValue(scope, out var broker) == false)
            {
                return;
            }

            broker.Compress();

            if (broker.IsEmpty)
            {
                scopedBrokers.Remove(scope);
                broker.Dispose();
            }
        }
    }
}