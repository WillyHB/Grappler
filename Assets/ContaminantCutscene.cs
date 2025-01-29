using System.Collections;
using System.Collections.Generic;
using Cutscene;
using UnityEngine;

public class ContaminantCutscene : CutsceneSystem
{

    public GameObject DialogueSystemPrefab;
    public Potrait PlayerPotrait;
    public GameObject Player;
    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>() {
            new PlayerMoveEvent(1, 500, true),
            new DelayEvent(1000),
            new Dialogue(DialogueSystemPrefab, new List<DialogueEvent>() {
                new DialogueTextEvent("Oh woow that stinks!", PlayerPotrait.Fear, true),
                new DialogueTextEvent("I should probably avoid touching that water...", PlayerPotrait.CloseEyes, true),
            }),
            new CameraMoveEvent(Player.transform),
            new DelayEvent(500),
            new CustomFunctionEvent(()=>Destroy(gameObject)),
        };
    }
}
