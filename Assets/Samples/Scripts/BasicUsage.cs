using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ZBase.Foundation.PubSub.Samples
{
    /// <summary>
    /// Samples of using PubSub in the <see cref="GlobalScope"/>
    /// </summary>
    public class BasicUsage : MonoBehaviour
    {
        [SerializeField]
        private Button _subscribeButton;

        [SerializeField]
        private Button _unsubscribeButton;

        [SerializeField]
        private TMPro.TMP_Text _deltaTimeText;

        private readonly Messenger _messenger = new();
        private readonly List<ISubscription> _subscriptions = new();
        private CancellationTokenSource _cts;
        private CachedPublisher<DeltaTimeMessage> _cachedDeltaTimePublisher;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            _cachedDeltaTimePublisher = _messenger.MessagePublisher.GlobalCache<DeltaTimeMessage>();
            _subscribeButton.onClick.AddListener(SubscribeToAllMessages);
            _unsubscribeButton.onClick.AddListener(UnsubscribeFromAllMessages);
        }

        private void Start()
        {
            SubscribeToAllMessages();
        }

        private void SubscribeToAllMessages()
        {
            var subscriptions = _subscriptions;

            if (subscriptions.Count > 0)
            {
                return;
            }

            var subscriber = _messenger.MessageSubscriber.Global();

            subscriber.Subscribe<FooMessage>(FooHandler).AddTo(subscriptions);
            subscriber.Subscribe<BarMessage>(BarHandler).AddTo(subscriptions);
            subscriber.Subscribe<TimeMessage>(TimeHandlerAsync).AddTo(subscriptions);
            subscriber.Subscribe<CancellableTimeMessage>(CancellableTimeHandlerAsync).AddTo(subscriptions);
            subscriber.Subscribe<FrameMessage>(FrameHandlerAsync).AddTo(subscriptions);

            var stateSubscriber = subscriber.WithState(this);
            stateSubscriber.Subscribe<DeltaTimeMessage>(static (x, msg) => x.SetDeltaTimeText(msg)).AddTo(subscriptions);

            Debug.Log("System has subscribed to all messages.");
        }

        private void UnsubscribeFromAllMessages()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            var subscriptions = _subscriptions;

            if (subscriptions.Count <= 0)
            {
                return;
            }

            var count = subscriptions.Count;

            for (var i = 0; i < count; i++)
            {
                subscriptions[i]?.Unsubscribe();

                /// Same functionality:
                // subscriptions[i]?.Dispose();
            }

            subscriptions.Clear();

            Debug.Log("System has unsubscribed from all messages.");
        }

        private void Update()
        {
            var pub = _messenger.MessagePublisher.Global();

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                pub.Publish(new FooMessage { value = "Fooooooo" });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                pub.Publish(new BarMessage { value = UnityEngine.Random.Range(0, 100) });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                pub.Publish(new TimeMessage { seconds = 2f });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                _cts?.Dispose();
                _cts = new();

                Debug.Log(Time.realtimeSinceStartupAsDouble);

                _cts.CancelAfter(TimeSpan.FromSeconds(2f));
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                pub.Publish(new FrameMessage { frames = 120 });
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha6))
            {
                RunPublishAsync(_messenger).Forget();
                return;
            }

            if (Input.GetKeyUp(KeyCode.Alpha7))
            {
                var cachedPub = _cachedDeltaTimePublisher;
                cachedPub.Publish(new DeltaTimeMessage { value = Time.deltaTime });
                return;
            }
        }

        private static void FooHandler(FooMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: {msg.value}");
        }

        private static void BarHandler(BarMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: {msg.value}");
        }

        private static async UniTask TimeHandlerAsync(TimeMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: wait for {msg.seconds} seconds");

            await UniTask.Delay(TimeSpan.FromSeconds(msg.seconds));

            Debug.Log($"{msg.GetType().Name}: done");
        }

        private static async UniTask CancellableTimeHandlerAsync(CancellableTimeMessage msg, CancellationToken cancelToken)
        {
            const string NAME = nameof(CancellableTimeHandlerAsync);

            Debug.Log($"{msg.GetType().Name}: {NAME}: wait for {msg.seconds} seconds");

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(msg.seconds), cancellationToken: cancelToken);
            }
            catch
            {
                Debug.LogWarning($"Cancellable time system has shut down");
            }

            Debug.Log($"{msg.GetType().Name}: {NAME}: done");
        }

        private static async UniTask FrameHandlerAsync(FrameMessage msg)
        {
            const string NAME = nameof(FrameHandlerAsync);

            Debug.Log($"{msg.GetType().Name}: {NAME}: wait for {msg.frames} frames");

            await UniTask.DelayFrame(msg.frames);

            Debug.Log($"{msg.GetType().Name}: {NAME}: done");
        }

        private void SetDeltaTimeText(DeltaTimeMessage msg)
        {
            Debug.Log($"{msg.GetType().Name}: {msg.value}");

            _deltaTimeText.text = $"DT = {msg.value}";
        }

        private static async UniTaskVoid RunPublishAsync(Messenger messenger)
        {
            var pub = messenger.MessagePublisher.Global();

            Debug.Log($"[BEGIN] {nameof(RunPublishAsync)}");
            Debug.Log($"Publish {nameof(TimeMessage)}");

            await pub.PublishAsync(new TimeMessage { seconds = 1f });

            Debug.Log($"Done {nameof(TimeMessage)}");

            Debug.Log($"Publish {nameof(FrameMessage)}");

            await pub.PublishAsync(new FrameMessage { frames = 60 });

            Debug.Log($"Done {nameof(FrameMessage)}");
            Debug.Log($"[END] {nameof(RunPublishAsync)}");
        }
    }
}
