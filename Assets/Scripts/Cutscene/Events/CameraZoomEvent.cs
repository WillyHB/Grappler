using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Cutscene
{
    public class CameraZoomEvent : CutsceneEvent
    {
        public CameraZoomEvent(float zoom) => Zoom = zoom;
        public float Zoom;

        public override async Task HandleEvent(CutsceneSystem system)
        {
            system.CamEventChannel.PerformZoom(Zoom);
        }
    }

}