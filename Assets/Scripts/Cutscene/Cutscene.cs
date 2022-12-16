using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEditor;


[CreateAssetMenu(menuName = "Cutscene")]
public class Cutscene : ScriptableObject
{

    public bool LockPlayerMovement { get; set; } = false;

    /*
    public PlayerMoveEvent EventAddPlayerMove(float direction, int duration)
    {
        Events.Add(new PlayerMoveEvent(direction, duration));

        return (PlayerMoveEvent)Events[^1];
    }

    public DelayEvent EventAddDelay(int milliseconds)
    {
        Events.Add(new DelayEvent(milliseconds));

        return (DelayEvent)Events[^1];
    }

    public CameraMoveEvent EventAddCameraMove(Vector2 position)
    {
        Events.Add(new CameraMoveEvent(position));

        return (CameraMoveEvent)Events[^1];
    }

    public CameraZoomEvent EventAddCameraZoom(float zoom)
    {
        Events.Add(new CameraZoomEvent(zoom));

        return (CameraZoomEvent)Events[^1];
    }*/

}

