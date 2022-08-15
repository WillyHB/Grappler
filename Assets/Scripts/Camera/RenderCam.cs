using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCam : MonoBehaviour
{
    private Transform renderSurface;

    public Camera PlayerCam;

    private Cam cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<Cam>();
        renderSurface = transform.GetChild(0);
    }

    public bool Smooth = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Smooth)
        {
            Vector2 dif = cam.FloatPosition - cam.transform.position;

            renderSurface.localPosition = new Vector3(-dif.x, -dif.y, 10);
        }

        PlayerCam.transform.position = cam.FloatPosition;


    }
}
