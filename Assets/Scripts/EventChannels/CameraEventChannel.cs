using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(menuName = "Event Channels/Camera")]
public class CameraEventChannel : ScriptableObject
{
    public event Action<float> Zoom;
    /// </summary>
    public event Action<float, float, float> Shake;

    public void PerformZoom(float zoom) => Zoom?.Invoke(zoom);
    public void PerformShake(float freq, float amp, float time) => Shake?.Invoke(freq, amp, time);
}
