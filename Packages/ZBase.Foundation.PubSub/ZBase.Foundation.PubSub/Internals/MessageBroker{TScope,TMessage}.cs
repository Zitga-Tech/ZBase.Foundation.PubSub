using System;
using System.Threading;
using ZBase.Collections.Pooled.Generic;
using ZBase.Collections.Pooled.Generic.Internals.Unsafe;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class MessageBroker<TScope, TMessage> : MessageBroker
    {
        private readonly ArrayDictionary<TScope, MessageBroker<TMessage>> _scopedBrokers = new();

        public bool IsEmpty => _scopedBrokers.Count <= 0;

        public UniTask PublishAsync(
              TScope scope, TMessage message
            , PublishingContext context
            , CancellationToken token
            , ILogger logger
        )
        {
            var scopedBrokers = _scopedBrokers;

            lock (scopedBrokers)
            {
                if (scopedBrokers.TryGetValue(scope, out var broker))
                {
                    return broker.PublishAsync(message, context, token, logger);
                }

                return UniTask.CompletedTask;
            }
        }

        public Subscription<TMessage> Subscribe(
              TScope scope
            , IHandler<TMessage> handler
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

        public MessageBroker<TMessage> Cache(TScope scope, CappedArrayPool<UniTask> taskArrayPool)
        {
            var scopedBrokers = _scopedBrokers;

            lock (scopedBrokers)
            {
                if (scopedBrokers.TryGetValue(scope, out var broker) == false)
                {
                    scopedBrokers[scope] = broker = new MessageBroker<TMessage>();
                    broker.TaskArrayPool = taskArrayPool;
                }

                broker.OnCache();
                return broker;
            }
        }

        public override void Dispose()
        {
            var scopedBrokers = _scopedBrokers;

            lock (scopedBrokers)
            {
                scopedBrokers.GetUnsafeValues(out var brokerArray, out var count);
                var brokers = brokerArray.AsSpan(0, count);

                foreach (var broker in brokers)
                {
                    broker?.Dispose();
                }

                scopedBrokers.Dispose();
            }
        }

        public override void Compress()
        {
            var scopedBrokers = _scopedBrokers;

            lock (scopedBrokers)
            {
                scopedBrokers.GetUnsafe(out var keys, out var values, out var count);

                var scopesToRemove = ValueList<TScope>.Create(count);

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                try
#endif
                {
                    for (var i = count - 1; i >= 0; i--)
                    {
                        var broker = values[i];
                        broker.Compress();

                        if (broker.IsEmpty)
                        {
                            broker.Dispose();
                            scopesToRemove.Add(keys[i].Key);
                        }
                    }

                    scopesToRemove.GetUnsafe(out var scopes, out count);

                    for (var i = count - 1; i >= 0; i--)
                    {
                        _scopedBrokers.Remove(scopes[i]);
                    }
                }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                finally
#endif
                {
                    scopesToRemove.Dispose();
                }
            }
        }

        /// <summary>
        /// Remove empty handler groups to optimize performance.
        /// </summary>
        public void Compress(TScope scope)
        {
            var scopedBrokers = _scopedBrokers;

            lock (scopedBrokers)
            {
                if (scopedBrokers.TryGetValue(scope, out var broker) == false)
                {
                    return;
                }

                if (broker.IsCached)
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
}