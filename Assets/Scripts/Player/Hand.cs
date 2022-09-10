using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hand : MonoBehaviour
{
    public Transform shoulder;
    public Vector2? followPosition;
    public float armLength = 1f;

    public Vector2 GrappleDirection { get; private set; }

    public Vector2 Direction { get; set; }

    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue() / ResolutionManager.ScaleValue);

        Direction = transform.parent.position - transform.position;

        var angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation *= new Quaternion(0, 0, -1, 0);

        Vector3 shoulderToDir = Vector3.zero;

        if (followPosition != null)
        {
            shoulderToDir = (Vector3)followPosition - shoulder.position;
        }

        else if (InputDeviceManager.CurrentDeviceType == InputDevices.MnK)
        {
            shoulderToDir = mousePos - shoulder.position;
        }

        else if (InputDeviceManager.CurrentDeviceType == InputDevices.Controller)
        {
            shoulderToDir = (Vector3)Gamepad.current.rightStick.ReadValue();
        }

        shoulderToDir.z = 0;

        GrappleDirection = shoulderToDir.normalized;

        transform.position = shoulder.position + (armLength * shoulderToDir.normalized);

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
