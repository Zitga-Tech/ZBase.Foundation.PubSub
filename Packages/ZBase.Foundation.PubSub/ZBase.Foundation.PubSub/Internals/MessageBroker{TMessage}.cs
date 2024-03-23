#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

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
            var handlerMap = _handlerMap;
            var ordering = _ordering;

            ordering.GetUnsafe(out var orderArray, out var orderCount);
            
            var orderValueArray = ZCPG.ValueArray<int>.Create(orderCount);
            orderArray.AsSpan(0, orderCount).CopyTo(orderValueArray.AsSpan(0, orderCount));
            orderValueArray.GetUnsafe(out orderArray, out orderCount);

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            try
#endif
            {
                for (var i = orderCount - 1; i >= 0; i--)
                {
                    var order = orderArray[i];

                    if (handlerMap.TryGetValue(order, out var handlers) == false || handlers.Count < 1)
                    {
                        continue;
                    }

                    await PublishAsync(handlers, message, cancelToken, TaskArrayPool, logger);
                }
            }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            catch (Exception ex)
            {
                logger?.LogException(ex);
            }
            finally
            {
                orderValueArray.Dispose();
            }
#endif
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

            var handlerValueArray = ZCPG.ValueArray<IHandler<TMessage>>.Create(count);
            handlerArray.AsSpan(0, count).CopyTo(handlerValueArray.AsSpan(0, count));
            handlerValueArray.GetUnsafe(out handlerArray, out count);

            var tasks = taskArrayPool.Rent(count);

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            try
#endif
            {
                for (var i = 0; i < count; i++)
                {
                    tasks[i] = handlerArray[i].Handle(message, cancelToken);
                }

                await UniTask.WhenAll(tasks);
            }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            catch (Exception ex)
            {
                logger?.LogException(ex);
            }
            finally
            {
                taskArrayPool.Return(tasks);
                handlerValueArray.Dispose();
            }
#endif
        }

        public Subscription<TMessage> Subscribe(IHandler<TMessage> handler, int order)
        {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
#endif

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

        public sealed override void Dispose()
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
        public sealed override void Compress()
        {
            var handlerMap = _handlerMap;
            var ordering = _ordering;

            lock (handlerMap)
            {
                ordering.GetUnsafe(out var orderArray, out var orderCount);

                var orderValueArray = ZCPG.ValueArray<int>.Create(orderCount);
                orderArray.AsSpan(0, orderCount).CopyTo(orderValueArray.AsSpan(0, orderCount));
                orderValueArray.GetUnsafe(out orderArray, out orderCount);

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                try
#endif
                {
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
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                finally
#endif
                {
                    orderValueArray.Dispose();
                }
            }
        }
    }
}