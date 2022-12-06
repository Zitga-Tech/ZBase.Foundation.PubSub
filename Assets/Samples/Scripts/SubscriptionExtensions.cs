using System.Collections.Generic;

namespace ZBase.Foundation.PubSub.Samples
{
    public static class SubscriptionExtensions
    {
        public static void AddTo(this ISubscription self, List<ISubscription> list)
            => list?.Add(self);
    }
}
