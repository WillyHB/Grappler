using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float SmoothSpeed;

    public Transform Follow;
    public Vector3 FloatPosition { get; private set; }
    public Vector3 FutureFloatPosition { get; private set; }
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
            Vector3 tempFloatPos = FutureFloatPosition;

            float sizeX = (Camera.main.orthographicSize - ResolutionManager.CameraZoom) * ResolutionManager.AspectRatio * 2;
            float sizeY = (Camera.main.orthographicSize - ResolutionManager.CameraZoom) * 2;

            Vector2 diff = new(tempFloatPos.x - FloatPosition.x, tempFloatPos.y - FloatPosition.y);


            RaycastHit2D hit
                = Physics2D.BoxCast(FloatPosition, new Vector2(sizeX, sizeY), 0, Vector2.zero, 0, BlindZoneLayers);

            if (hit)
            {
                Bounds camBounds = new(FloatPosition, new Vector3(sizeX, sizeY));

                Collider2D col = hit.collider;
                Vector2 edgePoint = camBounds.ClosestPoint(hit.point);

                if (edgePoint.x < hit.point.x)
                {
                    tempFloatPos.x = hit.point.x - sizeX / 2 + ResolutionManager.CameraZoom;
                }

                else
                {
                    tempFloatPos.x = hit.point.x + sizeX / 2 + ResolutionManager.CameraZoom;
                }
            }

            else
            {
                RaycastHit2D newHitX
                     = Physics2D.BoxCast(FloatPosition + new Vector3(diff.x, 0, 0), new Vector2(sizeX, sizeY), 0, Vector2.zero, 0, BlindZoneLayers);
                RaycastHit2D newHitY
                    = Physics2D.BoxCast(FloatPosition + new Vector3(0, diff.y, 0), new Vector2(sizeX, sizeY), 0, Vector2.zero, 0, BlindZoneLayers);

                Collider2D colX = newHitX.collider;
                Collider2D colY = newHitY.collider;

                if (colX != null)
                {
                    if ((Follow.position.x > colX.transform.position.x) != (tempFloatPos.x > colX.transform.position.x) 
                        && (LenientLayer & 1 << colX.gameObject.layer) == 1 << colX.gameObject.layer)
                    {
                        if (Follow.position.x > colX.transform.position.x)
                        {
                            tempFloatPos.x = colX.transform.position.x + sizeX / 2 + ResolutionManager.CameraZoom;
                        }

                        else if (Follow.position.x < colX.transform.position.x)
                        {
                            tempFloatPos.x = colX.transform.position.x - sizeX / 2 - ResolutionManager.CameraZoom;
                        }
                    }

                    else
                    {
                        tempFloatPos.x = FloatPosition.x;
                    }
                }

                if (colY != null)
                {
                    if ((Follow.position.y > colY.transform.position.y) != (tempFloatPos.y > colY.transform.position.y)
                         && (LenientLayer & 1 << colY.gameObject.layer) == 1 << colY.gameObject.layer)
                    {
                        if (Follow.position.y > colY.transform.position.y)
                        {
                            tempFloatPos.y = colY.transform.position.y + sizeY / 2 + ResolutionManager.CameraZoom;
                        }

                        else if (Follow.position.y < colY.transform.position.y)
                        {
                            tempFloatPos.y = colY.transform.position.y - sizeY / 2 - ResolutionManager.CameraZoom;
                        }
                    }

                    else
                    {
                        tempFloatPos.y = FloatPosition.y;
                    }
                }
            }

            FloatPosition = tempFloatPos;
            FutureFloatPosition = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);

            PixelPerfectPosition = new Vector3(
                FloatPosition.x - (FloatPosition.x % (1.0f / 16)),
                 FloatPosition.y - (FloatPosition.y % (1.0f / 16)),
                  FloatPosition.z - (FloatPosition.z % (1.0f / 16)));
        }
    }
}


