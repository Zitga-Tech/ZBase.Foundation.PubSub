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
        void Compress();

        ISubscription Subscribe(
              Action handler
            , int order = 0
        );

        ISubscription Subscribe(
              Func<CancellationToken, UniTask> handler
            , int order = 0
        );

        ISubscription Subscribe(
              Func<UniTask> handler
            , int order = 0
        );

        void Subscribe(
              Action handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        );

        void Subscribe(
              Func<CancellationToken, UniTask> handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        );

        void Subscribe(
              Func<UniTask> handler
            , in CancellationToken unsubscribeToken
            , int order = 0
        );
    }

    /// <inheritdoc/>
    public interface IAnonSubscriber<TScope> : IAnonSubscriber { }
}
