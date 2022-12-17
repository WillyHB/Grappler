using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class RenderCam : MonoBehaviour
{
    public GameObject RenderSurfaceParent;

    private float xShakeOffset;

    private float coroutineTime;

    public CameraEventChannel CamEventChannel;

    public async void OnShake(float frequency, float amplitude, float time)
    {
        coroutineTime = 0;
        xShakeOffset = 0;

        await Shake(frequency, amplitude, time);

        coroutineTime = 0;
        xShakeOffset = 0;
    }

    private async Task Shake(float frequency, float amplitude, float time)
    {
        do
        {
            xShakeOffset = amplitude * Mathf.Sin(frequency * coroutineTime);

            await Task.Yield();

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
        RenderSurfaceParent.transform.localPosition 
            = new Vector3(xShakeOffset, RenderSurfaceParent.transform.localPosition.y, RenderSurfaceParent.transform.localPosition.z);
    }
}

