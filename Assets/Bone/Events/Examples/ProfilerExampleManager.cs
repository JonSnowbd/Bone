using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.Events.Example
{
    public class ProfilerExampleManager : MonoBehaviour
    {
        int RunsPerUpdate = 100;
        string runs = "100";
        void FixedUpdate()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Events");
            for (int i = 0; i < RunsPerUpdate; i++)
            {
                BasicColorEvent.Raise(new ColorEventData(Color.red));
            }
            UnityEngine.Profiling.Profiler.EndSample();
        }

        private void OnGUI()
        {
            runs = GUI.TextField(new Rect(10, 10, 80, 22), runs);
            int x;
            if(int.TryParse(runs, out x))
            {
                RunsPerUpdate = x;
            }
        }
    }
}

