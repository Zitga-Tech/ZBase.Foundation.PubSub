using UnityEngine;

namespace ZBase.Foundation.PubSub.Testbeds
{
    internal class StatefulTestbed : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                Debug.Log("A");
                GlobalMessenger.Publisher.Scope<TestScope>()
                    .Publish(new InitMessage());
            }

            if (Input.GetKeyUp(KeyCode.B))
            {
                Debug.Log("B");
                GlobalMessenger.Publisher.Scope<TestScope>()
                    .Publish(new EventMessage(1));
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                Debug.Log("C");
                GlobalMessenger.Publisher.Scope<TestScope>()
                    .Publish(new EventMessage(2));
            }
        }
    }
}
