using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanimator : MonoBehaviour
{
    public Wanimation CurrentAnimation { get; private set; }

    public bool IsPlaying { get; private set; }

    private int currentFrame;

    private float time;

    public float NormalizedTime => currentFrame / (float)CurrentAnimation.Frames.Length;
    public void Play(Wanimation anim)
    {
        CurrentAnimation = anim;
        IsPlaying = true;
    }

    public void Stop()
    {
        CurrentAnimation = null;
        IsPlaying = false;
    }

    public void Pause()
    {
        IsPlaying = false;
    }

    public void Update()
    {
        time += Time.deltaTime;

        if (time > CurrentAnimation.FrameLength)
        {
            time = 0;
            currentFrame++;

            if (currentFrame > CurrentAnimation.Frames.Length - 1)
            {
                currentFrame = 0;
            }
        }

        GetComponent<SpriteRenderer>().sprite = CurrentAnimation.Frames[currentFrame];
    }
}
