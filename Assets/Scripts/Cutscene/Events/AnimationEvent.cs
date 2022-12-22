using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Cutscene
{
    public class AnimationEvent : CutsceneEvent
    {
        private GameObject go;
        private string animation;
        private bool isPlayer;

        public AnimationEvent(GameObject go, string animation, bool isPlayer)
        {
            this.isPlayer = isPlayer;
            this.go = go;
            this.animation = animation;
        }


        public override async Task HandleEvent(CutsceneSystem system)
        {
            if (isPlayer)
            {
                go.GetComponent<PlayerStateMachine>().EmoteState.Animation = animation;
                go.GetComponent<PlayerStateMachine>().Transition(go.GetComponent<PlayerStateMachine>().EmoteState, true);
            }

            else
            {
                go.GetComponent<Animator>().Play(animation);
            }
        }
    }

}