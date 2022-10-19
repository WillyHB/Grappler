using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveZone : MonoBehaviour
{
    public Vector2 Position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

        }
    }
}
