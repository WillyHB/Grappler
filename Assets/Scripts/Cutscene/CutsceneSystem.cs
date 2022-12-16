using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class CutsceneSystem : MonoBehaviour
{
    public CutsceneEvent BaseEvent;

    public CameraEventChannel CamEventChannel;
    public CutsceneSystemStateHandler stateHandler;

    public async Task Wait(int milliseconds)
    {
        await Task.Delay(milliseconds);
    }

}
public class CutsceneEvent : ScriptableObject
{
    public CutsceneEvent NextEvent;

}