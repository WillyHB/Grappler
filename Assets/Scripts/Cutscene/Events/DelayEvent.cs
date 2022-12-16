using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Events/Delay")]
public class DelayEvent : CutsceneEvent
{
    public DelayEvent(int milliseconds) => Milliseconds = milliseconds;
    public int Milliseconds;
}
