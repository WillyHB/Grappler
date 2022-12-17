using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeStartCutscene : CutsceneSystem
{
    public Transform MoveTransform;
    public Transform Player;

    public override void GenerateCutscene()
    {
        Events.Add(new Cutscene.DelayEvent(2000));
        Events.Add(new Cutscene.CameraMoveEvent(MoveTransform));
        Events.Add(new Cutscene.CameraZoomEvent(4));
        Events.Add(new Cutscene.DelayEvent(5000));
        Events.Add(new Cutscene.PlayerMoveEvent(-1, 2000, true));
        Events.Add(new Cutscene.DelayEvent(1000));
        Events.Add(new Cutscene.Dialogue(new List<Cutscene.DialogueEvent>()
        {
            new Cutscene.DialogueTextEvent("Yeah Mama"),
             new Cutscene.DialogueTextEvent("And papa too!"),
        }));
        Events.Add(new Cutscene.DelayEvent(2000));
        Events.Add(new Cutscene.PlayerMoveEvent(1, 2000, true));
        Events.Add(new Cutscene.DelayEvent(1000));
        Events.Add(new Cutscene.PlayerMoveEvent(-0.1f, 10, true));
        Events.Add(new Cutscene.DelayEvent(3000));
        Events.Add(new Cutscene.PlayerMoveEvent(0.1f, 10, true));
        Events.Add(new Cutscene.CameraMoveEvent(Player));
        Events.Add(new Cutscene.CameraZoomEvent(0));
    }
}
