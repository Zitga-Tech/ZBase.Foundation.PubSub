using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.PubSub.Samples
{
    /// <summary>
    /// Samples of using PubSub in a specific scope
    /// </summary>
    public class SpecificScopeUsage : MonoBehaviour
    {
        [SerializeField]
        private Button _subscribeButton;

        [SerializeField]
        private Button _unsubscribeButton;

        private readonly Messenger _messenger = new();
        private CancellationTokenSource _unsubcribeCts;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            _subscribeButton.onClick.AddListener(SubscribeToAllMessages);
            _unsubscribeButton.onClick.AddListener(UnsubscribeFromAllMessages);
        }

        private void Start()
        {
            SubscribeToAllMessages();
        }

        private void SubscribeToAllMessages()
        {
            if (_unsubcribeCts != null)
            {
                return;
            }

            _unsubcribeCts = new();

            var subscriber = _messenger.MessageSubscriber;
            var token = _unsubcribeCts.Token;

            subscriber.Scope(new IdScope(5))
               .Subscribe<FooMessage>(FooHandler_In_IdScope, unsubscribeToken: token);

            subscriber.Scope(new NameScope("Bar"))
               .Subscribe<BarMessage>(BarHandler_In_NameScope, unsubscribeToken: token);

            subscriber.Scope(new UnityObjectScope(this))
               .Subscribe<FooMessage>(FooHandler_In_MonoBehaviourScope, unsubscribeToken: token);

            subscriber.UnityScope(this.gameObject)
               .Subscribe<FooMessage>(FooHandler_In_GameObjectScope, unsubscribeToken: token);

            Debug.Log("System has subscribed to all messages.");
        }

        private void UnsubscribeFromAllMessages()
        {
            _unsubcribeCts?.Cancel();
            _unsubcribeCts?.Dispose();
            _unsubcribeCts = null;

            Debug.Log("System has unsubscribed from all messages.");
        }

        private void Update()
        {
            var pub = _messenger.MessagePublisher;

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                pub.Scope(new IdScope(5))
                    .Publish(new FooMessage { value = "[5] Fooooooo" });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                pub.Scope(new NameScope("Bar"))
                    .Publish(new BarMessage { value = UnityEngine.Random.Range(0, 100) });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                pub.Scope(new UnityObjectScope(this))
                   .Publish(new FooMessage { 
                    value = $"[{nameof(SpecificScopeUsage)}] InstanceId = {this.GetInstanceID()}" 
                });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                pub.UnityScope(this.gameObject)
                   .Publish(new FooMessage {
                    value = $"[{nameof(GameObject)}] InstanceId = {this.gameObject.GetInstanceID()}" 
                });
                return;
            }
        }

        private static void FooHandler_In_IdScope(FooMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: {msg.value}");
        }

        private static void BarHandler_In_NameScope(BarMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: {msg.value}");
        }

        private static void FooHandler_In_GameObjectScope(FooMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: {msg.value}");
        }

        private static void FooHandler_In_MonoBehaviourScope(FooMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: {msg.value}");
        }
    }
}
