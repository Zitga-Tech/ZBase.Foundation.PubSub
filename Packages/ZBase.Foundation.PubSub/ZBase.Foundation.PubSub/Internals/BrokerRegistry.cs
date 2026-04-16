using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZBase.Foundation.PubSub.Internals
{
    internal static class BrokerRegistry
    {
        private static readonly List<Action> s_cleanups = new();
        private static readonly object s_lock = new();

        static BrokerRegistry()
        {
            Init();
        }

        /// <seealso href="https://docs.unity3d.com/Manual/DomainReloading.html"/>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            lock (s_lock)
            {
                s_cleanups.Clear();
            }
        }

        public static void Register(Action cleanup)
        {
            lock (s_lock)
            {
                s_cleanups.Add(cleanup);
            }
        }

        public static void DisposeAll()
        {
            lock (s_lock)
            {
                foreach (var cleanup in s_cleanups)
                {
                    cleanup();
                }

                s_cleanups.Clear();
            }
        }
    }
}
