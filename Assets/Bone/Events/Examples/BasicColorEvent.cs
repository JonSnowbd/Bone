using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Bone.Events.Example
{
    [Serializable]
    public class ColorEventData : ISerializable
    {
        public Color TargetValue { get; set; }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("col", TargetValue, typeof(Color));
        }
        public ColorEventData(SerializationInfo info, StreamingContext context)
        {
            TargetValue = (Color)info.GetValue("col", typeof(Color));
        }
        public ColorEventData(Color c)
        {
            TargetValue = c;
        }
    }
    public class BasicColorEvent : Bone.Events.BaseBoneEvent<ColorEventData>
    {
    }

}
