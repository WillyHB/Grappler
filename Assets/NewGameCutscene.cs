using System.Collections;
using System.Collections.Generic;
using Cutscene;
using TMPro;
using UnityEngine;

public class NewGameCutscene : CutsceneSystem
{
    public Audio PlaneFlying, Crash;
    public AudioEventChannel MasterChannel;
    public CanvasGroup GameTitle;
    public CanvasGroup ByLine;

    public void Start() 
    {
        base.Start();
        if (GameData.Load().skipAllCutscenes) LevelTransition.LoadSync(2);
    }
    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new CustomFunctionEvent(()=>LevelTransition.StageLoad(2)),
            new AudioEvent(MasterChannel, PlaneFlying),
            new DelayEvent(3000),
            new CustomFunctionEvent(()=>LeanTween.alphaCanvas(ByLine, 1, 4).setEaseInExpo()),
            new DelayEvent(5000),
            new CustomFunctionEvent(()=>LeanTween.alphaCanvas(GameTitle, 1, 2).setEaseInExpo()),
            new AudioEvent(MasterChannel, Crash),
            new DelayEvent(6000),
            new CustomFunctionEvent(()=> {
                LeanTween.alphaCanvas(ByLine, 0, 4).setEaseOutExpo();
                LeanTween.alphaCanvas(GameTitle, 0, 4).setEaseOutExpo();
            }),
            new DelayEvent(4000),
            new CustomFunctionEvent(()=>LevelTransition.ActivateLoad()),
        };
    }
}
