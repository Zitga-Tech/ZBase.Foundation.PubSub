using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub
{
    public delegate UniTask MessageHandler<TMessage>(TMessage message, CancellationToken cancelToken);

    public delegate UniTask MessageHandler<TState, TMessage>(TState state, TMessage message, CancellationToken cancelToken)
        where TState : class;
}
