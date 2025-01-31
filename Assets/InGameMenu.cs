using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Menu;
    public InputProvider inputProvider;
    void Start()
    {
        
    }

    void OnEnable() 
    {
        inputProvider.MenuToggled += ToggleMenu;
    }

    void OnDisable() 
    {
        inputProvider.MenuToggled -= ToggleMenu;
    }

    void ToggleMenu() 
    {
        Menu.SetActive(!Menu.activeSelf);

        if (Menu.activeSelf) GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().Freeze(true);
        else GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().UnFreeze(true);

    }
}
