using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCam : MonoBehaviour
{
    private Transform renderSurface;

    public Camera PlayerCam;

    private Camera renderCam;
    private Cam cam;

    /*
     
    Aspect Ratio Settings changes the render textures size
    Render texture will try to fit, else it will letterbox
    We will try to automatically set the aspect ratio on start
    */
    // Start is called before the first frame update
    void Start()
    {
        renderCam = GetComponent<Camera>();

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
