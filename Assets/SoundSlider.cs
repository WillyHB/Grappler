using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundSlider : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    public Audio Click;
    public MixerGroup MixerGroup;
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioMaster.Instance.Play(Click, MixerGroup);
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
