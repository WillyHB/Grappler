using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowZone : MonoBehaviour
{
    public CameraEventChannel CamEventChannel;

    private Transform oldTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            oldTransform = FindObjectOfType<Cam>().Follow;
            CamEventChannel.SetFollow(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CamEventChannel.SetFollow(oldTransform);
        }
    }
}
