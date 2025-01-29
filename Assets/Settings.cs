using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider EnvironmentVolume;
    public Slider PlayerVolume;
    public Slider TargetFPS;

    public AudioMaster AudioMaster;

    public Toggle Vsync;
    public Toggle Fullscreen;
    public Toggle CameraShake;
    public Toggle SkipAllCutscenes;

    public void SetAudio(string mixerGroup) 
    {
        float value = Mathf.Log10(mixerGroup switch {
            "Master" => MasterVolume.value,
            "Music" => MusicVolume.value,
            "Environment" => EnvironmentVolume.value,
            "Player" => PlayerVolume.value,
            _ => throw new System.Exception("Unidentified mixer group used"),
        }) * 20;

        AudioMaster.SetLevel(mixerGroup, value);
    }

    public void ToggleVsync(bool tog) {
        SaveObject so = GameData.Load();

        Application.targetFrameRate = tog ? -1 : so.fps;
        QualitySettings.vSyncCount = tog ? 1 : 0;

        so.vsync = tog;
        GameData.Save(so);
    }

    public void SetFramerate(float val) {
        int value = (int)val;

        SaveObject so = GameData.Load();
        if (so.vsync) 
        {
             TargetFPS.value = so.fps;
             return;
        }

        value *= 10;
        if (value == 0) value = -1;
        Application.targetFrameRate = value;

        so.fps = value;
        GameData.Save(so);

    }
   
    void Start()
    {
        SaveObject so = GameData.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
