using System.Collections;
using System;
using UnityEngine;

public static class CameraEffects
{
    /// <summary>
    /// frequency, amp, time
    /// </summary>
    public static event Action<float, float, float> Shaked;

    public static event Action<float> Zoom;
    public static void PerformShake(float freq, float amp, float time)
    {
        Shaked?.Invoke(freq, amp, time);
    }

    private static float _zoom = -1;

    public static void SetZoomValue(float zoom)
    {
        if (zoom != _zoom)
        {
            Zoom?.Invoke(zoom < 0 ? 0 : zoom);
        }

        _zoom = zoom;

    }

}
