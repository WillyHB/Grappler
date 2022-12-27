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
        /*
         * FIX INTRO :)
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
            new Cutscene.DelayEvent(3000),
            new Cutscene.AnimationEvent(gameObject, "Intro", false),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(-1, 2500, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Hey, you ok?", true),
                new Cutscene.DialogueTextEvent("Yeah, I think I'm fine."),
                new Cutscene.DialogueTextEvent("Where are we?"),
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
                new Cutscene.DialogueTextEvent("We're northwest of the main island, I saw when we were going down"),
                new Cutscene.DialogueTextEvent("There's a bridge just south of here, but it didn't look very stable"),
            }),

            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(MoveTransform),
            new Cutscene.CameraZoomEvent(4),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Well, if this is true, then we're quite far from site A..."),
                new Cutscene.DialogueTextEvent("In George's condition he isn't going anywhere, but we can't just leave him..."),
                new Cutscene.DialogueTextEvent("I guess no ones coming to save us here, so it seems one of us has to go", true),
                new Cutscene.DialogueTextEvent("Since you're a nurse, I guess it only makes sense for it to be me", true),
                new Cutscene.DialogueTextEvent("It's quite a far journey, here take this map, it should help you")
            }),

            new Cutscene.AnimationEvent(gameObject, "HandMap", false),
            new Cutscene.DelayEvent(1050),
            new Cutscene.AnimationEvent(gameObject, "ReturnHand", false),
            new Cutscene.AnimationEvent(Player.gameObject, "GrabMap", true),
            new Cutscene.DelayEvent(2000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Also, we we're given a grapple hook each, perhaps it can be of use to you, it's it your bag."),
            }),

            new Cutscene.AnimationEvent(Player.gameObject, "UnlockGrapple", true),
            new Cutscene.DelayEvent(3000),
            new Cutscene.CustomFunctionEvent(Player.GetComponentInChildren<Grapple>().EnableGrapple),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Well, it doesn't make sense to just stand here, so I'll get to going", true),
                new Cutscene.DialogueTextEvent("Take care, I'll be back with rescue", true),
            }),
            new Cutscene.PlayerMoveEvent(1, 2000, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(GeorgeTransform),
            new Cutscene.PlayerMoveEvent(-0.1f, 10, true),
             new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Good luck!"),
            }),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(0.1f, 10, true),
            new Cutscene.CameraMoveEvent(Player),
            new Cutscene.CameraZoomEvent(0)
        };
    }
}
