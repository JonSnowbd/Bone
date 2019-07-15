using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace Bone.Events
{
    public enum DefaultEventPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
    public class BaseBoneEvent<T, P> where T : ISerializable where P : Enum
    {
        #region Public Facing Functions
        public delegate void EventListenerDelegate(T data);

        private static List<int> CorrectOrderedP;
        public static void Raise(T data)
        {
            if(CorrectOrderedP == null)
            {
                CorrectOrderedP = new List<int>();
                foreach(int x in Enum.GetValues(typeof(P)))
                {
                    CorrectOrderedP.Add(x);
                }
                CorrectOrderedP.Sort();
            }
            foreach(int x in CorrectOrderedP)
            {
                P val = (P)Enum.ToObject(typeof(P), x);
                if (!PriorityLookup.ContainsKey(val))
                {
                    continue;
                }
                foreach(var del in PriorityLookup[val])
                {
                    del(data);
                }
            }
        }
        public static void AddListener(EventListenerDelegate del, P prio)
        {
            if (!PriorityLookup.ContainsKey(prio))
            {
                PriorityLookup[prio] = new List<EventListenerDelegate>();
            }
            if (PriorityLookup[prio].Contains(del))
            {
                throw new System.Exception("Delegate is already listening to this event.");
            }
            PriorityLookup[prio].Add(del);
        }
        public static void RemoveListener(EventListenerDelegate del, P prio)
        {
            if (!PriorityLookup.ContainsKey(prio))
            {
                throw new System.Exception("Attempting to remove delegate listener in priority that has no delegate listeners.");
            }
            if (!PriorityLookup[prio].Contains(del))
            {
                throw new System.Exception("Attempting to remove delegate listener that is not subscribed to this event.");
            }
            PriorityLookup[prio].Remove(del);
        }
        #endregion
        protected static Dictionary<P, List<EventListenerDelegate>> PriorityLookup = new Dictionary<P, List<EventListenerDelegate>>();
    }
    public class BaseBoneEvent<T> : BaseBoneEvent<T, DefaultEventPriority> where T : ISerializable
    {

    }
}

