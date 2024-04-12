using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
#if ZBASE_FOUNDATION_PUBSUB_RELAX_MODE

    public interface IMessageSubscriber
    {
        /// <summary>
        /// Remove empty handler groups to optimize performance.
        /// </summary>
        void Compress<TMessage>(ILogger logger = null);

        ISubscription Subscribe<TMessage>(
              Action handler
            , int order = 0
            , ILogger logger = null
        );

        ISubscription Subscribe<TMessage>(
              Action<TMessage> handler
            , int order = 0
            , ILogger logger = null
        );

        ISubscription Subscribe<TMessage>(
              Func<CancellationToken, UniTask> handler
            , int order = 0
            , ILogger logger = null
        );

        ISubscription Subscribe<TMessage>(
              MessageHandler<TMessage> handler
            , int order = 0
            , ILogger logger = null
        );

        ISubscription Subscribe<TMessage>(
              Func<TMessage, UniTask> handler
            , int order = 0
            , ILogger logger = null
        );

        ISubscription Subscribe<TMessage>(
              Func<UniTask> handler
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe<TMessage>(
              Action handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe<TMessage>(
              Action<TMessage> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe<TMessage>(
              Func<CancellationToken, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe<TMessage>(
              MessageHandler<TMessage> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe<TMessage>(
              Func<TMessage, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe<TMessage>(
              Func<UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );
    }

#else

    public interface IMessageSubscriber
    {
        /// <summary>
        /// Remove empty handler groups to optimize performance.
        /// </summary>
        void Compress<TMessage>(ILogger logger = null) where TMessage : IMessage;

        ISubscription Subscribe<TMessage>(
              Action handler
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        ISubscription Subscribe<TMessage>(
              Action<TMessage> handler
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        ISubscription Subscribe<TMessage>(
              Func<CancellationToken, UniTask> handler
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        ISubscription Subscribe<TMessage>(
              MessageHandler<TMessage> handler
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        ISubscription Subscribe<TMessage>(
              Func<TMessage, UniTask> handler
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        ISubscription Subscribe<TMessage>(
              Func<UniTask> handler
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        void Subscribe<TMessage>(
              Action handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        void Subscribe<TMessage>(
              Action<TMessage> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        void Subscribe<TMessage>(
              Func<CancellationToken, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        void Subscribe<TMessage>(
              MessageHandler<TMessage> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        void Subscribe<TMessage>(
              Func<TMessage, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;

        void Subscribe<TMessage>(
              Func<UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        ) where TMessage : IMessage;
    }

#endif

    public interface IMessageSubscriber<TScope> : IMessageSubscriber { }
}
