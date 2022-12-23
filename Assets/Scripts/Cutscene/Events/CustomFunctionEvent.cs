using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace Cutscene
{
    public class CustomFunctionEvent : CutsceneEvent
    {
        private Action function;

        public CustomFunctionEvent(Action function)
        {
            this.function = function;
        }

        public override async Task HandleEvent(CutsceneSystem system)
        {
            function.Invoke();
        }
    }

}