using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCam : MonoBehaviour
{
    private Transform renderSurface;
    // Start is called before the first frame update
    void Start()
    {
        renderSurface = transform.GetChild(0);
    }

    public bool Smooth = true;

    // Update is called once per frame
    void Update()
    {
        if (Smooth)
        {
            Vector2 dif = FindObjectOfType<Cam>().FloatPosition - FindObjectOfType<Cam>().transform.position;

            renderSurface.localPosition = new Vector3(-dif.x, -dif.y, 10);
        }


        Vector3 mainCamPos = Camera.main.transform.position;
    }
}
