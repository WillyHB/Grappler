using System.Collections;
using System.Collections.Generic;
using Cutscene;
using UnityEngine;

public class BridgeStartCutscene : CutsceneSystem
{
    public Transform MoveTransform;
    public Transform GeorgeTransform;
    public GameObject DialogueSystemPrefab;

    public Potrait PlayerPotrait;
    public Potrait JessicaPotrait;
    public Sprite GeorgePotrait;

    public GameObject Player;
    public GameObject Jessica;

    public AudioEventChannel EnvironmentEventChannel;
    public Audio HeadScratch;

    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new Cutscene.CustomFunctionEvent(Player.GetComponentInChildren<Grapple>().DisableGrapple),
            new Cutscene.DelayEvent(1000),
            new Cutscene.AnimationEvent(Player, "Intro1", true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(MoveTransform),
            new Cutscene.CameraZoomEvent(4),
            new Cutscene.DelayEvent(5000),
            new Cutscene.AnimationEvent(Player, "Intro2", true),
            new Cutscene.DelayEvent(4000),
            new Cutscene.PlayerMoveEvent(-1, 2500, true),
            new Cutscene.AnimationEvent(Jessica, "Intro", false),
            new Cutscene.DelayEvent(2000),
            new Cutscene.AnimationEvent(Jessica, "Walk", false),
            new Cutscene.MoveEvent(Jessica, new Vector2(3, 0), 2500),
            new Cutscene.AnimationEvent(Jessica, "Idle", false),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Hey, you ok?", PlayerPotrait.Shocked, true),
                new Cutscene.DialogueTextEvent("Yeah, I think I'm fine.", JessicaPotrait.Poker),
                new Cutscene.DialogueTextEvent("Where are we?", JessicaPotrait.Confused),
            }),
            new Cutscene.AnimationEvent(Jessica, "Wonder", false),
            new Cutscene.DelayEvent(500),
            new Cutscene.AudioEvent(EnvironmentEventChannel, HeadScratch),
            new Cutscene.DelayEvent(1500),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("I'm certain we got to the island, I'm just not sure where...", JessicaPotrait.Confused),
            }),

            new Cutscene.CameraMoveEvent(GeorgeTransform),
            new Cutscene.CameraZoomEvent(6),
            new Cutscene.DelayEvent(1000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("We're northwest of the main island, I saw when we were going down", GeorgePotrait),
                new Cutscene.DialogueTextEvent("There's a bridge of some sort just south of here?", GeorgePotrait),
            }),

            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(MoveTransform),
            new Cutscene.CameraZoomEvent(4),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("A bridge?!", JessicaPotrait.Shocked),
                new Cutscene.DialogueTextEvent("That wasn't in any of the research documents... Must have been hidden by the foliage?", JessicaPotrait.Confused),
                new Cutscene.DialogueTextEvent("Well, if this is true, then we're quite far from site A...", JessicaPotrait.Shocked),
                new Cutscene.DialogueTextEvent("On the very island we came to study for danger...", PlayerPotrait.Fear, true),
                new Cutscene.DialogueTextEvent("In George's condition he isn't going anywhere, but we can't just leave him...", JessicaPotrait.Poker),
                new Cutscene.DialogueTextEvent("I guess no ones coming to save us here, so it seems one of us has to go", PlayerPotrait.Poker, true),
                new Cutscene.DialogueQuestionEvent("I have to tend to George's wounds, no way I'm leaving him", JessicaPotrait.Sad, "Yeah!", "I guess I have to", "If I have to...")
                {
                    Answer1Events = new List<Cutscene.DialogueEvent>()
                    {
                        new Cutscene.DialogueTextEvent("That's great to hear!", JessicaPotrait.Happy),
                    },

                    Answer2Events = new List<Cutscene.DialogueEvent>()
                    {
                        new Cutscene.DialogueTextEvent("Well I could go, but I'm not sure that would do any good for George. I'm sort of stuck here", JessicaPotrait.Sad),
                    },

                    Answer3Events = new List<Cutscene.DialogueEvent>()
                    {
                        new Cutscene.DialogueTextEvent("I know it's a tough journey, but it seems like our only choice right now. Would you rather just stay here? You said it yourself, no ones coming for us", JessicaPotrait.Angry)
                    }
                },
                new Cutscene.DialogueTextEvent("It's quite a far journey, here take this map, it should help you.", JessicaPotrait.Poker),
            }),

            new Cutscene.AnimationEvent(Jessica, "HandMap", false),
            new Cutscene.DelayEvent(1050),
            new Cutscene.AnimationEvent(Jessica, "ReturnHand", false),
            new Cutscene.AnimationEvent(Player, "GrabMap", true),
            new Cutscene.DelayEvent(2000),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("I'm worried about who or what is on this island. We came here to research them but it seems they're far more advanced than we thought they were...", JessicaPotrait.Fear),
                new Cutscene.DialogueTextEvent("We got the grappling hooks from HQ, it's in your bag, you might find it useful", JessicaPotrait.Poker),
            }),

            new Cutscene.AnimationEvent(Player.gameObject, "UnlockGrapple", true),
            new Cutscene.DelayEvent(3000),
            new Cutscene.CustomFunctionEvent(Player.GetComponentInChildren<Grapple>().EnableGrapple),
            new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Just press Right Click in order to deploy the grapple, it will deploy onto any surface your mouse is pointing on", JessicaPotrait.Poker),
                new Cutscene.DialogueTextEvent("Shift will retract the grapple line, and ctrl with detract it.", JessicaPotrait.Happy),
                new Cutscene.DialogueTextEvent("Finally, you can use space to jump out of the grapple, or simply right click to detach the grapple hook", JessicaPotrait.Poker),
                new Cutscene.DialogueQuestionEvent("So does that make sense to you? Do you need any re-explaining?", JessicaPotrait.Confused, "No I understand!", "Yes could you re-explain the grapple hook again?") {

                    Answer1Events = new List<DialogueEvent>() { new DialogueTextEvent("Perfect, I'm sure you'll get used to it quick!", JessicaPotrait.Happy) },
                    Answer2Events = new List<DialogueEvent>() 
                    {
                        new DialogueTextEvent("Okay, listen close, I'll only explain once again", JessicaPotrait.CloseEyes) ,
                        new DialogueTextEvent("Just press Right Click in order to deploy the grapple, it will deploy onto any surface your mouse is pointing on", JessicaPotrait.Poker),
                        new DialogueTextEvent("Shift will retract the grapple line, and ctrl with detract it.", JessicaPotrait.Happy),
                        new DialogueTextEvent("Finally, you can use space to jump out of the grapple, or simply right click to detach the grapple hook", JessicaPotrait.Poker),
                        new DialogueTextEvent("Don't worry too much about it, You'll master it quickly I'm sure", JessicaPotrait.Happy),
                    },
                },
                new Cutscene.DialogueTextEvent("Well, it doesn't make sense to just stand here, so I'll get to going", PlayerPotrait.Poker, true),
                new Cutscene.DialogueTextEvent("Take care, I'll be back with rescue", PlayerPotrait.Poker, true),
            }),
            new Cutscene.PlayerMoveEvent(1, 2000, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.CameraMoveEvent(GeorgeTransform),
            new Cutscene.PlayerMoveEvent(-0.1f, 10, true),
             new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Good luck! Be safe, it's a long journey", GeorgePotrait),
                new Cutscene.DialogueTextEvent("We don't know what's out there...", GeorgePotrait),
            }),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(0.1f, 10, true),
            new Cutscene.CameraMoveEvent(Player.transform),
            new Cutscene.CameraZoomEvent(0),
            new Cutscene.CustomFunctionEvent(()=>Destroy(this)),
        };
    }
}
