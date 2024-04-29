using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class HandlerMessage<TMessage> : IHandler<TMessage>
    {
        private readonly HandlerId _id;
        private MessageHandler<TMessage> _handler;

        public HandlerMessage(MessageHandler<TMessage> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler);
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke(message, cancelToken) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerFuncCancelToken<TMessage> : IHandler<TMessage>
    {
        private readonly HandlerId _id;
        private Func<CancellationToken, UniTask> _handler;

        public HandlerFuncCancelToken(Func<CancellationToken, UniTask> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler);
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke(cancelToken) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerFuncMessage<TMessage> : IHandler<TMessage>
    {
        private readonly HandlerId _id;
        private Func<TMessage, UniTask> _handler;

        public HandlerFuncMessage(Func<TMessage, UniTask> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler);
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke(message) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerFunc<TMessage> : IHandler<TMessage>
    {
        private readonly HandlerId _id;
        private Func<UniTask> _handler;

        public HandlerFunc(Func<UniTask> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler);
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke() ?? UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerActionMessage<TMessage> : IHandler<TMessage>
    {
        private readonly HandlerId _id;
        private Action<TMessage> _handler;

        public HandlerActionMessage(Action<TMessage> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler);
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return UniTask.CompletedTask;
            }

            _handler?.Invoke(message);
            return UniTask.CompletedTask;
        }
    }

    internal sealed class HandlerAction<TMessage> : IHandler<TMessage>
    {
        private readonly HandlerId _id;
        private Action _handler;

        public HandlerAction(Action handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler);
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested)
            {
                return UniTask.CompletedTask;
            }

            _handler?.Invoke();
            return UniTask.CompletedTask;
        }
    }
}
