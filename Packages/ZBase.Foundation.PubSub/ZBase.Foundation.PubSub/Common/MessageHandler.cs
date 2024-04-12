using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    public delegate UniTask MessageHandler<TMessage>(TMessage message, CancellationToken cancelToken);
}
