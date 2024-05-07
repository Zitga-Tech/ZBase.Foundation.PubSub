using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    public partial class MessageSubscriber
    {
        private readonly SingletonContainer<MessageBroker> _brokers;
        private readonly CappedArrayPool<UniTask> _taskArrayPool;

        internal MessageSubscriber(
              SingletonContainer<MessageBroker> brokers
            , CappedArrayPool<UniTask> taskArrayPool
        )
        {
            _brokers = brokers;
            _taskArrayPool = taskArrayPool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<GlobalScope> Global()
        {
            return new(this, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<TScope> Scope<TScope>()
            where TScope : struct
        {
            return new(this, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<TScope> Scope<TScope>([NotNull] TScope scope)
        {
            return new(this, scope);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UnitySubscriber<TScope> UnityScope<TScope>([NotNull] TScope scope)
            where TScope : UnityEngine.Object
        {
            return new(this, scope);
        }
    }
}
