using System.Collections;
using System.Collections.Generic;
using Cutscene;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public Audio Click;
    public AudioEventChannel AudioEventChannel;
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioEventChannel.Play(Click);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
