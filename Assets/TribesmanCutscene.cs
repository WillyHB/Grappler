using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribesmanCutscene : CutsceneSystem
{
    public Transform CutsceneTransform;
    public GameObject Tribesman;
    public GameObject Player;

    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new Cutscene.CameraMoveEvent(CutsceneTransform),
            new Cutscene.CameraZoomEvent(2),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(1, 1000, true),
            new Cutscene.AnimationEvent(Tribesman, "Stand", false),
            new Cutscene.DelayEvent(750),
            new Cutscene.DelayEvent(1000),
            new Cutscene.AnimationEvent(Tribesman, "Run", false),
            new Cutscene.MoveEvent(Tribesman, Vector2.right * 15, 5000),
            new Cutscene.CustomFunctionEvent(()=>Destroy(Tribesman)),
            new Cutscene.CameraZoomEvent(0),
            new Cutscene.CameraMoveEvent(Player.transform),

        };
    }
}
