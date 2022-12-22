using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeStartCutscene : CutsceneSystem
{
    public Transform MoveTransform;
    public Transform Player;
    public GameObject DialogueSystemPrefab;

    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
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
                new Cutscene.DialogueTextEvent("Yo bro, whats up baby!"),
                new Cutscene.DialogueQuestionEvent("You need help?", "Yeah", "Nah", "NO FUCK YOU!")
                {
                    Answer1Events = new()
                    {
                        new Cutscene.DialogueTextEvent("Yeah Ok I'll help you man"),
                    },

                    Answer2Events = new()
                    {
                        new Cutscene.DialogueTextEvent("Damn aight, keep your head up man!"),
                    },

                    Answer3Events = new()
                    {
                        new Cutscene.DialogueQuestionEvent("Wtf, Fuck you! Why would you say that?", "Idk thought it'd be funny", "Cause I HATE YOU FUCK YOU!")
                        {
                            Answer1Events = new()
                            {
                                new Cutscene.DialogueTextEvent("Yeah ok it was pretty funny ngl"),
                            }
                        },
                    }
                }
            }),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(1, 2000, true),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(-0.1f, 10, true),
             new Cutscene.Dialogue(DialogueSystemPrefab,new List<Cutscene.DialogueEvent>()
            {
                new Cutscene.DialogueTextEvent("Good luck to you out there! Come back for us!"),
            }),
            new Cutscene.DelayEvent(1000),
            new Cutscene.PlayerMoveEvent(0.1f, 10, true),
            new Cutscene.CameraMoveEvent(Player),
            new Cutscene.CameraZoomEvent(0)
        };
    }
}
