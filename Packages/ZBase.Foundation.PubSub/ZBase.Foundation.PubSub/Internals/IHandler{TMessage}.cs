using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal interface IHandler<TMessage> : IDisposable
    {
        HandlerId Id { get; }

        UniTask Handle(TMessage message, in CancellationToken cancelToken);
    }

    internal sealed class HandlerMessage<TMessage> : IHandler<TMessage>
    {
        private MessageHandler<TMessage> _handler;

        public HandlerMessage(MessageHandler<TMessage> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public HandlerId Id => new(_handler);

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, in CancellationToken cancelToken)
        {
            return _handler?.Invoke(message, cancelToken) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerFuncCancelToken<TMessage> : IHandler<TMessage>
    {
        private Func<CancellationToken, UniTask> _handler;

        public HandlerFuncCancelToken(Func<CancellationToken, UniTask> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public HandlerId Id => new(_handler);

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, in CancellationToken cancelToken)
        {
            return _handler?.Invoke(cancelToken) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerFuncMessage<TMessage> : IHandler<TMessage>
    {
        private Func<TMessage, UniTask> _handler;

        public HandlerFuncMessage(Func<TMessage, UniTask> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public HandlerId Id => new(_handler);

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, in CancellationToken cancelToken)
        {
            return _handler?.Invoke(message) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerFunc<TMessage> : IHandler<TMessage>
    {
        private Func<UniTask> _handler;

        public HandlerFunc(Func<UniTask> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public HandlerId Id => new(_handler);

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, in CancellationToken cancelToken)
        {
            return _handler?.Invoke() ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerActionMessage<TMessage> : IHandler<TMessage>
    {
        private Action<TMessage> _handler;

        public HandlerActionMessage(Action<TMessage> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public HandlerId Id => new(_handler);

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, in CancellationToken cancelToken)
        {
            _handler?.Invoke(message);
            return UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerAction<TMessage> : IHandler<TMessage>
    {
        private Action _handler;

        public HandlerAction(Action handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public HandlerId Id => new(_handler);

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, in CancellationToken cancelToken)
        {
            _handler?.Invoke();
            return UniTask.CompletedTask;
        }
    }
}
