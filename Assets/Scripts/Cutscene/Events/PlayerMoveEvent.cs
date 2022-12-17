using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Cutscene
{
    public class PlayerMoveEvent : CutsceneEvent
    {
        public PlayerMoveEvent(float direction, int duration, bool walk = false)
        {
            Direction = direction;
            DurationMilliseconds = duration;
            Walk = walk;

        }
        public float Direction;
        public int DurationMilliseconds;
        public bool Walk;

        public override async Task HandleEvent(CutsceneSystem system)
        {
            await system.stateHandler.Move(Direction, DurationMilliseconds, Walk);
        }
    }

}