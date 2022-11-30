using UnityEngine.Audio;
using UnityEngine;

public class Audio : ScriptableObject
{
    public enum MixerGroup
    {
        Master = 0,
        Environment,
        Music,
        Player
    }

    public AudioClip AudioClip;

    public float Volume;
    public bool Loop;
    [Range(-1, 1)]
    public float Pan;
    public MixerGroup AudioMixerGroup;
}
