using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.BGUIUtil
{
    public delegate void BGUIButtonDelegate();
    public delegate void BGUIValueDelegate<T>(T new_value);
    public interface IBGUIDrawRequest
    {
        void Draw();
    }
    class BGUILabelDrawRequest : IBGUIDrawRequest
    {
        Rect pos;
        GUIContent message;
        public BGUILabelDrawRequest(BGUIGroup group, string s)
        {
            message = new GUIContent(s);
            var labelsize = GUI.skin.label.CalcSize(message);
            pos = group.RequestNextRect(labelsize.x+1f, labelsize.y+1f); // This is due to minor clipping and CalcSize not being 100% accurate.
        }
        public void Draw()
        {
            GUI.Label(pos, message);
        }
    }
    class BGUISliderDrawRequest : IBGUIDrawRequest
    {
        Rect pos;
        BGUIValueDelegate<float> Delegate;
        float Left, Right, Width, Current;

        public BGUISliderDrawRequest(BGUIGroup group, float width, BGUIValueDelegate<float> del, float current, float left, float right)
        {
            Delegate = del;
            Current = current;
            Left = left;
            Right = right;
            Width = width;
            pos = group.RequestNextRect(Width, GUI.skin.horizontalSlider.fixedHeight);
        }
        public void Draw()
        {
            float new_val = GUI.HorizontalSlider(pos, Current, Left, Right);
            if (new_val != Current)
            {
                Delegate(new_val);
            }
        }
    }

    class BGUIButtonDrawRequest : IBGUIDrawRequest
    {
        BGUIButtonDelegate Delegate;
        Rect pos;
        GUIContent Message;

        public BGUIButtonDrawRequest(BGUIGroup context, BGUIButtonDelegate del, string s)
        {
            Message = new GUIContent(s);
            var size = GUI.skin.button.CalcSize(Message);
            pos = context.RequestNextRect(size.x + GUI.skin.button.padding.left, size.y);
            Delegate = del;
        }

        public void Draw()
        {
            if(GUI.Button(pos, Message))
            {
                Delegate();
            }
        }
    }
}