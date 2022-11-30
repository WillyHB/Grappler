using System;
using UnityEngine;

public class AudioEventChannel : ScriptableObject
{

    public delegate AudioMaster.PlayingClip PlayDelegate(Audio clip);

    public event PlayDelegate Played;


    public event Action<Audio?> Stopped;
    public event Action<AudioMaster.PlayingClip> StoppedSpecific;


    /// <summary>
    /// Plays an audio clip, returns null if no AudioMaster is found in the scene or the event channel is not connected to one.
    /// </summary>
    /// <param name="clip"></param>
    /// <returns></returns>
    public AudioMaster.PlayingClip? Play(Audio clip) => Played?.Invoke(clip);

    public void Stop(AudioMaster.PlayingClip playingClip) => StoppedSpecific?.Invoke(playingClip);

    public void Stop(Audio clip) => Stopped?.Invoke(clip);

    public void Stop() => Stopped?.Invoke(null);
}
