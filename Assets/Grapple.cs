using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public LayerMask GroundLayerMask;

    public GameObject TwoPointRope;

    private GameObject hook;
    private GameObject grappleInstance;


    void Start()
    {
        hook = new GameObject();
        hook.name = "Hook";

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

            hook.transform.position = point;

            grappleInstance = Instantiate(TwoPointRope);
            grappleInstance.GetComponent<TwoPointRope>().StartPoint = transform;
            grappleInstance.GetComponent<TwoPointRope>().EndPoint = hook.transform;


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

        if (Physics2D.Linecast(lineRenderer.GetPosition(0), lineRenderer.GetPosition(lineRenderer.positionCount-1), GroundLayerMask))
        {
            Debug.Log("HIT HIT HIT");
        }
    }
}
