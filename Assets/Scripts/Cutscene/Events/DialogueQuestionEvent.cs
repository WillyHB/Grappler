using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Cutscene
{
    public class DialogueQuestionEvent : DialogueEvent
    {
        private string question;
        private string answer1;
        private string answer2;
        private string answer3;

        public List<DialogueEvent> Answer1Events;
        public List<DialogueEvent> Answer2Events;
        public List<DialogueEvent> Answer3Events;

        private Sprite potrait;

        public DialogueQuestionEvent(string question, Sprite potrait, string answer1 = null, string answer2 = null, string answer3 = null)
        {
            this.potrait = potrait;
            this.question = question;

            this.answer1 = answer1;
            this.answer2 = answer2;
            this.answer3 = answer3;
        }

        public override async Task HandleEvent(Dialogue system)
        {
            Task<int> PoseQuestion = system.DialogueSystem.GetComponent<DialogueSystem>().SetOption(question, GameData.Load().dialogueTextSpeed, potrait, answer1, answer2, answer3);

            await PoseQuestion;
            int option = PoseQuestion.Result;

            foreach (var e in (option switch
            {
                1 => Answer1Events,
                2 => Answer2Events,
                3 => Answer3Events,
                _ => throw new System.Exception("Dialogue Option does not exist?")
            }))
            {
                await e.HandleEvent(system);
            }

        }
    }

}