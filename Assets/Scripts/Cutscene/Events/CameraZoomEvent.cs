using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Events/CameraZoom")]
public class CameraZoomEvent : CutsceneEvent
{
    public CameraZoomEvent(float zoom) => Zoom = zoom;
    public float Zoom;
}
