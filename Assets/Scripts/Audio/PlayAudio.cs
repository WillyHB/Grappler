using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioEventChannel EventChannel;
    public Audio Clip;
    public bool PlayOnStart;

    public void Start() 
    {
       if (PlayOnStart) Play();
    }

    public void Play() 
    {
        if (Clip == null || EventChannel == null) 
        {

            Debug.LogError("Clip or EventChannel is not assigned");
            return;
        }

        EventChannel.Play(Clip);
    }

}