using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public Scene MainScene;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void NewGame() {

    }

    public void ContinueGame() {
        
        LevelTransition.Load(1);
    }

}
