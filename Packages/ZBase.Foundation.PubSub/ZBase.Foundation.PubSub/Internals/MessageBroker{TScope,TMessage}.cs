using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class MessageBroker<TScope, TMessage> : MessageBroker
    {
        private readonly Dictionary<TScope, int> _scopeToIndex = new();
        private MessageBroker<TMessage>[] _brokers = new MessageBroker<TMessage>[4];
        private int _nextIndex;

        public bool IsEmpty => _nextIndex <= 0 || AreAllEmpty();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int FindIndex(TScope scope)
        {
            return _scopeToIndex.TryGetValue(scope, out var index) ? index : -1;
        }

        private int FindOrCreateIndex(TScope scope, CappedArrayPool<UniTask> taskArrayPool)
        {
            if (_scopeToIndex.TryGetValue(scope, out var index))
            {
                if (_brokers[index] == null)
                {
                    var newBroker = new MessageBroker<TMessage>();
                    newBroker.TaskArrayPool = taskArrayPool;
                    _brokers[index] = newBroker;
                }

                return index;
            }

            index = _nextIndex++;

            if (index >= _brokers.Length)
            {
                Array.Resize(ref _brokers, _brokers.Length * 2);
            }

            var broker = new MessageBroker<TMessage>();
            broker.TaskArrayPool = taskArrayPool;
            _brokers[index] = broker;
            _scopeToIndex[scope] = index;
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureCapacity(int index)
        {
            if (index >= _brokers.Length)
            {
                Array.Resize(ref _brokers, Math.Max(_brokers.Length * 2, index + 1));
            }
        }

        public UniTask PublishAsync(
              TScope scope, TMessage message
            , PublishingContext context
            , CancellationToken token
            , ILogger logger
        )
        {
            lock (_scopeToIndex)
            {
                var index = FindIndex(scope);

                if (index >= 0)
                {
                    var broker = _brokers[index];

                    if (broker != null)
                    {
                        return broker.PublishAsync(message, context, token, logger);
                    }
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
            lock (_scopeToIndex)
            {
                var index = FindOrCreateIndex(scope, taskArrayPool);
                return _brokers[index].Subscribe(handler, order);
            }
        }

        public MessageBroker<TMessage> Cache(TScope scope, CappedArrayPool<UniTask> taskArrayPool)
        {
            lock (_scopeToIndex)
            {
                var index = FindOrCreateIndex(scope, taskArrayPool);
                var broker = _brokers[index];
                broker.OnCache();
                return broker;
            }
        }

        public override void Dispose()
        {
            lock (_scopeToIndex)
            {
                for (var i = 0; i < _nextIndex; i++)
                {
                    _brokers[i]?.Dispose();
                    _brokers[i] = null;
                }

                _scopeToIndex.Clear();
                _nextIndex = 0;
            }
        }

        public override void Compress()
        {
            lock (_scopeToIndex)
            {
                for (var i = _nextIndex - 1; i >= 0; i--)
                {
                    var broker = _brokers[i];

                    if (broker == null)
                    {
                        continue;
                    }

                    if (broker.IsCached)
                    {
                        continue;
                    }

                    broker.Compress();

                    if (broker.IsEmpty)
                    {
                        broker.Dispose();
                        _brokers[i] = null;
                    }
                }
            }
        }

        /// <summary>
        /// Remove empty handler groups to optimize performance.
        /// </summary>
        public void Compress(TScope scope)
        {
            lock (_scopeToIndex)
            {
                var index = FindIndex(scope);

                if (index < 0)
                {
                    return;
                }

                var broker = _brokers[index];

                if (broker == null || broker.IsCached)
                {
                    return;
                }

                broker.Compress();

                if (broker.IsEmpty)
                {
                    _brokers[index] = null;
                    broker.Dispose();
                }
            }
        }

        private bool AreAllEmpty()
        {
            for (var i = 0; i < _nextIndex; i++)
            {
                var broker = _brokers[i];

                if (broker != null && broker.IsEmpty == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}