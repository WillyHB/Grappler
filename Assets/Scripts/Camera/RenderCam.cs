using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RenderCam : MonoBehaviour
{
    public GameObject[] RenderSurfaces;


    private float xShakeOffset;

    private float coroutineTime;

    public CameraEventChannel CamEventChannel;

    public void OnShake(float frequency, float amplitude, float time)
    {
        coroutineTime = 0;
        xShakeOffset = 0;

        StartCoroutine(ShakeCoroutine(frequency, amplitude, time));
    }

    private IEnumerator ShakeCoroutine(float frequency, float amplitude, float time)
    {
        do
        {
            xShakeOffset = amplitude * Mathf.Sin(frequency * coroutineTime);

            yield return null;

        } while ((coroutineTime += Time.deltaTime) < time);
    }

    void Start()
    {
        CamEventChannel.Shake += OnShake;
    }

    private void OnDisable()
    {
        CamEventChannel.Shake -= OnShake;
    }

    public bool Smooth = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        if (Smooth)
        {
            Vector2 dif = Cam.FloatPosition - Cam.PixelPerfectPosition;

            foreach (GameObject go in RenderSurfaces)
            {
                go.transform.localPosition = new Vector3((Cam.LockX ? 0 : -dif.x) - xShakeOffset, Cam.LockY ? 0 : -dif.y, go.transform.localPosition.z);
            }
        }*/
    }
}

