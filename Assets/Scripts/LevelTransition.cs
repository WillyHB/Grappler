using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class LevelTransition
{
    private static AsyncOperation _asyncOperation;

    private static async void LoadSceneAsyncProcess(int level, bool loadOneGo = false)
    {
        // Begin to load the Scene you have specified.
        _asyncOperation = SceneManager.LoadSceneAsync(level);
        // Don't let the Scene activate until you allow it to.
        _asyncOperation.allowSceneActivation = false;

        await ScreenTransition.Instance?.OutOfLevel();
        await Task.Yield();

        if (loadOneGo) ActivateLoad();
    }

    public static void StageLoad(int level) 
    {
        if (_asyncOperation == null)
        {
            LoadSceneAsyncProcess(level);
        }
    }
    
    public static void LoadSync(int level) {
        if (_asyncOperation == null) 
        {
            LoadSceneAsyncProcess(level, true);
        }
    }
    public static void ActivateLoad() 
    {
        _asyncOperation.allowSceneActivation = true;
        _asyncOperation = null;
    }

    public static void Reload()
    {
        LoadSync(SceneManager.GetActiveScene().buildIndex);
    }
}
