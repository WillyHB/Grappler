using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class CutsceneSystem : MonoBehaviour
{
    public List<CutsceneEvent> Events { get; set; } = new();

    public CameraEventChannel CamEventChannel;
    public CutsceneSystemStateHandler stateHandler;

    public abstract void GenerateCutscene();

    public void Start()
    {
        stateHandler.IsInCutscene = true;
        GenerateCutscene();
        Play();
    }

    public async void Play()
    {
        playing = true;

        foreach (var e in Events)
        {
            await e.HandleEvent(this);

            if (!playing) return;
        }

        Stop();
    }

    private bool playing;

    public void Stop()
    {
        stateHandler.IsInCutscene = false;
        playing = false;
    }

    public async Task Wait(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }

}
public abstract class CutsceneEvent
{
    public CutsceneEvent NextEvent;

    public abstract Task HandleEvent(CutsceneSystem system);

}