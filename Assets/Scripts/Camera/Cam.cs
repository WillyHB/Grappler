using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float SmoothSpeed;

    public Transform Follow;
    public Vector3 FloatPosition { get; private set; }
    public Vector3 PixelPerfectPosition { get; private set; }

    public Camera[] Cameras;

    public bool LockX;
    public bool LockY;

    
    public void SetPosition(Vector3 position) => FloatPosition = position;
    public void SetPosition(Vector2 position) => FloatPosition = position;

    void FixedUpdate()
    {
        foreach (Camera cam in Cameras)
        {
            cam.transform.position = new Vector3(
                LockX ? cam.transform.position.x : PixelPerfectPosition.x,
                LockY ? cam.transform.position.y : PixelPerfectPosition.y, -5);
        }

        if (Follow != null)
        {
            FloatPosition = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);

            PixelPerfectPosition = new Vector3(
                FloatPosition.x - (FloatPosition.x % (1.0f / 16)),
                 FloatPosition.y - (FloatPosition.y % (1.0f / 16)),
                  FloatPosition.z - (FloatPosition.z % (1.0f / 16)));
        }
    }
}


