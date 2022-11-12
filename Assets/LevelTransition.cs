using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class LevelTransition : MonoBehaviour
{
    private AsyncOperation _asyncOperation;

    public GameObject Transition;

    protected void Start()
    {
        Transition.transform.localPosition = new Vector2(0, 0);
    }

    private IEnumerator LoadSceneAsyncProcess(int level)
    {
        // Begin to load the Scene you have specified.
        _asyncOperation = SceneManager.LoadSceneAsync(level);

        // Don't let the Scene activate until you allow it to.
        _asyncOperation.allowSceneActivation = false;

        yield return new WaitForSeconds(2f);

        LoadLevel();
    }

    public void Load(int level)
    {
        if (_asyncOperation == null)
        {
            StartCoroutine(LoadSceneAsyncProcess(level));
        }
    }

    private void LoadLevel()
    {
        _asyncOperation.allowSceneActivation = true;
    }
}
