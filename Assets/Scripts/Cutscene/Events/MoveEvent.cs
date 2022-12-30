using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Cutscene
{
    public class MoveEvent : CutsceneEvent
    {
        GameObject go;
        Vector2 distance;
        int speed;
        int time;
        public MoveEvent(GameObject go, Vector2 distance, int time)
        {
            this.go = go;
            this.distance = distance;
            this.time = time;
        }

        public override async Task HandleEvent(CutsceneSystem system)
        {
            Vector2 pos = go.transform.position;

            Vector2 endPos = (Vector2)go.transform.position + distance;

            go.LeanMove(endPos, time/1000).setOnComplete(() => stopWaiting = true);

            while (true)
            {
                await Task.Yield();

                if (stopWaiting) break;
            }
        }

        bool stopWaiting;
    }

}