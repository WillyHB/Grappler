using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Cutscene
{
    public class CameraMoveEvent : CutsceneEvent
    {
        public CameraMoveEvent(Transform follow) => Follow = follow;
        public Transform Follow;

        public override async Task HandleEvent(CutsceneSystem system)
        {
            system.CamEventChannel.SetFollow(Follow);
        }
    }
}