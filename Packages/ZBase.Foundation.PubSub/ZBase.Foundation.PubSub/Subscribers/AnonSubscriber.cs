#if !(UNITY_EDITOR || DEBUG) || DISABLE_ZBASE_PUBSUB_DEBUG
#define __ZBASE_FOUNDATION_PUBSUB_NO_VALIDATION__
#endif

using Cysharp.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using ZBase.Foundation.PubSub.Internals;
using ZBase.Foundation.Singletons;

namespace ZBase.Foundation.PubSub
{
    /// <summary>
    /// Anonymous Subscriber allows registering handlers that take no message argument
    /// </summary>
    public partial class AnonSubscriber
    {
        private readonly MessageSubscriber _subscriber;

        internal AnonSubscriber(
              SingletonContainer<MessageBroker> brokers
            , CappedArrayPool<UniTask> taskArrayPool
        )
        {
            _subscriber = new(brokers, taskArrayPool);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<GlobalScope> Global()
        {
            return new(_subscriber.Global());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<TScope> Scope<TScope>()
            where TScope : struct
        {
            return new(_subscriber.Scope(default(TScope)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<TScope> Scope<TScope>([NotNull] TScope scope)
        {
            return new(_subscriber.Scope(scope));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Subscriber<TScope> UnityScope<TScope>([NotNull] TScope scope)
            where TScope : UnityEngine.Object
        {
            return new(_subscriber.UnityScope(scope));
        }
    }
}
