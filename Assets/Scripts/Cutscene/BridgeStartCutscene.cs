using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeStartCutscene : CutsceneSystem
{
    public Transform MoveTransform;
    public Transform Player;

    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(MoveTransform),
            new Cutscene.CameraZoomEvent(4),
            new Cutscene.DelayEvent(5000),
            new Cutscene.PlayerMoveEvent(-1, 2000, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.Dialogue(new List<Cutscene.DialogueEvent>()
            {
            new Cutscene.DialogueTextEvent("Yeah Mama"),
             new Cutscene.DialogueTextEvent("And papa too!"),
            }),
            new Cutscene.DelayEvent(2000),
            new Cutscene.PlayerMoveEvent(1, 2000, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(-0.1f, 10, true),
            new Cutscene.DelayEvent(3000),
            new Cutscene.PlayerMoveEvent(0.1f, 10, true),
            new Cutscene.CameraMoveEvent(Player),
            new Cutscene.CameraZoomEvent(0)
        };
    }
}
