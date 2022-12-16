using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Events/CameraMove")]
public class CameraMoveEvent : CutsceneEvent
{
    public CameraMoveEvent(Vector2 position) => Position = position;
    public Vector2 Position;
}