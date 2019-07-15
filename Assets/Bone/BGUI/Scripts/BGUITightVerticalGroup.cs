using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.BGUIUtil
{
    public class BGUITightVerticalGroup : BGUIGroup
    {

        private float YOffset;
        private float X;
        private float Y;
        public float Padding { get; private set; }
        public BGUITightVerticalGroup(float x, float y, float pad)
        {
            X = x;
            Y = y;
            Padding = pad;
            YOffset = pad;
            BoundingWidth = 0f;
        }

        public override Rect RequestNextRect(float preferredWidth, float preferredHeight)
        {
            if (preferredWidth >= BoundingWidth)
                BoundingWidth = preferredWidth;
            Rect new_position = new Rect(
                X + Padding,
                Y + YOffset,
                preferredWidth,
                preferredHeight
                );
            YOffset += preferredHeight + Padding;
            return new_position;
        }

        public override Rect Commit()
        {
            foreach (var req in Requests)
            {
                req.Draw();
            }
            Requests.Clear();
            return GetBoundingRect();
        }

        public override Rect GetBoundingRect()
        {
            return new Rect(X, Y, BoundingWidth + Padding * 2, BoundingHeight);
        }

        private float BoundingWidth;
        private float BoundingHeight { get { return YOffset; } }
    }
}