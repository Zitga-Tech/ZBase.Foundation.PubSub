namespace ZBase.Foundation.PubSub
{
    public readonly struct PublishingContext
    {
        public CallerInfo Caller { get; }

        public PublishingContext(in CallerInfo caller)
        {
            Caller = caller;
        }
    }
}
