using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Events/PlayerMove")]
public class PlayerMoveEvent : CutsceneEvent
{
    public PlayerMoveEvent(float direction, int duration)
    {
        Direction = direction;
        DurationMilliseconds = duration;
    }
    public float Direction;
    public int DurationMilliseconds;
}
