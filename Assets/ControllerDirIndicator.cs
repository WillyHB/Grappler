using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class ControllerDirIndicator : MonoBehaviour
{
    public LayerMask GroundLayerMask;
    private LineRenderer lineRenderer;

    public bool ShowIndicator = false;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (ShowIndicator)
        {
            if (InputDeviceManager.CurrentDeviceType == InputDevices.Controller && Gamepad.current.rightStick.ReadValue() != Vector2.zero)
            {
                lineRenderer.enabled = true;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, Gamepad.current.rightStick.ReadValue(), 1000, GroundLayerMask);

                Debug.Log(transform.position - (Vector3)hit.point);

                lineRenderer.SetPositions(new Vector3[]
                {
                  transform.position, hit ? hit.point : Gamepad.current.rightStick.ReadValue() * 100
                });
            }

            else
            {
                lineRenderer.enabled = false;
            }
        }

    }
}
