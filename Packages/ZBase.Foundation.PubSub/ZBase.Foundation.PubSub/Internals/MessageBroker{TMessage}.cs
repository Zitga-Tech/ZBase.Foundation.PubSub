#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ZBase.Collections.Pooled.Generic.Internals;
using ZBase.Collections.Pooled.Generic.Internals.Unsafe;

using ZCPG = ZBase.Collections.Pooled.Generic;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class MessageBroker<TMessage> : MessageBroker
    {
        private readonly ZCPG.List<int> _ordering = new(1);
        private readonly Dictionary<int, ZCPG.ArrayDictionary<HandlerId, IHandler<TMessage>>> _handlerMap = new(1);

        private CappedArrayPool<UniTask> _taskArrayPool;
        private long _refCount;

        public CappedArrayPool<UniTask> TaskArrayPool
        {
            get => _taskArrayPool ?? CappedArrayPool<UniTask>.Shared8Limit;
            set => _taskArrayPool = value ?? throw new ArgumentNullException(nameof(value));
        }

        public bool IsEmpty => _ordering.Count < 1;

        public bool IsCached => _refCount > 0;

        public async UniTask PublishAsync(TMessage message, CancellationToken cancelToken, ILogger logger)
        {
            var handlersArray = GetAllHandlers(logger);

            if (handlersArray.IsValid == false)
            {
                return;
            }

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            try
#endif
            {
                handlersArray.GetUnsafe(out var handlers, out var length);

                for (var i = 0; i < length; i++)
                {
                    if (cancelToken.IsCancellationRequested)
                    {
                        break;
                    }

                    await PublishAsync(handlers[i], message, cancelToken, TaskArrayPool, logger);
                }
            }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            catch (Exception ex)
            {
                logger?.LogException(ex);
            }
            finally
#endif
            {
                Dispose(ref handlersArray);
            }
        }

        private ZCPG.ValueArray<ZCPG.ValueArray<IHandler<TMessage>>> GetAllHandlers(ILogger logger)
        {
            var handlerMap = _handlerMap;
            var ordering = _ordering;

            lock (handlerMap)
            {
                ordering.GetUnsafe(out var orderArray, out var orderCount);

                var orderValueArray = ZCPG.ValueArray<int>.Create(orderArray, orderCount);
                orderValueArray.GetUnsafe(out var orders, out orderCount);

                var handlersList = ZCPG.ValueList<ZCPG.ValueArray<IHandler<TMessage>>>.Create(handlerMap.Count);

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                try
#endif
                {
                    for (var i = orderCount - 1; i >= 0; i--)
                    {
                        var order = orders[i];

                        if (handlerMap.TryGetValue(order, out var handlers) == false || handlers.Count < 1)
                        {
                            continue;
                        }

                        handlers.GetUnsafeValues(out var handlerArray, out var length);
                        handlersList.Add(ZCPG.ValueArray<IHandler<TMessage>>.Create(handlerArray, length));
                    }
                }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                catch (Exception ex)
                {
                    logger?.LogException(ex);
                }
                finally
#endif
                {
                    orderValueArray.Dispose();
                }

                return ValueCollectionInternals.ToValueArray(ref handlersList);
            }
        }

        private static async UniTask PublishAsync(
              ZCPG.ValueArray<IHandler<TMessage>> handlers
            , TMessage message
            , CancellationToken cancelToken
            , CappedArrayPool<UniTask> taskArrayPool
            , ILogger logger
        )
        {
            if (handlers.IsValid == false || handlers.Length < 1)
            {
                return;
            }

            handlers.GetUnsafe(out var items, out var length);

            var tasks = taskArrayPool.Rent(length);
            
            for (var i = 0; i < length; i++)
            {
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                try
#endif
                {
                    tasks[i] = items[i]?.Handle(message, cancelToken) ?? UniTask.CompletedTask;
                }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                catch (Exception ex)
                {
                    tasks[i] = UniTask.CompletedTask;
                    logger?.LogException(ex);
                }
#endif
            }
            
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            try
#endif
            {
                await UniTask.WhenAll(tasks);
            }
#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
            catch (Exception ex)
            {
                logger?.LogException(ex);
            }
            finally
#endif
            {
                taskArrayPool.Return(tasks);
            }
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

        private static void Dispose(ref ZCPG.ValueArray<ZCPG.ValueArray<IHandler<TMessage>>> handlersList)
        {
            if (handlersList.IsValid == false)
            {
                return;
            }

            handlersList.GetUnsafe(out var handlersArray, out var length);

            var span = handlersArray.AsSpan(0, length);

            for (var i = 0; i < length; i++)
            {
                ref var array = ref span[i];

                if (array.IsValid)
                {
                    array.Dispose();
                }

                array = default;
            }

            handlersList.Dispose();
            handlersList = default;
        }

        public sealed override void Dispose()
        {
            var handlerMap = _handlerMap;
            var ordering = _ordering;

            lock (handlerMap)
            {
                foreach (var kvp in handlerMap)
                {
                    kvp.Value?.Dispose();
                }

                handlerMap.Clear();
                ordering.Dispose();
            }
        }

        public void OnCache()
        {
            checked
            {
                _refCount++;
            }
        }

        public void OnUncache()
        {
            _refCount = Math.Max(0, _refCount - 1);
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

                var orderValueArray = ZCPG.ValueArray<int>.Create(orderArray, orderCount);
                orderValueArray.GetUnsafe(out var orders, out orderCount);

#if !__ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
                try
#endif
                {
                    for (var i = orderCount - 1; i >= 0; i--)
                    {
                        var order = orders[i];

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