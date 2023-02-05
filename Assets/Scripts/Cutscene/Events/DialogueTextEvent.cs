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
        private Sprite potrait;

        public DialogueTextEvent(string text, Sprite potrait, bool isPlayer = false)
        {
            this.text = text;
            this.isPlayer = isPlayer;
            this.potrait = potrait;
        }

        public override async Task HandleEvent(Dialogue system)
        {
            await system.DialogueSystem.GetComponent<DialogueSystem>().SetText(text, GameData.Load().dialogueTextSpeed, isPlayer, potrait);
        }
    }

}