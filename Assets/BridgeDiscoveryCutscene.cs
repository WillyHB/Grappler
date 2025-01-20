using System.Collections;
using System.Collections.Generic;
using Cutscene;
using UnityEngine;

public class BridgeDiscoveryCutscene : CutsceneSystem
{
    public GameObject DialogueSystemPrefab;
    public Potrait PlayerPotrait,
    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>() {
            new DelayEvent(1000),
            new Dialogue(DialogueSystemPrefab, new List<DialogueEvent>() {

                new DialogueTextEvent("This is the bridge George was talking about!", PlayerPotrait.Shocked, true),
            }),
            new PlayerMoveEvent(1, 5, true),
            new Dialogue(DialogueSystemPrefab, new List<DialogueEvent>() {

                new DialogueTextEvent("It collapsed!", PlayerPotrait.Fear, true),

            }),

        };
    }

}
