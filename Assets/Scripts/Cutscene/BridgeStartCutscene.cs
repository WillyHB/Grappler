using System.Collections;
using System.Collections.Generic;
using Cutscene;
using UnityEngine;
using UnityEngine.UI;

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
        if (GameData.Load().skipStartCutscene) 
        {
            Destroy(this);
            return new List<CutsceneEvent>();
        }

        return new List<CutsceneEvent>()
        {
            new CustomFunctionEvent(Player.GetComponentInChildren<Grapple>().DisableGrapple),
            new DelayEvent(500),
            new Cutscene.AnimationEvent(Player, "Intro1", true),
            new DelayEvent(1000),
            new CameraMoveEvent(MoveTransform),
            new CameraZoomEvent(4),
            new DelayEvent(5000),
            new Cutscene.AnimationEvent(Player, "Intro2", true),
            new DelayEvent(4000),
            new PlayerMoveEvent(-1, 2500, true),
            new Cutscene.AnimationEvent(Jessica, "Intro", false),
            new DelayEvent(2000),
            new Cutscene.AnimationEvent(Jessica, "Walk", false),
            new MoveEvent(Jessica, new Vector2(3, 0), 2500),
            new Cutscene.AnimationEvent(Jessica, "Idle", false),
            new Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new DialogueTextEvent("Hey, you ok?", PlayerPotrait.Shocked, true),
                new DialogueTextEvent("Yeah, I think I'm fine.", JessicaPotrait.Poker),
                new DialogueTextEvent("Where are we?", JessicaPotrait.Confused),
            }),
            new Cutscene.AnimationEvent(Jessica, "Wonder", false),
            new DelayEvent(500),
            new AudioEvent(EnvironmentEventChannel, HeadScratch),
            new DelayEvent(1500),
            new Dialogue(DialogueSystemPrefab,new List<DialogueEvent>()
            {
                new DialogueTextEvent("I'm certain we got to the island, I'm just not sure where...", JessicaPotrait.Confused),
            }),

            new CameraMoveEvent(GeorgeTransform),
            new CameraZoomEvent(6),
            new DelayEvent(1000),
            new Dialogue(DialogueSystemPrefab,new List<DialogueEvent>()
            {
                new DialogueTextEvent("We're on one of the outer islands, I saw it when we were going down", GeorgePotrait),
                new DialogueTextEvent("I saw it was connected by a bridge as far as I could tell", GeorgePotrait),
            }),

            new DelayEvent(1000),
            new CameraMoveEvent(MoveTransform),
            new CameraZoomEvent(4),
            new Dialogue(DialogueSystemPrefab,new List<DialogueEvent>()
            {
                new DialogueTextEvent("A bridge?!", JessicaPotrait.Shocked),
                new DialogueTextEvent("That wasn't in any of the research documents... Must have been hidden by the foliage?", JessicaPotrait.Confused),
                new DialogueTextEvent("Well, if this is true, then we're quite far from site A...", JessicaPotrait.Shocked),
                new DialogueTextEvent("With no one coming for us...", PlayerPotrait.Fear, true),
                new DialogueTextEvent("In George's condition he isn't going anywhere, but we can't just leave him...", JessicaPotrait.Poker),
                new DialogueTextEvent("It seems one of us has to go get rescue from Site A...", PlayerPotrait.Poker, true),
                new DialogueQuestionEvent("I have to tend to George's wounds, no way I'm leaving him in this state. What do you think about going?", JessicaPotrait.Sad, "Yeah!", "I guess I have to", "If I have to...")
                {
                    Answer1Events = new List<Cutscene.DialogueEvent>()
                    {
                        new DialogueTextEvent("That's great to hear!", JessicaPotrait.Happy),
                    },

                    Answer2Events = new List<DialogueEvent>()
                    {
                        new DialogueTextEvent("Well I could go, but I'm not sure that would do any good for George. I'm sort of stuck here", JessicaPotrait.Sad),
                    },

                    Answer3Events = new List<DialogueEvent>()
                    {
                        new DialogueTextEvent("I know it's a tough journey, but it seems like our only choice right now. Would you rather just stay here? You said it yourself, no ones coming for us", JessicaPotrait.Angry)
                    }
                },
                new DialogueTextEvent("It's quite a far journey, a few days as far as I can tell, here take this map, it should help you.", JessicaPotrait.Poker),
            }),

            new Cutscene.AnimationEvent(Jessica, "HandMap", false),
            new DelayEvent(1050),
            new Cutscene.AnimationEvent(Jessica, "ReturnHand", false),
            new Cutscene.AnimationEvent(Player, "GrabMap", true),
            new DelayEvent(2000),
            new Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new DialogueTextEvent("We got the grappling hooks from HQ, it's in your bag, you might find it useful", JessicaPotrait.Poker),
            }),

            new Cutscene.AnimationEvent(Player.gameObject, "UnlockGrapple", true),
            new DelayEvent(3000),
            new CustomFunctionEvent(Player.GetComponentInChildren<Grapple>().EnableGrapple),
            new Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new DialogueTextEvent("Just press Right Click in order to deploy the grapple, it will deploy onto any surface your mouse is pointing on", JessicaPotrait.Poker),
                new DialogueTextEvent("Shift will retract the grapple line, and ctrl with detract it.", JessicaPotrait.Happy),
                new DialogueTextEvent("Finally, you can use space to jump out of the grapple, or simply right click to detach the grapple hook", JessicaPotrait.Poker),
                new DialogueQuestionEvent("So does that make sense to you? Do you need any re-explaining?", JessicaPotrait.Confused, "No I understand!", "Yes could you re-explain the grapple hook again?") {

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
                new DialogueTextEvent("Well, it doesn't make sense to just stand here, so I'll get to going", PlayerPotrait.Poker, true),
                new DialogueTextEvent("Take care, I'll be back with rescue", PlayerPotrait.Poker, true),
            }),
            new PlayerMoveEvent(1, 2000, true),
            new DelayEvent(1000),
            new CameraMoveEvent(GeorgeTransform),
            new PlayerMoveEvent(-0.1f, 10, true),
             new Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new DialogueTextEvent("Good luck! Be safe, it's a long journey", GeorgePotrait),
                new DialogueTextEvent("We don't know what's out there...", GeorgePotrait),
            }),
            new DelayEvent(1000),
            new PlayerMoveEvent(0.1f, 10, true),
            new CameraMoveEvent(Player.transform),
            new CameraZoomEvent(0),
            new CustomFunctionEvent(()=>
            { 
                SaveObject so = GameData.Load();
                so.skipStartCutscene = true;
                GameData.Save(so);
            }),
            new CustomFunctionEvent(()=>Destroy(this)),
        };
    }
}
