using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class CachedPublisherEmpty<TMessage> : ZBase.Foundation.PubSub.CachedPublisher<TMessage>
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE
        where TMessage : new()
#else
        where TMessage : IMessage, new()
#endif
    {
        public readonly static CachedPublisherEmpty<TMessage> Default = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void Publish(in CancellationToken cancelToken = default, ILogger logger = null)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override void Publish(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override UniTask PublishAsync(in CancellationToken cancelToken = default, ILogger logger = null)
        {
            return UniTask.CompletedTask;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public sealed override UniTask PublishAsync(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
            return UniTask.CompletedTask;
        }
    }
}
