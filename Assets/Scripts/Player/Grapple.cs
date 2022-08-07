using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    public DistanceJoint2D PlayerDistanceJoint;
    private LineRenderer lineRenderer;

    public LayerMask GroundLayerMask;

    public GameObject TwoPointRope;
    public GameObject Hook;
    private GameObject grappleInstance;


    void Start()
    {
        PlayerDistanceJoint.enabled = false;

        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(1, new Vector3(0, 0, 0));

        GetComponentInParent<PlayerInput>().actions["Grapple"].started += MousePressed;

    }

    private void MousePressed(InputAction.CallbackContext cc)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Linecast(transform.position, mousePos, GroundLayerMask);

        if (hit)
        {
            if (grappleInstance != null)
            {
                Destroy(grappleInstance);
            }

            Vector2 point = hit.point;

            FindObjectOfType<PlayerMovement>().IsGrappling = true;
            Hook.transform.position = point;

            grappleInstance = Instantiate(TwoPointRope, transform.parent.parent);

            grappleInstance.GetComponent<TwoPointRope>().LineLength = (transform.position - Hook.transform.position).magnitude;
            grappleInstance.GetComponent<TwoPointRope>().StartPoint = transform;
            grappleInstance.GetComponent<TwoPointRope>().EndPoint = Hook.transform;

            PlayerDistanceJoint.distance = (transform.position - Hook.transform.position).magnitude;
            PlayerDistanceJoint.enabled = true;
        }

    }

    // Update is called once per frame
    private void Update()
    {
        //lineRenderer.SetPosition(0, transform.position);

        /*
        Vector2[] list = new Vector2[lineRenderer.positionCount];

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            list[i] = transform.InverseTransformPoint(lineRenderer.GetPosition(i));
        }

        GetComponent<EdgeCollider2D>().points = list;
        */

    }
}
