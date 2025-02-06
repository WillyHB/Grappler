using UnityEngine.Audio;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="AudioClip")]
public class Audio : ScriptableObject
{
    public AudioClip AudioClip;

    [Range(0, 1)]
    public float Volume;

    public bool Loop;
    [Range(-1, 1)]
    public float Pan;

    public static string MixerGroupToString(MixerGroup group) => group switch 
    {
        MixerGroup.Master => "Master",
        MixerGroup.Environment => "Environment",
        MixerGroup.Music => "Music",
        MixerGroup.Player => "Player",
        _ => throw new System.Exception("Mixer Group does not exist"),
    };
    
}

[SerializeField][Serializable]
public enum MixerGroup
{
    Master = 0,
    Environment,
    Music,
    Player
}
