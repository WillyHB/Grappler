using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider EnvironmentVolume;
    public Slider PlayerVolume;
    public Slider TargetFPS;

    public Toggle Vsync;
    public Toggle Fullscreen;
    public Toggle CameraShake;
    public Toggle SkipAllCutscenes;

    public void SetAudio(string mixerGroup) 
    {
        float value = Mathf.Log10(mixerGroup switch 
        {
            "Master" => MasterVolume.value,
            "Music" => MusicVolume.value,
            "Environment" => EnvironmentVolume.value,
            "Player" => PlayerVolume.value,
            _ => throw new System.Exception("Unidentified mixer group used"),
        }) * 20;

        AudioMaster.Instance.SetLevel(mixerGroup, value);

        SaveObject so = GameData.Load();
        so.volume = value;
        GameData.Save(so);
    }

    public void ToggleVsync(bool tog) 
    {
        Vsync.isOn = tog;
        SaveObject so = GameData.Load();

        Application.targetFrameRate = tog ? -1 : (so.fps >= 210 ? -1 : so.fps);
        QualitySettings.vSyncCount = tog ? 1 : 0;

        so.vsync = tog;
        GameData.Save(so);
    }

    public void ToggleCamShake(bool tog) 
    {
        CameraShake.isOn = tog;
        SaveObject so = GameData.Load();
        so.camShake = tog;
        GameData.Save(so);
    }
    
    public void ToggleSkipCutscenes(bool tog) 
    {
        SkipAllCutscenes.isOn = tog;
        SaveObject so = GameData.Load();
        so.skipAllCutscenes = tog;
        GameData.Save(so);
    }

    public void ToggleFullscreen(bool tog) 
    {
        Fullscreen.isOn = tog;
        SaveObject so = GameData.Load();
        Screen.fullScreen = tog;
        so.isFullscreen = tog;
        GameData.Save(so);
    }

    public void SetFramerate(float val) 
    {
        int value = (int)val;

        SaveObject so = GameData.Load();
        if (so.vsync) 
        {
             TargetFPS.value = so.fps / 10;
             return;
        }

        value *= 10;
        so.fps = value;
        GameData.Save(so);

        if (value == 210) value = -1;

        Application.targetFrameRate = value;
        TargetFPS.GetComponentInChildren<TextMeshProUGUI>().text = value == -1 ? "INF" : value.ToString();
    }
   
    void Awake() 
    {
        SaveObject so = GameData.Load();
        MasterVolume.value = Mathf.Pow(10, so.volume/20); //BUT IF
        MasterVolume.onValueChanged.Invoke(0);
        MusicVolume.value = Mathf.Pow(10, so.musicVolume/20);
        MusicVolume.onValueChanged.Invoke(0);
        EnvironmentVolume.value = Mathf.Pow(10, so.environmentVolume/20);
        EnvironmentVolume.onValueChanged.Invoke(0);
        PlayerVolume.value = Mathf.Pow(10,so.playerVolume/20);
        PlayerVolume.onValueChanged.Invoke(0);
        TargetFPS.value = so.fps / 10;
        ToggleVsync(so.vsync);
        ToggleCamShake(so.camShake);
        ToggleFullscreen(so.isFullscreen);
        ToggleSkipCutscenes(so.skipAllCutscenes);
    }
}
