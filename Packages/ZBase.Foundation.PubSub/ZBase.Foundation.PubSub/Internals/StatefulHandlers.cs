using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class StatefulHandlerMessage<TState, TMessage> : IHandler<TMessage> where TState : class
    {
        private readonly HandlerId _id;
        private readonly WeakReference<TState> _state;
        private MessageHandler<TState, TMessage> _handler;

        public StatefulHandlerMessage(TState state, MessageHandler<TState, TMessage> handler)
        {
            _state = new(state ?? throw new ArgumentNullException(nameof(state)));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler, state.GetHashCode());
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested || _state.TryGetTarget(out var state) == false)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke(state, message, cancelToken) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class StatefulHandlerFuncCancelToken<TState, TMessage> : IHandler<TMessage> where TState : class
    {
        private readonly HandlerId _id;
        private readonly WeakReference<TState> _state;
        private Func<TState, CancellationToken, UniTask> _handler;

        public StatefulHandlerFuncCancelToken(TState state, Func<TState, CancellationToken, UniTask> handler)
        {
            _state = new(state ?? throw new ArgumentNullException(nameof(state)));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler, state.GetHashCode());
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested || _state.TryGetTarget(out var state) == false)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke(state, cancelToken) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class StatefulHandlerFuncMessage<TState, TMessage> : IHandler<TMessage> where TState : class
    {
        private readonly HandlerId _id;
        private readonly WeakReference<TState> _state;
        private Func<TState, TMessage, UniTask> _handler;

        public StatefulHandlerFuncMessage(TState state, Func<TState, TMessage, UniTask> handler)
        {
            _state = new(state ?? throw new ArgumentNullException(nameof(state)));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler, state.GetHashCode());
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested || _state.TryGetTarget(out var state) == false)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke(state, message) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class StatefulHandlerFunc<TState, TMessage> : IHandler<TMessage> where TState : class
    {
        private readonly HandlerId _id;
        private readonly WeakReference<TState> _state;
        private Func<TState, UniTask> _handler;

        public StatefulHandlerFunc(TState state, Func<TState, UniTask> handler)
        {
            _state = new(state ?? throw new ArgumentNullException(nameof(state)));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler, state.GetHashCode());
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested || _state.TryGetTarget(out var state) == false)
            {
                return UniTask.CompletedTask;
            }

            return _handler?.Invoke(state) ?? UniTask.CompletedTask;
        }
    }

    internal sealed class StatefulHandlerActionMessage<TState, TMessage> : IHandler<TMessage> where TState : class
    {
        private readonly HandlerId _id;
        private readonly WeakReference<TState> _state;
        private Action<TState, TMessage> _handler;

        public StatefulHandlerActionMessage(TState state, Action<TState, TMessage> handler)
        {
            _state = new(state ?? throw new ArgumentNullException(nameof(state)));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler, state.GetHashCode());
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested || _state.TryGetTarget(out var state) == false)
            {
                return UniTask.CompletedTask;
            }

            _handler?.Invoke(state, message);
            return UniTask.CompletedTask;
        }
    }

    internal sealed class StatefulHandlerAction<TState, TMessage> : IHandler<TMessage> where TState : class
    {
        private readonly HandlerId _id;
        private readonly WeakReference<TState> _state;
        private Action<TState> _handler;

        public StatefulHandlerAction(TState state, Action<TState> handler)
        {
            _state = new(state ?? throw new ArgumentNullException(nameof(state)));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _id = new(handler, state.GetHashCode());
        }

        public HandlerId Id => _id;

        public void Dispose()
        {
            _handler = null;
        }

        public UniTask Handle(TMessage message, CancellationToken cancelToken)
        {
            if (cancelToken.IsCancellationRequested || _state.TryGetTarget(out var state) == false)
            {
                return UniTask.CompletedTask;
            }

            _handler?.Invoke(state);
            return UniTask.CompletedTask;
        }
    }
}
