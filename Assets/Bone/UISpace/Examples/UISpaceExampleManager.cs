using Bone.BGUIUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.UISpace.Example
{
    public class UISpaceExampleManager : MonoBehaviour
    {
        public Sprite Marker;
        public float Speed;

        Vector2 tar = Vector2.zero;
        float IconSize = 50f;
        float r = 1;
        float g = 0;
        float b = 0;
        void Update()
        {
            // Simple Walkaround for the cube being 1f tall.
            var PlayerWalkTarget = new Vector3(tar.x, 0f, tar.y);
            if(Vector3.Distance(transform.position, PlayerWalkTarget) == 0f)
            {
                // Simple randomly picking a new cell to move to
                var rng = new System.Random();
                var x = rng.Next(-5, 5);
                var y = rng.Next(-5, 5);
                tar = new Vector2(x, y);
                PlayerWalkTarget = new Vector3(tar.x, 0, tar.y);

                // This is the entire thing.
                UISpace.Instance.Request("UISpace Example", PlayerWalkTarget)
                .WithSprite(Marker)
                .WithDuration(1f)
                .Fades(true)
                .Sized(IconSize)
                .Colored(new Color(r, g, b));
            }


            transform.position = Vector3.MoveTowards(transform.position, PlayerWalkTarget, Speed*Time.deltaTime);
        }

        
        private void OnGUI()
        {
            BGUI.BeginVerticalStack(5, 5, 8)
                .Label($"Marker Size({IconSize.ToString()}):")
                .Slider(SetIconSize, IconSize, 10, 400, 100)

                .Label($"Red({r.ToString()}):")
                .Slider(SetR, r, 0, 1, 100)

                .Label($"Green({g.ToString()}):")
                .Slider(SetG, g, 0, 1, 100)

                .Label($"Blue({b.ToString()}):")
                .Slider(SetB, b, 0, 1, 100)
                .Background()
                .Commit();
        }
        void SetIconSize(float f)
        {
            IconSize = f;
        }
        void SetR(float f)
        {
            r = f;
        }
        void SetG(float f)
        {
            g = f;
        }
        void SetB(float f)
        {
            b = f;
        }
    }
}
