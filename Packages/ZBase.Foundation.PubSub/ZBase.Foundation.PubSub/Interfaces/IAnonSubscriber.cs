using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Anonymous Subscriber allows registering handlers that take no message argument
    /// </summary>
    public interface IAnonSubscriber
    {
        /// <summary>
        /// Remove empty handler groups to optimize performance.
        /// </summary>
        void Compress(ILogger logger = null);

        ISubscription Subscribe(
              Action handler
            , int order = 0
            , ILogger logger = null
        );

        ISubscription Subscribe(
              Func<CancellationToken, UniTask> handler
            , int order = 0
            , ILogger logger = null
        );

        ISubscription Subscribe(
              Func<UniTask> handler
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe(
              Action handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe(
              Func<CancellationToken, UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );

        void Subscribe(
              Func<UniTask> handler
            , CancellationToken unsubscribeToken
            , int order = 0
            , ILogger logger = null
        );
    }

    /// <inheritdoc/>
    public interface IAnonSubscriber<TScope> : IAnonSubscriber { }
}
