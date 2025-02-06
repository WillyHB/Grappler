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

    private void OnEnable() 
    {
        eventChannels.ForEach(ch => ch.Played += Play);
        eventChannels.ForEach(ch => ch.Stopped += Stop);
        eventChannels.ForEach(ch => ch.StoppedSpecific += Stop);
        eventChannels.ForEach(ch => ch.LevelSet += SetLevel);
    }

    private void OnDisable()
    {
        eventChannels.ForEach(ch => ch.Played -= Play);
        eventChannels.ForEach(ch => ch.Stopped -= Stop);
        eventChannels.ForEach(ch => ch.StoppedSpecific -= Stop);
        eventChannels.ForEach(ch => ch.LevelSet -= SetLevel);
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

    public void Stop(Audio clip = null)
    {
        for (int i = 0; i < PlayingClips.Count; i++) {

            if (PlayingClips[i].Clip == clip || clip == null)
            {
                Stop(PlayingClips[i]);
            }
        }
    }

    public void Stop(PlayingClip clip)
    {
        if (!PlayingClips.Contains(clip)) return;

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

    public void SetLevel(float level, MixerGroup group) {
        SaveObject so = GameData.Load();

        switch (group) {

            case MixerGroup.Master:
            so.volume = level;
            break;

            case MixerGroup.Environment:
            so.environmentVolume = level;
            break;

            case MixerGroup.Music:
            so.musicVolume = level;
            break;

            case MixerGroup.Player:
            so.playerVolume = level;
            break;

        }

        AudioMixer.SetFloat(Audio.MixerGroupToString(group), level);
    }
}
