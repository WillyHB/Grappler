using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    public Scene MainScene;

    public GameObject Main_Menu;
    public GameObject Settings_Menu;

    void Start()
    {
        Main_Menu.SetActive(true);
        Settings_Menu.SetActive(false);
    }

    public void NewGame() 
    {
        SaveObject so = GameData.Load();
        so.checkpoint = 0;
        so.skipStartCutscene = false;
        GameData.Save(so);
        ContinueGame();
    }

    public void Settings() 
    {
        Settings_Menu.SetActive(true);
        Main_Menu.SetActive(false);
    }

    public void Settings_Back() 
    {
        Settings_Menu.SetActive(false);
        Main_Menu.SetActive(true);

    }
    
    public void Quit() 
    {
        Application.Quit();
    }

    public void ContinueGame() 
    {
        LevelTransition.Load(1);
    }

}
