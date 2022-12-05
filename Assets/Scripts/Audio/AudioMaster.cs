using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMaster : MonoBehaviour
{
    public List<AudioEventChannel> eventChannels;

    public AudioMixer AudioMixer;

    private readonly List<PlayingClip> PlayingClips = new();

    public AudioMixerGroup PlayerMixerGroup;
    public AudioMixerGroup MasterMixerGroup;
    public AudioMixerGroup MusicMixerGroup;
    public AudioMixerGroup EnvironmentMixerGroup;

    [System.Serializable]
    public struct PlayingClip
    {
        public PlayingClip(Audio clip, AudioSource source)
        {
            Clip = clip;
            Source = source;
        }

        public Audio Clip;
        public AudioSource Source;
    }

    private void Start()
    {
        eventChannels.ForEach(ch => ch.Played += Play);
        eventChannels.ForEach(ch => ch.Stopped += Stop);
        eventChannels.ForEach(ch => ch.StoppedSpecific += Stop);
    }

    private void OnDisable()
    {
        eventChannels.ForEach(ch => ch.Played -= Play);
        eventChannels.ForEach(ch => ch.Stopped -= Stop);
        eventChannels.ForEach(ch => ch.StoppedSpecific -= Stop);
    }

    private void Update()
    {

        List<PlayingClip> toDelete = new();

        foreach (var pClip in PlayingClips)
        {
            if (!pClip.Source.isPlaying && Application.isFocused)
            {
                toDelete.Add(pClip);
            }
        }

        while (toDelete.Count > 0)
        {
            Stop(toDelete[0]);
            toDelete.RemoveAt(0);
        }
    }

    public void Stop(Audio? clip = null)
    {
        foreach (var pClip in PlayingClips)
        {
            if (pClip.Clip == clip || clip == null)
            {
                Stop(pClip);
            }
        }
    }

    public void Stop(PlayingClip clip)
    {
        PlayingClips.Remove(clip);

        clip.Source.Stop();
        Destroy(clip.Source);
    }

    public PlayingClip Play(Audio clip, MixerGroup mixerGroup)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.volume = clip.Volume;
        source.clip = clip.AudioClip;
        source.loop = clip.Loop;
        source.panStereo = clip.Pan;

        source.outputAudioMixerGroup = mixerGroup switch
        {
            MixerGroup.Player => PlayerMixerGroup,
            MixerGroup.Environment => EnvironmentMixerGroup,
            MixerGroup.Master => MasterMixerGroup,
            MixerGroup.Music => MusicMixerGroup,
            _ => null
        };

        source.Play();

        PlayingClip pClip = new(clip, source);
        PlayingClips.Add(pClip);

        return PlayingClips[^1];
    }
}
