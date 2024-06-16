namespace ZBase.Foundation.PubSub.Testbeds
{
    internal static class GlobalMessenger
    {
        private static readonly Messenger s_instance = new();

        public static MessageSubscriber Subscriber => s_instance.MessageSubscriber;

        public static MessagePublisher Publisher => s_instance.MessagePublisher;
    }
}