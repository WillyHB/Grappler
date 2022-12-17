using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Cutscene
{
    public class Dialogue : CutsceneEvent
    {
        public List<DialogueEvent> Events { get; set; }

        public Dialogue(List<DialogueEvent> events = null)
        {
            Events = events ?? new();
        }

        public override async Task HandleEvent(CutsceneSystem system)
        {
            foreach (var e in Events)
            {
                await e.HandleEvent(this);
            }
        }
    }

}