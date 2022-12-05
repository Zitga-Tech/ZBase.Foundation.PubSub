using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Anonymous Publisher that allows invoking handlers that take no message argument
    /// to the <see cref="GlobalScope"/>
    /// </summary>
    public static class AnonPublisherGlobal
    {
        /// <summary>
        /// Publishes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Publish(
              this AnonPublisher self
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
            self.Global().Publish(cancelToken, logger);
        }

        /// <summary>
        /// Publishes to the <see cref="GlobalScope"/>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask PublishAsync(
              this AnonPublisher self
            , in CancellationToken cancelToken = default
            , ILogger logger = null
        )
        {
            return self.Global().PublishAsync(cancelToken, logger);
        }
    }
}
