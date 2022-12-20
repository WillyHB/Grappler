using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Cutscene
{
    public class Dialogue : CutsceneEvent
    {
        public List<DialogueEvent> Events { get; set; }
        public GameObject DialogueSystemPrefab;
        public GameObject DialogueSystem;

        public Dialogue(GameObject dialogueSystemPrefab, List<DialogueEvent> events = null)
        {
            DialogueSystemPrefab = dialogueSystemPrefab;
            Events = events ?? new();
        }

        public override async Task HandleEvent(CutsceneSystem system)
        {
            DialogueSystem = Object.Instantiate(DialogueSystemPrefab);

            DialogueSystem.GetComponent<DialogueSystem>().Open();

            foreach (var e in Events)
            {
                await e.HandleEvent(this);
            }
            DialogueSystem.GetComponent<DialogueSystem>().Close();
            Object.Destroy(DialogueSystem);
        }
    }

}