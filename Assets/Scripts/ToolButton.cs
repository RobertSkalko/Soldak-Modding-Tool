using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SoldakModdingTool
{
    public abstract class ToolButton
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        private bool StoppedBecauseError = false;

        public void DoAction()
        {
            StoppedBecauseError = false;

            if (!Directory.Exists(Save.file.FilesToEditPath)) {
                Debug.Log("Please Enter FilesToEditPath");
                StoppedBecauseError = true;
            }
            if (!Directory.Exists(Save.file.OutputPath)) {
                Debug.Log("Please Enter OutputPath");
                StoppedBecauseError = true;
            }
            if (string.IsNullOrEmpty(Save.file.ModName)) {
                Debug.Log("Please Enter ModName");
                StoppedBecauseError = true;
            }
            if (!StoppedBecauseError) {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Action();
                stopwatch.Stop();
                Debug.Log("Action Took: " + stopwatch.ElapsedMilliseconds + " Miliseconds or " + stopwatch.ElapsedMilliseconds / 1000 + " Seconds");
            }
        }

        protected abstract void Action();
    }
}