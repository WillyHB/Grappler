using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Cutscene
{
    public class DialogueTextEvent : DialogueEvent
    {
        private string text;

        public DialogueTextEvent(string text)
        {
            this.text = text;
        }

        public override async Task HandleEvent(Dialogue system)
        {
            await system.DialogueSystem.GetComponent<DialogueSystem>().SetText(text, 100);
        }
    }

}