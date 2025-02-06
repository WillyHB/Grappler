using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Menu;
    public InputProvider inputProvider;
    public PlayerEventChannel PlayerEventChannel;
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

        if (Menu.activeSelf) PlayerEventChannel.Freeze(true);
        else PlayerEventChannel.UnFreeze(true);

    }
}
