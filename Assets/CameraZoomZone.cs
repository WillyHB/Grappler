using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomZone : MonoBehaviour
{

    public float Zoom;

    private float oldZoom;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oldZoom = ResolutionManager.CameraZoom;

            //CameraEffects.SetZoomValue(Zoom);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //CameraEffects.SetZoomValue(oldZoom);
        }
    }
}
