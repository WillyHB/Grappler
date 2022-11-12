using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathManager : LevelTransition
{
    public VoidEventChannel DeathEventChannel;
    new void Start()
    {
        base.Start();
        DeathEventChannel.Raised += DeathOccured;
    }

    public void DeathOccured()
    {
        Load(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
