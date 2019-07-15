using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.Events.Example
{
    public class EventExampleCube : MonoBehaviour
    {
        public float Shade = 1f;
        // Start is called before the first frame update
        void OnEnable()
        {
            BasicColorEvent.AddListener(SetColor, Bone.Events.DefaultEventPriority.High);
        }
        private void OnDisable()
        {
            BasicColorEvent.RemoveListener(SetColor, Bone.Events.DefaultEventPriority.High);
        }
        private void SetColor(ColorEventData col)
        {
            var red = col.TargetValue.r * Shade;
            var green = col.TargetValue.g * Shade;
            var blue = col.TargetValue.b * Shade;
            GetComponent<Renderer>().material.color = new Color(red, green, blue, 1f);
        }
    }
}

