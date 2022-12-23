using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeStartCutscene : CutsceneSystem
{
    public Transform MoveTransform;
    public Transform GeorgeTransform;
    public Transform Player;
    public GameObject DialogueSystemPrefab;

    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new Cutscene.CustomFunctionEvent(Player.GetComponentInChildren<Grapple>().DisableGrapple),
            new Cutscene.DelayEvent(1000),
            new Cutscene.AnimationEvent(Player.gameObject, "Intro1", true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(MoveTransform),
            new Cutscene.CameraZoomEvent(4),
            new Cutscene.DelayEvent(5000),
            new Cutscene.AnimationEvent(Player.gameObject, "Intro2", true),
            new Cutscene.DelayEvent(4000),
            new Cutscene.PlayerMoveEvent(-1, 2500, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Hey, you ok?", true),
                new Cutscene.DialogueTextEvent("Yeah, I think I'm fine."),
                new Cutscene.DialogueTextEvent("Where are we?"),
                new Cutscene.DialogueTextEvent("Not sure...", true),
            }),
            new Cutscene.AnimationEvent(gameObject, "Wonder", false),
            new Cutscene.DelayEvent(2000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("I'm certain we got to the island, I'm just not sure where..."),
            }),

            new Cutscene.CameraMoveEvent(GeorgeTransform),
            new Cutscene.CameraZoomEvent(6),
            new Cutscene.DelayEvent(1000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("We're northwest of the main island on a small island, I saw when we we're going down"),
                new Cutscene.DialogueTextEvent("There's a bridge just south of here, but it didn't look very well kept"),
            }),

            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(MoveTransform),
            new Cutscene.CameraZoomEvent(4),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Well, if this is true, then we're quite far from site A..."),
                new Cutscene.DialogueTextEvent("In George's condition he isn't going anywhere, but we can't just leave him..."),
                new Cutscene.DialogueTextEvent("As a trained nurse, I'm afraid I have to stay here and tend to his wounds. This leaves only you with the option to go get help"),
                new Cutscene.DialogueTextEvent("Well, It's quite a long way, I'm not sure how to get there...", true),
                new Cutscene.DialogueTextEvent("Here, take this map of the island"),
            }),

            new Cutscene.AnimationEvent(gameObject, "HandMap", false),
            new Cutscene.DelayEvent(1050),
            new Cutscene.AnimationEvent(gameObject, "ReturnHand", false),
            new Cutscene.AnimationEvent(Player.gameObject, "GrabMap", true),
            new Cutscene.DelayEvent(2000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("I hope it can help you"),
                new Cutscene.DialogueTextEvent("The terrain is terrible, there's no way I can get there though!", true),
                new Cutscene.DialogueTextEvent("You do have that grapple hook in your bag. Didn't test it, but it should work?"),
            }),

            new Cutscene.AnimationEvent(Player.gameObject, "UnlockGrapple", true),
            new Cutscene.CustomFunctionEvent(Player.GetComponentInChildren<Grapple>().EnableGrapple),
            new Cutscene.DelayEvent(3000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Well then, seems I have all I need", true),
                new Cutscene.DialogueTextEvent("See you in a bit", true),
                new Cutscene.DialogueTextEvent("Good luck."),
            }),
            new Cutscene.PlayerMoveEvent(1, 2000, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(GeorgeTransform),
            new Cutscene.PlayerMoveEvent(-0.1f, 10, true),
             new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Good luck out there! Send help back to us!"),
            }),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(0.1f, 10, true),
            new Cutscene.CameraMoveEvent(Player),
            new Cutscene.CameraZoomEvent(0)
        };
    }
}
