using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveZone : MonoBehaviour
{
    public CameraEventChannel CamEventChannel;
    public Vector2 Position;
    public bool SetToCurrentTransformPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            CamEventChannel.LockPosition();
            CamEventChannel.SetPosition(SetToCurrentTransformPosition ? (Vector2)transform.position : Position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamEventChannel.SetPosition(FindObjectOfType<Cam>().Follow.position);
            CamEventChannel.UnlockPosition();
        }
    }
}
