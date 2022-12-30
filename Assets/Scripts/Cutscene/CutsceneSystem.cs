using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class CutsceneSystem : MonoBehaviour
{
    private List<CutsceneEvent> events { get; set; } = new();

    public CameraEventChannel CamEventChannel;
    public CutsceneSystemStateHandler stateHandler;

    public abstract List<CutsceneEvent> GenerateCutscene();

    public bool RunOnStart;
    public bool RunOnTriggerEnter;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (RunOnTriggerEnter)
            {
                Play();
            }
        }
    }

    public void Awake()
    {
        stateHandler.IsInCutscene = false;    
    }

    public void Start()
    {
        events = GenerateCutscene();
        if (RunOnStart) Play();
    }

    public async void Play()
    {
        stateHandler.IsInCutscene = true;
        playing = true;

        foreach (var e in events)
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