using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal interface IHandler<TMessage> : IDisposable
    {
        HandlerId Id { get; }

        UniTask Handle(TMessage message, CancellationToken cancelToken);
    }
}
