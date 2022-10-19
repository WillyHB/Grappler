using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLockZone : MonoBehaviour
{
    public bool LockX;
    public float XLockValue;
    [Space(10)]
    public bool LockY;
    public float YLockValue;
    public CameraEventChannel CamEventChannel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamEventChannel.LockPosition(LockX, LockY);

            CamEventChannel.SetPosition(XLockValue, YLockValue);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamEventChannel.UnlockPosition();
        }
    }
}
