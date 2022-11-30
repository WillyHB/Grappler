using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMaster : MonoBehaviour
{
    public AudioEventChannel eventChannel;

    private AudioSource audioSource;

    public AudioMixer AudioMixer;

    public List<AudioMixerG> AudioMixerGroups = new();

    public List<PlayingClip> PlayingClips;

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

    [System.Serializable]
    public struct AudioMixerG
    {
        public Audio.MixerGroup MixerGroup;
        public AudioMixerGroup AudioMixerGroup;
    }

    private void Start()
    {
        eventChannel.Played += Play;
        eventChannel.Stopped += Stop;
        eventChannel.StoppedSpecific += Stop;
        audioSource = GetComponent<AudioSource>();
    }

    public void Stop(Audio? clip)
    {
        if (clip != null)
        {
            foreach (var pClip in PlayingClips)
            {
                if (pClip.Clip == clip)
                {
                    Stop(pClip);
                }
            }
        }

        else
        {
            foreach (var pClip in PlayingClips)
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

    public PlayingClip Play(Audio clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.volume = clip.Volume;
        source.clip = clip.AudioClip;
        source.loop = clip.Loop;



        source.panStereo = clip.Pan;

        source.Play();

        PlayingClip pClip = new(clip, source);
        PlayingClips.Add(pClip);

        return PlayingClips[^1];
    }
}
