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
            Debug.Log(text);

            await Task.Delay(2000);
        }
    }

}