using UnityEngine;

namespace ZBase.Foundation.PubSub.Samples
{
    /// <summary>
    /// Samples of using PubSub in a specific scope
    /// </summary>
    public class SpecificScopeUsage : MonoBehaviour
    {
        private readonly Messenger _messenger = new();

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            var sub = _messenger.MessageSubscriber;

            sub.Scope(new IdScope(5)).Subscribe<FooMessage>(FooHandler_In_IdScope);
            sub.Scope(new NameScope("Bar")).Subscribe<BarMessage>(BarHandler_In_NameScope);
            sub.Scope(new UnityObjectScope(this)).Subscribe<FooMessage>(FooHandler_In_MonoBehaviourScope);
            sub.Scope(new UnityObjectScope(this.gameObject)).Subscribe<FooMessage>(FooHandler_In_GameObjectScope);
        }

        private void Update()
        {
            var pub = _messenger.MessagePublisher;

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                pub.Scope(new IdScope(5)).Publish(new FooMessage { value = "[5] Fooooooo" });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                /// Sometimes the user will forget to publish to the right specific scope
                /// and they publish to the global scope instead.
                /// This is a misuse of extension APIs.
                pub.Publish(new BarMessage { value = UnityEngine.Random.Range(0, 100) });
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
                pub.Scope(new UnityObjectScope(this.gameObject))
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
