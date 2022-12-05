using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName ="AudioClip")]
public class Audio : ScriptableObject
{
    public AudioClip AudioClip;

    [Range(0, 1)]
    public float Volume;

    public bool Loop;
    [Range(-1, 1)]
    public float Pan;
}

public enum MixerGroup
{
    Master = 0,
    Environment,
    Music,
    Player
}
