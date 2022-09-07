using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class OnEnterAnimation : MonoBehaviour
{
    public AnimationClip Clip;

    public bool DestroyOnComplete;

    private AnimationClipPlayable clipPlayable;

    private PlayableGraph playableGraph;

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        clipPlayable = AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), Clip, out playableGraph);
    }

    public void Stop()
    {
        playableGraph.Destroy();
        clipPlayable.Destroy();

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Stop();
    }

    public void Update()
    {
        if (playableGraph.IsValid())
        {
            if (clipPlayable.GetTime() >= Clip.length && DestroyOnComplete)
            {
                Stop();
            }
        }
    }
}
