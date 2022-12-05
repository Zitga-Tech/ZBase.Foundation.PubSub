// https://github.com/hadashiA/UniTaskPubSub/blob/master/Assets/UniTaskPubSub/Runtime/Internal/CappedArrayPool.cs

using System;

namespace ZBase.Foundation.PubSub.Internals
{
    internal sealed class CappedArrayPool<T>
    {
        internal const int INITIAL_BUCKET_SIZE = 4;

        public static readonly T[] EmptyArray = new T[0];
        public static readonly CappedArrayPool<T> Shared8Limit = new(8);

        readonly T[][][] _buckets;
        readonly object _syncRoot = new();
        readonly int[] _tails;

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

                _tails[i] = _buckets[i].Length - 1;
            }
        }

        public T[] Rent(int length)
        {
            if (length <= 0)
                return EmptyArray;

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

                var result = bucket[tail] ?? new T[length];
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
                if (_tails[i] > 0)
                    _tails[i] -= 1;
            }
        }
    }
}