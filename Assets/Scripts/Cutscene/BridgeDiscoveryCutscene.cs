using System.Collections;
using System.Collections.Generic;
using Cutscene;
using UnityEngine;

public class BridgeDiscoveryCutscene : CutsceneSystem
{
    public GameObject DialogueSystemPrefab;
    public Potrait PlayerPotrait;
    public Transform CamTransform;
    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>() {
            new CustomFunctionEvent(() => Destroy(GetComponent<BoxCollider2D>())),
            new DelayEvent(1000),
            new CameraMoveEvent(CamTransform),
            new Dialogue(DialogueSystemPrefab, new List<DialogueEvent>() 
            {
                new DialogueTextEvent("This is the bridge George was talking about!", PlayerPotrait.Shocked, true),
            }),
            new PlayerMoveEvent(1, 3500, false),
            new Dialogue(DialogueSystemPrefab, new List<DialogueEvent>() {

                new DialogueTextEvent("It collapsed!", PlayerPotrait.Fear, true),
                new DialogueTextEvent("I guess I'm gonna have to make use of my grapplehook...", PlayerPotrait.Confused, true),

            }),
            new CameraMoveEvent(null),

        };
    }

}
