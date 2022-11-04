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

    public event Action<bool, bool> Locked;

    public event Action<Vector3> Repositioned;
    public event Action<Transform> ResetFollow;

    public void LockPosition() => Locked?.Invoke(true, true);
    public void UnlockPosition() => Locked?.Invoke(false, false);
    public void LockPosition(bool x, bool y) => Locked?.Invoke(x, y);

    public void SetPosition(Vector2 position) => Repositioned?.Invoke(position);
    public void SetPosition(float x, float y) => Repositioned?.Invoke(new Vector2(x, y));
    public void SetPosition(float x, float y, float z) => Repositioned?.Invoke(new Vector3(x, y, z));
    public void SetPosition(Vector3 position) => Repositioned?.Invoke(position);

    public void SetFollow(Transform follow) => ResetFollow?.Invoke(follow);

    public void PerformZoom(float zoom) => Zoom?.Invoke(zoom);
    public void PerformShake(float freq, float amp, float time) => Shake?.Invoke(freq, amp, time);
}
