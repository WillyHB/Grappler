using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float SmoothSpeed;

    public Transform Follow;
    public Vector3 FloatPosition { get; private set; }
    public Vector3 PixelPerfectPosition { get; private set; }

    public LayerMask BlindZoneLayer;

    public Camera[] Cameras;

    public bool LockX;
    public bool LockY;

    public CameraEventChannel CamEventChannel;

    public void Start()
    {
        CamEventChannel.Repositioned += SetPosition;
        CamEventChannel.Locked += Lock;
    }

    public void OnDisable()
    {
        CamEventChannel.Repositioned -= SetPosition;
        CamEventChannel.Locked -= Lock;
    }

    public void Lock(bool x, bool y)
    {
        LockX = x;
        LockY = y;
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
            Vector3 tempFloatPos = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);

            float sizeX = Camera.main.orthographicSize * ResolutionManager.AspectRatio*2;
            float sizeY = Camera.main.orthographicSize*2;

            if (Physics2D.BoxCast(
                FloatPosition, 
                new Vector2(sizeX, sizeY), 0, 
                new Vector2(tempFloatPos.x - FloatPosition.x, 0),
                Mathf.Abs(tempFloatPos.x - FloatPosition.x), 
                BlindZoneLayer))
            {
                tempFloatPos.x = FloatPosition.x;
            }

            if (Physics2D.BoxCast(
                FloatPosition,
                new Vector2(sizeX, sizeY), 0,
                new Vector2(0, tempFloatPos.y - FloatPosition.y),
                Mathf.Abs(tempFloatPos.y - FloatPosition.y),
                BlindZoneLayer))
            {
                tempFloatPos.y = FloatPosition.y;
            }



            FloatPosition = tempFloatPos;

            PixelPerfectPosition = new Vector3(
                FloatPosition.x - (FloatPosition.x % (1.0f / 16)),
                 FloatPosition.y - (FloatPosition.y % (1.0f / 16)),
                  FloatPosition.z - (FloatPosition.z % (1.0f / 16)));
        }
    }
}


