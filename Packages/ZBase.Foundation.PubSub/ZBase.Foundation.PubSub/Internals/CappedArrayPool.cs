// https://github.com/hadashiA/UniTaskPubSub/blob/master/Assets/UniTaskPubSub/Runtime/Internal/CappedArrayPool.cs

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class CappedArrayPool<T>
    {
        internal const int INITIAL_BUCKET_SIZE = 4;

        public static CappedArrayPool<T> Shared8Limit => s_shared8Limit;

        private readonly static bool s_isTManaged = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
        private static CappedArrayPool<T> s_shared8Limit;

        private readonly T[][][] _buckets;
        private readonly object _syncRoot = new();
        private readonly int[] _tails;

        static CappedArrayPool()
        {
            Init();
        }

        /// <seealso href="https://docs.unity3d.com/Manual/DomainReloading.html"/>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            s_shared8Limit = new(8);
        }

        internal CappedArrayPool(int maxLength)
        {
            _buckets = new T[maxLength][][];
            _tails = new int[maxLength];

            for (var i = 0; i < maxLength; i++)
            {
                var arrayLength = i + 1;
                _buckets[i] = new T[INITIAL_BUCKET_SIZE][];

                for (var j = 0; j < INITIAL_BUCKET_SIZE; j++)
                {
                    _buckets[i][j] = new T[arrayLength];
                }

                _tails[i] = 0;
            }
        }

        public T[] Rent(int length)
        {
            if (length <= 0)
                return Array.Empty<T>();

            if (length > _buckets.Length)
                return new T[length]; // Not supported

            var i = length - 1;

            lock (_syncRoot)
            {
                var bucket = _buckets[i];
                var tail = _tails[i];

                if (tail >= bucket.Length)
                {
                    Array.Resize(ref bucket, bucket.Length * 2);
                    _buckets[i] = bucket;
                }

                if (bucket[tail] == null)
                {
                    bucket[tail] = new T[length];
                }

                var result = bucket[tail];
                _tails[i] += 1;
                return result;
            }
        }

        public void Return(T[] array)
        {
            if (array.Length <= 0 || array.Length > _buckets.Length)
                return;

            var i = array.Length - 1;

            lock (_syncRoot)
            {
                if (s_isTManaged)
                    Array.Clear(array, 0, array.Length);

                if (_tails[i] > 0)
                    _tails[i] -= 1;
            }
        }
    }
}