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

    public bool IgnoreBlockages;

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
        Time.timeScale = 0;

        position = new Vector3(position.x, position.y, -5);


        RaycastHit2D hit
            = Physics2D.BoxCast(FloatPosition, new Vector2(ResolutionManager.CamViewDimensions.x, ResolutionManager.CamViewDimensions.y), 0, Vector2.zero, 0, BlindZoneLayers);

        if (hit)
        {
            position = ManageCameraBlockHit(position, hit);
        }

        FloatPosition = position;

        foreach (Camera cam in Cameras)
        {
            LeanTween.move(cam.gameObject, position, 1f).setEaseOutQuint().setOnComplete(() => { Time.timeScale = 1; }).setIgnoreTimeScale(true);
        }
    }

    private Vector3 ManageCameraBlockHit(Vector3 futurePos, RaycastHit2D hit)
    {
        if (hit.normal.y == 0)
        {
            Debug.Log("Poop");
            if (Follow.position.x > hit.transform.position.x)
            {
                futurePos.x = hit.transform.position.x + ResolutionManager.CamViewDimensions.x / 2 + 0.01f;
            }
            else
            {
                futurePos.x = hit.transform.position.x - ResolutionManager.CamViewDimensions.x / 2 - 0.01f;
            }
        }

        else
        {
            Debug.Log("Fart");
            if (Follow.position.y > hit.transform.position.y)
            {
                futurePos.y = hit.transform.position.y + ResolutionManager.CamViewDimensions.y / 2 + 0.01f;
            }
            else
            {
                futurePos.y = hit.transform.position.y - ResolutionManager.CamViewDimensions.y / 2 - 0.01f;
            }
        }

        return futurePos;
    }

    void FixedUpdate()
    {
        foreach (Camera cam in Cameras)
        {
            cam.transform.position = new Vector3(PixelPerfectPosition.x, PixelPerfectPosition.y, -5);
        }

        if (Follow != null)
        {
            Vector3 futurePos = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);

            Vector2 diff = futurePos - FloatPosition;

            RaycastHit2D hit
            = Physics2D.BoxCast(FloatPosition, new Vector2(ResolutionManager.CamViewDimensions.x, ResolutionManager.CamViewDimensions.y), 0, Vector2.zero, 0, BlindZoneLayers);

            if (!IgnoreBlockages)
            {
                if (hit)
                {
                    futurePos = ManageCameraBlockHit(futurePos, hit);
                }

                else
                {
                    RaycastHit2D newHitX
                         = Physics2D.BoxCast(FloatPosition + new Vector3(diff.x, 0, 0), new Vector2(ResolutionManager.CamViewDimensions.x, ResolutionManager.CamViewDimensions.y), 0, Vector2.zero, 0, BlindZoneLayers);
                    RaycastHit2D newHitY
                        = Physics2D.BoxCast(FloatPosition + new Vector3(0, diff.y, 0), new Vector2(ResolutionManager.CamViewDimensions.x, ResolutionManager.CamViewDimensions.y), 0, Vector2.zero, 0, BlindZoneLayers);


                    Collider2D colX = newHitX.collider;
                    Collider2D colY = newHitY.collider;
                    if (colX != null)
                    {
                        float difference = Mathf.Abs(FloatPosition.x - colX.transform.position.x);

                        if ((Follow.position.x > colX.transform.position.x) != (futurePos.x > colX.transform.position.x)
                            && (LenientLayer & 1 << colX.gameObject.layer) == 1 << colX.gameObject.layer)
                        {
                            if (Follow.position.x > colX.transform.position.x)
                            {
                                futurePos.x = colX.transform.position.x + difference;
                            }
                            else
                            {
                                futurePos.x = colX.transform.position.x - difference;
                            }

                            SetPosition(futurePos);
                        }
                        else
                        {
                            futurePos.x = FloatPosition.x;
                        }
                    }

                    if (colY != null)
                    {
                        float difference = Mathf.Abs(FloatPosition.y - colY.transform.position.y);

                        if ((Follow.position.y > colY.transform.position.y) != (futurePos.y > colY.transform.position.y)
                           && (LenientLayer & 1 << colY.gameObject.layer) == 1 << colY.gameObject.layer)
                        {
                            if (Follow.position.y > colY.transform.position.y)
                            {
                                futurePos.y = colY.transform.position.y + difference;
                            }
                            else
                            {
                                futurePos.y = colY.transform.position.y - difference;
                            }

                            SetPosition(futurePos);
                        }

                        else
                        {
                            futurePos.y = FloatPosition.y;
                        }
                    }
                }
            }
            

            OldFloatPosition = FloatPosition;
            // LERP VELOCITY??
            FloatPosition = new Vector3(LockX ? FloatPosition.x : futurePos.x, LockY ? FloatPosition.y : futurePos.y);
            //FutureFloatPosition = Vector3.Lerp(FloatPosition, new Vector3(Follow.position.x, Follow.position.y, -10), SmoothSpeed);

            PixelPerfectPosition = new Vector3(
                FloatPosition.x - (FloatPosition.x % (1.0f / 16)),
                 FloatPosition.y - (FloatPosition.y % (1.0f / 16)),
                  FloatPosition.z - (FloatPosition.z % (1.0f / 16)));
        }
    }
}


