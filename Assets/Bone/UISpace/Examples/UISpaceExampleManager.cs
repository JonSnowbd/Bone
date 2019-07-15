using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bone.UISpace.Example
{
    public class UISpaceExampleManager : MonoBehaviour
    {
        public Sprite Marker;
        public float Speed;

        Vector2 tar = Vector2.zero;
        void Update()
        {
            // Simple Walkaround for the cube being 1f tall.
            var true_target = new Vector3(tar.x, 0.5f, tar.y);
            if(Vector3.Distance(transform.position, true_target) == 0f)
            {
                // Simple randomly picking a new cell to move to
                var rng = new System.Random();
                var x = rng.Next(-5, 5);
                var y = rng.Next(-5, 5);
                tar = new Vector2(x, y);
                true_target = new Vector3(tar.x, 0.5f, tar.y);

                // This is pretty much the entire thing.
                // In order of Variables:
                // "destination" -> This is the identification ID of the request, you should always keep this the same if you are updating the same marker.
                // Marker -> This is a reference to the sprite you want it to show, you can change this in real time and it will seamlessly change.
                // new Vector3(...) -> This is the in-world location you want
                // 2f -> The duration of the request. If you want this to disappear after 2 seconds, set this to 2f. if you want this to only show on the frame its been requested, set it to null
                // true -> This is whether it should fade out as the duration expires. pointless if the previous value is null.
                UISpace.Instance.Request("destination", Marker, new Vector3(tar.x, 0f, tar.y), 2f, true);

            }
            transform.position = Vector3.MoveTowards(transform.position, true_target, Speed*Time.deltaTime);
        }
        private void OnGUI()
        {
            GUI.Label(new Rect(5f, 5f, 400, 60), new GUIContent("The cube will move to a random position, the marker is a \nUISpace marker that is displayed on the target location."));
        }
    }
}
