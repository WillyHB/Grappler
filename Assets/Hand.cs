using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    // Point you want to have sword rotate around
    public Transform shoulder;
    // how far you want the sword to be from point
    public float armLength = 1f;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        var dir = transform.parent.position - transform.position;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation *= new Quaternion(0, 0, -1, 0);

        Vector3 shoulderToMouseDir =
                mousePos - shoulder.position;
        shoulderToMouseDir.z = 0;

        transform.position = shoulder.position + (armLength * shoulderToMouseDir.normalized);


        if (transform.rotation.z > 0.7 || transform.rotation.z < -0.7)
        {
            GetComponent<SpriteRenderer>().flipY = true;
        }

        else
        {
            GetComponent<SpriteRenderer>().flipY = false;
        }

    }
}
