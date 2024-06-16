using System;

namespace ZBase.Foundation.PubSub.Testbeds
{
    internal readonly struct TestScope : IEquatable<TestScope>
    {
        public bool Equals(TestScope other)
            => true;

        public override bool Equals(object obj)
            => obj is TestScope;

        public override int GetHashCode()
            => 0;
    }

    internal readonly struct InitMessage : IMessage { }

    internal readonly struct EventMessage : IMessage
    {
        public int Value { get; }

        public EventMessage(int value)
        {
            Value = value;
        }
    }
}