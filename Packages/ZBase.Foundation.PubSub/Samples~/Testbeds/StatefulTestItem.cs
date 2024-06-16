using UnityEngine;

namespace ZBase.Foundation.PubSub.Testbeds
{
    internal class StatefulTestItem : MonoBehaviour
    {
        [SerializeField]
        private int _id;

        private ISubscription _subscription;

        private void Awake()
        {
            _subscription = GlobalMessenger.Subscriber.Scope<TestScope>().WithState(this)
                .Subscribe<InitMessage>(static (state, msg) => state.Handle(msg));
        }

        private void OnDestroy()
        {
            _subscription?.Dispose();
            _subscription = null;
        }

        private void Handle(InitMessage _)
        {
            _subscription?.Dispose();

            _subscription = GlobalMessenger.Subscriber.Scope<TestScope>().WithState(this)
                .Subscribe<EventMessage>(static (state, msg) => state.Handle(msg));

            Debug.Log($"Init {_id}");
        }

        private void Handle(EventMessage msg)
        {
            if (msg.Value == _id)
            {
                Debug.Log(_id, this);
            }
        }
    }
}