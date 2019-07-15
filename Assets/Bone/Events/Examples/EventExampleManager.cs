using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.Events.Example
{
    public class EventExampleManager : MonoBehaviour
    {
        float red = 0.3f;
        float green = 0.3f;
        float blue = 0.3f;
        private void OnGUI()
        {
            GUI.Label(new Rect(180, 5, 200, 20), "Red");
            GUI.Label(new Rect(180, 20, 200, 20), "Green");
            GUI.Label(new Rect(180, 35, 200, 20), "Blue");
            red = GUI.HorizontalSlider(new Rect(10, 10, 150, 8), red, 0f, 1f);
            green = GUI.HorizontalSlider(new Rect(10, 25, 150, 8), green, 0f, 1f);
            blue = GUI.HorizontalSlider(new Rect(10, 40, 150, 8), blue, 0f, 1f);
            if (GUI.Button(new Rect(10, 75, 100, 22), "Send"))
            {
                BasicColorEvent.Raise(new ColorEventData(new Color(red,green,blue,1f)));
            }
        }
    }
}

