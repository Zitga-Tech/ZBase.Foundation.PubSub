using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE

    public interface IMessagePublisher
    {
        void Publish<TMessage>(
              in CancellationToken cancelToken = default
            , ILogger logger = null
        ) where TMessage : new();

        void Publish<TMessage>(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        );

        UniTask PublishAsync<TMessage>(
              in CancellationToken cancelToken = default
            , ILogger logger = null
        ) where TMessage : new();

        UniTask PublishAsync<TMessage>(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        );
    }

#else

    public interface IMessagePublisher
    {
        void Publish<TMessage>(
              in CancellationToken cancelToken = default
            , ILogger logger = null
        ) where TMessage : IMessage, new();

        void Publish<TMessage>(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        ) where TMessage : IMessage;

        UniTask PublishAsync<TMessage>(
              in CancellationToken cancelToken = default
            , ILogger logger = null
        ) where TMessage : IMessage, new();

        UniTask PublishAsync<TMessage>(
              TMessage message
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        ) where TMessage : IMessage;
    }

#endif

    public interface IMessagePublisher<TScope> : IMessagePublisher { }
}
