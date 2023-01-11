using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeEndCutscene : CutsceneSystem
{
    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new Cutscene.CustomFunctionEvent(()=>GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero),
            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraZoomEvent(4),
            new Cutscene.DelayEvent(1500),
            new Cutscene.AnimationEvent(GameObject.Find("Player"), "Phew", true),
            new Cutscene.DelayEvent(2000),
            new Cutscene.CameraZoomEvent(0),
            new Cutscene.CustomFunctionEvent(()=>Destroy(this)),
        };
    }
}
