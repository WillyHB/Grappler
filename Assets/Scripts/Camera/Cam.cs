using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float SmoothSpeed;

    public Transform Follow;
    public Vector3 FloatPosition { get; private set; }
    public Vector3 OldFloatPosition { get; private set; }
    public Vector3 PixelPerfectPosition { get; private set; }

    public LayerMask BlindZoneLayers;
    public LayerMask LenientLayer;
    public LayerMask NonLenietLayer;

    public Camera[] Cameras;

    public bool LockX;
    public bool LockY;

    public CameraEventChannel CamEventChannel;

    public void Start()
    {
        CamEventChannel.Repositioned += SetPosition;
        CamEventChannel.Locked += Lock;
        CamEventChannel.ResetFollow += SetFollow;
    }

    public void OnDisable()
    {
        CamEventChannel.Repositioned -= SetPosition;
        CamEventChannel.Locked -= Lock;
        CamEventChannel.ResetFollow -= SetFollow;
    }

    public void Lock(bool x, bool y)
    {
        LockX = x;
        LockY = y;
    }

    public void SetFollow(Transform follow)
    {
        Follow = follow;
    }

    public void SetPosition(Vector3 position)
    {
        foreach (Camera cam in Cameras)
        {
            cam.transform.position = position;
        }
    }

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
            Vector3 newPos = FloatPosition;

            float distance = (FloatPosition - Follow.position).magnitude;

            OldFloatPosition = FloatPosition;
            FloatPosition = newPos;
            // LERP VELOCITY??
            FloatPosition = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);
            //FutureFloatPosition = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);

            PixelPerfectPosition = new Vector3(
                FloatPosition.x - (FloatPosition.x % (1.0f / 16)),
                 FloatPosition.y - (FloatPosition.y % (1.0f / 16)),
                  FloatPosition.z - (FloatPosition.z % (1.0f / 16)));
        }
    }
}


