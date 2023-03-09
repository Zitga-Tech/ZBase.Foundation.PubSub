using System;

namespace ZBase.Foundation.PubSub.Internals
{
    internal abstract class MessageBroker : IDisposable
    {
        public abstract void Dispose();
    }
}