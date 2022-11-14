using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class LevelTransition
{
    private static AsyncOperation _asyncOperation;

    private static async void LoadSceneAsyncProcess(int level)
    {
        // Begin to load the Scene you have specified.
        _asyncOperation = SceneManager.LoadSceneAsync(level);
        // Don't let the Scene activate until you allow it to.
        _asyncOperation.allowSceneActivation = false;
        await Task.Yield();

        LoadLevel();
    }

    public static void Load(int level)
    {
        if (_asyncOperation == null)
        {
            LoadSceneAsyncProcess(level);
        }
    }

    public static void Reload()
    {
        if (_asyncOperation == null)
        {
            LoadSceneAsyncProcess(SceneManager.GetActiveScene().buildIndex);
        }
    }


    private static void LoadLevel()
    {

        _asyncOperation.allowSceneActivation = true;
        _asyncOperation = null;
    }
}
