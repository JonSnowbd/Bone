using Bone.BGUIUtil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.BGUIUtil.Example
{
    public class BGUIExampleScript : MonoBehaviour
    {
        float spacing = 5f;
        private void OnGUI()
        {
            var bb = BGUI.BeginVerticalStack(5f, 5f, spacing)
                .Button(ResetPadding, "Reset Padding")
                .Label($"Padding ({spacing.ToString()})")
                .Slider(SetPadding, spacing, 0, 50, 110)
                .Background()
                .Commit();
            BGUI.BeginVerticalStack(5f, bb.y + bb.height + spacing, spacing)
                .Label("Hey! this is a\nchained box.")
                .Background()
                .Commit();
        }
        void SetPadding(float f)
        {
            spacing = f;
        }
        void ResetPadding()
        {
            spacing = 5f;
        }
    }
}

