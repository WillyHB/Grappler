using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RenderCam : MonoBehaviour
{
    private Transform renderSurface;

    public Camera PlayerCam;

    private Camera renderCam;
    private Cam cam;

    private float xShakeOffset;

    private float coroutineTime;

    public void Shake(float frequency, float amplitude, float time)
    {
        coroutineTime = 0;
        xShakeOffset = 0;

        StartCoroutine(ShakeCoroutine(frequency, amplitude, time));
    }

    private IEnumerator ShakeCoroutine(float frequency, float amplitude, float time)
    {
        do
        {
            Debug.Log(coroutineTime);

            xShakeOffset = amplitude * Mathf.Sin(frequency * coroutineTime);

            yield return null;

        } while ((coroutineTime += Time.deltaTime) < time);
    }

    /*
     
    Aspect Ratio Settings changes the render textures size
    Render texture will try to fit, else it will letterbox
    We will try to automatically set the aspect ratio on start
    */
    // Start is called before the first frame update
    void Start()
    {
        CameraEffects.Shaked += Shake;

        renderCam = GetComponent<Camera>();

        cam = Camera.main.GetComponent<Cam>();
        renderSurface = transform.GetChild(0);
    }

    private void OnDisable()
    {
        CameraEffects.Shaked -= Shake;
    }

    public bool Smooth = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Smooth)
        {
            Vector2 dif = cam.FloatPosition - cam.transform.position;

            renderSurface.localPosition = new Vector3(-dif.x - xShakeOffset, -dif.y, 10);
        }

        PlayerCam.transform.position = new Vector3(cam.FloatPosition.x + xShakeOffset, cam.FloatPosition.y, cam.FloatPosition.y);


    }
}

