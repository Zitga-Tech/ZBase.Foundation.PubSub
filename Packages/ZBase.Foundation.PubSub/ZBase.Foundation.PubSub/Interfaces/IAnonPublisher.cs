using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Anonymous Publisher that allows invoking handlers that take no message argument
    /// </summary>
    public interface IAnonPublisher
    {
        void Publish(
              CancellationToken cancelToken = default
            , ILogger logger = null
        );

        UniTask PublishAsync(
              CancellationToken cancelToken = default
            , ILogger logger = null
        );
    }

    /// <inheritdoc/>
    public interface IAnonPublisher<TScope> : IAnonPublisher { }
}
