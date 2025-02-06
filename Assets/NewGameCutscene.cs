using System.Collections;
using System.Collections.Generic;
using Cutscene;
using TMPro;
using UnityEngine;

public class NewGameCutscene : CutsceneSystem
{
    public Audio PlaneFlying, Crash;
    public AudioEventChannel MasterChannel;
    public TextMeshProUGUI GameTitle;
    public TextMeshProUGUI ByLine;

    public override List<CutsceneEvent> GenerateCutscene()
    {
        return new List<CutsceneEvent>()
        {
            new DelayEvent(1000),
            new AudioEvent(MasterChannel, PlaneFlying),
            new DelayEvent(2000),
            new CustomFunctionEvent(()=>LeanTween.alphaText(ByLine.rectTransform, 1, 3)),
            new DelayEvent(11000),
            new CustomFunctionEvent(()=>LeanTween.alphaText(GameTitle.rectTransform, 1, 2)),
            new DelayEvent(2000),
            new AudioEvent(MasterChannel, Crash),
            new DelayEvent(4000),
        };
    }
}
