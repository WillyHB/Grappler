using UnityEngine;

[System.Serializable]
public class SaveObject
{
    public string WARNING = "EDITING THIS FILE COULD CAUSE CORRUPTION!!! maybe";

    public bool skipStartCutscene = false;
    public bool skipTribesmanCutscene = false;
    public int checkpoint = 0;

    //SETTINGS
    public float volume;
    public float musicVolume;
    public float playerVolume;
    public float environmentVolume;
    public float enemiesVolume;
    public bool isFullscreen = false;
    public int fps = 60;
    public bool camShake = true;
    public bool vsync = true;
    public int dialogueTextSpeed = 80;
    public bool skipAllCutscenes = false;
}
