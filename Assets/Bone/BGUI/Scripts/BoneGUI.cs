using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.BGUIUtil
{
    public static class BGUI
    {
        public static BGUIGroup BeginVerticalStack(float x, float y, float padding = 2f)
        {
            return new BGUITightVerticalGroup(x, y, padding);
        }
    }
}

