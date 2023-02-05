using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribesmanCutscene : CutsceneSystem
{
    public Transform CutsceneTransform;
    public GameObject Tribesman;
    public GameObject Player;
    public GameObject DialogueSystemPrefab;
    public Potrait PlayerPotrait;

    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new Cutscene.CameraMoveEvent(CutsceneTransform),
            new Cutscene.CameraZoomEvent(2),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(1, 1000, true),
            new Cutscene.AnimationEvent(Tribesman, "Stand", false),
            new Cutscene.DelayEvent(500),
            new Cutscene.CustomFunctionEvent(()=>Tribesman.GetComponent<SpriteRenderer>().flipX = true),
            new Cutscene.AnimationEvent(Tribesman, "Walk", false),
            new Cutscene.MoveEvent(Tribesman, -Vector2.right, 1500),
            new Cutscene.AnimationEvent(Tribesman, "Idle", false),
            new Cutscene.DelayEvent(500),
            new Cutscene.Dialogue(DialogueSystemPrefab, new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Hello?", PlayerPotrait.Flabbergasted, true)
            }),
            new Cutscene.CustomFunctionEvent(()=>Tribesman.GetComponent<SpriteRenderer>().flipX = false),
            new Cutscene.AnimationEvent(Tribesman, "Run", false),
            new Cutscene.MoveEvent(Tribesman, Vector2.right * 17.5f, 4000),
            new Cutscene.CustomFunctionEvent(()=>Destroy(Tribesman)),
            new Cutscene.Dialogue(DialogueSystemPrefab, new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Wait come back!", PlayerPotrait.Shocked, true),
                new Cutscene.DialogueTextEvent("There's still people living here?!", PlayerPotrait.Confused, true),
                new Cutscene.DialogueTextEvent("I should follow him...", PlayerPotrait.Confused, true),
            }),
            new Cutscene.CameraZoomEvent(0),
            new Cutscene.CameraMoveEvent(Player.transform),

        };
    }
}
