using System;

namespace ZBase.Foundation.PubSub.Samples
{
    public readonly struct IdScope : IEquatable<IdScope>
    {
        public readonly int Id;

        public IdScope(int value) { Id = value; }

        public override bool Equals(object obj)
            => obj is IdScope other && other.Id == Id;

        public bool Equals(IdScope other)
            => Id == other.Id;

        public override int GetHashCode()
            => Id.GetHashCode();
    }

    public readonly struct NameScope : IEquatable<NameScope>
    {
        public readonly string Name;

        public NameScope(string value) { Name = value; }

        public override bool Equals(object obj)
            => obj is NameScope other && other.Name == Name;

        public bool Equals(NameScope other)
            => Name == other.Name;

        public override int GetHashCode()
            => Name.GetHashCode();
    }

    public readonly struct UnityObjectScope : IEquatable<UnityObjectScope>
    {
        public readonly int InstanceId;

        public UnityObjectScope(UnityEngine.Object obj)
        {
            InstanceId = obj == false ? 0 : obj.GetInstanceID();
        }

        public override bool Equals(object obj)
            => obj is UnityObjectScope other && other.InstanceId == InstanceId;

        public bool Equals(UnityObjectScope other)
            => InstanceId == other.InstanceId;

        public override int GetHashCode()
            => InstanceId.GetHashCode();
    }
}
