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

            var sm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();
            sm.Transition(sm.GrappleState);

            Hook.transform.position = point;

            grappleInstance = Instantiate(TwoPointRope, transform.parent.parent);

            grappleInstance.GetComponent<TwoPointRope>().LineLength = (transform.position - Hook.transform.position).magnitude;
            grappleInstance.GetComponent<TwoPointRope>().StartPoint = transform;
            grappleInstance.GetComponent<TwoPointRope>().EndPoint = Hook.transform;

            PlayerDistanceJoint.distance = (transform.position - Hook.transform.position).magnitude;
            PlayerDistanceJoint.enabled = true;
        }

    }

    public void ReleaseGrapple()
    {
        Destroy(grappleInstance);
        PlayerDistanceJoint.enabled = false;
    }
}
