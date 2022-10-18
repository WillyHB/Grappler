using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float SmoothSpeed;

    public Transform Follow;
    public Vector3 FloatPosition { get; private set; }
    public Vector3 PixelPerfectPosition { get; private set; }

    public Camera PlayerCam;
    public Camera FrontCam;
    public Camera BackCam;
    public Camera ReflectiveCam;

    void FixedUpdate()
    {
        if (Follow != null)
        {
            FloatPosition = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);

            PixelPerfectPosition = new Vector3(
                FloatPosition.x - (FloatPosition.x % (1.0f / 16)),
                 FloatPosition.y - (FloatPosition.y % (1.0f / 16)),
                  FloatPosition.z - (FloatPosition.z % (1.0f / 16)));
        }

        PlayerCam.transform.position = new Vector3(PixelPerfectPosition.x, PixelPerfectPosition.y, -5);
        FrontCam.transform.position = new Vector3(PixelPerfectPosition.x, PixelPerfectPosition.y, -5);
        BackCam.transform.position = new Vector3(PixelPerfectPosition.x, PixelPerfectPosition.y, -5);
        ReflectiveCam.transform.position = new Vector3(PixelPerfectPosition.x, PixelPerfectPosition.y, -5);
    }
}
