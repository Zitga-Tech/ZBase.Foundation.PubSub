#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    public abstract class CachedPublisher<TMessage>
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
        where TMessage : new()
#else
        where TMessage : IMessage, new()
#endif
    {
        public abstract void Publish(in CancellationToken cancelToken = default, ILogger logger = null);

        public abstract void Publish(TMessage message, in CancellationToken cancelToken = default, ILogger logger = null);

        public abstract UniTask PublishAsync(in CancellationToken cancelToken = default, ILogger logger = null);

        public abstract UniTask PublishAsync(TMessage message, in CancellationToken cancelToken = default, ILogger logger = null);
    }
}