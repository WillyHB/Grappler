using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Cutscene
{
    public class DelayEvent : CutsceneEvent
    {
        public DelayEvent(int milliseconds) => Milliseconds = milliseconds;
        public int Milliseconds;

        public override async Task HandleEvent(CutsceneSystem system)
        {
            await Task.Delay(Milliseconds);
        }
    }

}