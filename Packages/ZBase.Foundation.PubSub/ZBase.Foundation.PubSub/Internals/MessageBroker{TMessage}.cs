using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Collections.Pooled.Generic.Internals.Unsafe;

using ZCPG = ZBase.Collections.Pooled.Generic;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class MessageBroker<TMessage> : MessageBroker
    {
        private readonly ZCPG.List<int> _ordering = new(1);
        private readonly Dictionary<int, ZCPG.ArrayDictionary<HandlerId, IHandler<TMessage>>> _handlerMap = new(1);

        private CappedArrayPool<UniTask> _taskArrayPool;

        public CappedArrayPool<UniTask> TaskArrayPool
        {
            get => _taskArrayPool ?? CappedArrayPool<UniTask>.Shared8Limit;
            set => _taskArrayPool = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool IsEmpty => _ordering.Count < 1;

        public async UniTask PublishAsync(TMessage message, CancellationToken cancelToken, ILogger logger)
        {
            _ordering.GetUnsafe(out var orderArray, out var orderCount);

            var handlerMap = _handlerMap;

            for (var i = orderCount - 1; i >= 0; i--)
            {
                var order = orderArray[i];

                if (handlerMap.TryGetValue(order, out var handlers) == false)
                {
                    continue;
                }

                if (handlers.Count < 1)
                {
                    continue;
                }

                await PublishAsync(handlers, message, cancelToken, TaskArrayPool, logger);
            }
        }

        private static async UniTask PublishAsync(
              ZCPG.ArrayDictionary<HandlerId, IHandler<TMessage>> handlers
            , TMessage message
            , CancellationToken cancelToken
            , CappedArrayPool<UniTask> taskArrayPool
            , ILogger logger
        )
        {
            handlers.GetUnsafeValues(out var handlerArray, out var count);
            var tasks = taskArrayPool.Rent(count);

            try
            {
                for (var i = 0; i < count; i++)
                {
                    tasks[i] = handlerArray[i].Handle(message, cancelToken);
                }

                await UniTask.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                logger?.LogException(ex);
            }

            taskArrayPool.Return(tasks);
        }

        public Subscription<TMessage> Subscribe(IHandler<TMessage> handler, int order)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var ordering = _ordering;
            var handlerMap = _handlerMap;

            lock (handlerMap)
            {
                if (ordering.Contains(order) == false)
                {
                    ordering.Add(order);
                    ordering.Sort();
                }

                if (handlerMap.TryGetValue(order, out var handlers) == false)
                {
                    handlers = new ZCPG.ArrayDictionary<HandlerId, IHandler<TMessage>>();
                    handlerMap.Add(order, handlers);
                }

                if (handlers.TryAdd(handler.Id, handler))
                {
                    return new Subscription<TMessage>(handler, handlers);
                }

                return Subscription<TMessage>.None;
            }
        }

        public override void Dispose()
        {
            var handlerMap = _handlerMap;

            foreach (var kvp in handlerMap)
            {
                kvp.Value?.Dispose();
            }

            handlerMap.Clear();
            _ordering.Clear();
        }

        /// <summary>
        /// Remove empty handler groups to optimize performance.
        /// </summary>
        public void Compress()
        {
            var handlerMap = _handlerMap;
            var ordering = _ordering;

            lock (handlerMap)
            {
                ordering.GetUnsafe(out var orderArray, out var orderCount);

                for (var i = orderCount - 1; i >= 0; i--)
                {
                    var order = orderArray[i];

                    if (handlerMap.TryGetValue(order, out var handlers) == false)
                    {
                        continue;
                    }

                    if (handlers.Count > 0)
                    {
                        continue;
                    }

                    handlerMap.Remove(order);
                    ordering.RemoveAt(i);
                }
            }
        }
    }
}