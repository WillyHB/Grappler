using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    private Button play, settings, credits, exitToDesktop;
    public
    void Start()
    {
        var root = FindObjectOfType<UIDocument>().rootVisualElement;

        play = root.Q<Button>("play");
        settings = root.Q<Button>("settings");
        credits = root.Q<Button>("credits");
        exitToDesktop = root.Q<Button>("exit-to-desktop");
    
        play.clicked += () =>
        {
            Debug.Log("Time To Play!");
        };

    }

    private void OnDestroy()
    {
        play.clicked -= () =>
        {
            Debug.Log("Time To Play!");
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
