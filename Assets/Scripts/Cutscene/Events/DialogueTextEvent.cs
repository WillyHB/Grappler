using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Cutscene
{
    public class DialogueTextEvent : DialogueEvent
    {
        private string text;
        private bool isPlayer;

        public DialogueTextEvent(string text, bool isPlayer = false)
        {
            this.text = text;
        }

        public override async Task HandleEvent(Dialogue system)
        {
            await system.DialogueSystem.GetComponent<DialogueSystem>().SetText(text, 100, isPlayer);
        }
    }

}