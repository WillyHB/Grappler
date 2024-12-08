using UnityEngine;
using System.IO;

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

        string json = JsonUtility.ToJson(so);
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
        so = JsonUtility.FromJson<SaveObject>(json);

        cachedSO = so;

        return so;
    }

    private static void SetPaths()
    {
        FileDirectory = $"/Grappler/";
        FileName = $"GrapplerSaveData.txt";
    }
}



