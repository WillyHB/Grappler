using UnityEngine;

[System.Serializable]
public class SaveObject
{
    public string WARNING = "EDITING THIS FILE COULD CAUSE CORRUPTION!!!";

    public bool isFullscreen = true;

    public int resolutionIndex = 14;

    public float volume;
    public float musicVolume;
    public float playerVolume;
    public float environmentVolume;
    public float enemiesVolume;

    public int fps = 60;
    public bool camShake = true;
    public bool vsync = true;

    public int Checkpoint = 0;


}
