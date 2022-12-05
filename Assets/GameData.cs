using UnityEngine;
using System.IO;
using Newtonsoft.Json;


[System.Serializable]
public static class GameData
{
    public static string FileDirectory { get; private set; }
    public static string FileName { get; private set; }


    private static SaveObject cachedSO;

    public static void Save(SaveObject so)
    {
        cachedSO = null;

        SetPaths();

        string dir = Application.persistentDataPath + FileDirectory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        if (!File.Exists(dir))
        {
            File.Create(dir + FileName).Close();
        }

        string json = JsonConvert.SerializeObject(so, Formatting.Indented);
        File.WriteAllText(dir + FileName, json);

    }

    public static SaveObject Load()
    {
        if (cachedSO != null)
        {
            return cachedSO;
        }

        SetPaths();

        string fullPath = Application.persistentDataPath + FileDirectory + FileName;

        SaveObject so = new();

        if (!File.Exists(fullPath))
        {
            Save(so);
        }

        string json = File.ReadAllText(fullPath);
        so = JsonConvert.DeserializeObject<SaveObject>(json);

        cachedSO = so;

        return so;
    }

    private static void SetPaths()
    {
        FileDirectory = $"/Grappler/";
        FileName = $"GrapplerSaveData.txt";
    }
}



